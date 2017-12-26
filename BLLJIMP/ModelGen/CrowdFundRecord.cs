using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 众筹付款记录表 订单表
    /// </summary>
    public class CrowdFundRecord : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 记录ID 订单号
        /// </summary>
        public int RecordID { get; set; }
        /// <summary>
        ///众筹选项ID 关联 ZCJ_CrowdFundItem 表ItemId
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// 众筹项目ID 关联ZCJ_CrowdFundInfo 表AutoID
        /// </summary>
        public int CrowdFundID { get; set; }
        /// <summary>
        /// 付款用户名
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 付款状态 0未付款 1已经付款
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 一句话评论
        /// </summary>
        public string Review { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 订单状态 
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiveAddress { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }
        /// <summary>
        /// 买家留言
        /// </summary>
        public string BuyerMemo { get; set; }

        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string ExpressCompanyCode { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string ExpressCompanyName { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }


    }
}
