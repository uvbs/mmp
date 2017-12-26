using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
   /// <summary>
   /// 图片映射
   /// </summary>
   public class JuActivityImageMapping : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 远程图片地址
       /// </summary>
       public string RemoteUrl { get; set; }

       /// <summary>
       /// 本地图片地址 格式如 /FileUpLoad/ImageMapping/9bea31d1-0438-4dd6-9b49-6c88ce3772f0.jpg
       /// </summary>
       public string LocalPath { get; set; }
       /// <summary>
       /// 是否保存在OSS
       /// 0本地
       /// 1阿里Oss
       /// </summary>
       public int InOss { get; set; }

    }
}
