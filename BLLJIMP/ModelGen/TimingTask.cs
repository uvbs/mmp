using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public class TimingTask : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int? AutoId { get; set; }
        /// <summary>
        /// 任务类型
        /// 1 微信定时客服接口群发图文
        /// 2 微信模板消息通知积分账户变化
        /// 3 同步微信粉丝信息
        /// 4 群发模板消息(批量)
        /// 5 同步分销下级人数
        /// 6 同步微信素材
        /// 7 同步分销销售额
        /// 8 清洗会员数据
        /// 9 获取我的分销二维码
        /// 10 群发模板消息(全部)
        /// 11 同步渠道数据
        /// 12 商城统计(指定时间段内查询渠道,分销员订单及指定时间段内商品统计)
        /// 13 商城供应商结算任务
        /// 14 供应商渠道结算任务
        /// 15 积分统计任务
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 任务信息
        /// </summary>
        public string TaskInfo { get; set; }
        /// <summary>
        /// 接收类型
        /// </summary>
        public int ReceiverType { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Receivers { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? InsertDate { get; set; }
        /// <summary>
        /// 任务日期
        /// </summary>
        public DateTime? ScheduleDate{ get; set; }
        /// <summary>
        /// 完成日期
        /// </summary>
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// 1 等待
        /// 2 执行
        /// 3 结束
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 任务类型描述
        /// 1 微信定时客服接口群发图文
        /// 2 微信模板消息通知积分账户变化
        /// 3 同步微信粉丝信息
        /// 4 群发模板消息(批量)
        /// 5 同步分销下级人数
        /// 6 同步微信素材
        /// 7 同步分销销售额
        /// 8 清洗会员数据
        /// 9 获取我的分销二维码
        /// 10群发模板消息(全部)
        /// </summary>
        public string TaskTypeString
        {
            get
            {
                switch (TaskType)
                {
                    case 1:
                        return "微信定时客服接口群发图文";
                    case 2:
                        return "微信模板消息通知积分账户变化";
                    case 3:
                        return "同步微信粉丝信息";
                    case 4:
                        return "群发模板消息[批量]";
                    case 5:
                        return "同步分销下级人数";
                    case 6:
                        return "同步微信素材";
                    case 7:
                        return "同步分销销售额";
                    case 8:
                        return "会员清洗";
                    case 9:
                        return "获取我的分销二维码";
                    case 10:
                        return "群发模板消息[全部]";
                    default:
                        return "未定义";
                }
            }
        }
        /// <summary>
        /// 接收类型描述
        /// </summary>
        public string ReceiverTypeString
        {
            get
            {
                switch (ReceiverType)
                {
                    case 1:
                        return "所有人";
                    case 2:
                        return "群组列表";
                    default:
                        return "个人列表";
                }
            }
        }
        /// <summary>
        /// 状态类型描述
        /// </summary>
        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case -1:
                        return "已取消";
                    case 1:
                        return "待处理";
                    case 2:
                        return "进行中";
                    case 3:
                        return "已结束";
                    default:
                        return "未定义";
                }
            }
        }
        /// <summary>
        /// 任务类型描述
        /// </summary>
        public string TaskInfoString
        {
            get
            {
                switch (TaskType)
                {
                    case 1:
                        return "图文ID：" + TaskInfo;
                    case 2:
                        return "";
                    default:
                        return "未定义";
                }
            }
        }
        /// <summary>
        /// 插入日期
        /// </summary>
        public string InsertDateString { get { return InsertDate.ToString(); } }
        /// <summary>
        /// 完成日期
        /// </summary>
        public string FinishDateString { get { return FinishDate.ToString(); } }
        /// <summary>
        /// 完成日期
        /// </summary>
        public string ScheduleDateString { get { return ScheduleDate.ToString(); } }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string MsgContent { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// 渠道账户名
        /// </summary>
        public string ChannelUserId { get; set; }
        /// <summary>
        /// 分销员账户名
        /// </summary>
        public string DistributionUserId { get; set; }

    }
}
