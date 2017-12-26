using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.Mall
{
    /// <summary>
    /// 运费模板ID跟sku数量对应模型 用于计算运费
    /// </summary>
   public class FreightTemplateSkuMapping
    {
       /// <summary>
       /// 模板ID
       /// </summary>
       public int TemplateId { get; set; }
       /// <summary>
       /// 购买数量 
       /// </summary>
       public int SkuCount { get; set; }
       /// <summary>
       /// 单件商品重量
       /// </summary>
       public decimal Weight { get; set; }

       /// <summary>
       /// 价格
       /// </summary>
       public decimal Price { get; set; }
      
       


    }
}
