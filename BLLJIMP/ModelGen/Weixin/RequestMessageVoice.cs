using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model.Weixin;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 接收语音类
    /// </summary>
   public class RequestMessageVoice : RequestMessageBase, IRequestMessageBase
    {
       /// <summary>
       /// 语音识别内容
       /// </summary>
       public string Recognition { get; set; }
       /// <summary>
       /// 媒体id，可以调用多媒体文件下载接口拉取该媒体
       /// </summary>
       public string MediaId { get; set; }
       /// <summary>
       /// 语音格式：amr
       /// </summary>
       public string Format { get; set; }

    }
}
