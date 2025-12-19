using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloneIntime.Services
{
    public class DirectionService : IDirectionService
    {
        private readonly Context _context;
        public DirectionService(Context context)
        {
            _context = context;
        }

        private List<DirectionDTO> FillDirection(IQueryable<DirectionEntity> faculty)
        {
            var result = new List<DirectionDTO>();
            result.AddRange(faculty.Select(direction => new DirectionDTO
            {
                Name = direction.Name,
                Number = direction.Number,
            }));

            return result;
        }

        public async Task<List<DirectionDTO>> GetDirections(string facultyId)
        {
            var directionEntity = _context.DirectionEntities.Include(x => x.Faculty)
                .Where(j => j.Faculty.Id.ToString() == facultyId && j.IsActive);

            //TODO: прописать исключение
            if (directionEntity == null)
                return new List<DirectionDTO>();


            return FillDirection(directionEntity);
        }
    }
}