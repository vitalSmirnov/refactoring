using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;

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
            var AuditoryDTO = new AuditoryDTO();
            result.AddRange(audit.Select(auditory => new AuditoryDTO
            {
                Id = auditory.Id,
                Name = AuditoryDTO.CreateName(auditory.Number, auditory.Building),
                Number = auditory.Number,
            }));

            return result;
        }

        public async Task<List<AuditoryDTO>> GetAuditory()
        {
            var auditoryEntities = _context.AuditoryEntities.Where(x => x.IsActive);

            if (auditoryEntities == null)
                return new List<AuditoryDTO>(); 


            return FillProfessors(auditoryEntities);
        }
    }
}
