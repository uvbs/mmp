using Aliyun.OpenServices.OpenStorageService;
using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.Common;

namespace AliOss
{
    public class OssHelper
    {
        public static string GetBucket(string websiteOwner)
        {
            return ConfigHelper.GetConfigString("Oss_Bucket");
        }
        public static string GetBaseDir(string websiteOwner)
        {
            return "www/" + websiteOwner;
        }

        public static OssClient ConnOssClient()
        {
            string ossPoint = ConfigHelper.GetConfigString("Oss_Point");
            string accessId = ConfigHelper.GetConfigString("Oss_AccessId");
            string accessKey = ConfigHelper.GetConfigString("Oss_AccessKey");
            return new OssClient(ossPoint, accessId, accessKey);
        }
        public static string GetDomain()
        {
            return ConfigHelper.GetConfigString("Oss_Domain");
        }
        #region 创建bucket
        /// <summary>
        /// 创建bucket
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="aclType">开放类型</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static bool CreateBucket(string bucketName, string aclType)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");



            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (client.DoesBucketExist(bucketName)) throw new Exception("Bucket已经存在");

            //创建Bucket
            client.CreateBucket(bucketName);

            //设置读写
            switch (aclType)
            {
                case "1":
                    client.SetBucketAcl(bucketName, CannedAccessControlList.PublicRead);
                    break;
                case "2":
                    client.SetBucketAcl(bucketName, CannedAccessControlList.Private);
                    break;
                default:
                    client.SetBucketAcl(bucketName, CannedAccessControlList.PublicReadWrite);
                    break;
            }
            return true;
        }
        #endregion

        #region 检查bucket是否存在
        /// <summary>
        /// 检查bucket是否存在
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static bool CheckBucket(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();

            return client.DoesBucketExist(bucketName);
        }
        #endregion

        #region 修改bucket的Acl开放类型
        /// <summary>
        /// 创建bucket
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="aclType">开放类型</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static bool SetBucketAcl(string bucketName, string aclType)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //设置读写
            switch (aclType)
            {
                case "1":
                    client.SetBucketAcl(bucketName, CannedAccessControlList.PublicRead);
                    break;
                case "2":
                    client.SetBucketAcl(bucketName, CannedAccessControlList.Private);
                    break;
                default:
                    client.SetBucketAcl(bucketName, CannedAccessControlList.PublicReadWrite);
                    break;
            }
            return true;
        }
        #endregion
        
        #region 修改bucket的白名单
        /// <summary>
        /// 创建bucket
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="referers">白名单字符串数组Json</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static bool SetBucketReferer(string bucketName, string referers)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //设置白名单
            if (string.IsNullOrWhiteSpace(referers))
            {
                var request = new SetBucketRefererRequest(bucketName);
                client.SetBucketReferer(request);
            }
            else
            {
                List<string> refererList = JSONHelper.JsonToModel<List<string>>(referers);//jSON 反序列化
                var request = new SetBucketRefererRequest(bucketName, refererList);
                client.SetBucketReferer(request);
            }
            return true;
        }
        #endregion

        #region 检查文件是否已存在

        /// <summary>
        /// 检查bucket是否存在
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="key">key文件key</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static bool CheckFile(string bucketName, string key)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();

            //网络连接中取出key
            if (key.ToLower().StartsWith("http"))
            { 
                string tempKey = key.Replace("://","");
                key = tempKey.Substring(tempKey.IndexOf("/")+1);
            }

            return client.DoesObjectExist(bucketName,key);
        }
        #endregion

        #region 上传文件到Oss
        /// <summary>
        /// 上传文件到Oss
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="dir">文件夹</param>
        /// <param name="userId">用户ID</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="file">文件</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <param name="newFileName">是否新文件名 默认生成新文件名</param>
        /// <returns></returns>
        public static string UploadFile(string bucketName, string dir, string userId ,string fileType, HttpPostedFile file,bool newFileName=true)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");
            if (string.IsNullOrWhiteSpace(userId)) throw new Exception("userId为空");
            if (string.IsNullOrWhiteSpace(fileType)) throw new Exception("fileType为空");

            if(file ==null || string.IsNullOrWhiteSpace(file.FileName)) throw  new Exception("请选择文件");
            
            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if(!client.DoesBucketExist(bucketName)) throw  new Exception("Bucket还没有建立");

            string fileName = file.FileName;

            string fileExtension = Path.GetExtension(fileName).ToLower();

            //文件保存Key生成
            string Key = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString("N").ToUpper() + fileExtension;

            if (!newFileName)//保留原文件名
            {
                Key = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileName;
            }
            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            Metadata.CacheControl = string.Format("max-age={0}", 157680000);
            //Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));

            //上传文件到Oss
            PutObjectResult putResult = client.PutObject(bucketName, Key, file.InputStream, Metadata);
            return GetDomain() + "/" + Key;
        }


        /// <summary>
        /// 上传文件到Oss
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="dir">文件夹</param>
        /// <param name="userId">用户ID</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="relPath">服务器相对路径</param>
        /// <param name="physicalPath">服务器物理路径</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static string UploadFile(string bucketName, string dir, string userId, string fileType, string relPath,string physicalPath="")
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");
            if (string.IsNullOrWhiteSpace(userId)) throw new Exception("userId为空");
            if (string.IsNullOrWhiteSpace(fileType)) throw new Exception("fileType为空");

            // if (file == null || string.IsNullOrWhiteSpace(file.FileName)) throw new Exception("请选择文件");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");
            FileStream file;
            string fileExtension;
            if (!string.IsNullOrEmpty(physicalPath))
            {
                fileExtension = Path.GetExtension(physicalPath).ToLower();
                file = System.IO.File.OpenRead(physicalPath);
            }
            else
            {
                fileExtension = Path.GetExtension(relPath).ToLower();
                file = System.IO.File.OpenRead(HttpContext.Current.Server.MapPath(relPath));
            }
            //string fileName = file.FileName;

            

            //文件保存Key生成
            string Key = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString("N").ToUpper() + fileExtension;

            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            Metadata.CacheControl = string.Format("max-age={0}", 157680000);
            //Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));

            //using (StreamWriter sw = new StreamWriter(@"C:\MonitorHandlerException.txt", true, Encoding.UTF8))
            //{
            //    sw.WriteLine(string.Format("{0} 上传ossKey：{1}bucketName:{2} FileLeng{3}", DateTime.Now.ToString(), Key,bucketName,file.Length));

            //}
            //上传文件到Oss
            PutObjectResult putResult = client.PutObject(bucketName, Key, file, Metadata);
            return GetDomain() + "/" + Key;
        }


        /// <summary>
        /// 上传文件到Oss
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="dir">文件夹</param>
        /// <param name="userId">用户ID</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="base64Str">文件Base64字符串</param>
        /// <param name="fileExt">文件后缀</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static string UploadFileFromBase64(string bucketName, string dir, string userId, string fileType, string base64Str, string fileExt)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");
            if (string.IsNullOrWhiteSpace(userId)) throw new Exception("userId为空");
            if (string.IsNullOrWhiteSpace(fileType)) throw new Exception("fileType为空");

            if (string.IsNullOrWhiteSpace(base64Str)) throw new Exception("请选择文件");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            byte[] imageBytes = Convert.FromBase64String(base64Str);
            //读入MemoryStream对象
            Stream stream = new MemoryStream(imageBytes);

            string fileExtension = fileExt.ToLower();

            //文件保存Key生成
            string Key = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString("N").ToUpper() + fileExtension;

            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            Metadata.CacheControl = string.Format("max-age={0}", 157680000);
            //Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));

            //上传文件到Oss
            client.PutObject(bucketName, Key, stream, Metadata);

            return GetDomain() + "/" + Key;
        }

        /// <summary>
        /// 上传文件到Oss
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="dir">文件夹</param>
        /// <param name="userId">用户ID</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="FileStream">文件流</param>
        /// <param name="fileExt">文件后缀</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static string UploadFileFromStream(string bucketName, string dir, string userId, string fileType, Stream FileStream, string fileExt)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");
            if (string.IsNullOrWhiteSpace(userId)) throw new Exception("userId为空");
            if (string.IsNullOrWhiteSpace(fileType)) throw new Exception("fileType为空");

            if (FileStream.Length ==0) throw new Exception("请选择文件");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //读入MemoryStream对象
            string fileExtension = fileExt.ToLower();

            //文件保存Key生成
            string Key = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString("N").ToUpper() + fileExtension;

            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            Metadata.CacheControl = string.Format("max-age={0}", 157680000);
            //Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));

            //上传文件到Oss
            client.PutObject(bucketName, Key, FileStream, Metadata);

            return GetDomain() + "/" + Key;
        }


        /// <summary>
        /// 上传文件到Oss 文件名不变
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="bucketName">bucket名</param>
        /// <param name="dir">文件夹</param>
        /// <param name="userId">用户ID</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="FileStream">文件流</param>
        /// <param name="fileExt">文件后缀</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static string UploadFileFromStream(string fileName,string bucketName, string dir, string userId, string fileType, Stream FileStream)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");
            if (string.IsNullOrWhiteSpace(userId)) throw new Exception("userId为空");
            if (string.IsNullOrWhiteSpace(fileType)) throw new Exception("fileType为空");

            if (FileStream.Length == 0) throw new Exception("请选择文件");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //读入MemoryStream对象
            string fileExtension = Path.GetExtension(fileName);

            //文件保存Key生成
            string key = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fileName;

            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            Metadata.CacheControl = string.Format("max-age={0}", 157680000);
            //Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));

            //上传文件到Oss
            client.PutObject(bucketName, key, FileStream, Metadata);

            return GetDomain() + "/" + key;
        }


        /// <summary>
        /// 上传文件到Oss
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="Key">文件夹</param>
        /// <param name="FileStream">文件流</param>
        /// <param name="fileExt">文件后缀</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static string UploadFileFromStream(string bucketName, string Key, Stream FileStream, string fileExt, 
            bool isExpires = true, bool isGzip = false)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            if (FileStream.Length == 0) throw new Exception("请选择文件");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //读入MemoryStream对象
            string fileExtension = fileExt.ToLower();

            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            if (isExpires) { Metadata.CacheControl = string.Format("max-age={0}", 157680000); }
            else { Metadata.CacheControl = "Private"; }
            //if (isExpires) Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //if (isExpires) Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));
            if (isGzip) Metadata.AddHeader("Content-Encoding", "gzip");

            //上传文件到Oss
            client.PutObject(bucketName, Key, FileStream, Metadata);

            return GetDomain() + "/" + Key;
        }
        /// <summary>
        /// 上传文件到Oss
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="Key">文件夹</param>
        /// <param name="FileByte">文件字节数组</param>
        /// <param name="fileExt">文件后缀</param>
        /// <param name="ossPoint">站点</param>
        /// <param name="accessId">accessId</param>
        /// <param name="accessKey">accessKey</param>
        /// <returns></returns>
        public static string UploadFileFromByte(string bucketName, string Key, byte[] FileByte, string fileExt, 
            bool isExpires = true, bool isGzip = false)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            if (FileByte.Length == 0) throw new Exception("请选择文件");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //读入MemoryStream对象
            string fileExtension = fileExt.ToLower();

            //读入MemoryStream对象
            Stream stream = new MemoryStream(FileByte);

            //设置缓存
            ObjectMetadata Metadata = new ObjectMetadata();
            //Metadata.CacheControl = "Private";
            if (isExpires) { Metadata.CacheControl = string.Format("max-age={0}", 157680000); }
            else { Metadata.CacheControl = "Private"; }
            //if (isExpires) Metadata.AddHeader("Expires", DateTime.MaxValue.GetDateTimeFormats('r')[0]);
            //if (isExpires) Metadata.AddHeader("Expires", 2628000);
            Metadata.AddHeader("Content-Type", FileContentTypeHelper.GetMimeType(fileExtension));
            if (isGzip) Metadata.AddHeader("Content-Encoding", "gzip");
            //上传文件到Oss
            client.PutObject(bucketName, Key, stream, Metadata);

            return GetDomain() + "/" + Key;
        }
        #endregion
        
        #region 生成Post上传时的签名
        public static OssSign BuildSign(string bucketName, string dir, string userId, string fileType, int? maxLength, int hours = 2)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new Exception("Bucket名称为空");

            //建立Oss客户端连接
            OssClient client = ConnOssClient();
            if (!client.DoesBucketExist(bucketName)) throw new Exception("Bucket还没有建立");

            //如果文件夹未设置则设置为临时文件夹
            if (string.IsNullOrWhiteSpace(dir)) dir = "Temp";

            string ossDomain = ConfigHelper.GetConfigString("Oss_Domain");
            string ossPoint = ConfigHelper.GetConfigString("Oss_Point");
            string accessId = ConfigHelper.GetConfigString("Oss_AccessId");
            string accessKey = ConfigHelper.GetConfigString("Oss_AccessKey");

            OssSign ossSign = new OssSign();
            ossSign.OssDomain = ossDomain;
            ossSign.OSSAccessKeyId = accessId;
            ossSign.bucketUrl = ossPoint.Replace("oss-", bucketName + ".oss-");
            string keyPath = dir + "/" + userId + "/" + fileType + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            ossSign.key = keyPath + "${filename}";
            ossSign.guid = Guid.NewGuid().ToString("N").ToUpper();
            policy policyModel = new policy();
            policyModel.expiration = DateTime.Now.AddHours(hours).ToString("yyyy-MM-ddTHH:mm:00.000Z");
            policyModel.conditions = new List<object>();
            policyModel.conditions.Add(new
            {
                bucket = bucketName
            });
            policyModel.conditions.Add(new string[] { "starts-with", "$key", keyPath });
            if (maxLength.HasValue)
            {
                List<object> range = new List<object>();
                range.Add("content-length-range");
                range.Add(1);
                range.Add(maxLength);
                policyModel.conditions.Add(range);
            }
            string jsonPolicy = JSONHelper.ObjectToJson(policyModel);
            ossSign.policy = Base64Change.EncodeBase64ByUTF8(jsonPolicy);
            ossSign.Signature = SHA1.Hmac_sha1(accessKey, ossSign.policy);
            return ossSign;
        }
            
        #endregion
    }
}
