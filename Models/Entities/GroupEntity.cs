namespace CloneIntime.Entities;

public class GroupEntity : BaseEntity
{
    public string Number { get; set; }
    public DirectionEntity Direction { get; set; }
    public List<PairEntity> Pairs { get; set; }
    public List<DisciplineEntity> Disciplines { get; set; }
    public string Name { get; set; }
}