using Microsoft.EntityFrameworkCore;
using PlayRoulette.API.Data;
using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Enum;
using PlayRoulette.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayRoulette.API.Helpers
{
    public class RouletteHelper : IRouletteHelper
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public RouletteHelper(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<Guid> Create()
        {
            var model = new Roulette() { Name = Guid.NewGuid(), StateRoulette = StateRoulette.Open };
            _context.Roulettes.Add(model);
            await _context.SaveChangesAsync();

            return model.Name;
        }

        public async Task<bool> Open(Guid name)
        {
            var model = await _context.Roulettes.Where(x => x.Name == name && x.StateRoulette == StateRoulette.NA).FirstOrDefaultAsync();
            if (model == null)
            {
                return false;
            }
            model.StateRoulette = StateRoulette.Open;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<HistoryRouletteWinners>> Close(Guid name)
        {
            var model = await _context.Roulettes.Where(x => x.Name == name && x.StateRoulette == StateRoulette.Open).FirstOrDefaultAsync();
            if (model == null)
            {
                return null;
            }
            model.StateRoulette = StateRoulette.Close;
            model.WinNumber = new Random().Next(0, 36);
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var result = await (from a in _context.Roulettes
                                join b in _context.HistoryRoulettes on a.Id equals b.RouletteId
                                join c in _context.Users on b.UserId equals c.Id
                                orderby c.UserName
                                where a.Id == model.Id &&
                                    (
                                        (b.TypeBet == TypeBet.Number && a.WinNumber == b.Number) ||
                                        (b.TypeBet == TypeBet.Color && b.Color == (a.WinNumber % 2 == 0 ? Colors.Red : Colors.Black))
                                    )                                
                                select new HistoryRouletteWinners()
                                {
                                    UserName = c.UserName,
                                    RouletteName = a.Name,
                                    WinNumber = a.WinNumber,

                                    TypeBet = b.TypeBet,
                                    Number = b.Number,
                                    Color = b.Color,
                                    BetValue = b.BetValue,

                                    TotalNumber = (b.TypeBet == TypeBet.Number && a.WinNumber == b.Number) ? 5 * (b.BetValue) : 0,
                                    TotalColor = (b.TypeBet == TypeBet.Color && b.Color == (a.WinNumber % 2 == 0 ? Colors.Red : Colors.Black)) ? Convert.ToDecimal(1.8) * (b.BetValue) : 0,

                                }).ToListAsync();
            return result;
        }

        public async Task<string> Bet(HistoryRoulette model)
        {
            var user = await _userHelper.GetUser(id: model.UserId);
            if (user == null)
            {
                return "El usuario no existe.";
            }
            if (model.BetValue > user.AccountBalance)
            {
                return "El valor de la apuesta supera el saldo del usuario.";
            }
            if (await _context.Roulettes.Where(x => x.Id == model.RouletteId && x.StateRoulette != StateRoulette.Open).AnyAsync())
            {
                return "La ruleta no se encuentra abierta.";
            }
            _context.HistoryRoulettes.Add(model);
            await _context.SaveChangesAsync();

            return "Apuesta Creada";
        }

        public async Task<List<Roulette>> GetAll()
        {
            return await _context.Roulettes.ToListAsync();
        }

        public async Task<List<Roulette>> GetStatus()
        {
            return await _context.Roulettes.ToListAsync();
        }

    }
}
