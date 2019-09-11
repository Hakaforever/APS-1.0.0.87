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
    public partial class SettingsChange : Form
    {
        List<Label> labels = new List<Label>();

        public SettingsChange()
        {
            InitializeComponent();
        }

        private void SettingsChange_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Startup.Paths.settingsList.Count; i++)
            {
                Label lb = new Label();
                lb.Text = Startup.Paths.settingsList[i].name;
                lb.Location = new Point(10, 30 + 50 * i);
                lb.Size = new Size(70, 15);
                //lb.Font = new Font(Font.FontFamily Font.Bold);
                this.Controls.Add(lb);

                Label lb1 = new Label();
                lb1.Text = Startup.Paths.settingsList[i].value;
                lb1.Location = new Point(80, 30 + 50 * i);
                lb1.Size = new Size(180, 15);
                lb1.Tag = lb.Text;
                lb.Font = new Font(lb.Font, FontStyle.Bold);
                this.Controls.Add(lb1);

                Button bt = new Button();
                bt.Image = Properties.Resources.Browse_3;
                bt.TextImageRelation = TextImageRelation.ImageBeforeText;
                bt.Text = "";
                bt.Size = new Size(48, 48);
                bt.Location = new Point(260, 10 + 50 * i);
                bt.Tag = lb1;
                bt.Click += new EventHandler(OpenFolder_Click);
                this.Controls.Add(bt);

                labels.Add(lb1);
            }
            this.Height = Startup.Paths.settingsList.Count * 50 + 120;
            this.Location = Startup.Location(this);
        }

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            Label lb = (Label)bt.Tag;
            findFolder.SelectedPath = lb.Text;
            if (findFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                lb.Text = findFolder.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Label l in labels)
            {
                Startup.Paths.ChangeValue(l.Tag.ToString(), l.Text);
            }
            Startup.Paths.WriteINI();
            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
