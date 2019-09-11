using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace APS
{
    class ExportXML
    {
        string[] header;
        string[] page;
        MainForm mOwner;
        string EditionCode;
        string EditionName;
        string PlaneDate;
        int height = 990;
        int width = 708;
        int colour = 4;
        int dps = 0;
        List<string> Templates;
        List<string> Sections = new List<string>();

        public ExportXML(MainForm owner)
        {
            header = new string[] { "publication_plan", "units", "publication", "edition", "pubdate" };
            page = new string[] { "physical_page_number", "logical_page_number", "base_editions", "height", "width", "colour", "unique_id", "modifier", "section", "dps" };
            mOwner = owner;

            EditionCode = Startup.myData.GetEditionCodebyID((int)mOwner.cmbEdition.SelectedValue);
            EditionName = mOwner.cmbEdition.SelectedItem.ToString();
            PlaneDate = Startup.globalDate.ToString("ddMMyy");
            Templates = new List<string>();
            List<string> Issues = Startup.myData.GetIssuesList((int)mOwner.cmbEdition.SelectedValue, true);

            foreach (ListViewItem li in Startup.myData.GetActiveSections((int)mOwner.cmbEdition.SelectedValue, true).OrderBy(p => p.ImageKey))
            {
                if (li.ImageIndex == -1)
                    Sections.Add(li.ImageKey);
            }
            int t = 0;
        }

        private void SectionsSave()
        { 

        }
    }
}
