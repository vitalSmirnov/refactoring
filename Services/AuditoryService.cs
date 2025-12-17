using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloneIntime.Services
{
    public class AuditoryService : IAuditoryService
    {
        private readonly Context _context;
        public AuditoryService(Context context)
        {
            _context = context;
        }
        private List<AuditoryDTO> FillProfessors(IQueryable<AuditoryEntity> audit)
        {
            var result = new List<AuditoryDTO>();
            result.AddRange(audit.Select(auditory => new AuditoryDTO
            {
                Id = auditory.Id,
                Name = auditory.Number + " (" + auditory.Building.Trim() + ")",
                Number = auditory.Number,
            }));

            return result;
        }

        public async Task<List<AuditoryDTO>> GetAuditory() // Получить группы на определенном направлении
        {
            var auditoryEntities = _context.AuditoryEntities.Where(x => x.IsActive);

            if (auditoryEntities == null)
                return new List<AuditoryDTO>(); //прописать исключение


            return FillProfessors(auditoryEntities);
        }
    }
}
