using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliOss
{
    /// <summary>
    /// 签名
    /// </summary>
    public class OssSign
    {
        public string bucketUrl { get; set; }
        public string OSSAccessKeyId { get; set; }
        public string policy { get; set; }
        public string Signature { get; set; }
        public string key { get; set; }
        public string guid { get; set; }
        public string OssDomain { get; set; }

    }
}
