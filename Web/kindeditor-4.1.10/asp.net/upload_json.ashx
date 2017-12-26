<%@ WebHandler Language="C#" Class="Upload" %>



using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using ZentCloud.Common;
using ZentCloud.JubitIMP.Web;
using System.Web.SessionState;
public class Upload : IHttpHandler, IReadOnlySessionState
{
    private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

        //文件保存目录路径
        String savePath = "/FileUpload/Weibo/";

        //文件保存目录URL
        String saveUrl = "/FileUpload/Weibo/";//aspxUrl + 

        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2,mp3,wmv");

        //最大文件大小
        //int maxSize = 900000000;
        this.context = context;

        HttpPostedFile imgFile = context.Request.Files["imgFile"];
        if (imgFile == null)
        {
            showError("请选择文件。");
        }

        String dirPath = context.Server.MapPath(savePath);
        if (!Directory.Exists(dirPath))
        {
            showError("上传目录不存在。");
        }

        String dirName = context.Request.QueryString["dir"];
        if (String.IsNullOrEmpty(dirName))
        {
            dirName = "image";
        }
        if (!extTable.ContainsKey(dirName))
        {
            showError("目录名不正确。");
        }

        String fileName = imgFile.FileName;
        var filepath=fileName.Split('\\');
        if (filepath.Length>1)//ie模式
        {
            fileName = filepath[filepath.Length - 1];
        }
        String fileExt = Path.GetExtension(fileName).ToLower();

        //if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        //{
        //    showError("上传文件大小超过限制。");
        //}

        if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        }

        //创建文件夹
        dirPath += dirName + "/";
        saveUrl += dirName + "/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        string userName = DataLoadTool.GetCurrUserID();
        if (string.IsNullOrEmpty(userName))
        {
            userName = "comm";
        }
        String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
        //dirPath += userName + "/" + ymd + "/" + context.Session["SendBatchGUID"] + "/";
        //saveUrl += userName + "/" + ymd + "/" + context.Session["SendBatchGUID"] + "/";
        dirPath += userName + "/" + ymd + "/";
        saveUrl += userName + "/" + ymd + "/" ;

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        //String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
        String newFileName = fileName;

        String filePath = dirPath + newFileName;

        imgFile.SaveAs(filePath);

        String fileUrl = saveUrl + newFileName;
        fileUrl = new ZentCloud.BLLJIMP.BLLImage().CreateThumbImage(fileUrl, 600, 600);
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
