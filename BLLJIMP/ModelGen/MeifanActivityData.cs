using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 美帆活动 (活动,比赛,培训共用)报名数据
    /// </summary>
    public class MeifanActivityData : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否已经支付
        /// </summary>
        public int IsPay { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDay { get; set; }
        /// <summary>
        /// 时间范围
        /// </summary>
        public string DateRange { get; set; }
        /// <summary>
        /// 组别
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// 是否会员
        /// </summary>
        public string IsMember { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 点评 比赛结果
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        ///报名日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InsertDateStr { get {
            return InsertDate.ToString();
        
        } }
        /// <summary>
        /// 用户备注
        /// </summary>
        public string UserRemark { get; set; }
        /// <summary>
        /// 类型
        /// activity 活动
        /// train 培训
        /// match 竞赛
        /// </summary>
        public string ActivityType { get; set; }
    }
}
