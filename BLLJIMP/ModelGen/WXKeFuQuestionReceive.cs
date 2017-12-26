using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信客服问题接收
    /// </summary>
    public class WXKeFuQuestionReceive : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int? QuestionID { get; set; }
        /// <summary>
        /// 问题发起人OpenId
        /// </summary>
        public string FromWeixinOpenId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string WXNickName { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        public string QuestionContent { get; set; }
        /// <summary>
        /// 状态
        /// 0未处理
        /// 1已处理
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 消息类型
        /// NULL为文本消息
        /// voice 声音
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
