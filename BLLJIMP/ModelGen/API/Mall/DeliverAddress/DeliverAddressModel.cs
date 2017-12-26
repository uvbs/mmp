using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall.DeliverAddress
{
   public class DeliverAddressModel
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int id { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string consigneename { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 是否默认收货地址 1表示默认
        /// </summary>
        public int isdefault { get; set; }

    }
}
