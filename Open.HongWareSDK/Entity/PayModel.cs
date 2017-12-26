using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 支付信息
    /// </summary>
    [Serializable]
    public class PayModel : RespBase
    {
        /// <summary>
        /// 信息
        /// </summary>
        public DataModel data { get; set; }

    }

    public class DataModel {

        /// <summary>
        /// 支付链接
        /// </summary>
        public string url_all { get; set; }
    
    }
}
