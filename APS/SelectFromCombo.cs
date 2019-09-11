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
    public partial class SelectFromCombo : Form
    {
        protected int issueId;

        public SelectFromCombo(int inIssueId)
        {
            InitializeComponent();
            issueId = inIssueId;
        }

        public virtual void ViewPlane_Load(object sender, EventArgs e)
        {
            Startup.myData.bsMain.DataMember = "Templates";
            Startup.myData.bsMain.Filter = "daytoday = false and issue_id = " + issueId.ToString();

            cmbSelect.DataSource = Startup.myData.bsMain;
            cmbSelect.DisplayMember = "name";
            cmbSelect.ValueMember = "id";
            cmbSelect.SelectedIndex = -1;
        }

        public virtual void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelect.SelectedIndex == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }
    }
}
