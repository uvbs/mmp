using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ZentCloud.Common
{
	public class MyRegex
	{
		public static string CreatePatternForHtmlTag(string startTag, string endTag)
		{
			return string.Format("(?is)<{0}[^>]*>(?><{0}[^>]*>(?<o>)|</{1}>(?<-o>)|(?:(?!</?{1}\\b).)*)*(?(o)(?!))</{1}>", startTag, endTag);
		}
		public static List<string> GetListByPattern(string input, string pattern)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(input, pattern);
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static List<string> GetPadA(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "(?is)<a href=[^>]*>(?><a href=[^>]*>(?<o>)|</a>(?<-o>)|(?:(?!</?a\\b).)*)*(?(o)(?!))</a>");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
        /// <summary>
        /// 获取body内容
        /// </summary>
        /// <param name="strdata"></param>
        /// <returns></returns>
        public static string GetPadBody(string strdata)
        {
            MatchCollection matchCollection = Regex.Matches(strdata, "(?i)\\<body\\>([\\s\\S]*)\\<\\/body\\>", RegexOptions.IgnoreCase);
            foreach (Match match in matchCollection)
            {
                return match.Value;
            }
            return "";
        }
		public static List<string> GetPadTitle(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "(?is)<title[^>]*>(?><title[^>]*>(?<o>)|</title>(?<-o>)|(?:(?!</?title\\b).)*)*(?(o)(?!))</title>");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static List<string> GetPadTD(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "(?is)<td[^>]*>(?><td[^>]*>(?<o>)|</td>(?<-o>)|(?:(?!</?td\\b).)*)*(?(o)(?!))</td>");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static List<string> GetPadTDRemoteTD(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "(?is)<td[^>]*>(?><td[^>]*>(?<o>)|</td>(?<-o>)|(?:(?!</?td\\b).)*)*(?(o)(?!))</td>");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value.Replace("<td>", "").Replace("</td>", ""));
			}
			return list;
		}
		public static List<string> GetPadTR(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "(?is)<tr[^>]*>(?><tr[^>]*>(?<o>)|</tr>(?<-o>)|(?:(?!</?tr\\b).)*)*(?(o)(?!))</tr>");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static List<string> GetPadLi(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "(?is)<li[^>]*>(?><li[^>]*>(?<o>)|</li>(?<-o>)|(?:(?!</?li\\b).)*)*(?(o)(?!))</li>");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static List<string> GetPadImg(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata.ToLower(), "(<img[\\w\\W].+?>)");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static string GetPadSrcUrl(string strdata)
		{
			string text = "";
			MatchCollection matchCollection = Regex.Matches(strdata.ToLower(), "(src=[\\w\\W].+?\")");
			foreach (Match match in matchCollection)
			{
				text = match.Value;
			}
			if (text.StartsWith("src="))
			{
				text = text.Replace("src=", "");
			}
			if (text.StartsWith("\""))
			{
				text = text.Substring(1);
			}
			if (text.EndsWith("\""))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text.Trim();
		}
		public static string GetPadSrcUrl(string strdata, string webSite)
		{
			string text = "";
			MatchCollection matchCollection = Regex.Matches(strdata.ToLower(), "(src=[\\w\\W].+?\")");
			foreach (Match match in matchCollection)
			{
				text = match.Value;
			}
			if (text.StartsWith("src="))
			{
				text = text.Replace("src=", "");
			}
			if (text.StartsWith("\""))
			{
				text = text.Substring(1);
			}
			if (text.EndsWith("\""))
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (!text.StartsWith("http"))
			{
				if (!webSite.EndsWith("/"))
				{
					webSite += "/";
				}
				if (text.StartsWith("/"))
				{
					text = text.Substring(1);
				}
				text = webSite + text;
			}
			return text.Trim();
		}
		public static string GetPadHrefUrl(string strdata)
		{
			string text = "";
			MatchCollection matchCollection = Regex.Matches(strdata, "(href=[\\w\\W].+?\")");
			foreach (Match match in matchCollection)
			{
				text = match.Value;
			}
			if (text.StartsWith("href="))
			{
				text = text.Replace("href=", "");
			}
			if (text.StartsWith("\""))
			{
				text = text.Substring(1);
			}
			if (text.EndsWith("\""))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text.Trim();
		}
		public static string GetPadHrefUrl(string strdata, string webSite)
		{
			string text = "";
			MatchCollection matchCollection = Regex.Matches(strdata, "(href=[\\w\\W].+?\")");
			foreach (Match match in matchCollection)
			{
				text = match.Value;
			}
			if (text.StartsWith("href="))
			{
				text = text.Replace("href=", "");
			}
			if (text.StartsWith("\""))
			{
				text = text.Substring(1);
			}
			if (text.EndsWith("\""))
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (!text.StartsWith("http"))
			{
				if (!webSite.EndsWith("/"))
				{
					webSite += "/";
				}
				if (text.StartsWith("/"))
				{
					text = text.Substring(1);
				}
				text = webSite + text;
			}
			return text.Trim();
		}
		public static List<string> GetPadHuaKuoHao(string strdata)
		{
			List<string> list = new List<string>();
			Regex regex = new Regex("{(.*?)}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			MatchCollection matchCollection = regex.Matches(strdata);
			foreach (Match match in matchCollection)
			{
				list.Add(match.Groups[1].Value);
			}
			return list;
		}
		public static List<string> GatherEmail(string strdata)
		{
			List<string> list = new List<string>();
			MatchCollection matchCollection = Regex.Matches(strdata, "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
			foreach (Match match in matchCollection)
			{
				list.Add(match.Value);
			}
			return list;
		}
		public static string RemoveScriptTags(string _html)
		{
			_html = Regex.Replace(_html, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
			_html.Replace("\r\n", "");
			return _html;
		}
		public static string FilterScript(string str)
		{
			string pattern = "<script[\\s\\S]+</script *>";
			return MyRegex.StripScriptAttributesFromTags(Regex.Replace(str, pattern, string.Empty, RegexOptions.IgnoreCase));
		}
		private static string StripScriptAttributesFromTags(string str)
		{
			string pattern = "(?<ScriptAttr>on\\w+=\\s*(['\"\\s]?)([/s/S]*[^\\1]*?)\\1)[\\s|>|/>]";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			foreach (Match match in regex.Matches(str))
			{
				string value = match.Groups["ScriptAttr"].Value;
				if (!string.IsNullOrEmpty(value))
				{
					str = str.Replace(value, string.Empty);
				}
			}
			str = MyRegex.FilterHrefScript(str);
			return str;
		}
		public static string FilterHrefScript(string str)
		{
			string pattern = " href[ ^=]*=\\s*(['\"\\s]?)[\\w]*script+?:([/s/S]*[^\\1]*?)\\1[\\s]*";
			return Regex.Replace(str, pattern, " ", RegexOptions.IgnoreCase);
		}
		public static string RemoveHTMLTags(string html)
		{
			html = Regex.Replace(html, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "([\\r\\n])[\\s]+", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "-->", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<!--.*", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(nbsp|#160);", "", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(cent|#162);", "¢", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(pound|#163);", "£", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(copy|#169);", "©", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&#(\\d+);", "", RegexOptions.IgnoreCase);
			html.Replace("<", "");
			html.Replace(">", "");
			html.Replace("\r\n", "");
			return html;
		}
		public static string RemoveHTMLTags(string html, string newStr)
		{
			html = Regex.Replace(html, "<script[^>]*?>.*?</script>", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<style[^>]*?>.*?</style>", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<(.[^>]*)>", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "([\\r\\n])[\\s]+", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "-->", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "<!--.*", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(nbsp|#160);", newStr, RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(cent|#162);", "¢", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(pound|#163);", "£", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&(copy|#169);", "©", RegexOptions.IgnoreCase);
			html = Regex.Replace(html, "&#(\\d+);", newStr, RegexOptions.IgnoreCase);
			html.Replace("<", newStr);
			html.Replace(">", newStr);
			html.Replace("\r\n", newStr);
			return html;
		}
		public static string GetFileNameByFullPath(string name)
		{
			if (name.Contains("\\"))
			{
				try
				{
					name = name.Substring(name.IndexOf("\\"));
					name = MyRegex.GetFileNameByFullPath(name);
				}
				catch
				{
				}
			}
			return name;
		}
		public static string IntegrationSpace(string str)
		{
			return Regex.Replace(str, "\\s+", " ");
		}
		public static bool EmailLogicJudge(string str)
		{
			string pattern = "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
			Regex regex = new Regex(pattern);
			return regex.IsMatch(str);
		}
		public static bool PhoneNumLogicJudge(string str)
		{
			bool result;
			if (str == null)
			{
				result = false;
			}
			else
			{
				if (str == "")
				{
					result = false;
				}
				else
				{
                    string pattern = "^(13[0-9]|14[5|7]|15[0-9]|17[0-9]|18[0-9])\\d{8}$";
					result = Regex.IsMatch(str, pattern);
				}
			}
			return result;
		}
		public static bool YDPhoneNumLogicJudge(string str)
		{
            string pattern = "^1(3[4-9]|4[7]|5[012789]|78|8[2378])\\d{8}$";
			return Regex.IsMatch(str, pattern);
		}
		public static bool LTPhoneNumLogicJudge(string str)
		{
            string pattern = "^1(3[0-2]|5[456]|76|8[56])\\d{8}$";
			return Regex.IsMatch(str, pattern);
		}
		public static bool DXPhoneNumLogicJudge(string str)
		{
			string pattern = "^1([35]3|77|8[019])\\d{8}$";
			return Regex.IsMatch(str, pattern);
		}
		public static bool IsNumber(string inputData)
		{
			Regex regex = new Regex("^[0-9]+$");
			Match match = regex.Match(inputData);
			return match.Success;
		}

		public static bool IsIP(string ip)
		{
			return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
		}
        /// <summary>
        /// 是否是身份证
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public static bool IsIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                return CheckIDCard18(Id);
            }
            if (Id.Length == 15)
            {
                return CheckIDCard15(Id);
            }
            return false;
            //return Id.Length == 6 || Id.Length == 8 || Id.Length == 10;
        }
        private static bool CheckIDCard18(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
            {
                return false;//数字验证
            }
            string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime dateTime = default(DateTime);
            if (!DateTime.TryParse(s, out dateTime))
            {
                return false;//生日验证
            }
            string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(new string[]
			{
				","
			}, StringSplitOptions.RemoveEmptyEntries);
            string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(new string[]
			{
				","
			}, StringSplitOptions.RemoveEmptyEntries);
            char[] array3 = Id.Remove(17).ToCharArray();
            int num2 = 0;
            for (int i = 0; i < 17; i++)
            {
                num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
            }
            int num3 = -1;
            Math.DivRem(num2, 11, out num3);
            //符合GB11643-1999标准
            return !(array[num3] != Id.Substring(17, 1).ToLower()); //校验码验证
        }
        private static bool CheckIDCard15(string Id)
        {
            long num = 0L;
            if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
            {
                return false;//数字验证
            }
            string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (text.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime dateTime = default(DateTime);
            //符合15位身份证标准
            return DateTime.TryParse(s, out dateTime); //生日验证
        }
        /// <summary>
        /// 根据url获取域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDoMainByUrl(string url)
        {
            string pattern = @"(?<=http://)[\w\.]+[^/]";　//C#正则表达式提取匹配URL的模式，       
            string doMain = "";
            MatchCollection mc = Regex.Matches(url, pattern);//满足pattern的匹配集合        
            foreach (Match match in mc)
            {
                doMain = match.ToString();
            }
            return doMain;
        }
	}
}
