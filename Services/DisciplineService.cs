using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using CloneIntime.Utils.helpers;
using Microsoft.EntityFrameworkCore;

namespace CloneIntime.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly Context _context;
        private readonly DisciplineHelper _disciplineHelper;

        public DisciplineService(Context context, DisciplineHelper disciplineHelper)
        {
            _context = context;
            _disciplineHelper = disciplineHelper;

        }

        public async Task<List<DisciplineDTO>> GetDisciplines(string facultyId)
        {
            var disciplineEntity = await _context.FacultyEntities
                .Include(x=> x.Disciplines)
                .FirstOrDefaultAsync(x => x.Id.ToString() == facultyId);

            if (disciplineEntity == null)
                return new List<DisciplineDTO>();

            return _disciplineHelper.FillDisciplines(disciplineEntity);

        }

        public async Task<List<DisciplineDTO>> GetDisciplines()
        {
            var disciplineEntity = await _context.DisciplineEntities.Where(x=> x.IsActive).ToListAsync();

            if (disciplineEntity == null)
                return new List<DisciplineDTO>();


            return _disciplineHelper.FillDisciplines(disciplineEntity);

        }
    }
}
