using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    class InputTextWin : SelectFromCombo
    {
        public TextBox namePlane = new TextBox();

        public InputTextWin(int inIssueId) : base(inIssueId)
        {
        }

        public override void ViewPlane_Load(object sender, EventArgs e)
        {
            base.ViewPlane_Load(sender, e);
            cmbSelect.Visible = false;
            namePlane.Size = new System.Drawing.Size(220, 21);
            namePlane.Location = new System.Drawing.Point(12, 19);
            namePlane.KeyPress += new KeyPressEventHandler(InputText);

            btnSave.Image = Properties.Resources.Save;
            btnSave.Text = "Сохранить";

            Controls.Add(namePlane);
            namePlane.Select();
        }

        private void InputText(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Text != "")
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        
    }
}
