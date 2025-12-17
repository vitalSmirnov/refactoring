using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using CloneIntime.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Controllers
{
    [ApiController]
    [Route("api/auditories")]
    public class AuditoryController : ControllerBase
    {
        private readonly IAuditoryService _auditoryService;

        public AuditoryController(AuditoryService auditoryService)
        {
            _auditoryService = auditoryService;
        }

        [HttpGet]
        public async Task<List<AuditoryDTO>> GetAuditory()
        {
            return await _auditoryService.GetAuditory();
        }

    }
}
