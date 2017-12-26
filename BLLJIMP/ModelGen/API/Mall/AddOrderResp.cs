using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 下单响应模型
    /// </summary>
    public class AddOrderResp : BaseResponse
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
    }
}
