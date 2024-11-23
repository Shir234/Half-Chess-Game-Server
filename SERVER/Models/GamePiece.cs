using static SERVER.GameManager;

namespace SERVER.Models
{
    public class GamePiece
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public ChessPieceType PieceType { get; set; }


    }
}
