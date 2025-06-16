using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domino.Players
{
    internal class AIPlayerMedium : AIPlayer
    {
        public AIPlayerMedium(string name, string difficulty) : base(name, difficulty)
        {

        }

        protected override DominoTile GetBestTile(List<DominoTile> validTiles)
        {
            return validTiles.OrderBy(t => t.Left + t.Right).First();
        }
    }
}
