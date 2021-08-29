using System;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Models
{
    public class HistoryRouletteNumber : HistoryRouletteRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Número Apuesta")]
        [Range(minimum: 0, maximum: 36, ErrorMessage = "")]
        public int Number { get; set; }
    }
}
