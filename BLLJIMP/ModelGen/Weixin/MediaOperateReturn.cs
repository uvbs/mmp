using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 微信上传多媒体文件返回结果类
    /// </summary>
   public class MediaOperateReturn
    {
       /// <summary>
       /// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）
       /// </summary>
       public string type { get; set; }
       /// <summary>
       /// 媒体文件ID
       /// </summary>
       public string media_id { get; set; }
        ///<summary>
       /// 创建时间 媒体文件在后台保存时间为3天，即3天后media_id失效。 
       /// </summary>
       public string created_at { get; set; }



    }
}
