using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.IO;
using System.Text;
using System.Web.Routing;
using System.Web.Configuration;



namespace ZentCloud.JubitIMP.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //CommonPlatform.Helper.Aspose.WordHelper.SetLicense(); //Aspose设置License
            ZCDALEngine.DALEngine.GetMetas();//获取表结构
            ZentCloud.JubitIMP.Web.Comm.StaticData.InitStaticData(); //微信编辑器数据

            RouteConfig.RegisterRoutes(RouteTable.Routes);//注册路由
        }
        
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //#region 超过最大请求长度
            ////本代码的功能是检查页面请求的大小，如果超过了配置文件maxRequestLength的设定值，就提示用户超过了所允许的文件大小。
            ////从配置文件里得到配置的允许上传的文件大小
            //HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
            ////maxRequestLength 为整个页面的大小，不仅仅是上传文件的大小，所以扣除 100KB 的大小，
            ////maxRequestLength单位为KB
            //int maxRequestLength = (runTime.MaxRequestLength) * 1024;

            ////当前请求上下文的HttpApplication实例
            ////HttpContext context = ((HttpApplication)sender).Context;
            ////判断请求的内容长度是否超过了设置的字节数
            //if (Request.ContentLength > maxRequestLength)
            //{
            //    #region 不理解这些代码存在的意义
            //    /*
            //    //得到服务对象
            //    IServiceProvider provider = (IServiceProvider)context;
            //    HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

            //    //检查请求是否包含正文数据
            //    if (workerRequest.HasEntityBody())
            //    {
            //        //请求正文数据的长度
            //        int requestLength = workerRequest.GetTotalEntityBodyLength();
            //        //得到加载的初始字节数
            //        int initialBytes = 0;
            //        if (workerRequest.GetPreloadedEntityBody() != null)
            //            initialBytes = workerRequest.GetPreloadedEntityBody().Length;

            //        //检查是否所有请求数据可用
            //        if (!workerRequest.IsEntireEntityBodyIsPreloaded())
            //        {
            //            byte[] buffer = new byte[512000];
            //            //设置要接收的字节数为初始字节数
            //            int receivedBytes = initialBytes;
            //            //读取数据，并把所有读取的字节数加起来，判断总的大小
            //            while (requestLength - receivedBytes >= initialBytes)
            //            {
            //                //读取下一块字节
            //                initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);
            //                //更新接收到的字节数
            //                receivedBytes += initialBytes;
            //            }
            //            initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
            //        }
            //    }
            //    */
            //    #endregion
            //    //注意这里可以跳转，可以直接终止；在VS里调试时候得不到想要的结果，通过IIS才能得到想要的结果；FW4.0经典或集成都没问题
            //    Response.Write("请求大小" + Request.ContentLength);
            //    Response.End();
            //}
            //#endregion

            var path = HttpContext.Current.Request.Path.ToLower();

            List<string> pathFilter = new List<string>()
            {
                "/serv/api/admin/config/get.ashx",
                "/serv/api/admin/mall/exportorder.ashx",
                "/serv/api/admin/mall/statistics/export.ashx",
                "/serv/api/common/exportfromcache.ashx",
                "/serv/api/common/exportfromredis.ashx",
                "/handler/imghandler.ashx",
                "/handler/activity/activitydata.ashx",
                "/admin/booking/doctor/handler/exportorder.ashx",
                "/admin/distributionoffline/handler/projectcommission/export.ashx",
                "/serv/api/admin/mall/exportproduct.ashx",
                "/serv/api/admin/user/score/export.ashx",
                "/serv/api/admin/mall/settlement/supplier/exportsettlementlist.ashx",
                "/ValidateCode.aspx",
                "/serv/api/admin/mall/settlement/supplier/exportunsettlementlist.ashx",
                "/serv/api/admin/mall/settlement/supplier/exportsettlementdetail.ashx"
            };

            if (pathFilter.Contains(path))
            {
                return;
            }
            
            var rawUrl = HttpContext.Current.Request.RawUrl.ToLower();

            List<string> rawUrlFilter = new List<string>()
            {
                "DownLoadForwardData",
                "/Serv/API/Admin/Lottery/WinningData/Export.ashx",
                "/Serv/API/Admin/Question/ExportQuestionRecords.ashx",
                "/ValidateCode.aspx"
            };

            foreach (var item in rawUrlFilter)
            {
                if (rawUrl.IndexOf(item.ToLower()) > -1)
                {
                    return;
                }
            }

            string fileEx = Path.GetExtension(path);
            string currAbsolutePath = HttpContext.Current.Request.Url.AbsolutePath == null ? "" : HttpContext.Current.Request.Url.AbsolutePath.ToLower();
            List<string> pageExtraNameFilterList = new List<string>()
            {
                ".aspx",
                ".ashx",
                ".cn",
                ".com",
                ".net",
                ".chtml"
            };
            List<string> pageV2List = new List<string>()
            {
                "/",
                "/login",
                "/index",
                "/index2",
                "/adminlogin"
            };
            if (pageExtraNameFilterList.Contains(fileEx) || pageV2List.Exists(p => p.Equals(currAbsolutePath)))
            {
                Response.Filter = new ResponseFilter(Response.Filter);
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError().GetBaseException();
            //Response.Redirect(Common.ConfigHelper.GetConfigString("loginUrl") + "?error=" + Common.Base64Change.EncodeBase64ByUTF8(ex.Message));

            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
            //    {
            //        sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), ex.Message));
            //    }
            //}
            //catch { }
            //Response.Redirect("/error.htm");

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

    }

    /// <summary>
    /// 筛选器
    /// </summary>
    public class ResponseFilter : Stream
    {
        /// <summary>
        /// Oss旧域名
        /// </summary>
       static string ossOldDomain = Common.ConfigHelper.GetConfigString("OSS_Old_Domain");
        /// <summary>
        /// Oss新域名
        /// </summary>
       static string ossDomain = Common.ConfigHelper.GetConfigString("OSS_New_Domain");


        #region properties

        Stream responseStream;
        long position;
        StringBuilder html = new StringBuilder();
        #endregion

        #region constructor
        /// <summary>
        /// 筛选器
        /// </summary>
        /// <param name="inputStream"></param>
        public ResponseFilter(Stream inputStream)
        {
            responseStream = inputStream;
        }
        #endregion

        #region implemented abstract members

        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override void Close()
        {
            responseStream.Close();
        }
        public override void Flush()
        {
            responseStream.Flush();
        }
        public override long Length
        {
            get { return 0; }
        }
        public override long Position
        {
            get { return position; }
            set { position = value; }
        }
        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return responseStream.Seek(offset, direction);
        }
        public override void SetLength(long length)
        {
            responseStream.SetLength(length);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read(buffer, offset, count);
        }
        #endregion

        #region write method

        public override void Write(byte[] buffer, int offset, int count)
        {
            string sBuffer = System.Text.UTF8Encoding.UTF8.GetString(buffer, offset, count);
            if (!string.IsNullOrWhiteSpace(ossOldDomain) && !string.IsNullOrWhiteSpace(ossDomain))
            {
                foreach (var item in ossOldDomain.Split(','))
                {
                    //处理返回内容
                    if (sBuffer.Contains(item))
                    {
                        sBuffer = sBuffer.Replace(item, ossDomain);
                    }
                }
            }
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(sBuffer);
            responseStream.Write(data, 0, data.Length);
        }
        #endregion
    }
}