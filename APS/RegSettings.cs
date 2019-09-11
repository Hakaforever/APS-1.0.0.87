using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace APS
{
    class RegSettings
    {
        RegistryKey adressINI;

        private string keyName;

        int status = 0;

        public class Keys
        {
            public string name;
            public string value;

            public Keys(string _name, string _value)
            {
                name = _name;
                value = _value;
            }

            public void ChangeValue(string _value)
            {
                value = _value;
            }
        };

        public List<Keys> settingsList = new List<Keys>();

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public RegSettings(string Key)
//инициализируем, открываем запись в регистре по ключу
//Key - название папки в основной ветке (например, "Config" - для общих настроек)
        {
            keyName = "Software\\" + Properties.Settings.Default.ProjectName.ToString() + "\\" + Key;

            try
            {
                adressINI = Registry.CurrentUser.OpenSubKey(keyName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            if (adressINI == null)
            {
                Status = 0;
                CreateSettings();
            }
            else
            {
                string[] values = adressINI.GetValueNames();

                Status = (values.Count() == 0 ? 2 : 1);

                for (int y = 0; y < values.Length; y++)
                {
                    settingsList.Add(new Keys(values[y], (string)adressINI.GetValue(values[y])));
                }
            }
        }

        public void AddKeys(string name, string value)
        //добавление записи
        {
            settingsList.Add(new Keys(name, value));
        }

        public void ChangeValue(string name, string value)
        //изменение записи
        {
            foreach (Keys key in settingsList)
            {
                if (key.name == name)
                {
                    key.value = value;
                    Status = 2;
                }
            }
        }

        public string GetValue(string name)
        //получение значения
        {
            var value = from setting in settingsList
                        where setting.name == name
                        select setting.value;

            string result = "";

            if (value.Any())
            {
                result = value.ToArray<string>()[0];
            }
            return result;
        }

        public bool CheckSettings()
        //хрен пойми что это??
        {
            return true;
        }

        public bool ReadINI()
        //читаем настройки и возвращаем успех/неуспех результата
        {
            if (Status == 0)
            {
                DialogResult newINI;

                newINI = MessageBox.Show("Не найдены настройки для программы! Создать новые?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (newINI == DialogResult.No)
                {
                    Application.Exit();
                }
                else
                {
                    WriteINI();
                }
            }
            else
            {
                string[] reg_values = adressINI.GetValueNames();

                settingsList.Clear();

                foreach (string setting in reg_values)
                {
                    settingsList.Add(new Keys(setting, (string)adressINI.GetValue(setting)));
                }
            }

            Status = 1;

            adressINI.Close();

            return (Status == 1);
        }

        public void WriteINI()
        //записываем настройки
        {
            try
            {
                if (this.settingsList.Count != 0)
                {                
                    switch (Status)
                    {
                        case 0:
                            adressINI = Registry.CurrentUser.CreateSubKey(keyName);
                            break;
                        case 2:
                            adressINI.Close();
                            adressINI = Registry.CurrentUser.OpenSubKey(keyName, true);
                            break;
                        default:
                            break;
                    }

                    foreach (Keys k in settingsList)
                    {
                        adressINI.SetValue(k.name, (string)k.value);
                    }
                    adressINI.Close();
                    Status = 1;
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(keyName + " Ошибка! Работа программы прервана.", "Создание настроек в регистре");
                Application.Exit();
            }
        }

        private void CreateSettings()
        //создание настроек, если не существует
        {
            switch(keyName.Split('\\').Last())
            {
                case "Config":
                    AddKeys("position_x", "-1");
                    AddKeys("position_y", "-1");
                break;
                case "Paths":
                    
                break;
            }
            WriteINI();
        }
    }
}
