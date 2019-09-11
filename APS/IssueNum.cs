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
    public partial class IssueNum : Form
    {
        int diff = 0;
        int prevValue = -1;

        public IssueNum()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Startup.issueBigNum = (int)bNumUpDown.Value;
            Startup.issueNum = (int)sNumUpDown.Value;
            Startup.myData.SaveIssueNum();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void IssueNum_Load(object sender, EventArgs e)
        {
            if (Startup.issueNum == -1)
            {
                Startup.myData.GetNewIssueNum();
            }
            sNumUpDown.Value = Startup.issueNum + 1;
            bNumUpDown.Value = Startup.issueBigNum + 1;
            sNumUpDown.Minimum = sNumUpDown.Value;
            bNumUpDown.Minimum = sNumUpDown.Value;
            diff = Startup.issueBigNum - Startup.issueNum;
            prevValue = (int)sNumUpDown.Value;
        }

        private void sNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            bNumUpDown.Value = sNumUpDown.Value + diff;
        }
    }
}
