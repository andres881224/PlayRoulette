using System;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Models
{
    public class HistoryRouletteRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Ruleta")]
        public Guid RouletteId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Valor Apuesta")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "El monto maximo de la apuesta es 10.000 USD")]
        public decimal BetValue { get; set; }
    }
}
