using System;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    public class RequestMessageImage : RequestMessageBase, IRequestMessageBase
    {
        /// <summary>
        ///图片url
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 媒体id，可以调用多媒体文件下载接口拉取该媒体
        /// </summary>
        public string MediaId { get; set; }


    }
}
