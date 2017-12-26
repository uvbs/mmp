using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 商城-积分记录
    /// </summary>
    public class WXMallScoreRecord : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 积分收入支出 大于0 为收入 小于0 为支出
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 积分类型 1微商城购物 2积分购物 3积分赠送
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 时间字符串
        /// </summary>
        public string InsertDateStr { get {
            return InsertDate.ToString("yyyy-MM-dd");
        
        } }



    }
}
