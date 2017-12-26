using AliOss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// UeditorController 的摘要说明
    /// </summary>
    public class UeditorController : IHttpHandler, IReadOnlySessionState
    {
        private BLLUser bllUser = new BLLUser();
        private UserInfo currentUserInfo = new UserInfo();
        private WebsiteInfo webSite = new WebsiteInfo();
        public void ProcessRequest(HttpContext context)
        {
            Ueditor.Helper.Handler action = null;
            string configPath = context.Request["configPath"];
            switch (context.Request["action"])
            {
                case "uploadimage":
                    webSite = bllUser.GetWebsiteInfoModelFromDataBase();
                    currentUserInfo = bllUser.GetCurrentUserInfo();
                    break;
                default:
                    break;
            }
            switch (context.Request["action"])
            {
                case "config":
                    action = new Ueditor.Helper.ConfigHandler(context, configPath);
                    break;
                case "uploadimage":
                    action = new Ueditor.Helper.UploadHandler(context, new Ueditor.Helper.UploadConfig()
                    {
                        AllowExtensions = Ueditor.Helper.Config.GetStringList("imageAllowFiles", configPath),
                        PathFormat = Ueditor.Helper.Config.GetString("imagePathFormat", configPath),
                        SizeLimit = Ueditor.Helper.Config.GetInt("imageMaxSize", configPath),
                        UploadFieldName = Ueditor.Helper.Config.GetString("imageFieldName", configPath),
                        WebsiteOwner = webSite.WebsiteOwner,
                        OssBucket = webSite.OssBucket,
                        UserID = currentUserInfo.UserID,
                        FileType = "image"
                    });
                    break;
                case "uploadscrawl":
                    action = new Ueditor.Helper.UploadHandler(context, new Ueditor.Helper.UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = Ueditor.Helper.Config.GetString("scrawlPathFormat", configPath),
                        SizeLimit = Ueditor.Helper.Config.GetInt("scrawlMaxSize", configPath),
                        UploadFieldName = Ueditor.Helper.Config.GetString("scrawlFieldName", configPath),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                    break;
                case "uploadvideo":
                    action = new Ueditor.Helper.UploadHandler(context, new Ueditor.Helper.UploadConfig()
                    {
                        AllowExtensions = Ueditor.Helper.Config.GetStringList("videoAllowFiles", configPath),
                        PathFormat = Ueditor.Helper.Config.GetString("videoPathFormat", configPath),
                        SizeLimit = Ueditor.Helper.Config.GetInt("videoMaxSize", configPath),
                        UploadFieldName = Ueditor.Helper.Config.GetString("videoFieldName", configPath)
                    });
                    break;
                case "uploadfile":
                    action = new Ueditor.Helper.UploadHandler(context, new Ueditor.Helper.UploadConfig()
                    {
                        AllowExtensions = Ueditor.Helper.Config.GetStringList("fileAllowFiles", configPath),
                        PathFormat = Ueditor.Helper.Config.GetString("filePathFormat", configPath),
                        SizeLimit = Ueditor.Helper.Config.GetInt("fileMaxSize", configPath),
                        UploadFieldName = Ueditor.Helper.Config.GetString("fileFieldName", configPath)
                    });
                    break;
                case "listimage":
                    action = new Ueditor.Helper.ListFileManager(context, Ueditor.Helper.Config.GetString("imageManagerListPath", configPath), Ueditor.Helper.Config.GetStringList("imageManagerAllowFiles", configPath),configPath);
                    break;
                case "listfile":
                    action = new Ueditor.Helper.ListFileManager(context, Ueditor.Helper.Config.GetString("fileManagerListPath", configPath), Ueditor.Helper.Config.GetStringList("fileManagerAllowFiles", configPath), configPath);
                    break;
                case "catchimage":
                    action = new Ueditor.Helper.CrawlerHandler(context, configPath);
                    break;
                default:
                    action = new Ueditor.Helper.NotSupportedHandler(context);
                    break;
            }
            action.Process();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}