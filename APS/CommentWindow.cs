using System;
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
    public partial class CommentWindow : Form
    {
        internal string comment; //комментарий???
        internal string wType;
        Dictionary<string, string> CommentDict = new Dictionary<string, string>();

        public CommentWindow()
        {
            InitializeComponent();
        }

        private void Comment_Load(object sender, EventArgs e)
        {
            SplitText();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            comment = null;

            if (String.IsNullOrEmpty(txtMain.Text) || String.IsNullOrWhiteSpace(txtMain.Text))
                CommentDict.Remove(Startup.UserRole.ToUpper());
            else
                CommentDict[Startup.UserRole.ToUpper()] = txtMain.Text;

            for (int i = 0; i < CommentDict.Count; i++)
            {
                comment = comment + "\n" + CommentDict.ElementAt(i).Key.ToLower() + "\n" + CommentDict.ElementAt(i).Value + Startup.splitter;
            }
            System.Media.SystemSounds.Beep.Play();
            this.Close();
        }

        //private void SplitText()
        //{
        //    //разбиваем запись из базы по блокам-отделам где разделитель - char Startup.splitter
        //    string[] CommentBloks = comment.Split(new char[] {Startup.splitter}, StringSplitOptions.RemoveEmptyEntries);
            
        //    for(int k = 0; k < CommentBloks.Length; k++)
        //    //разбираем блоки-отделы на соответствие зашедшему пользователю и тот, что 
        //    //соответствует выделяем в окошко для редактирования, остальное - в окошко для чтения
        //    {
        //        CommentBloks[k] = (CommentBloks[k][0] != '\n' ? CommentBloks[k] : CommentBloks[k].Substring(1));
        //        int i = CommentBloks[k].IndexOf('\n');
        //        try
        //        {
        //            CommentDict.Add(CommentBloks[k].Substring(0, i).ToUpper(), CommentBloks[k].Substring(i + 1).TrimEnd('\n'));
        //            if (CommentDict.ElementAt(CommentDict.Count - 1).Key != Startup.UserRole.ToUpper())
        //            {
        //                int selStart = rtfBox.Text.Length;
        //                rtfBox.AppendText(CommentDict.ElementAt(CommentDict.Count - 1).Key.ToUpper() + "\n");
        //                rtfBox.Select(selStart, selStart + i);
        //                rtfBox.SelectionFont = new Font(rtfBox.Font.FontFamily, this.Font.Size, FontStyle.Bold);
        //                rtfBox.DeselectAll();
        //                rtfBox.AppendText(CommentDict.ElementAt(CommentDict.Count - 1).Value + "\n");
        //            }
        //            else
        //            {
        //                txtMain.Text = CommentDict.ElementAt(CommentDict.Count - 1).Value;
        //                txtMain.SelectionStart = txtMain.TextLength;
        //                txtMain.SelectionLength = 0;
        //            }
        //        }
        //        catch (Exception) { }
        //    }

        //}

        private void SplitText()
        {
            CommentAnalysis.StepOne(comment);
            txtMain.Text = CommentAnalysis.CurrentUser;
            string[] otherComment = (CommentAnalysis.OtherComment != null ? CommentAnalysis.OtherComment.Split('\n') : new string[]{});

            foreach (string s in otherComment)
            {
                if (s.All(c => char.IsUpper(c)))
                {
                    int selStart = rtfBox.Text.Length;
                    rtfBox.AppendText(s + "\n");
                    rtfBox.Select(selStart, selStart + s.Length + 1);
                    rtfBox.SelectionFont = new Font(rtfBox.Font.FontFamily, this.Font.Size, FontStyle.Bold);
                    rtfBox.DeselectAll();
                }
                else
                {
                    rtfBox.AppendText(s + "\n");
                }
            }
        }
    }
}
