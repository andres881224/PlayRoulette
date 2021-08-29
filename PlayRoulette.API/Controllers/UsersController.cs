using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlayRoulette.API.Data.Entities;
using PlayRoulette.API.Helpers;
using PlayRoulette.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PlayRoulette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RoulettesController> _logger;

        public UsersController(IUserHelper userHelper, ILogger<RoulettesController> logger, IConfiguration configuration)
        {
            _userHelper = userHelper;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Token")]
        public async Task<IActionResult> Token([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUser(userName: model.UserName);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePassword(user, model.Password);
                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };
                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.TokensKey]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token = new JwtSecurityToken(
                            _configuration[Constants.TokensIssuer],
                            _configuration[Constants.TokensAudience],
                            claims,
                            expires: DateTime.UtcNow.AddDays(Constants.DayNumberThree),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }




    }
}
