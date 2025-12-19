using CloneIntime.Models.ModelTypes;

namespace CloneIntime.Entities;

public class FacultyEntity : FacultyModel
{

    public List<DisciplineEntity> Disciplines { get; set; }
}