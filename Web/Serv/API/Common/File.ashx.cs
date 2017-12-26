using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Globalization;
using System.Collections;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using AliOss;
using System.Drawing;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// 文件上传接口
    /// </summary>
    public class File : BaseHandler
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
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,pdf,htm,html,txt,zip,rar,gz,bz2,mp3,wmv");
            extTable.Add("app", "apk,app,ipa");

            String dirName = context.Request["dir"];
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
            if (currentUserInfo != null) {
                userId = currentUserInfo.UserID;
            }
            else
            {
                userId = "other";
            }

            List<string> fileUrlList = new List<string>();
            List<dynamic> otherInfoList = new List<dynamic>();

            var postFileList = context.Request.Files;
            for (int i = 0; i < postFileList.Count; i++)
            {
                String fileName = postFileList[i].FileName;
                String fileExt = Path.GetExtension(fileName).ToLower();

                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    return ZentCloud.Common.JSONHelper.ObjectToJson(new
                    {
                        errcode = 1,
                        errmsg = "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。"
                    });
                }
                try
                {
                    int maxWidth = 800;
                    int maxHeight = 0;
                    if (!string.IsNullOrWhiteSpace(context.Request["maxWidth"])) maxWidth = Convert.ToInt32(context.Request["maxWidth"]);
                    if (!string.IsNullOrWhiteSpace(context.Request["maxHeight"])) maxHeight = Convert.ToInt32(context.Request["maxHeight"]);
                    if (dirName == "image" && (maxWidth > 0 || maxHeight > 0))
                    {
                        ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
                        Image image = Image.FromStream(postFileList[i].InputStream);

                        if ((image.Width > maxWidth && maxWidth > 0) || (image.Height > maxHeight && maxHeight > 0))
                        {
                            double ratio = imgHelper.GetRatio(image.Width, image.Height, maxWidth, maxHeight);
                            int newWidth = Convert.ToInt32(Math.Round(ratio * image.Width));
                            int newHeight = Convert.ToInt32(Math.Round(ratio * image.Height));
                            image = imgHelper.PhotoSizeChange(image, newWidth, newHeight);
                        }
                        if (ZentCloud.Common.ConfigHelper.GetConfigInt("Oss_Enable") == 0)
                        {
                            //文件保存目录路径
                            String fname = Guid.NewGuid().ToString("N").ToUpper() + fileExt;
                            String savePath = "/FileUpload/" + dirName + "/" + bll.WebsiteOwner + "/" + userId + "/" + DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo) + "/";
                            String serverPath = context.Server.MapPath(savePath);
                            if (!Directory.Exists(serverPath))
                            {
                                Directory.CreateDirectory(serverPath);
                            }
                            String serverFile = serverPath + fname;
                            String saveFile = savePath + fname;
                            image.Save(serverFile, imgHelper.GetImageFormat(fileExt));

                            fileUrlList.Add(saveFile);

                            otherInfoList.Add(new
                            {
                                width = image.Width,
                                height = image.Height,
                                old_name = postFileList[i].FileName,
                                filelength = postFileList[i].ContentLength
                            });
                        }
                        else
                        {

                            Stream stream = new MemoryStream();
                            image.Save(stream, imgHelper.GetImageFormat(fileExt));
                            stream.Position = 0;

                            string fileUrl = bllUploadOtherServer.upload(stream, fileExt); //= OssHelper.UploadFileFromStream(OssHelper.GetBucket(webSiteInfo.WebsiteOwner), OssHelper.GetBaseDir(webSiteInfo.WebsiteOwner), userId, "image", stream, fileExt);

                            fileUrlList.Add(fileUrl);

                            otherInfoList.Add(new
                            {
                                width = image.Width,
                                height = image.Height,
                                old_name = postFileList[i].FileName,
                                filelength = postFileList[i].ContentLength
                            });
                        }
                    }
                    else
                    {

                        if (ZentCloud.Common.ConfigHelper.GetConfigInt("Oss_Enable") == 0)
                        {
                            //文件保存目录路径
                            String fname = Guid.NewGuid().ToString("N").ToUpper() + fileExt;
                            String savePath = "/FileUpload/" + dirName + "/" + bll.WebsiteOwner + "/" + userId + "/" + DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo) + "/";
                            String serverPath = context.Server.MapPath(savePath);
                            if (!Directory.Exists(serverPath))
                            {
                                Directory.CreateDirectory(serverPath);
                            }
                            String serverFile = serverPath + fname;
                            String saveFile = savePath + fname;
                            fileUrlList.Add(saveFile);
                            if (dirName == "image")
                            {
                                ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
                                Image image = Image.FromStream(postFileList[i].InputStream);
                                image.Save(serverFile, imgHelper.GetImageFormat(fileExt));
                                otherInfoList.Add(new
                                {
                                    width = image.Width,
                                    height = image.Height,
                                    old_name = postFileList[i].FileName,
                                    filelength = postFileList[i].ContentLength
                                });
                            }
                            else
                            {
                                postFileList[i].SaveAs(serverFile);
                                otherInfoList.Add(new
                                {
                                    old_name = postFileList[i].FileName,
                                    file_length = postFileList[i].ContentLength
                                });
                            }
                        }
                        else
                        {
                            String fileUrl = OssHelper.UploadFile(OssHelper.GetBucket(webSiteInfo.WebsiteOwner), OssHelper.GetBaseDir(webSiteInfo.WebsiteOwner), userId, dirName, postFileList[i]);
                            fileUrlList.Add(fileUrl);

                            if (dirName == "image")
                            {
                                Image image = Image.FromStream(postFileList[i].InputStream);
                                otherInfoList.Add(new
                                {
                                    width = image.Width,
                                    height = image.Height,
                                    old_name = postFileList[i].FileName,
                                    filelength = postFileList[i].ContentLength
                                });
                            }
                            else
                            {
                                otherInfoList.Add(new
                                {
                                    old_name = postFileList[i].FileName,
                                    file_length = postFileList[i].ContentLength
                                });
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ZentCloud.Common.ConfigHelper.GetConfigInt("Oss_Enable") == 0)
                    {
                        return ZentCloud.Common.JSONHelper.ObjectToJson(new
                        {
                            errcode = 4,
                            errmsg = "上传出错：" + ex.Message
                        });
                    }
                    else
                    {
                        return ZentCloud.Common.JSONHelper.ObjectToJson(new
                        {
                            errcode = 4,
                            errmsg = "上传到Oss出错：" + ex.Message
                        });
                    }
                }
            }
            //
            if (fileUrlList.Count > 0)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "ok",
                    file_url_list = fileUrlList,
                    other_info_list = otherInfoList
                });
            }
            else
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    errmsg = "fail"

                });
            }
        }

    }
}