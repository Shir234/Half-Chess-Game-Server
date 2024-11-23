using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SERVER.Models;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
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
        public IActionResult StartGame([FromForm] int playerId)
        {
            Console.WriteLine($"PLAYER ID IN START: {playerId}");
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

            Console.WriteLine("END OF START GAME CONTROLLER");
            return Ok(new
            {
                Board = board,
                isPlayerTurn = gameManager.IsPlayerTurn
            });
        }

        // Get: api/Game/board/{playerId}
        [HttpGet("board/{playerId}")]
        public IActionResult GetGameBoard(int playerId)
        {
            Console.WriteLine("GET GAME BOARD CONTROLLER");
            foreach (var game in ActiveGames)
            {
                Console.WriteLine($"Active player ID: {game.Key}");
            }
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
        public IActionResult PutMovePiece(int playerId, [FromBody] MoveData moveData)
        {
            foreach (var game in ActiveGames)
            {
                Console.WriteLine($"Active player ID: {game.Key}");
            }
            if (!ActiveGames.ContainsKey(playerId))
            {
                return NotFound("No active game found for this player.");
            }

            ChessPieceType piece = (ChessPieceType) moveData.piece;
            int fromRow = (int)moveData.fromRow;
            int fromCol = (int)moveData.fromCol;
            int toRow = (int)moveData.toRow;
            int toCol = (int)moveData.toCol;

            var gameManager = ActiveGames[playerId];
            bool isLegal = gameManager.IsMoveLegal(piece, fromRow, fromCol, toRow, toCol);

            if (isLegal)
            {
                gameManager.MovePiece(piece, fromRow, fromCol, toRow, toCol);
            }
            
            return Ok(new { IsLegal = isLegal });
        }


        // Get: api/Game/State/{playerId}
        [HttpGet("State/{playerId}")]
        public IActionResult GetGameState(int playerId)
        {
            if (!ActiveGames.ContainsKey(playerId))
            {
                return NotFound("No active game found for this player.");
            }

            var gameManager = ActiveGames[playerId];
            //var board = JsonConvert.SerializeObject(gameManager.getGameBoard());

            return Ok(new
            {
                IsActive = gameManager.isGameActive,
                IsCheck = gameManager.IsCheck,
                IsPlayerWin = gameManager.IsPlayerWin
            });
        }


        // Get: api/Game/move/server/{playerId}
        [HttpGet("move/server/{playerId}")]
        public IActionResult GetServerMove(int playerId)
        {
            if (!ActiveGames.ContainsKey(playerId))
            {
                return NotFound("No active game found for this player.");
            }

            var gameManager = ActiveGames[playerId];

            // Init server move
            int selectedPieceStartRow, selectedPieceStartCol,
                selectedPieceEndRow, selectedPieceEndCol;
            ChessPieceType selectedPieceType;

            // Get server move
            gameManager.ServerMove(out selectedPieceType,
                out selectedPieceStartRow, out selectedPieceStartCol,
                out selectedPieceEndRow, out selectedPieceEndCol);
            
            // Send server mvoe to client
            return Ok(new
            {
                SelectedPieceType = selectedPieceType,
                SelectedPieceStartRow = selectedPieceStartRow,
                SelectedPieceStartCol = selectedPieceStartCol,
                SelectedPieceEndRow = selectedPieceEndRow,
                SelectedPieceEndCol = selectedPieceEndCol
            });
        }


        // PUT: api/Game/finish/{playerId}
        [HttpPut("finish/{playerId}")]
        public async Task<IActionResult> PutGameOver(int playerId, [FromBody] GameOverRequest request)
        {
            foreach (var game in ActiveGames)
            {
                Console.WriteLine($"Active player ID: {game.Key}");
            }
            if (!ActiveGames.ContainsKey(playerId))
            {
                return NotFound("No active game found for this player.");
            }
            
            // NEED TO DELETE THIS ACTIVE GAME I THINK?
            var gameManager = ActiveGames[playerId];
            // NEED TO SAVE THE GAME TO DB
            await gameManager.EndGameAsync(_gameRepository, request.Result);
            ActiveGames.Remove(playerId);

            return Ok();
        }



        public class MoveData
        {
            public int piece { get; set; }
            public int fromRow { get; set; }
            public int fromCol { get; set; }
            public int toRow { get; set; }
            public int toCol { get; set; }
        }

        public class GameOverRequest
        {
            [Required]
            public GAME_RESULT Result { get; set; }
        }
    }
}
