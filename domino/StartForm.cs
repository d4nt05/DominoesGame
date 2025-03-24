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
    public partial class StartForm: Form
    {
        public StartForm()
        {
            InitializeComponent();
            rb2Players.Checked = true;
            cbDifficulty.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int playerCount = 0;
            if (rb2Players.Checked) playerCount = 2;
            else if (rb3Players.Checked) playerCount = 3;
            else if (rb4Players.Checked) playerCount = 4;

            string difficulty =cbDifficulty.SelectedItem.ToString();

            if (playerCount == 0)
            {
                MessageBox.Show("Выберите количество игроков!");
                return;
            }

            var gameForm = new GameForm(playerCount, difficulty);
            gameForm.Show();
            this.Hide();
        }
    }
}
