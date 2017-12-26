using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.Common
{
    /// <summary>
    /// 字典集合
    /// </summary>
    public class DictionaryClump
    {
        /// <summary>
        /// 邮件发送状态:0.等待处理；1.正在处理；2.等待发送；3.正在发送；4.完成发送；
        /// </summary>
        public static Dictionary<int, string> EmailSendStatusDictionary = new Dictionary<int, string>()
        {
            {0,"等待处理"},
            {1,"正在处理"},
            {2,"等待发送"},
            {3,"正在发送"},
            {4,"完成发送"},
            {-1,"发送失败"},
            {-2,"已取消"}
        };
    }
}
