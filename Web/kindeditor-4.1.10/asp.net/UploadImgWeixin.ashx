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
    /// <summary>
    /// 微信BLL
    /// </summary>
    BLLWeixin bllWeixin = new BLLWeixin();
    /// <summary>
    /// 把文件上传到微信服务器
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        this.context = context;
        
        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "jpg,png");
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

        if (imgFile.InputStream == null || imgFile.InputStream.Length >1024000)
        {
            showError("文件大小在1Mb以下。");
        }
        
        String dirName = context.Request.QueryString["dir"];
        if (String.IsNullOrEmpty(dirName))
        {
            dirName = "image";
        }
        if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        }

        

        string fileUrl = "";
        try
        {
            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName).ToLower();
            string localPath = context.Server.MapPath("~/FileUpload/") + dirName + "/" + newFileName;
            imgFile.SaveAs(localPath);
            string result = bllWeixin.UploadImageToWeixin(bllWeixin.GetAccessToken(), "image", localPath);
            Newtonsoft.Json.Linq.JToken jToken = Newtonsoft.Json.Linq.JToken.Parse(result);
            fileUrl=jToken["url"].ToString();
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
