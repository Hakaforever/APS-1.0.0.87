using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    public partial class MainForm
    //сюда собрано управление верхним меню
    {
        private void AddDateStrip()
        //создание пункта верхнего меню для выбора даты
        {
            DateTimePicker mainDTP = new DateTimePicker();
            mainDTP.Value = DateTime.Now.AddDays(1);
            mainDTP.Format = DateTimePickerFormat.Short;
            mainDTP.Size = new System.Drawing.Size(82, 20);
            mainDTP.CloseUp += new System.EventHandler(this.dtOneDayPlanePicker_ValueChanged);
            var dateMenuItem = new ToolStripControlHost(mainDTP);
            dateMenuItem.Tag = "all";
            int VV = menuStrip.Items.Add(dateMenuItem);//, Properties.Resources.Calendar_Multiweek);
            menuStrip.Items[VV].Image = Properties.Resources.Calendar_Multiweek;
            menuStrip.Items[VV].DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            menuStrip.Items[VV].TextImageRelation = TextImageRelation.ImageBeforeText;
            menuStrip.Items.Insert(0, menuStrip.Items[VV]);

            Startup.globalDate = Convert.ToDateTime(menuStrip.Items[0].Text).Date;
            Startup.myData.GetCurrentIssueNum();
        }
//*****раздел изданий
        private void addIssue_Click(object sender, EventArgs e)
        //добавление выпуска
        {
            mForm = new MultiTaskForms("issue_create");
            mForm.Owner = this;

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void viewIssue_Click(object sender, EventArgs e)
        //просмотр списка выпусков
        {
            vForm = new ViewDB("issue_view");
            vForm.Owner = this;

            vForm.ShowDialog();
            vForm.Dispose();
        }

        private void editIssue_Click(object sender, EventArgs e)
        //редактирование выпуска
        {
            mForm = new MultiTaskForms("issue_edit");
            mForm.Owner = this;

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void deleteIssue_Click(object sender, EventArgs e)
        //удаление выпуска
        {
            mForm = new MultiTaskForms("issue_delete");
            mForm.Owner = this;

            mForm.ShowDialog();
            mForm.Dispose();
        }

//***раздел секций
        private void addSection_Click(object sender, EventArgs e)
        //добавление секции
        {
            mForm = new MultiTaskForms("section_create");
            mForm.Owner = this;

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void viewSection_Click(object sender, EventArgs e)
        //просмотр списка секций
        {
            vForm = new ViewDB("section_view");
            vForm.Owner = this;

            vForm.ShowDialog();
        }

        private void deleteSection_Click(object sender, EventArgs e)
        //удаление секции
        {
            mForm = new MultiTaskForms("section_delete");
            mForm.Owner = this;

            mForm.ShowDialog();
            mForm.Dispose();
        }

        private void editSection_Click(object sender, EventArgs e)
        //редактирование секции
        {
            mForm = new MultiTaskForms("section_edit");
            mForm.Owner = this;

            mForm.ShowDialog();
            mForm.Dispose();
        }

    }
}
