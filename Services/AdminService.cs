using CloneIntime.Entities;
using CloneIntime.Models.DTO;
using CloneIntime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using CloneIntime.Models.Entities;

namespace CloneIntime.Services
{
    public class AdminService
    {
        private readonly Context _context;
        private readonly SupportService _support;
        public AdminService(Context context, SupportService support)
        {
            _context = context;
            _support = support;
        }

        private List<DisciplineEntity> fillDiscipline(List<DisciplineDTO> disciplines)
        {
            var disciplineIds = disciplines.Select(disc => disc.Id).ToList();

            var result = _context.DisciplineEntities
                    .Where(x => disciplineIds.Contains(x.Id))
                .ToList();

            return result;
        }

        public async Task<IActionResult> AddTeacher(ProffessorDTO newTeacher)// Получить группы на определенном направлении
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
        public async Task<IActionResult> UpdateTeacher(string teacherId, ProffessorDTO newTeacher)// Получить группы на определенном направлении
        {
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => x.Id.ToString() == teacherId && x.IsActive);
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
        public async Task<IActionResult> DeleteTeacher(string teacherId)// Получить группы на определенном направлении
        {
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => x.Id.ToString() == teacherId && x.IsActive);

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

        private WeekEnum checkWeekDay(DateTime day)
        {
            switch (day.DayOfWeek)
            {
                case DayOfWeek.Monday: 
                    return WeekEnum.Monday;
                case DayOfWeek.Tuesday:
                    return WeekEnum.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekEnum.Wednesday;
                case DayOfWeek.Thursday:
                    return WeekEnum.Thursday;
                case DayOfWeek.Friday:
                    return WeekEnum.Friday;
                case DayOfWeek.Saturday:
                    return WeekEnum.Saturday;
                default:
                    return WeekEnum.Sunday;
            }
        }

        public async Task<IActionResult> SetPair(SetTimeSlotModel newPairData)// Получить группы на определенном направлении
        {
            var auditory = await _context.AuditoryEntities.FirstOrDefaultAsync(x => x.Number == newPairData.Auditory);
            var discipline = await _context.DisciplineEntities.FirstOrDefaultAsync(x => x.Id.ToString() == newPairData.Discipline);
            var teacher = await _context.TeachersEntities.FirstOrDefaultAsync(x => x.Id.ToString() == newPairData.Professor);
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
                var newTimeslot = new TimeSlotEntity
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Pair = new List<PairEntity> { newPair },
                    PairNumber = newPairData.PairNumber
                };
                day = new DayEntity
                {
                    Date = newPairData.Date,
                    DayName = checkWeekDay(newPairData.Date.Date),
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    Lessons = new List<TimeSlotEntity> {
                        newTimeslot
                    }
                };
                await _context.TimeSlotEntities.AddAsync(newTimeslot);
                await _context.DayEntities.AddAsync(day);
            }
            else
            {
                var existingTimeSlot = day.Lessons.FirstOrDefault(x => x.PairNumber == newPairData.PairNumber);//переделать, так как не работает

                if (existingTimeSlot == null) {
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

            if (pair == null)
                return new NotFoundObjectResult(new {message =  "Pair not found" }); // прописать ошибку
            if (auditory == null)
                return new NotFoundObjectResult(new { message = "auditory not found" }); // прописать ошибку
            if (discipline == null)
                return new NotFoundObjectResult(new { message = "discipline not found" }); // прописать ошибку
            if (teacher == null)
                return new NotFoundObjectResult(new { message = "teacher not found" }); // прописать ошибку

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
                return new NotFoundObjectResult(new { message = "Pair not found" }); // прописать ошибку

            _context.PairEntities.Remove(pair);
            _context.SaveChangesAsync();

            return new OkResult();

        }

        public async Task<ActionResult<TokenResponseDTO>> Login(CredentialsModel model)
        {
            var user = await _context.EditorEntity.FirstOrDefaultAsync(x => x.Login == model.Email);

            if (user == null)
                return new NotFoundResult();

            if (user.Password != model.Password)
                return new BadRequestObjectResult(new { 
                    message = "Incorrect Password",
                });

            var id = user.Id;
            var token = new JwtSecurityTokenHandler().WriteToken(_support.GenerateJWT(model.Email, user.Id.ToString()));


            return new TokenResponseDTO
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

            return new JsonResult(new { message = "Logged out" });
        }
    }
}
