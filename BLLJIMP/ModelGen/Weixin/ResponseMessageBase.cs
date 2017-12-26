using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    public interface IResponseMessageBase
    {
        string ToUserName { get; set; }
        string FromUserName { get; set; }
        DateTime CreateTime { get; set; }
        WeixinResponseMsgType MsgType { get; set; }
        bool FuncFlag { get; set; }
    }

    /// <summary>
    /// 响应回复消息
    /// </summary>
    public class ResponseMessageBase : IResponseMessageBase
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public DateTime CreateTime { get; set; }
        public WeixinResponseMsgType MsgType { get; set; }
        public string Content { get; set; }
        public bool FuncFlag { get; set; }

        public static ResponseMessageBase CreateFromRequestMessage(IRequestMessageBase requestMessage, WeixinResponseMsgType msgType)
        {
            ResponseMessageBase responseMessage = null;
            switch (msgType)
            {
                case WeixinResponseMsgType.Text:
                    responseMessage = new ResponseMessageText()
                    {
                        ToUserName = requestMessage.FromUserName,
                        FromUserName = requestMessage.ToUserName,
                        CreateTime = requestMessage.CreateTime,//这个时间不是Ticks，用DateTime.Now会出错
                        MsgType = msgType
                    };
                    break;
                case WeixinResponseMsgType.News:
                    responseMessage = new ResponseMessageNews()
                    {
                        ToUserName = requestMessage.FromUserName,
                        FromUserName = requestMessage.ToUserName,
                        CreateTime = requestMessage.CreateTime,
                        MsgType = msgType
                    };
                    break;
                case WeixinResponseMsgType.Event:
                    responseMessage = new ResponseMessageNews()
                    {
                        ToUserName = requestMessage.FromUserName,
                        FromUserName = requestMessage.ToUserName,
                        CreateTime = requestMessage.CreateTime,
                        MsgType = msgType
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("msgType");
            }

            return responseMessage;
        }

        public static ResponseMessageBase CreateFromRequestMessage(IRequestMessageBase requestMessage, string msgType)
        {
            ResponseMessageBase responseMessage = null;
            switch (msgType)
            {
                case "text":
                    responseMessage = CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.Text);
                    break;
                case "news":
                    responseMessage = CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.News);
                    break;
                case "event":
                    responseMessage = CreateFromRequestMessage(requestMessage, WeixinResponseMsgType.Event);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("msgType");
            }

            return responseMessage;
        }

    }
}
