using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class DayDTO
    {
        public List<TimeSlotDTO> Timeslot { get; set; }
        public string Day { get; set; }
        public WeekEnum WeekDay { get; set; }
        public Int32 countClases { get; set; }

    }
}
