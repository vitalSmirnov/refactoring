using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloneIntime.Services
{
    public class ProfessorsService : IProfessorsService
    {
        private readonly Context _context;
        public ProfessorsService(Context context)
        {
            _context = context;
        }

        private List<DisciplineDTO> fillDisciplines(List<DisciplineEntity> disciplines)
        {
            var result = new List<DisciplineDTO>();
            result.AddRange(disciplines.Select(x => new DisciplineDTO
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            }));
            return result;
        }

        private List<ProffessorDTO> FillProfessors(List<TeacherEntity> teachers)
        {
            var result = new List<ProffessorDTO>();
            
            result.AddRange(teachers.Select(professor => new ProffessorDTO
            {
                Id = professor.Id,
                Name = professor.Name,
                Email = professor.Email,
                //Disciplines = fillDisciplines(professor.Disciplines)
            }));

            return result;
        }

        public async Task<List<ProffessorDTO>> GetProfessors() // Получить преподов на определенном направлении
        {
            var professorsEntities = await _context.TeachersEntities
                .Include(x=> x.Disciplines)
                .Where(x => x.IsActive)
                .ToListAsync();

            if (professorsEntities == null)
                return new List<ProffessorDTO>(); //прописать исключение


            return FillProfessors(professorsEntities);
        }

        public async Task<List<ProffessorDTO>> GetProfessors(string disciplineId) // Получить преподов на определенном Предмете
        {
            var professorsEntities = await _context.TeachersEntities
                .Where(x => x.IsActive && x.Disciplines
                .Any(d => d.Id.ToString() == disciplineId))
                .ToListAsync();


            if (professorsEntities == null)
                return new List<ProffessorDTO>(); //прописать исключение


            return FillProfessors(professorsEntities);
        }
    }
}
