using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class ProffessorDTO
    {

        public Guid id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public List<DisciplineDTO> Disciplines { get; set; }
    }
}
