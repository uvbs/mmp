using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class EmailEventParams
    {
        public string EmailID{get;set;}
        public int EventType { get; set; }
        public string EventEmail { get; set; }
        public string LinkID { get; set; }
    }
}
