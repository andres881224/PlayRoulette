using PlayRoulette.API.Enum;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Models
{
    public class HistoryRouletteColor : HistoryRouletteRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Color Apuesta")]
        public Colors Color { get; set; }
    }
}
