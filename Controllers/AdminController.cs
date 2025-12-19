using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Models.ModelTypes;
using CloneIntime.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService; // вернуть интерфейс вмесо сервиса
        private readonly SupportService _supportService;

        public AdminController(AdminService adminService, SupportService supportService)
        {
            _adminService = adminService;
            _supportService = supportService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenModel>> Login([FromBody] CredentialsModel loginCredentials)
        {
            return await _adminService.Login(loginCredentials);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            return await _adminService.Logout(await _supportService.GetToken(HttpContext.Request.Headers));
        }

        [HttpPost("add/teacher")]
        public async Task<IActionResult> AddTeacher([FromBody] ProffessorDTO newTeacher)
        {
            return await _adminService.AddTeacher(newTeacher);
        }

        [HttpPut("update/teacher/{teacherId}")]
        public async Task<IActionResult> UpdateTeacher(string teacherId, [FromBody] ProffessorDTO newTeacherData)
        {
            return await _adminService.UpdateTeacher(teacherId, newTeacherData);
        }

        [HttpDelete("delete/teacher/{teacherId}")]
        public async Task<IActionResult> DeleteTeacher(string teacherId)
        {
            return await _adminService.DeleteTeacher(teacherId);
        }


        [HttpPost("add/pair")]
        [Authorize]
        public async Task<IActionResult> SetPair(SetTimeSlotModel newPairData)
        {
            return await _adminService.SetPair(newPairData);
        }

        [HttpDelete("delete/pair/{id}")]
        public async Task DeletePair(string id)
        {
            await _adminService.DeletePair(id);
        }

        [HttpPut("update/pair/{id}")]
        public async Task UpdatePair(string id, SetTimeSlotModel PairNewData)
        {
            await _adminService.UpdatePair(id, PairNewData);
        }
    }
}