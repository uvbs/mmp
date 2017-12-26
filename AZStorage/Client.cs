using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AZStorage
{
    /// <summary>
    /// AZ上传客户端
    /// </summary>
    public class Client
    {

        // CloudStorageAccount 类表示一个 Azure Storage Account，我们需要先创建它的实例，才能访问属于它的资源。
        // 注意连接字符串中的 xxx 和 yyy，分别对应 Access keys 中的 Storage account name 和 key。
        CloudStorageAccount storageAccount;

        // CloudBlobClient 类是 Windows Azure Blob Service 客户端的逻辑表示，我们需要使用它来配置和执行对 Blob Storage 的操作。
        CloudBlobClient blobClient;

        // CloudBlobContainer 表示一个 Blob Container 对象。
        CloudBlobContainer container;

        /// <summary>
        /// 当前账户名
        /// </summary>
        string accountName = string.Empty;

        /// <summary>
        /// 当前内容空间名
        /// </summary>
        string containerName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        /// <param name="containerName">
        /// Container 名称规则
        /// MSDN 上不厌其烦的描述 Blob Container 的名称规则，足以说明其重要性，本文试图以简要的文字进行描述：
        ///1. 以小写字母或数字开头，只能包含字母、数字和 dash(-)。
        ///2. 不能有连续的 dash(-)，dash(-) 不能是第一个字符，也不能是最后一个字符。
        ///3. 所有字符小写，总长度为 3-63 字符。
        ///违反任何一个规则，在创建 Blob Container 时都会受到(400) Bad Request 错误。
        /// </param>
        public Client(string accountName, string accountKey, string containerName)
        {
            storageAccount = CloudStorageAccount.Parse(string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};EndpointSuffix=core.chinacloudapi.cn",
                    accountName,
                    accountKey
                ));

            this.accountName = accountName;

            this.containerName = containerName;

            blobClient = storageAccount.CreateCloudBlobClient();

            container = blobClient.GetContainerReference(containerName);

            // 如果不存在就创建名为 picturecontainer 的 Blob Container。
            container.CreateIfNotExists();
        }

        public string upload(string filePath)
        {
            string result = string.Empty;//返回的文件路径

            var fileNewName = Guid.NewGuid().ToString() + Path.GetExtension(filePath);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileNewName);
            using (var fileStream = File.OpenRead(filePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            //URL 格式： Blob 可使用以下 URL 格式寻址：http://<storage account>.blob.core.chinacloudapi.cn/<container>/<blob>
            result = string.Format("http://{0}.blob.core.chinacloudapi.cn/{1}/{2}",
                   accountName,
                   containerName,
                   fileNewName
               );

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileExtension">扩展名记得带上点 . </param>
        /// <returns></returns>
        public string upload(Stream fileStream, string fileExtension)
        {
            string result = string.Empty;//返回的文件路径

            var fileNewName = Guid.NewGuid().ToString() + fileExtension;

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileNewName);
            blockBlob.UploadFromStream(fileStream);

            //URL 格式： Blob 可使用以下 URL 格式寻址：http://<storage account>.blob.core.chinacloudapi.cn/<container>/<blob>
            result = string.Format("http://{0}.blob.core.chinacloudapi.cn/{1}/{2}",
                    accountName,
                    containerName,
                    fileNewName
                );

            return result;
        }

        public string uploadFromBase64(string base64Str, string fileExtension)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Str);
            //读入MemoryStream对象
            Stream stream = new MemoryStream(imageBytes);

            return upload(stream, fileExtension);
        }


    }
}
