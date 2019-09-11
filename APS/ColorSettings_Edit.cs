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
    public partial class ColorSettings_Edit : ColorSettings_base
    //изменение цветовой схемы 
    {
        public ColorSettings_Edit() : base()
        {
        }

        internal override void ColorSettings_Load(object sender, EventArgs e)
        {
            Startup.myData.bsThird.MoveFirst();

            base.formHeight = 366;
            base.ColorSettings_Load(sender, e);
            base.lblScheme.Visible = true;
            base.cmbScheme.Visible = true;
        }

        internal override void cmbScheme_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbScheme.SelectedIndex != -1)
            {
                base.colors = Startup.myData.GetColorsForSheme((int)cmbScheme.SelectedValue);
            }
        }

        internal override void cmbPages_DropDownClosed(object sender, EventArgs e)
        { 
            if (cmbPages.SelectedIndex != -1)
            {
                base.colors = new int[Convert.ToInt16(cmbPages.Text)];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = 1;
                }
            }
        }

        internal override void btnSave_Click(object sender, EventArgs e)
        {
            base.btnSave_Click(sender, e);
            info_Text("Схема " + txbName.Text + " изменена.");
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
