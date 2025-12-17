using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.DTO
{
    public class TokenResponseDTO
    {
        [Required, MinLength(1)]
        public string Token { get; set; }
    }
}
