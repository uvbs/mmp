using System;
using System.Collections.Generic;
using System.Text;

namespace CommonPlatform.Helper
{
    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class StringHandler
    {
        /// <summary>
        /// url移除指定参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parmList"></param>
        /// <returns></returns>
        public static string UrlRemoveParm(string url, List<string> parmList)
        {
            string result = url;

            if (string.IsNullOrEmpty(result))
            {
                return "";
            }

            var arr = url.Split('?');
            if (arr.Length > 1)
            {
                var arr2 = arr.GetValue(1).ToString().Split('&');

                for (int i = 0; i < arr2.Length; i++)
                {
                    for (int j = 0; j < parmList.Count; j++)
                    {
                        if (arr2.GetValue(i).ToString().IndexOf(parmList[j] + "=") == 0)
                        {
                            result = result.Replace(arr2.GetValue(i).ToString(), "");
                        }
                    }
                }

                result = result.Replace("&&", "&").Replace("?&", "?").TrimEnd(new char[] { '&', '?' });
            }

            return result;
        }

        /// <summary>
        /// 随机取字符串
        /// </summary>
        /// <param name="arrayList">字符串数组</param>
        /// <param name="split">分割符号</param>
        /// <returns></returns>
        public static string RandomStrArray(string arrayList,char split)
        {
            string[] str = arrayList.Split(split);
            Random rd = new Random();
            int num = rd.Next(str.Length);
            return str[num];
        }



    }
}
