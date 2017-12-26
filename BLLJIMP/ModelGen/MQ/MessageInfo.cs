using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.MQ
{
    /// <summary>
    /// 消息对象实体
    /// </summary>
    [Serializable]
    public class MessageInfo
    {
        /// <summary>
        /// 消息历史id：非rabbit里面的id
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 消息,根据不同类型消息不同处理，可能是json
        /// </summary>
        public string Msg { get; set; }
        
        public string WebsiteOwner { get; set; }
        
    }
}
