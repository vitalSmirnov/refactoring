using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IDisciplineService
    {
        Task<List<DisciplineDTO>> GetDisciplines(string groupId);
        Task<List<DisciplineDTO>> GetDisciplines();
    }
}
