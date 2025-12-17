using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloneIntime.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly Context _context;
        public DisciplineService(Context context)
        {
            _context = context;
        }

        private List<DisciplineDTO> FillDisciplines(FacultyEntity disciplines)
        {
            var result = new List<DisciplineDTO>();

            result.AddRange(disciplines.Disciplines.Select(x => new DisciplineDTO
            {
                Name = x.Name,
                Id = x.Id,
                IsActive = x.IsActive
            }));
            return result;
        }

        private List<DisciplineDTO> FillDisciplines(List<DisciplineEntity> disciplines)
        {
            var result = new List<DisciplineDTO>();

            result.AddRange(disciplines.Select(x => new DisciplineDTO
            {
                Name = x.Name,
                Id = x.Id,
                IsActive = x.IsActive
            }));
            return result;
        }
        public async Task<List<DisciplineDTO>> GetDisciplines(string facultyId)
        {
            var disciplineEntity = await _context.FacultyEntities
                .Include(x=> x.Disciplines)
                .FirstOrDefaultAsync(x => x.Id.ToString() == facultyId);

            if (disciplineEntity == null)
                return new List<DisciplineDTO>();


            return FillDisciplines(disciplineEntity);

        }

        public async Task<List<DisciplineDTO>> GetDisciplines()
        {
            var disciplineEntity = await _context.DisciplineEntities.Where(x=> x.IsActive).ToListAsync();

            if (disciplineEntity == null)
                return new List<DisciplineDTO>();


            return FillDisciplines(disciplineEntity);

        }
    }
}
