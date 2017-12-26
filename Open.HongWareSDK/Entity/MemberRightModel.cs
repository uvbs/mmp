using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.HongWareSDK.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberRightModel:RespBase
    {
        public List<OrderModel> orders { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class OrderModel{
        /// <summary>
        /// 
        /// </summary>
        public string shopCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string saleDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<OrderItem> orderItems { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class OrderItem {
        /// <summary>
        /// 条形码
        /// </summary>
        public string sku { get; set; }
        /// <summary>
        /// 正常价格
        /// </summary>
        public string originPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// 享受商品折率
        /// </summary>
        public string pro_dis { get; set; }
        /// <summary>
        /// 享受商品折率实售单价
        /// </summary>
        public string pro_RealPrice { get; set; }
        /// <summary>
        /// 享受商品折率实售金额
        /// </summary>
        public string pro_saleAmt { get; set; }
        /// <summary>
        /// 享受会员折率
        /// </summary>
        public string mem_dis { get; set; }
        /// <summary>
        /// 享受会员折率实售单价
        /// </summary>
        public string mem_realPrice { get; set; }
        /// <summary>
        /// 享受会员折率实售金额
        /// </summary>
        public string mem_saleAmt { get; set; }
        /// <summary>
        /// 享受生日折率 
        /// </summary>
        public string birth_dis { get; set; }
        /// <summary>
        /// 享受生日折率实售单价
        /// </summary>
        public string birth_realPrice { get; set; }
        /// <summary>
        /// 享受生日折率实售金额
        /// </summary>
        public string birth_saleAmt { get; set; }


    
    }
}
