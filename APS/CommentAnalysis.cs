using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS
{
    static class CommentAnalysis
    {
        internal static string CurrentUser = null;
        internal static string OtherComment = null;
        //internal System.Windows.Forms.Rich

        internal static void StepOne(string comment)
        {
            string sysText = null;
            //разбиваем входящий текст по блокам-отделам где разделитель - char Startup.splitter
            string[] CommentBloks = comment.Split(new char[] { Startup.splitter }, StringSplitOptions.RemoveEmptyEntries);

            //формируем Dictionary из разобранного массива с отсечением первых и последних '\n' (для унификации)
            Dictionary<string, string> Bloks = new Dictionary<string, string>();

            for (int i = 0; i < CommentBloks.Length; i++)
            {
                CommentBloks[i] = CommentBloks[i].TrimStart('\n').TrimEnd('\n');

                int u = CommentBloks[i].IndexOf('\n');
                if (u > 1)
                    Bloks.Add(CommentBloks[i].Substring(0, u).ToUpper(), CommentBloks[i].Substring(u + 1));
            }
            Bloks.TryGetValue("system".ToUpper(), out sysText);
            Bloks.Remove("system".ToUpper());
            Bloks.TryGetValue(Startup.UserRole.ToUpper(), out CurrentUser);
            Bloks.Remove(Startup.UserRole.ToUpper());

            if (!String.IsNullOrEmpty(sysText) && !String.IsNullOrWhiteSpace(sysText))
                OtherComment = "SYSTEM\n" + sysText;
            foreach (KeyValuePair<string, string> s in Bloks)
            {
                OtherComment = OtherComment + "\n" + s.Key.ToUpper() + "\n" + s.Value;
            }
        }

        internal static void Clear()
        {
            CurrentUser = null;
            OtherComment = null;
        }
    }
}
