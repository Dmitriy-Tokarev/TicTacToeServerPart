using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicTacToeServerPart.Models;

namespace TicTacToeServerPart.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        public InformationController(TicTacToeContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly TicTacToeContext _dbContext;

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<PlayerModel>>> GetPlayers()
        {
            if (_dbContext.Players == null)
            {
                return NotFound();
            }

            return await _dbContext.Players.Include(x => x.LoginModel).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PlayerModel>> GetPlayer(int id)
        {
            if (_dbContext.Players.Any(player => player.Id == id))
            {
                return await _dbContext.Players
                    .Include(player => player.LoginModel)
                    .FirstAsync(player => player.Id == id);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<InGameLogicModel>>> GetAvailableGames()
        {
            return await _dbContext.InGameLogic
                .Where(game => game.OnLine == true && game.PublicSearchType == true)
                .ToListAsync();
        }
        
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<PlayerModel>> ChangePlayerData(int id, PlayerModel dataToChange)
        {
            if (await _dbContext.Players
                .Where(player => player.Id != dataToChange.Id)
                .AnyAsync(player => player.PhoneNumber == dataToChange.PhoneNumber))
            {
                return BadRequest("Не корректные данные");
            }

            if (await _dbContext.Players
                .Include(player => player.LoginModel)
                .Where(player => player.Id != dataToChange.Id)
                .AnyAsync(player => player.LoginModel.EmailAddress == dataToChange.LoginModel.EmailAddress))
            {
                return BadRequest("Не корректные данные");
            }

            if (_dbContext.Players.Any(player => player.Id == id))
            {
                var changeablePlayer = await _dbContext.Players
                    .Include(player => player.LoginModel)
                    .FirstAsync(player => player.Id == id);

                changeablePlayer.Name = changeablePlayer.Name == dataToChange.Name ? 
                    changeablePlayer.Name : dataToChange.Name;

                changeablePlayer.PhoneNumber = changeablePlayer.PhoneNumber == dataToChange.PhoneNumber ?
                    changeablePlayer.PhoneNumber : dataToChange.PhoneNumber;

                changeablePlayer.LoginModel.Password = changeablePlayer.LoginModel.Password == dataToChange.LoginModel.Password ? 
                    changeablePlayer.LoginModel.Password : dataToChange.LoginModel.Password;

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return NotFound("Пользователь не найден");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> DeletePlayer(int id)
        {
            if (_dbContext.Players.Any(player => player.Id == id))
            {
                var player = await _dbContext.Players
                    .Include(player => player.LoginModel)
                    .FirstAsync(player => player.Id == id);

                var loginId = player.LoginModel;

                _dbContext.Players.Remove(player);
                _dbContext.Login.Remove(loginId);
                await _dbContext.SaveChangesAsync();
                
                return Ok($"Игрок {player.Name} удален!");
            }
            else 
            { 
                return NotFound("Игрок не найден"); 
            }
        }
    }
}
