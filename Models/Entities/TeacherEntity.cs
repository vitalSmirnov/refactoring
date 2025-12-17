using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Entities;

public class TeacherEntity : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [EmailAddress]
    [MinLength(6)]
    public string Email { get; set; }
    public List<DisciplineEntity> Disciplines { get; set; }
}