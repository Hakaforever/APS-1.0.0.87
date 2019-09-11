using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ComponentModel;

namespace APS
{
    class DataAccess
    {
        public MainDBDataSet mainDBdataset = new MainDBDataSet();

        public MainDBDataSet.activityDataTable tblActivity;
        public MainDBDataSet.configDataTable tblConfig;
        public MainDBDataSet.issuesDataTable tblIssues;
        public MainDBDataSet.layoutsDataTable tblLayouts;
        public MainDBDataSet.sectionsDataTable tblSections;
        public MainDBDataSet.templatesDataTable tblTemplates;
        public MainDBDataSet.usersDataTable tblUsers;
        public MainDBDataSet.editionsDataTable tblEditions;
        public MainDBDataSet.globalplanesDataTable tblGlobalPlanes;
        public MainDBDataSet.linksectionsDataTable tblLinkSections;
        public MainDBDataSet.colorsDataTable tblColorSettings;
        public MainDBDataSet.sessionsDataTable tblSessions;
        public MainDBDataSet.issue_numDataTable tblIssueNum;
        public MainDBDataSet.colors_schemesDataTable tblColorSchemes;
        public MainDBDataSet.globalcolorsDataTable tblGlobalColors;
        public MainDBDataSet.pdffilesDataTable tblPDF;
        public MainDBDataSet.deadlinesDataTable tblDeadlines;

        public MainDBDataSetTableAdapters.activityTableAdapter taActivity = new MainDBDataSetTableAdapters.activityTableAdapter();
        public MainDBDataSetTableAdapters.configTableAdapter taConfig = new MainDBDataSetTableAdapters.configTableAdapter();
        public MainDBDataSetTableAdapters.issuesTableAdapter taIssues = new MainDBDataSetTableAdapters.issuesTableAdapter();
        public MainDBDataSetTableAdapters.layoutsTableAdapter taLayouts = new MainDBDataSetTableAdapters.layoutsTableAdapter();
        public MainDBDataSetTableAdapters.sectionsTableAdapter taSections = new MainDBDataSetTableAdapters.sectionsTableAdapter();
        public MainDBDataSetTableAdapters.templatesTableAdapter taTemplates = new MainDBDataSetTableAdapters.templatesTableAdapter();
        public MainDBDataSetTableAdapters.usersTableAdapter taUsers = new MainDBDataSetTableAdapters.usersTableAdapter();
        public MainDBDataSetTableAdapters.TableAdapterManager taMain = new MainDBDataSetTableAdapters.TableAdapterManager();
        public MainDBDataSetTableAdapters.editionsTableAdapter taEditions = new MainDBDataSetTableAdapters.editionsTableAdapter();
        public MainDBDataSetTableAdapters.globalplanesTableAdapter taGlobalPlane = new MainDBDataSetTableAdapters.globalplanesTableAdapter();
        public MainDBDataSetTableAdapters.linksectionsTableAdapter taLinkSections = new MainDBDataSetTableAdapters.linksectionsTableAdapter();
        public MainDBDataSetTableAdapters.colorsTableAdapter taColorSettings = new MainDBDataSetTableAdapters.colorsTableAdapter();
        public MainDBDataSetTableAdapters.sessionsTableAdapter taSessions = new MainDBDataSetTableAdapters.sessionsTableAdapter();
        public MainDBDataSetTableAdapters.issue_numTableAdapter taIssueNum = new MainDBDataSetTableAdapters.issue_numTableAdapter();
        public MainDBDataSetTableAdapters.colors_schemesTableAdapter taColorSchemes = new MainDBDataSetTableAdapters.colors_schemesTableAdapter();
        public MainDBDataSetTableAdapters.globalcolorsTableAdapter taGlobalColors = new MainDBDataSetTableAdapters.globalcolorsTableAdapter();
        public MainDBDataSetTableAdapters.pdffilesTableAdapter taPDF = new MainDBDataSetTableAdapters.pdffilesTableAdapter();
        public MainDBDataSetTableAdapters.deadlinesTableAdapter taDeadlines = new MainDBDataSetTableAdapters.deadlinesTableAdapter();

        public BindingSource bsMain = new BindingSource();
        public BindingSource bsTemp = new BindingSource();
        public BindingSource bsThird = new BindingSource();

        //MainForm myOwner;

        public ImageList iconList = new ImageList();
        public ImageList iconSmallList = new ImageList();

//********** 1.0.0.85
        internal BackgroundWorker checkDB = new BackgroundWorker();

        public class myPlane
//сбор информации о плане
        {
            public int PageCount { 
                get {return PageCount;} 
                set { if (value >= 0) PageCount = value; else PageCount = 0;} 
            }
            public string Name {get; set;}

            public myPlane(string _name, int _pagesCount)
            {
                this.PageCount = _pagesCount;
                this.Name = _name;
            }

            public int GetPages(string _name)
            {
                if (this.Name == _name)
                    return PageCount;
                else
                    return -1;
            }
        }

        public string[] pages { get; set;}

        public DataAccess()
//инициализатор класса
//входит главная форма
//вопрос - а она нужна? может сделать внутри сруктуру с нужными параметрами
        {
            AdaptersFill();

            taMain.activityTableAdapter = taActivity;
            taMain.configTableAdapter = taConfig;
            taMain.issuesTableAdapter = taIssues;
            taMain.layoutsTableAdapter = taLayouts;
            taMain.sectionsTableAdapter = taSections;
            taMain.templatesTableAdapter = taTemplates;
            taMain.editionsTableAdapter = taEditions;
            taMain.globalplanesTableAdapter = taGlobalPlane;
            taMain.linksectionsTableAdapter = taLinkSections;
            taMain.usersTableAdapter = taUsers;
            taMain.colorsTableAdapter = taColorSettings;
            taMain.sessionsTableAdapter = taSessions;
            taMain.issue_numTableAdapter = taIssueNum;
            taMain.colors_schemesTableAdapter = taColorSchemes;
            taMain.globalcolorsTableAdapter = taGlobalColors;
            taMain.pdffilesTableAdapter = taPDF;
            taMain.deadlinesTableAdapter = taDeadlines;

            tblActivity = mainDBdataset.activity;
            tblConfig = mainDBdataset.config;
            tblIssues = mainDBdataset.issues;
            tblLayouts = mainDBdataset.layouts;
            tblSections = mainDBdataset.sections;
            tblTemplates = mainDBdataset.templates;
            tblEditions = mainDBdataset.editions;
            tblGlobalPlanes = mainDBdataset.globalplanes;
            tblLinkSections = mainDBdataset.linksections;
            tblUsers = mainDBdataset.users;
            tblColorSettings = mainDBdataset.colors;
            tblSessions = mainDBdataset.sessions;
            tblIssueNum = mainDBdataset.issue_num;
            tblColorSchemes = mainDBdataset.colors_schemes;
            tblGlobalColors = mainDBdataset.globalcolors;
            tblPDF = mainDBdataset.pdffiles;
            tblDeadlines = mainDBdataset.deadlines;

            //внутренние биндинги
            bsMain.DataSource = mainDBdataset;
            bsTemp.DataSource = mainDBdataset;
            bsThird.DataSource = mainDBdataset;

            iconList.ImageSize = new Size(60, 30);
            iconList.ColorDepth = ColorDepth.Depth8Bit;
            iconList.TransparentColor = Color.Transparent;

            iconSmallList.ImageSize = new Size(60, 30);
            iconSmallList.ColorDepth = ColorDepth.Depth8Bit;
            iconSmallList.TransparentColor = Color.Transparent;

            GetPages();
            GetIcons();
//****** 1.0.0.85
            checkDB.DoWork += new DoWorkEventHandler(ChekDBChanges);
        }

        private void ChekDBChanges(object sender, DoWorkEventArgs e) //** 1.0.0.85
        //проверка обновления базы данных другими пользователями
        {
            SqlConnection connect = new SqlConnection(Startup.APSDBConnectionString);

            SqlCommand readData = new SqlCommand();
            readData.CommandText = "SELECT * FROM activity WHERE last_access > @login_time AND user_id NOT LIKE @user";
            readData.Connection = connect;
            readData.Parameters.AddWithValue("@login_time", Startup.loginTime);
            readData.Parameters.AddWithValue("@user", Startup.User);

            SqlDataReader myReader;

            try
            {
                connect.Open();
            }
            catch(Exception)
            {
                return;
            }

            try
            {
                myReader = readData.ExecuteReader();
                if (myReader.HasRows)
                    Startup.baseChanged = true;
                if (!myReader.IsClosed)
                {
                    myReader.Close();
                }
            }
            catch(Exception){}
            finally
            {
                connect.Close();
            }
            System.Media.SystemSounds.Beep.Play();
        }

        private void GetIcons()
//получение иконок из таблицы секций и их загон в ImageList
//тут первый случай применения отсылки к owner
//который забарывается созданием своего ImageList
        {
            iconList.Images.Clear();
            iconSmallList.Images.Clear();

            iconList.Images.Add("---", Properties.Resources.EMP_);
            iconList.Images.Add("-!-", Properties.Resources.ZZZ_);
            iconList.Images.Add("+++", Properties.Resources.PDF_new_2);
            iconSmallList.Images.Add("---", ChangeImageOpacity(Properties.Resources.EMP_, 0.6));
            iconSmallList.Images.Add("-!-", ChangeImageOpacity(Properties.Resources.ZZZ_, 0.6));
            iconSmallList.Images.Add("+++", Properties.Resources.PDF_new_2);

            
            for (int i = 0; i < tblSections.Rows.Count; i++)
            {
                Byte[] reader = tblSections.Rows[i].Field<Byte[]>("icon");
                Byte[] readerBW = tblSections.Rows[i].Field<Byte[]>("icon_bw");
                if (reader != null)
                {
                    iconList.Images.Add(tblSections.Rows[i].Field<string>("code"), Image.FromStream(new MemoryStream(reader)));
                    iconList.Images.Add(tblSections.Rows[i].Field<string>("code") + "BW", Image.FromStream(new MemoryStream(readerBW)));
                    iconSmallList.Images.Add(tblSections.Rows[i].Field<string>("code"), ChangeImageOpacity(Image.FromStream(new MemoryStream(reader)), 0.6));
                    iconSmallList.Images.Add(tblSections.Rows[i].Field<string>("code")+"BW", ChangeImageOpacity(Image.FromStream(new MemoryStream(readerBW)), 0.6));
                }                
            }
        }

        private const int bytesPerPixel = 4;

        public static Image ChangeImageOpacity(Image originalImage, double opacity)
//изменение прозрачности изображения
//входит изображение и уровень прозрачности в диапазоне 0-1
//выходит готовое изображение
        {
            if ((originalImage.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
            {
                // Cannot modify an image with indexed colors
                return originalImage;
            }

            Bitmap bmp = (Bitmap)originalImage.Clone();

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 32 bits per pixels 
            // (32 bits = 4 bytes, 3 for RGB and 1 byte for alpha).
            int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];

            // Copy the ARGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the
            // RGB values for all pixels in the the bitmap.
            for (int counter = 0; counter < argbValues.Length; counter += bytesPerPixel)
            {
                // argbValues is in format BGRA (Blue, Green, Red, Alpha)

                // If 100% transparent, skip pixel
                if (argbValues[counter + bytesPerPixel - 1] == 0)
                    continue;

                int pos = 0;
                pos++; // B value
                pos++; // G value
                pos++; // R value

                argbValues[counter + pos] = (byte)(argbValues[counter + pos] * opacity);
            }

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public void AdaptersFill()
//заполнение адаптеров таблиц
        {
            try
            {
                taActivity.Fill(mainDBdataset.activity);
                taConfig.Fill(mainDBdataset.config);
                taIssues.Fill(mainDBdataset.issues);
                taLayouts.Fill(mainDBdataset.layouts);
                taSections.Fill(mainDBdataset.sections);
                taTemplates.Fill(mainDBdataset.templates);
                taEditions.Fill(mainDBdataset.editions);
                taUsers.Fill(mainDBdataset.users);
                taLinkSections.Fill(mainDBdataset.linksections);
                taGlobalPlane.Fill(mainDBdataset.globalplanes);
                taSessions.Fill(mainDBdataset.sessions);
                taColorSettings.Fill(mainDBdataset.colors);
                taColorSchemes.Fill(mainDBdataset.colors_schemes);
                taIssueNum.Fill(mainDBdataset.issue_num);
                taGlobalColors.Fill(mainDBdataset.globalcolors);
                taPDF.Fill(mainDBdataset.pdffiles);
                taDeadlines.Fill(mainDBdataset.deadlines);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        internal void ClearBSFilters()
        {
            bsMain.Filter = null;
            bsTemp.Filter = null;
            bsThird.Filter = null;
        }

        public Exception RefreshTableAdapters()
//обновление табличных адаптеров
        {
            try
            {
                taMain.UpdateAll(mainDBdataset);
            }
            catch (SqlException ex)
            {
                //MessageBox.Show(ex.Number + "\n" + ex.Message);
                return ex;
            }
            return null;
        }

        public Exception EndEdit()
//конец редактирования bindingSource
        {
            try
            {
                bsTemp.EndEdit();
                bsMain.EndEdit();
                bsThird.EndEdit();
                AddTime();
                RefreshTableAdapters();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Exception AddTime()
//управляем колонкой времени в записях
//работает только с внутренними bindingSource
//надо бы создать такую же для отдельных таблиц
        {
            if (bsMain.DataMember.Length > 0)
            {
                foreach (DataRowView r in bsMain)
                {
                    if (r.IsEdit == true || r.IsNew == true)
                    {
                        try
                        {
                            r.Row.SetField<DateTime>("last_access", DateTime.Now);
                        }
                        catch (Exception ex)
                        {
                            //return ex;
                        }
                    }
                }
            }
            return null;
        }

        public void CancelEdit()
//отмена изменений
//работает только с внутренними bindingSource
        {
            bsMain.CancelEdit();
            bsTemp.CancelEdit();
            bsThird.CancelEdit();
            //RefreshTableAdapters();
        }

        public void MoveFirst()
//переход к первой записи bindingSource
        {
            bsMain.MoveFirst();
        }

        public Exception AddNew(BindingSource bsIn)
//добавление новой записи в bindingSource с вызовом конца редактирования bindingSource
//работает со входящим bindingSource 
        {
            try
            {
                bsIn.EndEdit();
                RefreshTableAdapters();
                bsIn.AddNew();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания новой записи" + "\n" + ex.Message);
                return ex;
            }
            return null;
        }

        internal int AddNonFormatSection(string secName, int edId)
//добавляет в таблицу Sections секцию, отсутствующую в Атексе
//в случае неудачи возвращает -2, в случае удачи - id секции
        {
            MainDBDataSet.sectionsRow sr = mainDBdataset.sections.NewsectionsRow();

            sr.code = "-!-";
            sr.edition_id = edId;
            sr.from_atex = false;
            sr.is_spread = false;
            sr.last_access = DateTime.Now;
            sr.name = secName;
            sr.comment = "Секция создана вручную";

            try
            {
                tblSections.AddsectionsRow(sr);
                taSections.Update(tblSections);
            }
            catch (Exception)
            {
                return -2;
            }
            return sr.id;

        }

        internal Exception DeleteSection(string secName)
//удаление секции по названию
        {
            Exception err = null;

            var secID = from section in tblSections.AsEnumerable()
                        where section.name == secName
                        select section;
            DataRow o = secID.FirstOrDefault();

            err = DeleteRow("sections", tblSections.Rows.IndexOf(o));
            return err;
        }

        internal Exception DeleteTemplate(string tplName)
//удаление шаблона по названию
        {
            Exception err = null;

            var tplID = from template in tblTemplates.AsEnumerable()
                        where template.name == tplName
                        select template;
            DataRow o = tplID.FirstOrDefault();

            err = DeleteRow("templates", tblSections.Rows.IndexOf(o));
            return err;
        }

        internal Exception DeleteLayout(int tmplId)
//удаление шаблона по id шаблона
        {
            Exception err = null;

            var layouts = from layout in tblLayouts.AsEnumerable()
                          where layout.template_id == tmplId
                          select layout;

            while (layouts.Any())
            {
                DataRow o = layouts.FirstOrDefault();
                err = DeleteRow("layouts", tblSections.Rows.IndexOf(o));
            }
            return err;
        }

        internal void UserSession(string user, bool status)
//запись о состоянии статус сессии пользователя
//если записи нет - значит пользователь ещё не логинился и тогда создаём её
//status - статус записи: 0 - закрываем. 1 открываем
        {
            IEnumerable<MainDBDataSet.sessionsRow> x = from ses in tblSessions 
                                                       join usr in tblUsers on ses.user_id equals usr.id
                                                       where usr.name == user
                                                       select ses;

            if (x.Any())
            {
                x.First().SetField("status", status);
            }
            else
            {
                MainDBDataSet.sessionsRow d = mainDBdataset.sessions.NewsessionsRow();
                try
                {
                    d.user_id = tblUsers.Where(p => p.name == user).First().id;
                    d.status = true;
                    mainDBdataset.sessions.AddsessionsRow(d);
                }
                catch (Exception) { };
            }

            taSessions.Update(tblSessions);
            try
            {
                tblUsers.Where(p => p.name == user).First().SetField("last_access", DateTime.Now);
                taUsers.Update(tblUsers);
            }
            catch(Exception){}
        }

        internal Exception DeleteLink(int templateId, int pageNum, int issueId)
//удаление записи о связи
        {
            var x = (pageNum != -1 
                    ?
                    from link in tblLinkSections.AsEnumerable()
                    where link.template_id == templateId
                    && link.page_num == pageNum
                    && link.issue_id == issueId
                    select link
                    :
                    from link in tblLinkSections.AsEnumerable()
                    where link.template_id == templateId
                    && link.issue_id == issueId
                    select link
                    );

            if (x.Any())
            { 
                DataRow o = x.First();
                DeleteRow("linksections", tblLinkSections.Rows.IndexOf(o));
            }

            return null;
        }

        internal Exception DeleteIssue(string issName)
//удаление выпуска по названию
        {
            Exception err = null;

            var issID = from issue in tblIssues.AsEnumerable()
                        where issue.name == issName
                        select issue;
            DataRow o = issID.FirstOrDefault();
            err = DeleteRow("issues", tblIssues.Rows.IndexOf(o));
            return err;
        }

        internal Exception DeleteEdition(string edName)
//удаление издания по названию
        {
            Exception err = null;

            var edID = from edition in tblEditions.AsEnumerable()
                       where edition.name == edName
                       select edition;

            DataRow o = edID.FirstOrDefault();
            err = DeleteRow("editions", tblEditions.Rows.IndexOf(o));
            return err;
        }

        public void DeletePlane(string planeName, int issueId)
//удаляем план
        {
            int tplId = GetTemplateID(planeName, issueId);

            if (tplId > 0)
            {
                DeleteLayout(tplId);
                DeleteTemplate(planeName);
            }
        }

        public Exception DeleteRow(string nameTable, int rowNumber)
//удаление строки из таблицы
//входят имя таблицы и номер строки
        {
            try
            {
                bsMain.EndEdit();
                if (String.IsNullOrEmpty(nameTable) || String.IsNullOrWhiteSpace(nameTable))
                {
                    throw new DeletedRowInaccessibleException("Ups!");
                }
                mainDBdataset.Tables[nameTable.ToLower()].Rows[rowNumber].Delete();
                RefreshTableAdapters();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        public int GetTemplatePages(string planeName, int issueId)
//получение количества полос по имени плана
        {
            var p = from template in tblTemplates
                    where template.name == planeName
                    && template.issue_id == issueId
                    select template.pages;
            if (planeName == "" || issueId == -1)
                return -1;
            try
            {
                return Convert.ToInt32(p.FirstOrDefault());
            }
            catch (Exception)
            {
            }
            return -1;
        }

        public List<ListViewItem> GetTemplate(string planeName, int issueId)
//получение схемы выпуска из базы и возврат в виде ListView
        {
            var myName = from layout in tblLayouts.AsEnumerable()
                         join template in tblTemplates.AsEnumerable() on layout.template_id equals template.id
                         join section in tblSections.AsEnumerable() on layout.section_id equals section.id
                         where (string)template.name == planeName && template.issue_id == issueId
                         orderby layout.page_num
                         select new 
                         {
                             Name = section.name,
                             Code = section.code,
                             NumberPage = layout.page_num,
                             Tsvet = section.color.Split(new char[]{','}),
                             Comment = layout.Field<string>("comment") == null ? "" : layout.comment
                         };

            if (!myName.Any() || planeName == "") return null;

            List<ListViewItem> lCollection = new List<ListViewItem>();

            foreach (var s in myName)
            {
                ListViewItem c = new ListViewItem();
                c.ToolTipText = s.Name + System.Environment.NewLine + s.Comment;  //имя страницы как всплывающий текст
                c.Text = Startup.CreateName(s.NumberPage); //номер полосы как текст с добавлением "0" к одноцифровым значениям

                c.ImageKey = s.Code; //код секции как код для иконки

                c.Name = s.Name;
                c.Tag = s.Comment;
                c.StateImageIndex = 0;
                c.UseItemStyleForSubItems = false;
                c.SubItems.Add(s.NumberPage.ToString("D2"), Color.Black, Color.FromArgb(Convert.ToInt32(s.Tsvet[0]), Convert.ToInt32(s.Tsvet[1]), Convert.ToInt32(s.Tsvet[2]), Convert.ToInt32(s.Tsvet[3])), new System.Drawing.Font("Microsoft Sans Serif", 11));
                c.SubItems.Add(s.Name, Color.Black, Color.FromArgb(Convert.ToInt32(s.Tsvet[0]), Convert.ToInt32(s.Tsvet[1]), Convert.ToInt32(s.Tsvet[2]), Convert.ToInt32(s.Tsvet[3])), new System.Drawing.Font("Microsoft Sans Serif", 11));
                c.SubItems.Add("", Color.Black, Color.White, new System.Drawing.Font("Microsoft Sans Serif", 8));
                c.SubItems.Add("", Color.Black, Color.White, new System.Drawing.Font("Microsoft Sans Serif", 9));
                string f = (s.Comment.Length == 0 ? "" : "!");
                c.SubItems.Add(f, Color.Red, Color.White, new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold));
                if (f == "!") c.BackColor = Color.LightPink;
                if (GetColorForPage(s.NumberPage, issueId, (int)Startup.globalDate.DayOfWeek) == 0)
                {
                    //c.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    //c.ForeColor = Color.MediumVioletRed;
                    c.ImageKey = s.Code + "BW";
                }
                //else
                //{
                //    c.SubItems[3].BackColor = Color.LightGray;
                //}

                lCollection.Add(c);
            }
            
            return lCollection;
        }

        internal int GetTemplateID(string planeName, int issueId)
//получение id шаблона по имени
        {
            var tplId = from template in tblTemplates.AsEnumerable()
                        where template.name == planeName
                        && template.issue_id == issueId
                        select template.id;

            return (int)tplId.FirstOrDefault();
        }

        internal string GetTemplateNamebyID(int planeId)
//получение название шаблона по id
        {
            var tplId = from template in tblTemplates.AsEnumerable()
                        where template.id == planeId
                        select template.name;

            return tplId.FirstOrDefault().ToString();
        }

        internal List<string> GetTemplate(DateTime date, int issue_id)
//получение имени плана на дату для выпуска с входящим issue_id
//если в таблице шаблонов план не найден,
//возвращает название плана на этот день недели из таблицы глобальных планов
//и вторым эелементом - тип плана
        {
            List<string> a = new List<string>();

            string hDate = date.ToShortDateString();
            //получаем номер шаблона из таблицы шаблонов
            IEnumerable<string> templDname = from templateN in tblTemplates.AsEnumerable()
                             where templateN.name == hDate
                             && templateN.issue_id == issue_id
                             select templateN.name;

            //получаем номер шаблона на день из глобальной таблицы 
            IEnumerable<string> templGname = from plane in tblGlobalPlanes.AsEnumerable()
                             join templateN in tblTemplates.AsEnumerable()
                             on plane.template_id equals templateN.id
                             where plane.issue_id == issue_id
                             && plane.day_of_week == (int)date.DayOfWeek
                             select templateN.name;

            try
            {
                if (templDname.Any())
                {
                    a.Add(templDname.First().ToString());
                    a.Add("day");
                }
                else
                {
                    a.Add(templGname.First().ToString());
                    a.Add("global");
                }
            }
            catch(Exception)
            {
            }
            return a;
        }

        internal Exception SaveDTDplane(DateTime inDate, string issueName, ListView.ListViewItemCollection inList)
//сохранение подневного плана
//входят дата, название выпуска и ListViewItem
        {
            Exception ex = null;

            //string[] tmp = inList.OfType<ListViewItem>().Select(item => item.ToolTipText).ToArray();
            string[] tmp = inList.OfType<ListViewItem>().Select(item => item.Name).ToArray();
            tmp = tmp.Where((x, j) => (j % 2) == 0).ToArray().Concat(tmp.Where((x, j) => (j % 2) == 1).Reverse()).ToArray();
            int issId = GetIssueIDbyName(issueName);
            UpdatePlane(inDate.ToShortDateString(), issId, tmp, true);
            return ex;
        }

        internal int GetSectionIDbyCode(string code)
//получаем id секции по коду
        {
            var x = from sections in tblSections.AsEnumerable()
                    where sections.code == code
                    select sections.id;

            if (x.Any())
                return (int)x.First();
            else
                return -1;
        }

        internal string GetSectionCodeByID(int section_id)
//получаем код секции по id
        {
            var x = from sections in tblSections.AsEnumerable()
                    where sections.id == section_id
                    select sections.code;

            return x.FirstOrDefault().ToString();
        }

        internal string GetSectionNameByID(int section_id)
        //получаем имя секции по id
        {
            var x = from sections in tblSections.AsEnumerable()
                    where sections.id == section_id
                    select sections.name;

            return x.FirstOrDefault().ToString();
        }

        public List<ListViewItem> GetSections(int edition)
//возвращает список секций из базы в виде элементов ListView
        {
            var mySec = from sections in tblSections
                        where sections.edition_id == edition
                        orderby sections.name
                        select new
                        {
                            Name = sections.name,
                            Code = sections.code
                        };
            
            if (mySec.Count() == 0 || edition <= 0) return null;

            List<ListViewItem> ret = new List<ListViewItem>();

            foreach(var s in mySec)
            {
                ListViewItem c = new ListViewItem();
                c.ToolTipText = s.Name;
                c.ImageKey = s.Code;
                c.Name = s.Name;
                ret.Add(c);
            }
            return ret;
        }

        public List<ListViewItem> GetActiveSections(int edition, bool fromAtex)
//возвращает список секций из базы в виде элементов ListView
        {
            var mySec = (fromAtex == false ?
                        from sections in tblSections
                        where sections.edition_id == edition
                        where sections.from_atex == fromAtex
                        orderby sections.name
                        select new
                        {
                            Name = sections.name,
                            Code = sections.code,
                            Atex = sections.from_atex
                        }
                        :
                        from sections in tblSections
                        where sections.edition_id == edition
                        orderby sections.name
                        select new
                        {
                            Name = sections.name,
                            Code = sections.code,
                            Atex = sections.from_atex
                        });

            if (mySec.Count() == 0 || edition <= 0) return null;

            List<ListViewItem> ret = new List<ListViewItem>();

            foreach (var s in mySec)
            {
                ListViewItem c = new ListViewItem();
                c.ToolTipText = s.Name;
                c.ImageIndex = Convert.ToInt32(s.Atex);
                c.ImageKey = s.Code;
                c.Name = s.Name;
                ret.Add(c);
            }
            return ret;
        }

        public List<ListViewItem> GetSectionsByMask(string mask, int edition)
//mask позволяет отобрать секции по символам
//возвращает список секций из базы в виде элементов ListView
        {
            var mySec = from sections in tblSections
                        where sections.name.ToLower().Contains(mask)
                        where sections.edition_id == edition
                        orderby sections.name
                        select new
                        {
                            Name = sections.name,
                            Code = sections.code
                        };

            if (mySec.Count() == 0) return null;

            List<ListViewItem> ret = new List<ListViewItem>();

            foreach (var s in mySec)
            {
                ListViewItem c = new ListViewItem();
                c.ToolTipText = s.Name;
                c.ImageKey = s.Code;
                c.Name = s.Name;
                ret.Add(c);
            }
            return ret;
        }

        public List<ListViewItem> GetSectionsByMask(string mask, int edition, bool fromAtex)
//mask позволяет отобрать секции по символам
//возвращает список секций из базы в виде элементов ListView
        {
            var mySec = (fromAtex == false ?
                        from sections in tblSections
                        where sections.name.ToLower().Contains(mask)
                        where sections.edition_id == edition
                        where sections.from_atex == fromAtex
                        orderby sections.name
                        select new
                        {
                            Name = sections.name,
                            Code = sections.code
                        }
                        :
                        from sections in tblSections
                        where sections.name.ToLower().Contains(mask)
                        where sections.edition_id == edition
                        orderby sections.name
                        select new
                        {
                            Name = sections.name,
                            Code = sections.code
                        });

            if (mySec.Count() == 0) return null;

            List<ListViewItem> ret = new List<ListViewItem>();

            foreach (var s in mySec)
            {
                ListViewItem c = new ListViewItem();
                c.ToolTipText = s.Name;
                c.ImageKey = s.Code;
                c.Name = s.Name;
                ret.Add(c);
            }
            return ret;
        }

        internal int GetLinkPageNum(int issueId, int sourceSectionId, int sourcePageNum, int sourceIssueId, int templateId)
//получаем номер дублируемой полосы
        {
            if (templateId == 0)
                return -1;

            IEnumerable<int> b = (sourceSectionId != -1 ?
                    from linksection in tblLinkSections.AsEnumerable()
                    where linksection.issue_id == issueId && linksection.section_id == sourceSectionId &&
                    linksection.source_page_num == sourcePageNum && linksection.template_id == templateId &&
                    linksection.source_issue_id == sourceIssueId
                    select linksection.page_num
                    :
                    from linksection in tblLinkSections.AsEnumerable()
                    where linksection.issue_id == issueId && linksection.source_page_num == sourcePageNum
                    && linksection.template_id == templateId && linksection.source_issue_id == sourceIssueId
                    select linksection.page_num);

            if (!b.Any())
                return -1;
            return b.First();
        }

        public void UpdatePlane(string tplName, int issueId, string[] newSections, bool dayToDay)
//обновляем или создаём план выпуска в базе
//входят название плана и список секций
        {
            int templateId = -1;

            //определяем id плана по имени
            var searchTemplID = from templates in tblTemplates.AsEnumerable()
                                where templates.name == tplName &&
                                templates.issue_id == issueId
                                select templates.id;

            //выбираем всё, что касается старого плана
            var oldPlane = from layouts in tblLayouts.AsEnumerable()
                           where layouts.template_id == templateId
                           where layouts.issue_id == issueId
                           orderby layouts.page_num
                           select layouts;

            //определяем id издания
            var edId = from issues in tblIssues.AsEnumerable()
                       where issues.id == issueId
                       select issues.edition_id;

            string tmpN = "";

            //определяем id секции для замены
            var sid = from sect in tblSections.AsEnumerable()
                      where sect.name == tmpN
                      select sect.id.ToString();

            templateId = (int)searchTemplID.FirstOrDefault();

            if (templateId < 1)
            {
                templateId = AddTemplate(tplName, issueId, newSections.Count(), dayToDay);
            }

            MainDBDataSet.templatesRow tRow = tblTemplates.FindByid(templateId);
            tRow.SetField<int>("pages", (int)newSections.Count());
            //tblTemplates.AcceptChanges();
            taTemplates.Update(tblTemplates);
            foreach (MainDBDataSet.layoutsRow dr in oldPlane)
            //определяем в списке секций новые секции
            {
                try
                {
                    tmpN = newSections[dr.Field<int>("page_num") - 1];
                    dr.SetField("section_id", Convert.ToInt32(sid.FirstOrDefault()));
                    dr.SetField("issue_id", issueId);
                    dr.SetField("last_access", DateTime.Now);
                    newSections[dr.Field<int>("page_num") - 1] = "";
                }
                catch (Exception)
                {
                    //если секции найдены - удаляем запись из массива oldPlane
                    if (dr.Field<int>("page_num") > newSections.Length)
                        dr.Delete();
                }
            }

            for (int i = 0; i < newSections.Length; i++)
            {
                MainDBDataSet.layoutsRow lytRow = mainDBdataset.layouts.NewlayoutsRow();
                if (newSections[i] != "" && !newSections[i].Contains("Не назначено"))
                {
                    try
                    {
                        tmpN = newSections[i];
                        //надо настроить добавление ненормативной секции
                        int ttt = Convert.ToInt32(sid.FirstOrDefault());
                        if (ttt < 1)
                            lytRow.section_id = AddNonFormatSection(tmpN, Convert.ToInt32(edId.FirstOrDefault()));
                        else
                            lytRow.section_id = ttt;
                        lytRow.template_id = templateId;
                        lytRow.issue_id = issueId;
                        lytRow.last_access = DateTime.Now;
                        lytRow.page_num = i + 1;
                        tblLayouts.Rows.Add(lytRow);
                    }
                    catch (Exception)
                    { }
                }
            }
            taLayouts.Update(tblLayouts);
            EndEdit();
        }

        private int AddTemplate(string tplName, int issueId, int p, bool dayToDay)
//создаёт запись в таблице шаблонов
        {
            MainDBDataSet.templatesRow tRow = mainDBdataset.templates.NewtemplatesRow();

            tRow.issue_id = issueId;
            tRow.name = tplName;
            tRow.pages = p;
            tRow.last_access = DateTime.Now;
            tRow.comment = "added automate";
            tRow.daytoday = dayToDay;

            try
            {
                tblTemplates.AddtemplatesRow(tRow);
                taTemplates.Update(tblTemplates);
                EndEdit();
                return tRow.id;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        private void GetPages()
//получение значения из таблицы Config
//собственно, строим массив значений количества страниц
//исходя из значения максимального количества страниц
        {
            var getRow = from config in tblConfig.AsEnumerable()
                         where config.parametr == "MaxPages".ToLower()
                         select config.value;

            try
            {
                int f = Convert.ToInt32(getRow.FirstOrDefault());
                pages = new string[f / 4];

                for (int i = 0; i < f / 4; i++)
                    pages[i] = ((i + 1) * 4).ToString();
            }
            catch(Exception){}
        }

        public Exception Write_Activity(string user, string action, string addText)
//записываем что творит пользователь
        {
            try
            {
                taActivity.Insert(user, DateTime.Now, action, addText);
                    //user, DateTime.Now, action + " " + addText);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        internal List<ListViewItem> GetPlane(int issue)
//получаем список раскладок для издания
//чуть непонятно, это решается через создание биндинга
        {
            List<ListViewItem> tmplList = new List<ListViewItem>();

            var planes = from templates in tblTemplates.AsEnumerable()
                         where templates.issue_id == issue
                         select templates.name;

            if (planes == null)
                return null;

            foreach (string s in planes)
            { 
                ListViewItem lvi = new ListViewItem();
                lvi.Text = s;
                tmplList.Add(lvi);
            }
            return tmplList;
        }

        internal DataTable ViewSections(int type, string ed_name)
//получение списка секций из базы для просмотра
//сделано, чтобы объёдинить секции и название выпуска
        {
            DataTable rTable = new DataTable();
            rTable.Columns.Add("Код", Type.GetType("System.String"));
            rTable.Columns.Add("Название", Type.GetType("System.String"));
            rTable.Columns.Add("Издание", Type.GetType("System.String"));
            rTable.Columns.Add("Разворот", Type.GetType("System.Boolean"));
            rTable.Columns.Add("Из Atex", Type.GetType("System.Boolean"));
            rTable.Columns.Add("Последнее изменение", Type.GetType("System.DateTime"));
            rTable.Columns.Add("Комментарий", Type.GetType("System.String"));

            var x = from section in tblSections.AsEnumerable()
                    from edition in tblEditions.AsEnumerable()
                    where edition.id == section.edition_id
                    select new { 
                        Code = section.code, 
                        Name = section.name, 
                        edName = edition.name, 
                        Atex = section.Field<bool?>("from_atex") == null ? false : section.from_atex,
                        Spread = section.Field<bool?>("is_spread") == null ? false : section.is_spread, 
                        Access = (section.Field<DateTime?>("last_access") == null ? DateTime.MinValue : section.last_access),
                        Comment = section.Field<string>("comment") == null ? "" : section.comment
                    };

            foreach (var s in x)
            {
                DataRow dRow = rTable.NewRow();

                dRow[0] = s.Code;
                dRow[1] = s.Name;
                dRow[2] = s.edName;
                dRow[3] = s.Spread;
                dRow[4] = s.Atex;
                dRow[5] = s.Access;
                dRow[6] = s.Comment;

                switch (type)
                {
                    case 0:
                        rTable.Rows.Add(dRow);
                        break;
                    case 1:
                    //только "Не из Атекса"
                        if ((bool)dRow[4] == false)
                            rTable.Rows.Add(dRow);
                        break;
                    case 2:
                        //только "Сегодня"
                        if ((string)dRow[2] == ed_name)
                            rTable.Rows.Add(dRow);
                        break;
                }
            }

            return rTable;
        }

        internal List<string> GetEditionsList()
//возвращает список изданий
        { 
            List<string> retList = new List<string>();
            var x = from edition in tblEditions
                    select edition.name;

            foreach(string s in x)
            {
                retList.Add(s);
            }
            return retList;
        }

        internal int GetIssueIDbyName(string name)
//возвращает номер выпуска по его имени
        {
            var x = from issues in tblIssues.AsEnumerable()
                    where issues.name == name
                    select issues.id;

            return ((int)x.FirstOrDefault());
        }

        internal List<string> GetIssuesByEditionID(int edition_id)
//возвращает список выпусков
        {
            List<string> retList = new List<string>();
            var x = from issue in tblIssues.AsEnumerable()
                    where issue.edition_id == edition_id
                    select issue.name;

            foreach (string s in x)
            {
                retList.Add(s);
            }
            return retList;
        }

        internal List<string> GetIssuesList(int edition_id, bool enabled)
//возвращает список выпусков
        {
            List<string> retList = new List<string>();
            var x = from issue in tblIssues.AsEnumerable()
                    where issue.edition_id == edition_id && issue.Field<bool?>("enabled") == enabled
                    select issue.name;

            return x.ToList();
        }

        internal string GetIssueCodebyID(int issue_id)
//возвращает код выпуска по id
        {
            var x = from issue in tblIssues
                    where issue.id == issue_id
                    select issue.code;

            return x.FirstOrDefault();
        }

        internal int GetIssueIDbyCode(string code)
//возвращает id выпуска по коду
        {
            var x = from issue in tblIssues
                    where issue.code == code
                    select issue.id;

            return x.FirstOrDefault();
        }

        internal string GetIssuesNamebyID(int issue_id)
//возвращает название выпуска по id
        {
            var x = from issue in tblIssues
                    where issue.id == issue_id
                    select issue.name;

            return x.FirstOrDefault();
        }

        internal int GetEditionIDbyIssueID(int issue_id)
//получаем id издания по id выпуска
        {
            var x = from issue in tblIssues.AsEnumerable()
                    where issue.id == issue_id
                    select issue.edition_id;
            if (!x.Any())
                return -1;
            return Convert.ToInt32(x.First());
        }

        internal int GetEditionIDbyEditionName(string edition_name)
//получаем id издания по названию издания
        {
            var x = from edition in tblEditions.AsEnumerable()
                    where edition.name == edition_name
                    select edition.id;
            if (x == null)
                return -1;
            return Convert.ToInt32(x.First());
        }

        internal string GetEditionCodebyID(int edition_id)
//получаем код издания по id
        {
            var x = from edition in tblEditions.AsEnumerable()
                    where edition.id == edition_id
                    select edition.code;
            //if (!x.Any())
            //    return "";
            return x.FirstOrDefault();
        }

        internal string GetEditionNamebyID(int edition_id)
//получаем название издания по id
        {
            var x = from edition in tblEditions.AsEnumerable()
                    where edition.id == edition_id
                    select edition.name;

            return x.FirstOrDefault();
        }

        internal List<ListViewItem> GetLinks(int templateId, int issueId)
//получение связанных секций для издания
//входит id шаблона и id выпуска
//если issueId -1 - возвращает все дубли для этого шаблона
//что выходит - пока не понятно
        {
            IEnumerable<MainDBDataSet.linksectionsRow> x = (issueId != -1 ?
                                                        from links in tblLinkSections.AsEnumerable()
                                                        where links.issue_id == issueId && links.template_id == templateId
                                                        select links
                                                        :
                                                        from links in tblLinkSections.AsEnumerable()
                                                        where links.issue_id == issueId
                                                        select links);

            List<ListViewItem> outResult = new List<ListViewItem>();

            foreach (MainDBDataSet.linksectionsRow row in x)
            {
                ListViewItem o = new ListViewItem();

                o.ImageKey = GetSectionCodeByID(row.section_id);
                o.Text = Startup.CreateName(row.page_num);
                o.ToolTipText = GetSectionNameByID(row.section_id);
                o.Name = GetSectionNameByID(row.section_id);
                outResult.Add(o);
            }
            return outResult;
        }

        internal int FindLink(int templateId, int issueId, int pageNum)
//получение id секции для дублирования внутри выпуска
//входят id шаблона, id выпуска и номер полосы
        {
            var x = (pageNum != -1
                    ?
                    from link in tblLinkSections.AsEnumerable()
                    where link.issue_id == issueId
                    && link.template_id == templateId
                    && link.page_num == pageNum
                    select link.source_page_num
                    :
                    from link in tblLinkSections.AsEnumerable()
                    where link.issue_id == issueId
                    && link.template_id == templateId
                    select link.source_page_num);

            if (x.Any())
                return (int)x.First();
            else
                return -1;
        }

        internal int AddLink(int templateId, int issueId, int pageNum, int sectionId, int sourcePageNum, int sourceIssueId)
//создание записи в таблице связей секций
        {
            //ахтунг! предусмотреть чистку комментария о дублировании если полоса удаляется
            //а признак удаления - (pNum == -1)
            int ret = -1;
            
            RemoveLink(templateId, issueId, sourcePageNum, pageNum);

            //if (pageNum == -1)
            //    return -1;

            //if (FindLink(templateId, issueId, pageNum) != -1)
            //{
            //    //предусмотреть обновление комментария о дублировании
            //    ret = UpdateLink(templateId, issueId, pageNum, sectionId, sourcePageNum, sourceIssueId);
            //}
            //else 
            if(pageNum != -1)
            {
                MainDBDataSet.linksectionsRow linkRow = mainDBdataset.linksections.NewlinksectionsRow();

                linkRow.section_id = sectionId;
                linkRow.page_num = pageNum;
                linkRow.issue_id = issueId;
                linkRow.last_access = DateTime.Now;
                linkRow.source_page_num = sourcePageNum;
                linkRow.template_id = templateId;
                linkRow.source_issue_id = sourceIssueId;

                tblLinkSections.AddlinksectionsRow(linkRow);
                taLinkSections.Update(tblLinkSections);
                RefreshTableAdapters();
                ret = linkRow.id;
            }
            MakeComentForDoblePages(pageNum, issueId, sourceIssueId, sourcePageNum, templateId);
            return ret;
        }

        private void MakeComentForDoblePages(int target_PageNum, int target_IssueId, int source_IssueId, int source_PageNum, int templateId)
        //добавление информации о дублировании полосы к полосе, на которую идёт дубль
        {
            var x = from row in tblLayouts.AsEnumerable()
                    where row.issue_id == target_IssueId &&
                    row.page_num == target_PageNum &&
                    row.template_id == templateId
                    select row;

            MainDBDataSet.layoutsRow r = x.FirstOrDefault();
            if (r == null)
                return;

            string comment = r.Field<string>("comment");
            string h = (target_PageNum == -1 ? null : Startup.Roles.Editor.ToLower() + "\n" + "Дубль полосы " + source_PageNum + " из выпуска \"" + GetIssuesNamebyID(source_IssueId) + "\"" + Startup.splitter);
            string tmpComment = null;

            if (comment != null && comment.ToLower().Contains(Startup.Roles.Editor.ToLower()))// + Startup.splitter))
            {
                string[] CommentBloks = comment.Split(new char[] { Startup.splitter }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < CommentBloks.Length; i++)
                {
                    if (CommentBloks[i].ToLower().Contains(Startup.Roles.Editor.ToLower()))// + Startup.splitter))
                    {
                        CommentBloks[i] = h;
                    }

                    tmpComment = tmpComment + "\n" + CommentBloks[i].TrimStart('\n');//Substring(1) == "\n" ? CommentBloks[i].Substring(1, CommentBloks[i].Length - 1) : CommentBloks[i]);
                }
            }
            else
            {
                tmpComment = tmpComment + h + "\n" + comment;
            }
            UpdateComment(templateId, target_IssueId, target_PageNum, tmpComment + Startup.splitter);
        }

        internal void RemoveLink(int templateId, int issueId, int sourcePageNum, int pageNum)
//удаление информации о связи
        {
            var x = (pageNum > 0 ? //pageNum == 1
                    from links in tblLinkSections.AsEnumerable()
                    where links.template_id == templateId
                    && links.issue_id == issueId
                    //&& links.source_page_num == sourcePageNum
                    && links.page_num == pageNum
                    select links
                    :
                    from links in tblLinkSections.AsEnumerable()
                    where links.template_id == templateId
                    && links.issue_id == issueId
                    && links.source_page_num == sourcePageNum
                    select links);

            foreach (MainDBDataSet.linksectionsRow s in x)
            {
                try
                {
                    s.Delete();
                }
                catch (Exception ex)
                {}
            }
            try
            {
                taLinkSections.Update(tblLinkSections);
            }
            catch (Exception)
            { }
            RefreshTableAdapters();
            return; 
        }

        internal int UpdateLink(int templateId, int issueId, int pageNum, int sectionId, int sourcePageNum, int sourceIssueId)
//обновление информации о связи
        {
            var x = from links in tblLinkSections.AsEnumerable()
                    where links.template_id == templateId
                    && links.issue_id == issueId
                    && links.page_num == pageNum
                    select links;

            if (x.Any())
            {
                try
                {
                    x.First().SetField<int>("section_id", sectionId);
                    x.First().SetField<int>("source_issue_id", sourceIssueId);
                    tblLinkSections.AcceptChanges();
                    taLinkSections.Update(tblLinkSections);
                    RefreshTableAdapters();
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //else
            //{
            //    return AddLink(templateId, issueId, pageNum, sectionId, sourcePageNum, sourceIssueId);
            //}
            return x.First().Field<int>("id");
        }

        internal Exception UpdateGlobalPlanes(int issueId, int dayOfWeek, int editionId, int templateId)
//обновление таблицы глобального плана
        {
            DataRow x = tblGlobalPlanes.AsEnumerable().Where(p => p.day_of_week == dayOfWeek).Where(v => v.issue_id == issueId).FirstOrDefault();

            if (x != null)
            {
                x.SetField<int>("template_id", templateId);
                x.SetField<DateTime>("last_access", DateTime.Now);
            }
            else 
            {
                try
                {
                    MainDBDataSet.globalplanesRow gpRow = mainDBdataset.globalplanes.NewglobalplanesRow();
                    gpRow.day_of_week = dayOfWeek;
                    gpRow.template_id = templateId;
                    gpRow.last_access = DateTime.Now;
                    gpRow.comment = "";
                    gpRow.issue_id = issueId;
                    gpRow.edition_id = editionId;
                    tblGlobalPlanes.AddglobalplanesRow(gpRow);
                }
                catch (Exception d)
                {
                    return d;
                }
            }
            taGlobalPlane.Update(tblGlobalPlanes);
            return null;
        }

        internal Exception UpdateGlobalColors(int issueId, int dayOfWeek, int editionId, int schemeId)
        //обновление таблицы глобального плана
        {
            DataRow x = tblGlobalColors.AsEnumerable().Where(p => p.day_of_week == dayOfWeek).Where(v => v.issue_id == issueId).FirstOrDefault();

            if (x != null)
            {
                x.SetField<int>("scheme_id", schemeId);
                x.SetField<DateTime>("last_access", DateTime.Now);
            }
            else
            {
                try
                {
                    MainDBDataSet.globalcolorsRow gpRow = mainDBdataset.globalcolors.NewglobalcolorsRow();
                    gpRow.day_of_week = dayOfWeek;
                    gpRow.scheme_id = schemeId;
                    gpRow.last_access = DateTime.Now;
                    gpRow.comment = "";
                    gpRow.issue_id = issueId;
                    gpRow.edition_id = editionId;
                    tblGlobalColors.AddglobalcolorsRow(gpRow);
                }
                catch (Exception d)
                {
                    return d;
                }
            }
            taGlobalColors.Update(tblGlobalColors);
            return null;
        }

        internal DataTable ViewIssues()
//получение списка выпусков таблицей из базы для просмотра
//сделано, чтобы объёдинить секции и название выпуска
        {
            DataTable rTable = new DataTable();
            rTable.Columns.Add("Код", Type.GetType("System.String"));
            rTable.Columns.Add("Название", Type.GetType("System.String"));
            rTable.Columns.Add("Издание", Type.GetType("System.String"));
            rTable.Columns.Add("Используется", Type.GetType("System.Boolean"));
            rTable.Columns.Add("Последнее изменение", Type.GetType("System.DateTime"));
            rTable.Columns.Add("Комментарий", Type.GetType("System.String"));

            var x = from issues in tblIssues.AsEnumerable()
                    from edition in tblEditions.AsEnumerable()
                    where edition.id == issues.edition_id
                    select new
                    {
                        Code = issues.code,
                        Name = issues.name,
                        edName = edition.name,
                        Active = (issues.Field<bool?>("enabled") == null ? false : issues.enabled),
                        Access = (issues.Field<DateTime?>("last_access") == null ? DateTime.MinValue : issues.last_access),
                        Comment = issues.Field<string>("comment") == null ? "" : issues.comment
                    };

            foreach (var s in x)
            {
                DataRow dRow = rTable.NewRow();

                dRow[0] = s.Code;
                dRow[1] = s.Name;
                dRow[2] = s.edName;
                dRow[3] = s.Active;
                dRow[4] = s.Access;
                dRow[5] = s.Comment;

                rTable.Rows.Add(dRow);
            }

            return rTable;
        }

        internal void FoundUser()
//определяем права пользователя
        {
            var x = from user in tblUsers.AsEnumerable()
                    where user.name == Startup.User
                    select new
                    {
                        user.role,
                        user.editions
                    };

            if (x.Any())
            {
                Startup.Edition = GetEditionIDbyEditionName(x.First().editions);
                Startup.UserRole = x.First().role.ToString().ToLower();
            }
            else
            {
                Startup.Edition = GetEditionIDbyEditionName("Сегодня");
                Startup.UserRole = "guest";
            }
        }

        internal List<string> FoundRoles()
//получаем строку с перечислением ролей пользователей и возвращаем массив
        {
            var x = from roles in tblConfig.AsEnumerable()
                    where roles.parametr == "roles"
                    select roles.value.ToString();

            List<string> sOut = new List<string>();

            if (x.Any())
            {
                string s = x.First().ToString();
                sOut = s.Split(new char[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return sOut;
        }

        internal void UpdateComment(int planeId, int issueId, int pageNum, string commentText)
//обновление комментария к полосе
//входят id шаблона, id выпуска, номер страницы, сам комментарий
        {
            IEnumerable<MainDBDataSet.layoutsRow> x = from lay in tblLayouts.AsEnumerable()
                                                      where lay.template_id == planeId &&
                                                      lay.page_num == pageNum &&
                                                      lay.issue_id == issueId
                                                      select lay;

            x.First().SetField<string>("comment", commentText);
            taLayouts.Update(tblLayouts);
            RefreshTableAdapters();
        }

        internal Dictionary<string, string> GetPathsSettings()
//получение из базы путей, необходимых для работы дизайнера
//входит имя параметра
        {
            var x = from row in tblConfig
                    where row.Field<string>("comment") != null && row.comment == "folder"
                    select row;

            Dictionary<string, string>res = new Dictionary<string,string>();

            foreach (var s in x)
            {
                res.Add(s.parametr, s.value);
            }

            return res;
        }

        internal bool SaveColorScheme(int edition_id, int issue_id, string name, int[] scheme)
        //сохраняем схему цветности
        //входят id выпуска, id издания, название, массив значений
        {
            int id = -1;
            DataRow x = tblColorSchemes.AsEnumerable().Where(p => p.edition_id == edition_id).Where(v => v.issue_id == issue_id).Where(n => n.name == name).FirstOrDefault();

            id = x.Field<int>("id");

            if (id == -1) return false;

            for (int i = 0; i < scheme.Length; i++)
            {
                DataRow z = tblColorSettings.AsEnumerable().Where(p => p.scheme_id == id).Where(v => v.page_num == i+1).FirstOrDefault();

                if (z != null)
                {
                    z.SetField<bool>("is_color", Convert.ToBoolean(scheme[i]));
                    z.SetField<DateTime>("last_access", DateTime.Now);
                    z.SetField<string>("comment", Startup.User);
                }
                else
                {
                    MainDBDataSet.colorsRow colorsRow = mainDBdataset.colors.NewcolorsRow();

                    colorsRow.comment = "";
                    colorsRow.last_access = DateTime.Now;
                    colorsRow.page_num = i + 1;
                    colorsRow.is_color = Convert.ToBoolean(scheme[i]);
                    colorsRow.scheme_id = id;

                    tblColorSettings.AddcolorsRow(colorsRow);
                }
            }

            RemoveColorsRow(id, scheme.Length);
            taColorSettings.Update(tblColorSettings);

            return true;
        }

        internal void RemoveColorsRow(int scheme_id, int maxNumber)
        //удаление лишних записей при уменьшении количества страниц в схеме цветности
        //входит id схемы и количество страниц
        //если в количестве 0, то удаляет всё по заданной схеме
        {
            IEnumerable<MainDBDataSet.colorsRow> x = from rows in tblColorSettings.AsEnumerable()
                                                     where rows.scheme_id == scheme_id && rows.page_num > maxNumber
                                                     select rows;

                foreach (MainDBDataSet.colorsRow s in x)
                {
                    try
                    {
                        s.Delete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show (ex.Message);
                    }
                }
        }

        internal void GetCurrentIssueNum()
        //получение номера текущего выпуска
        {
            IEnumerable<MainDBDataSet.issue_numRow> x = from row in tblIssueNum.AsEnumerable()
                                                        where row.data.Date == Startup.globalDate.Date
                                                        select row;

            if (x.Any())
            {
                Startup.issueNum = x.FirstOrDefault().small_num;
                Startup.issueBigNum = x.FirstOrDefault().big_num;
            }
            else
            {
                Startup.issueNum = -1;
                Startup.issueBigNum = -1;
            }            
        }

        internal void GetNewIssueNum()
        //получение номера выпуска на текущую дату
        //если номер не найден, уменьшает дату на один день и заново запрашивает базу
        //счётчик count предохраняет от бесконечного цикла
        {
            DateTime tmpDt = Startup.globalDate;
            int count = 0;

            while (Startup.issueNum == -1 && tblIssueNum.Rows.Count > count++)
            {
                Startup.globalDate = Startup.globalDate.AddDays(-1);
                GetCurrentIssueNum();
            }

            Startup.globalDate = tmpDt;
        }

        internal void SaveIssueNum()
        //добавление в базу номера выпуска
        {
            try
            {
                MainDBDataSet.issue_numRow z = mainDBdataset.issue_num.Newissue_numRow();

                z.big_num = Startup.issueBigNum;
                z.small_num = Startup.issueNum;
                z.data = Startup.globalDate;
                tblIssueNum.Addissue_numRow(z);
                taIssueNum.Update(tblIssueNum);
            }
            catch(Exception){};
        }

        internal int GetColorShemeId(int dayOfWeek, int issueId)
        //получение id цветовой схемы по дню неделю и id выпуска
        {
            IEnumerable<int> x = from scheme in tblGlobalColors.AsEnumerable()
                                 where scheme.day_of_week == dayOfWeek && scheme.issue_id == issueId
                                 select scheme.scheme_id;
            return x.FirstOrDefault();
        }

        internal int[] GetColorsForSheme(int scheme_id)
        //получение массива цветности страниц по id схемы
        {
            IEnumerable<int> x = from col in tblColorSettings
                    where col.scheme_id == scheme_id
                    orderby col.page_num
                    select Convert.ToInt32(col.is_color);

            return x.ToArray();
        }

        internal int GetColorForPage(int pNum, int issueId, int DayOfWeek)
        //получение информации о цвете страницы
        //входят номер полосы, id выпуска, номер дня недели
        {
            IEnumerable<bool> x = from ctable in tblColorSettings.AsEnumerable()
                                                     join globtable in tblGlobalColors.AsEnumerable() on ctable.scheme_id equals globtable.scheme_id
                                                     where globtable.day_of_week == DayOfWeek
                                                     && globtable.issue_id == issueId
                                                     && ctable.page_num == pNum
                                                     select ctable.is_color;

            if (!x.Any())
                return -1;
            else
                return Convert.ToInt32(x.First());
        }

        internal void SendPDF(string fileName, bool original)
        //сохраняем информацию об отправленной полосе
        //через SQL потому что нужно иметь информацию всем пользователям мгновенно
        //а не ждать автообновления таблиц в памяти
        {
            SqlConnection pdfTable = new SqlConnection(Startup.APSDBConnectionString);

            var x = from issue in tblIssues.AsEnumerable()
                    join edition in tblEditions.AsEnumerable() on issue.edition_id equals edition.id
                    where issue.code == fileName.Split(new char[] { '_' })[2]
                    select new
                    {
                        Edition = edition.id,
                        Issue = issue.id
                    };

            
            try
            {
                pdfTable.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Send PDF base error");
                return;
            }
            SqlCommand addRow = new SqlCommand();
            addRow.CommandText = "INSERT INTO pdffiles (page_name, send_time, owner, is_original, issue_id, edition_id, page_num) VALUES (@fName, @time, @owner, @orig, @issue, @edition, @page_num)";
            addRow.Parameters.AddWithValue("@fName", fileName);
            addRow.Parameters.AddWithValue("@orig", original);
            addRow.Parameters.AddWithValue("@time", DateTime.Now);
            addRow.Parameters.AddWithValue("@owner", Startup.User);
            if (x.Any())
            {
                addRow.Parameters.AddWithValue("@issue", Convert.ToInt32(x.ElementAt(0).Issue));
                addRow.Parameters.AddWithValue("@edition", Convert.ToInt32(x.ElementAt(0).Edition));
            }
            else
            {
                addRow.Parameters.AddWithValue("@issue", -1);
                addRow.Parameters.AddWithValue("@edition", -1);
            }
            addRow.Parameters.AddWithValue("@page_num", Convert.ToInt32(fileName.Split(new char[]{'_'})[3]));
            addRow.Connection = pdfTable;
            addRow.ExecuteNonQuery();

            pdfTable.Close();
        }

        internal string GetVersion()
        //получение версии приложения для недопущения работы старых версий
        {
            IEnumerable<string> x = from val in tblConfig.AsEnumerable()
                                                     where val.parametr.ToLower() == "version"
                                                     select val.value.ToString();

            return x.FirstOrDefault();
        }

        internal void ColorsStatuses()
        { 

        }
//1.0.0.86
        internal void GetDeadline()
        {
            IEnumerable<MainDBDataSet.deadlinesRow> dl = from deadines in tblDeadlines.AsEnumerable()
                                       where deadines.day_of_week == (int)Startup.globalDate.DayOfWeek
                                       select deadines;

            if (!dl.Any())
                return;
            else
            {
                foreach (Startup.issue issue in Startup.issuesList)
                    try
                    {
                        issue.DeadLine = dl.First(p => p.issue_id == issue.Id).time;
                    }
                    catch { }
            }

            
        }

        internal int CountSendedPDF(string fName)
        {
            var pdfCount = from pdftable in tblPDF.AsEnumerable()
                           where pdftable.page_name.Contains(fName)
                           select pdftable;

            return pdfCount.Count();
        }

        internal bool CheckActiveIssue(int issue_id)
        {
            var x = from templ in tblTemplates.AsEnumerable()
                    where templ.name == Startup.globalDate.ToShortDateString()
                    && templ.issue_id == issue_id
                    select templ;

            bool res = x.Any();
            return (res);
        }

    }
}