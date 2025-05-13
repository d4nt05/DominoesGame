using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public partial class main: Form
    {
        public main()
        {
            InitializeComponent();
            this.FormClosing += main_FormClosing;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var startForm = new StartForm();
            startForm.Show();
            this.Hide();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Domino save files|*.domino",
                Title = "Открыть сохраненную игру"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var saveData = SaveLoadManager.LoadGame(openFileDialog.FileName);
                if (saveData != null)
                {
                    var gameForm = new GameForm(openFileDialog.FileName);
                    gameForm.LoadGameData(openFileDialog.FileName);
                    gameForm.Show();
                    this.Hide();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }
}
