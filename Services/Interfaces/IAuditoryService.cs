using CloneIntime.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Services.Interfaces
{
    public interface IAuditoryService 
    {
        Task<List<AuditoryDTO>> GetAuditory();

    }
}
