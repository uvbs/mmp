using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace ZentCloud.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ini
    {
        public static string inipath = Application.StartupPath + @"\app.ini";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="INIPath">文件路径</param>
        public ini(string filePath)
        {
            inipath = filePath;
        }


        /// <summary>
        /// 写入INI文件(静态方法)
        /// </summary>
        /// <param name="section">项目名称(如 [TypeName] )</param>
        /// <param name="ley">键</param>
        /// <param name="value">值</param>
        public static void IniWriteValue(string section, string ley, string value)
        {
            WritePrivateProfileString(section, ley, value, inipath);
        }

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">项目名称(如 [TypeName] )</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void IniWriteValue1(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, inipath);
        }

        /*参数说明：section：INI文件中的段落；key：INI文件中的关键字；val：
         * INI文件中关键字的数值；filePath：INI文件的完整的路径和名称。*/


        /// <summary>
        /// 读出INI文件(静态方法)
        /// </summary>
        /// <param name="section">项目名称(如 [TypeName] )</param>
        /// <param name="key">键</param>
        public static string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(section, key, "", temp, 500, inipath);
            return temp.ToString();
        }
        /// <summary>
        /// 读出INI文件
        /// </summary>
        /// <param name="section">项目名称(如 [TypeName] )</param>
        /// <param name="key">键</param>
        public string IniReadValue1(string section, string key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(section, key, "", temp, 500, inipath);
            return temp.ToString();
        }
        /*参数说明：section：INI文件中的段落名称；key：INI文件中的关键字；
         * def：无法读取时候时候的缺省数值；retVal：读取数值；
         * size：数值的大小；filePath：INI文件的完整路径和名称。*/


        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        /// <returns>布尔值</returns>
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
    }
}
