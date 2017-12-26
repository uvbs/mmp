using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;

namespace ZentCloud.Common
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SysLog
    {
        /// <summary>
        /// 系统日志所在的主机名。当前设定：本地电脑
        /// </summary>
        public const string MACHINE_NAME = ".";
        /// <summary>
        /// 系统日志名。当前设定：应用程序
        /// </summary>
        public const string LOG_NAME = "Application";
        /// <summary>
        /// 消息事件类型。当前设定：无;该值为0的时候分类为“无”。
        /// </summary>
        public const short CATEGORY = 0;
        /// <summary>
        /// 消息事件的种类：信息(Information)，警告(Warning)，重大错误(Error)
        /// </summary>       
        public enum LogType
        {
            Error = 1,
            Warning = 2,
            Information = 4
        }
        static SysLog()
        { }

        /// <summary>
        /// 向日志管理器写入系统日志
        /// </summary>
        /// <param name="source">消息事件来源</param>
        /// <param name="message">要在消息事件中写入的信息</param>
        /// <param name="type">消息事件类型</param>       
        /// <param name="eventID">消息事件的事件ID。（0～65535）</param>
        /// <exception cref="System.ComponentModel.Win32Exception">系统日志存储空间不足时</exception>
        /// <exception cref="System.Security.SecurityException">操作系统日志权限不足时</exception>
        static public void WriteEntry(string source, string message, LogType type, int eventID)
        {
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, LOG_NAME);
                }
                EventLog.WriteEntry(source, message, GetLogEntryType(type), eventID, CATEGORY);
            }
            catch (SecurityException)
            {
                //碰到权限不够无法操作系统日志的情况时，模拟本地管理员权限进行操作。
                //提示：当注册表中没有消息事件来源时，会发生该例外
                //ImpersonateAccount类的代码参考鄙人另一篇文章C#模拟AD用户 
                //不要指望可以使用System.Diagnostics.EventLogInstaller类，使用它也需要本地系统管理员的权限
                //using (ImpersonateAccount sa = new ImpersonateAccount("<用户名>", "<域>", "<密码>"))
                //{
                //    if (!EventLog.SourceExists(source))
                //    {
                //        EventLog.CreateEventSource(source, LOG_NAME);
                //    }
                //    EventLog.WriteEntry(source, message, GetLogEntryType(type), eventID, CATEGORY);
                //}
            }
        }
        /// <summary>
        /// 向日志管理器写入系统日志
        /// </summary>
        /// <param name="source">消息事件来源</param>       
        /// <param name="ex">要在消息事件中写入的信息</param>
        /// <param name="eventID">消息事件的事件ID。（0～65535）</param>       
        static public void WriteEntry(string source, Exception ex, int eventID)
        {
            StringBuilder sb = new StringBuilder();
            if (ex != null)
            {
                sb.AppendFormat("[Message]{0}\r\n", ex.Message);
                sb.AppendFormat("[Source]{0}\r\n", ex.Source);
                sb.AppendFormat("[TargetSite]{0}\r\n", ex.TargetSite);
                sb.AppendFormat("[ToString]{0}\r\n", ex.ToString());
                if (ex.Data.Count > 0)
                {
                    sb.Append("[Data]");
                    foreach (System.Collections.DictionaryEntry var in ex.Data)
                    { sb.AppendFormat("\t[{0}]:{1}\n", var.Key, var.Value); }
                }
            }
            WriteEntry(source, sb.ToString(), LogType.Error, eventID);
        }
        /// <summary>
        /// 将LogType类型转换成EventLogEntryType类型
        /// </summary>
        /// <param name="type">LogType</param>
        /// <returns>EventLogEntryType</returns>
        static private EventLogEntryType GetLogEntryType(LogType type)
        {
            Type t = typeof(EventLogEntryType);
            if (Enum.IsDefined(t, (int)type))
            { return (EventLogEntryType)Enum.Parse(t, ((int)type).ToString()); }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("LogType类型无法转换为EventLogEntryType类型");
                sb.AppendFormat("(LogType:{0})", type);
                throw new ApplicationException(sb.ToString());
            }
        }
    }
}
