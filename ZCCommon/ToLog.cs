using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZentCloud.Common
{
    public class ToLog
    {
        static String fileName = System.Windows.Forms.Application.StartupPath + @"\LogFile.txt";
        static StreamWriter sw = null;
        static StreamReader sr = null;
        static char[] ch;
        private static bool isUse = true;

        private static void init()
        {
            if (File.Exists(fileName))
            {
                Console.WriteLine("we into the exist");
                sr = new StreamReader(fileName);
                int length = (int)sr.BaseStream.Length;
                ch = new char[length];
                sr.ReadBlock(ch, 0, length);
                sr.Close();
                sw = new StreamWriter(fileName);
                sw.Write(new String(ch));

                Console.Write(new String(ch));
            }
            else
            {
                Console.WriteLine("we into the not exist,an we will create the file :" + fileName);
                sw = new StreamWriter(fileName);
            }
        }


        public static void toLog(String log)
        {
            if (isUse)
            {
                init();
                isUse = false;
            }
            sw.WriteLine(DateTime.Now.ToString() + " " + log);
            sw.Flush();
        }

        public static void toLog2(String log)
        {
            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\log"))
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\log");

            fileName = System.Windows.Forms.Application.StartupPath + string.Format(@"\log\{0}.txt", DateTime.Now.ToShortDateString().Replace("\\", "-").Replace("/", "-"));
            using (StreamWriter sw2 = new StreamWriter(fileName, true,Encoding.GetEncoding("GB2312")))
            {
                sw2.WriteLine(DateTime.Now.ToString() + " " + log);
            }
        }

        public static void toLog(string msg, out string appLog, out bool isToLog)
        {
            //MySpider.ToLog.toLog2(msg);

            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\log"))
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\log");

            string fileName = System.Windows.Forms.Application.StartupPath + string.Format(@"\log\{0}.txt", DateTime.Now.ToShortDateString().Replace("\\", "-").Replace("/", "-"));
            using (StreamWriter sw2 = new StreamWriter(fileName, true, Encoding.GetEncoding("GB2312")))
            {
                sw2.WriteLine(DateTime.Now.ToString() + " " + msg);
            }

            appLog = msg;
            isToLog = true;
        }

    }
}
