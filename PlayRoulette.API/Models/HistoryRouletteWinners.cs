using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Enum;
using System;

namespace PlayRoulette.API.Models
{
    public class HistoryRouletteWinners
    {
        public string UserName { get; set; }
        public Guid RouletteName { get; set; }
        public int WinNumber { get; set; }

        public TypeBet TypeBet { get; set; }
        public int Number { get; set; }
        public Colors Color { get; set; }
        public decimal BetValue { get; set; }

        public decimal TotalColor { get; set; }
        public decimal TotalNumber { get; set; } 

        public string TypeBetName => TypeBet.ToString();
        public string ColorName => Color.ToString();
        public decimal Total => TotalColor + TotalNumber;
    }
}
