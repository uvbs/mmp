using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 消息队列执行历史表：
    /// 
    /// 发布和消费拆分成两记录，可对更多异常情况追踪，表无需拆分两个
    ///     -可能会有多次消费处理的情况
    ///     -可能会有多次发布的情况
    /// 
    /// </summary>
    [Serializable]
    public class MQHistoryInfo : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// Guid，非RabbitMQ里面的id
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 消息处理类型：生产/消费  produce/consume
        /// </summary>
        public string MsgActionType { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 客户端id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 订阅队列id，处理消息才会有这个id
        /// </summary>
        public string QueueId { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionInfo { get; set; }

    }
}
