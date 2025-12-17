using CloneIntime.Models.DTO;
using CloneIntime.Services;
using CloneIntime.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Controllers
{
    [Route("api/faculties")]
    [ApiController]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(FacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet]
        public async Task<List<FacultyDTO>> GetFaculties()
        {
            var result = await _facultyService.GetFaculties();
            return result;
        }
    }
}
