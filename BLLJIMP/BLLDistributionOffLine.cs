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
    /// 线下分销BLL
    /// </summary>
    public class BLLDistributionOffLine : BLL
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLUser bllUser = new BLLUser("");
        /// <summary>
        /// 真实活动BLL
        /// </summary>
        BLLActivity bllActivity = new BLLActivity("");
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 日志
        /// </summary>
        BLLLog bllLog = new BLLLog();

        /// <summary>
        /// 执行分佣动作的项目状态
        /// </summary>
        private string SuccessStatus
        {

            get
            {
                var statusList = QueryProjectStatusList();
                var statusModel = statusList.FirstOrDefault(p => p.StatusAction == "DistributionOffLineCommission");
                if (statusModel != null)
                {
                    return statusModel.OrderStatu;
                }
                return "已分佣";

            }
        }

        #region 分销用户模块
        /// <summary>
        /// 获取用户的分销上一级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUpUser(string userId)
        {
            UserInfo preUserInfo = Get<UserInfo>(string.Format("UserId='{0}'", userId));
            if (preUserInfo == null)
            {
                return null;
            }
            else
            {
                return Get<UserInfo>(string.Format("UserID='{0}'", preUserInfo.DistributionOffLinePreUserId));
            }
        }


        /// <summary>
        ///获取用户的分销上级,第level级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public UserInfo GetUpUser(string userId, int level)
        {
            UserInfo preUser = new UserInfo();
            for (int i = 0; i < level; ++i)
            {
                preUser = GetUpUser(userId);
                if (preUser == null)
                {
                    return null;
                }
                else
                {
                    userId = preUser.UserID;
                }
            }
            return preUser;
        }


        /// <summary>
        ///获取用户的分销下一级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserInfo> GetDownUserList(string userId)
        {
            return GetList<UserInfo>(string.Format("DistributionOffLinePreUserId='{0}'", userId));
        }

        /// <summary>
        /// 获取用户的分销下级,第level级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<UserInfo> GetDownUserList(string userId, int level)
        {
            List<UserInfo> userList = new List<UserInfo>();
            userList.Add(Get<UserInfo>(string.Format("UserId='{0}'", userId)));

            for (int i = 0; i < level; ++i)
            {
                List<UserInfo> tempList = new List<UserInfo>();
                tempList.AddRange(userList);
                userList.Clear();
                foreach (UserInfo user in tempList)
                {
                    userList.AddRange(GetDownUserList(user.UserID));
                }
            }
            return userList;
        }

        /// <summary>
        ///获取下N级用户总和
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDownUserTotalCount(string userId, int level)
        {
            List<UserInfo> list = new List<UserInfo>();
            for (int i = 1; i <= level; i++)
            {
                var userList = GetDownUserList(userId, i);
                list.AddRange(userList);
            }
            return list.Distinct().ToList().Count;

        }

        /// <summary>
        ///获取下线第N级用户数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDownUserCount(string userId, int level)
        {
            return GetDownUserList(userId, level).Count;

        }
        /// <summary>
        /// 获取上级用户跟下级用之间的级数
        /// </summary>
        /// <param name="upUserInfo"></param>
        /// <param name="downUserInfo"></param>
        /// <returns></returns>
        public int GetUserBetweenLevel(UserInfo upUserInfo, UserInfo downUserInfo)
        {
            int level = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (GetDownUserList(upUserInfo.UserID, i).Where(p => p.UserID.Equals(downUserInfo.UserID)).Count() > 0)
                {
                    level = i;
                    break;
                }

            }
            return level;

        }
        /// <summary>
        /// 获取用户可提现金额
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public decimal GetUserCanUseAmount(UserInfo userInfo)
        {
            decimal result = userInfo.DistributionOffLineTotalAmount - userInfo.DistributionOffLineFrozenAmount;
            return result > 0 ? result : 0;
        }
        /// <summary>
        /// 获取用户推荐的人数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserCommendCount(string userId)
        {
            return GetCount<UserInfo>(string.Format(" DistributionOffLinePreUserId='{0}'", userId));

        }
        /// <summary>
        /// 是否是分销会员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsDistributionMember(string userId)
        {

            UserInfo userInfo = bllUser.GetUserInfo(userId);
            if (!string.IsNullOrEmpty(userInfo.DistributionOffLinePreUserId))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否有下级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsHaveLowerLevel(string userId)
        {

            var count = GetCount<UserInfo>(string.Format(" DistributionOffLinePreUserId = '{0}' AND UserID <> '{0}' ", userId));

            return count > 0;
        }

        /// <summary>
        /// 获取设置的会员显示级别
        /// </summary>
        /// <returns></returns>
        public int GetDistributionShowLevel()
        {
            return GetWebsiteInfoModelFromDataBase().DistributionOffLineShowLevel;
        }

        /// <summary>
        /// 获取用户累计直接销售额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetDirectSaleAmount(string userId)
        {
            string sql = string.Format("select Sum(Amount) From ZCJ_Project where ProjectType='DistributionOffLine' And UserId='{0}'", userId);
            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
            if (result != null)
            {
                return decimal.Parse(result.ToString());
            }
            return 0;

        }
        /// <summary>
        /// 获取用户累计直接销售项目数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetDirectSaleCount(string userId)
        {
            return GetCount<Project>(string.Format(" WebsiteOwner='{0}' And UserId='{1}'", WebsiteOwner, userId));

        }
        /// <summary>
        /// 获取佣金
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public decimal GetCommissionAmount(string userId, int level)
        {
            string sql = string.Format("select Sum(Amount) From ZCJ_ProjectCommission where UserId='{0}' And CommissionLevel ={1}", userId, level);
            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
            if (result != null)
            {
                return decimal.Parse(result.ToString());
            }
            return 0;

        }

        /// <summary>
        /// 贡献佣金
        /// </summary>
        /// <param name="parentUserId">上级用户</param>
        /// <param name="level">级数</param>
        /// <returns></returns>
        public decimal GetContributionCommissionAmount(string parentUserId, string childUserId, int level = 1)
        {

            decimal totalAmount = 0;
            string sql = "";
            switch (level)
            {
                case 1:
                    sql = string.Format("select Sum(Amount) From ZCJ_ProjectCommission where UserId='{0}' And CommissionUserId ='{1}' And CommissionLevel=1", parentUserId, childUserId);//一级分销贡献
                    break;
                case 2:
                    var userList = GetDownUserList(childUserId);
                    if (userList.Count > 0)
                    {
                        string userIds = "";
                        foreach (var user in userList)
                        {
                            userIds += string.Format("'{0}',", user.UserID);

                        }
                        userIds = userIds.TrimEnd(',');
                        sql = string.Format("select Sum(Amount) From ZCJ_ProjectCommission where UserId='{0}' And CommissionUserId in ({1})", parentUserId, userIds);//二级分销贡献
                    }
                    break;
                case 3:
                    var userList1 = GetDownUserList(childUserId, 2);
                    if (userList1.Count > 0)
                    {
                        string userIds = "";
                        foreach (var user in userList1)
                        {
                            userIds += string.Format("'{0}',", user.UserID);

                        }
                        userIds = userIds.TrimEnd(',');
                        sql = string.Format("select Sum(Amount) From ZCJ_ProjectCommission where UserId='{0}' And CommissionUserId in ({1})", parentUserId, userIds);//二级分销贡献
                    }
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
                if (result != null)
                {
                    return decimal.Parse(result.ToString());
                }
            }
            return totalAmount;

        }


        /// <summary>
        /// 项目分佣记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="keyWord"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ProjectCommission> QueryProjectCommissionList(int pageIndex, int pageSize, out int totalCount, string keyWord = "", string userId = "", string type = "",string fromDate="",string toDate="")
        {

            StringBuilder sbWhere = new StringBuilder();
            if (type=="")
            {
                type = "DistributionOnLine";
            }
            sbWhere.AppendFormat("  WebsiteOwner='{0}' And ProjectType ='{1}'", WebsiteOwner, type);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", keyWord);

            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId= '{0}'", userId);

            }
            if (!string.IsNullOrEmpty(type))
            {
                 sbWhere.AppendFormat(" And ProjectType= '{0}'", type);
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                sbWhere.AppendFormat(" And InsertDate>='{0}'", fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                sbWhere.AppendFormat(" And InsertDate<='{0}'", toDate);
            }
            totalCount = GetCount<ProjectCommission>(sbWhere.ToString());
            return GetLit<ProjectCommission>(pageSize, pageIndex, sbWhere.ToString(), " ProjectId ASC,InsertDate ASC");


        }

        /// <summary>
        /// 更新分销上级
        /// </summary>
        /// <param name="autoIds"></param>
        /// <param name="preUserId"></param>
        /// <returns></returns>
        public bool UpdatePreUserId(string autoIds,string preUserId) {

            if (Update(new UserInfo(), string.Format("DistributionOffLinePreUserId='{0}'", preUserId), string.Format(" AutoId in({0}) And WebsiteOwner='{1}'",autoIds,WebsiteOwner)) == autoIds.Split(',').Count())
            {
                bllLog.Add(Enums.EnumLogType.DistributionOffLine,Enums.EnumLogTypeAction.Update,GetCurrUserID(), string.Format("设置上级，上级用户{0},用户ID({1})", preUserId, autoIds));
                return true;
            }
            return false;
        
        }
        /// <summary>
        /// 增加累计佣金
        /// </summary>
        /// <param name="autoIds"></param>
        /// <param name="preUserId"></param>
        /// <returns></returns>
        public bool UpdateHistoryTotalAmount(string autoIds, decimal amount)
        {

            if (Update(new UserInfo(), string.Format("HistoryDistributionOffLineTotalAmount+={0}", amount), string.Format(" AutoId in({0}) And WebsiteOwner='{1}'", autoIds, WebsiteOwner)) == autoIds.Split(',').Count())
            {

                bllLog.Add(Enums.EnumLogType.DistributionOffLine,Enums.EnumLogTypeAction.Add,GetCurrUserID(), string.Format("增加累计佣金,金额{0},用户ID({1})",amount,autoIds));
                return true;
            }
            return false;

        }


        #endregion

        #region 提现模块

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="bankCardId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ApplyWithrawCash(UserInfo userInfo, int bankCardId, string amount, string websiteOwner, out string msg)
        {

            bool result = false;
            msg = "";
            decimal amountD = 0;
            if (!decimal.TryParse(amount, out amountD))
            {
                msg = "金额不正确";
                goto outoff;
            }
            if (amountD <= 1)
            {
                msg = "金额必须大于1";
                goto outoff;
            }
            BindBankCard bankCard = Get<BindBankCard>(string.Format("AutoId={0} And UserId='{1}'", bankCardId, userInfo.UserID));
            if (bankCard == null)
            {
                msg = "银行卡不存在";
                goto outoff;

            }
            if (GetUserCanUseAmount(userInfo) < amountD)
            {
                msg = "您的可用余额不足";
                goto outoff;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();
            try
            {
                if (Update(userInfo, string.Format(" DistributionOffLineFrozenAmount+={0}", amountD), string.Format(" AutoID={0}", userInfo.AutoID), tran) > 0)
                {
                    //插入申请提现记录表
                    WithdrawCash model = new WithdrawCash();
                    model.AccountBranchCity = bankCard.AccountBranchCity;
                    model.AccountBranchName = bankCard.AccountBranchName;
                    model.AccountBranchProvince = bankCard.AccountBranchProvince;
                    model.AccountName = bankCard.AccountName;
                    model.Amount = amountD;
                    model.BankAccount = bankCard.BankAccount;
                    model.BankName = bankCard.BankName;
                    model.InsertDate = DateTime.Now;
                    model.IsPublic = 1;
                    model.Phone = userInfo.Phone;
                    model.ServerFee = 0;
                    model.RealAmount = amountD;
                    model.Status = 0;
                    model.TrueName = userInfo.TrueName;
                    model.UserId = userInfo.UserID;
                    model.LastUpdateDate = DateTime.Now;
                    model.WebSiteOwner = websiteOwner;
                    model.WithdrawCashType = "DistributionOffLine";
                    if (Add(model, tran))
                    {
                        tran.Commit();
                        msg = "您的提现申请已经成功提交!";
                        result = true;

                    }
                    else
                    {
                        msg = "插入提现申请失败";
                        tran.Rollback();
                        goto outoff;
                    }


                }
                else
                {
                    msg = "更新冻结金额失败";
                    tran.Rollback();
                    goto outoff;
                }



            }
            catch (Exception ex)
            {
                msg = ex.Message;
                tran.Rollback();
                goto outoff;

            }

        outoff:
            return result;

        }

        /// <summary>
        ///修改提现状态
        /// </summary>
        /// <param name="list"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateWithrawCashStatus(List<WithdrawCash> list, int status, out string msg)
        {

            msg = "";
            bool result = false;
            if (list.Count() <= 0)
            {
                msg = "修改记录数量不能为零";
                goto outoff;
            }
            if (status.Equals(0))
            {
                msg = "待审核状态不能修改";
                goto outoff;
            }
            if (list.Where(p => p.Status.Equals(status)).Count() > 0)
            {
                msg = "新状态与旧状态相同，不能修改";
                goto outoff;
            }
            if (list.Where(p => p.Status.Equals(2)).Count() > 0)
            {
                msg = "状态为成功的记录不能修改";
                goto outoff;
            }
            if (list.Where(p => p.Status.Equals(3)).Count() > 0)
            {
                msg = "状态为失败的记录不能修改";
                goto outoff;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            StringBuilder sbSQL = new StringBuilder();
            try
            {
                foreach (var item in list)
                {
                    UserInfo userInfo = bllUser.GetUserInfo(item.UserId);

                    if (userInfo.DistributionOffLineFrozenAmount - item.Amount < 0)
                    {
                        msg = string.Format("账户 {0} 冻结金额不足,请检查", userInfo.UserID);
                        goto outoff;
                    }
                    if (userInfo.DistributionOffLineTotalAmount - item.Amount < 0)
                    {
                        msg = string.Format("账户 {0} 余额不足,请检查", userInfo.UserID);
                        goto outoff;
                    }
                    if (status.Equals(1))//修改为受理中
                    {

                    }
                    else if (status.Equals(2))//修改为成功
                    {
                        //扣除账户余额并解冻
                        sbSQL.AppendFormat(" UPDATE ZCJ_UserInfo SET DistributionOffLineTotalAmount-={0},DistributionOffLineFrozenAmount-={0} where UserId='{1}';", item.Amount, userInfo.UserID);



                    }
                    else if (status.Equals(3))//修改为失败
                    {
                        //解冻
                        sbSQL.AppendFormat(" UPDATE ZCJ_UserInfo SET DistributionOffLineFrozenAmount-={0} where UserId='{1}';", item.Amount, userInfo.UserID);

                    }



                }

                sbSQL.AppendFormat("update ZCJ_WithdrawCash set Status={0},LastUpdateDate=getdate() where AutoID in({1})", status, string.Join(",", list.SelectMany(p => new List<int> { (int)p.AutoID })));//修改提现记录状态

                int count = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSQL.ToString(), tran);
                if (count > 0)
                {
                    tran.Commit();
                    result = true;
                    msg = "操作成功!";

                }
                else
                {
                    tran.Rollback();
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = ex.Message;
                goto outoff;
            }
        outoff:
            return result;
        }


        /// <summary>
        /// 提现申请纪录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="totalCount"></param>
        /// <param name="keyWord"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<WithdrawCash> QueryWithdrawCashList(int pageIndex, int pageSize, string userId, out int totalCount, string status = "",
            string withdrawCashType = "DistributionOffLine", string ids = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' And WithdrawCashType ='{1}'", WebsiteOwner, withdrawCashType);
            if (!string.IsNullOrWhiteSpace(status))
            {
                sbWhere.AppendFormat(" And Status In ({0})", status);
            }
            if (!string.IsNullOrEmpty(userId)){
                if (userId.IndexOf(",") > 0)
                {
                    sbWhere.AppendFormat(" And UserId In ({0})", userId);
                }
                else
                {
                    sbWhere.AppendFormat(" And UserId= '{0}'", userId);
                }
            }
            if (!string.IsNullOrWhiteSpace(ids))
            {
                sbWhere.AppendFormat(" And AutoID In ({0})", ids);
            }
            totalCount = GetCount<WithdrawCash>(sbWhere.ToString());
            return GetLit<WithdrawCash>(pageSize, pageIndex, sbWhere.ToString(), "AutoID DESC");
        }
        #endregion

        #region 项目模块
        /// <summary>
        /// 项目查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="totalCount">总数量</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="status">状态</param>
        /// <param name="userId">用户名</param>
        /// <param name="from">时间开始</param>
        /// <param name="to">时间结束</param>
        /// <param name="projectType">项目类型</param>
        /// <returns></returns>
        public List<Project> QueryProjectList(int pageIndex, int pageSize, out int totalCount, string keyWord = "", string status = "", string userId = "", string from = "", string to = "", string projectType = "")
        {

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (projectType == "HouseRecommend")
                {
                    sbWhere.AppendFormat(" And ( Ex1 like '%{0}%' or Ex2='{0}' or Ex4='{0}' or Ex6='{0}')", keyWord);
                }
                else if (projectType == "CompanyBranchApply" || projectType == "CompanyBranchRecommend")
                {
                    sbWhere.AppendFormat(" And ( Ex3 = '{0}' or Contact like '%{0}%' or Phone like '%{0}%')", keyWord);
                }
                else if (projectType == "HouseAppointment" || projectType == "HouseBuyerRecommend")
                {
                    sbWhere.AppendFormat(" And ( Ex2 like '%{0}%' or Contact like '%{0}%' or Phone like '%{0}%' or Introduction like '%{0}%' )", keyWord);
                }
                else
                {
                    sbWhere.AppendFormat(" And ProjectName like '%{0}%'", keyWord);
                }

            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status= '{0}'", status);

            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId= '{0}'", userId);

            }
            if (!string.IsNullOrEmpty(from))
            {
                sbWhere.AppendFormat(" And InsertDate>= '{0}'", from);

            }
            if (!string.IsNullOrEmpty(to))
            {
                sbWhere.AppendFormat(" And InsertDate<= '{0}'", to);

            }

            if (!string.IsNullOrWhiteSpace(projectType))
            {
                sbWhere.AppendFormat(" And ProjectType = '{0}'", projectType);
            }
            else
            {
                sbWhere.AppendFormat(" And ProjectType = 'DistributionOffLine'");
            }

            totalCount = GetCount<Project>(sbWhere.ToString());
            return GetLit<Project>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");


        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectIntro">项目介绍</param>
        /// <param name="amount">项目金额</param>
        /// <param name="status">状态</param>
        /// <param name="contact">联系人</param>
        /// <param name="phone">手机</param>
        /// <param name="weixin">微信</param>
        /// <param name="company">公司</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public bool AddProject(string userId, string projectName, string projectIntro, string amount, out string msg, string status = "待审核", string contact = "", string phone = "", string weixin = "", string company = "", string remark = "", string ex1="",
            string ex2="",string ex3="",string ex4="",string ex5="",string ex6="",string ex7="",string ex8="",string ex9="",string ex10="",string ex11="",string ex12="",string ex13="",string ex14="",string proJectType="")
        {
            msg = "";
            if (bllUser.GetUserInfo(userId) == null)
            {
                msg = "用户不存在,请检查";
                return false;
            }
            Project model = new Project();
            model.ProjectId = int.Parse(GetGUID(TransacType.CommAdd));
            model.UserId = userId;
            model.ProjectName = projectName;
            model.Introduction = projectIntro;
            model.InsertDate = DateTime.Now;
            model.ProjectType = "DistributionOffLine";
            if (!string.IsNullOrEmpty(proJectType)) model.ProjectType = proJectType;
            model.Status = status;
            model.WebsiteOwner = WebsiteOwner;
            if (!string.IsNullOrEmpty(amount)) model.Amount = decimal.Parse(amount);
            model.Contact = contact;
            model.Phone = phone;
            model.WeiXin = weixin;
            model.Company = company;
            model.Ex1 = ex1;
            model.Ex2 = ex2;
            model.Ex3 = ex3;
            model.Ex4 = ex4;
            model.Ex5 = ex5;
            model.Ex6 = ex6;
            model.Ex7 = ex7;
            model.Ex8 = ex8;
            model.Ex9 = ex9;
            model.Ex10 = ex10;
            model.Ex11 = ex11;
            model.Ex12 = ex12;
            model.Ex13 = ex13;
            model.Ex14 = ex14;
            ProjectLog log = new ProjectLog();
            log.InsertDate = DateTime.Now;
            log.ProjectId = model.ProjectId;
            log.ProjectName = model.ProjectName;
            log.Remark = string.Format("提交了项目{0}操作人:{1}", model.ProjectName, GetCurrUserID());
            log.Status = model.Status = model.Status;
            log.UserId = GetCurrUserID();
            log.WebsiteOwner = WebsiteOwner;
            Add(log);

            return Add(model);

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="projectName"></param>
        /// <param name="projectIntro"></param>
        /// <param name="amount"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateProject(int projectId, string userId, string projectName, string projectIntro, string amount, string status, out string msg, string remark = "", string ex1 = "",
            string ex2 = "", string ex3 = "", string ex4 = "", string ex5 = "", string ex6 = "", string ex7 = "", string ex8 = "", string ex9 = "", string ex10 = "", string ex11 = "", string ex12 = "", string ex13 = "",string ex14="", string proJectType = "",string contack="",string phone="")
        {
            msg = "";
            if (bllUser.GetUserInfo(userId) == null)
            {
                msg = "用户不存在,请检查";
                return false;
            }
            Project model = GetProject(projectId);
            if (model.IsComplete == 1)
            {
                msg = "项目已分佣，不可更改";
                return false;
            }
            string oldStatus = model.Status;
            model.ProjectName = projectName;
            model.Introduction = projectIntro;
            model.Status = status;
            if (!string.IsNullOrEmpty(amount)) model.Amount = decimal.Parse(amount);
            if (!string.IsNullOrEmpty(contack)) model.Contact = contack;
            if (!string.IsNullOrEmpty(phone)) model.Phone = phone;
            model.Remark = remark;
            if (!string.IsNullOrEmpty(ex1)) model.Ex1 = ex1;
            if (!string.IsNullOrEmpty(ex2)) model.Ex2 = ex2;
            model.Ex3 = ex3;
            model.Ex4 = ex4;
            model.Ex5 = ex5;
            model.Ex6 = ex6;
            model.Ex7 = ex7;
            model.Ex8 = ex8;
            model.Ex9 = ex9;
            model.Ex10 = ex10;
            model.Ex11 = ex11;
            model.Ex12 = ex12;
            model.Ex13 = ex13;
            model.Ex14 = ex14;
            if (model.Status == SuccessStatus)
            {
                 //<summary>
                 //线上分销BLL
                 //</summary>
                BLLDistribution bllDisOnLine = new BLLDistribution();
                if (!bllDisOnLine.Transfers(model.ProjectId, out msg))
                //if (!Transfers(model.ProjectId, out msg))
                {
                    return false;
                }
                else
                {
                    model.IsComplete = 1;
                }
            }
            if (Update(model))
            {
                ProjectLog log = new ProjectLog();
                log.InsertDate = DateTime.Now;
                log.ProjectId = model.ProjectId;
                log.ProjectName = model.ProjectName;
                log.Remark = string.Format("项目状态由{0}变更为:{1}.操作人:{2}", oldStatus, model.Status, "管理员");
                log.Status = model.Status = model.Status;
                log.UserId = GetCurrUserID();
                log.WebsiteOwner = WebsiteOwner;
                log.UserId = model.UserId;
                Add(log);

                #region 给用户发送消息
                if (model.ProjectType == "DistributionOffLine")
                {
                    UserInfo userInfo = bllUser.GetUserInfo(model.UserId);
                    bllWeixin.SendTemplateMessageNotifyComm(userInfo, string.Format("项目状态变动通知"), string.Format("您提交的项目“{0}”状态变更为”{1}“", model.ProjectName, model.Status), string.Format("http://{0}/app/distribution/m/index.aspx", System.Web.HttpContext.Current.Request.Url.Host));
                }
                #endregion
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// 获取单个项目信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Project GetProject(int projectId)
        {
            return Get<Project>(string.Format(" ProjectId={0}", projectId));

        }
        /// <summary>
        /// 获取项目日志列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="UserID"></param>
        /// <param name="WebsiteOwner"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProjectLog> GetProJectLogList(int pageSize, int pageIndex, string UserID, string projectId, string WebsiteOwner, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' ", WebsiteOwner));
            sbWhere.AppendFormat(" AND UserId='{0}' ", UserID);
            if (!string.IsNullOrEmpty(projectId))
            {
                sbWhere.AppendFormat(" AND ProjectId={0}", projectId);
            }
            totalCount = GetCount<ProjectLog>(sbWhere.ToString());
            return GetLit<ProjectLog>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate DESC ");
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public int DeleteProject(string projectId)
        {
            return Delete(new Project(), string.Format(" ProjectId ={0}", projectId));

        }

        #region 项目字段
        /// <summary>
        /// 查询项目字段映射
        /// </summary>
        /// <returns></returns>
        public List<TableFieldMapping> QueryProjectFieldMapList(string moduleType="")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' And TableName ='ZCJ_Project'", WebsiteOwner);
            if (!string.IsNullOrEmpty(moduleType))
            {
                sbWhere.AppendFormat(" AND ModuleType='{0}' ", moduleType);
            }
            else
            {
                sbWhere.AppendFormat(" AND ModuleType is Null ");
            }
            sbWhere.AppendFormat(" Order by Sort DESC ");
            return GetList<TableFieldMapping>(sbWhere.ToString());

        }

        /// <summary>
        /// 添加字段映射
        /// </summary>
        /// <param name="field"></param>
        /// <param name="fieldShowName"></param>
        /// <returns></returns>
        public bool AddProjectFieldMap(string field, string fieldShowName, string isNull = "", string sort = "",string moduleType="",string isShowList="")
        {

            TableFieldMapping model = new TableFieldMapping();
            model.TableName = "ZCJ_Project";
            model.WebSiteOwner = WebsiteOwner;
            model.Field = field;
            model.MappingName = fieldShowName;
            if (!string.IsNullOrEmpty(isShowList)) model.IsShowInList = int.Parse(isShowList);
            if (!string.IsNullOrEmpty(moduleType)) model.ModuleType = moduleType;
            if (!string.IsNullOrEmpty(isNull)) model.FieldIsNull = int.Parse(isNull);
            if (!string.IsNullOrEmpty(sort)) model.Sort = int.Parse(sort);
            return Add(model);


        }
        /// <summary>
        /// 更新字段映射
        /// </summary>
        /// <param name="autoId"></param>
        /// <param name="field"></param>
        /// <param name="fieldShowName"></param>
        /// <returns></returns>
        public bool UpdateProjectFieldMap(string autoId, string field, string fieldShowName, string isNull = "", string sort = "", string moduleType = "", string isShowList = "")
        {

            TableFieldMapping model = GetProjectFieldMap(autoId);
            model.Field = field;
            model.MappingName = fieldShowName;
            if (!string.IsNullOrEmpty(isShowList)) model.IsShowInList = int.Parse(isShowList);
            if (!string.IsNullOrEmpty(moduleType)) model.ModuleType = moduleType;
            if (!string.IsNullOrEmpty(isNull)) model.FieldIsNull = int.Parse(isNull);
            if (!string.IsNullOrEmpty(sort)) model.Sort = int.Parse(sort);
            return Update(model);


        }
        /// <summary>
        /// 单个字段映射
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public TableFieldMapping GetProjectFieldMap(string autoId)
        {
            return Get<TableFieldMapping>(string.Format("AutoId={0}", autoId));
        }

        /// <summary>
        /// 删除字段映射
        /// </summary>
        /// <param name="autoId"></param>
        /// <param name="field"></param>
        /// <param name="fieldShowName"></param>
        /// <returns></returns>
        public int DeleteProjectFieldMap(string autoIds)
        {
            return Delete(new TableFieldMapping(), string.Format("AutoId in({0})", autoIds));
        }
        #endregion

        /// <summary>
        /// 前端获取项目需要的字段
        /// </summary>
        /// <returns></returns>
        public object GetProjectFieldMapListF()
        {
            List<ProjectField> result = new List<ProjectField>();
            var list = QueryProjectFieldMapList();
            if (list.FirstOrDefault(p => p.Field == "ProjectName") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "ProjectName");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort=model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Contact") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Contact");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Phone") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Phone");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Company") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Company");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "WeiXin") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "WeiXin");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Remark") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Remark");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex1") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex1");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex2") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex2");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex3") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex3");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex4") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex4");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex5") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex5");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex6") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex6");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex7") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex7");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex8") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex8");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex9") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex9");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex10") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex10");
                result.Add(new ProjectField
                {
                    field = model.Field,
                    field_show_name = model.MappingName,
                    field_is_null = model.FieldIsNull,
                    field_sort = model.Sort
                });
            }


            return result.OrderByDescending(p=>p.field_sort) ;

        }
        /// <summary>
        /// 项目字段
        /// </summary>
        private class ProjectField {

            /// <summary>
            /// 字段名称
            /// </summary>
            public string field { get; set; }
            /// <summary>
            /// 字段显示名称
            /// </summary>
            public string field_show_name { get; set; }
            /// <summary>
            /// 可以为空
            /// </summary>
            public int field_is_null { get; set; }
            /// <summary>
            /// 排序号
            /// </summary>
            public int field_sort { get; set; }

        }



        /// <summary>
        /// 前端获取项目需要的字段
        /// </summary>
        /// <returns></returns>
        public object GetProjectPropF(Project projectInfo)
        {
            List<object> result = new List<object>();
            var list = QueryProjectFieldMapList();
            if (list.FirstOrDefault(p => p.Field == "ProjectName") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "ProjectName");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.ProjectName
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Contact") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Contact");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Contact
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Phone") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Phone");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Phone
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Company") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Company");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Company
                });
            }
            if (list.FirstOrDefault(p => p.Field == "WeiXin") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "WeiXin");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.WeiXin
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Remark") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Remark");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Remark
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex1") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex1");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex1
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex2") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex2");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex2
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex3") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex3");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex3
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex4") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex4");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex4
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex5") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex5");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex5
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex6") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex6");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex6
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex7") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex7");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex7
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex8") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex8");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex8
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex9") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex9");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex9
                });
            }
            if (list.FirstOrDefault(p => p.Field == "Ex10") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex10");
                result.Add(new
                {
                    field_show_name = model.MappingName,
                    field_value = projectInfo.Ex10
                });
            }
            return result;

        }

        /// <summary>
        /// 检查项目字段
        /// </summary>
        /// <returns></returns>
        public bool CheckProjectInfo(Project projectInfo, out string msg)
        {
            msg = "";
            var list = QueryProjectFieldMapList();
            if (list.FirstOrDefault(p => p.Field == "ProjectName") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "ProjectName");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.ProjectName)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Contact") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Contact");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Contact)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Phone") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Phone");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Phone)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Company") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Company");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Company)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "WeiXin") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "WeiXin");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.WeiXin)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Remark") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Remark");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Remark)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex1") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex1");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex1)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex2") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex2");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex2)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex3") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex3");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex3)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex4") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex4");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex4)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex5") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex5");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex5)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex6") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex6");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex6)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex7") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex7");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex7)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex8") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex8");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex8)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }
            }
            if (list.FirstOrDefault(p => p.Field == "Ex9") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex9");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex9)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }

            }
            if (list.FirstOrDefault(p => p.Field == "Ex10") != null)
            {
                var model = list.FirstOrDefault(p => p.Field == "Ex10");
                if (model.FieldIsNull == 0 && (string.IsNullOrEmpty(projectInfo.Ex10)))
                {
                    msg = model.MappingName + "必填";
                    return false;
                }

            }
            return true;

        }



        /// <summary>
        /// 分销打佣金
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool Transfers(int projectId, out string msg)
        {

            msg = "";
            Project projectInfo = GetProject(projectId);
            if (projectInfo.IsComplete == 1)
            {
                msg = "已经分过佣金了";
                return false;
            }
            if (projectInfo.Amount == 0)
            {
                msg = "项目金额为0不可以分佣";
                return false;
            }
            decimal totalAmount = projectInfo.Amount;
            int disLevel = GetDistributionLevel();//设置的分销级别
            UserInfo upUserLevel0 = bllUser.GetUserInfo(projectInfo.UserId);//项目提供者直销
            UserInfo upUserLevel1 = null;//分销上一级
            UserInfo upUserLevel2 = null;//分销上二级
            UserInfo upUserLevel3 = null;//分销上三级
            StringBuilder sbSql = new StringBuilder();
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();//事务
            #region 直销一级 给项目直接提供者佣金
            if (upUserLevel0 != null)//给直销上级打款
            {
                int rate = GetDistributionRate(upUserLevel0, 0);
                decimal amount = ((decimal)rate / 100) * totalAmount;
                sbSql.AppendFormat(" Update ZCJ_UserInfo Set DistributionOffLineTotalAmount+={0},HistoryDistributionOffLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel0.UserID);

                ProjectCommission model = new ProjectCommission();
                model.UserId = upUserLevel0.UserID;
                model.Amount = amount;
                model.CommissionUserId = projectInfo.UserId;
                model.InsertDate = DateTime.Now;
                model.ProjectAmount = projectInfo.Amount;
                model.ProjectId = projectInfo.ProjectId;
                model.ProjectName = projectInfo.ProjectName;
                model.Rate = rate;
                model.WebsiteOwner = bllUser.WebsiteOwner;
                model.ProjectType = "DistributionOffLine";
                model.Remark = "直销佣金";
                model.CommissionLevel = "0";
                if (!Add(model, tran))
                {
                    tran.Rollback();
                    msg = "分佣失败";
                    return false;
                }

            }
            #endregion

            #region 分销上一级
            if (disLevel >= 1)
            {
                upUserLevel1 = GetUpUser(projectInfo.UserId, 1);
                if (upUserLevel1 != null)//一级分销打款
                {
                    int rate = GetDistributionRate(upUserLevel1, 1);
                    decimal amount = ((decimal)rate / 100) * totalAmount;
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set DistributionOffLineTotalAmount+={0},HistoryDistributionOffLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel1.UserID);
                    ProjectCommission model = new ProjectCommission();
                    model.UserId = upUserLevel1.UserID;
                    model.Amount = amount;
                    model.CommissionUserId = projectInfo.UserId;
                    model.InsertDate = DateTime.Now;
                    model.ProjectAmount = projectInfo.Amount;
                    model.ProjectId = projectInfo.ProjectId;
                    model.ProjectName = projectInfo.ProjectName;
                    model.Rate = rate;
                    model.WebsiteOwner = bllUser.WebsiteOwner;
                    model.ProjectType = "DistributionOffLine";
                    model.Remark = "一级分销佣金";
                    model.CommissionLevel = "1";
                    if (!Add(model, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }

            }
            #endregion

            #region 分销上二级
            if (disLevel >= 2)
            {
                upUserLevel2 = GetUpUser(projectInfo.UserId, 2);
                if (upUserLevel2 != null)//二级分销打款
                {
                    int rate = GetDistributionRate(upUserLevel2, 2);
                    decimal amount = ((decimal)rate / 100) * totalAmount;
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set DistributionOffLineTotalAmount+={0},HistoryDistributionOffLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel2.UserID);
                    ProjectCommission model = new ProjectCommission();
                    model.UserId = upUserLevel2.UserID;
                    model.Amount = amount;
                    model.CommissionUserId = projectInfo.UserId;
                    model.InsertDate = DateTime.Now;
                    model.ProjectAmount = projectInfo.Amount;
                    model.ProjectId = projectInfo.ProjectId;
                    model.ProjectName = projectInfo.ProjectName;
                    model.Rate = rate;
                    model.WebsiteOwner = bllUser.WebsiteOwner;
                    model.ProjectType = "DistributionOffLine";
                    model.Remark = "二级分销佣金";
                    model.CommissionLevel = "2";
                    if (!Add(model, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }
            }
            #endregion

            #region 分销上三级
            if (disLevel >= 3)
            {
                upUserLevel3 = GetUpUser(projectInfo.UserId, 3);
                if (upUserLevel3 != null)//三级分销打款
                {
                    int rate = GetDistributionRate(upUserLevel3, 3);
                    decimal amount = ((decimal)rate / 100) * totalAmount;
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set DistributionOffLineTotalAmount+={0},HistoryDistributionOffLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel3.UserID);
                    ProjectCommission model = new ProjectCommission();
                    model.UserId = upUserLevel3.UserID;
                    model.Amount = amount;
                    model.CommissionUserId = projectInfo.UserId;
                    model.InsertDate = DateTime.Now;
                    model.ProjectAmount = projectInfo.Amount;
                    model.ProjectId = projectInfo.ProjectId;
                    model.ProjectName = projectInfo.ProjectName;
                    model.Rate = rate;
                    model.WebsiteOwner = bllUser.WebsiteOwner;
                    model.ProjectType = "DistributionOffLine";
                    model.Remark = "三级分销佣金";
                    model.CommissionLevel = "3";
                    if (!Add(model, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }

            }
            #endregion
            int result = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSql.ToString(), tran);
            if (result > 0)
            {
                tran.Commit();
                return true;
            }
            else
            {
                msg = "操作失败";
                tran.Rollback();
                return false;
            }


        }

        #endregion

        #region 项目操作日志

        /// <summary>
        /// 查询项目日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public List<ProjectLog> QueryProjectLogList(int pageIndex, int pageSize, string keyWord = "")
        {

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}'", WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And ProjectName like '%{0}%'", keyWord);

            }
            return GetLit<ProjectLog>(pageSize, pageIndex, sbWhere.ToString(), "InsertDate DESC");

        }
        /// <summary>
        /// 添加项目操作日志
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="projectName"></param>
        /// <param name="projectIntro"></param>
        /// <param name="amount"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddProjectLog(int projectId, string userId, string projectName, string projectIntro, decimal amount, string status, string remark = "")
        {
            ProjectLog model = new ProjectLog();
            model.ProjectId = projectId;
            model.UserId = userId;
            model.ProjectName = projectName;
            model.InsertDate = DateTime.Now;
            model.Status = status;
            model.WebsiteOwner = WebsiteOwner;
            model.Remark = remark;
            return Add(model);

        }



        #endregion

        #region 项目状态模块
        /// <summary>
        /// 查询项目状态
        /// </summary>
        /// <returns></returns>
        public List<WXMallOrderStatusInfo> QueryProjectStatusList(string moduleType="")
        {
            string type = "DistributionOffLine";
            if (!string.IsNullOrEmpty(moduleType)) type = moduleType;
            return GetList<WXMallOrderStatusInfo>(string.Format(" WebsiteOwner='{0}' And StatusType='{1}' Order By Sort DESC", WebsiteOwner,type));


        }

        /// <summary>
        /// 添加项目状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool AddProjectStatus(string status, string sort = "", string action = "",string module="")
        {
            WXMallOrderStatusInfo model = new WXMallOrderStatusInfo();
            model.OrderStatu = status;
            model.StatusAction = action;
            if (!string.IsNullOrEmpty(sort)) model.Sort = int.Parse(sort);
            model.WebsiteOwner = WebsiteOwner;
            model.StatusType = "DistributionOffLine";
            if (!string.IsNullOrEmpty(module)) model.StatusType = module;
        
            return Add(model);

        }

        /// <summary>
        /// 添加项目状态
        /// </summary>
        /// <param name="autoId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateProjectStatus(int autoId, string status = "", string sort = "", string action = "", string module = "")
        {
            WXMallOrderStatusInfo model = GetProjectStatus(autoId);
            model.OrderStatu = status;
            model.StatusAction = action;
            model.StatusType = module;
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = int.Parse(sort);
            }
            if (action == "DistributionOffLineCommission")//执行分销动作
            {
                Update(new WXMallOrderStatusInfo(), string.Format("StatusAction=''"), string.Format(" WebsiteOwner='{0}' And StatusType='DistributionOffLine'", WebsiteOwner));
            }
            return Update(model);

        }

        /// <summary>
        /// 获取单个项目状态
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public WXMallOrderStatusInfo GetProjectStatus(int autoId)
        {
            return Get<WXMallOrderStatusInfo>(string.Format(" AutoID={0}", autoId));

        }

        /// <summary>
        /// 删除项目状态
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public int DeleteProjectStatus(string autoId)
        {
            return Delete(new WXMallOrderStatusInfo(), string.Format(" AutoID ={0}", autoId));

        }





        #endregion

        #region 用户等级模块
        /// <summary>
        /// 查询用户等级
        /// </summary>
        /// <returns></returns>
        public List<UserLevelConfig> QueryUserLevelList()
        {
            return GetList<UserLevelConfig>(string.Format(" WebsiteOwner='{0}' And LevelType='DistributionOffLine'", WebsiteOwner));


        }

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public bool AddUserLevel(string status)
        //{
        //    UserLevelConfig model = new UserLevelConfig();
        //    model.OrderStatu = status;
        //    model.WebsiteOwner = WebsiteOwner;
        //    model.StatusType = "DistributionOffLine";
        //    return Add(model);

        //}

        ///// <summary>
        ///// 更新
        ///// </summary>
        ///// <param name="autoId"></param>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public bool UpdateUserLevel(int autoId, string status)
        //{
        //    UserLevelConfig model = GetProjectStatus(autoId);
        //    model.OrderStatu = status;
        //    return Update(model);

        //}

        ///// <summary>
        ///// 获取
        ///// </summary>
        ///// <param name="autoId"></param>
        ///// <returns></returns>
        //public UserLevelConfig GetUserLevel(int autoId)
        //{
        //    return Get<UserLevelConfig>(string.Format(" AutoID={0}", autoId));

        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public int DeleteUserLevel(string autoId)
        {
            return Delete(new UserLevelConfig(), string.Format(" AutoID ={0}", autoId));

        }

        /// <summary>
        /// 根据级数及用户获取提成比例
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDistributionRate(UserInfo userInfo, int level)
        {
            UserLevelConfig levelConfig = GetUserLevel(userInfo);
            switch (level)
            {
                case 0://直销比例
                    return int.Parse(levelConfig.DistributionRateLevel0);
                case 1://一级分销比例
                    return int.Parse(levelConfig.DistributionRateLevel1);
                case 2://二级分销比例
                    return int.Parse(levelConfig.DistributionRateLevel2);
                case 3://三级分销比例
                    return int.Parse(levelConfig.DistributionRateLevel3);
                default:
                    break;
            }

            return 0;
        }
        /// <summary>
        /// 获取用户对应等级
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public UserLevelConfig GetUserLevel(UserInfo userInfo)
        {
            UserLevelConfig level = new UserLevelConfig();
            level.DistributionRateLevel0 = "0";
            level.DistributionRateLevel1 = "0";
            level.DistributionRateLevel2 = "0";
            level.DistributionRateLevel3 = "0";
            level.LevelString = "";
            foreach (var item in QueryUserLevelList())
            {
                if ((double)userInfo.HistoryDistributionOffLineTotalAmount >= item.FromHistoryScore && (double)userInfo.HistoryDistributionOffLineTotalAmount < item.ToHistoryScore)
                {

                    return item;
                }
            }
            return level;
        }

        public UserLevelConfig GetUserLevel(UserInfo userInfo, out string nextLevelName, out double distanceNextLevelScore)
        {
            nextLevelName = "";
            distanceNextLevelScore = 0;

            UserLevelConfig level = new UserLevelConfig();
            level.DistributionRateLevel0 = "0";
            level.DistributionRateLevel1 = "0";
            level.DistributionRateLevel2 = "0";
            level.DistributionRateLevel3 = "0";
            level.LevelString = "";

            var ls = QueryUserLevelList();

            for (int i = 0; i < ls.Count; i++)
            {
                var item = ls[i];
                if ((double)userInfo.HistoryDistributionOffLineTotalAmount >= item.FromHistoryScore && (double)userInfo.HistoryDistributionOffLineTotalAmount < item.ToHistoryScore)
                {
                    level = item;

                    if (i < ls.Count - 1)
                    {
                        nextLevelName = ls[i + 1].LevelString;
                        distanceNextLevelScore = ls[i + 1].FromHistoryScore - (double)userInfo.HistoryDistributionOffLineTotalAmount;
                    }

                }
            }

            return level;
        }


        #endregion

        #region 银行卡
        /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<BindBankCard> GetBindBankCardList(int pageSize, int pageIndex, string keyword, string userId, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" UserId='{0}' ", userId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" AND (AccountName like '%{0}%' or BankAccount like '%{0}%' or BankName like '%{0}%')", keyword);
            }
            totalCount = GetCount<BindBankCard>(sbWhere.ToString());
            return GetLit<BindBankCard>(pageSize, pageIndex, sbWhere.ToString(), " AutoID Desc");
        }
        /// <summary>
        /// 获取银行卡详情
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public BindBankCard GetBindBankCard(int autoId)
        {
            return Get<BindBankCard>(string.Format(" AutoID={0} ", autoId));
        }
        #endregion

        #region 设置相关
        /// <summary>
        /// 获取线下分销申请活动ID
        /// </summary>
        /// <returns></returns>
        public string GetDistributionOffLineApplyActivityID()
        {
            var ws = GetWebsiteInfoModelFromDataBase();
            bool isCreate = false;

            if (string.IsNullOrWhiteSpace(ws.DistributionOffLineApplyActivityID))
            {
                isCreate = true;
            }
            else
            {
                //判断活动是否存在
                BLLActivity bllActivity = new BLLActivity(GetCurrUserID());
                var activity = bllActivity.GetActivityInfoByActivityID(ws.DistributionOffLineApplyActivityID);
                if (activity == null)
                    isCreate = true;
            }

            if (isCreate)
            {
                ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                try
                {
                    //创建数据活动

                    ActivityInfo model = new ActivityInfo();
                    model.ActivityName = "线下业务分销员资格申请";
                    model.ActivityDescription = "本活动由系统自动创建：线下业务分销员";
                    model.ActivityStatus = 1;
                    model.ActivityID = GetGUID(TransacType.ActivityAdd);

                    model.UserID = ws.WebsiteOwner;
                    model.WebsiteOwner = ws.WebsiteOwner;

                    model.ActivityDate = DateTime.Now;

                    //TODO：设置通知的客服


                    if (Add(model, tran))
                    {
                        ws.DistributionOffLineApplyActivityID = model.ActivityID;

                        if (Update(ws, tran))
                        {
                            //添加默认分销申请字段：姓名、手机、邮箱、公司、职位、年龄、推荐码（默认）

                            List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>()
                            {
                                new ActivityFieldMappingInfo()
                                {
                                    ActivityID = model.ActivityID,
                                    ExFieldIndex = 1,
                                    FieldIsDefauld = 0,
                                    FieldType = 0,
                                    FormatValiFunc = "email",
                                    MappingName = "邮箱"
                                    
                                },
                                new ActivityFieldMappingInfo()
                                {
                                    ActivityID = model.ActivityID,
                                    ExFieldIndex = 2,
                                    FieldIsDefauld = 0,
                                    FieldType = 0,
                                    MappingName = "公司"
                                },
                                new ActivityFieldMappingInfo()
                                {
                                    ActivityID = model.ActivityID,
                                    ExFieldIndex = 3,
                                    FieldIsDefauld = 0,
                                    FieldType = 0,
                                    MappingName = "职位"
                                },
                                new ActivityFieldMappingInfo()
                                {
                                    ActivityID = model.ActivityID,
                                    ExFieldIndex = 4,
                                    FieldIsDefauld = 0,
                                    FieldType = 0,
                                    MappingName = "年龄"
                                }
                            };

                            if (!AddList(fieldData))
                            {
                                tran.Rollback();

                                return "";
                            };

                            tran.Commit();
                            System.Web.HttpContext.Current.Session["WebsiteInfoModel"] = ws;

                        }
                        else
                        {
                            tran.Rollback();
                            return "";
                        }

                    }
                    else
                    {
                        tran.Rollback();
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    throw ex;

                }


            }

            return ws.DistributionOffLineApplyActivityID;
        }


        /// <summary>
        /// 获取站点设置的分销级别 1-3
        /// </summary>
        /// <returns></returns>
        public int GetDistributionLevel()
        {
            return GetWebsiteInfoModelFromDataBase().DistributionOffLineLevel;
        }
        #endregion



        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="statusInt"></param>
        /// <returns></returns>
        public string ConvertStatus(int statusInt)
        {
            string status = "";
            switch (statusInt)
            {
                case 0:
                    status = "待审核";
                    break;
                case 1:
                    status = "已受理";
                    break;
                case 2:
                    status = "已打款";
                    break;
                case 3:
                    status = "提现失败";
                    break;
                default:
                    break;
            }
            return status;
        }

        /// <summary>
        /// 微转发自动申请分销员
        /// </summary>
        /// <param name="currentUserInfo">当前用户信息</param>
        /// <param name="activityInfo">转发活动信息</param>
        /// <param name="disActivityInfo">分销活动信息</param>
        /// <param name="recommendUserInfo">推荐用户信息信息</param>
        /// <param name="data">微转发报名信息</param>
        /// <returns></returns>
        public bool AutoApply(UserInfo currentUserInfo, ActivityInfo activityInfo, ActivityInfo disActivityInfo, UserInfo recommendUserInfo, ActivityDataInfo data)
        {
            if (currentUserInfo.UserID==recommendUserInfo.UserID)
            {
                return false;
            }
            if (string.IsNullOrEmpty(recommendUserInfo.DistributionOffLinePreUserId))
            {
                 return false;
            }
            if (!string.IsNullOrEmpty(currentUserInfo.DistributionOffLinePreUserId))
            {
                return false;
            }
            ActivityDataInfo applyData = bllActivity.GetActivityDataInfo(disActivityInfo.ActivityID, currentUserInfo.UserID);
            if (applyData != null)
            {
                return false;//已经自动申请过了
            }
            ActivityDataInfo model = new ActivityDataInfo();
            var newActivityUID = 1001;
            var lastActivityDataInfo = Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", disActivityInfo.ActivityID));
            if (lastActivityDataInfo != null)
            {
                newActivityUID = lastActivityDataInfo.UID + 1;
            }
            model.UID = newActivityUID;
            model.ActivityID = disActivityInfo.ActivityID;
            model.SpreadUserID = recommendUserInfo.UserID;
            model.UserId = currentUserInfo.UserID;
            model.WebsiteOwner = WebsiteOwner;
            model.InsertDate = DateTime.Now;
            model.WeixinOpenID = currentUserInfo.WXOpenId;
            model.Name = data.Name;
            model.Phone = data.Phone;
            model.DistributionOffLineRecommendCode = recommendUserInfo.AutoID.ToString();
            model.K59 = recommendUserInfo.TrueName;//推荐人
            model.K60 = activityInfo.ActivityName;//活动名称

            //其它信息补足
            var activityMap = bllActivity.GetActivityFieldMappingList(activityInfo.ActivityID);
            var disMap = bllActivity.GetActivityFieldMappingList(disActivityInfo.ActivityID);
            Type type = model.GetType();
            Type typeSource = data.GetType();
            foreach (var item in disMap.Where(p=>p.FieldName!="Name"&&p.FieldName!="Phone"))
            {
                if (activityMap.Where(p => p.MappingName == item.MappingName).Count() > 0)
                {

                    var mapName = activityMap.Where(p => p.MappingName == item.MappingName).First();//公司 职位等
                    var value = typeSource.GetProperty("K" + mapName.ExFieldIndex).GetValue(data, null);
                    type.GetProperty("K" + item.ExFieldIndex).SetValue(model, value, null);
                }

            }
            return Add(model);

        }

    }
}
