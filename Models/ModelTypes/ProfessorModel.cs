using CloneIntime.Entities;
using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.ModelTypes
{
    public class ProfessorModel : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        [MinLength(6)]
        public string Email { get; set; }

        public List<DisciplineEntity> Disciplines { get; set; }
    }
}
