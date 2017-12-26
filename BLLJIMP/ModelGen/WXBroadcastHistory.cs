using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// WXBroadcastHistory:实体类(属性说明自动提取数据库字段的描述信息)
    /// 记录微信群发历史记录
    /// </summary>
    [Serializable]
    public partial class WXBroadcastHistory : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// AutoId
        /// </summary>
        public int? AutoId { get; set; }

        /// <summary>
        /// 群发批次
        /// </summary>
        public string SerialNum { get; set; }


        /// <summary>
        /// 接收人openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 发送返回状态消息
        /// </summary>
        public string StatusMsg { get; set; }

        /// <summary>
        /// 网站owner
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? InsertDate { get; set; }

        
        /// <summary>
        /// 微信广播消息类型，由 BroadcastType 枚举定义类型字符串
        /// </summary>
        public string BroadcastType { get; set; }

        public string Title { get; set; }

        public string Msg { get; set; }

        public string Url { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// 1发送成功，0发送失败 -1待开始 2仅发送成功App 3仅发送成功微信
        /// </summary>
        public int Status { get; set; }

        #region ModelEx
        /// <summary>
        /// 经过翻译的发送状态
        /// </summary>
        public string TranslateStatus
        {
            get
            {
                if (StatusMsg.Contains("\"errcode\":0"))
                {
                    return "发送成功";
                }
                else if (StatusMsg.Contains("\"errcode\":45015"))
                {
                    return "失败：超出48小时限制";
                }
                //else if (StatusMsg.Contains("\"errcode\":45015"))
                //{
                //    return "失败：鉴权错误";
                //}
                else
                {
                    return "失败：错误";
                }
            }
        }


        public string InsertDateStr
        {
            get
            {
                return InsertDate == null ? "" : InsertDate.Value.ToString();
            }
        }

        public string TrueName { get; set; }

        public string WxNikeName { get; set; }

        public string Phone { get; set; }

        #endregion
    }
}
