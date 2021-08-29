using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayRoulette.API.Helpers
{
    public interface IRouletteHelper
    {
        Task<Guid> Create();
        Task<bool> Open(Guid name);
        Task<List<HistoryRouletteWinners>> Close(Guid name);
        Task<string> Bet(HistoryRoulette model);
        Task<List<Roulette>> GetStatus();

    }
}
