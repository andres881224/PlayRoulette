using Microsoft.AspNetCore.Identity;
using PlayRoulette.API.Data.Entities;
using System.Threading.Tasks;

namespace PlayRoulette.API.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUser(string id = null, string userName = null);
        Task NewRole(string roleName);
        Task AddUserToRole(User user, string roleName);
        Task<IdentityResult> AddUser(User user, string password);
        Task<SignInResult> ValidatePassword(User user, string password);
    }
}
