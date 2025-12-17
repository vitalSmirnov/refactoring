namespace CloneIntime.Entities;

public class TimeSlotEntity : BaseEntity
{
    public List<PairEntity> Pair { get; set; }
    public UInt16 PairNumber { get; set; }
}