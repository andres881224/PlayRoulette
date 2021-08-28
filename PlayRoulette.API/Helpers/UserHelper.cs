using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayRoulette.API.Data;
using PlayRoulette.API.Data.Entities;
using System.Threading.Tasks;

namespace PlayRoulette.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<User> GetUser(string id = null, string userName = null)
        {
            if (id!= null)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task NewRole(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> AddUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRole(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<SignInResult> ValidatePassword(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }


    }
}
