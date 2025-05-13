using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domino
{
    public class AIPlayer : Player
    {
        public readonly string _difficulty;

        public AIPlayer(string name, string difficulty) : base(name)
        {
            _difficulty = difficulty;
        }
        
        public DominoTile ChooseTile(List<DominoTile> stock, List<DominoTile> board)
        {
            var validTiles = GetValidTiles(board);
            if (validTiles.Any())
            {
                return GetBestTile(validTiles);
            }
            while (stock.Any())
            {
                Random rnd = new Random();
                int index = rnd.Next(stock.Count);
                DominoTile takenTile = stock[index];
                stock.RemoveAt(index);
                Hand.Add(takenTile);
                validTiles = GetValidTiles(board);
                if (validTiles.Any())
                {
                    return GetBestTile(validTiles);
                }
            }
            return null;
        }

        private DominoTile GetBestTile(List<DominoTile> validTiles)
        {
            if (_difficulty == "Легкий")
            {
                return validTiles[new Random().Next(validTiles.Count)];
            }
            else if (_difficulty == "Средний")
            {
                return validTiles.OrderBy(t => t.Left + t.Right).First();
            }
            else if (_difficulty == "Сложный")
            {
                return validTiles.OrderByDescending(t => t.Left + t.Right).First();
            }
            return validTiles.First();
        }
    }
}
