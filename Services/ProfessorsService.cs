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

        public async Task<List<ProffessorDTO>> GetProfessors()
        {
            var professorsEntities = await _context.TeachersEntities
                .Include(x=> x.Disciplines)
                .Where(x => x.IsActive)
                .ToListAsync();
            //TODO: прописать исключение
            if (professorsEntities == null)
                return new List<ProffessorDTO>(); 


            return FillProfessors(professorsEntities);
        }

        public async Task<List<ProffessorDTO>> GetProfessors(string disciplineId)
        {
            var professorsEntities = await _context.TeachersEntities
                .Where(x => x.IsActive && x.Disciplines
                .Any(d => d.Id.ToString() == disciplineId))
                .ToListAsync();


            if (professorsEntities == null)
                return new List<ProffessorDTO>();


            return FillProfessors(professorsEntities);
        }
    }
}
