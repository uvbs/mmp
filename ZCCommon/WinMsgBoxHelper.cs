using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZentCloud.Common
{
    public class WinMsgBoxHelper
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="str"></param>
        public static void ShowMessgeBox(string str)
        {
            MessageBox.Show(str, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="str"></param>
        public static void ShowSusBox(string str)
        {
            MessageBox.Show(str, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 系统提问框(OKCancel)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DialogResult ShowQuestionBox(string str)
        {
            return MessageBox.Show(str, "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
    }
}
