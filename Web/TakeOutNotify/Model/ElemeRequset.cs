using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.TakeOutNotify.Model
{
    /// <summary>
    /// 饿了么请求实体类
    /// </summary>
    public class ElemeRequset
    {
        /// <summary>
        /// 应用id，应用创建时系统分配的唯一id
        /// </summary>
        public string requestId { get; set; }
        /// <summary>
        /// 消息的唯一id，用于唯一标记每个消息
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 消息类型，参加下方【消息类型】
        /// </summary>
        public long appId { get; set; }
        /// <summary>
        /// JSON格式字符串，每种类型消息的结构体内容不一样，具体以对应类型定义的消息体为准。
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 商户的店铺id
        /// </summary>
        public long shopId { get; set; }
        /// <summary>
        /// 消息发送的时间戳，每次推送时生成，单位毫秒
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        /// 授权商户的账号id，注意这里的userId跟订单结构体中的userId(下单用户的id)不一样
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 消息签名，32位全大写
        /// </summary>
        public long userId { get; set; }
    }
}