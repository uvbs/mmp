using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    public class SkuListResp:RespDataBase
    {
        public List<SkuInfo> list { get; set; }
    }
}
