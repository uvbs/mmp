using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace CommonPlatform.Helper.Aspose
{
    public class WordHelper
    {
        private const string LicenseKey =
            "PExpY2Vuc2U+DQogIDxEYXRhPg0KICAgIDxMaWNlbnNlZFRvPkFzcG9zZSBTY290bGFuZCB" +
            "UZWFtPC9MaWNlbnNlZFRvPg0KICAgIDxFbWFpbFRvPmJpbGx5Lmx1bmRpZUBhc3Bvc2UuY2" +
            "9tPC9FbWFpbFRvPg0KICAgIDxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgT0VNPC9MaWNlbnNlV" +
            "HlwZT4NCiAgICA8TGljZW5zZU5vdGU+TGltaXRlZCB0byAxIGRldmVsb3BlciwgdW5saW1p" +
            "dGVkIHBoeXNpY2FsIGxvY2F0aW9uczwvTGljZW5zZU5vdGU+DQogICAgPE9yZGVySUQ+MTQ" +
            "wNDA4MDUyMzI0PC9PcmRlcklEPg0KICAgIDxVc2VySUQ+OTQyMzY8L1VzZXJJRD4NCiAgIC" +
            "A8T0VNPlRoaXMgaXMgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPg0KICAgIDxQc" +
            "m9kdWN0cz4NCiAgICAgIDxQcm9kdWN0PkFzcG9zZS5Ub3RhbCBmb3IgLk5FVDwvUHJvZHVj" +
            "dD4NCiAgICA8L1Byb2R1Y3RzPg0KICAgIDxFZGl0aW9uVHlwZT5FbnRlcnByaXNlPC9FZGl" +
            "0aW9uVHlwZT4NCiAgICA8U2VyaWFsTnVtYmVyPjlhNTk1NDdjLTQxZjAtNDI4Yi1iYTcyLT" +
            "djNDM2OGYxNTFkNzwvU2VyaWFsTnVtYmVyPg0KICAgIDxTdWJzY3JpcHRpb25FeHBpcnk+M" +
            "jAxNTEyMzE8L1N1YnNjcmlwdGlvbkV4cGlyeT4NCiAgICA8TGljZW5zZVZlcnNpb24+My4w" +
            "PC9MaWNlbnNlVmVyc2lvbj4NCiAgICA8TGljZW5zZUluc3RydWN0aW9ucz5odHRwOi8vd3d" +
            "3LmFzcG9zZS5jb20vY29ycG9yYXRlL3B1cmNoYXNlL2xpY2Vuc2UtaW5zdHJ1Y3Rpb25zLm" +
            "FzcHg8L0xpY2Vuc2VJbnN0cnVjdGlvbnM+DQogIDwvRGF0YT4NCiAgPFNpZ25hdHVyZT5GT" +
            "zNQSHNibGdEdDhGNTlzTVQxbDFhbXlpOXFrMlY2RThkUWtJUDdMZFRKU3hEaWJORUZ1MXpP" +
            "aW5RYnFGZkt2L3J1dHR2Y3hvUk9rYzF0VWUwRHRPNmNQMVpmNkowVmVtZ1NZOGkvTFpFQ1R" +
            "Hc3pScUpWUVJaME1vVm5CaHVQQUprNWVsaTdmaFZjRjhoV2QzRTRYUTNMemZtSkN1YWoyTk" +
            "V0ZVJpNUhyZmc9PC9TaWduYXR1cmU+DQo8L0xpY2Vuc2U+";

        private static Stream LStream = (Stream)new MemoryStream(Convert.FromBase64String(LicenseKey));

        public static void SetLicense()
        {
            //CleanOldHtmlDir();//清除历史文件

            License li = new License();
            li.SetLicense(LStream);
        }
        public static string WordToHtml(Stream stream)
        {
            Stream outputStream = new MemoryStream();
            Document d = new Document(stream);
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            string path = "/FileUpload/WordToHtml/" + guid + "/";
            string filePath = HttpContext.Current.Server.MapPath(path + guid + ".html");
            d.Save(filePath, SaveFormat.Html);
            string result = File.ReadAllText(filePath);
            List<string> imgs = ZentCloud.Common.MyRegex.GetPadImg(result);
            foreach (string img in imgs)
            {
                string src = ZentCloud.Common.MyRegex.GetPadSrcUrl(img);
                string srcPath = HttpContext.Current.Server.MapPath(path + src);
                byte[] bt = File.ReadAllBytes(srcPath);
                string imgBase64 = Convert.ToBase64String(bt);
                result = Regex.Replace(result, src, "data:image/png;base64," + imgBase64, RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrWhiteSpace(result))
            {
                result = ZentCloud.Common.MyRegex.GetPadBody(result);
            }
            return result;
        }
        public static void CleanOldHtmlDir()
        {
            string path = HttpContext.Current.Server.MapPath("/FileUpload/WordToHtml/");
            foreach (string cpath in Directory.GetDirectories(path))
	        {
		        if(Directory.GetLastAccessTime(cpath)< DateTime.Now.AddHours(-1)){
                    Directory.Delete(cpath,true);
                }
	        }
        }
    }
}
