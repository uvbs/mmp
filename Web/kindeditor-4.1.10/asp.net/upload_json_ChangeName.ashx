<%@ WebHandler Language="C#" Class="Upload" %>



using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using ZentCloud.Common;
using ZentCloud.JubitIMP.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using AliOss;
using System.Web.SessionState;

public class Upload : IHttpHandler, IReadOnlySessionState
{
    private HttpContext context;
    private BLLUser bllUser = new BLLUser();
    private UserInfo currentUserInfo = new UserInfo();
    private WebsiteInfo webSite = new WebsiteInfo();

    public void ProcessRequest(HttpContext context)
    {
        this.context = context;

        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        extTable.Add("file", "doc,docx,xls,ppt,htm,html,txt,zip,rar,gz,bz2,mp3,wmv");

        HttpPostedFile imgFile = context.Request.Files["imgFile"];

        if (imgFile == null)
        {
            showError("请选择文件。");
        }

        String fileName = imgFile.FileName;
        var filepath = fileName.Split('\\');
        if (filepath.Length > 1)//ie模式
        {
            fileName = filepath[filepath.Length - 1];
        }
        String fileExt = Path.GetExtension(fileName).ToLower();

        //if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        //{
        //    showError("上传文件大小超过限制。");
        //}

        String dirName = context.Request.QueryString["dir"];
        if (String.IsNullOrEmpty(dirName))
        {
            dirName = "image";
        }
        if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        }


        webSite = bllUser.GetWebsiteInfoModel();
        if (webSite == null)
        {
            showError("找不到站点信息。");
        }
        currentUserInfo = bllUser.GetCurrentUserInfo();
        string userId = "";
        if (currentUserInfo == null)
        {
            if (!string.IsNullOrWhiteSpace(context.Request["userID"]))
            {
                userId = context.Request["userID"];
            }
        }
        else
        {
            userId = currentUserInfo.UserID;
        }
        if (string.IsNullOrWhiteSpace(userId))
        {
            showError("找不到当前用户信息。");
        }

        string fileUrl = "";
        try
        {
            if (ZentCloud.Common.ConfigHelper.GetConfigInt("Oss_Enable") == 0)
            {
                String fname = Guid.NewGuid().ToString("N").ToUpper() + fileExt;
                String savePath = "/FileUpload/" + dirName + "/" + webSite.WebsiteOwner + "/" + userId + "/" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/";
                String serverPath = context.Server.MapPath(savePath);
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }
                String serverFile = serverPath + fname;
                String saveFile = savePath + fname;
                imgFile.SaveAs(serverFile);
                fileUrl = saveFile;
            }
            else
            {
                fileUrl = new BLLUploadOtherServer().uploadFromHttpPostedFile(imgFile);//OssHelper.UploadFile(OssHelper.GetBucket(webSite.WebsiteOwner), OssHelper.GetBaseDir(webSite.WebsiteOwner), userId, dirName, imgFile);

            }
        }
        catch (Exception ex)
        {
            showError(ex.Message);
        }
        Hashtable hash = new Hashtable();
        hash["error"] = 0;
        hash["url"] = fileUrl;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JSONHelper.ObjectToJson(hash));
        context.Response.End();
    }

    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JSONHelper.ObjectToJson(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
