using PlayRoulette.API.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayRoulette.API.Data.Entities
{
    public class Roulette
    {
        public int Id { get; set; }
        public Guid Name { get; set; }
        public StateRoulette StateRoulette { get; set; }
        public int WinNumber { get; set; }

        [NotMapped]
        public string StateRouletteName => StateRoulette.ToString();
    }
}
