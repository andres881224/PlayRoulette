using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        public decimal AccountBalance { get; set; }
    }
}
