using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLTransfersAudit : BLL
    {
        /// <summary>
        /// 添加交易待打款记录
        /// </summary>
        /// <param name="type">类型
        /// MallRefund 商城退款
        /// DistributionWithdraw 分销提现
        /// </param>
        /// <param name="tranId">交易Id</param>
        /// <param name="tarnInfo">交易备注信息</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        public bool Add(string type, string tranId, string tranInfo, decimal amount)
        {

            if (GetCount<TransfersAudit>(string.Format("WebsiteOwner='{0}' And Type='{1}' And TranId='{2}'", WebsiteOwner, type, tranId)) > 0)
            {
                return true;
            }
            TransfersAudit model = new TransfersAudit();
            model.Type = type;
            model.TranId = tranId;
            model.TranInfo = tranInfo;
            model.Amount = amount;
            model.InsertDate = DateTime.Now;
            model.Status = 0;
            model.WebsiteOwner = WebsiteOwner;
            return Add(model);

        }
        /// <summary>
        /// 审核通过打款
        /// </summary>
        /// <param name="autoId">记录Id</param>
        /// <param name="userId">操作人账号</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public bool Pass(int autoId, string userId, out string msg)
        {
            msg = "";
            var record = Get(autoId);
            if (record.Status == 1)
            {
                msg = "该记录已打款";
                return false;
            }

            switch (record.Type)
            {
                #region 分销提现
                case "DistributionWithdraw"://分销提现
                    BLLDistribution bllDis = new BLLDistribution();
                    List<WithdrawCash> list = new List<WithdrawCash>();
                    WithdrawCash model = Get<WithdrawCash>(string.Format("TranId='{0}'", record.TranId));
                    if (model == null)
                    {
                        msg = "该记录已标记为失败";
                        return false;
                    }
                    list.Add(model);
                    if (!bllDis.UpdateWithrawCashStatus(list, 2, out msg))
                    {
                        return false;
                    }
                    break;
                #endregion

                #region 商城退款
                case "MallRefund"://退款
                    BLLMall bllMall = new BLLMall();
                    WXMallRefund refund = bllMall.Get<WXMallRefund>(string.Format("RefundId='{0}'", record.TranId));
                    switch (refund.Status)
                    {
                        case 2:
                            msg = "未同意退款";
                            return false;
                        case 5:
                            msg = "未收到货拒绝退款";
                            return false;
                        case 7:
                            msg = "退款申请关闭";
                            return false;
                        default:
                            break;
                    }
                    if (!bllMall.Refund(refund.OrderDetailId, out msg))
                    {
                        return false;
                    }

                    break;
                #endregion
                default:
                    msg = "未定义的类型";
                    return false;
            }
            record.UpdateTime = DateTime.Now;
            record.Status = 1;
            record.OperaUserId = userId;
            if (Update(record))
            {
                return true;
            }
            return false;


        }


        /// <summary>
        /// 设置客服的审核权限
        /// </summary>
        /// <param name="autoId"></param>
        /// <param name="value">1 有权限 0无权限</param>
        /// <returns></returns>
        public bool UpdateKefuTranPer(int autoId, string value)
        {
            WXKeFu model = Get<WXKeFu>(string.Format("AutoId={0}", autoId));
            model.IsTransfersAuditPer = int.Parse(value);
            if (Update(model))
            {
                return true;
            }
            return false;


        }
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="autoId">记录id</param>
        /// <returns></returns>
        public TransfersAudit Get(int autoId)
        {

            return Get<TransfersAudit>(string.Format("AutoId={0}", autoId));

        }
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="tranId">交易id</param>
        /// <returns></returns>
        public TransfersAudit GetByTranId(string tranId)
        {

            return Get<TransfersAudit>(string.Format("TranId='{0}'", tranId));

        }
        /// <summary>
        ///审核列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="status">状态</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public List<TransfersAudit> List(int pageIndex, int pageSize, string websiteOwner, string status, string fromDate, string toDate, string type, string keyWord, out int totalCount)
        {
            totalCount = 0;
            string orderBy = " Status ASC,AutoId ASC";
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status={0}", status);
                if (status == "1")
                {
                    orderBy = "AutoId DESC";
                }
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And Type='{0}'", type);
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                sbWhere.AppendFormat(" And InsertDate>='{0}'", fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                sbWhere.AppendFormat(" And InsertDate<='{0}'", toDate);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And TranId like '%{0}%'", keyWord);
            }

            totalCount = GetCount<TransfersAudit>(sbWhere.ToString());
            return GetLit<TransfersAudit>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

        }

        /// <summary>
        /// 是否有审核打款权限
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool IsTransfersAuditPer(UserInfo userInfo)
        {

            if (string.IsNullOrEmpty(userInfo.WXOpenId))
            {
                return false;
            }
            return GetCount<WXKeFu>(string.Format(" WebsiteOwner='{0}' And WeiXinOpenID='{1}' And IsTransfersAuditPer=1", userInfo.WebsiteOwner, userInfo.WXOpenId)) > 0;


        }
        /// <summary>
        /// 获取有审核权限的所有客服
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<WXKeFu> TransferAuditPerKefuList(string websiteOwner)
        {

            return GetList<WXKeFu>(string.Format("WebsiteOwner='{0}' And IsTransfersAuditPer=1", websiteOwner));

        }


    }
}
