using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domino
{
    public class ChooseDialog : Form
    {
        public ChooseDialog()
        {
            Button OkButton = new Button();
            OkButton.Text = "Слева";
            OkButton.DialogResult = DialogResult.OK;
            OkButton.Location = new Point(8, 20);
            OkButton.Size = new Size(50, 24);
            this.Controls.Add(OkButton);

            Button CancelButton = new Button();
            CancelButton.Text = "Справа";
            CancelButton.DialogResult = DialogResult.Cancel;
            CancelButton.Location = new Point(90, 20);
            CancelButton.Size = new Size(55, 24);
            this.Controls.Add(CancelButton);

            this.Text = "Куда поместить фишку?";
            this.Size = new Size(170, 90);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ControlBox = false;
        }
    }
}
