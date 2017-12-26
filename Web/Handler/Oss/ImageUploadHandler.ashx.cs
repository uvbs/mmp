using AliOss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.Oss
{
    /// <summary>
    /// ImageUploadHandler（单图片上传）
    /// </summary>
    public class ImageUploadHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLWebsiteDomainInfo bllWebsiteDomainInfo = new BLLWebsiteDomainInfo();
        UserInfo currentUserInfo;
        string websiteOwner;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            #region 检查是否登录

            currentUserInfo = bllWebsiteDomainInfo.GetCurrentUserInfo();
            if (currentUserInfo == null)
            {
                resp.Msg = "您还未登录";
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            websiteOwner = bllWebsiteDomainInfo.WebsiteOwner;
            #endregion
            #region 检查文件
            //文件夹
            string fd = context.Request["fd"];
            if (string.IsNullOrWhiteSpace(fd)) fd = "image";

            //检查文件
            if (context.Request.Files.Count == 0)
            {
                resp.Msg = "请选择文件";
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            string fileName = context.Request.Files[0].FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                resp.Msg = "请选择文件";
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //格式限制
            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".gif" && fileExtension != ".bmp"
                && fileExtension != ".webp" && fileExtension != ".tiff")
            {
                resp.Msg = "仅支持文件的格式jpg,png,bmp,gif,webp,tiff";
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            #endregion

            string filePath = "";
            try
            {
                filePath = OssHelper.UploadFile(OssHelper.GetBucket(websiteOwner), OssHelper.GetBaseDir(websiteOwner), currentUserInfo.UserID,"image", context.Request.Files[0]);
            }
            catch (Exception ex)
            {
                resp.Msg = ex.Message;
                context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            //返回字符串
            resp.IsSuccess = true;
            resp.ExStr = filePath;
            resp.Msg = "上传完成";
            context.Response.Write(Common.JSONHelper.ObjectToJson(resp));
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