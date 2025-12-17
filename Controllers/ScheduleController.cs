using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services;
using CloneIntime.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("group")]
        public async Task<WeekDTO> GetGroupsSchedule([FromQuery] List<string> number, DateTime startDate, DateTime endDate)
        {
            return await _scheduleService.GetGroupsSchedule(number, startDate, endDate);
        }

        [HttpGet("auditory/{auditoryNumber}")]
        public async Task<WeekDTO> GetAuditorySchedule(string auditoryNumber, DateTime startDate, DateTime endDate)
        {
            return await _scheduleService.GetAuditorySchedule(auditoryNumber, startDate, endDate);
        }

        [HttpGet("teacher")]
        public async Task<WeekDTO> GetTecherSchedule(string id, DateTime startDate, DateTime endDate)
        {
            return await _scheduleService.GetTecherSchedule(id, startDate, endDate);
        }
    }
}
