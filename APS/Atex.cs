using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace APS
{
    class Atex
    //обращение к базе Атекса за статусами, формирование списка статусов по выпускам
    {
        SqlConnection atexConnect;
        private string connectionString = Properties.Settings.Default.PrestigeConnectionString;
        //public static Dictionary<string, int> StatusMark = new Dictionary<string, int>();
        public static List <string> StatusMark = new List<string>{"1043", "1010", "1011", "1012", "1013", "1014", "1022", "Отправлен", "Wait", "Alert", "1043-1"};

        public class sInfo
        //класс с информацией о статусе полосы
        {
            int pNum; //номер страницы
            int status; //статус
            string fName; //имя файла
            int data_no; //dbo.prdatareference.data_no - если 2, то это разворот
            string code; //код региона из имени файла
            int issue_id; //id выпуска 

            public sInfo(int pNum, string status, string fName, int data_no)
            //инициализатор
            {
                this.pNum = pNum;
                //if (!StatusMark.TryGetValue(status, out this.status))
                //    this.status = "empty";
                this.status = StatusMark.IndexOf(status);
                //this.status = status;
                this.fName = fName;
                this.data_no = data_no;
                this.code = fName.Substring(11, 3);
                this.issue_id = Startup.myData.GetIssueIDbyCode(this.code);
            }

            //public void MakeSpread(string status, int pNum)
            public void MakeSpread(int pNum, int status)
            {
                //this.status = status;
                this.pNum = pNum;
                this.Status = status;
            }

            public int PageNum { get { return pNum; } }
            public int Status { get { return status; } set{status = value;} }
            public string FileName { get { return fName; } }
            public int Data_No { get { return data_no; } }
            public string Code { get { return code; } }
            public int IssueId { get { return issue_id; } }
        }

        public List<sInfo> statusesList = new List<sInfo>();
        //список статусов выпуска

        public Atex()
        {
            //StatusMark.Add("1043", 0);//System.Drawing.Color.White);//пустое
            //StatusMark.Add("1010", 1);//System.Drawing.Color.Yellow);//макетирование
            //StatusMark.Add("1011", 2);//System.Drawing.Color.DarkOrange);//в работе
            //StatusMark.Add("1012", 3);//System.Drawing.Color.Lime);//контроль полос
            //StatusMark.Add("1013", 4);//Создание PDF
            //StatusMark.Add("1014", 5);//System.Drawing.Color.Black);//проверка PDF
            //StatusMark.Add("1022", 6);//System.Drawing.Color.Red);//архив
        }

        public void GetStatuses()
        {
            if (Read())
            { 
                for (int i = 0; i < Startup.issuesList.Count; i++)
                {
                    int count = Count(Startup.issuesList[i].Code);
                    IEnumerable<sInfo> x = from item in statusesList.AsEnumerable()
                                           where item.Data_No == 2 && item.Code == Startup.issuesList[i].Code
                                           select item;

                    foreach (sInfo s in x)
                    {
                        int pNum = s.PageNum + 1;
                        int y = statusesList.FindIndex(p => p.PageNum == pNum && p.Code == Startup.issuesList[i].Code);
                        if (y != -1)
                        {
                            pNum = count - s.PageNum + 1;
                            y = statusesList.FindIndex(p => p.PageNum == pNum && p.Data_No == 1 && p.Code == Startup.issuesList[i].Code);
                        }
                        s.MakeSpread(pNum, statusesList[statusesList.FindIndex(p => p.PageNum == s.PageNum && p.Code == Startup.issuesList[i].Code)].Status);
                    }
                }
            }
        }

        private void OpenConnection()
        //открытие базы Атекса
        {
            atexConnect = new SqlConnection(connectionString + ";Password=Atex~12345");

            try
            {
                atexConnect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close()
        //закрытие базы Атекса
        {
            if (atexConnect != null && atexConnect.State != ConnectionState.Closed)
            {
                atexConnect.Close();
            }
        }

        private void ReadSendedBase()
        //здесь читаем нашу базу на предмет уже отправленных файлов
        {
            SqlConnection pdfConnect = new SqlConnection(Startup.APSDBConnectionString);

            SqlCommand readData = new SqlCommand();
            readData.CommandText = "SELECT DISTINCT page_name FROM pdffiles WHERE page_name LIKE @data AND is_original = 'True'";
            readData.Connection = pdfConnect;
            readData.Parameters.AddWithValue("@data", Startup.issueDate() + "%");

            SqlCommand readData2 = new SqlCommand();
            readData2.CommandText = "SELECT DISTINCT page_name FROM pdffiles WHERE page_name LIKE @data AND is_original = 'False'";
            readData2.Connection = pdfConnect;
            readData2.Parameters.AddWithValue("@data", Startup.issueDate() + "%");

            SqlDataReader myReader;

            try
            {
                pdfConnect.Open();
            }
            catch 
            {
                return;
            }

            try
            {
                myReader = readData.ExecuteReader();
                while (myReader.Read())
                {
                    int index = statusesList.FindIndex(p => p.FileName == System.IO.Path.GetFileNameWithoutExtension(myReader.GetValue(0).ToString()));
                    if (index != -1)
                    {
                        if (statusesList[index].Status == StatusMark.FindIndex(p => p == "1014"))
                        {
                            sInfo o = new sInfo(statusesList[index].PageNum, "Отправлен", statusesList[index].FileName, statusesList[index].Data_No);
                            statusesList[index] = o;
                        }
                    }
                }
                if (!myReader.IsClosed)
                myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PDF-files read base error!");
            }

            try
            {
                myReader = readData2.ExecuteReader();
                while (myReader.Read())
                {
                    int index = statusesList.FindIndex(p => p.FileName == System.IO.Path.GetFileNameWithoutExtension(myReader.GetValue(0).ToString()));
                    if (index != -1)
                    {
                        if (statusesList[index].Status == StatusMark.FindIndex(p => p == "1043"))
                        {
                            sInfo o = new sInfo(statusesList[index].PageNum, "1043-1", statusesList[index].FileName, statusesList[index].Data_No);
                            statusesList[index] = o;
                        }
                    }
                }
                if (!myReader.IsClosed)
                myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PDF-files read base error!");
            }

            pdfConnect.Close();

            List<string> outFiles = new List<string>();

            outFiles.AddRange(System.IO.Directory.GetFiles(Startup.Paths.GetValue("outputPDF"), "*.pdf", System.IO.SearchOption.TopDirectoryOnly));
            outFiles.AddRange(System.IO.Directory.GetFiles(Startup.Paths.GetValue("recolorPDF"), "*.pdf", System.IO.SearchOption.TopDirectoryOnly));

            foreach (string s in outFiles)
            {
                int index = statusesList.FindIndex(p => p.FileName == System.IO.Path.GetFileNameWithoutExtension(s).Substring(0, 17));
                if (index != -1)
                {
                    sInfo o = new sInfo(statusesList[index].PageNum, "Wait", statusesList[index].FileName, statusesList[index].Data_No);
                    statusesList[index] = o;
                }
            }

            outFiles.Clear();

            try
            {
                outFiles.AddRange(System.IO.Directory.GetFiles(Startup.Paths.GetValue("inputPDF"), "*.pdf", System.IO.SearchOption.TopDirectoryOnly));
                foreach (string s in outFiles)
                {
                    int index = statusesList.FindIndex(p => p.FileName == System.IO.Path.GetFileNameWithoutExtension(s));
                    if (index != -1)
                    {
                        sInfo o = new sInfo(statusesList[index].PageNum, "1014", statusesList[index].FileName, statusesList[index].Data_No);
                        statusesList[index] = o;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool Read()
        //читаем из базы наши статусы
        {
            bool result = true;

            OpenConnection();

            if (atexConnect == null)
                return false;

            SqlCommand myCommand = new SqlCommand();
            SqlDataReader myReader1;
            string codeEdition = Startup.myData.GetEditionCodebyID(Startup.Edition);

            myCommand.CommandText = "SELECT page_number, status_id, item_name, data_no FROM dbo.pritem LEFT JOIN dbo.prdatareference ON dbo.pritem.item_id = dbo.prdatareference.item_id LEFT JOIN dbo.pritemdata ON dbo.prdatareference.data_id = dbo.pritemdata.data_id WHERE dbo.pritem.item_name LIKE @PageName AND (dbo.prdatareference.data_no > 0 ) AND dbo.pritem.item_name NOT LIKE '%_SC_%' AND dbo.pritem.item_name NOT LIKE '%_BF_%' AND (dbo.prdatareference.data_type=2) ORDER BY item_name, data_no";
            myCommand.Connection = atexConnect;

            statusesList.Clear();

            string pageName = Startup.issueDate() + "_" + codeEdition + "_%"; //предусмотреть если null!!!

            myCommand.Parameters.AddWithValue("@PageName", pageName);

            try
            {
                myReader1 = myCommand.ExecuteReader();
                while (myReader1.Read())
                {
                    sInfo o = new sInfo(Convert.ToInt32(myReader1.GetValue(0)), myReader1.GetValue(1).ToString(), myReader1.GetValue(2).ToString(), (int)myReader1.GetValue(3));
                    statusesList.Add(o);
                }
                if (!myReader1.IsClosed)
                    myReader1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atex base error!");
                result = false;
            }
            Close();
            if (Startup.UserRole == Startup.Roles.Advert)
            {
                ColorsStatuses();
            }
            else
            {
                ReadSendedBase();
            }
            return result;
        }

        private void ColorsStatuses()
        //это для рекламы - вместо статусов Атекса
        //ставит статусы цветности полосы по схеме цветности
        {
            for (int i = 0; i < statusesList.Count; i++)
            {
                int status = Startup.myData.GetColorForPage(statusesList[i].PageNum, statusesList[i].IssueId, (int)Startup.globalDate.DayOfWeek);
                sInfo tempElement = new sInfo(statusesList[i].PageNum, (status != 1 ? "1043" : "1010"), statusesList[i].FileName, statusesList[i].Data_No);
                statusesList[i] = tempElement;
            }
        }

        private int Count(string issCode)
        //считаем количество записей по коду
        {
            IEnumerable<sInfo> x = from item in statusesList.AsEnumerable()
                                   where item.FileName.Contains(issCode)
                                   select item;

            return x.Count();
        }

    }
}
