using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domino.Players
{
    internal class AIPlayerHard : AIPlayer
    {
        public AIPlayerHard(string name, string difficulty) : base(name, difficulty)
        {

        }

        protected override DominoTile GetBestTile(List<DominoTile> validTiles)
        {
            return validTiles.OrderByDescending(t => t.Left + t.Right).First();
        }
    }
}
