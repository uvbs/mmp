using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZentCloud.Common
{
    public class IOHelper
    {

        //读取文本文件转换为List 
        public List<string> ReadTextFileToList(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(fs);
            //使用StreamReader类来读取文件 
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            // 从数据流中读取每一行，直到文件的最后一行 
            string tmp = sr.ReadLine();

            while (tmp != null)
            {
                list.Add(tmp);
                tmp = sr.ReadLine();
            }

            //关闭此StreamReader对象 
            sr.Close();
            fs.Close();
            return list;

        }

        //将List转换为TXT文件 
        public void WriteListToTextFile(List<string> list, string txtFile)
        {
            //创建一个文件流，用以写入或者创建一个StreamWriter 
            FileStream fs = new FileStream(txtFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();

            // 使用StreamWriter来往文件中写入内容 
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
            //关闭此文件 
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">HttpPostedFile</param>
        /// <param name="relativelyPath">文件相对路径 格式如 /FileUpload/Dir/file.jpg</param>
        /// <returns></returns>
        public bool UploadFile(System.Web.HttpPostedFile file, string relativelyPath)
        {

            try
            {

                string relativelysaveDirectory = relativelyPath.Substring(0, (relativelyPath.LastIndexOf("/") + 1));//文件保存相对目录
                string AbsoluteDirectory = System.Web.HttpContext.Current.Server.MapPath(relativelysaveDirectory);
                if (!Directory.Exists(AbsoluteDirectory))
                {
                    Directory.CreateDirectory(AbsoluteDirectory);
                }
                file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(relativelyPath));
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

        public static List<string> GetFilesPathListByDire(string direPath)
        {
            List<string> result = new List<string>();

            Directory.GetFiles(direPath, "*.mp3", SearchOption.AllDirectories).ToList();

            return result;
        }

        /// <summary>
        /// 根据路径获取字符数组(按行分隔)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFileDataByPath(string path)
        {
            //读取所有文件
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                return StrToArray(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// 根据字符串获取字符串数组(默认以行分隔)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] StrToArray(string str)
        {
            string _text = str;
            string[] temp1 = { "\r\n" };
            string[] _textArray = _text.Split(temp1, StringSplitOptions.RemoveEmptyEntries);
            if (_textArray.Length.Equals(0))
            {
                string[] temp2 = { "\n\r" };
                _textArray = _text.Split(temp2, StringSplitOptions.RemoveEmptyEntries);
            }
            if (_textArray.Length.Equals(0))
            {
                string[] temp3 = { "\n" };
                _textArray = _text.Split(temp3, StringSplitOptions.RemoveEmptyEntries);
            }
            return _textArray;
        }

        /// <summary>
        /// 流获取数组(默认以行分隔)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string[] StreamToArray(Stream stream, Encoding encoding)
        {
            return StrToArray(StreamToString(stream, encoding));
        }

        /// <summary>
        /// 获取文件字符串(全部)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件内字符串</returns>
        public static string GetFileStr(string path)
        {
            string str = string.Empty;
            try
            {

                StreamReader sr = new StreamReader(path);
                str = sr.ReadToEnd();
                sr.Close();

            }
            catch (Exception ex)
            {
                str = ex.Message;
            }

            return str;
        }
        public static string GetFileStr(string path, Encoding encoding)
        {
            string str = string.Empty;
            try
            {

                StreamReader sr = new StreamReader(path, encoding);
                str = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }

            return str;
        }

        /// <summary>
        /// 获取文件字全部符串(StringBuilder优化处理)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public StringBuilder GetFileAllStr(string path)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    sb.Append(sr.ReadToEnd());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return sb;
        }

        /// <summary>
        /// 获取文件字全部符串(StringBuilder优化处理+编码输入)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public StringBuilder GetFileAllStr(string path, string encoding)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding(encoding)))
                {
                    sb.Append(sr.ReadToEnd());
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return sb;
        }

        /// <summary>
        /// 保存数据到文件
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="path">文件名</param>
        /// <returns></returns>
        public static bool SaveGb2312DataToFile(string data, string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.Write(data);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据目录路径获取该目录下所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static FileInfo[] GetFilesByPath(string path)
        {
            DirectoryInfo dire = new DirectoryInfo(path);
            return dire.GetFiles();
        }

        /// <summary>
        /// 根据目录路径获取该目录下所有文件及子目录下的所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles2ByPath(string path)
        {
            List<FileInfo> files = new List<FileInfo>();
            DirectoryInfo dire = new DirectoryInfo(path);
            foreach (FileInfo file in dire.GetFiles())
            {
                files.Add(file);
            }

            foreach (DirectoryInfo d in dire.GetDirectories())
            {
                foreach (FileInfo file in GetFiles2ByPath(d.FullName))
                {
                    files.Add(file);
                }
            }

            return files;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="direPath">目录路径</param>
        /// <returns></returns>
        public static bool DireExists(string direPath)
        {
            return Directory.Exists(direPath);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="direPath">目录路径</param>
        /// <returns></returns>
        public static bool CreateDire(string direPath)
        {
            try
            {
                Directory.CreateDirectory(direPath);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 清除目录下所有文件
        /// </summary>
        /// <param name="direPath">目录路径</param>
        /// <returns></returns>
        public static void ClearDireFiles(string direPath)
        {
            try
            {
                FileInfo[] files = GetFilesByPath(direPath);
                foreach (FileInfo file in files)
                {
                    File.Delete(file.FullName);
                }
            }
            catch { }
        }

        /// <summary>
        /// 获取文件字节数组byte[]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetFileByte(string path)
        {
            Stream s = File.Open(path, FileMode.Open);
            int leng = 0;
            if (s.Length < Int32.MaxValue)
                leng = (int)s.Length;
            byte[] by = new byte[leng];
            s.Read(by, 0, leng);//把图片读到字节数组中
            s.Close();
            return by;
        }

        /// <summary>
        /// 检查文件名
        /// </summary>
        /// <param name="filePath">文件路径或者文件名，带格式的</param>
        /// <param name="extraNameList">文件格式，以一个分隔符分隔，随便什么都可以</param>
        /// <returns></returns>
        public static bool CheckFileName(string filePath, string extraNameList)
        {
            if (filePath != "" && filePath != null && filePath.Contains("."))
            {
                string extraName = filePath.ToLower().Substring(filePath.ToLower().LastIndexOf(".") + 1);
                if (extraNameList.Contains(extraName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetExtraName(string filePath)
        {
            return filePath.ToLower().Substring(filePath.ToLower().LastIndexOf(".") + 1);
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            string tmpStr = filePath;

            int i = tmpStr.LastIndexOf("/");
            if (i > 0)
                tmpStr = tmpStr.Substring(i + 1);

            int j = tmpStr.LastIndexOf(".");

            if (j > 0)
                tmpStr = tmpStr.Substring(0, j);

            return tmpStr;
        }

        /// <summary>
        /// 获取数据流字符串
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string StreamToString(Stream stream, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(stream, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        #region 日志记录
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
        #endregion


    }
}
