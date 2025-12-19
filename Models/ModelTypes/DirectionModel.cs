using CloneIntime.Entities;
using CloneIntime.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.ModelTypes
{
    public class DirectionModel : BaseEntity
    {
        public string Name { get; set; }
        [Required]
        public string Number { get; set; }
        public FacultyDTO Faculty { get; set; }
    }
}
