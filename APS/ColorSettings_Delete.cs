using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    class ColorSettings_Delete : SelectFromCombo
    //изменение цветовой схемы 
    {
        public ColorSettings_Delete() : base(-1)
        {

        }

        public override void ViewPlane_Load(object sender, EventArgs e)
        {
            Startup.myData.bsMain.Filter = null;
            Startup.myData.bsMain.DataMember = "Colors_schemes";
            
            cmbSelect.DataSource = Startup.myData.bsMain;
            cmbSelect.DisplayMember = "name";
            cmbSelect.ValueMember = "id";
            cmbSelect.SelectedIndex = -1;

            btnSave.Image = Properties.Resources.Recycle_Bin_Empty_2;
            btnSave.Text = "Удалить";
            btnCancel.Image = Properties.Resources.Back_2;
            btnCancel.Text = "Вернуться";

            this.DialogResult = System.Windows.Forms.DialogResult.None;
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbSelect.SelectedIndex != -1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить настройки цветности \"" + cmbSelect.Text + "\"?", "Внимание!", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;
                int index = (int)cmbSelect.SelectedValue;
                Startup.myData.EndEdit();
                Startup.myData.bsThird.RemoveCurrent();
                Startup.myData.RemoveColorsRow(index, 0);
                Startup.myData.taColorSettings.Update(Startup.myData.tblColorSettings);
                
                System.Media.SystemSounds.Beep.Play();
            }
        }

    
    }
}
