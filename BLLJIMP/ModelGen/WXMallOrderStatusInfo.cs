using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 订单状态信息表
    /// </summary>
   public class WXMallOrderStatusInfo: ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动标识
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 订单状态
       /// </summary>
       public string OrderStatu { get; set; }
       /// <summary>
       /// 订单状态改变后给用户发送的提示信息
       /// </summary>
       public string OrderMessage { get; set; }
       /// <summary>
       /// 排序 (从小到大)
       /// </summary>
       public int Sort{get;set;}
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
       /// <summary>
       /// 状态类型
       /// DistributionOffLine(线下分销)
       /// BranchApply(分公司申请)
       /// BookRoomApply(预约看房)
       /// RecommendedBranch（推荐分公司）
       /// RecommendBuyRoomCustomer（推荐购房顾客）
       /// UploadHouses(上传楼盘)楼盘：新房、二手房
       /// </summary>
        public string StatusType { get; set; }
        /// <summary>
        /// 状态对应的动作
        /// DistributionOffLineCommission(线下分销分佣)
        /// </summary>
        public string StatusAction { get; set; }
    }
}
