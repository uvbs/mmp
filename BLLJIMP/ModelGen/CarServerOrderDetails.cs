using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class CarServerOrderDetails:ZCBLLEngine.ModelTable
    {
        public int AutoId { get; set; }
        public int OrderId { get; set; }
        public int ServerId { get; set; }
        public int PartsId { get; set; }
        public int Count { get; set; }
        public double SingelPrice { get; set; }
    }
}
