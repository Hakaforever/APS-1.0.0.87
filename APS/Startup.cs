using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace APS
{
    public sealed class Startup
    {
        //класс, в котором планируется хранение общих настроек 
        //и экземпляров общих классов, необходимых в работе всем объектам пограммы

        internal static DataAccess myData { get; set; } //экземпляр для доступа к базе данных

        internal static DrawPlane mainPlane { get; set; } //экземпляр для рисования планов и секций

        internal static Atex myAtex { get; set; } //экземпляр класса для доступа к базе Атекса

        internal static int counter = 0;

        internal static RegSettings Settings;
        internal static RegSettings Paths;

        internal static char splitter = Convert.ToChar(17);

        internal static int issueNum = -1;
        internal static int issueBigNum = -1;

        public static string User { get; set; } //пользователь
        public static string UserRole { get; set; } //роль пользователя
        public static int Edition { get; set; } //ID издания

        public static bool anyChanges { get; set; }//регистрация изменений

        //1.0.0.85 
        public static DateTime loginTime { get; set; } //дата входа в систему
        public static bool baseChanged = false;
        //1.0.0.86
        //public static TimeSpan deadLine { get; set; } //дедлайн
        public static DateTime globalDate { get; set; } //глобальная дата

        public static class Roles
        {
            public const string Admin = "admin";
            public const string Designer = "designer";
            public const string Guest = "guest";
            public const string Advert = "advert";
            public const string Editor = "editor";
            public const string Circulation = "circulation";

            public static string[] roles = new string[6] { Admin, Designer, Guest, Advert, Editor, Circulation };
        }

        public static string APSDBConnectionString = "Data Source=ATEXDB-01;Initial Catalog=APS-DB;Persist Security Info=True;User ID=rouser;Password=Atex~12345";

        public class issue
        //информация о выпусках
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public int Pages { get; set; } //количество старниц в выпуске
            public string Code { get; set; }
            public TimeSpan DeadLine { get; set; }
            public bool Active { get; set; }
        }

        internal static List<issue> issuesList = new List<issue>(); //список всех выпусков

        public static void FillIssuesList()
//заполнение списка выпусков по изданию, полученному при логине пользователя 
        {
            issuesList.Clear();
            List<string> issues_tmp = myData.GetIssuesList(Edition, true);

            if (issues_tmp != null)
            {
                for (int i = 0; i < issues_tmp.Count; i++)
                {
                    issue tmpIssue = new issue();
                    tmpIssue.Name = issues_tmp[i];
                    tmpIssue.Id = myData.GetIssueIDbyName(issues_tmp[i]);
                    tmpIssue.Code = myData.GetIssueCodebyID(tmpIssue.Id);
                    tmpIssue.Pages = 0;
                    tmpIssue.DeadLine = TimeSpan.Zero;
                    tmpIssue.Active = Startup.myData.CheckActiveIssue(tmpIssue.Id);
                    issuesList.Add(tmpIssue);
                }
            }
        }

        public static Point Location(Control control)
        //вычисляем координаты для построения окна
        {
            Screen p = Screen.FromControl(control);
            int screenW = mainPlane.myOwner.ScreenWidth;
            int screenH = Screen.GetBounds(control).Height;

            Point curPosition = control.PointToClient(Cursor.Position);

            int x = curPosition.X;
            int y = curPosition.Y;
            int width = control.Size.Width;
            int height = control.Size.Height;

            Point retLoc = new Point();

            retLoc.X = (screenW < (x + width) ? screenW - width - 20 : x + 10);
            retLoc.Y = (screenH < (y + height) ? screenH - height -20 : y + 10);
            
            return retLoc;
        }

        public static string issueDate()
        //превращение даты в строку для обработки полос выпусков
        {
            if (globalDate != null)
                return Startup.globalDate.Year.ToString().Substring(2) + Startup.globalDate.Month.ToString("D2") +  Startup.globalDate.Day.ToString("D2");
            else
                return null;
        }

        public static string CreateName(int inName)
        //универсальный создатель имени для элемента ListView
        {
            if (inName > 0)
                return "    " + inName.ToString("D2") + "    ";
            return null;
        }

    }
}
