using System;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Models
{
    public class RouletteRequest
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Ruleta")]
        public Guid Name { get; set; }
    }
}
