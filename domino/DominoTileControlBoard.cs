using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public partial class DominoTileControlBoard: DominoTileControl
    {
        public DominoTileControlBoard(DominoTile tile) : base(tile)
        {
            this.pictureBox1.Click -= pictureBox1_Click;
        }

        protected override void UpdateAppearance()
        {
            this.pictureBox1.Image = GetTileImage(Tile);
            this.Size = new Size(60, 30);
        }
    }
}
