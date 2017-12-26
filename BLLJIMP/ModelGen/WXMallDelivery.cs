using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 商城配送方式
    /// </summary>
   public class WXMallDelivery : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 配送方式编号
       /// </summary>
       public int AutoId { get; set; }
       /// <summary>
       /// 排序 从小到大排序
       /// </summary>
       public int Sort { get; set; }
       /// <summary>
       /// 配送类型 0代表快递 1代表上门自取(配送费用为0) 2代表卖家承担(配送费用为0)
       /// </summary>
       public int? DeliveryType { get; set; }
       /// <summary>
       /// 配送方式名称
       /// </summary>
       public string DeliveryName { get; set; }
       /// <summary>
       /// 初始数量
       /// </summary>
       public int InitialProductCount { get; set; }
       /// <summary>
       /// 初始价格
       /// </summary>
       public decimal InitialDeliveryMoney { get; set; }
       /// <summary>
       /// 增量数量
       /// </summary>
       public int AddProductCount { get; set; }
       /// <summary>
       /// 增量费用
       /// </summary>
       public decimal AddMoney { get; set; }
       /// <summary>
       /// 入库时间
       /// </summary>
       public DateTime InsertDate { get; set; }
       /// <summary>
       /// 网站所有者
       /// </summary>
       public string WebsiteOwner { get; set; }


    }
}
