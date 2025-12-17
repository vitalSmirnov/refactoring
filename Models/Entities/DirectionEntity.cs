namespace CloneIntime.Entities;

public class DirectionEntity : BaseEntity
{
    public string Number { get; set; }
    public string Name { get; set; }
    public FacultyEntity Faculty { get; set; }
}