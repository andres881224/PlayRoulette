using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Enum;
using PlayRoulette.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayRoulette.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await NewRoles();
            await NewUser("Administrador", "administrador", "administrador@correo.com", UserType.Admin, 0);

            await NewUser("Usuario 1", "usuario1", "usuario1@correo.com", UserType.User, 1000);
            await NewUser("Usuario 2", "usuario2", "usuario2@correo.com", UserType.User, 2000);
            await NewUser("Usuario 3", "usuario3", "usuario3@correo.com", UserType.User, 3000);
            await NewUser("Usuario 4", "usuario4", "usuario4@correo.com", UserType.User, 4000);
            await NewUser("Usuario 5", "usuario5", "usuario5@correo.com", UserType.User, 5000);
            await NewUser("Usuario 6", "usuario6", "usuario6@correo.com", UserType.User, 6000);
            await NewUser("Usuario 7", "usuario7", "usuario7@correo.com", UserType.User, 7000);
            await NewUser("Usuario 8", "usuario8", "usuario8@correo.com", UserType.User, 8000);
            await NewUser("Usuario 9", "usuario9", "usuario9@correo.com", UserType.User, 9000);

            await CheckRoulettes();
            await CheckHistoryRoulette();
        }

        private async Task<User> NewUser(string name, string userName, string email, UserType userType, decimal accountBalance)
        {
            User user = await _userHelper.GetUser(userName);
            if (user == null)
            {
                user = new User
                {
                    FullName = name,
                    Email = email,
                    UserName = userName,
                    AccountBalance = accountBalance
                };
                await _userHelper.AddUser(user, userName);
                await _userHelper.AddUserToRole(user, userType.ToString());
            }

            return user;
        }

        private async Task NewRoles()
        {
            await _userHelper.NewRole(UserType.Admin.ToString());
            await _userHelper.NewRole(UserType.User.ToString());
        }

        private async Task CheckRoulettes()
        {
            if (!_context.Roulettes.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    _context.Roulettes.Add(new Roulette { Name = Guid.NewGuid(), StateRoulette = StateRoulette.NA, WinNumber = -1 });
                }
                await _context.SaveChangesAsync();
            }
        }
        //BetValue = Convert.ToDecimal(new Random().Next(0, 10000))
        private async Task CheckHistoryRoulette()
        {
            Random r = new Random();
            List<string> lstUsers = _context.Users.Select(x => x.Id).ToList();

            if (!_context.HistoryRoulettes.Any())
            {
                for (int i = 1; i <= 1000; i++)
                {
                    HistoryRoulette model = new HistoryRoulette()
                    {
                        UserId = lstUsers[r.Next(lstUsers.Count)],
                        RouletteId = r.Next(1, 10),
                        TypeBet = TypeBet.Color,
                        Color = Colors.Black,
                        BetValue = r.Next(1, 10000),
                        Number = 0
                    };
                    _context.HistoryRoulettes.Add(model);
                }
                await _context.SaveChangesAsync();

                for (int i = 1; i <= 1000; i++)
                {
                    HistoryRoulette model = new HistoryRoulette()
                    {
                        UserId = lstUsers[r.Next(lstUsers.Count)],
                        RouletteId = r.Next(1, 10),
                        TypeBet = TypeBet.Number,
                        Color = Colors.NA,
                        BetValue = r.Next(1, 10000),
                        Number = r.Next(0, 36)
                    };
                    _context.HistoryRoulettes.Add(model);
                }

                await _context.SaveChangesAsync();
            }
        }

    }
}
