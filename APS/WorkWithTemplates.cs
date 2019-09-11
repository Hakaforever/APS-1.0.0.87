using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace APS
{
    public partial class WorkWithTemplates : Form
    {
        MainForm myOwner;
        string wType;

         //объект для формы курсора
        Timer stripTimer;

        BindingSource mainBS = new BindingSource();
        BindingSource tempBS = new BindingSource();

        internal class forDraw
        {
            internal Rectangle ghotstRectangle { get; set; }
            internal int sourceSection { get; set; }
            internal int sourceIssue { get; set; }
            internal int targetSection { get; set; }
            internal Point screenOffset { get; set; }
            internal ListViewItem specItem { get; set; }

            internal forDraw()
            {
                sourceSection = -1;
                sourceIssue = -1;
                targetSection = -1;
                specItem = null;
                ghotstRectangle = Rectangle.Empty;
                screenOffset = Point.Empty;
            }
        }

        forDraw ddd = new forDraw();

        public WorkWithTemplates(string inType)
        {
            InitializeComponent();
            wType = inType;
        }

        private void btnCancel_Click(object sender, EventArgs e)
//кнопка отмены
        {
            mainBS.CancelEdit();
            tempBS.CancelEdit();
            Startup.anyChanges = false;
            this.Close();
        }

        private void WorkWithTemplates_Load(object sender, EventArgs e)
//загрузка окна
        {
            myOwner = this.Owner as MainForm;

            mainBS.DataSource = Startup.myData.mainDBdataset;
            tempBS.DataSource = Startup.myData.mainDBdataset;
            mainBS.MoveLast();

            mainBS.DataMember = "Templates";
            mainBS.Filter = "daytoday = false";
            cmbSelect.DataSource = mainBS;
            cmbSelect.DisplayMember = "Name";
            cmbSelect.ValueMember = "Id";

            tempBS.DataMember = "Issues";
            cmbIssues.DataSource = tempBS;
            cmbIssues.DisplayMember = "Name";
            cmbIssues.ValueMember = "Id";
            cmbIssues.DataBindings.Add(new Binding("SelectedValue", mainBS, "issue_id", true));

            cmbPages.DataSource = Startup.myData.pages;
            txtName.DataBindings.Add(new Binding("Text", mainBS, "name", true, DataSourceUpdateMode.OnPropertyChanged));
            cmbPages.DataBindings.Add(new Binding("Text", mainBS, "pages", true, DataSourceUpdateMode.OnPropertyChanged));

            ckbDayToDay.DataBindings.Add(new Binding("CheckState", mainBS, "daytoday", true));
            ckbDayToDay.Visible = false;
            ckbDayToDay.CheckState = CheckState.Unchecked;

            switch (wType)
            {
                case "template_create":
                    lblSelect.Visible = cmbSelect.Visible = false;
                    mainBS.AddNew();
                    lblName.Location = new Point(lblName.Location.X, lblName.Location.Y - 20);
                    lblPages.Location = new Point(lblPages.Location.X, lblPages.Location.Y - 20);
                    lblIssue.Location = new Point(lblIssue.Location.X, lblIssue.Location.Y - 20);
                    txtName.Location = new Point(txtName.Location.X, txtName.Location.Y - 20);
                    cmbPages.Location = new Point(cmbPages.Location.X, cmbPages.Location.Y - 20);
                    cmbIssues.Location = new Point(cmbIssues.Location.X, cmbIssues.Location.Y - 20);
                    txtName.Text = "";
                    cmbPages.SelectedIndex = -1;
                    cmbIssues.SelectedIndex = -1;
                    this.Text = "Создание шаблона";
                    break;
                case "template_edit":
                    this.Text = "Редактирование шаблона";
                    break;
                case "template_copy":
                    this.Text = "Дублирование шаблона";
                    break;
                case "template_delete":
                    lblName.Visible = txtName.Visible = cmbPages.Visible = lblPages.Visible = false;
                    lblSelect.Location = new Point(lblSelect.Location.X, lblSelect.Location.Y + 10);
                    cmbSelect.Location = new Point(cmbSelect.Location.X, cmbSelect.Location.Y + 10);
                    lblIssue.Visible = cmbIssues.Visible = ckbNonAtexSection.Visible = false;
                    lstSections.Enabled = false;
                    lblSearch.Visible = txtSearch.Visible = btnSearchClear.Visible = btnSpec.Visible = false;
                    lstIssue.AllowDrop = false;
                    btnSave.Text = "Удалить";
                    btnSave.Image = Properties.Resources.Recycle_Bin_Empty_2;
                    this.Text = "Удаление шаблона";

                    break;
            }
            lstIssue.LargeImageList = Startup.myData.iconList;
            lstSections.LargeImageList = Startup.myData.iconList;

            Startup.mainPlane.FillSections(this.lstSections, this.cmbIssues.SelectedValue, txtSearch.Text, ckbNonAtexSection.Checked.Equals(false));
            if (mainBS.Count != 0)
            {
                Startup.mainPlane.FillIssue(lstIssue, wType,
                    (cmbPages.SelectedItem == null ? -1 : Convert.ToInt32(cmbPages.SelectedItem)),
                    (cmbSelect.SelectedIndex == -1 ? cmbSelect.Items[0].ToString() : cmbSelect.Text),
                    (cmbIssues.SelectedItem == null ? -1 : (int)cmbIssues.SelectedValue));
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
//при изменении текста быстрого поиска меняем список секций в соответствующем окне
        {
            Startup.mainPlane.FillSections(lstSections, cmbIssues.SelectedValue, txtSearch.Text, ckbNonAtexSection.Checked.Equals(false));
            if (lstSections.Items.Count != 0)
                btnSpec.Visible = false;
            else
                btnSpec.Visible = true;
        }

//кнопка "сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (wType)
            {
                case "template_create":
                    ckbDayToDay.CheckState = CheckState.Unchecked;
                    break;
                case "template_edit":
                    break;
                case "template_delete":
                    cmbPages.Tag = "delete";
                    if(MessageBox.Show("Вы действительно хотите удалить этот шаблон?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != System.Windows.Forms.DialogResult.No)
                    {
                        //if (Startup.myData.DeletePlane(cmbSelect.Text) == false)
                        Startup.myData.DeletePlane(cmbSelect.Text, (int)cmbIssues.SelectedValue);
                        //MessageBox.Show("Ошибка удаления шаблона!");
                    }
                    cmbPages.Tag = null;
                    break;
                case "template_copy":
                    if (txtName.Text == cmbSelect.Text)
                    {
                        MessageBox.Show("Не изменено название шаблона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string f = txtName.Text;
                    int ff = cmbPages.SelectedIndex;
                    int issue_ = cmbIssues.SelectedIndex;
                    txtName.Text = cmbSelect.Text;
                    txtName.DataBindings[0].WriteValue();
                    cmbPages.DataBindings[0].WriteValue();
                    mainBS.AddNew();
                    ckbDayToDay.CheckState = CheckState.Unchecked;
                    mainBS.MoveLast();
                    txtName.Text = f;
                    cmbIssues.SelectedIndex = issue_;
                    txtName.DataBindings[0].WriteValue();
                    cmbPages.DataBindings[0].WriteValue();
                    cmbIssues.DataBindings[0].WriteValue();
                    break;
            }

            if (!CheckBoxses()) return;
            mainBS.EndEdit();
            Exception err = Startup.myData.RefreshTableAdapters();
            if (err != null)
            {
                MessageBox.Show("Не удалось сохранить шаблон. Возможно шаблон с таким названием уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
//-->>      и тут заполняем Лайауты!
            bool r = WriteLayout();
            lstIssue.Tag = null;
            toolStrip.Image = Properties.Resources.Help_2;
            if (r == true)
            {
                SystemSounds.Beep.Play();
                toolStrip.Text = (wType == "template_delete" ? "Удалено!" : "Сохранено!");
                toolStrip.Visible = true;
                switch (wType)
                { 
                    case "template_delete":
                            Startup.mainPlane.FillIssue(lstIssue, wType, 
                                (cmbPages.SelectedItem == null ? -1 : Convert.ToInt32(cmbPages.SelectedItem)), 
                                (cmbSelect.SelectedIndex == -1 ? cmbSelect.Items[0].ToString() : cmbSelect.Text),
                                (cmbIssues.SelectedItem == null ? -1 : (int)cmbIssues.SelectedValue));
                            cmbPages.SelectedIndex = (cmbPages.SelectedIndex == 0 ? 0 : cmbPages.SelectedIndex - 1);
                        break;
//                    case "template_create":
                        
////                        lstIssue.Items.Clear();
////                        cmbPages.SelectedIndex = -1;
//                        break;
                }
            }
            else
            {
                SystemSounds.Asterisk.Play();
                toolStrip.Image = Properties.Resources.Help_1;
                toolStrip.Text = "Ошибка!";
                toolStrip.Visible = true;
            }
            Startup.myData.Write_Activity(Startup.User, wType, cmbSelect.Text);
            Startup.anyChanges = false;
        }

        private bool CheckBoxses()
//проверям поля на заполненность
        {
            if (String.IsNullOrEmpty(txtName.Text) || String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Поле не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.BackColor = Color.Pink;
                txtName.Focus();
                return false;
            }
            if (cmbPages.SelectedIndex == -1)
            {
                MessageBox.Show("Укажите количество полос!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbPages.DroppedDown = true;
                return false;
            }

            if (cmbIssues.SelectedIndex == -1)
            {
                MessageBox.Show("Укажите выпуск!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbIssues.DroppedDown = true;
                return false;
            }
            return true;
        }

        private bool CheckIssue()
//проверяем, что выпуск заполнен секциями
        {
            foreach (ListViewItem item in lstIssue.Items)
            {
                if (item.Text.Contains("Страница"))
                {
                    MessageBox.Show("Заполните полностью порядок полос в выпуске.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lstIssue.BackColor = Color.Pink;
                    return false;
                }
            }
            return true;
        }

        private bool WriteLayout()
//записываем содержимое окошка выпуска в базу
        {
            if (wType == "template_delete")
                return true;

            if (cmbSelect.SelectedIndex != -1)
            {
                string[] tmp = lstIssue.Items.OfType<ListViewItem>().Select(item => item.Name).ToArray();
                tmp = tmp.Where((x, i) => (i % 2) == 0).ToArray().Concat(tmp.Where((x, i) => (i%2) == 1).Reverse()).ToArray();
                Startup.myData.UpdatePlane(cmbSelect.Text, (int)cmbIssues.SelectedValue, tmp, false);
                return true;
            }
            return false;
        }

//----------->
//блок прорисовки Драг-Энд-Дропа
        private void lstSections_MouseDown(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseDown((ListView)sender, e);
        }

        private void dragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void lstSections_MouseUp(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseUp((ListView)sender, e);
            //Clear_Selection();
        }

        private void lstIssue_MouseUp(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseUp(lstSections, lstIssue);
        }

        private void lstIssue_DragDrop(object sender, DragEventArgs e)
        {
            Startup.mainPlane.dnd_DragDrop(lstSections, lstIssue);
        }

        private void lstIssue_DragOver(object sender, DragEventArgs e)
        {
            Startup.mainPlane.dnd_DragOver((ListView)sender, e);
        }

        private void lstIssue_MouseDown(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseDown((ListView)sender, e);
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseMove((ListView)sender, e);
        }

        private void btnSpec_MouseDown(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseDown(txtSearch.Text, e);
        }

        private void btnSpec_MouseMove(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseMove((Button)sender, e);
        }

//--------------->
        private void btnSearchClear_Click(object sender, EventArgs e)
//очистить окошко быстрого поиска секции
        {
            txtSearch.Text = "";
            lstSections.Focus();
        }

        private void cmbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(wType)
            {
                case "template_edit":
                    break;
            }
            Startup.mainPlane.FillSections(lstSections, cmbIssues.SelectedValue, txtSearch.Text, ckbNonAtexSection.Checked.Equals(false));
            mainBS.Position = cmbSelect.SelectedIndex;
            Startup.mainPlane.FillIssue(lstIssue, wType, Convert.ToInt32(cmbPages.Text), cmbSelect.Text, (int)cmbIssues.SelectedValue);            
        }

        private void cmbPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            string planeName;

            switch (wType)
            { 
                case "template_copy":
                    planeName = txtName.Text;
                    break;
                default:
                    planeName = (cmbSelect.SelectedIndex == -1 ? cmbSelect.Items[0].ToString() : txtName.Text);
                    break;
            }
            Startup.mainPlane.FillIssue(lstIssue, wType, 
                (cmbPages.SelectedItem == null ? -1 : Convert.ToInt32(cmbPages.SelectedItem)),
                planeName, (cmbIssues.SelectedItem == null ? -1 : (int)cmbIssues.SelectedValue));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStrip.Visible = false;
            stripTimer.Stop();
            stripTimer.Dispose();
            stripTimer = null;
        }

        private void toolStrip_VisibleChanged(object sender, EventArgs e)
        {
            if (toolStrip.Visible.Equals(true))
            {
                stripTimer = new Timer();
                stripTimer.Interval = 3000;
                stripTimer.Start();
                stripTimer.Tick += new EventHandler(timer1_Tick);
            }
        }

        private void cmbEdition_DropDownClosed(object sender, EventArgs e)
//событие смены названия выпуска
        {
            if (this.Visible == true)
                Startup.mainPlane.FillSections(lstSections, cmbIssues.SelectedValue, txtSearch.Text, ckbNonAtexSection.Checked.Equals(false));
        }

        private void ckbNonAtexSection_CheckedChanged(object sender, EventArgs e)
//событие смены чекбокса "ручные секции"
        {
            Startup.mainPlane.FillSections(lstSections, cmbIssues.SelectedValue, "", ckbNonAtexSection.Checked.Equals(false));
        }
    }
}
