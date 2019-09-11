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
    public partial class SendPage : Form
    {
        internal int initialIssueID;

        List<CheckBox> checkList = new List<CheckBox>();
        List<ComboBox> comboList = new List<ComboBox>();
        List<CheckBox> colorList = new List<CheckBox>();
        DrawPlane.SendPageInfo sendPageInfo;

        internal SendPage(ref DrawPlane.SendPageInfo sendPageInfo)
        {
            InitializeComponent();
            this.sendPageInfo = sendPageInfo;
        }

        private void SendPage_Load(object sender, EventArgs e)
        {
            int step = 40;

            for (int y = 0; y < Startup.issuesList.Count; y++)//pageInfo.targetPageInfo.Count; y++)
            {
                CheckBox cbx = new CheckBox();
                cbx.Text = Startup.issuesList[y].Name;//pageInfo.targetPageInfo[y].issueName;
                cbx.Tag = Startup.issuesList[y].Id;
                cbx.Location = new Point(15, 20 + y * step);
                cbx.Click += new EventHandler(CheckChange);
                cbx.CheckStateChanged += new EventHandler(CheckStateChange);

                ComboBox pNum = new ComboBox();
                pNum.Location = new Point(135, 20 + y * step);
                pNum.Enabled = false;
                pNum.Tag = Startup.issuesList[y].Id;
                for (int j = 1; j <= Startup.issuesList[y].Pages; j++)
                    pNum.Items.Add(j.ToString());
                pNum.DropDownStyle = ComboBoxStyle.DropDownList;
                try
                {
                    pNum.SelectedIndex = Convert.ToInt32(sendPageInfo.pagesInfo.Where(r => r.issueId == Startup.issuesList[y].Id).Select(item => item.pageNum).FirstOrDefault()) - 1;
                }
                catch { pNum.SelectedIndex = -1; }
                pNum.SelectedIndexChanged += new EventHandler(PageChanged);
                pNum.Size = new Size(60, 80);

                CheckBox cbColor = new CheckBox();
                cbColor.Size = new System.Drawing.Size(30, 30);
                cbColor.Appearance = Appearance.Button;
                cbColor.Size = new Size(35, 35);
                cbColor.ImageList = this.colorsIcon;
                cbColor.ImageIndex = Convert.ToInt32(sendPageInfo.pagesInfo.Where(r => r.issueId == Startup.issuesList[y].Id).Select(item => item.isColor).FirstOrDefault());
                cbColor.Location = new Point(215, 12 + y * step);
                cbColor.Click += new EventHandler(StateChange);
                cbColor.Tag = Startup.issuesList[y].Id;
                cbColor.Enabled = false;
                this.Controls.Add(cbx);
                this.Controls.Add(pNum);
                this.Controls.Add(cbColor);

                checkList.Add(cbx);
                comboList.Add(pNum);
                colorList.Add(cbColor);

                if (Startup.issuesList[y].Id == sendPageInfo.initialIssueId)
                    cbx.Checked = true;
                else
                    cbx.Checked = false;
            }
            this.Size = new Size(300, Startup.issuesList.Count * step + 125);
            //this.Location = Startup.Location(this);
        }

        private void CheckStateChange(object sender, EventArgs e)
        {
            CheckChange(sender, e);
        }

        private void CheckChange(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            int index = checkList.FindIndex(p => p == cb);
            if (index != -1)
            {
                comboList[index].Enabled = cb.Checked;
                colorList[index].Enabled = cb.Checked;
            }

            index = sendPageInfo.pagesInfo.FindIndex(p => p.issueId == (int)cb.Tag);

            DrawPlane.SendPageInfo.pInfo item = sendPageInfo.pagesInfo[index];

            item.isChecked = cb.Checked;
            sendPageInfo.pagesInfo[index] = item;
        }

        private void PageChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            int index = sendPageInfo.pagesInfo.FindIndex(p => p.issueId == (int)cb.Tag);

            DrawPlane.SendPageInfo.pInfo item = sendPageInfo.pagesInfo[index];

            item.pageNum = Convert.ToInt32(cb.SelectedItem);
            sendPageInfo.pagesInfo[index] = item;
        }

        private void StateChange(object sender, EventArgs e)
        //изменение статуса (цветности) кнопки-страницы
        {
            CheckBox cb = (CheckBox)sender;

            cb.ImageIndex = (cb.ImageIndex == 1 ? 0 : 1);
            cb.CheckState = CheckState.Unchecked;

            int index = sendPageInfo.pagesInfo.FindIndex(p => p.issueId == (int)cb.Tag);

            DrawPlane.SendPageInfo.pInfo item = sendPageInfo.pagesInfo[index];

            item.isColor = cb.ImageIndex;
            sendPageInfo.pagesInfo[index] = item;
        }

        private void btnSend_Click(object sender, EventArgs e)
        //сохранение 
        {
            sendPageInfo.Close();
            comboList.Clear();
            checkList.Clear();
            colorList.Clear();
            this.Close();
        }
    }
}
