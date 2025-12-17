using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class DirectionDTO
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Number { get; set; }
        public FacultyDTO  Faculty {get;set;}
    }
}
