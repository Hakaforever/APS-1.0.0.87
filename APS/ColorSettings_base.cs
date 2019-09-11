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
    public partial class ColorSettings_base : Form
    {
        //настройки по цветности полос

        internal int[] colors;

        internal string wType = "";

        internal int formHeight = 315;

        internal MainForm myOwner;

        public ColorSettings_base()
        {
            InitializeComponent();
        }

        internal virtual void ColorSettings_Load(object sender, EventArgs e)
        //загрузка базового окна
        {
            Startup.myData.ClearBSFilters();
            this.Height = formHeight;
            infoText.Text = "";
            Startup.myData.bsThird.DataMember = "Colors_schemes";
            Startup.myData.bsMain.DataMember = "Issues";
            Startup.myData.bsTemp.DataMember = "Editions";

            cmbEdition.DataSource = Startup.myData.bsTemp;
            cmbEdition.DisplayMember = "Name";
            cmbEdition.ValueMember = "id";

            cmbIssue.DataSource = Startup.myData.bsMain;
            cmbIssue.DisplayMember = "Name";
            cmbIssue.ValueMember = "id";

            cmbPages.Items.AddRange(Startup.myData.pages);

            cmbScheme.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsThird, "id", true));
            cmbScheme.DataSource = Startup.myData.bsThird;
            cmbScheme.DisplayMember = "Name";
            cmbScheme.ValueMember = "Id";

            cmbEdition.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsThird, "edition_id", true, DataSourceUpdateMode.OnPropertyChanged));
            cmbIssue.DataBindings.Add(new Binding("SelectedValue", Startup.myData.bsThird, "issue_id", true, DataSourceUpdateMode.OnPropertyChanged));
            txbName.DataBindings.Add(new Binding("Text", Startup.myData.bsThird, "name", true, DataSourceUpdateMode.OnPropertyChanged));

            try
            {
                colors = Startup.myData.GetColorsForSheme((int)cmbScheme.SelectedValue);
            }
            catch (Exception)
            {
                colors = null;
            }
            cmbPages.DataBindings.Add(new Binding("Text", Startup.myData.bsThird, "pages_count", true, DataSourceUpdateMode.OnPropertyChanged));
        }

        internal virtual void pCount_SelectedIndexChanged(object sender, EventArgs e)
        //событие вбыора количества страниц для выпуска
        {
            if (cmbPages.SelectedIndex != -1)
            {
                panelClear();
                for (int i = 0; i < colors.Length; i++)
                {
                    addCheckButton(i + 1, colors[i]);
                }
            }
        }

        internal void panelClear()
        //очистка окошка (панели) со списком страниц
        {
            for (int i = panel1.Controls.Count - 1; i >= 0; i--)
            {
                panel1.Controls.Remove(panel1.Controls[i]);
            }
        }

        private void addCheckButton(int index, int state)
        //добавление кнопки-страницы в панель
        {
            CheckBox cb = new CheckBox();

            cb.Name = index.ToString("D02");
            cb.Text = index.ToString("D02");
            cb.Appearance = Appearance.Button;
            cb.Size = new Size(50, 55);
            cb.ImageList = this.colorsIcon;
            cb.TextAlign = ContentAlignment.BottomCenter;
            cb.TextImageRelation = TextImageRelation.ImageAboveText;
            cb.ImageIndex = state;
            cb.Visible = true;
            cb.Location = new Point(3 + ((index <= colors.Length / 2) ? 56 * (index - 1) : 56 * (colors.Length - index)), (index <= colors.Length / 2) ? 3 : 64);
            cb.Click += new EventHandler(stateChange);

            panel1.Controls.Add(cb);
        }

        private void stateChange(object sender, EventArgs e)
        //изменение статуса (цветности) кнопки-страницы
        {
            CheckBox cb = (CheckBox)sender;

            if (wType != "view" && wType != "delete")
                cb.ImageIndex = (cb.ImageIndex == 1 ? 0: 1);
            cb.CheckState = CheckState.Unchecked;
        }

        private void btnClose_Click(object sender, EventArgs e)
        //кнопка выхода без сохранения
        {
            this.Hide();
            Startup.myData.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ColorSettings_SizeChanged(object sender, EventArgs e)
        //если пользователь изменяет размер окна
        // не даём окну увеличиваться по вертикали
        {
            this.Height = formHeight;
        }

        internal virtual void btnSave_Click(object sender, EventArgs e)
        //событие по клику по кнопке "Сохранить" базового класса
        //проверка на правильность заполнения и формирование массива значений цветности по количеству страниц
        {
            if(String.IsNullOrEmpty(txbName.Text) || string.IsNullOrWhiteSpace(txbName.Text))
            {
                MessageBox.Show("Название схемы не может быть пустым!");
                return;
            }

            if (cmbEdition.SelectedIndex == -1)
            {
                MessageBox.Show("Не указано издание!");
                return;
            }

            if (cmbIssue.SelectedIndex == -1)
            {
                MessageBox.Show("Не указан выпуск!");
                return;
            }

            colors = new int[panel1.Controls.Count];

            for (int i = 0; i < colors.Length; i++)
            {
                CheckBox cb = (CheckBox)panel1.Controls.Find((i+1).ToString("D02"), true).FirstOrDefault();
                colors[i] = cb.ImageIndex;
            }

            DataRowView r = (DataRowView)Startup.myData.bsThird.Current;

            r.Row.SetField<DateTime>("last_access", DateTime.Now);
            r.Row.SetField<string>("comment", Startup.User);

            Startup.myData.EndEdit();

            Startup.myData.SaveColorScheme((int)cmbEdition.SelectedValue, (int)cmbIssue.SelectedValue, txbName.Text, colors);

            Startup.myData.Write_Activity(Startup.User, "color_scheme", txbName.Text);
        }

//эти функции обозначены только для переопределения в производных классах!
        internal virtual void cmbPages_DropDownClosed(object sender, EventArgs e)
        {
        }

        internal virtual void cmbScheme_DropDownClosed(object sender, EventArgs e)
        {
        }

        internal virtual void cmbScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmbEdition_DropDownClosed(object sender, EventArgs e)
        {
        }

        private void cmbEdition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEdition.SelectedIndex != -1)
                Startup.myData.bsMain.Filter = "enabled = True";// and edition_id = " + cmbEdition.SelectedValue.ToString();
        }

        internal void info_Text(string text)
        {
            timer1.Start();
            infoText.Image = Properties.Resources.Help_2;
            infoText.Text = text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            infoText.Text = "";
            infoText.Image = null;
            timer1.Stop();
        }

    }
}
