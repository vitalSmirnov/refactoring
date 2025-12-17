using CloneIntime.Entities;
using CloneIntime.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CloneIntime.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<WeekDTO> GetGroupsSchedule(List<string> groupId, DateTime startDate, DateTime endDate);
        Task<WeekDTO> GetAuditorySchedule(string audId, DateTime startDate, DateTime endDate);
        Task<WeekDTO> GetTecherSchedule(string teacherId, DateTime startDate, DateTime endDate);
    }
}
