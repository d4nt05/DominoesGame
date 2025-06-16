using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domino.Players
{
    internal class AIPlayerEasy : AIPlayer
    {
        public AIPlayerEasy(string name, string difficulty) : base(name, difficulty) {

        }

        protected override DominoTile GetBestTile(List<DominoTile> validTiles) {
            return validTiles[new Random().Next(validTiles.Count)];
        }
    }
}
