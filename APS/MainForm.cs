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
using System.Security.Principal;
using System.Media;

namespace APS
{
    public partial class MainForm : Form
    {
        Timer stripTimer;
        TimeSpan deadLineDelta = new TimeSpan(0, 5, 0);

        public int ScreenWidth = 0; //= SystemInformation.PrimaryMonitorSize.Width;
        public int ScreenHeight; //= SystemInformation.PrimaryMonitorSize.Height;

        MultiTaskForms mForm;
        ViewDB vForm;
        WorkWithTemplates tWindow;
        WorkWithPlane pWindow;

        Login lWin = new Login();

        public MainForm()
        {
            Cursor.Current = Cursors.WaitCursor;

            InitializeComponent();
            for (int i = 0; i < Screen.AllScreens.Count(); i++)
            {
                ScreenWidth = ScreenWidth + Screen.AllScreens[i].Bounds.Width;
            }
            ScreenHeight = Screen.GetBounds(this).Height;
        }

        private void MainForm_Load(object sender, EventArgs e)
//загрузка главного окна
        {
            lWin.Show();
            Startup.Settings = new RegSettings("Config");
            Size mySize = new Size((int)ScreenWidth * 2 / 3, (int)ScreenHeight * 2 / 3);

            try
            {
                this.Location = new Point (Convert.ToInt32(Startup.Settings.GetValue("position_x")), 
                    Convert.ToInt32(Startup.Settings.GetValue("position_y")));
            }
            catch (Exception){
                Startup.Settings.AddKeys("position_x", "0");
                Startup.Settings.AddKeys("position_y", "0");
                this.Location = new Point(1, 1);
            }

            try
            {
                this.Size = new Size(Convert.ToInt32(Startup.Settings.GetValue("width")),
                    Convert.ToInt32(Startup.Settings.GetValue("height")));
            }
            catch (Exception)
            {
                Startup.Settings.AddKeys("width", mySize.Width.ToString());
                Startup.Settings.AddKeys("height", mySize.Height.ToString());
                this.Size = mySize;
            }

            CheckFormSize();

            Startup.myData = new DataAccess();
            Startup.mainPlane = new DrawPlane();
            Startup.myAtex = new Atex();
/****************/
            Startup.User = WindowsIdentity.GetCurrent().Name.Substring(WindowsIdentity.GetCurrent().Name.LastIndexOf('\\') 
                + 1, WindowsIdentity.GetCurrent().Name.Length - WindowsIdentity.GetCurrent().Name.LastIndexOf('\\') - 1);
            //Startup.User = "l.doroshenko";
/****************/
            Startup.anyChanges = false;
            Startup.myData.FoundUser();

            string[] sVer = Startup.myData.GetVersion().Split('.');

            //if (Convert.ToInt32(sVer[0]) <= Convert.ToInt32(ProductVersion.ToString().Split('.')[0]))
            //    if (Convert.ToInt32(sVer[1]) <= Convert.ToInt32(ProductVersion.ToString().Split('.')[1]))
            //        if (Convert.ToInt32(sVer[2]) <= Convert.ToInt32(ProductVersion.ToString().Split('.')[2]))

            if (Startup.myData.GetVersion() != ProductVersion.ToString())
            {
                MessageBox.Show("Ваша версия приложения ниже, чем рабочая! Работа будет прекращена!");
                Application.Exit();
            }

            Startup.FillIssuesList();

            AddDateStrip();
            Prepare();
            //*************
            tabControl.SelectTab(0);
            tabControl_Selected(this, null);//заполняем listview выпусков

            Startup.mainPlane.FillSections(lstSections, Startup.Edition, "", true);

            if (Startup.UserRole == Startup.Roles.Admin)
                Startup.Edition = Startup.myData.GetEditionIDbyIssueID(Startup.myData.GetIssueIDbyName(cmbEdition.Text));
            if (Startup.UserRole == Startup.Roles.Designer)
                refreshToolStripItem_Click_1(this, EventArgs.Empty);

            RefreshPagesStatuses();

            Startup.myData.UserSession(Startup.User, true);
            Startup.myData.GetDeadline();
            deadLineTimer_Tick(this, EventArgs.Empty);

            loginLabel.Text = Startup.User + " as " + Startup.UserRole + " | ";
            StatusText("Загрузка завершена ");// + Startup.deadLine.Hours + ":" + Startup.deadLine.Minutes);
            if (Startup.UserRole == Startup.Roles.Admin || Startup.UserRole == Startup.Roles.Designer)
                deadLineTimer.Enabled = true;
            lWin.Close();
            lWin.Dispose();
            versionInfo.Text = ProductName + " " + ProductVersion;
            //issueNumLabel.Text = "№" + Startup.issueNum.ToString();
            CheckPlanesVsColors();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        //действие при показе главного окна
        {
            Cursor.Current = Cursors.Arrow;
            Startup.loginTime = DateTime.Now; //регситрация времени входа в систему
            //1.0.0.85
        }

        private void Prepare()
        //скрытие элементов верхнего меню по уровню доступа
        {
            foreach (ToolStripItem s in menuStrip.Items)
            {
                if (!s.Tag.ToString().Contains(Startup.UserRole) && !s.Tag.ToString().Contains("all"))
                    s.Visible = false;
            }
            switch (Startup.UserRole)
            {
                case Startup.Roles.Admin:
                    PathSettings();
                    break;
                case Startup.Roles.Editor:
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(1);
                    break;
                case Startup.Roles.Guest:
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(0);
                    menuStrip.ContextMenuStrip = this.rClickGuestMenu;
                    break;
                case Startup.Roles.Designer:
                    PathSettings();
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(0);
                    patchsButton.Visible = true;
                    break;
                case Startup.Roles.Circulation:
                    tabControl.TabPages.RemoveAt(1);
                    tabControl.TabPages.RemoveAt(0);
                    break;
                case Startup.Roles.Advert:
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(0);
                    break;
            }
            statusesTimer.Enabled = true;
        }

        private void btnQSearchClear_Click(object sender, EventArgs e)
        //очистка окошка быстрого поиска
        {
            txtQSearch.Text = "";
        }

        private void txtQSearch_TextChanged(object sender, EventArgs e)
        //событие изменения текста в окошке быстрого поиска
        {
            Startup.mainPlane.FillSections(lstSections, Startup.Edition, txtQSearch.Text, true);
        }

        private void tabControl_Selected(object sender, EventArgs e)
        //событие выбора панели главного окна
        {
            TableLayoutPanel mPanel = new TableLayoutPanel();

            foreach (Control s in tabControl.TabPages[tabControl.SelectedIndex].Controls)
            {
                if (s.GetType().ToString().Contains("TableLayoutPanel"))
                {
                    mPanel = (TableLayoutPanel)s;
                    break;
                }
            }

            if (mPanel.Tag.ToString() == "edit")
                Startup.mainPlane.FillSections(lstSections, Startup.Edition, "", true);

            switch (mPanel.Tag.ToString())
            {
                case "circulation":
                    FillCirculationPanel(mPanel);
                    break;
                default:
                    Startup.mainPlane.Clear();
                    Startup.mainPlane.ClearIssueFrames();
                    Startup.mainPlane = new DrawPlane(this, mPanel);
                    break;
            }
            RefreshPagesStatuses();            
        }

//**верхнее меню
        private void exitToolStripItem_Click(object sender, EventArgs e)
//выход из программы
        {
            this.Close();
        }

//раздел шаблонов выпусков
        private void deleteTemplate_Click(object sender, EventArgs e)
    //удаление шаблона выпуска
        {
            tWindow = new WorkWithTemplates("template_delete");
            tWindow.Owner = this;

            tWindow.ShowDialog();
            tWindow.Dispose();
        }

        private void addTemplate_Click(object sender, EventArgs e)
    //добавление шаблона выпуска
        {
            tWindow = new WorkWithTemplates("template_create");
            tWindow.Owner = this;

            tWindow.ShowDialog();
            tWindow.Dispose();
        }

        private void editTemplate_Click(object sender, EventArgs e)
    //редактирование шаблона выпуска
        {
            tWindow = new WorkWithTemplates("template_edit");
            tWindow.Owner = this;

            tWindow.ShowDialog();
            tWindow.Dispose();
        }

        private void copyTemplate_Click(object sender, EventArgs e)
    //создание выпуска на основе уже существующего
        {
            tWindow = new WorkWithTemplates("template_copy");
            tWindow.Owner = this;

            tWindow.ShowDialog();
            tWindow.Dispose();
        }

//раздел данных 
        private void refreshToolStripItem_Click_1(object sender, EventArgs e)
    //обновление данных из внешней базы
        {
            bool canRefresh = false;

            switch (Startup.UserRole)
            {
                case Startup.Roles.Designer:
                case Startup.Roles.Advert:
                    canRefresh = true;
                    break;
                case Startup.Roles.Editor:
                case Startup.Roles.Admin:
                    if (Startup.anyChanges)
                    {
                        canRefresh = (MessageBox.Show("Внесённые изменения не сохранены!\nСбросить и получить данные из базы?", "Внимание!",
                            MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes);
                    }
                    canRefresh = true;
                    break;
            }
            if (canRefresh)
            {
                SystemSounds.Beep.Play();
                Startup.myData.AdaptersFill();
                if (Startup.myData.RefreshTableAdapters() == null)
                {
                    toolStrip.Image = Properties.Resources.Help_2;
                    toolStrip.Text = "Данные обновлены";
                    toolStrip.Visible = true;
                }
                else
                {
                    toolStrip.Image = Properties.Resources.Help_1;
                    toolStrip.Text = "Ошибка обновления данных...";
                    toolStrip.Visible = true;
                }
                RefreshEnabledIssues();
                Startup.mainPlane.FillIssuesFrames();
                if (Startup.mainPlane.workingTable.Tag.ToString().Contains("view"))
                    RefreshPagesStatuses();
            }
            CheckPlanesVsColors();
            Startup.anyChanges = false;
            Startup.baseChanged = false;
        }

        private void saveToolStripItem_Click(object sender, EventArgs e)
    //сохранение базы из памяти во внешнюю базу
        {
            SystemSounds.Beep.Play();
            if (Startup.myData.EndEdit() == null)
            {
                toolStrip.Image = Properties.Resources.Help_2;
                toolStrip.Text = "Данные сохранены";
                toolStrip.Visible = true;
            }
            else
            {
                toolStrip.Image = Properties.Resources.Help_1;
                toolStrip.Text = "Ошибка сохранения данных...";
                toolStrip.Visible = true;
            }
            Startup.anyChanges = false;
            saveMenuItem.Enabled = false;
        }

        private void viewActivityToolStripItem_Click(object sender, EventArgs e)
    //просмотр списка активности пользователей
        {
            vForm = new ViewDB("view_activity");
            vForm.Owner = this;
            vForm.Text = "Просмотр активности";

            vForm.ShowDialog();
            vForm.Dispose();
        }

        internal void StatusText(string text)
        //надпись в статусной строке
        {
            toolStrip.Text = text;
            toolStrip.Image = Properties.Resources.Help_2;
            toolStrip.Visible = true;
        }

        internal void StatusText(string text, int critical)
        //надпись в статусной строке
        {
            toolStrip.Text = text;
            toolStrip.Image = Properties.Resources.Help_1;
            toolStrip.Visible = true;
            System.Media.SystemSounds.Beep.Play();
        }

        private void toolStrip_VisibleChanged(object sender, EventArgs e)
//???
        {
            if (toolStrip.Visible.Equals(true))
            {
                stripTimer = new Timer();
                stripTimer.Interval = 3000;
                stripTimer.Start();
                stripTimer.Tick += new EventHandler(timer1_Tick);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
    //таймер для обновления информационной строки 
        {
            toolStrip.Visible = false;
            stripTimer.Stop();
            stripTimer.Dispose();
            stripTimer = null;
        }

//раздел планов
        private void standardPlane_Click(object sender, EventArgs e)
    //создание общего плана на неделю
        {
            pWindow = new WorkWithPlane("main_plane");
            pWindow.myOwner = this;
            pWindow.Text = "Общий план";

            pWindow.ShowDialog();
            pWindow.Dispose();
        }

        private void datePlane_Click(object sender, EventArgs e)
    //создание плана на конкретную дату
        {
            OneDayPlane oForm = new OneDayPlane();
            oForm.Owner = this;
            oForm.Text = "План на дату";

            oForm.ShowDialog();
            oForm.Dispose();
        }

//раздел изданий
        private void addEdition_Click_1(object sender, EventArgs e)
    //создание издания
        {
            mForm = new MultiTaskForms("edition_create");
            mForm.Owner = this;
            mForm.Text = "Добавление издания";

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void viewEdition_Click(object sender, EventArgs e)
    //просмотр списка изданий
        {
            vForm = new ViewDB("editions_view");
            vForm.Owner = this;

            vForm.ShowDialog();
            vForm.Dispose();
        }

        private void editEdition_Click(object sender, EventArgs e)
    //изменение издания
        {
            mForm = new MultiTaskForms("edition_edit");
            mForm.Owner = this;
            mForm.Text = "Изменение издания";

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void deleteEdition_Click(object sender, EventArgs e)
    //удаление издания
        {
            mForm = new MultiTaskForms("edition_delete");
            mForm.Owner = this;
            mForm.Text = "Удаление издания";

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void cmbEdition_DropDownClosed(object sender, EventArgs e)
    //событие смены выпуска
        {
            Startup.Edition = Startup.myData.GetIssueIDbyName(cmbEdition.Text);
            Startup.mainPlane.FillSections(lstSections, Startup.Edition, "", true);
        }

        private void dtOneDayPlanePicker_ValueChanged(object sender, EventArgs e)
    //событие смены даты
        {
            Cursor myCurr = this.Cursor;

            this.Cursor = Cursors.WaitCursor;

            Startup.globalDate = Convert.ToDateTime(menuStrip.Items[0].Text);

            tabControl_Selected(this, EventArgs.Empty);

            this.Cursor = myCurr;
            menuStrip.Focus();
            StatusText("Дата изменена.");
            Startup.myData.GetCurrentIssueNum();
        }

//блок управления Drag-n-Drop секций        
        private void lstSections_MouseDown(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseDown((ListView)sender, e);
        }

        private void lstSections_MouseUp(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseUp((ListView)sender, e);
        }

        private void dragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void lstSections_MouseMove(object sender, MouseEventArgs e)
        {
            Startup.mainPlane.dnd_MouseMove((ListView)sender, e);
        }

//---------->
        private void dayTemplateSaveToolStripMenuItem_Click(object sender, EventArgs e)
//сохранение отредактированного плана
        {
            Cursor t = this.Cursor;

            this.Cursor = Cursors.WaitCursor;
            //if (Startup.issueNum == -1)
            //{
            //    Startup.issueNum = Convert.ToInt32(Startup.globalDate.Year.ToString() + Startup.globalDate.Month.ToString("d02") + Startup.globalDate.Day.ToString("d02"));
            //    Startup.issueBigNum = Convert.ToInt32(Startup.globalDate.Year.ToString() + Startup.globalDate.Month.ToString("d02") + Startup.globalDate.Day.ToString("d02"));
            //    //Startup.myData.SetIssueNum();
            //    //Startup.myData.GetIssueNum();

            //}

            foreach (Control s in this.tabControl.TabPages[tabControl.SelectedIndex].Controls)
            {
                if (s.GetType().ToString().ToLower().Contains("layoutpanel"))
                {
                    TableLayoutPanel tblP = (TableLayoutPanel)s;

                    for (int k = 0; k < tblP.ColumnCount; k++)
                    {
                        try
                        {
                            CheckBox cb = (CheckBox)tblP.GetControlFromPosition(k, 0);
                            ListView lv = (ListView)tblP.GetControlFromPosition(k, 1);
                            if (cb.CheckState == CheckState.Checked)
                            {
                                Startup.myData.SaveDTDplane(Startup.globalDate, cb.Text, lv.Items);
                                //Startup.mainPlane.ClearCrossLinks(Startup.globalDate, cb.Text);
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            Startup.myData.EndEdit();
            StatusText("Данные сохранены");
            Startup.myData.Write_Activity(Startup.User, "edit_plane", Startup.globalDate.ToShortDateString());
            SystemSounds.Beep.Play();
            Startup.anyChanges = false;
            saveMenuItem.Enabled = false;
            this.Cursor = t;
        }

        private void usersToolStripItem_Click(object sender, EventArgs e)
//работа с пользователями
        {
            Users uWindow = new Users();

            uWindow.ShowDialog();
        }

//меню по правой кнопке мышки
        private void dubleMenu_Click(object sender, EventArgs e)
        //дублирование выбранной полосы
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            if (tsi != null)
            {
                ContextMenuStrip owner = (ContextMenuStrip)tsi.Owner;
                if (owner != null)
                {
                    ListView s = (ListView)owner.SourceControl;
                    if (Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), Convert.ToInt32(s.Tag.ToString())) == 0)
                    {
                        StatusText("Необходимо сохранить шаблон!", 1);
                        return;
                    }

                    //StatusText(" X - " + Cursor.Position.X + " | Y - " + Cursor.Position.Y + " | W - " + Screen.GetBounds(this).Width + " | H - " + Screen.GetBounds(this).Height);
                    Startup.mainPlane.Double(s);
                }
            }
        }

        private void commentMenu_Click(object sender, EventArgs e)
        //работа с комментарием
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            if (tsi != null)
            {
                ContextMenuStrip owner = (ContextMenuStrip)tsi.Owner;
                if (owner != null)
                {
                    ListView s = (ListView)owner.SourceControl;
                    if (Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), Convert.ToInt32(s.Tag.ToString())) == 0)
                    {
                        StatusText("Необходимо сохранить шаблон!", 1);
                        return;
                    }

                    Startup.mainPlane.Comment(s);
                }
            }
        }

        private void planeMenu_Click(object sender, EventArgs e)
        //выбрать план из стандартных
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            if (tsi != null)
            {
                ContextMenuStrip owner = (ContextMenuStrip)tsi.Owner;
                if (owner != null)
                {
                    ListView s = (ListView)owner.SourceControl;
                    Startup.mainPlane.ChoisePlane(s);
                }
            }
        }

        private void savePlaneMenu_Click(object sender, EventArgs e)
        //сохранить план как стандартный
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            if (tsi != null)
            {
                ContextMenuStrip owner = (ContextMenuStrip)tsi.Owner;
                if (owner != null)
                {
                    ListView s = (ListView)owner.SourceControl;
                    Startup.mainPlane.SavePlaneAsStandard(s);

                    toolStrip.Image = Properties.Resources.Help_2;
                    toolStrip.Text = "Данные обновлены";
                    toolStrip.Visible = true;
                    SystemSounds.Asterisk.Play();
                }
            }
        }

        private void statusesTimer_Tick(object sender, EventArgs e)
        //таймер для отслеживания изменения статусов полос из Атекса
        {
            Cursor myCurr = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            RefreshPagesStatuses();
            this.Cursor = myCurr;
        }

        internal void RefreshPagesStatuses()
        //событие обновления статусов полос
        {
            if (!Startup.mainPlane.workingTable.Tag.ToString().Contains("view") || Startup.UserRole == Startup.Roles.Guest)
                return;

            ImageList iconsList;

            if (Startup.UserRole == Startup.Roles.Advert)
            {
                iconsList = this.colorStates;
            }
            else
            {

                iconsList = this.stateIndexes;
            }
                Startup.myAtex.GetStatuses();
            StatusText("Обновление статусов");

            int c = Startup.mainPlane.workingTable.ColumnCount;
            for (int i = 0; i < c; i++)
            {
                ListView lv = new ListView();
                lv = (ListView)Startup.mainPlane.workingTable.GetControlFromPosition(i, Startup.mainPlane.workingTable.RowCount-1);
                lv.StateImageList = iconsList;
                foreach (ListViewItem li in lv.Items)
                {
                    int index = Startup.myAtex.statusesList.FindIndex(p => p.PageNum == Convert.ToInt32(li.Text) && p.IssueId == Convert.ToInt32(lv.Tag.ToString()));
                    try
                    {
                        li.StateImageIndex = Startup.myAtex.statusesList[index].Status;
                        if (li.StateImageIndex > 5)
                        {
                            li.SubItems[1].BackColor = Color.Black;
                            li.SubItems[1].ForeColor = Color.White;
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private void rClickGuestMenu_Opening(object sender, CancelEventArgs e)
        {
            if (Startup.UserRole == Startup.Roles.Guest)
                return;
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            AddUserForm uForm = new AddUserForm();

            uForm.ShowDialog();
        }

//****** всякое
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        //событие закрытия формы
        //если есть изменения, спаршивает, закрывать ли без сохранения
        {
            if (Startup.anyChanges)
            {
                if (MessageBox.Show("Выйти без сохранения изменений?", "Внимание!",
                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Startup.myData.UserSession(Startup.User, false);
            Startup.Settings.ChangeValue("position_x", this.Location.X.ToString());
            Startup.Settings.ChangeValue("position_y", this.Location.Y.ToString());
            Startup.Settings.ChangeValue("width", this.Size.Width.ToString());
            Startup.Settings.ChangeValue("height", this.Size.Height.ToString());
            Startup.Settings.WriteINI();
        }

        private void changeView_Click(object sender, EventArgs e)
        {
            Startup.mainPlane.ChangeView_Click();
        }

//управление цветностью
        private void addColorSchemeMenuItem_Click(object sender, EventArgs e)
        {
            ColorSettings_New wColors = new ColorSettings_New();

            wColors.Text = "Создание схемы цветности";

            wColors.ShowDialog();
        }

        private void editColorSchemeMenuItem_Click(object sender, EventArgs e)
        {
            ColorSettings_Edit eColors = new ColorSettings_Edit();

            eColors.myOwner = this;
            eColors.Text = "Изменение схемы цветности";

            eColors.ShowDialog();
        }

        private void viewColorSchemeMenuItem_Click(object sender, EventArgs e)
        {
            ColorSettings_View vColors = new ColorSettings_View();
            vColors.myOwner = this;
            vColors.Text = "Просмотр схемы цветности";

            vColors.ShowDialog();
        }

        private void deleteColorSchemeMenuItem_Click(object sender, EventArgs e)
        {
            ColorSettings_Delete dColors = new ColorSettings_Delete();

            dColors.Location = Startup.Location(dColors);

            dColors.Text = "Delete color Scheme";

            dColors.ShowDialog();
        }

        private void colorsShablonesStripMenuItem_Click(object sender, EventArgs e)
        {
            pWindow = new WorkWithPlane("main_color");

            pWindow.myOwner = this;
            pWindow.Text = "План цветности";

            pWindow.ShowDialog();
            pWindow.Dispose();
        }

        private void viewColorsByDays_Click(object sender, EventArgs e)
        {
            pWindow = new WorkWithPlane("view_color");

            pWindow.myOwner = this;
            pWindow.Text = "План цветности";
            foreach (Control c in pWindow.Controls)
            {
                if (!c.Text.Equals("Вернуться"))
                    c.Enabled = false;
            }
            pWindow.ShowDialog();
            pWindow.Dispose();
        }

        private void dbChangesTimer_Tick(object sender, EventArgs e) //1.0.0.85
        //таймер для запуска процесса проверки обновления базы
        {
            if (!Startup.myData.checkDB.IsBusy)
                Startup.myData.checkDB.RunWorkerAsync();
            StatusText("Проверка базы на обновления");
            if (Startup.baseChanged)
            {
                //MessageBox.Show("В базу данных были внесены изменения.\nТребется выполнить обновление.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StatusText("В базу данных были внесены изменения. Требется выполнить обновление.");
            }
        }

        private void deadlinesStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkWithPlane dWindow = new WorkWithPlane("main_deadline");

            dWindow.Show();
        }

        private void weekPlanes_Click(object sender, EventArgs e)
        {
            pWindow = new WorkWithPlane("main_plane");
            pWindow.myOwner = this;
            pWindow.Text = "Общий план";

            pWindow.ShowDialog();
            pWindow.Dispose();
        }

        private void deadLineTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan curTime = DateTime.Now.TimeOfDay;
            string s = "";
            TimeSpan dT = new TimeSpan(23, 59, 00);

            StatusText("Check Deadlines...");

            foreach (Startup.issue issue in Startup.issuesList)
            {
                if (issue.DeadLine < curTime + deadLineDelta && issue.DeadLine != TimeSpan.Zero && issue.Active)
                {
                    int j = issue.Pages - Startup.myData.CountSendedPDF(Startup.issueDate() + "_SEG_" + issue.Code);
                    if (j > 0)
                        s = s + issue.Name + " - " + j + "\n";
                    if(issue.DeadLine < dT)
                        dT = issue.DeadLine;
                }
            }

            if (s != "")
            {
                try
                {
                    ((System.Windows.Forms.Timer)sender).Enabled = false;
                }
                catch { }
                int period = (dT - curTime + deadLineDelta).Minutes;
                if (period >= 0)
                    s = "Осталось " + period + " минут до дедлайна.\nНе отправлено следующее количество полос:\n" + s;
                else
                    s = "Прошло " + Math.Abs(period) + " минут после дедлайна.\nНе отправлено следующее количество полос:\n" + s;
                DialogResult dr = MessageBox.Show(new NativeWindow(), s, "Внимание!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand);
                SystemSounds.Beep.Play();
                try
                {
                    if (dr == System.Windows.Forms.DialogResult.Retry)
                    {
                        ((System.Windows.Forms.Timer)sender).Enabled = true;
                    }
                    else
                        StatusText("Таймер остановлен.");


                }
                catch { }
            }

        }

        private void RefreshEnabledIssues()
        {
            foreach (Startup.issue issue in Startup.issuesList)
            {
                issue.Active = Startup.myData.CheckActiveIssue(issue.Id);
            }
        }

        private void timerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deadLineTimer_Tick(this, EventArgs.Empty);
        }

        private void exportXMLToolStripItem_Click(object sender, EventArgs e)
        {
            ExportXML mExportXML = new ExportXML(this);
        }

    }
}
