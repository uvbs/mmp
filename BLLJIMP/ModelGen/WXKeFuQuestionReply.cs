using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信客服回复问题
    /// </summary>
    public class WXKeFuQuestionReply : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动编号
        /// </summary>
        public int? ReplyID { get; set; }
        /// <summary>
        /// 问题id 外键WXKeFuQuestionReceive AutoID
        /// </summary>
        public int? QuestionId { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }
        /// <summary>
        /// 回复人OpenId
        /// </summary>
        public string ReplyWeixinOpenId { get; set; }
        /// <summary>
        /// 状态
        /// 0待处理
        /// 1处理中
        /// 2已处理
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 消息类型
        /// NULL  文字
        /// voice 语音
        /// image 图片
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 媒体文件 ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
