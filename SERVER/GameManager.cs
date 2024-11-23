using Microsoft.VisualBasic;
using SERVER.Models;

namespace SERVER
{
    public class GameManager
    {

        // Enum to identify game pieces
        public enum ChessPieceType { None = 0, WKing, BKing, WBishop, BBishop, WKnight, BKnight, WCastle, BCastle, WPawn, BPawn };
        public enum GAME_RESULT { Win = 1, Tie, Lose = 0 };

        // FROM CONSTANCE - SAVE? DELETE?
        public static readonly int BoardLength = 8;
        public static readonly int BoardWidth = 4;
        public static readonly int BoardSquareSize = 90;
        public static readonly int BoardChessPieceSize = 80;
        public static readonly int BoardMargin = 20;

        // Variables to manage the game
        public bool isGameActive;                              // is the current game active ? 
        public bool IsPlayerWin { get; set; }
        public bool isPlayerTurn { get; set; }                              // is the user or server turn



        private ChessPieceType[,] gameBoard;                    // Matrix - represent the game board
       
        private int WKingLocationX, BKingLocationX;
        private int WKingLocationY, BKingLocationY;             // King location on the board
        private int boardLength = BoardLength, boardWidth = BoardWidth;



        ////// NEW HERE //////////////////////////////////////////////////////////////
        private readonly GameRepository _gameRepository;
        public TblGames Game { get; set; } = default!;
        public int Timer { get; set; }
        public bool IsCheck { get; set; }

        ////// END NEW //////////////////////////////////////////////////////////////


        // Constructor - activate the game, set the game board
        public GameManager(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            Console.WriteLine("END OF MANAGER C'TOR");
        }

        public void StartGame(int playerId)
        {
            Game = new TblGames();
            Game.PlayerId = playerId;
            Game.Date = DateOnly.FromDateTime(DateTime.Today);
            Game.StartTime = TimeOnly.FromDateTime(DateTime.Now);

            this.isGameActive = true;
            this.isPlayerTurn = true;
            setGameBoard();

            Console.WriteLine("END OF MANAGER START GAME");
        }

        public void SetPlayerTurn()
        {
            isPlayerTurn = !isPlayerTurn;
        }



        // Initiate a matrix in the game board starting position
        private void setGameBoard()
        {
            this.gameBoard = new ChessPieceType[,]
            { { ChessPieceType.BKing, ChessPieceType.BKnight, ChessPieceType.BBishop, ChessPieceType.BCastle},
                { ChessPieceType.BPawn, ChessPieceType.BPawn, ChessPieceType.BPawn, ChessPieceType.BPawn},
                { 0, 0, 0, 0},
                { 0, 0, 0, 0},
                { 0, 0, 0, 0},
                { 0, 0, 0, 0},
                { ChessPieceType.WPawn, ChessPieceType.WPawn, ChessPieceType.WPawn, ChessPieceType.WPawn},
                { ChessPieceType.WKing, ChessPieceType.WKnight, ChessPieceType.WBishop, ChessPieceType.WCastle} };
        }

        // Get the current game board (with the current pieces positions)
        public ChessPieceType[,] getGameBoard()
        {
            return gameBoard;
        }

        /// Check if a move is legal
        public bool IsMoveLegal(ChessPieceType piece, int fromRow, int fromCol, int toRow, int toCol)
        {
            if (toRow < 0 || toRow >= boardLength || toCol < 0 || toCol >= boardWidth)                  // check the board borders
                return false;

            if (gameBoard[toRow, toCol] != ChessPieceType.None)                                         // check if a move isn't on player own game pieces             
                if (IsSameColor(piece, gameBoard[toRow, toCol]))
                    return false;
            
            switch (piece)                                                                              // check all other cases
            {
                case ChessPieceType.WPawn:
                case ChessPieceType.BPawn:
                    return IsValidPawnMove(piece, fromRow, fromCol, toRow, toCol);
                case ChessPieceType.WKing:
                case ChessPieceType.BKing:
                    return IsValidKingMove(fromRow, fromCol, toRow, toCol);
                case ChessPieceType.WKnight:
                case ChessPieceType.BKnight:
                    return IsValidKnightMove(fromRow, fromCol, toRow, toCol);
                case ChessPieceType.WBishop:
                case ChessPieceType.BBishop:
                    return IsValidBishopMove(fromRow, fromCol, toRow, toCol);
                case ChessPieceType.WCastle:
                case ChessPieceType.BCastle:
                    return IsValidCastleMove(fromRow, fromCol, toRow, toCol);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if the two pieces are of the same color.
        /// </summary>
        /// <param name="piece">The first piece</param>
        /// <param name="targetPiece">The second piece</param>
        /// <returns></returns>
        public bool IsSameColor(ChessPieceType piece, ChessPieceType targetPiece)
        {
            return (piece == ChessPieceType.WPawn || piece == ChessPieceType.WKing || piece == ChessPieceType.WKnight || piece == ChessPieceType.WBishop || piece == ChessPieceType.WCastle) &&
                   (targetPiece == ChessPieceType.WPawn || targetPiece == ChessPieceType.WKing || targetPiece == ChessPieceType.WKnight || targetPiece == ChessPieceType.WBishop || targetPiece == ChessPieceType.WCastle) ||
                   (piece == ChessPieceType.BPawn || piece == ChessPieceType.BKing || piece == ChessPieceType.BKnight || piece == ChessPieceType.BBishop || piece == ChessPieceType.BCastle) &&
                   (targetPiece == ChessPieceType.BPawn || targetPiece == ChessPieceType.BKing || targetPiece == ChessPieceType.BKnight || targetPiece == ChessPieceType.BBishop || targetPiece == ChessPieceType.BCastle);
        }

        /// <summary>
        /// Determine if the pawn move is valid.
        /// </summary>
        /// <param name="pawn">The pawn piece</param>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        /// <returns></returns>
        public bool IsValidPawnMove(ChessPieceType pawn, int fromRow, int fromCol, int toRow, int toCol)
        {
            int direction = (pawn == ChessPieceType.WPawn) ? -1 : 1;
            if (fromCol == toCol)
            {
                if (toRow == fromRow + direction && gameBoard[toRow, toCol] == ChessPieceType.None)
                    return true;
                if ((pawn == ChessPieceType.WPawn && fromRow == 6 || pawn == ChessPieceType.BPawn && fromRow == 1) &&
                    toRow == fromRow + (2 * direction) && gameBoard[toRow, toCol] == ChessPieceType.None)
                    return true;
            }
            if (Math.Abs(fromCol - toCol) == 1 && toRow == fromRow + direction)
            {
                if (gameBoard[toRow, toCol] != ChessPieceType.None)                                     // For capturing diagonally
                    return true;
            }
            if (Math.Abs(fromRow - toRow) == 0 && Math.Abs(fromCol - toCol) == 1)
            {                                                                                           // Move left or right (without capturing)
                if (gameBoard[toRow, toCol] == ChessPieceType.None)                                     // Ensure the destination square is empty
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determine if the king move is valid.
        /// </summary>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        /// <returns></returns>
        public bool IsValidKingMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            return Math.Abs(fromRow - toRow) <= 1 && Math.Abs(fromCol - toCol) <= 1;
        }

        /// <summary>
        /// Determine if the knight move is valid.
        /// </summary>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        /// <returns></returns>
        public bool IsValidKnightMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            return Math.Abs(fromRow - toRow) == 2 && Math.Abs(fromCol - toCol) == 1 ||
                   Math.Abs(fromRow - toRow) == 1 && Math.Abs(fromCol - toCol) == 2;
        }

        /// <summary>
        /// Determine if the bishop move is valid.
        /// </summary>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        /// <returns></returns>
        public bool IsValidBishopMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            return Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol);
        }

        /// <summary>
        /// Determine if the castle move is valid.
        /// </summary>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        /// <returns></returns>
        public bool IsValidCastleMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            return fromRow == toRow || fromCol == toCol;
        }



        // Move Piece --> set it in the new location and empty the last location on board
        /// <summary>
        /// Move the piece on the board
        /// </summary>
        /// <param name="piece">The piece to move</param>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        public void MovePiece(ChessPieceType piece, int fromRow, int fromCol, int toRow, int toCol)
        {
            gameBoard[toRow, toCol] = piece;
            gameBoard[fromRow, fromCol] = ChessPieceType.None;

            CheckGameState();                                       // After every move, check if the game is in check or checkmate 
            SetPlayerTurn();                                        // and switch turn
        }

        /// <summary>
        /// Determine the current game state
        /// </summary>
        public void CheckGameState()
        {
            if (IsInCheck(ChessPieceType.WKing))                    // Check if white King is in check
            {
                IsCheck = true;
                Console.WriteLine("White King is in check.");
                if (IsCheckmate(ChessPieceType.WKing))
                {
                    Console.WriteLine("Checkmate! Black wins.");
                    isGameActive = false; // End game
                    IsPlayerWin = false;
    }
            }
            else if (IsInCheck(ChessPieceType.BKing))                    // Check if black King is in check
            {
                IsCheck = true;
                Console.WriteLine("Black King is in check.");
                if (IsCheckmate(ChessPieceType.BKing))
                {
                    Console.WriteLine("Checkmate! White wins.");
                    isGameActive = false; // End game
                    IsPlayerWin = true;
                }
            }
            else
            {
                IsCheck = false;
            }
        }

        /// <summary>
        /// Determine if there is a check.
        /// </summary>
        /// <param name="playerKing">The king piece</param>
        /// <param name="kingRow">The king piece row. default -1 if unknown</param>
        /// <param name="kingCol">The king piece col. default -1 if unknown</param>
        /// <returns></returns>
        public bool IsInCheck(ChessPieceType playerKing, int kingRow = -1, int kingCol = -1)
        {
            if (kingRow == -1 || kingCol == -1)
            {
                FindTheKingPiece(playerKing, ref kingRow, ref kingCol);                                 // Find the King's location
            }
            if (kingRow == -1) 
                return false;                                                                           // No King found, invalid scenario
            Console.WriteLine("King at [" + kingRow + ", " + kingCol + "]");

            // Check if any opposing pieces can move to the King's position
            for (int opposingPieceRow = 0; opposingPieceRow < boardLength; opposingPieceRow++)
            {
                for (int opposingPieceCol = 0; opposingPieceCol < boardWidth; opposingPieceCol++)
                {
                    ChessPieceType piece = gameBoard[opposingPieceRow, opposingPieceCol];
                    if (piece == ChessPieceType.None) continue;                             // Skip empty squares
                    if (IsSameColor(playerKing, piece)) continue;                           // Skip pieces of the same color

                    // Check if this piece can legally attack the King's position
                    if (IsMoveLegal(piece, opposingPieceRow, opposingPieceCol, kingRow, kingCol))
                    {
                        Console.WriteLine("King at [" + kingRow + ", " + kingCol + "]");
                        Console.WriteLine(piece.ToString() + " at [" + opposingPieceRow + ", " + opposingPieceCol + "] threatens " + playerKing.ToString());
                        return true;    // King is in check
                    }
                }
            }
            return false;               // King is not in check
        }

        /// <summary>
        /// Determine if there is a checkmate
        /// </summary>
        /// <param name="playerKing">The king piece</param>
        /// <returns></returns>
        public bool IsCheckmate(ChessPieceType playerKing)
        {
            int kingRow = -1, kingCol = -1;
            FindTheKingPiece(playerKing, ref kingRow, ref kingCol);                                  // Find the location of the King
            Console.WriteLine("King at [" + kingRow + ", " + kingCol + "]");

            if (!IsInCheck(playerKing, kingRow, kingCol))                                            // If the King is not in check, it's not checkmate
            {
                return false;
            }

            for (int row = kingRow - 1; row <= kingRow + 1; row++)                                   // Check if the King has any legal moves to escape the check
            {
                for (int col = kingCol - 1; col <= kingCol + 1; col++)
                {
                    if (row < 0 || row >= boardLength)
                    {
                        continue;
                    }
                    if (col < 0 || col >= boardWidth)
                    {
                        continue;
                    }
                    // Skip the King's current position
                    if (row == kingRow && col == kingCol)
                        continue;

                    ChessPieceType targetPiece = gameBoard[row, col];

                    // Check if the square is either empty or contains an opponent's piece that can be captured
                    if (IsMoveLegal(playerKing, kingRow, kingCol, row, col) && !IsSameColor(playerKing, targetPiece))
                    {
                        // Simulate moving the King to see if it would still be in check
                        ChessPieceType originalPiece = gameBoard[row, col];
                        gameBoard[row, col] = playerKing;
                        gameBoard[kingRow, kingCol] = ChessPieceType.None;

                        if (!IsInCheck(playerKing)) // King is not in check after move
                        {
                            gameBoard[row, col] = originalPiece;
                            gameBoard[kingRow, kingCol] = playerKing;
                            return false; // Not checkmate, King can escape
                        }

                        // Restore the board state
                        gameBoard[row, col] = originalPiece;
                        gameBoard[kingRow, kingCol] = playerKing;
                    }
                }
            }
            // If no legal move can escape the check, it's checkmate
            return true;
        }

        /// <summary>
        /// Search for the king piece
        /// </summary>
        /// <param name="playerKing"></param>
        /// <param name="kingRow"></param>
        /// <param name="kingCol"></param>
        private void FindTheKingPiece(ChessPieceType playerKing, ref int kingRow, ref int kingCol)
        {
            for (int row = 0; row < boardLength; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    if (gameBoard[row, col] == playerKing)
                    {
                        kingRow = row;
                        kingCol = col;
                        break;
                    }
                }
                if (kingRow != -1) break;  // Exit if King is found
            }
        }


        /////////////////////////////////////////// NEW - END GAME ---> ADD TO DB ///////////////////////
        public async Task EndGameAsync(GameRepository _gameRepository, GAME_RESULT result)
        {
            Console.WriteLine("ENDING GAME BEFORE SAVE:");
            var endTime = TimeOnly.FromDateTime(DateTime.Now);
            Game.Duration = (int)Math.Round((endTime - Game.StartTime).TotalMinutes);
            Game.Result = (int)result;

            Console.WriteLine($"START TIME: {Game.StartTime}");
            Console.WriteLine($"end TIME: {endTime}");
            Console.WriteLine($"DURATION: {endTime - Game.StartTime}");
            Console.WriteLine($"DURATION TO MIN: {(endTime - Game.StartTime).TotalMinutes}");
            // Save the game to the database
            await _gameRepository.SaveGameAsync(Game);
        }
    }
}
