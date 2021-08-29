using System;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Models
{
    public class RouletteRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Ruleta")]
        public Guid Name { get; set; }
    }
}
