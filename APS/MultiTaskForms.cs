using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Media;

namespace APS
{
    public partial class MultiTaskForms : Form
    {
        MainForm myOwner;
        string wType;

        public MultiTaskForms(string inType)
        {
            InitializeComponent();
            wType = inType;
        }

//закрыть окно
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Startup.myData.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void MultiTaskForms_Load(object sender, EventArgs e)
        {
            myOwner = this.Owner as MainForm;
            Startup.myData.bsMain.MoveFirst();
            switch (wType)
            {
//работа с выпусками
                case "issue_create":
                    Startup.myData.bsMain.DataMember = "Issues";
                    Startup.myData.bsTemp.DataMember = "Editions";
                    cmbEdition.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsMain, "edition_id", true));
                    cmbEdition.DataSource = Startup.myData.bsTemp;
                    cmbEdition.DisplayMember = "Name";
                    cmbEdition.ValueMember = "id";

                    //cmbSelect.Visible = lblCombo.Visible = btnColor.Visible = chkSpread.Visible = chkAtex.Visible = lblColor.Visible = false;
                    cmbSelect.Visible = lblCombo.Visible = btnIcon.Visible = chkAtex.Visible = lblColor.Visible = false;
                    chkSpread.Text = "Активный";
                    chkSpread.Location = new Point(339, 50);
                    txtCode.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Code".ToLower(), true));
                    txtName.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Name".ToLower(), true));
                    txtComment.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Comment".ToLower(), true));
                    chkSpread.DataBindings.Add(new Binding("CheckState", Startup.myData.bsMain, "Enabled", true));
                    Startup.myData.AddNew(Startup.myData.bsMain);
                    chkSpread.Checked = false;
                    txtCode.Text = txtName.Text = txtComment.Text = "";
                    Move_TextBoxses();
                    btnOK.Text = "Добавить";
                    this.Text = "Добавление выпуска";
                    txtCode.Focus();
                    break;
                case "issue_edit":
                    Startup.myData.bsMain.DataMember = "Issues";
                    Startup.myData.bsTemp.DataMember = "Editions";
                    cmbEdition.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsMain, "edition_id", true));
                    cmbEdition.DataSource = Startup.myData.bsTemp;
                    cmbEdition.DisplayMember = "Name";
                    cmbEdition.ValueMember = "id";

                    //btnColor.Visible = chkSpread.Visible = chkAtex.Visible = lblColor.Visible = false;
                    btnIcon.Visible = chkAtex.Visible = lblColor.Visible = false;
                    chkSpread.Text = "Активный";
                    chkSpread.Location = new Point(339, 30);
                    chkSpread.DataBindings.Add(new Binding("CheckState", Startup.myData.bsMain, "Enabled", true));
                    cmbSelect.DataSource = Startup.myData.bsMain;
                    cmbSelect.DisplayMember = "Name";
                    cmbSelect.ValueMember = "Id";
                    txtCode.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Code".ToLower(), true));
                    txtName.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Name".ToLower(), true));
                    txtComment.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Comment".ToLower(), true));
                    btnOK.Text = "Изменить";
                    this.Text = "Изменение выпуска";
                    txtCode.Focus();
                    break;
                case "issue_delete":
                    Startup.myData.bsMain.DataMember = "Issues";
                    lblEdition.Visible = cmbEdition.Visible = false;
                    cmbSelect.DataSource = Startup.myData.bsMain;
                    cmbSelect.DisplayMember = "Name";
                    cmbSelect.ValueMember = "Id";
                    btnOK.Image = Properties.Resources.Recycle_Bin_Empty_2;
                    btnIcon.Visible = chkSpread.Visible = chkAtex.Visible = txtCode.Visible = txtName.Visible = txtComment.Visible = lblName.Visible = lblCode.Visible = lblComment.Visible = lblColor.Visible = false;
                    btnOK.Text = "Удалить";
                    this.Text = "Удаление выпуска";
                    break;
//работа с секциями
                case "section_create":
                    Startup.myData.bsMain.DataMember = "Sections";
                    Startup.myData.bsTemp.DataMember = "Editions";
                    cmbEdition.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsMain, "edition_id", true));
                    cmbEdition.DataSource = Startup.myData.bsTemp;
                    cmbEdition.DisplayMember = "Name";
                    cmbEdition.ValueMember = "id";

                    cmbSelect.Visible = lblCombo.Visible = false;
                    txtCode.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Code".ToLower(), true));
                    txtName.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Name".ToLower(), true));
                    txtComment.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Comment".ToLower(), true));
                    chkSpread.DataBindings.Add(new Binding("CheckState", Startup.myData.bsMain, "is_spread".ToLower(), true));
                    chkAtex.DataBindings.Add(new Binding("CheckState", Startup.myData.bsMain, "from_atex".ToLower(), true));
                    btnIcon.DataBindings.Add(new Binding("BackgroundImage", Startup.myData.bsMain, "Icon".ToLower(), true));
                    Startup.myData.AddNew(Startup.myData.bsMain);
                    txtCode.Text = txtName.Text = txtComment.Text = btnIcon.Text = "";
                    chkSpread.CheckState = chkAtex.CheckState = CheckState.Unchecked;
                    //chkSpread.ThreeState = chkAtex.ThreeState = false;
                    btnOK.Text = "Добавить";
                    this.Text = "Добавление секции";
                    Move_TextBoxses();
                    txtCode.Focus();
                    break;
                case "section_edit":
                    Startup.myData.bsMain.DataMember = "Sections";
                    Startup.myData.bsTemp.DataMember = "Editions";
                    cmbEdition.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsMain, "edition_id", true));
                    cmbEdition.DataSource = Startup.myData.bsTemp;
                    cmbEdition.DisplayMember = "Name";
                    cmbEdition.ValueMember = "id";

                    cmbSelect.DataSource = Startup.myData.bsMain;
                    cmbSelect.DisplayMember = "Name";
                    cmbSelect.ValueMember = "Id";
                    txtCode.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Code".ToLower(), true));
                    txtName.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Name".ToLower(), true));
                    txtComment.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Comment".ToLower(), true));
                    chkSpread.DataBindings.Add(new Binding("CheckState", Startup.myData.bsMain, "is_spread".ToLower(), true));
                    chkAtex.DataBindings.Add(new Binding("CheckState", Startup.myData.bsMain, "from_atex".ToLower(), true));
                    btnIcon.DataBindings.Add(new Binding("BackgroundImage", Startup.myData.bsMain, "icon".ToLower(), true));
                    //lblEdition.Visible = cmbEdition.Visible = false;
                    ChangeBtnColor();
                    btnOK.Text = "Изменить";
                    this.Text = "Изменение секции";
                    txtCode.Focus();
                    break;
                case "section_delete":
                    Startup.myData.bsMain.DataMember = "Sections";

                    cmbSelect.DataSource = Startup.myData.bsMain;
                    cmbSelect.DisplayMember = "Name";
                    cmbSelect.ValueMember = "Id";
                    lblEdition.Visible = cmbEdition.Visible = false;
                    btnOK.Image = Properties.Resources.Recycle_Bin_Empty_2;
                    btnIcon.Visible = chkSpread.Visible = chkAtex.Visible = txtCode.Visible = txtName.Visible = txtComment.Visible = lblName.Visible = lblCode.Visible = lblComment.Visible = lblColor.Visible = false;
                    btnOK.Text = "Удалить";
                    this.Text = "Удаление секции";
                    break;
//работа с изданиями
                case "edition_create":
                    Startup.myData.bsMain.DataMember = "Editions";

                    cmbSelect.Visible = lblCombo.Visible = btnIcon.Visible = chkSpread.Visible = chkAtex.Visible = lblColor.Visible = false;
                    txtCode.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Code".ToLower(), true));
                    txtName.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Name".ToLower(), true));
                    txtComment.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Comment".ToLower(), true));
                    lblEdition.Visible = cmbEdition.Visible = false;
                    Startup.myData.AddNew(Startup.myData.bsMain);
                    txtCode.Text = txtName.Text = txtComment.Text = "";
                    Move_TextBoxses();
                    btnOK.Text = "Добавить";
                    this.Text = "Добавление издания";
                    txtCode.Focus();
                    break;
                case "edition_edit":
                    Startup.myData.bsMain.DataMember = "Editions";

                    btnIcon.Visible = chkSpread.Visible = chkAtex.Visible = lblColor.Visible = false;
                    cmbSelect.DataSource = Startup.myData.bsMain;
                    cmbSelect.DisplayMember = "Name";
                    cmbSelect.ValueMember = "Id";
                    txtCode.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Code".ToLower(), true));
                    txtName.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Name".ToLower(), true));
                    txtComment.DataBindings.Add(new Binding("Text", Startup.myData.bsMain, "Comment".ToLower(), true));
                    lblEdition.Visible = cmbEdition.Visible = false;
                    btnOK.Text = "Изменить";
                    this.Text = "Изменение издания";
                    txtCode.Focus();
                    break;
                case "edition_delete":
                    Startup.myData.bsMain.DataMember = "Editions";

                    cmbSelect.DataSource = Startup.myData.bsMain;
                    cmbSelect.DisplayMember = "Name";
                    cmbSelect.ValueMember = "Id";
                    btnOK.Image = Properties.Resources.Recycle_Bin_Empty_2;
                    btnIcon.Visible = chkSpread.Visible = chkAtex.Visible = txtCode.Visible = txtName.Visible = txtComment.Visible = lblName.Visible = lblCode.Visible = lblComment.Visible = lblColor.Visible = false;
                    lblEdition.Visible = cmbEdition.Visible = false;

                    btnOK.Text = "Удалить";
                    this.Text = "Удаление издания";
                    break;
            }

        }

//кнопка ОК
        private void btnOK_Click(object sender, EventArgs e)
        {
            int index = cmbSelect.SelectedIndex;

            if (!CheckBoxes()) return;

            string tempText = txtName.Text;
            Startup.myData.AddTime();

            switch (wType)
            {
//работа с выпусками
                case "issue_create":
                    this.Validate();
                    if (Startup.myData.AddNew(Startup.myData.bsMain) == null)
                    {
                        stripStatus.Text = "Выпуск " + "\"" + tempText + "\"" + " создан.";
                    }
                    else
                    {
                        MessageBox.Show("Выпуск с такими параметрами уже существует!", "Создание выпуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stripStatus.Text = "Ошибка создания выпуска!";
                        return;
                    }
                    break;
                case "issue_edit":
                    this.Validate();
                    if (Startup.myData.EndEdit() == null)
                    {
                        stripStatus.Text = "Выпуск " + "\"" + tempText + "\"" + " изменен.";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения выпуска...", "Изменение выпуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stripStatus.Text = "Ошибка изменения выпуска!";
                        return;
                    }
                    break;
                case "issue_delete":
                    if (MessageBox.Show("Вы действительно хотите удалить этот выпуск?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string s = cmbSelect.Text;
                        this.Validate();
                        if (Startup.myData.DeleteIssue(cmbSelect.Text) == null)
                        {
                            stripStatus.Text = "Выпуск " + "\"" + s + "\"" + " удалён.";
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить выпуск...", "Удаление выпуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stripStatus.Text = "Ошибка удаления выпуска!";
                            return;
                        }
                    }
                    Startup.myData.MoveFirst();
                    break;
//работа с секциями
                case "section_create":
                    this.Validate();
                    if (Startup.myData.AddNew(Startup.myData.bsMain) == null)
                    {
                        stripStatus.Text = "Секция " + "\"" + tempText + "\"" + " создана.";
                    }
                    else
                    {
                        MessageBox.Show("Секция с такими параметрами уже существует!", "Создание секции", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show(ex.Message, "Создание секции", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stripStatus.Text = "Ошибка создания секции!";
                        return;
                    }
                    chkAtex.CheckState = chkSpread.CheckState = CheckState.Unchecked;
                    break;
                case "section_edit":
                    this.Validate();
                    Exception ex = Startup.myData.EndEdit();
                    if ( ex == null)
                    {
                        cmbSelect.SelectedIndex = index;
                        stripStatus.Text = "Секция " + "\"" + tempText + "\"" + " изменена.";
                    }
                    else
                    {
                        MessageBox.Show(ex.Message, "Изменение секции", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stripStatus.Text = "Ошибка изменения секции!";
                        return;
                    }
                    break;
                case "section_delete":
                    if (MessageBox.Show("Вы действительно хотите удалить эту секцию?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string s = cmbSelect.Text;
                        this.Validate();
                        if (Startup.myData.DeleteSection(cmbSelect.Text) == null)
                        {
                            stripStatus.Text = "Секция " + "\"" + s + "\"" + " удалена.";
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить секцию...", "Удаление секции", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stripStatus.Text = "Ошибка удаления секции!";
                            return;
                        }
                    }
                    Startup.myData.MoveFirst();
                    break;
//издания
                case "edition_create":
                    this.Validate();
                    if (Startup.myData.AddNew(Startup.myData.bsMain) == null)
                    {
                        stripStatus.Text = "Издание " + "\"" + tempText + "\"" + " создано.";
                    }
                    else
                    {
                        MessageBox.Show("Издание с такими параметрами уже существует!", "Создание издания", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stripStatus.Text = "Ошибка создания издания!";
                        return;
                    }
                    break;
                case "edition_edit":
                    this.Validate();
                    if (Startup.myData.EndEdit() == null)
                    {
                        stripStatus.Text = "Издание " + "\"" + tempText + "\"" + " изменено.";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка изменения издания...", "Изменение издания", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        stripStatus.Text = "Ошибка изменения издания!";
                        return;
                    }
                    break;
                case "edition_delete":
                    if (MessageBox.Show("Вы действительно хотите удалить это издание?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string s = cmbSelect.Text;
                        this.Validate();
                        if (Startup.myData.DeleteEdition(cmbSelect.Text) == null)
                        {
                            stripStatus.Text = "Издание " + "\"" + s + "\"" + " удалено.";
                        }
                        else
                        {
                            MessageBox.Show("Не удалось удалить издание...", "Удаление издания", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            stripStatus.Text = "Ошибка удаления издания!";
                            return;
                        }
                    }
                    Startup.myData.MoveFirst();
                    break;
            }
            Startup.myData.Write_Activity(Startup.User, wType, "каша");
        }

//проверка полей формы на пустоту
        private bool CheckBoxes()
        {
            if (txtCode.Visible && (String.IsNullOrEmpty(txtCode.Text) || String.IsNullOrWhiteSpace(txtCode.Text)))
            {
                MessageBox.Show("Поле не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCode.BackColor = Color.Pink;
                txtCode.Focus();
                return false;
            }
            if (txtName.Visible && (String.IsNullOrEmpty(txtName.Text) || String.IsNullOrWhiteSpace(txtName.Text)))
            {
                MessageBox.Show("Поле не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.BackColor = Color.Pink;
                txtName.Focus();
                return false;
            }
            if (cmbSelect.Visible && cmbSelect.SelectedIndex == -1)
            {
                MessageBox.Show("Требуется выбрать значение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbSelect.DroppedDown = true;// .BackColor = Color.Pink;
                return false;
            }
            if (cmbEdition.Visible && cmbEdition.SelectedIndex == -1)
            {
                MessageBox.Show("Требуется выбрать значение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbEdition.DroppedDown = true;// .BackColor = Color.Pink;
                return false;
            }
            return true;
        }

        private void stripStatus_TextChanged(object sender, EventArgs e)
        {
            stripStatus.Image = Properties.Resources.Help_2;
            stripStatus.Visible = true;
            SystemSounds.Beep.Play();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            //colorDialog1.Color = btnColor.BackColor;
            //if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    btnColor.BackColor = colorDialog1.Color;
            //    btnColor.ForeColor = colorDialog1.Color;
            //    btnColor.Text = colorDialog1.Color.A.ToString() + ", " + colorDialog1.Color.R.ToString() + ", " + colorDialog1.Color.G.ToString() + ", " + colorDialog1.Color.B.ToString();
            //}
            if (setIcon.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                btnIcon.BackgroundImage = new Bitmap(setIcon.FileName);
            }
        }
//другое
        private void Move_TextBoxses()
//двигаем контролы для разных случаев инициализации окна
        {
            cmbSelect.Location = new Point(cmbSelect.Location.X, cmbSelect.Location.Y + 20);
            lblCombo.Location = new Point(lblCombo.Location.X, lblCombo.Location.Y + 20);
            txtCode.Location = new Point(txtCode.Location.X, txtCode.Location.Y - 20);
            lblCode.Location = new Point(lblCode.Location.X, lblCode.Location.Y - 20);
            txtName.Location = new Point(txtName.Location.X, txtName.Location.Y - 20);
            lblName.Location = new Point(lblName.Location.X, lblName.Location.Y - 20);
            txtComment.Location = new Point(txtComment.Location.X, txtComment.Location.Y - 20);
            lblComment.Location = new Point(lblComment.Location.X, lblComment.Location.Y - 20);
            lblEdition.Location = new Point(lblEdition.Location.X, lblEdition.Location.Y - 20);
            cmbEdition.Location = new Point(cmbEdition.Location.X, cmbEdition.Location.Y - 20);
        }

        private void ChangeBtnColor()
        //меняем цвет кнопки при работе с секциями
        {
            //string[] data = btnIcon.Text.Split(',');

            //if (data.Length == 4)
            //{
            //    Color r = Color.FromArgb(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]));
            //    btnIcon.BackColor = btnIcon.ForeColor = r;
            //}
            //else
            //{
            //    btnIcon.BackColor = btnIcon.ForeColor = btnOK.BackColor;
            //}

        }

        private void cmbEdition_DropDownClosed(object sender, EventArgs e)
        {
            //int i = 0;
        }

    }
}
