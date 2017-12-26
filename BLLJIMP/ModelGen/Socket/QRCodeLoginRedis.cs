using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Socket
{
    [Serializable]
    public class QRCodeLoginRedis
    {
        public string sessionID { get; set; }
        public string userID { get; set; }
    }
}
