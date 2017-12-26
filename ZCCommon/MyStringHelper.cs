using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;
namespace ZentCloud.Common
{
	public class MyStringHelper
	{
		public static string ReverseStr(string str)
		{
			string result;
			if (str.Trim() == "")
			{
				result = "";
			}
			else
			{
				char[] array = str.Trim().ToCharArray();
				Array.Reverse(array);
				StringBuilder stringBuilder = new StringBuilder();
				char[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					char c = array2[i];
					stringBuilder.Append(c.ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
		public static string GetDateTimeNum()
		{
			return DateTime.Now.ToString().Replace(" ", "").Replace("-", "").Replace(":", "").Replace(".", "").Replace("\\", "").Replace("/", "");
		}
		public static DateTime StringToDateTime(string s)
		{
			DateTime result = DateTime.Parse(SqlDateTime.MinValue.ToString());
			s = s.Trim();
			if (!s.Equals(""))
			{
				DateTime.TryParse(s, out result);
			}
			return result;
		}
		public static decimal StringToDecimal(string s)
		{
			decimal result = 0m;
			s = s.Trim();
			if (!s.Equals(""))
			{
				decimal.TryParse(s, out result);
			}
			return result;
		}
		public static string StrToMD5(string str)
		{
			byte[] bytes = Encoding.Default.GetBytes(str.Trim());
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] value = mD.ComputeHash(bytes);
			return BitConverter.ToString(value).Replace("-", "");
		}
		public string StrToSHA1(string str, Encoding encoding)
		{
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] bytes = encoding.GetBytes(str);
			byte[] inArray = sHA1CryptoServiceProvider.ComputeHash(bytes);
			sHA1CryptoServiceProvider.Clear();
			((IDisposable)sHA1CryptoServiceProvider).Dispose();
			return Convert.ToBase64String(inArray);
		}
		public static string ListToStr<T>(List<T> List, string includeBy, string splitChar)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (T current in List)
			{
				stringBuilder.AppendFormat("{1}{0}{1},", current, includeBy);
			}
			return stringBuilder.ToString().Trim(new char[]
			{
				char.Parse(splitChar)
			});
		}
		public static int GetLength(string str)
		{
			int result;
			if (str.Length == 0)
			{
				result = 0;
			}
			else
			{
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				int num = 0;
				byte[] bytes = aSCIIEncoding.GetBytes(str);
				for (int i = 0; i < bytes.Length; i++)
				{
					if (bytes[i] == 63)
					{
						num += 2;
					}
					else
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}
		public static string CutByStarTag(string str, string tag, bool isRemoteTag = false)
		{
			int num = str.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
			if (num > 0)
			{
				if (isRemoteTag)
				{
					str = str.Substring(num + tag.Length);
				}
				else
				{
					str = str.Substring(num);
				}
			}
			return str;
		}
		public static string CutByEndTag(string str, string tag)
		{
			int num = str.IndexOf(tag, StringComparison.OrdinalIgnoreCase);
			if (num > 0)
			{
				str = str.Substring(0, num);
			}
			return str;
		}
	}
}
