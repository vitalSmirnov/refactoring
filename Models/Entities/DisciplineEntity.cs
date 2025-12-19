using CloneIntime.Models.ModelTypes;

namespace CloneIntime.Entities;

public class DisciplineEntity : DisciplineModel
{
    public List<TeacherEntity> Teachers { get; set; }
}