using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToeServerPart.Models;

namespace TicTacToeServerPart.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InGameController : ControllerBase
    {
        public InGameController(TicTacToeContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly TicTacToeContext _dbContext;

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<InGameLogicModel>> CreateGameSession(PlayerModel gameCreator, bool publicSearchType)
        {
            if (await _dbContext.Players.AnyAsync(player => player.Id == gameCreator.Id))
            {
                var gameSession = new InGameLogicModel()
                {
                    FirstPlayerId = gameCreator.Id,
                    PublicSearchType = publicSearchType
                };

                await _dbContext.InGameLogic
                    .AddAsync(gameSession);

                await _dbContext.SaveChangesAsync();

                return gameSession;
            }
            else
            {
                return BadRequest("Игрок не найден");
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<InGameLogicModel>> JoinTheGame(PlayerModel connectingPlayer, int gameId)
        {
            var currentGame = await _dbContext.InGameLogic
                .FirstAsync(game => game.Id == gameId);

            if (currentGame.FirstPlayerId == 0 || currentGame.SecondPlayerId == 0)
            {
                if (currentGame.FirstPlayerId == 0)
                {
                    currentGame.FirstPlayerId = connectingPlayer.Id;
                    currentGame.OnLine = false;
                }
                else if (currentGame.SecondPlayerId == 0)
                {
                    currentGame.SecondPlayerId = connectingPlayer.Id;
                    currentGame.OnLine = false;
                }

                await _dbContext.SaveChangesAsync();

                return Ok(currentGame);
            }
            else
            {
                return BadRequest("Лобби занято");
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<InGameLogicModel>> ExitTheGame(int playerId, int gameId)
        {
            var currentGame = await _dbContext.InGameLogic
                .FirstAsync(game => game.Id == gameId);

            if (currentGame.FirstPlayerId == playerId)
            {
                currentGame.FirstPlayerId = 0;

                if (currentGame.WinnerId == 0)
                {
                    currentGame.OnLine = true;
                }
            }

            if (currentGame.SecondPlayerId == playerId)
            {
                currentGame.SecondPlayerId = 0;

                if (currentGame.WinnerId == 0)
                {
                    currentGame.OnLine = true;
                }
            }

            if ((currentGame.FirstPlayerId == 0 && currentGame.SecondPlayerId == 0) && !(currentGame.WinnerId == 0))
            {
                _dbContext.InGameLogic.Remove(currentGame);
            }

            await _dbContext.SaveChangesAsync();

            return Ok(currentGame);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<int> GameProgresses(int gameId, string line, string tool)
        {
            var field = GetField(line);

            var firstPlayer = _dbContext.InGameLogic
                .FirstOrDefault(game => game.Id == gameId)?
                .FirstPlayerId;

            var secondPlayer = _dbContext.InGameLogic
                .FirstOrDefault(game => game.Id == gameId)?
                .SecondPlayerId;

            if (!((firstPlayer == null && firstPlayer == 0) && (secondPlayer == null && secondPlayer == 0)))
            {
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    if ((field[i, 0] == tool) && (field[i, 1] == tool) && (field[i, 2] == tool))
                    {
                        return CheckWinner(firstPlayer, secondPlayer, tool);
                    }
                }

                for (int i = 0; i < field.GetLength(0); i++)
                {
                    if ((field[0, i] == tool) && (field[1, i] == tool) && (field[2, i] == tool))
                    {
                        return CheckWinner(firstPlayer, secondPlayer, tool);
                    }
                }

                if ((field[0, 0] == tool) && (field[1, 1] == tool) && (field[2, 2] == tool))
                {
                    return CheckWinner(firstPlayer, secondPlayer, tool);
                }
                else if ((field[0, 2] == tool) && (field[1, 1] == tool) && (field[2, 0] == tool))
                {
                    return CheckWinner(firstPlayer, secondPlayer, tool);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return BadRequest($"Отсутствует Id игрока. Первый игрок ID: {firstPlayer}, Второй игрок: {secondPlayer}");
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<InGameLogicModel>> SettingWinner(int gameId, int winnerId)
        {
            var currentGame = _dbContext.InGameLogic
                .First(game => game.Id == gameId);

            currentGame.WinnerId = winnerId;

            var player = _dbContext.Players
                .First(player => player.Id == winnerId);

            player.Scores += 10;

            await _dbContext.SaveChangesAsync();

            return Ok(currentGame);
        }

        private static string[,] GetField(string line)
        {
            var iterForLine = 0;
            var field = new string[3, 3];

            line = line.Replace(",", string.Empty);

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = line[iterForLine].ToString();
                    iterForLine++;
                }
            }

            return field;
        }

        /// <summary>
        /// The tool parameter takes a string value depending on the player's move. "X" is the first player, "0" (zero) is the second player
        /// </summary>
        private static int CheckWinner(int? firstPlayer, int? secondPlayer, string tool)
        {
            return tool == "X" ? (int)firstPlayer! : (int)secondPlayer!;
        }
    }
}
