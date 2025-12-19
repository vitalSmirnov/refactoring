using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;

namespace CloneIntime.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly Context _context;

        public FacultyService(Context context)
        {
            _context = context;
        }

        private List<FacultyDTO> FillFaculties(IQueryable<FacultyEntity> faculty)
        {
            var result = new List<FacultyDTO>();
            result.AddRange(faculty.Select(direction => new FacultyDTO
            {
                id = direction.Id,
                Name = direction.Name,
                Number = direction.Number,
            }));

            return result;
        }

        public async Task<List<FacultyDTO>> GetFaculties()
        {
            var directionEntity = _context.FacultyEntities.Where(x => x.IsActive == true);

            if (directionEntity == null)
                return new List<FacultyDTO>(); //прописать исключение


            return FillFaculties(directionEntity);
        }
    }
}
