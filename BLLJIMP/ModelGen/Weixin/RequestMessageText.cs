using System;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    public class RequestMessageText : RequestMessageBase, IRequestMessageBase
    {
        public string Content { get; set; }
    }
}
