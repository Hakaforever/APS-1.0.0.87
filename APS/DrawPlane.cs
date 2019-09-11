using System;
using System.Collections;
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
    class DrawPlane
    //предполагается, что тут будем рисовать все окошки с данными
    {
        public MainForm myOwner;

        internal TableLayoutPanel workingTable;

        internal string wType;

//это нужно для сбора информации о дублировании полос
        internal struct pInfo
        //структура с информацией о дублируемой полосе
        {
            internal int issueId { get; set; }
            internal int planeId { get; set; }
            internal string issueName { get; set; }
            internal int pageNum { get; set; }
            internal int pageCount { get; set; }
        }

        public class DblPageInfo
        //собираем всё в класс
        {
            internal int sourceSectionId { get; set; }
            //internal int planeId { get; set; }
            internal List<pInfo> targetPageInfo { get; set; }

            internal DblPageInfo()
            {
                sourceSectionId = -1;
                //planeId = -1;
                targetPageInfo = new List<pInfo>();
            }

            internal void Clear()
            { 
                sourceSectionId = -1;
                //planeId = -1;
                targetPageInfo.Clear();
            }
        }

//а это нужно для сбора информации о дублировании полосы
        public class SendPageInfo
        {
            public int initialIssueId;
            public List<pInfo> pagesInfo;
            public string initialFileName;
            public string inFolder;
            public List<string> outFiles = new List<string>();

            internal struct pInfo
            {
                public int issueId { get; set; }
                public int pageNum { get; set; }
                public string issueCode { get; set; }
                public int isColor { get; set; }
                public bool isChecked { get; set; }
                public string outFileName { get; set; }
                public string sectionCode { get; set; }
            }

            public SendPageInfo(int issId, int pNum)
            {
                initialIssueId = issId;
                pagesInfo = new List<pInfo>();
                for (int i = 0; i < Startup.issuesList.Count; i++)
                {
                    Add(Startup.issuesList[i].Id, pNum);
                }
                initialFileName = Startup.issueDate() + "_" + Startup.myData.GetEditionCodebyID(Startup.Edition) + "_" + Startup.myData.GetIssueCodebyID(issId) + "_" + pNum.ToString("D02") + ".PDF";
            }

            public void Add(int issId, int pNum)
            {
                pInfo p = new pInfo();
                p.issueId = issId;
                //int x = Startup.myData.GetLinkPageNum(initialIssueId, -1, pNum, issId, Startup.globalDate);
                int x = Startup.myData.GetLinkPageNum(issId, -1, pNum, initialIssueId, Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), issId));
                p.pageNum = (x == -1 ? pNum : x);
                p.isColor = Startup.myData.GetColorForPage(p.pageNum, issId, (int)Startup.globalDate.DayOfWeek);
                p.issueCode = Startup.myData.GetIssueCodebyID(issId);
                p.isChecked = (x != -1 ? true : (issId == initialIssueId ? true : false));
                pagesInfo.Add(p);
            }

            public void Close()
            {
                foreach (pInfo p in pagesInfo)
                {
                    if (p.isChecked)
                        outFiles.Add(Startup.issueDate() + "_" + Startup.myData.GetEditionCodebyID(Startup.Edition) + "_" + p.issueCode 
                            + "_" + p.pageNum.ToString("D02") + (p.isColor == 1 ? "K" : "") + ".PDF");
                }
            }
        }

        internal class DnDLocation
//класс, необходимый для прорисовки секции
        {
            internal Rectangle ghotstRectangle { get; set; } //область для прорисовки прямоугольника под курсором
            internal int sourceSection { get; set; }         //номер элемента в списке окна-источника
            internal int sourceIssue { get; set; }           //
            internal int targetSection { get; set; }         //номер элемента в списке окна-цели
            internal Point screenOffset { get; set; }        //смещение
            internal ListViewItem specItem { get; set; }     //элемент ListViewItem на замену
            internal ListView listView { get; set; }         //окно, в которое перетаскиваем

            internal DnDLocation() //инициализация
            {
                this.Reset();
            }

            internal void Reset()
            {
                sourceSection = -1;
                sourceIssue = -1;
                targetSection = -1;
                specItem = null;
                ghotstRectangle = Rectangle.Empty;
                screenOffset = Point.Empty;
                listView = null;
            }
        }

        DnDLocation dndPoint = new DnDLocation();
        //объект этого класса

        BindingSource Bs_One = new BindingSource();

        public void Clear()
//очистка экземпляра класса
        {
            Bs_One = null;
            dndPoint.Reset();
            workingTable = null;
            //Startup.issuesList.Clear();
            myOwner = null;
            wType = null;
        }

        public DrawPlane()
        { }

        public DrawPlane(MainForm inOwner, TableLayoutPanel inTblLayout)
//инициализатор
        {
            myOwner = inOwner;
            switch (inTblLayout.Tag.ToString())
            {
                case "edit":
                    wType = "template_edit";
                    break;
                case "circulation":
                    wType = "circulation";
                    break;
                default:
                    wType = "template_view";
                    break;
            }
            workingTable = inTblLayout;
            workingTable.Visible = false;
            Cursor.Current = Cursors.WaitCursor;

            Bs_One.DataSource = Startup.myData.mainDBdataset;
            Bs_One.DataMember = "Issues";
            Bs_One.Filter = "edition_id = " + Startup.Edition;

            myOwner.cmbEdition.DataSource = Bs_One;
            myOwner.cmbEdition.DisplayMember = "Name";
            myOwner.cmbEdition.ValueMember = "id";

            FillIssuesFrames();
            Cursor.Current = Cursors.Arrow;
            workingTable.Visible = true;
        }

        internal bool FillSections(ListView inListBox, object issueId, string mask, bool nonAtex)
//заполнение списка секций. входят: 
//inListBox - окно, в котором выдаем список секций,
//issueId - SelectedValue из comboBox списка выпусков
//mask - маска для поиска секции
//nonAtex - если true показывать только "ручные" секции
        {
            inListBox.Items.Clear();

            List<ListViewItem> g;

            try
            {
                g = (mask == "" ? g = Startup.myData.GetActiveSections(Startup.myData.GetEditionIDbyIssueID(Startup.Edition), nonAtex) : Startup.myData.GetSectionsByMask(mask.ToLower(), Startup.myData.GetEditionIDbyIssueID(Startup.Edition), nonAtex));
                for (int y = 0; y < g.Count; y++)
                {
                    g[y].Text = g[y].ToolTipText;
                    inListBox.Items.Add(g[y]);
                }
                inListBox.LargeImageList = Startup.myData.iconList;
                return true;
            }
            catch (Exception)
            {
                g = null;
            }
            return false;
        }

        public void ClearIssueFrames()
//перезаливка окон выпусков
        {
            if (workingTable == null) return;
            for (int i = workingTable.ColumnCount - 1; i >= 0; i--)
            {
                for (int j = 0; j < workingTable.RowCount; j++)
                {
                    var ctrl = workingTable.GetControlFromPosition(i, j);
                    workingTable.Controls.Remove(ctrl);
                }
                workingTable.ColumnStyles.RemoveAt(i);
                workingTable.ColumnCount--;
            }
        }

        public void FillIssuesFrames()
//создаем таблицу выпусков, закидываем названия выпусков и listView для показа страничек выпусков
//и заполняем listView по схеме
        {
            if (workingTable == null) return;

            int t = 0;

            ClearIssueFrames(); //очищаем заполненные блоки

            workingTable.ColumnCount = Startup.issuesList.Count;

            for (int i = 0; i < Startup.issuesList.Count; i++)
            {
                CheckBox lb = new CheckBox();
                lb.Text = Startup.issuesList[i].Name;
                lb.Padding = new Padding(10, 0, 0, 0);
                lb.Size = new Size(120, 18);
                //*******
                lb.Click += new EventHandler(CheckBox_Click);

                Label ll = new Label();
                ll.Text = Startup.issuesList[i].Name;
                ll.Padding = new Padding(3);
                ll.Size = new Size(120, 18);

                ListView lv = new ListView();
                lv.Dock = DockStyle.Fill;
                lv.Tag = Startup.issuesList[i].Id;
                lv.ShowItemToolTips = true;
                lv.Enabled = false;
                
                //switch (Startup.UserRole)
                switch (workingTable.Tag.ToString())
                { 
                    //case Startup.Roles.Admin:
                    //case Startup.Roles.Editor:
                    case "edit":
                        lv.View = View.LargeIcon;
                        lv.LargeImageList = Startup.myData.iconSmallList;
                        lv.Font = new System.Drawing.Font("Microsoft Sans Serif", 12);
                        lv.HideSelection = false;
                        lv.AllowDrop = true;
                        lv.MouseMove += new MouseEventHandler(lstIssue_mouseMove);
                        lv.MouseUp += new MouseEventHandler(lstIssue_MouseUp);
                        lv.MouseDown += new MouseEventHandler(lstIssue_MouseDown);
                        lv.DragDrop += new DragEventHandler(lstIssue_DragDrop);
                        lv.DragOver += new DragEventHandler(lstIssue_DragOver);
                        lv.MultiSelect = true;
                        break;
                    //case Startup.Roles.Designer:
                    case "view":
                        lv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
                        //lv.ContextMenuStrip.Tag = Startup.issuesList[i].Name;
                        lv.View = View.Details;
                        lv.StateImageList = myOwner.stateIndexes;
                        lv.Columns.Add("", 30, HorizontalAlignment.Left);
                        lv.Columns.Add("", 30, HorizontalAlignment.Left);
                        lv.Columns.Add("Секция", 110, HorizontalAlignment.Left);
                        lv.Columns.Add("Цветность", 0, HorizontalAlignment.Left);
                        lv.Columns.Add("Статус", 25, HorizontalAlignment.Left);
                        lv.Columns.Add("Комментарий", 25, HorizontalAlignment.Left);
                        lv.HeaderStyle = ColumnHeaderStyle.None;
                        lv.FullRowSelect = true;
                        lv.DoubleClick += new EventHandler(myOwner.viewPDF_Click);
                        break;
                }

                //получаем имя плана для дальнейшего набора страниц выпуска
                List<string> gen = Startup.myData.GetTemplate(Startup.globalDate.Date, Startup.issuesList[i].Id);
                //и рисуем его
                if (gen.Any())
                {
                    FillIssue(lv, wType, -1, gen[0], Startup.issuesList[i].Id); //заполнение с учётом дубликатов
                    if (gen[1] == "day")
                    {
                        lb.CheckState = CheckState.Checked;
                        lv.Enabled = true;
                        lv.LargeImageList = Startup.myData.iconList; //если чекбокс включен - сменяем нбор иконок на полноцветный
                    }
                }

                switch (Startup.UserRole)
                { 
                    case Startup.Roles.Admin:
                        switch (workingTable.Tag.ToString())
                        { 
                            case "edit":
                                lv.ContextMenuStrip = myOwner.rClickEditorMenu;
                                break;
                            case "view":
                                lv.ContextMenuStrip = myOwner.rClickDesignerMenu;
                                break;
                            case "advert":
                                lv.ContextMenuStrip = myOwner.rClickAdvertMenu;
                                break;
                        }
                        break;
                    case Startup.Roles.Editor:
                        lv.ContextMenuStrip = myOwner.rClickEditorMenu;
                        break;
                    case Startup.Roles.Designer:
                        lv.ContextMenuStrip = myOwner.rClickDesignerMenu;
                        break;
                    case Startup.Roles.Advert:
                        lv.ContextMenuStrip = myOwner.rClickAdvertMenu;
                        break;
                }
                //lv.ContextMenuStrip.Tag = lv;
                Startup.issuesList[i].Pages = lv.Items.Count;
                int ColumnWidth = 270;
                if (Startup.UserRole == Startup.Roles.Designer && !Startup.issuesList[i].Active) ColumnWidth = 0;
                workingTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ColumnWidth));
         
                if (wType == "template_edit")
                {
                    workingTable.Controls.Add(lb, i, 0);
                }
                else
                {
                    workingTable.Controls.Add(ll, i, 0);
                }
                workingTable.Controls.Add(lv, i, workingTable.RowCount - 1);
                t = t + 250;
                myOwner.issueNumLabel.Text = lv.ClientRectangle.Width.ToString();
            }
            //myOwner.issueNumLabel.Text = workingTable.Width.ToString();
        }

        internal void FillDouble(ref ListView inList, int issueId)
//заполняет окошки выпусков полосами, которые дублируются между выпусками
        {
            List<ListViewItem> doublesPages = Startup.myData.GetLinks(Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), issueId), issueId);

            for (int i = 0; i < doublesPages.Count; i++)
            {
                int index = inList.Items.IndexOf(inList.FindItemWithText(doublesPages[i].Text.Trim()));
                try
                {
                    inList.Items[index].Name = doublesPages[i].Name;
                    inList.Items[index].Text = (Startup.UserRole == Startup.Roles.Designer ? doublesPages[i].Text.Trim(new char[]{' '}) : doublesPages[i].Text);
                    //inList.Items[index].ToolTipText = doublesPages[i].ToolTipText;
                    inList.Items[index].ImageKey = doublesPages[i].ImageKey.Substring(0, 3) + (Startup.myData.GetColorForPage(Convert.ToInt32(inList.Items[index].Text), issueId, (int)Startup.globalDate.DayOfWeek) == 0 ? "BW" : "");
                    inList.Items[index].SubItems[4].Text = "Д";
                }
                catch (Exception ex){ }
            }
            return;
        }

        internal void Comment(object sender)
//создание окошка для работы с комментарием к полосе внутри выпуска одного дня
        {
            ListView m = sender as ListView;

            if (m.SelectedItems.Count != 0)
            {
                CommentWindow wComment = new CommentWindow();
                wComment.Location = Startup.Location(wComment);

                for (int i = 0; i < m.SelectedItems.Count; i++)
                {
                    if (m.SelectedItems[i].Name != "")
                    {
                        wComment.wType = m.View.ToString();
                        wComment.Text = "Комментарий к полосе " + Convert.ToInt16(m.SelectedItems[i].Text) + ", выпуск " + Startup.myData.GetIssuesNamebyID(Convert.ToInt32(m.Tag));
                        wComment.comment = m.SelectedItems[i].Tag.ToString();

                        if (wComment.ShowDialog() == DialogResult.OK)
                        {
                            Startup.myData.UpdateComment(Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), Convert.ToInt32(m.Tag.ToString())), Convert.ToInt32(m.Tag), Convert.ToInt32(m.SelectedItems[i].Text), wComment.comment);
                            if (!String.IsNullOrEmpty(wComment.comment) || !String.IsNullOrWhiteSpace(wComment.comment))
                            {
                                m.SelectedItems[i].Tag = wComment.comment;
                                m.SelectedItems[i].ToolTipText = m.SelectedItems[i].Name + "\n" + wComment.comment;
                                m.SelectedItems[i].SubItems[5].Text = "!";
                                m.SelectedItems[i].BackColor = Color.LightPink;
                            }
                        }
                    }
                    else
                    {
                        myOwner.StatusText("Комментарий для пустой страницы невозможен!", 1);
                    }
                }
            }
        }

        internal void ChoisePlane(object sender)
//вызов правой клавишей мышки окошка для выбора стандартного плана
        {
            ListView lv = sender as ListView;
            SelectFromCombo cPlane = new SelectFromCombo((int)lv.Tag);
            cPlane.Location = Startup.Location(cPlane);

            if (cPlane.ShowDialog() == DialogResult.OK)
            {
                if (cPlane.cmbSelect.SelectedIndex == -1) return;
                int pCount = Startup.myData.GetTemplatePages(cPlane.cmbSelect.Text, (int)lv.Tag);
                FillIssue(lv, "", pCount, cPlane.cmbSelect.Text, (int)lv.Tag);
                Startup.anyChanges = true;
                myOwner.saveMenuItem.Enabled = true;
            }
        }

        internal void SavePlaneAsStandard(object sender)
//вызов правой клавишей мышки окошка для записи плана как стандартного
        {
            ListView lv = sender as ListView;
            InputTextWin sWin = new InputTextWin((int)lv.Tag);
            sWin.Location = Startup.Location(sWin);
            sWin.Text = "Сохранить план как стандартный";

            if (sWin.ShowDialog() == DialogResult.OK)
            {
                string[] tmp = lv.Items.OfType<ListViewItem>().Select(item => item.Name).ToArray();
                tmp = tmp.Where((x, i) => (i % 2) == 0).ToArray().Concat(tmp.Where((x, i) => (i % 2) == 1).Reverse()).ToArray();
                Startup.myData.UpdatePlane(sWin.namePlane.Text, (int)lv.Tag, tmp, false);
            }
        }

        internal string Scatter_Files(SendPageInfo inFile)
        //разбрасываем PDF-файл по папкам
        {
            string outPath;
            System.IO.FileStream fs = null;

            //foreach (string p in paths)
            //{
            //    if (System.IO.Directory.Exists(p))
            //    //проверяем папки на наличие
            //    {
            //        foreach (string outFile in inFile.outFiles)
            //        {
            //            if (System.IO.File.Exists(System.IO.Path.Combine(p, outFile)))
            //            //проверяем папки на наличие такого файла
            //            {
            //                MessageBox.Show("File " + outFile + " exist in " + p + "!");
            //                return "Exist";
            //            }
            //            //System.IO.FileStream fs = null;
            try
            //проверяем исходный файл на то, что он никем не открыт
            {
                fs = System.IO.File.Open(System.IO.Path.Combine(inFile.inFolder, inFile.initialFileName), System.IO.FileMode.Open);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Невозможно отправить " + inFile.initialFileName + "!\nУбедитесь, что он не откырт для просмотра \nВами или кем-то другим и повторите отправку.", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return "Close Acrobat";
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            //      }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Directory " + p + " is not exist!");
            //        return "Directory not exist";
            //    }
            //}

            foreach (string f in inFile.outFiles)
            {
                if (f.Contains("K.PDF"))
                    outPath = Startup.Paths.GetValue("outputPDF");
                else
                    outPath = Startup.Paths.GetValue("recolorPDF");

                try
                {
                    System.IO.File.Copy(System.IO.Path.Combine(inFile.inFolder, inFile.initialFileName), System.IO.Path.Combine(outPath, f));
                    Startup.myData.SendPDF(System.IO.Path.GetFileNameWithoutExtension(f).Substring(0, 17), System.IO.Path.GetFileNameWithoutExtension(f).Substring(0, 17) == System.IO.Path.GetFileNameWithoutExtension(inFile.initialFileName).Substring(0, 17));
                }
                catch (Exception ex)
                {
                    return "Copy error";
                }
            }

            if (inFile.inFolder != Startup.Paths.GetValue("archivePDF"))
            {
                try
                {
                    System.IO.File.Move(System.IO.Path.Combine(inFile.inFolder, inFile.initialFileName),
                        System.IO.Path.Combine(Startup.Paths.GetValue("archivePDF"), Startup.issueNum.ToString(), inFile.initialFileName));
                }
                catch (System.IO.IOException)
                {
                    try
                    {
                        System.IO.File.Replace(System.IO.Path.Combine(inFile.inFolder, inFile.initialFileName),
                        System.IO.Path.Combine(Startup.Paths.GetValue("archivePDF"), Startup.issueNum.ToString(), inFile.initialFileName), null);
                    }
                    catch (System.IO.IOException)
                    {
                        myOwner.StatusText("Возможно в архивной папке файл открыт кем-то.");
                        return "Archive error";
                    }
                }
            }
            return null;
        }

        internal void SendPDF(ListView m)
        //создание правой клавишей мышки окошка для дублирования полосы внутри выпусков одного дня
        {
            if (Startup.issueNum == -1)
            {
                IssueNum iWin = new IssueNum();
                DialogResult dr = iWin.ShowDialog();
                iWin.Dispose();
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show("Невозможно продолжить работу!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (m.SelectedItems.Count != 0)
            {
                for (int f = 0; f < m.SelectedItems.Count; f++)
                {
                    ListViewItem tempPnum = m.SelectedItems[f];
                    DrawPlane.SendPageInfo sendClass = new DrawPlane.SendPageInfo((int)m.Tag, Convert.ToInt32(tempPnum.Text));
                    SendPage wSend = new SendPage(ref sendClass);
                    string result;
                    wSend.Location = Startup.Location(wSend);


                    wSend.Text = "Отправить полосу " + tempPnum.Text.Trim();

                    string inFolder = CheckSourceFile(sendClass.initialFileName);
                    string archPDF = CheckOldFolder();

                    if (String.IsNullOrEmpty(archPDF) || String.IsNullOrEmpty(inFolder))
                        break;
                    sendClass.inFolder = System.IO.Path.GetDirectoryName(inFolder);
                    if (wSend.ShowDialog() == DialogResult.OK)
                    {
                        result = Scatter_Files(sendClass);
                    }
                }
            }
        }

        private string CheckOldFolder()
        //проверяем на наличие папку для архивирования PDF
        { 
            if (!System.IO.Directory.Exists(Startup.Paths.GetValue("archivePDF") + "\\" + Startup.issueNum))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Startup.Paths.GetValue("archivePDF") + "\\" + Startup.issueNum);
                }
                catch (Exception)
                {
                    myOwner.StatusText("Ошибка создания папки для архивирования PDF!");
                    return "";
                }
            }
            return System.IO.Path.Combine(Startup.Paths.GetValue("archivePDF"), Startup.issueNum.ToString());
        }

        private string CheckSourceFile(string fileName)
        //проверяем не лежит ли исходный файл в архивной папке
        {
            string f1 = System.IO.Path.Combine(Startup.Paths.GetValue("inputPDF"), fileName);
            string f2 = System.IO.Path.Combine(Startup.Paths.GetValue("archivePDF") + "\\", Startup.issueNum.ToString(), fileName);
            if (!System.IO.File.Exists(f1))
            {
                if (System.IO.File.Exists(f2))
                {
                    if (MessageBox.Show("Файл " + fileName + " находится в архиве. Отправить повторно?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        return f2;
                }
                return "";
            }

            return f1;
        }

        internal void Double(object sender)
//создание правой клавишей мышки окошка для дублирования полосы внутри выпусков одного дня
        {
            ListView m = (ListView)sender;

            if (m.SelectedItems.Count != 0)
            {
                DblPageInfo pageToDouble = new DblPageInfo();

                for (int f = 0; f < m.SelectedItems.Count; f++)
                {
                    ListViewItem tempPnum = m.SelectedItems[f];

                    pageToDouble.sourceSectionId = Startup.myData.GetSectionIDbyCode(tempPnum.ImageKey);

                    for (int c = 0; c < Startup.issuesList.Count; c++)
                    {
                        if (Startup.issuesList[c].Id != Convert.ToInt32(m.Tag))
                        {
                            pInfo o = new pInfo();

                            o.issueName = Startup.issuesList[c].Name;
                            o.issueId = Startup.issuesList[c].Id;
                            o.pageCount = Startup.issuesList[c].Pages;
                            o.planeId = Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), o.issueId);

                            o.pageNum = Startup.myData.GetLinkPageNum(o.issueId, pageToDouble.sourceSectionId, Convert.ToInt32(m.SelectedItems[f].Text), Convert.ToInt32(m.Tag), o.planeId);

                            pageToDouble.targetPageInfo.Add(o);
                        }
                    }
                    DoublePages wDuble = new DoublePages(ref pageToDouble);
                    wDuble.Location = Startup.Location(wDuble);

                    wDuble.Text = wDuble.Text + " полосу " + tempPnum.Text.Trim();
                    wDuble.Tag = tempPnum.Text.Trim();

                    if (wDuble.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < pageToDouble.targetPageInfo.Count; i++)
                        {
                                Startup.myData.AddLink(Startup.myData.GetTemplateID(Startup.globalDate.ToShortDateString(), pageToDouble.targetPageInfo[i].issueId),
                                                        pageToDouble.targetPageInfo[i].issueId,
                                                        pageToDouble.targetPageInfo[i].pageNum,
                                                        pageToDouble.sourceSectionId,
                                                        Convert.ToInt32(tempPnum.Text),
                                                        Convert.ToInt32(m.Tag));
                        }
                        Startup.anyChanges = true;
                        myOwner.saveMenuItem.Enabled = true;
                    }
                    pageToDouble.Clear();
                }
                for (int k = 0; k < workingTable.ColumnCount; k++)
                {
                    ListView m1 = (ListView)workingTable.GetControlFromPosition(k, workingTable.RowCount - 1);
                    FillIssue(m1, "template_edit", m1.Items.Count, Startup.myData.GetTemplate(Startup.globalDate, Convert.ToInt32(m1.Tag.ToString()))[0], Convert.ToInt32(m1.Tag.ToString()));
                }                    

            }
        }

        private void UpdateCrossLinkedPages(int templ_id)
//показываем по регионам дублируемые из наца полосы
        {
//******************
            //return;
            for (int i = 0; i < workingTable.ColumnCount; i++)
            {

                ListView l = (ListView)workingTable.GetControlFromPosition(i, workingTable.RowCount - 1);
                
                int issueId = (int)l.Tag;
                List<ListViewItem> linkSections = Startup.myData.GetLinks(Startup.myData.GetTemplateID(Startup.globalDate.Date.ToString(), issueId), issueId);
                foreach (ListViewItem lItem in linkSections)
                {
                    if (Convert.ToInt32(lItem.Text) > 0)
                    {
                        l.FindItemWithText(lItem.Text).Text = lItem.Text;
                        l.FindItemWithText(lItem.Text).ImageKey = lItem.ImageKey;
                        l.FindItemWithText(lItem.Text).Name = lItem.Name;
                        l.FindItemWithText(lItem.Text).ToolTipText = lItem.ToolTipText;
                    }
                }
            }
        }

        private void CheckBox_Click(object sender, EventArgs e)
//событие клика по чек-боксу в созданной панели
//применяется, чтобы редактор открыл режим редакторивания для дневного палан
        {
            CheckBox cb = (CheckBox)sender;

            int columnNum = (int)workingTable.GetColumn(cb);
            if (columnNum == -1) return;
            ListView l = (ListView)workingTable.GetControlFromPosition(columnNum, workingTable.RowCount - 1);
            switch (cb.CheckState)
            { 
                case CheckState.Checked:
                    workingTable.GetControlFromPosition(columnNum, 1).Enabled = true;
                    l.LargeImageList = Startup.myData.iconList;
                    l.Enabled = true;
                    break;
                case CheckState.Unchecked:
                    workingTable.GetControlFromPosition(columnNum, 1).Enabled = false;
                    l.LargeImageList = Startup.myData.iconSmallList;
                    l.Enabled = false;
                    break;
            }
            Startup.anyChanges = true; //случились изменения!!!
            myOwner.saveMenuItem.Enabled = true;
        }

        internal void ChangeView_Click()
        //функция смены вида в панели редактора
        {
            foreach (Control s in Startup.mainPlane.workingTable.Controls)
            {
                try
                {
                    ListView lv = (ListView)s;
                    switch (lv.View)
                    {
                        case View.LargeIcon:
                            //если был в в иде иконок
                            lv.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
                            lv.View = View.Details;
                            lv.StateImageList = myOwner.stateIndexes;
                            lv.Columns.Add("", 30, HorizontalAlignment.Left);
                            lv.Columns.Add("", 30, HorizontalAlignment.Left);
                            lv.Columns.Add("Секция", 110, HorizontalAlignment.Left);
                            lv.Columns.Add("Статус", 25, HorizontalAlignment.Left);
                            lv.Columns.Add("Комментарий", 25, HorizontalAlignment.Left);
                            lv.HeaderStyle = ColumnHeaderStyle.None;
                            lv.MouseMove -= new MouseEventHandler(lstIssue_mouseMove);
                            lv.MouseUp -= new MouseEventHandler(lstIssue_MouseUp);
                            lv.MouseDown -= new MouseEventHandler(lstIssue_MouseDown);
                            lv.DragDrop -= new DragEventHandler(lstIssue_DragDrop);
                            lv.DragOver -= new DragEventHandler(lstIssue_DragOver);
                            myOwner.rClickEditorMenu.Items[2].Enabled = false;
                            myOwner.rClickEditorMenu.Items[3].Enabled = false;
                            lv.FullRowSelect = true;
                            myOwner.RefreshPagesStatuses();
                            break;
                        case View.Details:
                            //если был в виде списка
                            lv.Columns.Clear();
                            lv.StateImageList = null;
                            lv.View = View.LargeIcon;
                            //lv.LargeImageList = Startup.myData.iconSmallList;
                            lv.Font = new System.Drawing.Font("Microsoft Sans Serif", 12);
                            lv.HideSelection = false;
                            lv.AllowDrop = true;
                            lv.MouseMove += new MouseEventHandler(lstIssue_mouseMove);
                            lv.MouseUp += new MouseEventHandler(lstIssue_MouseUp);
                            lv.MouseDown += new MouseEventHandler(lstIssue_MouseDown);
                            lv.DragDrop += new DragEventHandler(lstIssue_DragDrop);
                            lv.DragOver += new DragEventHandler(lstIssue_DragOver);
                            myOwner.rClickEditorMenu.Items[2].Enabled = true;
                            myOwner.rClickEditorMenu.Items[3].Enabled = true;
                            break;
                    }
                }
                catch (Exception) { }
            }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
//событие клика по комбобоксу в созданной панели
//применяется, когда редакор создаёт или редактирует план на день
        {
            ComboBox cb = (ComboBox)sender;

            int columnNum = (int)workingTable.GetColumn(cb);
            int pCount = Startup.myData.GetTemplatePages(cb.Text, (int)cb.SelectedValue);
            FillIssue((ListView)workingTable.GetControlFromPosition(columnNum, 2), wType, pCount, cb.Text, (int)cb.SelectedValue);
        }

        internal bool FillIssue(ListView inListView, string wType, int inPageCount, string planeName, int issueId)
//создаём схему выпуска, где входят:
//inListView - окно для списка полос, wType - что именно делаем (иначе: тип окна)
//inPageCount - количество страниц из окна, cmbEdition - комбобокс со списком выпусков
        {
            Startup.counter++;

            int pCount = Startup.myData.GetTemplatePages(planeName, issueId);

            if (planeName == "")
                return false;

            switch (wType)
            {
                case "template_create":
                    pCount = inPageCount;
                    break;
                //case "template_edit":
                //case "template_copy":
                //case "template_delete":
                default:
                    if (planeName == "")
                        return false;
                    pCount = (inPageCount == -1 ? pCount : (pCount == inPageCount ? pCount : inPageCount));
                    //pCount = (pCount == myOwner.myData.GetTemplatePages(planeName) ? pCount : myOwner.myData.GetTemplatePages(planeName));
                    break;
            }

            if (pCount == -1)
                return false;

            List<ListViewItem> tmp = new List<ListViewItem>();
            List<ListViewItem> tmp1 = new List<ListViewItem>();
            List<ListViewItem> work = new List<ListViewItem>();

            List<ListViewItem> s = Startup.myData.GetTemplate(planeName, issueId);

            //делаем список из пустых страниц на всё количество полос нового выпуска
            for (int j = 0; j < pCount; j++)
            {
                tmp.Add(new ListViewItem(Startup.CreateName(j + 1), "---"));
                tmp.Last().ToolTipText = "Не назначено";
            }

            //если список секций из базы содержит хоть одну запись
            //добавляем полученные секции в пустой список
            if (s != null)
            {
                if (wType != "template_create")
                {
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        int k = s.FindIndex(v => Convert.ToInt32(v.Text) == (i + 1));
                        if (k != -1)
                        {
                            tmp[i] = s[k];
                        }
                    }
                }
                else
                {
                    //при условии, что список секций в окне ранее заполнен, 
                    //спрашиваем у пользователя сохрянять ли нарисованное и 
                    //накидываем существующие секции в пустой список
                    if (inListView.Tag != null)
                        if (MessageBox.Show("Очистить внесённые изменения?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                        {
                            if (inListView.Items.Count != 0)
                            {
                                int c = (inListView.Items.Count < pCount ? inListView.Items.Count : pCount);
                                for (int i = 0; i < c / 2; i++)
                                {
                                    if (!inListView.Items[i * 2].ImageKey.Contains("---")) tmp[i] = inListView.Items[i * 2];
                                    if (!inListView.Items[i * 2 + 1].ImageKey.Contains("---")) tmp[c - i - 1] = inListView.Items[i * 2 + 1];
                                }
                            }
                            inListView.Tag = null;
                        }
                }
            }

            inListView.Items.Clear();

            switch (inListView.View)
            {

                case View.LargeIcon:
                    for (int l = 0; l < pCount / 2; l++)
                    {
                        inListView.Items.Add(tmp[l]);
                        inListView.Items.Add(tmp[tmp.Count - l - 1]);
                    }
                    break;

                case View.Details:
                    for (int l = 0; l < pCount; l++)
                    {
                        //tmp[l].Text = tmp[l].Text;//.Trim(new char[] { ' ' });
                        inListView.Items.Add(tmp[l]);
                    }
                    break;
            }
            FillDouble(ref inListView, issueId);
            return true;
        }

//----------->
//блок прорисовки Драг-Энд-Дропа
        private void lstIssue_MouseUp(object sender, MouseEventArgs e)
        {
            if (dndPoint.sourceSection >= 0)
                dnd_MouseUp(myOwner.lstSections, (ListView)sender);
        }

        private void lstIssue_DragDrop(object sender, DragEventArgs e)
        {
            //if (dndPoint.sourceSection >= 0)
                dnd_DragDrop(myOwner.lstSections, (ListView)sender);
        }

        private void lstIssue_DragOver(object sender, DragEventArgs e)
        {
            //if (dndPoint.sourceSection >= 0)
                dnd_DragOver((ListView)sender, e);
        }

        private void lstIssue_MouseDown(object sender, MouseEventArgs e)
        {
            if (dndPoint.sourceSection >= 0)
                dnd_MouseDown((ListView)sender, e);
        }

        private void lstIssue_mouseMove(object sender, MouseEventArgs e)
        {
            if (dndPoint.sourceSection >= 0)
                dnd_MouseMove((ListView)sender, e);
        }

        public void dnd_in_MouseDown(ListView inWindow, MouseEventArgs e)
//нажатие кнопки мышки на окне
//входят окно, где нажата мышка
//и событие нажатия мышки
        {

            try
            {
                dndPoint.sourceSection = inWindow.Items.IndexOf(inWindow.GetItemAt(e.X, e.Y));
                dndPoint.specItem = inWindow.Items[inWindow.Items.IndexOf(inWindow.GetItemAt(e.X, e.Y))];
                Size dragSize = SystemInformation.DragSize;
                dndPoint.ghotstRectangle = new Rectangle(new Point((e.X - dragSize.Width / 2), e.Y - dragSize.Height / 2), dragSize);
            }
            catch (Exception)
            { }
        }

        public void dragOver(object sender, DragEventArgs e)
//протаскивание над
        {
            e.Effect = DragDropEffects.Copy;
        }

        public void dnd_MouseUp(ListView inWindow, MouseEventArgs e)
//отпускание мышки
//входят окно, где нажата мышка
//и событие нажатия мышки
        {
            if (dndPoint.ghotstRectangle != Rectangle.Empty)
            {
                dndPoint.ghotstRectangle = Rectangle.Empty;
                if (dndPoint.sourceSection != -1)
                {
                    try
                    {
                        inWindow.Items[dndPoint.sourceSection].Selected = false;
                    }
                    catch (Exception)
                    { }
                }
            }
            dnd_ClearSelection(inWindow);
            if (dndPoint.listView != null)
                dnd_ClearSelection(dndPoint.listView);
            dndPoint.Reset();
            Startup.anyChanges = true;
            myOwner.saveMenuItem.Enabled = true;
        }

        public void dnd_ClearSelection(ListView fromWindow, ListView toWindow)
//очистка окон секции и выпуска
        {
            for (int i = 0; i < fromWindow.Items.Count; i++)
                fromWindow.Items[i].Selected = false;
            for (int i = 0; i < toWindow.Items.Count; i++)
                toWindow.Items[i].Selected = false;
        }

        public void dnd_ClearSelection(ListView inWindow)
//очистка окон секции и выпуска
        {
            for (int i = 0; i < inWindow.Items.Count; i++)
                inWindow.Items[i].Selected = false;
        }

        public void dnd_MouseUp(ListView fromWindow, ListView toWindow)
//отпусканеи кнопки мышки
//входят окно секций и окно выпуска
        {
            dndPoint.ghotstRectangle = Rectangle.Empty;
            foreach (ListViewItem item in fromWindow.Items)
            {
                item.Selected = false;
            }
            foreach (ListViewItem item in toWindow.Items)
            {
                item.Selected = false;
            }
            dnd_ClearSelection(fromWindow);
            dndPoint.Reset();
        }

        public void dnd_DragDrop(ListView fromWindow, ListView toWindow)
//входят окно секций и окно выпуска
        {
            if (toWindow.BackColor == Color.Pink) toWindow.BackColor = fromWindow.BackColor;
            if (dndPoint.targetSection != -1)
            {
                foreach (ListViewItem item in fromWindow.Items)
                {
                    item.Selected = false;
                }

                foreach (ListViewItem item in toWindow.Items)
                {
                    if (item.Selected)
                    {
                        item.ToolTipText = dndPoint.specItem.ToolTipText;
                        item.Name = dndPoint.specItem.Name;
                        if (toWindow.Tag == null)
                            item.ImageKey = dndPoint.specItem.ImageKey;
                        else
                            item.ImageKey = (Startup.myData.GetColorForPage(Convert.ToInt32(item.Text),
                            Convert.ToInt32(toWindow.Tag.ToString()),
                            (int)Startup.globalDate.DayOfWeek) != 0 ? dndPoint.specItem.ImageKey : dndPoint.specItem.ImageKey + "BW");

                        item.Selected = false;
                        //Startup.myData.Write_Activity(Startup.User, "create_plane", );
                    }
                }
            }
            //toWindow.Tag = "edited";
            dnd_ClearSelection(fromWindow, toWindow);
        }

        public void dnd_DragOver(ListView inWindow, DragEventArgs e)
//прорисовка при проведении объекта над элементами списка
//входит окно,в которое перетаскиваем
//и событие 
        {
            Point p = inWindow.PointToClient(Cursor.Position);
            ListViewItem n = inWindow.GetItemAt(p.X, p.Y);
            List<ListViewItem> other = new List<ListViewItem>();

            other.Add(inWindow.GetItemAt(p.X + 40, p.Y));
            other.Add(inWindow.GetItemAt(p.X - 40, p.Y));
            other.Add(inWindow.GetItemAt(p.X, p.Y + 25));
            other.Add(inWindow.GetItemAt(p.X, p.Y - 25));

            e.Effect = DragDropEffects.Copy;

            if (n != null && dndPoint.specItem != null)//(sourceSection != -1 || sourceIssue != -1))
            {
                foreach (ListViewItem item in inWindow.Items)
                {
                    item.Selected = false;
                }
                dndPoint.listView = n.ListView;
                
                dndPoint.targetSection = n.Index;

                foreach (ListViewItem item in other)
                {
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        public void dnd_MouseDown(ListView inWindow, MouseEventArgs e)
//нажатие в окне выпуска
//входят окно и 
//событие мышки
        {
            //if (lstIssue.BackColor == Color.Pink) lstIssue.BackColor = lstSections.BackColor;
            try
            {
                dndPoint.sourceIssue = inWindow.Items.IndexOf(inWindow.GetItemAt(e.X, e.Y));
                dndPoint.specItem = inWindow.Items[inWindow.Items.IndexOf(inWindow.GetItemAt(e.X, e.Y))];
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            Size dragSize = SystemInformation.DragSize;
            dndPoint.ghotstRectangle = new Rectangle(new Point((e.X - dragSize.Width / 2), e.Y - dragSize.Height / 2), dragSize);
        }

        public void dnd_MouseMove(ListView inWindow, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dndPoint.ghotstRectangle != Rectangle.Empty && !dndPoint.ghotstRectangle.Contains(e.X, e.Y))
                {
                    dndPoint.screenOffset = SystemInformation.WorkingArea.Location;
                    DragDropEffects dropEff = inWindow.DoDragDrop(dndPoint.specItem, DragDropEffects.Copy);
                }
            }
        }

//кнопка "ручной" секции
        public void dnd_MouseDown(string inText, MouseEventArgs e)
        {
            dndPoint.specItem = new ListViewItem();

            dndPoint.specItem.ToolTipText = inText;
            dndPoint.specItem.ImageKey = "-!-";
            
            Size dragSize = SystemInformation.DragSize;
            dndPoint.ghotstRectangle = new Rectangle(new Point((e.X - dragSize.Width / 2), e.Y - dragSize.Height / 2), dragSize);
        }

        public void dnd_MouseMove(Button inButton, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dndPoint.ghotstRectangle != Rectangle.Empty && !dndPoint.ghotstRectangle.Contains(e.X, e.Y))
                {
                    dndPoint.screenOffset = SystemInformation.WorkingArea.Location;
                    DragDropEffects dropEff = inButton.DoDragDrop(inButton, DragDropEffects.Copy);
                }
            }
        }

//--------------->
    }
}
