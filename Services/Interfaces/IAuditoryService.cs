using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IAuditoryService 
    {
        Task<List<AuditoryDTO>> GetAuditory();

    }
}
