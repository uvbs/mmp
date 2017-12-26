using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using Yahoo.Yui.Compressor;
using System.IO.Compression;

namespace CommonPlatform.Helper
{
    public class MergingCompactionHelper
    {
        //js,css合并
        public static string ToCompressor(string type, string files){
            List<string> list = files.Split(',').ToList();
            Compressor compressor = new JavaScriptCompressor();
            bool canCompressor = (type == "js" || type == "css");
            if (type == "css") compressor = new CssCompressor();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(list[i])) continue;
                var li = list[i].Split('|');
                string lipath = HttpContext.Current.Server.MapPath(li[0]);
                if (!File.Exists(lipath)) continue;
                string readstr = File.ReadAllText(lipath, Encoding.UTF8);
                try
                {
                    if (canCompressor && li.Length == 2 && li[1] == "1") readstr = compressor.Compress(readstr);
                }
                catch (Exception ex)
                {
                    throw new Exception("压缩出错:"+li[0]);
                }

                sb.AppendLine(readstr);
            }
            return sb.ToString();
        }
        //文本内容gzip压缩
        public static byte[] ToGzip(string filestring)
        {
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes(filestring);  
            MemoryStream ms = new MemoryStream();
            using (GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
            {
                compressedzipStream.Write(rawData, 0, rawData.Length);  
            }
            return ms.ToArray();
        }
    }
}
