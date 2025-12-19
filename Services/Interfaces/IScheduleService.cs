using CloneIntime.Models.DTO;

namespace CloneIntime.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<WeekDTO> GetGroupsSchedule(List<string> groupId, WeekDateDTO date);
        Task<WeekDTO> GetAuditorySchedule(string audId, WeekDateDTO date);
        Task<WeekDTO> GetTecherSchedule(string teacherId, WeekDateDTO date);
    }
}
