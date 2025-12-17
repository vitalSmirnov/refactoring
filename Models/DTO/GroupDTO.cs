using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class GroupDTO
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string Name { get; set; }
        public DirectionDTO Direction { get; set; }
    }
}
