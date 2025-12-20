using CloneIntime.Entities;
using CloneIntime.Migrations;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Models.Entities;
using CloneIntime.Models.ModelTypes;
using CloneIntime.Services.Interfaces;
using CloneIntime.Utils.Constants;
using CloneIntime.Utils.helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace CloneIntime.Services
{
    public class AdminService : IAdminService
    {
        private readonly Context _context;
        private readonly SupportService _support;
        private readonly AdminHelper _adminHelper;
        public AdminService(Context context, SupportService support, AdminHelper adminHelper)
        {
            _context = context;
            _support = support;
            _adminHelper = adminHelper;

        }

        private List<DisciplineEntity> fillDiscipline(List<DisciplineEntity> disciplines)
        {
            var disciplineIds = disciplines.Select(disc => disc.Id).ToList();

            var result = _context.DisciplineEntities
                    .Where(x => disciplineIds.Contains(x.Id))
                .ToList();

            return result;
        }

        private async void CreateNewDay(PairEntity newPair, SetTimeSlotModel newPairData)
        {
            var newTimeslot = new TimeSlotEntity
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Pair = new List<PairEntity> { newPair },
                PairNumber = newPairData.PairNumber
            };
            var newDay = new DayEntity
            {
                Date = newPairData.Date,
                DayName = _adminHelper.GetWeekDayNameFromDate(newPairData.Date.Date),
                Id = Guid.NewGuid(),
                IsActive = true,
                Lessons = new List<TimeSlotEntity> {
                        newTimeslot
                    }
            };
            await _context.TimeSlotEntities.AddAsync(newTimeslot);
            await _context.DayEntities.AddAsync(newDay);
        }

        private async void CreateNewTimeslot(DayEntity day, PairEntity newPair, SetTimeSlotModel newPairData)
        {
            var newTimeslot = new TimeSlotEntity
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Pair = new List<PairEntity> { newPair },
                PairNumber = newPairData.PairNumber
            };
            day.Lessons.Add(newTimeslot);
            await _context.TimeSlotEntities.AddAsync(newTimeslot);
        }

        private bool GetActiveTeacherById(TeacherEntity teacher, string teacherId)
        {
            return teacher.Id.ToString() == teacherId && teacher.IsActive;
        }

        public async Task<IActionResult> AddTeacher(ProffessorDTO newTeacher)
        {
            var id = Guid.NewGuid();
            await _context.AddAsync(new TeacherEntity
            {
                Name = newTeacher.Name,
                Email = newTeacher.Email,
                Id = id,
                IsActive = true,
                Disciplines = fillDiscipline(newTeacher.Disciplines)
        });

            _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> UpdateTeacher(string teacherId, ProffessorDTO newTeacher)
        {
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => GetActiveTeacherById(x, teacherId));
            var disciplines = new List<DisciplineEntity>();
            var disciplinesEntity = await _context.DisciplineEntities
                .Where(x => newTeacher.Disciplines.Any(name => name.Id.ToString() == x.Id.ToString()))
                .ToListAsync();
            disciplines.AddRange(disciplinesEntity);

            if (teacher == null)
                return new NotFoundResult();

            teacher.Disciplines = disciplines;
            teacher.Name = newTeacher.Name;
            teacher.Email = newTeacher.Email;


            _context.Update(teacher);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> DeleteTeacher(string teacherId)
        {
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => GetActiveTeacherById(x, teacherId));

            if (teacher == null)
                return new NotFoundResult();

            teacher.IsActive = false;

            return new OkResult();
        }

        private List<GroupEntity> fillGroups(List<string> groups)
        {
            var groupEntity = _context.GroupEntities.Where(x => groups.Any(g => g == x.Number) && x.IsActive).ToList();
            return groupEntity;
        }

        public async Task<IActionResult> SetPair(SetTimeSlotModel newPairData)
        {
            var auditory = await _context.AuditoryEntities.FirstOrDefaultAsync(x => x.Number == newPairData.Auditory);
            var discipline = await _context.DisciplineEntities.FirstOrDefaultAsync(x => x.Id.ToString() == newPairData.Discipline);
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => GetActiveTeacherById(x, newPairData.Professor));
            var day = await _context.DayEntities
                .Include(x=> x.Lessons)
                .ThenInclude(j => j.Pair)
                .FirstOrDefaultAsync(x => x.Date.Date == newPairData.Date.Date);
            var newPair = new PairEntity
            {
                Auditory = auditory,
                Discipline = discipline,
                Group = fillGroups(newPairData.Groups),
                IsActive = true,
                Id = Guid.NewGuid(),
                LessonType = newPairData.Type,
                Teacher = teacher
            };
            _context.PairEntities.AddAsync(newPair);


            if (day == null)
            {
                CreateNewDay(newPair, newPairData);
            }
            else
            {
                var existingTimeSlot = day.Lessons.FirstOrDefault(x => x.PairNumber == newPairData.PairNumber);

                if (existingTimeSlot == null) {
                    CreateNewTimeslot(day, newPair, newPairData);
                }
                else
                {
                    foreach (var d in day.Lessons)
                    {
                        if (d.PairNumber == newPairData.PairNumber)
                        {
                            d.Pair.Add(newPair);
                            _context.TimeSlotEntities.Update(d);
                            break;
                        }
                    }

                }
                _context.DayEntities.Update(day);
            }


            await _context.SaveChangesAsync();
            var message = "All right";
            return new OkObjectResult(message);
        }
        public async Task<IActionResult> UpdatePair(string id, SetTimeSlotModel PairNewData)
        {
            var pair = await _context.PairEntities.FirstOrDefaultAsync(x => x.Id.ToString() == id && x.IsActive);
            var auditory = await _context.AuditoryEntities.FirstOrDefaultAsync(x => x.Number == PairNewData.Auditory);
            var discipline = await _context.DisciplineEntities.FirstOrDefaultAsync(x => x.Name == PairNewData.Discipline);
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => x.Name == PairNewData.Professor);
            // TODO: Добавить обработку ошибок
            if (pair == null)
                return new NotFoundObjectResult(new { message = new ErrorMessage().GetNotFoundMessage("Pair") }); 
            if (auditory == null)
                return new NotFoundObjectResult(new { message = new ErrorMessage().GetNotFoundMessage("Auditory") }); 
            if (discipline == null)
                return new NotFoundObjectResult(new { message = new ErrorMessage().GetNotFoundMessage("Discipline") });
            if (teacher == null)
                return new NotFoundObjectResult(new { message = new ErrorMessage().GetNotFoundMessage("Profesor") });

            pair.Auditory = auditory;
            pair.Discipline = discipline;
            pair.Teacher = teacher;
            pair.Group = fillGroups(PairNewData.Groups);

            _context.PairEntities.Update(pair);
            await _context.SaveChangesAsync();

            return new OkObjectResult(pair);

        }
        public async Task<IActionResult> DeletePair(string pairId)
        {
            var pair = await _context.PairEntities.FirstOrDefaultAsync(x => x.Id.ToString() == pairId && x.IsActive);


            if (pair == null)
                return new NotFoundObjectResult(new { message = new ErrorMessage().GetNotFoundMessage("Pair") });

            _context.PairEntities.Remove(pair);
            _context.SaveChangesAsync();

            return new OkResult();

        }

        public async Task<ActionResult<TokenModel>> Login(CredentialsModel model)
        {
            var user = await _context.EditorEntity.FirstOrDefaultAsync(x => x.Login == model.Email);

            if (user == null)
                return new NotFoundResult();

            if (user.Password != model.Password)
                return new BadRequestObjectResult(new { 
                    message = ErrorMessage.IncorrectPasswordMessage,
                });

            var id = user.Id;
            var token = new JwtSecurityTokenHandler().WriteToken(_support.GenerateJWT(model.Email, user.Id.ToString()));


            return new TokenModel
            {
                Token = token
            };
        }
        public async Task<IActionResult> Logout(string userToken)
        {
            var id = Guid.NewGuid();
            var token = new TokenEntity
            {
                Id = id,
                Token = userToken

            };
            await _context.TokenEntity.AddAsync(token);
            await _context.SaveChangesAsync();

            return new JsonResult(new { message = ErrorMessage.LoggedOutMessage });
        }
    }
}
