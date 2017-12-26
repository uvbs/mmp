using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Security.Cryptography;

namespace ZentCloud.Common
{
    public class StringHelper
    {
        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReverseStr(string str)
        {
            if (str.Trim() == "")
                return "";
            
            char[] arr = str.Trim().ToCharArray();
            Array.Reverse(arr);
            StringBuilder sb = new StringBuilder();
            foreach (char item in arr)
            {
                sb.Append(item.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取时间数值(包含分秒)
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeNum()
        {
            return DateTime.Now.ToString().Replace(" ", "").Replace("-", "").Replace(":", "").Replace(".", "").Replace("\\", "").Replace("/", "");
        }

        /// <summary>
        /// 字符串转换成日期(默认sql数据最小类型)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string str)
        {
            DateTime result = DateTime.Parse(SqlDateTime.MinValue.ToString());

            str = str.Trim();

            if (!str.Equals(""))
                DateTime.TryParse(str, out result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Decimal StringToDecimal(string str)
        {
            Decimal result = 0;
            str = str.Trim();

            if (!str.Equals(""))
                Decimal.TryParse(str, out result);

            return result;
        }

        /// <summary>  
        /// 字符串md5加密  
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        public static string StrToMD5(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str.Trim());

            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] output = md5.ComputeHash(result);

            return BitConverter.ToString(output).Replace("-", "");
        }
        /// <summary>
        /// 字符串SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string StrToSHA1(string str, Encoding encoding)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = encoding.GetBytes(str);
            byte[] str2 = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return Convert.ToBase64String(str2);
        }

        /// <summary>
        /// 泛型数据转换成分隔符字符串
        /// </summary>
        /// <param name="list">数组</param>
        /// <param name="includeBy">包含数据的字符</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string ListToStr<T>(List<T> list, string includeBy, string splitChar)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T item in list)
            {
                sb.AppendFormat("{1}{0}{1},", item, includeBy);
            }

            return sb.ToString().Trim(char.Parse(splitChar));
        }

        /// <summary>  
        /// 获取字符串长度，一个汉字算两个字节  
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0; byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        /// <summary>
        /// 根据开始标签截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string CutByStarTag(string str, string tag, bool isRemoteTag = false)
        {

            int i;
            //截取目标数据段
            i = str.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
            if (i > 0)
            {
                if (isRemoteTag)
                    str = str.Substring(i + tag.Length);
                else
                    str = str.Substring(i);
            }
            return str;
        }

        /// <summary>
        /// 根据结束标签截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string CutByEndTag(string str, string tag)
        {

            int i;
            //截取目标数据段
            i = str.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
            if (i > 0)
                str = str.Substring(0, i);
            return str;
        }

        /// <summary>
        /// 根据最大长度截取字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string CutByMaxLength(string input, int maxLength)
        {
            string result = input;

            if (string.IsNullOrWhiteSpace(input))
                return input;

            //if (GetLength(input) > maxLength)
            //    result = input.Substring(0, maxLength);

            if (input.Length > maxLength)
                result = input.Substring(0, maxLength);

            return result;
        }

        /// <summary>
        /// 移除指定字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="removeStr"></param>
        /// <returns></returns>
        public static string RemoveOnString(string input, string removeStr)
        {
            if (!string.IsNullOrEmpty(input))
                input = input.Replace(removeStr, "");
            return input;
        }
        /// <summary>
        /// 移除空格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveSpace(string input)
        {
            input = RemoveOnString(input, " ");
            return input;
        }

        /// <summary>
        ///  检查是否含有中文
        /// </summary>
        /// <param name="InputText">需要检查的字符串</param>
        /// <returns></returns>
        public static bool IsHasChzn_C(string str)
        {
            byte[] strASCII = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
            int tmpNum = 0;
            for (int i = 0; i < str.Length; i++)
            {
                //中文检查
                if ((int)strASCII[i] >= 63 && (int)strASCII[i] < 91)
                {

                    tmpNum += 2;
                }
            }
            if (tmpNum > 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 过滤掉中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FilterChzn_C(string input)
        {
            //if (input.Equals("請联系service@riversunhk.com"))
            //{

            //}

            string result = "";
            foreach (var item in input.ToCharArray())
            {
                if (!IsHasChzn_C(item.ToString()))
                {
                    result += item;
                }
                else
                {

                }
            }
            return result;
        }
        /// <summary>
        ///  自动替换单引号、换行符号、结尾的%20
        /// </summary>
        /// <returns></returns>
        public static string GetReplaceStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str= str.Replace("\r\n", "").Replace("\n", "").Replace("'", "").Replace("\"", "");
            if (str.EndsWith("%20"))
            {
                str = str.Substring(0, str.Length - 3);
            }
            return str;
        }
        /// <summary>
        /// 移除链接后面的#RD
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string ReplaceLinkRD(string inputStr)
        {
            
            if (string.IsNullOrWhiteSpace(inputStr)) return "";
            //return inputStr.Replace("#rd", "");
            if (inputStr.Length <= 3) return inputStr;
            if (!inputStr.ToLower().Substring(inputStr.Length - 3).Equals("#rd")) return inputStr;
            return inputStr.Substring(0, inputStr.Length - 3);


        }
        /// <summary>
        /// 是否包含字符串
        /// </summary>
        /// <param name="orgin"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ContainsStr(string orgin,string str)
        {

            if (string.IsNullOrWhiteSpace(orgin) || string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            if (orgin.ToLower().IndexOf(str.ToLower()) > -1)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 字符串分隔 增加单引号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AddSplitChar(string str) {

            return  "'" + str.Replace(",", "','") + "'";
        
        }


    }
}
