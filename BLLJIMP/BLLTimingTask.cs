using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 定时任务BLL
    /// </summary>
    public class BLLTimingTask : BLL
    {
        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="taskInfo"></param>
        /// <param name="receiverType"></param>
        /// <param name="receivers"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool AddTimingTask(TaskType taskType, string taskInfo, ReceiverType receiverType, string receivers, string websiteOwner)
        {
            TimingTask timingTask = new TimingTask();
            timingTask.TaskType = (int)taskType;
            timingTask.ReceiverType = (int)receiverType;
            timingTask.Receivers = receivers;
            timingTask.TaskInfo = taskInfo;
            timingTask.InsertDate = DateTime.Now;
            timingTask.WebsiteOwner = websiteOwner;
            return this.Add(timingTask);
        }
        /// <summary>
        /// 获取定时任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="taskStatus"></param>
        /// <returns></returns>
        public TimingTask GetNextTimingTask(TaskType taskType, TaskStatus taskStatus)
        {
            return this.Get<TimingTask>(string.Format("TaskType={0} and Status={1} order by ScheduleDate ASC", (int)taskType, (int)taskStatus));
        }
        /// <summary>
        /// 获取定时任务
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public TimingTask GetTimingTask(int autoId)
        {
            return Get<TimingTask>(string.Format(" WebsiteOwner='{0}' AND AutoId={1} ",WebsiteOwner,autoId));
        }
        /// <summary>
        /// 获取最后一条定时任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <returns></returns>
        public TimingTask GetLastTinmingTask(TaskType taskType)
        {
            return Get<TimingTask>(string.Format("WebsiteOwner='{0}' AND  TaskType={1} ORDER BY AutoId DESC  ",WebsiteOwner,(int)taskType));
        }

        /// <summary>
        /// 获取定时任务列表
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="taskType">定时任务类型</param>
        /// <param name="status">状态</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public List<TimingTask> GetTimingTaskList(int pageSize, int pageIndex, string keyWord, string taskType, string taskStatus, out int totalCount, string sort)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ",WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND  TaskInfo like '%{0}%' ",keyWord);
            }
            if (!string.IsNullOrEmpty(taskType))
            {
                sbWhere.AppendFormat(" AND  TaskType={0} ", taskType);
            }
            if (!string.IsNullOrEmpty(taskStatus))
            {
                sbWhere.AppendFormat(" AND  Status={0} ", taskStatus);
            }
            string orderBy = " InsertDate DESC ";
            switch (sort)
            {
                case "Status":
                    orderBy = " Status ASC ";
                    break;
                case "TaskType":
                    orderBy = " TaskType ASC ";
                    break;
                case "FinishDate":
                    orderBy = " FinishDate ASC ";
                    break;
                default:
                    break;
            }
            totalCount = GetCount<TimingTask>(sbWhere.ToString());
            return GetLit<TimingTask>(pageSize, pageIndex, sbWhere.ToString(), orderBy);


        }

        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool ExecuteTimingTask(TimingTask task)
        {
            switch ((TaskType)task.TaskType)
            {
                case TaskType.SendImageTextMessage:
                    return SendImageTextMessage(task);//客服接口 群发
                //case TaskType.SendTMUserScoreNotify:
                //    return BLLUserScore.SendTMAllUserScoreDailyAccountBillNotify(task.WebsiteOwner);
                case TaskType.SendMassMessage:
                    return new BLLWeixin("").SendMassMessage(task);
                case TaskType.SynDistributionMember:
                    return new BLLDistribution().SynDistributionCount(task.WebsiteOwner);//更新上下级人数
                case TaskType.SynDistributionSaleAmount:
                    return new BLLDistribution().SynDistributionSaleAmount(task.WebsiteOwner);//更新销售额
                case TaskType.FlashChannelData:
                    return new BLLDistribution().FlashChannelData(task.WebsiteOwner);//更新渠道数据
                case TaskType.CleanUser:
                    return new BLLUser().CleanUser(task.WebsiteOwner)>0;
                case TaskType.GetDistributionImage://分销二维码
                    return new BLLDistribution().GetDistributionImage(task);
                case TaskType.StatisticsOrderProduct://统计订单及商品
                    return new BLLMall().StatisticsOrderAndProduct(task);
                case TaskType.SupplierSettlement://供应商结算
                    return new BLLMall().SupplierSettlementTask(task);
                case TaskType.SupplierChannelSettlement://供应商渠道结算
                    return new BLLMall().SupplierChannelSettlementTask(task);
                case TaskType.ScoreStatis://积分统计任务
                    return new BllScore().ExceScoreStatis(task);
                default:
                    return false;
            }
        }
        /// <summary>
        ///客服接口 群发
        /// </summary>
        /// <param name="timingTask"></param>
        /// <returns></returns>
        public bool SendImageTextMessage(TimingTask timingTask)
        {
            BLLWeixin bllWeixin = new BLLWeixin("");
            List<BLLWeixin.WeiXinArticle> articleList = new List<BLLWeixin.WeiXinArticle>();
            string[] idarray = timingTask.TaskInfo.Split(',');
            foreach (string id in idarray)
            {
                WeixinMsgSourceInfo msg = bllWeixin.Get<WeixinMsgSourceInfo>(string.Format("SourceID={0}", id));
                articleList.Add(new BLLWeixin.WeiXinArticle()
                {
                    Title = msg.Title,
                    Description = msg.Description,
                    Url = msg.Url,
                    PicUrl = msg.PicUrl
                });
            }

            switch ((ReceiverType)timingTask.ReceiverType)
            {
                case ReceiverType.All:
                    bllWeixin.BroadcastKeFuMessageImageText(timingTask.WebsiteOwner, articleList, timingTask.TaskInfo);
                    return true;

                case ReceiverType.Group:

                    return false;

                case ReceiverType.List:
                    string[] openidArray = timingTask.Receivers.Split(',');
                    foreach(string openid in openidArray)
                    {
                        bllWeixin.SendKeFuMessageImageText(bllWeixin.GetAccessToken(timingTask.WebsiteOwner), openid, articleList);
                    }
                    return openidArray.Length > 0;
                default:
                    return false;
            }
        }

        
        /// <summary>
        /// 任务类型
        /// </summary>
        public enum TaskType
        {
            /// <summary>
            /// 客服接口发消息
            /// </summary>
            SendImageTextMessage = 1, 
            /// <summary>
            /// 群发接口发消息
            /// </summary>
            SendMassMessage=2,
            /// <summary>
            /// 更新微信粉丝
            /// </summary>
            SynFans=3,
            /// <summary>
            /// 群发模板消息[批量]
            /// </summary>
            SendTemplateMessage = 4,
            /// <summary>
            /// 同步分销
            /// </summary>
            SynDistributionMember=5,
            /// <summary>
            /// 同步分销销售额
            /// </summary>
            SynDistributionSaleAmount = 7,
            /// <summary>
            /// 会员清洗
            /// </summary>
            CleanUser = 8,
            /// <summary>
            /// 获取分销二维码图片
            /// </summary>
            GetDistributionImage = 9,
            /// <summary>
            /// 群发模板消息[全部]
            /// </summary>
            SendTemplateMessageAll = 10,
            /// <summary>
            /// 同步渠道数据
            /// </summary>
            FlashChannelData = 11,
            /// <summary>
            /// 统计订单及商品
            /// </summary>
            StatisticsOrderProduct = 12,
            /// <summary>
            /// 供应商结算
            /// </summary>
            SupplierSettlement = 13,
            /// <summary>
            /// 供应商渠道结算
            /// </summary>
            SupplierChannelSettlement = 14,
            /// <summary>
            /// 积分统计
            /// </summary>
            ScoreStatis = 15,
            /// <summary>
            /// 群发模板消息通知
            /// </summary>
            SendTMUserScoreNotify = 501


        }

        /// <summary>
        /// 接收类型
        /// </summary>
        public enum ReceiverType
        {
            /// <summary>
            /// //对该站点所有用户执行任务
            /// </summary>
            All = 1,    
            /// <summary>
            /// //对该站点用户群组执行任务
            /// </summary>
            Group = 2,  
            /// <summary>
            /// //对用户列表执行任务
            /// </summary>
            List = 3    
        }
        /// <summary>
        /// 任务状态
        /// </summary>
        public enum TaskStatus
        {
            /// <summary>
            ///等待
            /// </summary>
            Waiting = 1,    
            /// <summary>
            /// 执行中
            /// </summary>
            Executing = 2, 
            /// <summary>
            /// 结束
            /// </summary>
            Finished = 3    //结束
        }
    }
}
