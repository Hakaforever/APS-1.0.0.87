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
    public partial class ViewDB : Form
    {
        string wType;
        MainForm myOwner;

        public ViewDB(string inType)
        {
            InitializeComponent();
            wType = inType;
        }

        private void ViewDB_Load(object sender, EventArgs e)
        {
            myOwner = this.Owner as MainForm;

            switch (wType)
            {
                case "editions_view":
                    this.Width = 503;
                    this.Location = new Point((myOwner.ScreenWidth - this.Width) / 2, this.Location.Y);
                    dataGridView1.DataSource = Startup.myData.mainDBdataset;
                    dataGridView1.DataMember = "Editions";
                    dataGridView1.Columns["id"].Visible = false;
                    dataGridView1.Columns["last_access"].Width = 100;
                    dataGridView1.Columns["last_access"].HeaderText = "Последнее изменение";
                    dataGridView1.Columns["code"].Width = 40;
                    dataGridView1.Columns["code"].HeaderText = "Код";
                    dataGridView1.Columns["name"].Width = 180;
                    dataGridView1.Columns["name"].HeaderText = "Название";
                    dataGridView1.Columns["comment"].HeaderText = "Комментарий";
                    this.Text = "Просмотр списка изданий";
                    //toolLabel.Text = "Всего записей: " + dataGridView1.RowCount.ToString();
                    break;
                case "issue_view":
                    this.Width = 693;
                    this.Location = new Point((myOwner.ScreenWidth - this.Width) / 2, this.Location.Y);
                    dataGridView1.DataSource = Startup.myData.ViewIssues();
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 150;
                    dataGridView1.Columns[2].Width = 80;
                    dataGridView1.Columns[3].Width = 80;
                    dataGridView1.Columns[4].Width = 100;
                    dataGridView1.Columns[5].Width = 160;
                    this.Text = "Просмотр списка выпусков";
                    //toolLabel.Text = "Всего записей: " + dataGridView1.RowCount.ToString();
                    break;
                case "section_view":
                    this.Width = 780;
                    this.Location = new Point((myOwner.ScreenWidth - this.Width) / 2, this.Location.Y);
                    dataGridView1.DataSource = Startup.myData.ViewSections(0, "");
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 80;
                    dataGridView1.Columns[3].Width = 60;
                    dataGridView1.Columns[4].Width = 40;
                    dataGridView1.Columns[5].Width = 100;
                    dataGridView1.Columns[6].Width = 160;
                    add_issues();
                    this.Text = "Просмотр списка секций";
                    toolStripButton.Visible = true;
                    break;
                case "view_activity":
                    dataGridView1.DataSource = Startup.myData.tblActivity;
                    dataGridView1.Columns[0].Visible = false;
                    break;
            }
            toolLabel.Text = "Всего записей: " + dataGridView1.RowCount.ToString();
        }

        private void add_issues()
//создаём список изданий для фильтровки в окне просмотра
        {
            List<string> issues = Startup.myData.GetEditionsList();
            if (issues.Count == 0)
            {
                issuesMenuItem.Enabled = false;
                return;
            }

            for (int i = 0; i < issues.Count; i++)
            {
                issuesMenuItem.DropDownItems.Add(issues[i]);
                issuesMenuItem.DropDownItems[i].Click += new EventHandler(editionClick);

            }
        }

        private void editionClick(object sender, EventArgs e)
        {
            ToolStripMenuItem b = (ToolStripMenuItem)sender;
            int filter = -1;

            foreach (ToolStripMenuItem s in issuesMenuItem.DropDownItems)
            {
                s.Checked = false;
            }
            
            filter = Startup.myData.GetEditionIDbyEditionName(b.Text);
            dataGridView1.DataSource = Startup.myData.ViewSections(2, b.Text);

            b.Checked = b.Checked.Equals(false);
        }

        private void ViewDB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void atexMenuItem_Click(object sender, EventArgs e)
//показать/скрыть только секции из Атекса
        {
            if (atexMenuItem.Checked.Equals(true))
            {
                dataGridView1.DataSource = Startup.myData.ViewSections(0, "");
            }
            else
            {
                dataGridView1.DataSource = Startup.myData.ViewSections(1, "");
            }
            atexMenuItem.Checked = atexMenuItem.Checked.Equals(false);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
