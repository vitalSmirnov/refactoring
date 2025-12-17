namespace CloneIntime.Entities;

public class FacultyEntity : BaseEntity
{
    public string Number { get; set; }
    public string Name { get; set; }

    public List<DisciplineEntity> Disciplines { get; set; }
}