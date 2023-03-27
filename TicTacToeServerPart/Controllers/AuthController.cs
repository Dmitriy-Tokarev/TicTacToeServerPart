using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicTacToeServerPart.Models;

namespace TicTacToeServerPart.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        public AuthController(TicTacToeContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        private readonly TicTacToeContext _dbContext;

        private readonly IConfiguration _configuration;

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<PlayerModel>> RegistratePlayer(PlayerModel player)
        {
            if (_dbContext.Players.Any(playerInDb => playerInDb.LoginModel.EmailAddress == player.LoginModel.EmailAddress ))
            {
                return BadRequest("Данный E-mail уже существует");
            }
            else
            {
                await _dbContext.Players.AddAsync(player);
                await _dbContext.SaveChangesAsync();

                return Ok(player);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public string Login(string email, string password)
        {
            if (_dbContext.Login.Any(loginData => loginData.EmailAddress == email))
            {
                if (_dbContext.Login.Any(loginData => loginData.Password == password))
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuth:Key"]!));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Email, email),
                    };

                    var jwt = new JwtSecurityToken(
                        audience: _configuration["JwtAuth:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromDays(5)),
                        signingCredentials: credentials
                        );

                    return new JwtSecurityTokenHandler().WriteToken(jwt);
                }
                else
                {
                    return "Не верный E-mail/пароль";
                }
            }
            else
            {
                return "Не верный E-mail/пароль";
            }
        }
    }
}
