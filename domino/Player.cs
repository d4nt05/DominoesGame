using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace domino
{
    public abstract class Player
    {
        public string Name { get; set; }
        public List<DominoTile> Hand { get; set; } = new List<DominoTile>();

        public Player(string name)
        {
            Name = name;
        }

        public List<DominoTile> GetValidTiles(List<DominoTile> board)
        {
            if (board.Count == 0) return Hand;

            int leftEnd = board.First().Left; // Левый конец левой фишки
            int rightEnd = board.Last().Right; // Правый конец правой фишки

            return Hand.Where(t =>
                t.Left == leftEnd ||
                t.Right == leftEnd ||
                t.Left == rightEnd ||
                t.Right == rightEnd
            ).ToList();
        }
    }
}
