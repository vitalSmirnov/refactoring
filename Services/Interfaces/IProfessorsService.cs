using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IProfessorsService
    {
        Task<List<ProffessorDTO>> GetProfessors();

        Task<List<ProffessorDTO>> GetProfessors(string id);
    }
}
