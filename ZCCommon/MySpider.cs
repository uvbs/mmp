using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Data;

namespace ZentCloud.Common
{

    /// <summary>
    /// 数据采集方法集合类
    /// </summary>
    public class MySpider
    {

        /// <summary>
        /// 获取网页html源文件
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <param name="encoding">网页文件编码</param>
        /// <returns>html源文件</returns>
        #region GetPageSource
        public static string GetPageSource(string url, Encoding encoding, string cookiesStr = "")
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                if (!string.IsNullOrWhiteSpace(cookiesStr))
                {
                    req.Headers.Add("Cookie", cookiesStr);
                }
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), encoding);
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {

            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }
        #endregion

        /// <summary>
        /// 获取网页html源文件
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <param name="encodingStr">网页文件编码字符串</param>
        /// <returns>html源文件</returns>
        #region GetPageSource
        public static string GetPageSource(string url, string encodingStr)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(encodingStr));
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {

            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }
        #endregion

        /// <summary>
        /// 获取网页html源文件
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <returns>html源文件</returns>
        #region GetPageSource
        public static string GetPageSourceForGB2312(string url)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {

            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }
        #endregion


        public static string GetPageSourceForUTF8(string url)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {

            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        public static string GetPageSourceForGBK(string url)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Accept = "text/Html,application/xhtml+XML,application/xml;q=0.9,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("GBK"));
                strResult = reader.ReadToEnd();
                reader.Close();

            }
            catch
            {

            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            try
            {
                if (result.IndexOf(",") > -1)
                {
                    var tmp = result.Split(',');
                    result = tmp[0];
                }
            }
            catch { }

            return result;
        }
        public static string GetClientIP(HttpContext context)
        {
            try
            {
                string result = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (null == result || result == String.Empty)
                {
                    result = context.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (null == result || result == String.Empty)
                {
                    result = context.Request.UserHostAddress;
                }
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        
        /// <summary>
        ///根据IP地址获取IP所在地
        /// </summary>
        public static string GetIPLocation(string ip)
        {
            try
            {
                return IPLocation.IPLocation.IPLocate(HttpContext.Current.Server.MapPath("/FileUpload/IPLocation/qqwry.dat"), ip);
            }
            catch (Exception ex)
            {
                return "";// ex.Message;
            }
        }
        public static string GetIPLocation(string ip, HttpContext context)
        {

            try
            {
                return IPLocation.IPLocation.IPLocate(context.Server.MapPath("/FileUpload/IPLocation/qqwry.dat"), ip);
            }
            catch (Exception)
            {
                return "";
            }

        }
        
        /// <summary>
        /// 导出DataTable到Execl
        /// </summary>
        /// <param name="dt">传入页面</param>
        /// <param name="dt">数据表</param>
        /// <param name="fileName">生成Excel的名字 </param>
        public static void DataTableToExcel(Page page, DataTable dt, string fileName)
        {
            HttpResponse resp;
            //resp = Page.Response;
            resp = page.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            string colHeaders = "", ls_item = "";

            ////定义表对象与行对象，同时用DataSet对其值进行初始化 
            //DataTable dt = ds.Tables[0]; 
            DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的 
            int i = 0;
            int cl = dt.Columns.Count;

            //取得数据表各列标题，各标题之间以t分割，最后一个列标题后加回车符 
            for (i = 0; i < cl; i++)
            {
                if (i == (cl - 1))//最后一列，加n 
                {
                    colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                }
                else
                {
                    colHeaders += dt.Columns[i].Caption.ToString() + "\t";
                }

            }
            resp.Write(colHeaders);
            //向HTTP输出流中写入取得的数据信息 

            //逐行处理数据 
            foreach (DataRow row in myRow)
            {
                //当前行数据写入HTTP输出流，并且置空ls_item以便下行数据 
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))//最后一列，加n 
                    {
                        ls_item += row[i].ToString() + "\n";
                    }
                    else
                    {
                        ls_item += row[i].ToString() + "\t";
                    }

                }
                resp.Write(ls_item);
                ls_item = "";

            }
            resp.End();
        }

        /// <summary>
        /// 保存采集的图片
        /// </summary>
        /// <param name="url">图片网络路径</param>
        /// <param name="savePath">图片保存路径</param>
        public static bool PicGather(string url, string savePath)
        {
            try
            {
                System.Net.WebClient webClient = new System.Net.WebClient();

                System.Uri uri = new Uri(url);

                webClient.DownloadFile(uri.ToString(), savePath);

                webClient.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存采集的图片
        /// </summary>
        /// <param name="url">图片网络路径</param>
        public static byte[] PicGather(string url)
        {
            byte[] imgByte;
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    Uri uri = new Uri(url);
                    imgByte = webClient.DownloadData(uri);
                }
            }
            catch
            {
                return null;
            }
            return imgByte;
        }

        /// <summary>
        /// 采集网页源文件指定标签间内容
        /// </summary>
        /// <param name="url">链接地址</param>
        /// <param name="pageEncoding">编码:gb2312、utf8、gbk</param>
        /// <param name="startTag">开始标签</param>
        /// <param name="endTag">结束标签</param>
        /// <param name="isContainsStartTag">采集的内容是否包含查找的开始标签</param>
        /// <param name="isContainsEndTag">采集的内容是否包含查找的结束标签</param>
        /// <returns>返回采集到的指定内容</returns>
        public static string GatherPageContent(string url, string pageEncoding, string startTag, string endTag, bool isContainsStartTag, bool isContainsEndTag)
        {
            string result = "";

            //switch (pageEncoding)
            //{
            //    case "gb2312":
            //        result = GetPageSourceForGB2312(url);
            //        break;
            //    case "utf8":
            //        result = GetPageSourceForUTF8(url);
            //        break;
            //    case "gbk":
            //        result = GetPageSourceForGBK(url);
            //        break;
            //    default:
            //        break;
            //}

            result = GetPageSource(url, pageEncoding);

            result = GatherContentByStr(result, startTag, endTag, isContainsStartTag, isContainsEndTag);

            return result;
        }

        /// <summary>
        /// 根据字符串采集内容(分步处理方式：先处理开头标签，后处理结尾标签)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="startTag">开始标签</param>
        /// <param name="endTag">结束标签</param>
        /// <param name="isContainsStartTag">采集的内容是否包含查找的开始标签</param>
        /// <param name="isContainsEndTag">采集的内容是否包含查找的结束标签</param>
        /// <returns>返回采集到的指定内容</returns>
        public static string GatherContentByStr(string str, string startTag, string endTag, bool isContainsStartTag, bool isContainsEndTag)
        {
            string result = "";

            result = str.Trim().ToLower();

            if (result != "")
            {
                //处理开始部分
                if (result.Contains(startTag))
                {
                    if (isContainsStartTag)//判断是否包含开头标签内容
                    {
                        result = result.Substring(result.IndexOf(startTag));
                    }
                    else
                    {
                        result = result.Substring(result.IndexOf(startTag) + startTag.Length);
                    }
                }

                //处理结尾部分
                if (result.Contains(endTag))
                {
                    if (isContainsEndTag)
                    {
                        result = result.Substring(0, result.IndexOf(endTag) + endTag.Length);
                    }
                    else
                    {
                        result = result.Substring(0, result.IndexOf(endTag));
                    }

                }

            }

            return result;
        }




    }
}
