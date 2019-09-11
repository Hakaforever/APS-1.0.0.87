using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    public partial class OneDayPlane : Form
    {
        MainForm myOwner;

        public OneDayPlane()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OneDayPlane_Load(object sender, EventArgs e)
        {
            myOwner = this.Owner as MainForm;
            dtPicker.Value = DateTime.Now.AddDays(1);

            Startup.myData.bsMain.DataMember = "Editions";
            cmbEdition.DataSource = Startup.myData.bsMain;
            cmbEdition.ValueMember = "id";
            cmbEdition.DisplayMember = "name";


        }
    }
}
