using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall.DeliverAddress
{
   public class DeliverAddressList
    {
       /// <summary>
       /// 总数量
       /// </summary>
       public int totalcount { get; set; }
       /// <summary>
       /// 收货地址列表
       /// </summary>
       public List<DeliverAddressModel> list { get; set; }

    }
}
