using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.EfastSDK.Entity
{
    public class SizeListResp : RespDataBase
    {
        public List<SizeInfo> list { get; set; }
    }
}
