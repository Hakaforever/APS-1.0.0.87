using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    public partial class ColorSettings_New : ColorSettings_base
    {
        public ColorSettings_New() : base()
        {
           
        }

        internal override void ColorSettings_Load(object sender, EventArgs e)
        {
            base.formHeight = 337;
            base.ColorSettings_Load(sender, e);
            //cmbEdition.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsMain, "edition_id", true));
            Startup.myData.AddNew(Startup.myData.bsThird);
            cmbPages.SelectedIndex = -1;
            cmbIssue.SelectedIndex = -1;
            cmbEdition.SelectedIndex = -1;
            txbName.Text = "";
            colors = null;
            panelClear();
        }

        internal override void pCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPages.SelectedIndex != -1)
            {
                colors = new int[Convert.ToInt16(cmbPages.Text)];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = 1;
                base.pCount_SelectedIndexChanged(sender, e);
            }
        }

        internal override void btnSave_Click(object sender, EventArgs e)
        {
            base.btnSave_Click(sender, e);
            info_Text("Схема " + txbName.Text + " создана.");

            Startup.myData.AddNew(Startup.myData.bsThird);
            cmbPages.SelectedIndex = -1;
            cmbIssue.SelectedIndex = -1;
            cmbEdition.SelectedIndex = -1;
            base.panelClear();
            txbName.Text = "";

            System.Media.SystemSounds.Beep.Play();
        }

    }
}
