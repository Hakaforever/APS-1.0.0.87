using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace APS
{
    public partial class WorkWithPlane : Form
    {
        public MainForm myOwner;

        string wType;

        public WorkWithPlane(string WindowType)
        {
            InitializeComponent();
            wType = WindowType;
        }

        List<Control> addedCombo = new List<Control>();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkWithPlane_Load(object sender, EventArgs e)
        {
            Startup.myData.ClearBSFilters();

            switch (wType)
            { 
                case "main_plane":
                    Startup.myData.bsMain.DataMember = "Editions";
                    Startup.myData.bsTemp.DataMember = "GlobalPlanes";
                    Startup.myData.bsThird.DataMember = "Issues";

                    break;
                case "main_color":
                    Startup.myData.bsMain.DataMember = "editions";
                    Startup.myData.bsTemp.DataMember = "globalcolors";
                    Startup.myData.bsThird.DataMember = "Issues";
                    break;
                case "main_deadline":
                    Startup.myData.bsMain.DataMember = "editions";
                    Startup.myData.bsTemp.DataMember = "deadlines";
                    Startup.myData.bsThird.DataMember = "Issues";
                    break;
                case "view_color":
                    Startup.myData.bsMain.DataMember = "editions";
                    Startup.myData.bsTemp.DataMember = "globalcolors";
                    Startup.myData.bsThird.DataMember = "Issues";
                    this.btnSave.Visible = false;
                    break;
            }
            cmbEdition.DataSource = Startup.myData.bsMain;
            cmbEdition.ValueMember = "id";
            cmbEdition.DisplayMember = "name";
            Startup.myData.bsMain.MoveFirst();
            FillPlanes();

        }

        private void FillPlanes()
        {
            tblPanel.Visible = false;
            double sSize = 20.0f;

            if (addedCombo.Count != 0)
            {
                for (int i = addedCombo.Count - 1; i >= 0; i--)
                {
                    tblPanel.Controls.Remove(addedCombo[i]);
                }
                addedCombo.Clear();
                for (int i = tblPanel.RowCount - 1; i > 1; i--)
                {
                    tblPanel.RowStyles.RemoveAt(i);
                    tblPanel.RowCount--;
                }
            }

            //switch (wType)
            //{ 
            //    case "main_plane":
                    Startup.myData.bsThird.Filter = "edition_id = " + cmbEdition.SelectedValue + " AND enabled = 1";
            //        break;
            //}
            //BindingSource issuesSource = new BindingSource();
            //issuesSource.DataSource = Startup.myData.mainDBdataset;
            //issuesSource.DataMember = "Issues";
            //issuesSource.Filter = "edition_id = " + cmbEdition.SelectedValue + " AND enabled = 1";

            List<string> editions = Startup.myData.GetIssuesList((int)cmbEdition.SelectedValue, true);

            tblPanel.RowStyles[1].SizeType = SizeType.Absolute;
            tblPanel.RowStyles[1].Height = 0.0f;

            for (int i = 0; i < Startup.myData.bsThird.Count; i++)
            {
                DataRowView rv = (DataRowView)Startup.myData.bsThird[i];
                tblPanel.RowCount++;

                tblPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30.0f));
                sSize = sSize + 35.0f;
                Label newLB = new Label() { Text = rv.Row.Field<string>("name"), Margin = new Padding(0, 9, 0, 0) };

                tblPanel.Controls.Add(newLB, 0, tblPanel.RowCount - 1);
                addedCombo.Add(newLB);
            
                for (int j = 1; j < tblPanel.ColumnCount; j++)
                {
                    CheckBox chk = new CheckBox();
                    foreach(Control c in tblPanel.Controls)
                    {
                        if (c is CheckBox && c.Name == "checkBox" + j.ToString() && ((CheckBox)c).Checked != false)
                        {
                            BindingSource bS = new BindingSource();
                            bS.DataSource = Startup.myData.mainDBdataset;
                            switch (wType)
                            {
                                case "main_plane":
                                    bS.DataMember = "Templates";
                                    bS.Filter = "issue_id = " + Startup.myData.GetIssueIDbyName(editions[i]) + "and daytoday = false";
                                    break;
                                case "view_color":
                                case "main_color":
                                    bS.DataMember = "colors_schemes";
                                    bS.Filter = "issue_id = " + Startup.myData.GetIssueIDbyName(editions[i]);
                                    break;
                            }
                            ComboBox newCB = new ComboBox()
                            {
                                DataSource = bS,
                                DropDownStyle = ComboBoxStyle.DropDownList,
                                ValueMember = "id",
                                DisplayMember = "name",
                                DropDownWidth = 180,
                                Tag = rv.Row.Field<int>("id") + "/" + (j == 7 ? "0" : j.ToString())
                                //складываем айди выпуска и номер дня недели, представленный как номер колонки в TablePnelLayout
                            };
                            tblPanel.Controls.Add(newCB, j, tblPanel.RowCount - 1);
                            newCB.SelectedIndex = -1;
                            addedCombo.Add(newCB);
                        }
                    }
                }
            }
            tblPanel.Size = new Size(tblPanel.Width, (int)sSize);
            this.Size = new Size(this.Width, 102 + tblPanel.Height);
            tblPanel.Visible = true;

            BindingSource planesBS = new BindingSource();
            BindingSource tmpBS = new BindingSource();

            switch (wType)
            {
                case "main_plane":
                    planesBS.DataSource = Startup.myData.mainDBdataset;
                    planesBS.DataMember = "GlobalPlanes";
                    tmpBS.DataSource = Startup.myData.tblGlobalPlanes;
                    break;
                case "view_color":
                case "main_color":
                    planesBS.DataSource = Startup.myData.mainDBdataset;
                    planesBS.DataMember = "colors_schemes";
                    tmpBS.DataSource = Startup.myData.tblGlobalColors;
                    break;
            }

            for (int i = 0; i < tblPanel.Controls.Count; i++)
            {
                if (tblPanel.Controls[i].GetType().ToString().Contains("ComboBox"))
                {
                    ComboBox war = (ComboBox)tblPanel.Controls[i];
                    string[] s = war.Tag.ToString().Split('/');
                    tmpBS.Filter = "issue_id = " + s[0] + " AND day_of_week = " + s[1];
                    DataRowView z;
                    if (tmpBS.Count != 0)
                    {
                        z = (DataRowView)tmpBS[0];
                        switch (wType)
                        {
                            case "main_plane":
                                war.SelectedValue = z.Row.Field<int>("template_id");
                                break;
                            case "view_color":
                            case "main_color":
                                war.SelectedValue = z.Row.Field<int>("scheme_id");
                                break;
                        }
                    } 
                    else
                        war.SelectedIndex = -1;
                }

            }
        }

        private void cmbEdition_DropDownClosed(object sender, EventArgs e)
        {
            FillPlanes();
        }

        private void btnSave_Click(object sender, EventArgs e)
//записываем обновлённый план
        {
            foreach (Control combo in tblPanel.Controls)
            {
                ComboBox c;
                if (combo.GetType().ToString().Contains("ComboBox"))
                {
                    c = (ComboBox)combo;
                    if (c.SelectedIndex != -1)
                    {
                        string[] s = c.Tag.ToString().Split('/');
                        switch (wType)
                        {
                            case "main_plane":
                                Startup.myData.UpdateGlobalPlanes(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), (int)cmbEdition.SelectedValue, (int)c.SelectedValue);
                                break;
                            case "view_color":
                            case "main_color":
                                Startup.myData.UpdateGlobalColors(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), (int)cmbEdition.SelectedValue, (int)c.SelectedValue);
                                break;
                        }
                    }
                }
            }
            Startup.myData.EndEdit();
            SystemSounds.Beep.Play();
        }
    }
}
