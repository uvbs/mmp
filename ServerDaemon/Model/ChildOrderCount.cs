using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerDaemon.Model
{
    public class ChildOrderCountModel : ZentCloud.ZCBLLEngine.ModelTable
    {
        public string OrderId { get; set; }

        public int ChildOrderCount { get; set; }

    }
}
