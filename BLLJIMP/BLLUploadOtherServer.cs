using AliOss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 图片文件上传到其他第三方服务平台
    /// </summary>
    public class BLLUploadOtherServer : BLL
    {
        public string upload(WebsiteInfo websiteInfo,string userId, Stream fileStream, string fileExtension)
        {
            string fileUrl = "";

            //判断是否有用到微软云
            if (HasWindowsAzure(websiteInfo))
            {
                //AZStorage.Client azClient = new AZStorage.Client("ehtwshopoos", "+8nN68pmvaGax4UqrowjKFbjKikPatgk/hLOZjDMzwJ8YORztDl3vQo2JyDnhYdWkEiJ4+4mXyP0KHA5gL2tOw==", "ehtwshopimg");
                AZStorage.Client azClient = new AZStorage.Client(websiteInfo.AzureAccountName, websiteInfo.AzureAccountKey, websiteInfo.AzureContainerName);
                fileUrl = azClient.upload(fileStream, fileExtension);
            }
            else
            {
                fileUrl = OssHelper.UploadFileFromStream(OssHelper.GetBucket(websiteInfo.WebsiteOwner), OssHelper.GetBaseDir(websiteInfo.WebsiteOwner), userId, "image", fileStream, fileExtension);
            }

            return fileUrl;
        }

        public string upload(Stream fileStream, string fileExtension)
        {
            WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase();
            string userId = GetCurrUserID();

            return upload(websiteInfo, userId, fileStream, fileExtension);
        }

        public string uploadFromBase64(string base64Str, string fileExtension)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Str);
            //读入MemoryStream对象
            Stream stream = new MemoryStream(imageBytes);

            return upload(stream, fileExtension);
        }

        public string uploadFromHttpPostedFile(HttpPostedFile file)
        {
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();

            return upload(file.InputStream, fileExtension);
        }

        public string uploadFromByte(byte[] fileByte, string fileExtension)
        {
            //读入MemoryStream对象
            Stream stream = new MemoryStream(fileByte);
            return upload(stream, fileExtension);
        }

        /// <summary>
        /// 是否有微软云配置
        /// </summary>
        /// <param name="websiteInfo"></param>
        /// <returns></returns>
        public bool HasWindowsAzure(WebsiteInfo websiteInfo)
        {
            return !string.IsNullOrWhiteSpace(websiteInfo.AzureAccountName)
                    && !string.IsNullOrWhiteSpace(websiteInfo.AzureAccountKey)
                    && !string.IsNullOrWhiteSpace(websiteInfo.AzureContainerName);
        }

    }
}
