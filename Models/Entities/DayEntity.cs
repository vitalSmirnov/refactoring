namespace CloneIntime.Entities;

public class DayEntity:BaseEntity
{
    public WeekEnum DayName { get; set; }
    public DateTime Date { get; set; }
    public List<TimeSlotEntity> Lessons { get; set; }
}   