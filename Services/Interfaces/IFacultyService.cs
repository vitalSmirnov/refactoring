using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<List<FacultyDTO>> GetFaculties();
    }
}
