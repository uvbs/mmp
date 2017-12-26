using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.Common
{
    /// <summary>
    /// 用于文本和Base64编码文本的互相转换 和 Byte[]和Base64编码文本的互相转换
    /// </summary>
    public class Base64Change
    {
        /// <summary>
        /// 将普通文本转换成Base64编码的文本
        /// </summary>
        /// <param name="value">普通文本</param>
        /// <returns></returns>
        public static string StringToBase64String(String value)
        {
            byte[] binBuffer = (new UnicodeEncoding()).GetBytes(value);
            int base64ArraySize = (int)Math.Ceiling(binBuffer.Length / 3d) * 4;
            char[] charBuffer = new char[base64ArraySize];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, charBuffer, 0);
            string s = new string(charBuffer);
            return s;
        }

        /// <summary>
        /// 将Base64编码的文本转换成普通文本
        /// </summary>
        /// <param name="base64">Base64编码的文本</param>
        /// <returns></returns>
        public static string Base64StringToString(string base64)
        {
            char[] charBuffer = base64.ToCharArray();
            byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            return (new UnicodeEncoding()).GetString(bytes);
        }

        /// <summary>
        /// 将Byte[]转换成Base64编码文本
        /// </summary>
        /// <param name="binBuffer">Byte[]</param>
        /// <returns></returns>
        public static string toBase64(byte[] binBuffer)
        {
            int base64ArraySize = (int)Math.Ceiling(binBuffer.Length / 3d) * 4;
            char[] charBuffer = new char[base64ArraySize];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, charBuffer, 0);
            string s = new string(charBuffer);
            return s;
        }

        /// <summary>
        /// 将Base64编码文本转换成Byte[]
        /// </summary>
        /// <param name="base64">Base64编码文本</param>
        /// <returns></returns>
        public static Byte[] Base64ToBytes(string base64)
        {
            char[] charBuffer = base64.ToCharArray();
            byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            return bytes;
        }

        public static string GB2312ToUTF8(string str)
        {
            try
            {
                Encoding uft8 = Encoding.GetEncoding(65001);
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] temp = gb2312.GetBytes(str);
                byte[] temp1 = Encoding.Convert(gb2312, uft8, temp);
                string result = uft8.GetString(temp1);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string UTF8ToGB2312(string str)
        {
            try
            {
                Encoding utf8 = Encoding.GetEncoding(65001);
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] temp = utf8.GetBytes(str);
                byte[] temp1 = Encoding.Convert(utf8, gb2312, temp);
                string result = gb2312.GetString(temp1);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 生成Base64编码(默认格式)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeBase64(string source)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(source);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解析Base64编码(默认格式)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string DecodeBase64(string result)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(result);
                return System.Text.Encoding.Default.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 生成Base64编码(GB2312)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeBase64ByGB2312(string source)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("gb2312").GetBytes(source);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解析Base64编码(GB2312)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string DecodeBase64ByGB2312(string result)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(result);
                return System.Text.Encoding.GetEncoding("gb2312").GetString(bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 生成Base64编码(utf8)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncodeBase64ByUTF8(string source)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(source);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解析Base64编码(utf8)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string DecodeBase64ByUTF8(string result)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(result);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }
    }
}
