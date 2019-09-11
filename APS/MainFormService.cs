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
using System.IO;
using System.Diagnostics;

namespace APS
{
    public partial class MainForm
    //всякие служебные функции при запуске программы
    {            
        string programmAcrobat = "";
        string fileMask = "";

        internal void viewPDF_Click(object sender, EventArgs e)
        //открываем PDF в Acrobat
        {
            string code;
            ListView s;

            if (Startup.Paths == null)
            {
                StatusText("Нет доступа!", 1);
                return;
            }

            if (fileMask == "")
            {
                fileMask = Startup.globalDate.ToString("yy") + Startup.globalDate.Month.ToString("D02") + Startup.globalDate.Day.ToString("D02") + "_" + Startup.myData.GetEditionCodebyID(Startup.Edition);
            }

            try
            {
                ToolStripItem tsi = (ToolStripItem)sender;
                ContextMenuStrip owner = (ContextMenuStrip)tsi.Owner;
                s = (ListView)owner.SourceControl;
            }
            catch (Exception)
            {
                s = (ListView)sender;
            }
                    
            try
            {
                code = Startup.myData.GetIssueCodebyID(Convert.ToInt32(s.Tag.ToString()));
            }
            catch (Exception)
            {
                StatusText("Ошибка получения Id выпуска");
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            CheckAcrobat();

            //string outPath = Startup.Paths.GetValue("outputPDF");
            string inPath = Startup.Paths.GetValue("inputPDF");
            if (inPath == "")
            {
                StatusText("Ошибка получения входного пути для PDF");
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            for (int i = 0; i < s.SelectedItems.Count; i++)
            {
                string fName = fileMask + "_" + code + "_" + s.SelectedItems[i].Text.Trim() + ".PDF";

                if (!File.Exists(inPath + "\\" + fName))
                {
                    inPath = inPath + "\\old\\" + Startup.issueNum.ToString();
                    if (Startup.issueNum == -1 || !File.Exists(inPath + "\\" + fName))
                    {
                        StatusText("Файл " + fName + " не найден");
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                    if (MessageBox.Show("Файл находится в архиве! Всё равно открыть?", "Внимание!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                fName = inPath + "\\" + fName;
                Process.Start(programmAcrobat, fName);
                Startup.myData.Write_Activity(Startup.User, "view_PDF", fName);
            }
        }

        private void CheckAcrobat()
        //поверяем на наличие акробата в системе
        {
            if (programmAcrobat != "")
                return;
            try
            {
                programmAcrobat = (Startup.Paths.GetValue("acrobat"));
                if (programmAcrobat == "")
                {
                    MessageBox.Show("В настройках не найден путь к Acrobat!");
                    return;
                }
                programmAcrobat = programmAcrobat + "\\Acrobat.exe";
                if (!File.Exists(programmAcrobat))
                {
                    MessageBox.Show("Не найден Acrobat!");
                    return;
                }
            }
            catch (Exception) 
            {
                StatusText("Нет доступа!", 1);
            }
        }

        private void PathSettings()
        //считывание путей к папкам и программам
        {
            Startup.Paths = new RegSettings("Paths");
            if (Startup.Paths.settingsList.Count == 0)
            //если в регистре не найдено - считываем из базы и записываем в регистр
            {
                Dictionary<string, string> paths = Startup.myData.GetPathsSettings();

                if (paths.Count == 0)
                {
                    StatusText("Ошибка настроек программмы. Не найдены пути для работы с PDF файлами.");
                    return;
                }

                foreach (KeyValuePair<string, string> s in paths)
                {
                        if (s.Key.Contains("acrobat"))
                        {
                            if (Directory.Exists(s.Value))
                            {
                                Startup.Paths.AddKeys("acrobat", s.Value);
                            }
                        }
                        else
                            Startup.Paths.AddKeys(s.Key, s.Value);
                }
                Startup.Paths.WriteINI();
            }

            foreach (RegSettings.Keys rs in Startup.Paths.settingsList)
            {
                if (!Directory.Exists(rs.value))
                {
                    MessageBox.Show("Внимание! Путь для " + rs.name + " не найден!");
                }
            }
        }

        private void CheckFormSize()
        //проверяем, чтобы окно не вывлилось за пределы экрана
        { 
            int w = this.Width;
            int h = this.Height;
            int x = this.Location.X;
            int y = this.Location.Y;
            
            if (this.Size.Width > (int)ScreenWidth * 1.05)
                w = (int)(ScreenWidth * 0.75);
            if (this.Size.Height > (int)ScreenHeight * 1.05)
                h = (int)(ScreenHeight * 0.75);
            this.Size = new Size(w, h);

            if (x + w < 0 || x > (int)ScreenWidth)
                x = 1;
            if (y + h < 0 || y > (int)ScreenHeight)
                y = 1;
            this.Location= new Point(x, y);
        }

        private void sendPDF_Click(object sender, EventArgs e)
        //отправка PDF
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
                    Startup.mainPlane.SendPDF(s);
                }
            }
        }

        private void patchsButton_Click(object sender, EventArgs e)
        {
            SettingsChange sWin = new SettingsChange();

            if (sWin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                StatusText("Настройки обновлены.");
        }

        private void CheckPlanesVsColors()
        //проверяем соответствие количества полос в планах и схемах цветности
        {
            foreach (Startup.issue s in Startup.issuesList)
            { 
                int[] x = Startup.myData.GetColorsForSheme(Startup.myData.GetColorShemeId((int)Startup.globalDate.DayOfWeek, s.Id));
                if (s.Pages != x.Length && !s.Name.ToLower().Contains("Спец".ToLower()))
                {
                    MessageBox.Show("Количество страниц в выпуске \n" + s.Name +
                        "\nне соответствует количеству полос, \n указанному в схеме цветности!\nОтправка в типографию будет заблокирована!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rClickDesignerMenu.Items[2].Enabled = false;
                    return;
                }
            }
            rClickDesignerMenu.Items[2].Enabled = true;
        }

    }
}
