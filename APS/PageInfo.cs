using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS
{
    class PageInfo
    {
        int pageNum; //номер страницы в плане
        int editionId; //ID издания
        int issueId; //ID выпуска
        int sectionId; //ID секции
        int templateId; //ID плана
        string templateName; //название шаблона
        string sectionCode; //код секции
        string sectionName; //название секции
        string issueCode; //код выпуска
        string issueName; //название выпуска
        string editionCode; //код издания
        string editionName; //название издания
        string comment; //комментарий

        System.Windows.Forms.ListViewItem listItem; //представление для списка в ListView

        public PageInfo(int pageNum = -1, int sectionId = -1, int editionId = -1, int issueId = -1, int templateId = -1)
        {
            listItem = new System.Windows.Forms.ListViewItem();

            PageNum = pageNum;
            SectionId = sectionId;
            EditionId = editionId;
            IssueId = issueId;
            TemplateId = templateId;
        }

        public int PageNum
        {
            get
            {
                return pageNum;
            }

            set
            {
                pageNum = value;
                if (listItem != null)
                    listItem.Text = Startup.CreateName(pageNum);
            }
        }

        public int SectionId
        {
            get
            {
                return sectionId;
            }

            set
            {
                sectionId = value;
                if (sectionId > 0)
                {
                    sectionName = Startup.myData.GetSectionNameByID(sectionId);
                    sectionCode = Startup.myData.GetSectionCodeByID(sectionId);
                    if (listItem != null)
                        listItem.ToolTipText = sectionName;
                }
            }
        }

        public string SectionCode
        {
            get
            {
                try
                {
                    return sectionCode;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string SectionName
        {
            get
            {
                try
                {
                    return sectionName;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int EditionId
        {
            get
            {
                return editionId;
            }
            set
            {
                editionId = value;
                if (editionId > 0)
                {
                    editionName = Startup.myData.GetEditionNamebyID(editionId);
                    editionCode = Startup.myData.GetEditionCodebyID(editionId);
                }
            }
        }

        public string EditionCode
        {
            get
            {
                try
                {
                    return editionCode;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string EditionName
        {
            get
            {
                try
                {
                    return editionName;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int IssueId
        {
            get
            {
                return issueId;
            }
            set
            {
                issueId = value;
                if (issueId > 0)
                {
                    issueName = Startup.myData.GetIssuesNamebyID(issueId);
                    issueCode = Startup.myData.GetIssueCodebyID(issueId);
                }
            }
        }

        public string IssueCode
        {
            get
            {
                try
                {
                    return issueCode;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string IssueName
        {
            get
            {
                try
                {
                    return issueName;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int TemplateId
        {
            get
            {
                return templateId;
            }
            set
            {
                templateId = value;
                if (templateId > 0)
                {
                    templateName = Startup.myData.GetTemplateNamebyID(templateId);
                }
            }
        }

        public string TemplateName
        {
            get
            {
                try
                {
                    return templateName;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public System.Windows.Forms.ListViewItem ListItem
        {
            get
            {
                try
                {
                    return listItem;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
