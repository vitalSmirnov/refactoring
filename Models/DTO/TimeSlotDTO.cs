using CloneIntime.Entities;
using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class TimeSlotDTO
    {
        public Int32 SlotNumber { get; set; }
        public List<PairDTO>? Pairs { get; set; }
    }
}
