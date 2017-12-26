using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.TakeOutNotify.Model
{
    public class RespApplyCencel
    {
        public string orderId { get; set; }
        public string refundStatus { get; set; }

        public string reason { get; set; }
        public long shopId { get; set; }
        public long updateTime { get; set; }
    }
}