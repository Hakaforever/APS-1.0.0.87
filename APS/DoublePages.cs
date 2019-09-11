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
    public partial class DoublePages : Form
    {
        DrawPlane.DblPageInfo pageInfo;
        List<CheckBox> labelList = new List<CheckBox>();
        List<ComboBox> comboList = new List<ComboBox>();

        internal DoublePages(ref DrawPlane.DblPageInfo inPageInfo)
        {
            InitializeComponent();
            pageInfo = inPageInfo;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DoubleIssue_Load(object sender, EventArgs e)
//загрузка формы
        {
            //for (int y = 0; y < pageInfo.targetPageInfo.Count; y++)
            //{
            //    Label lb = new Label();
            //    lb.Text = pageInfo.targetPageInfo[y].issueName;
            //    lb.Location = new Point(15, 20 + y * 30);
            //    ComboBox cb = new ComboBox();
            //    cb.Location = new Point(135, 20 + y * 30);
            //    cb.Items.Add("Не копировать/Удалить связь");
            //    cb.DropDownWidth = 162;
            //    cb.Tag = pageInfo.targetPageInfo[y].issueId;
            //    for (int j = 1; j <= pageInfo.targetPageInfo[y].pageCount; j++)
            //        cb.Items.Add(j.ToString());
            //    //определяем номер полосы для дублирования
            //    try
            //    {
            //        cb.SelectedIndex = pageInfo.targetPageInfo[y].pageNum;
            //    }
            //    catch (Exception)
            //    {
            //        cb.SelectedIndex = -1;
            //    }
            //    cb.DropDownStyle = ComboBoxStyle.DropDownList;
            //    cb.Size = new Size(60, 210);
            //    this.Controls.Add(lb);
            //    this.Controls.Add(cb);
            //    labelList.Add(lb);
            //    comboList.Add(cb);
            //}
            for (int y = 0; y < pageInfo.targetPageInfo.Count; y++)
            {
                CheckBox lb = new CheckBox();
                lb.Text = pageInfo.targetPageInfo[y].issueName;
                lb.Location = new Point(15, 20 + y * 30);

                ComboBox cb = new ComboBox();
                cb.Location = new Point(135, 20 + y * 30);
                cb.Enabled = false;
                //cb.Items.Add("Не копировать/Удалить связь");
                //cb.DropDownWidth = 162;
                cb.Tag = pageInfo.targetPageInfo[y].issueId;
                for (int j = 1; j <= pageInfo.targetPageInfo[y].pageCount; j++)
                    cb.Items.Add(j.ToString());
                if (pageInfo.targetPageInfo[y].pageNum == -1)
                {
                    lb.CheckState = CheckState.Unchecked;
                    cb.SelectedIndex = -1;
                }
                else
                {
                    lb.CheckState = CheckState.Checked;
                    try
                    {
                        cb.SelectedIndex = Convert.ToInt32(this.Tag) - 1;
                    }
                    catch
                    {
                        cb.SelectedIndex = -1;
                    }
                    cb.Enabled = true;
                }
                lb.Tag = cb;

                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Size = new Size(60, 210);
                lb.CheckStateChanged += new EventHandler(iss_Click);
                this.Controls.Add(lb);
                this.Controls.Add(cb);
                labelList.Add(lb);
                comboList.Add(cb);
            }
            this.Size = new Size(253, pageInfo.targetPageInfo.Count * 30 + 150);
            //this.Location = Startup.Location(this);
        }

        private void iss_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            ComboBox combo = (ComboBox)cb.Tag;

            switch (cb.CheckState)
            { 
                case CheckState.Checked:
                    combo.Enabled = true;
                    try
                    {
                        combo.SelectedIndex = Convert.ToInt32(this.Tag) - 1;
                    }
                    catch 
                    {
                        combo.SelectedIndex = -1;
                    }
                    break;
                case CheckState.Unchecked:
                    combo.Enabled = false;
                    combo.SelectedIndex = -1;
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
//сохранение 
        {
            //for (int z = 0; z < labelList.Count; z++)
            //{
            //    int index = pageInfo.targetPageInfo.FindIndex(p => p.issueId == (int)comboList[z].Tag);
            //    if (index != -1)
            //    {
            //        DrawPlane.pInfo o = new DrawPlane.pInfo();
            //        //o.issueId = pageInfo.targetPageInfo[index].issueId;
            //        o.issueId = Convert.ToInt32(comboList[z].Tag.ToString());
            //        o.issueName = pageInfo.targetPageInfo[index].issueName;
            //        o.pageCount = pageInfo.targetPageInfo[index].pageCount;
            //        o.pageNum = comboList[z].SelectedIndex;
            //        o.planeId = pageInfo.targetPageInfo[index].planeId;
            //        pageInfo.targetPageInfo.RemoveAt(index);
            //        pageInfo.targetPageInfo.Add(o);
            //        this.Controls.Remove(labelList[z]);
            //        this.Controls.Remove(comboList[z]);
            //    }
            //}

            for (int z = 0; z < comboList.Count; z++)
            {
                int index = pageInfo.targetPageInfo.FindIndex(p => p.issueId == (int)comboList[z].Tag);
                if (index != -1)
                {
                    DrawPlane.pInfo o = new DrawPlane.pInfo();
                    o.issueId = Convert.ToInt32(comboList[z].Tag.ToString());
                    o.issueName = pageInfo.targetPageInfo[index].issueName;
                    o.pageCount = pageInfo.targetPageInfo[index].pageCount;
                    o.pageNum = (comboList[z].Enabled == true ? Convert.ToInt32(comboList[z].Text) : -1);
                    o.planeId = pageInfo.targetPageInfo[index].planeId;
                    pageInfo.targetPageInfo[index] = o;
                }
            }
            comboList.Clear();
            labelList.Clear();
            this.Close();
        }
    }
}
