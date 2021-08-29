using PlayRoulette.API.Enum;
using System.ComponentModel.DataAnnotations;

namespace PlayRoulette.API.Data.Entities
{
    public class HistoryRoulette
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RouletteId { get; set; }
        public TypeBet TypeBet { get; set; }
        public int Number { get; set; }
        public Colors Color { get; set; }
        public decimal BetValue { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Roulette Roulette { get; set; }
    }
}
