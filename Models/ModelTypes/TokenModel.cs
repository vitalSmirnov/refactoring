using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models.ModelTypes
{
    public class TokenModel
    {
        [Required, MinLength(1)]
        public string Token { get; set; }
    }
}
