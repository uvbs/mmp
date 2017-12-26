using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    public class GoodsListResp:RespDataBase
    {
        public List<GoodsInfo> list { get; set; }
    }
}
