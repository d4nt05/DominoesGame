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
    public partial class DominoTileControlAI: DominoTileControl
    {
        public DominoTileControlAI(DominoTile tile, bool isRotated) : base(tile) 
        {
            IsAI = true;
            IsRotated = isRotated;
            UpdateAppearance();
        }
        protected override void UpdateAppearance()
        {
            this.PictureBox1.Image = Properties.Resources.tile_0_0;

            if (!IsRotated)
            {
                var rotatedImage = RotateImage(this.pictureBox1.Image, RotateFlipType.Rotate90FlipNone);
                this.PictureBox1.Image = rotatedImage;
                this.Size = new Size(60, 120);
            }
            else this.Size = new Size(120, 60);
        }
    }
}
