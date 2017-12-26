using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
    public class SocketLog:ModelTable
    {
        public int AutoID { get; set; }
        public int UserAutoID { get; set; }
        public string UserID { get; set; }
        public string UserNickname { get; set; }
        public string UserAvatar { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Minutes { get; set; }
        public string CloseReason { get; set; }
        public long StartTimestamp { get; set; }
        
    }
}
