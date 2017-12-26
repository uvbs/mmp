using System;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 接收请求接口
    /// </summary>
    public interface IRequestMessageBase
    {
        /// <summary>
        /// 接收方
        /// </summary>
        string ToUserName { get; set; }
        /// <summary>
        /// 发送方
        /// </summary>
        string FromUserName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        WeixinRequestMsgType MsgType { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        long MsgId { get; set; }
    }

    /// <summary>
    /// 接收到请求的消息
    /// </summary>
    public class RequestMessageBase : IRequestMessageBase
    {
        /// <summary>
        /// 接收方
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public WeixinRequestMsgType MsgType { get; set; }
        /// <summary>
        /// 消息编号
        /// </summary>
        public long MsgId { get; set; }
    }
}
