using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZentCloud.Common
{
    /// <summary>
    /// 格式验证助手
    /// </summary>
    public class ValidatorHelper
    {
        /// <summary>
        /// 创建HTML标签内容正则表达式
        /// </summary>
        /// <param name="startTag">开始标签</param>
        /// <param name="endTag">结尾标签</param>
        /// <returns>构造的正则表达式</returns>
        public static string CreatePatternForHtmlTag(string startTag, string endTag)
        {
            return string.Format(@"(?is)<{0}[^>]*>(?><{0}[^>]*>(?<o>)|</{1}>(?<-o>)|(?:(?!</?{1}\b).)*)*(?(o)(?!))</{1}>", startTag, endTag);
        }

        /// <summary>
        /// 根据表达式匹配文本字符串集合
        /// </summary>
        /// <param name="input">输入文本</param>
        /// <param name="pattern">字符串匹配表达式</param>
        /// <returns>匹配的文本字符串集合</returns>
        public static List<string> GetListByPattern(string input, string pattern)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(input, pattern);
            foreach (Match m in mc)//遍历MatchCollection,取出每一个匹配字符串然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取a标签数据集合
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadA(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, @"(?is)<a href=[^>]*>(?><a href=[^>]*>(?<o>)|</a>(?<-o>)|(?:(?!</?a\b).)*)*(?(o)(?!))</a>");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取title标签数据集合
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadTitle(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, @"(?is)<title[^>]*>(?><title[^>]*>(?<o>)|</title>(?<-o>)|(?:(?!</?title\b).)*)*(?(o)(?!))</title>");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取TD标签数据集合
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadTD(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, @"(?is)<td[^>]*>(?><td[^>]*>(?<o>)|</td>(?<-o>)|(?:(?!</?td\b).)*)*(?(o)(?!))</td>");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取TD标签数据集合(去除td标签)
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadTDRemoteTD(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, @"(?is)<td[^>]*>(?><td[^>]*>(?<o>)|</td>(?<-o>)|(?:(?!</?td\b).)*)*(?(o)(?!))</td>");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add((m.Value.Replace("<td>", "")).Replace("</td>", ""));
            }
            return list;
        }

        /// <summary>
        /// 获取tr标签数据集合
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadTR(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, @"(?is)<tr[^>]*>(?><tr[^>]*>(?<o>)|</tr>(?<-o>)|(?:(?!</?tr\b).)*)*(?(o)(?!))</tr>");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取li标签数据集合(小写字母)
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadLi(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, @"(?is)<li[^>]*>(?><li[^>]*>(?<o>)|</li>(?<-o>)|(?:(?!</?li\b).)*)*(?(o)(?!))</li>");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取img标签数据集合(小写字母)
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadImg(string strdata)
        {
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata.ToLower(), @"(<img[\w\W].+?>)");
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<img>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取标签内src链接地址(小写字母)
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static string GetPadSrcUrl(string strdata)
        {
            string result = "";
            MatchCollection mc =
                Regex.Matches(strdata.ToLower(), @"(src=[\w\W].+?"")");

            foreach (Match m in mc)//遍历MatchCollection,取出每一个<img>然后存入数组
            {
                result = m.Value;
            }

            if (result.StartsWith("src="))
                result = result.Replace("src=", "");

            if (result.StartsWith("\""))
                result = result.Substring(1);

            if (result.EndsWith("\""))
                result = result.Substring(0, result.Length - 1);

            return result.Trim();
        }

        /// <summary>
        /// 获取标签内src链接地址(小写字母)
        /// </summary>
        /// <param name="strdata"></param>
        /// <param name="website">网站地址</param>
        /// <returns></returns>
        public static string GetPadSrcUrl(string strdata, string webSite)
        {
            string result = "";
            MatchCollection mc =
                Regex.Matches(strdata.ToLower(), @"(src=[\w\W].+?"")");

            foreach (Match m in mc)//遍历MatchCollection,取出每一个<img>然后存入数组
            {
                result = m.Value;
            }

            if (result.StartsWith("src="))
                result = result.Replace("src=", "");

            if (result.StartsWith("\""))
                result = result.Substring(1);

            if (result.EndsWith("\""))
                result = result.Substring(0, result.Length - 1);

            if (!result.StartsWith("http"))
            {
                if (!webSite.EndsWith("/"))
                    webSite += "/";
                if (result.StartsWith("/"))
                    result = result.Substring(1);

                result = webSite + result;
            }

            return result.Trim();
        }

        /// <summary>
        /// 获取标签内href链接地址
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static string GetPadHrefUrl(string strdata)
        {
            string result = "";
            MatchCollection mc =
                Regex.Matches(strdata, @"(href=[\w\W].+?"")");

            foreach (Match m in mc)
            {
                result = m.Value;
            }

            if (result.StartsWith("href="))
                result = result.Replace("href=", "");

            if (result.StartsWith("\""))
                result = result.Substring(1);

            if (result.EndsWith("\""))
                result = result.Substring(0, result.Length - 1);

            return result.Trim();
        }

        /// <summary>
        /// 获取标签内href链接地址
        /// </summary>
        /// <param name="strdata"></param>
        /// <param name="website">网站地址</param>
        /// <returns></returns>
        public static string GetPadHrefUrl(string strdata, string webSite)
        {
            string result = "";
            MatchCollection mc =
                Regex.Matches(strdata, @"(href=[\w\W].+?"")");

            foreach (Match m in mc)
            {
                result = m.Value;
            }

            if (result.StartsWith("href="))
                result = result.Replace("href=", "");

            if (result.StartsWith("\""))
                result = result.Substring(1);

            if (result.EndsWith("\""))
                result = result.Substring(0, result.Length - 1);

            if (!result.StartsWith("http"))
            {
                if (!webSite.EndsWith("/"))
                    webSite += "/";
                if (result.StartsWith("/"))
                    result = result.Substring(1);

                result = webSite + result;
            }

            return result.Trim();
        }


        /// <summary>
        /// 获取花括号内容数据集合
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GetPadHuaKuoHao(string strdata)
        {
            List<string> list = new List<string>();

            Regex reg1 = new Regex("{(.*?)}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection matches = reg1.Matches(strdata);
            foreach (Match match in matches)
            {
                list.Add(match.Groups[1].Value);
            }

            return list;
        }

        /// <summary>
        /// 采集邮箱地址
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static List<string> GatherEmail(string strdata)
        {
            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";//@"(?<email>[^@\s:]+@\S+)";//@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";//@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
            List<string> list = new List<string>();
            MatchCollection mc =
                Regex.Matches(strdata, pattern);
            foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
            {
                list.Add(m.Value);
            }
            return list;
        }


        ///// <summary>
        ///// 根据标签参数数据集合
        ///// </summary>
        ///// <param name="strdata"></param>
        ///// <returns></returns>
        //public static List<string> GetPadByParm(string first,string last, string strdata)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("(?is)");
        //    sb.Append(parm);
        //    sb.Append(" href=[^>]*>(?><");
        //    sb.Append(parm);
        //    sb.Append(" href=[^>]*>(?<o>)|</");
        //    sb.Append(parm);


        //    List<string> list = new List<string>();
        //    MatchCollection mc =
        //        Regex.Matches(strdata, "(?is)<td href=[^>]*>(?><td href=[^>]*>(?<o>)|</td>(?<-o>)|(?:(?!</?td\b).)*)*(?(o)(?!))</td>");
        //    foreach (Match m in mc)//遍历MatchCollection,取出每一个<a>然后存入数组
        //    {
        //        list.Add(m.Value);
        //    }
        //    return list;
        //}


        /// <summary>
        /// 去掉脚本标记
        /// </summary>
        /// <param name="_html">HTML</param>
        /// <returns></returns>
        public static string RemoveScriptTags(string _html)
        {
            //删除脚本
            _html = Regex.Replace(_html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            _html.Replace("\r\n", "");
            return _html;
        }

        /// <summary>
        /// 滤除script引用和区块
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterScript(string str)
        {
            string pattern = @"<script[\s\S]+</script *>";
            return StripScriptAttributesFromTags(Regex.Replace(str, pattern, string.Empty, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// 去除标签中的script属性
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string StripScriptAttributesFromTags(string str)
        {
            //\s 空白字符，包括换行符\n、回车符\r、制表符\t、垂直制表符\v、换页符\f
            //\S \s的补集
            //\w 单词字符，指大小写字母、0-9的数字、下划线
            //\W \w的补集

            //方法一：整体去除，不能去除不被单引号或双引号包含的属性值
            //string pattern = @"on\w+=\s*(['""\s]?)([/s/S]*[^\1]*?)\1[\s]*";
            //content = Regex.Replace(str, pattern, string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            ////方法二：去除属性值
            //string pattern = @"<\w+\s+(?<Attrs>[^>]*?)[>|/>]";
            //Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //foreach (Match m in r.Matches(content))
            //{
            //    //获取标签的全部属性
            //    string attrs = m.Groups["Attrs"].Value;

            //    if (!string.IsNullOrEmpty(attrs))
            //    {
            //        //获取每一个属性
            //        Regex rt = new Regex(@"(?<AttrName>\w+)\s*=(?<AttrPre>[\s]*(['""\s]?))(?<AttrVal>[^\1]*?)\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //        foreach (Match mt in rt.Matches(attrs))
            //        {
            //            string attrName = mt.Groups["AttrName"].Value.Trim().ToLower();
            //            string attrVal = mt.Groups["AttrVal"].Value.Trim().ToLower();

            //            //匹配以on开头的属性
            //            if (attrName.StartsWith("on") && !string.IsNullOrEmpty(attrVal))
            //            {
            //                //将属性值替换为空
            //                str = str.Replace(attrVal, string.Empty);
            //            }
            //        }
            //    }
            //}

            //整体去除
            string pattern = @"(?<ScriptAttr>on\w+=\s*(['""\s]?)([/s/S]*[^\1]*?)\1)[\s|>|/>]";
            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match m in r.Matches(str))
            {
                string attrs = m.Groups["ScriptAttr"].Value;
                if (!string.IsNullOrEmpty(attrs))
                {
                    str = str.Replace(attrs, string.Empty);
                }
            }

            //滤除包含script的href
            str = FilterHrefScript(str);

            return str;
        }

        /// <summary>
        /// 滤除包含script的href
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterHrefScript(string str)
        {
            //整体去除，不能去除不被单引号或双引号包含的属性值
            string regexstr = @" href[ ^=]*=\s*(['""\s]?)[\w]*script+?:([/s/S]*[^\1]*?)\1[\s]*";
            return Regex.Replace(str, regexstr, " ", RegexOptions.IgnoreCase);
        }



        /// <summary>
        /// 去掉HTML标记
        /// </summary>
        /// <param name="_html">HTML</param>
        /// <returns></returns>
        public static string RemoveHTMLTags(string _html)
        {
            //删除脚本
            _html = Regex.Replace(_html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除样式
            _html = Regex.Replace(_html, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
            //删除HTML
            _html = Regex.Replace(_html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"-->", "", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"<!--.*", "", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            _html.Replace("<", "");
            _html.Replace(">", "");
            _html.Replace("\r\n", "");
            return _html;
        }

        /// <summary>
        /// 替换HTML标记为指定字符
        /// </summary>
        /// <param name="_html">HTML</param>
        /// <param name="newStr">Html替换的字符</param>
        /// <returns></returns>
        public static string RemoveHTMLTags(string _html, string newStr)
        {
            //删除脚本
            _html = Regex.Replace(_html, @"<script[^>]*?>.*?</script>", newStr, RegexOptions.IgnoreCase);
            //删除样式
            _html = Regex.Replace(_html, @"<style[^>]*?>.*?</style>", newStr, RegexOptions.IgnoreCase);
            //删除HTML
            _html = Regex.Replace(_html, @"<(.[^>]*)>", newStr, RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"([\r\n])[\s]+", newStr, RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"-->", newStr, RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"<!--.*", newStr, RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(nbsp|#160);", newStr, RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            _html = Regex.Replace(_html, @"&#(\d+);", newStr, RegexOptions.IgnoreCase);
            _html.Replace("<", newStr);
            _html.Replace(">", newStr);
            _html.Replace("\r\n", newStr);
            return _html;
        }

        /// <summary>
        /// 根据完整路径获取文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFileNameByFullPath(string name)
        {
            if (name.Contains(@"\"))
            {
                try
                {
                    name = name.Substring(name.IndexOf(@"\"));
                    name = GetFileNameByFullPath(name);
                }
                catch { }
            }
            return name;
        }

        /// <summary>
        /// 多个空格匹配成一个
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IntegrationSpace(string str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }

        /// <summary>
        /// 邮箱地址逻辑判断
        /// </summary>
        /// <param name="str">判断字符</param>
        /// <returns></returns>
        public static bool EmailLogicJudge(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            //string format = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$

            string format = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex myRegex = new Regex(format);
            if (myRegex.IsMatch(str))
                return true;
            return false;
        }

        /// <summary>
        /// 手机号码逻辑判断
        /// </summary>
        /// <param name="str">判断字符</param>
        /// <returns></returns>
        public static bool PhoneNumLogicJudge(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            /*
             * 
                电信：133 153 180 189

                移动：134 135 136 137 138 139 150 151 152 157 158 159 182 183 187 188

                联通：130 131 132 155 156 185 186 (154) 

                数据卡:145 147
             * 
             */


            string s = @"^(13[0-9]|14[5|7]|15[0-9]|17[0-9]|18[0-9])\d{8}$";
            //string s = @"^1(3|5|8)\d{5,9}$";更开放匹配
            if (Regex.IsMatch(str, s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 移动手机号码逻辑判断134.135.136.137.138.139.150.151.152.157.158.159.187.188 .147.182 183
        /// </summary>
        /// <param name="str">判断字符</param>
        /// <returns></returns>
        public static bool YDPhoneNumLogicJudge(string str)
        {
            string s = @"^1(3[4-9]|4[7]|5[012789]|8[2378])\d{8}$";
            if (Regex.IsMatch(str, s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 联通手机号码逻辑判断130.131.132.155.156.185.186 .154
        /// </summary>
        /// <param name="str">判断字符</param>
        /// <returns></returns>
        public static bool LTPhoneNumLogicJudge(string str)
        {
            string s = @"^1(3[0-2]|5[456]|8[56])\d{8}$";
            if (Regex.IsMatch(str, s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 电信手机号码逻辑判断133.153.180.189 
        /// </summary>
        /// <param name="str">判断字符</param>
        /// <returns></returns>
        public static bool DXPhoneNumLogicJudge(string str)
        {
            string s = @"^1([35]3|8[019])\d{8}$";
            if (Regex.IsMatch(str, s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Regex RegNumber = new Regex("^[0-9]+$");
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
