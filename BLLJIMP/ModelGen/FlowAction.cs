using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_FlowAction
    /// </summary>
    [Serializable]
    public partial class FlowAction : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public int FlowID { get; set; }
        /// <summary>
        /// FlowKey
        /// </summary>
        public string FlowKey { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 当前环节ID
        /// </summary>
        public int StepID { get; set; }
        /// <summary>
        /// 当前环节名称
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 起始环节
        /// </summary>
        public int StartStepID { get; set; }
        /// <summary>
        /// 会员AutoID
        /// </summary>
        public int MemberAutoID { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public string MemberID { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 会员头像
        /// </summary>
        public string MemberAvatar { get; set; }
        /// <summary>
        /// 会员手机
        /// </summary>
        public string MemberPhone { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public int MemberLevel { get; set; }
        /// <summary>
        /// 会员等级名称
        /// </summary>
        public string MemberLevelName { get; set; }
        /// <summary>
        /// ex1
        /// 线下充值：充值渠道
        /// 线下注册：充值渠道
        /// 提现：开户银行
        /// </summary>
        public string StartEx1 { get; set; }
        /// <summary>
        /// ex2
        /// 线下注册：注册信息
        /// 提现：开户名
        /// </summary>
        public string StartEx2 { get; set; }
        /// <summary>
        /// ex3
        /// 提现：银行卡号
        /// </summary>
        public string StartEx3 { get; set; }
        /// <summary>
        /// 提交内容
        /// </summary>
        public string StartContent { get; set; }
        /// <summary>
        /// 提交时选择时间，不同流程用处不同，表达意思不同，如4.1号提交的5.1的请假
        /// </summary>
        public DateTime StartSelectDate { get; set; }
        /// <summary>
        /// 状态
        /// 0处理中  8已结束（审核未通过） 9已结束（审核通过） 10已取消 11申请取消 12拒绝取消
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime CancelDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 金额
        /// （管理奖）
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 消耗金额 
        /// 提现扣税金额
        /// 撤单消耗金额
        /// 公积金（管理奖）
        /// </summary>
        public decimal DeductAmount { get; set; }
        /// <summary>
        /// 金额
        /// 提现实际到账金额
        /// 撤单实际退款金额
        /// 开票金额（管理奖）
        /// </summary>
        public decimal TrueAmount { get; set; }
        /// <summary>
        /// 导数据用
        /// </summary>
        public long FromId { get; set; }
        /// <summary>
        /// 关联ID （管理奖确认）
        /// </summary>
        public int RelationId { get; set; }
    }
}