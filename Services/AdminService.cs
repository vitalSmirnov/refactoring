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

        private bool GetActiveTeacherById(TeacherEntity teacher, string teacherId)
        {
            return teacher.Id.ToString() == teacherId && teacher.IsActive;
        }

        private async Task<DayEntity> GetOrCreateDayAsync(DateTime date)
        {
            var day = await _context.DayEntities
                .Include(x => x.Lessons)
                .ThenInclude(j => j.Pair)
                .FirstOrDefaultAsync(x => x.Date.Date == date.Date);

            if (day != null)
            {
                return day;
            }

            var newDay = new DayEntity
            {
                Id = Guid.NewGuid(),
                Date = date,
                DayName = _adminHelper.GetWeekDayNameFromDate(date.Date),
                IsActive = true,
                Lessons = new List<TimeSlotEntity>()
            };

            await _context.DayEntities.AddAsync(newDay);
            return newDay;
        }

        private async Task<TimeSlotEntity> GetOrCreateTimeSlotAsync(DayEntity day, UInt16 pairNumber)
        {
            day.Lessons ??= new List<TimeSlotEntity>();

            var slot = day.Lessons.FirstOrDefault(x => x.PairNumber == pairNumber);
            if (slot != null)
            {
                return slot;
            }

            slot = new TimeSlotEntity
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                PairNumber = pairNumber,
                Pair = new List<PairEntity>()
            };

            day.Lessons.Add(slot);
            await _context.TimeSlotEntities.AddAsync(slot);

            return slot;
        }

        private IActionResult? ValidatePairUpdateDependencies(PairEntity pair, AuditoryEntity auditory, DisciplineEntity discipline, TeacherEntity teacher)
        {
            var checks = new (object entity, string name)[]
            {
                (pair, "Pair"),
                (auditory, "Auditory"),
                (discipline, "Discipline"),
                (teacher, "Profesor")
            };

            foreach (var check in checks)
            {
                if (check.entity == null)
                {
                    return new NotFoundObjectResult(new { message = new ErrorMessage().GetNotFoundMessage(check.name) });
                }
            }

            return null;
        }

        public async Task<IActionResult> AddTeacher(ProffessorDTO newTeacher)
        {
            var id = Guid.NewGuid();
            var disciplineIds = newTeacher.Disciplines.Select(d => d.Id).ToList();
            var disciplines = await _context.DisciplineEntities
                .Where(x => disciplineIds.Contains(x.Id))
                .ToListAsync();

            await _context.AddAsync(new TeacherEntity
            {
                Name = newTeacher.Name,
                Email = newTeacher.Email,
                Id = id,
                IsActive = true,
                Disciplines = disciplines
            });

            await _context.SaveChangesAsync();

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

        public async Task<IActionResult> SetPair(SetTimeSlotModel newPairData)
        {
            var auditory = await _context.AuditoryEntities.FirstOrDefaultAsync(x => x.Number == newPairData.Auditory);
            var discipline = await _context.DisciplineEntities.FirstOrDefaultAsync(x => x.Id.ToString() == newPairData.Discipline);
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => GetActiveTeacherById(x, newPairData.Professor));

            var groups = await _context.GroupEntities
                .Where(x => newPairData.Groups.Contains(x.Number) && x.IsActive)
                .ToListAsync();

            var newPair = new PairEntity
            {
                Auditory = auditory,
                Discipline = discipline,
                Group = groups,
                IsActive = true,
                Id = Guid.NewGuid(),
                LessonType = newPairData.Type,
                Teacher = teacher
            };
            await _context.PairEntities.AddAsync(newPair);

            var day = await GetOrCreateDayAsync(newPairData.Date);
            var timeSlot = await GetOrCreateTimeSlotAsync(day, newPairData.PairNumber);

            timeSlot.Pair ??= new List<PairEntity>();
            timeSlot.Pair.Add(newPair);

            _context.DayEntities.Update(day);
            _context.TimeSlotEntities.Update(timeSlot);


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

            var validationResult = ValidatePairUpdateDependencies(pair, auditory, discipline, teacher);
            if (validationResult != null)
            {
                return validationResult;
            }


            var groups = await _context.GroupEntities
                .Where(x => PairNewData.Groups.Contains(x.Number) && x.IsActive)
                .ToListAsync();

            pair.Auditory = auditory;
            pair.Discipline = discipline;
            pair.Teacher = teacher;
            pair.Group = groups;

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
