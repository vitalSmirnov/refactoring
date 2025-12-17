using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IDirectionService
    {
        Task<List<DirectionDTO>> GetDirections(string facultyId);
    }
}
