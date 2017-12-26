using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Socket
{
    [Serializable]
    public class SendToClient
    {
        public string action { get; set; }
        public string message { get; set; }
    }
}
