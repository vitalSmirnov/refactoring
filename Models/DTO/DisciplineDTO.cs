using CloneIntime.Entities;
using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class DisciplineDTO : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
