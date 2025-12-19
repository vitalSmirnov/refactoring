using CloneIntime.Services.Interfaces;
using CloneIntime.Services;
using Microsoft.AspNetCore.Mvc;
using CloneIntime.Models.DTO;

namespace CloneIntime.Controllers
{
    [Route("api/direction")]
    [ApiController]
    public class DirectionController : ControllerBase
    {
        private readonly IDirectionService _directionService;

        public DirectionController(DirectionService directionService)
        {
            _directionService = directionService;
        }

        [HttpGet("{facultyId}")]
        public async Task<List<DirectionDTO>> GetDirections(string facultyId)
        {
            return await _directionService.GetDirections(facultyId);
        }
    }
}
