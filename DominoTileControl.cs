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
    public partial class DominoTileControl: UserControl
    {
        public DominoTile Tile { get; set; }
        public bool IsAI { get; set; }
        public bool IsRotated { get; set; }

        public event EventHandler TileClicked;

        public DominoTileControl(DominoTile tile)
        {
            InitializeComponent();
            this.Size = new Size(60, 120);
            Tile = tile;
            UpdateAppearance();
        }
        private void UpdateAppearance()
        {
            if (IsAI)
            {
                this.pictureBox1.Image = Properties.Resources.tile_0_0;
            }
            else
            {
                this.pictureBox1.Image = GetTileImage(Tile);
            }

            if (IsRotated)
            {
                this.pictureBox1.Image = RotateImage(this.pictureBox1.Image, RotateFlipType.Rotate90FlipNone);
            }
        }

        private Image RotateImage(Image image, RotateFlipType rotateType)
        {
            var bmp = new Bitmap(image);
            bmp.RotateFlip(rotateType);
            return bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TileClicked?.Invoke(this, EventArgs.Empty);
        }

        private Image GetTileImage(DominoTile tile)
        {
            string imageName = $"tile_{tile.Left}_{tile.Right}";
            return Properties.Resources.ResourceManager.GetObject(imageName) as Image;
        }
    }
}
