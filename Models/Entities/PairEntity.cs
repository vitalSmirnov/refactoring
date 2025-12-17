namespace CloneIntime.Entities;

public class PairEntity : BaseEntity
{
    public DisciplineEntity Discipline { get; set; }
    public List<GroupEntity> Group { get; set; }
    public TeacherEntity Teacher { get; set; }
    public AuditoryEntity Auditory { get; set; }
    public LessonTypeEnum LessonType { get; set; }
}