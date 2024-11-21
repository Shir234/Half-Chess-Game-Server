using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SERVER.Models;
using System.Collections.Concurrent;
using static SERVER.GameManager;

namespace SERVER.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameRepository _gameRepository;
        private static Dictionary<int, GameManager> ActiveGames = new Dictionary<int, GameManager>(); // Active games

        public GameController(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // Start a new game
        // POST: api/Game/start
        [HttpPost("start")]
        public IActionResult StartGame(int playerId)
        {
            // Check if the player already has an active game
            if (ActiveGames.ContainsKey(playerId))
            {
                return BadRequest("A game is already active for this player.");
            }
            // Create a new game
            var gameManager = new GameManager(_gameRepository);
            ActiveGames[playerId] = gameManager;
            gameManager.StartGame(playerId);

            var board = JsonConvert.SerializeObject(gameManager.getGameBoard());

            return Ok(new
            {
                Board = board,
                isPlayerTurn = gameManager.isPlayerTurn
            });
        }

        // Get: api/Game/board/{playerId}
        [HttpGet("board/{playerId}")]
        public IActionResult GetGameBoard(int playerId)
        {
            if(!ActiveGames.ContainsKey(playerId))
            {
                return NotFound("No active game found for this player.");
            }

            var gameManager = ActiveGames[playerId];
            var board = JsonConvert.SerializeObject(gameManager.getGameBoard());
            
            return Ok(new
            {
                Board = board,
            });
        }

        // PUT: api/Game/move/check/{playerId}
        [HttpPut("move/check/{playerId}")]
        public IActionResult PutMovePiece(int playerId, dynamic moveData)
        {
            if (!ActiveGames.ContainsKey(playerId))
            {
                return NotFound("No active game found for this player.");
            }
            var gameManager = ActiveGames[playerId];
            bool isLegal = gameManager.IsMoveLegal(
                (ChessPieceType)(int)moveData.piece,
                (int)moveData.fromRow,
                (int)moveData.fromCol,
                (int)moveData.toRow,
                (int)moveData.toCol
            );

            if(isLegal)
            {
                gameManager.MovePiece(
                    (ChessPieceType)(int)moveData.piece,
                    (int)moveData.fromRow,
                    (int)moveData.fromCol,
                    (int)moveData.toRow,
                    (int)moveData.toCol
                );
            }

            return Ok(new { IsLegal = isLegal });
        }
    }
}
