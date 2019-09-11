using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APS
{
    public partial class MainForm
    {
        internal void FillCirculationPanel(TableLayoutPanel mPanel)
        {
            mPanel.ColumnCount = Startup.issuesList.Count + 1;
            string[] Days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            DateTime startDayOfWeek = Startup.globalDate.AddDays(-(int)Startup.globalDate.DayOfWeek + 1);

            for (int i = 0; i < Startup.issuesList.Count; i++)
            {
                ColumnStyle rs = new ColumnStyle();
                mPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            }

            for (int j = 1; j < mPanel.RowStyles.Count; j++)
            { 
                CheckBox l = new CheckBox();
                l.Text = startDayOfWeek.Date.AddDays(j-1).ToShortDateString();
                int h = (int)(mPanel.Height / mPanel.RowCount - 15) / 2;
                l.Margin = new System.Windows.Forms.Padding(15, h, 0, 0);
                l.Tag = "issueDate";
                mPanel.Controls.Add(l, 0, j);
            }

        }

        internal void ClearCirculationPanel(TableLayoutPanel mPanel)
        {
            for (int i = mPanel.ColumnCount - 1; i > 0; i--)
            {
                for (int j = 0; j < mPanel.RowCount; j++)
                {
                    var ctrl = mPanel.GetControlFromPosition(i, j);
                    mPanel.Controls.Remove(ctrl);
                }
                mPanel.ColumnStyles.RemoveAt(i);
                mPanel.ColumnCount--;
            }
        }
    }
}
