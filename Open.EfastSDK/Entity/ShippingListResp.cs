using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    public class ShippingListResp:RespDataBase
    {
        public List<ShippingInfo> list { get; set; }
    }
}
