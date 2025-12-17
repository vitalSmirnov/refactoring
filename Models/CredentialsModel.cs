using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CloneIntime.Models
{
    public class CredentialsModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
