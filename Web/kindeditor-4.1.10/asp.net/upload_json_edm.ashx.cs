using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.JubitIMP.Web.Comm;
using System.Globalization;
using System.IO;
using LitJson;
using System.Collections;

namespace ZentCloud.JubitIMP.Web.Kindeditor.asp.net
{
    /// <summary>
    /// 导入电子邮件 处理文件
    /// </summary>
    public class upload_json_edm : IHttpHandler, IRequiresSessionState
    {
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            


            //文件保存目录路径
            String savePath = "/FileUpload/EDM/";

            //文件保存目录URL
            String saveUrl = "/FileUpload/EDM/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("file", "txt,csv");

            //最大文件大小
            int maxSize = 400000000;
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
                dirName = "file";
            }
            if (!extTable.ContainsKey(dirName))
            {
                showError("目录名不正确。");
            }

            String fileName = imgFile.FileName;
            var filepath = fileName.Split('\\');
            if (filepath.Length > 1)//ie模式
            {
                fileName = filepath[filepath.Length - 1];
            }
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                showError("上传文件大小超过限制。");
            }

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

            String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += userName + "/" + ymd + "/";
            saveUrl += userName + "/" + ymd + "/" ;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = Guid.NewGuid() + fileExt;

            String filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
           

        }

        private void showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
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
}