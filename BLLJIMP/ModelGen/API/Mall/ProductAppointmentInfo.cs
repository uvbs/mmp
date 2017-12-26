using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 预购商品信息
    /// </summary>
   public class ProductAppointmentInfo
    {
       /// <summary>
       /// 状态
       /// 0 未开始
       /// 1 已开始
       /// 2 已经结束
       /// </summary>
       public int status { get; set; }
       /// <summary>
       /// 预购开始时间
       /// </summary>
       public string appointment_start_time { get; set; }
       /// <summary>
       /// 预购结束时间
       /// </summary>
       public string appointment_end_time { get; set; }
       /// <summary>
       /// 预购发货时间
       /// </summary>
       public string appointment_delivery_time { get; set; }
    }
}
