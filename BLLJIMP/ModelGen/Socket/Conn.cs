using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Socket
{
    [Serializable]
    public class Conn
    {
        /// <summary>
        /// Socket sessionid
        /// </summary>
        public string SessionID { get; set; }
        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime conntime { get; set; }
        /// <summary>
        /// 在线状态
        /// </summary>
        public bool online { get; set; }
    }
}
