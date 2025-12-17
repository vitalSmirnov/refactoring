using CloneIntime.Models.DTO;
using CloneIntime.Services;
using CloneIntime.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Controllers
{
    [Route("api/disciplines")]
    [ApiController]
    public class DisciplineController : ControllerBase
    {

        private readonly IDisciplineService _disciplineService;

        public DisciplineController(DisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        [HttpGet("{facultyId}")]
        public async Task<List<DisciplineDTO>> GetDisciplines(string facultyId)
        {
            return await _disciplineService.GetDisciplines(facultyId);
        }

        [HttpGet]
        public async Task<List<DisciplineDTO>> GetAllDisciplines()
        {
            return await _disciplineService.GetDisciplines();
        }
    }
}
