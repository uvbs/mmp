using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 服务每天价格配件详情
    /// </summary>
    public class CarServerDayPartsDetail
    {
        public int PartsId { get; set; }

        public string PartsName { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }

        public double TotalPrice
        {
            get
            {
                return Count * Price;
            }
        }
    }
}
