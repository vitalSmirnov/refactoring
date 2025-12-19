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
            var interval = new WeekDateDTO{StartDate = startDate, EndDate = endDate};
            return await _scheduleService.GetGroupsSchedule(number, interval);
        }

        [HttpGet("auditory/{auditoryNumber}")]
        public async Task<WeekDTO> GetAuditorySchedule(string auditoryNumber, DateTime startDate, DateTime endDate)
        {
            var interval = new WeekDateDTO { StartDate = startDate, EndDate = endDate };
            return await _scheduleService.GetAuditorySchedule(auditoryNumber, interval);
        }

        [HttpGet("teacher")]
        public async Task<WeekDTO> GetTecherSchedule(string id, DateTime startDate, DateTime endDate)
        {
            var interval = new WeekDateDTO { StartDate = startDate, EndDate = endDate };
            return await _scheduleService.GetTecherSchedule(id, interval);
        }
    }
}
