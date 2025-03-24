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
    public partial class DominoTileControl : UserControl
    {
        public DominoTile Tile { get; set; }
        public bool IsAI { get; set; }
        public bool IsRotated { get; set; }
        public event EventHandler TileClicked;
        public bool board { get; set; }  

        public PictureBox PictureBox1 => pictureBox1;

        public DominoTileControl(DominoTile tile)
        {
            InitializeComponent();
            this.Size = new Size(120, 60);
            this.BorderStyle = BorderStyle.None;
            Tile = tile;
            UpdateAppearance();
        }

        protected virtual void UpdateAppearance()
        {
            this.pictureBox1.Image = GetTileImage(Tile);
            this.PictureBox1.Image = RotateImage(this.pictureBox1.Image, RotateFlipType.Rotate90FlipNone);
            this.Size = new Size(60, 120);
        }

        public Image RotateImage(Image image, RotateFlipType rotateType)
        {
            var bmp = new Bitmap(image);
            bmp.RotateFlip(rotateType);
            return bmp;
        }

        protected void pictureBox1_Click(object sender, EventArgs e)
        {
            TileClicked?.Invoke(this, EventArgs.Empty);
        }

        protected Image GetTileImage(DominoTile tile)
        {
            string imageName = $"tile_{tile.Left}_{tile.Right}";
            var image = Properties.Resources.ResourceManager.GetObject(imageName) as Image;
            if (image == null)
            {
                Console.WriteLine($"Изображение {imageName} не найдено");
                // Возвращаем тестовое изображение, если исходное не найдено
                var bmp = new Bitmap(120, 60);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Red);
                }
                return bmp;
            }
            return image;
        }
    }
}