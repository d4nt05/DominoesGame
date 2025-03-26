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
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var startForm = new StartForm();
            startForm.Show();
            this.Hide();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openSave = new OpenFileDialog();
            openSave.Filter = "Domino game save|*.domino";
            openSave.Title = "Открыть сохраненную игру";
            openSave.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
