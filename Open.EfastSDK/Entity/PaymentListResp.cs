using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    public class PaymentListResp:RespDataBase
    {
        public List<Entity.PaymentInfo> list { get; set; }
    }
}
