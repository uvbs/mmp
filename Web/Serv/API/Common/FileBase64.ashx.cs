using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Globalization;
using System.Collections;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using AliOss;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 文件上传接口 BASE 64
    /// </summary>
    public class FileBase64 : BaseHandler
    {
        /// <summary>
        /// 
        /// </summary>
        BLL bll = new BLL();

        BLLUploadOtherServer bllUploadOtherServer = new BLLUploadOtherServer();
        /// <summary>
        /// 当前用户
        /// </summary>
        UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// 当前站点
        /// </summary>
        WebsiteInfo webSiteInfo = new WebsiteInfo();
        /// <summary>
        /// 当前用户
        /// </summary>
        string userId = "other";

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,ppt,htm,html,txt,zip,rar,gz,bz2,mp3,wmv");

            String dirName = context.Request.QueryString["dir"];
            context.Response.ContentType = "text/plain";
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }

            webSiteInfo = bll.GetWebsiteInfoModel();
            if (webSiteInfo == null)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 2,
                    errmsg = "找不到站点信息。"
                });
            }
            currentUserInfo = bll.GetCurrentUserInfo();
            if (currentUserInfo != null)
            {
                userId = currentUserInfo.UserID;
            }
            string data=context.Request["data"];
            string fileExt=context.Request["file_ext"];
            if (string.IsNullOrEmpty(fileExt))
            {
                fileExt = ".jpg";
            }
            try
            {
                String fileUrl = bllUploadOtherServer.uploadFromBase64(data, fileExt) ;// = OssHelper.UploadFileFromBase64(OssHelper.GetBucket(webSiteInfo.WebsiteOwner), OssHelper.GetBaseDir(webSiteInfo.WebsiteOwner), userId, dirName, data, fileExt);
                
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                    {
                        errcode = 0,
                        errmsg = "ok",
                        file_url = fileUrl
                    });
            }
            catch (Exception ex)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 4,
                    errmsg = "上传到Oss出错：" + ex.Message
                });
            }

        }

    }
}