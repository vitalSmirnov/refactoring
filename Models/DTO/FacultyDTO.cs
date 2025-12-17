using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class FacultyDTO
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Number { get; set; }
    }
}
