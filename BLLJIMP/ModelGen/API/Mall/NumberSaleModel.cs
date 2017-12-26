using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.ModelGen.API.Mall
{
    /// <summary>
    /// 件数特卖 
    /// </summary>
   public  class NumberSaleModel
    {
       /// <summary>
       /// 件数
       /// </summary>
       public int count { get; set; }
       /// <summary>
       /// 价格
       /// </summary>
       public string price { get; set; }
    }
}
