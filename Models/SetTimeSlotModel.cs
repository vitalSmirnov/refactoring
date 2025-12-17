using CloneIntime.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models
{
    public class SetTimeSlotModel
    {
        public DateTime Date { get; set; }
        [Required]
        public UInt16 PairNumber { get; set; }
        public LessonTypeEnum Type { get; set; }
        [Required]
        public string Professor { get; set; }
        [Required]
        public List<string> Groups { get; set; }
        [Required]
        public string Discipline { get; set; }
        [Required]
        public string Auditory { get; set; }

    }
}
