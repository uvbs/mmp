using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{   
    /// <summary>
    /// 卡券响应
    /// </summary>
    public class RespCard : RespBase
    {
        /// <summary>
        ///json数据
        /// </summary>
        public JToken data { get; set; }

    }
}
