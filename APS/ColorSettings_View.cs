using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS
{
    class ColorSettings_View : ColorSettings_base
    {
        public ColorSettings_View() : base()
        {
        }

        internal override void ColorSettings_Load(object sender, EventArgs e)
        {
            Startup.myData.bsThird.MoveFirst();
            formHeight = 294;
            wType = "view";
            base.ColorSettings_Load(sender, e);

            lblScheme.Visible = cmbScheme.Visible = true;
            lblEdition.Visible = lblIssue.Visible = lblName.Visible = txbName.Visible =
                lblPages.Visible = btnSave.Visible = cmbEdition.Visible = cmbIssue.Visible = cmbPages.Visible = false;

            lblScheme.Location = new System.Drawing.Point(12, 15);
            cmbScheme.Location = new System.Drawing.Point(104, 12);
            btnClose.Location = new System.Drawing.Point(349, 12);
        }

        internal override void cmbScheme_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbScheme.SelectedIndex != -1)
            {
                base.colors = Startup.myData.GetColorsForSheme((int)cmbScheme.SelectedValue);
            }
            base.pCount_SelectedIndexChanged(sender, e);
        }

    }
}
