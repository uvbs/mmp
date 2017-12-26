using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZentCloud.Common;

namespace CommonPlatform.Helper
{
    public class LogHelper
    {
        //写日志
        public static void WriteLog(string logName, string logInfo, bool writeTm)
        {
            if (!ConfigHelper.GetConfigBool("logWrite")) return;
            string logPath = ConfigHelper.GetConfigString("logPath");
            if(string.IsNullOrWhiteSpace(logPath)) return;

            DeleteLog();//清理日志

            string logFileName = logPath + logName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            using (StreamWriter sr = new StreamWriter(logFileName, true))
            {
                if (writeTm) sr.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：");
                sr.WriteLine(logInfo);
            }
        }
        //清理日志
        private static void DeleteLog(){
            int logDayNum  = ConfigHelper.GetConfigInt("logDayNum");
            if(logDayNum<=0) return;
            string logPath = ConfigHelper.GetConfigString("logPath");
            if(string.IsNullOrWhiteSpace(logPath)) return;

            DirectoryInfo dir = new DirectoryInfo(logPath);
            foreach (var item in dir.GetFiles().Where(p=>p.LastWriteTime<DateTime.Now.AddDays(0-logDayNum)))
	        {
		        item.Delete();
	        }
        }
    }
}
