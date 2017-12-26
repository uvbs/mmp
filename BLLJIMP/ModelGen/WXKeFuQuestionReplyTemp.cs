using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 客服回复多媒体信息(语音，图片，视频) 临时表
    /// </summary>
    public class WXKeFuQuestionReplyTemp : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int? AutoID { get; set; }
        /// <summary>
        /// 问题id 外键WXKeFuQuestionReceive AutoID
        /// </summary>
        public int? QuestionId { get; set; }
        /// <summary>
        /// 回复人OpenId
        /// </summary>
        public string ReplyWeixinOpenId { get; set; }
        /// <summary>
        /// 状态0待处理1已处理
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 媒体文件 ID
        /// </summary>
        public string MediaId { get; set; }

    }
}
