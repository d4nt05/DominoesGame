using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domino
{
    public abstract class AIPlayer : Player
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

        virtual protected DominoTile GetBestTile(List<DominoTile> validTiles)
        {
            return null;
        }
    }
}