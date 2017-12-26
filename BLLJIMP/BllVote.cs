using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 投票逻辑
    /// </summary>
    public class BLLVote : BLL
    {
        BLLUser bllUser = new BLLUser("");
        /// <summary>
        /// 投票BLL
        /// </summary>
        public BLLVote()
            : base()
        {

        }
        /// <summary>
        /// 新建投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddVoteInfo(VoteInfo model)
        {

            return Add(model);


        }
        /// <summary>
        /// 获取单个投票信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VoteInfo GetVoteInfo(int id)
        {

            return Get<VoteInfo>(string.Format("AutoID={0}", id));
        }
        ///// <summary>
        ///// 获取单个投票对象信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public VoteObjectInfo GetSingleVoteObjectInfo(int id)
        //{

        //    return Get<VoteObjectInfo>(string.Format("AutoID={0}", id));
        //}
        ///// <summary>
        ///// 获取单个投票对象信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public VoteObjectInfo GetVoteObjectInfo(int id)
        //{

        //    return Get<VoteObjectInfo>(string.Format("AutoID={0}", id));
        //}


        /// <summary>
        /// 获取投票列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="sort"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<VoteInfo> GetVoteInfoList(int pageIndex, int pageSize, string keyWord, string sort, out int totalCount)
        {
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'", WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND VoteName like '%{0}%'", keyWord);
            }
            totalCount = GetCount<VoteInfo>(sbWhere.ToString());
            string orderBy = "";
            switch (sort)
            {
                case "time_asc":
                    orderBy = " StopDate ASC ";
                    break;
                case "time_desc":
                    orderBy = " StopDate ASC ";
                    break;
                default:
                    orderBy = " VoteStatus DESC";
                    break;
            }
            return GetLit<VoteInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
        }


        /// <summary>
        /// 修改投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateVoteInfo(VoteInfo model)
        {
            return Update(model);


        }
        /// <summary>
        /// 删除投票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteVoteInfo(VoteInfo model)
        {
            return Delete(model) > 0;


        }

        /// <summary>
        /// 新建投票对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddVoteObjectInfo(VoteObjectInfo model)
        {

            return Add(model);


        }
        /// <summary>
        /// 修改投票对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateVoteObjectInfo(VoteObjectInfo model)
        {
            return Update(model);


        }
        /// <summary>
        /// 修改投票对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteVoteObjectInfo(VoteObjectInfo model)
        {
            return Delete(model) > 0;


        }

        /// <summary>
        /// 新增投票记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddVoteLogInfo(VoteLogInfo model)
        {

            return Add(model);


        }
        /// <summary>
        /// 获取投票对象列表 按得票数排名
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<VoteObjectInfo> GetVoteObjectInfoList(int voteId, int rows)
        {

            return GetLit<VoteObjectInfo>(rows, 1, string.Format("VoteID={0}", voteId), "VoteCount DESC");

        }
        /// <summary>
        /// 获取投票对象列表 分页 搜索条件
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<VoteObjectInfo> GetVoteObjectInfoList(int voteId, int pageIndex, int pageSize, out int totalCount, string keyWord = "", string voteObjectNumber = "", string status = "", string orderByReq = "", string area = "")
        {

            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format("VoteID={0}", voteId));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And  (VoteObjectName like '%{0}%' Or Number = '{0}' Or Contact like '%{0}%')", keyWord);
            }
            if (!string.IsNullOrEmpty(voteObjectNumber))
            {
                sbWhere.AppendFormat(" And  Number='{0}'", voteObjectNumber);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status={0}", status);
            }
            if (!string.IsNullOrEmpty(area))
            {
                sbWhere.AppendFormat(" And Area='{0}'", area);
            }
            string orderBy = "Rank ASC,VoteCount DESC,AutoID ASC";
            if (!string.IsNullOrEmpty(orderByReq))
            {
                switch (orderByReq)
                {
                    case "time_desc":
                        orderBy = "AutoID DESC,VoteObjectName ASC";//按照时间降序
                        break;
                    case "time_asc":
                        orderBy = "AutoID ASC,VoteObjectName ASC";//按照时间升序
                        break;
                    case "rank_desc":
                        orderBy = "Rank DESC,AutoID ASC";//按照排名降序
                        break;
                    case "rank_asc":
                        orderBy = "Rank ASC,AutoID DESC";//按照排名升序
                        break;
                    default:
                        break;
                }


            }

            //if (!string.IsNullOrEmpty(status))
            //{
            //    totalCount = GetCount<VoteObjectInfo>(string.Format(" VoteID={0} And Status={1}", voteId,status));
            //}
            //else
            //{
            //    totalCount = GetCount<VoteObjectInfo>(string.Format(" VoteID={0}", voteId));
            //}
            totalCount = GetCount<VoteObjectInfo>(sbWhere.ToString());
            return GetLit<VoteObjectInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);

        }
        /// <summary>
        /// 检查用户是否可以建立投票  true 可以新建 false 不能新建
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckUserCanCreateVote(string userId)
        {

            if (userId.Equals(WebsiteOwner))//所有者无限制
            {
                return true;
            }
            BLLUser bllUser = new BLLUser("");

            var maxVoteCount = bllUser.GetUserInfo(userId).VoteCount;

            if (maxVoteCount == 0) return true;

            if (GetUserVoteCount(userId) < maxVoteCount)
            {
                return true;
            }

            return false;

        }
        /// <summary>
        /// 获取用户已经建立的投票数
        /// </summary>
        /// <returns></returns>
        public int GetUserVoteCount(string userId)
        {
            return GetCount<VoteInfo>(string.Format(" CreateUserID='{0}'", userId));
        }


        /// <summary>
        /// 获取某个投票的 最大投票编号
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public int GetVoteObjectMaxNumber(int voteId)
        {

            int maxNumber = 0;
            object result = GetSingle(string.Format("Select Max(CAST(Number as INTEGER)) from ZCJ_VoteObjectInfo where  VoteID={0}", voteId));
            if (result != null)
            {
                maxNumber = Convert.ToInt32(result);
            }
            return maxNumber;

        }

        ///// <summary>
        ///// 检查编号是否已经存在 编辑
        ///// </summary>
        ///// <param name="voteID"></param>
        ///// <param name="voteObjectId"></param>
        ///// <param name="number"></param>
        ///// <returns></returns>
        //public bool IsExitsVoteObjectNumber(int voteID,int voteObjectId,string number) {

        //    return GetCount<VoteObjectInfo>(string.Format(" VoteID={0} And Number='{1}' And AutoID !={2}", voteID, number, voteObjectId))>0;

        //}
        /// <summary>
        /// 检查编号是否已经存在 添加
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="voteObjectId"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsExitsVoteObjectNumber(int voteId, string number)
        {

            return GetCount<VoteObjectInfo>(string.Format(" VoteID={0} And Number='{1}'", voteId, number)) > 0;

        }

        ///// <summary>
        ///// 更新投票数 OLD VERSION
        ///// </summary>
        ///// <returns></returns>
        //public bool UpdateVoteObjectVoteCount(int voteId,int voteObjectId,string userId,int count){
        //    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
        //    try
        //    {
        //        VoteInfo VoteInfo = GetSingleVoteInfo(voteId);
        //        int VoteLogCount = GetVoteLogCount(voteId, userId);
        //        int AddCount = 0;
        //        if (VoteLogCount<=VoteInfo.FreeVoteCount)
        //        {
        //            AddCount = VoteInfo.FreeVoteCount - VoteLogCount;
        //        }

        //        if (VoteInfo==null||VoteInfo.VoteStatus.Equals(0))
        //        {
        //            return false;
        //        }

        //        bool logresult = false;
        //        int logcount = 0;
        //        for (int i =1; i <=count; i++)
        //        {
        //            VoteLogInfo log = new VoteLogInfo();
        //            log.UserID = userId;
        //            log.VoteID = voteId;
        //            log.VoteObjectID = voteObjectId;
        //            log.InsertDate = DateTime.Now;
        //            log.CreateUserID = VoteInfo.CreateUserID;
        //            log.WebsiteOwner = WebsiteOwner;
        //            log.VoteCount = count;
        //            if (Add(log,tran))
        //            {
        //                logcount++;
        //                VoteLogCount++;

        //            }

        //        }
        //        if (count!=logcount)
        //        {
        //            tran.Rollback();
        //            return false;
        //        }
        //        else
        //        {
        //            logresult = true;
        //        }

        //        if (logresult)
        //        {
        //            VoteObjectInfo model = GetSingleVoteObjectInfo(voteObjectId);
        //            model.VoteCount += count;
        //            if (Update(model))
        //            {

        //                if (VoteInfo.IsFree.Equals(0))//收费 检查是否需要扣除用户剩余票数
        //                {

        //                    int FreeVoteCount = VoteInfo.FreeVoteCount-VoteLogCount;
        //                    //计算应该扣除的点数



        //                    //计算应该扣除的点数
        //                    if (FreeVoteCount<0)//剩余票数不够，减掉用户购买的的票数
        //                    {

        //                        //
        //                        UserInfo userInfo = new BLLUser("").GetUserInfo(userId);
        //                        userInfo.AvailableVoteCount -=count-AddCount;

        //                        if (userInfo.AvailableVoteCount<0)
        //                        {
        //                            userInfo.AvailableVoteCount = 0;
        //                        }
        //                        if (Update(userInfo,tran))
        //                        {
        //                            tran.Commit();
        //                            return true;
        //                        }
        //                        else
        //                        {
        //                            tran.Rollback();
        //                            return false;
        //                        }


        // /                    }
        //                    else
        //                    {
        //                        tran.Commit();
        //                        return true;

        //                    }


        //                }
        //                else
        //                {
        //                    tran.Commit();
        //                    return true;

        //                }


        //            }
        //            else
        //            {
        //                tran.Rollback();
        //                return false;

        //            }
        //        }
        //        else
        //        {
        //            tran.Rollback();
        //            return false;

        //        }

        //    }
        //    catch (Exception)
        //    {

        //        tran.Rollback();
        //        return false;
        //    }


        //}


        /// <summary>
        /// 更新投票数 20140917
        /// </summary>
        /// <returns></returns>
        public bool UpdateVoteObjectVoteCount(int voteId, int voteObjectId, string userId, int count)
        {

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                VoteInfo voteInfo = GetVoteInfo(voteId);





                //int voteLogCount = GetVoteLogCount(voteId, userId);
                //int addCount = 0;
                //if (voteLogCount <= voteInfo.FreeVoteCount)
                //{
                //    addCount = voteInfo.FreeVoteCount - voteLogCount;
                //}

                if (voteInfo == null || voteInfo.VoteStatus.Equals(0))
                {
                    return false;
                }

                bool logResult = false;//是否可以投票标识
                VoteLogInfo log = new VoteLogInfo();
                log.UserID = userId;
                log.VoteID = voteId;
                log.VoteObjectID = voteObjectId;
                log.InsertDate = DateTime.Now;
                log.CreateUserID = voteInfo.CreateUserID;
                log.WebsiteOwner = WebsiteOwner;
                log.VoteCount = count;
                log.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                log.IPLocation = Common.MySpider.GetIPLocation(log.IP);
                if (!Add(log, tran))
                {

                    tran.Rollback();
                    return false;
                }
                else
                {
                    //voteLogCount += count;
                    logResult = true;
                }




                if (logResult)
                {
                    VoteObjectInfo model = GetVoteObjectInfo(voteObjectId);
                    model.VoteCount += count;
                    if (Update(model))
                    {

                        //if (voteInfo.IsFree.Equals(0))//收费 检查是否需要扣除用户剩余票数
                        //{

                        //    int freeVoteCount = voteInfo.FreeVoteCount - voteLogCount;
                        //    //计算应该扣除的点数



                        //    //计算应该扣除的点数
                        //    if (freeVoteCount < 0)//剩余票数不够，减掉用户购买的的票数
                        //    {

                        //        //
                        //        UserInfo userInfo =  bllUser.GetUserInfo(userId);
                        //        userInfo.AvailableVoteCount -= count - addCount;
                        //        if (userInfo.AvailableVoteCount < 0)
                        //        {
                        //            userInfo.AvailableVoteCount = 0;
                        //        }
                        //        if (Update(userInfo, string.Format(" AvailableVoteCount={0}", userInfo.AvailableVoteCount), string.Format(" AutoID={0}", userInfo.AutoID), tran) > 0)
                        //        {
                        //            tran.Commit();
                        //            return true;
                        //        }
                        //        else
                        //        {
                        //            tran.Rollback();
                        //            return false;
                        //        }


                        //    }
                        //    else
                        //    {
                        //        tran.Commit();
                        //        return true;

                        //    }


                        //}
                        //else
                        //{
                        tran.Commit();
                        return true;

                        //}


                    }
                    else
                    {
                        tran.Rollback();
                        return false;

                    }
                }
                else
                {
                    tran.Rollback();
                    return false;

                }

            }
            catch (Exception)
            {

                tran.Rollback();
                return false;
            }


        }

        /// <summary>
        /// 投票
        /// </summary>
        /// <param name="voteId">投票ID</param>
        /// <param name="voteObjectId">选手ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="count">票数</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public bool UpdateVoteObjectVoteCount(int voteId, int voteObjectId, string userId, int count, out string msg)
        {
            msg = "false";
            VoteInfo voteInfo = GetVoteInfo(voteId);
            if (voteInfo.VoteStatus.Equals(0))
            {
                msg = "投票停止";
                return false;

            }
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now >= (DateTime.Parse(voteInfo.StopDate)))
                {

                    msg = "投票结束";
                    return false;

                }
            }

            VoteObjectInfo voteObj = GetVoteObjectInfo(voteObjectId);
            if (!voteObj.Status.Equals(1))
            {

                msg = "审核通过的选手才能投票";
                return false;
            }
            //检查是否可以投票
            if (voteInfo.LimitType == 0)//每人最多可以投多少票
            {
                if ((GetCount<VoteLogInfo>(string.Format(" VoteId={0} And UserID='{1}' ", voteInfo.AutoID, userId)) >= voteInfo.FreeVoteCount))
                {
                    msg = "您已经投过票了";//总票数已经用完
                    return false;
                }
            }
            else if (voteInfo.LimitType == 1)//每人每天可以投多少票
            {

                string toDay = DateTime.Now.ToString("yyyy-MM-dd");
                string tomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                if (GetCount<VoteLogInfo>(string.Format(" VoteId={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}'", voteInfo.AutoID, userId, toDay, tomorrow)) >= voteInfo.FreeVoteCount)
                {
                    msg = "今天票数已经用完,明天再来吧";//总票数已经用完
                    return false;
                }

            }
            if ((voteInfo.LimitType==0)&& GetCount<VoteLogInfo>(string.Format(" VoteId={0} And UserID='{1}' And VoteObjectID={2}", voteInfo.AutoID, userId, voteObjectId)) >= voteInfo.VoteObjectLimitVoteCount)
            {

                msg = "您已经投过票了";//每个选手最多可以投多少票
                return false;
            }
            //检查是否可以投票
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {


                VoteLogInfo log = new VoteLogInfo();
                log.UserID = userId;
                log.VoteID = voteId;
                log.VoteObjectID = voteObjectId;
                log.InsertDate = DateTime.Now;
                log.CreateUserID = voteInfo.CreateUserID;
                log.WebsiteOwner = WebsiteOwner;
                log.VoteCount = count;
                log.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                log.IPLocation = Common.MySpider.GetIPLocation(log.IP);
                if (!Add(log, tran))
                {

                    tran.Rollback();
                    return false;
                }
               
                if (Update(voteObj, string.Format(" VoteCount+={0}", count), string.Format(" AutoID={0}",voteObj.AutoID)) > 0)
                {

                    tran.Commit();
                    msg = "投票成功";
                    return true;

                }
                else
                {
                    tran.Rollback();
                    return false;

                }


            }
            catch (Exception ex)
            {

                tran.Rollback();
                msg = ex.Message;
                return false;
            }


        }


        ///// <summary>
        ///// 获取用户可用的投票数
        ///// </summary>
        ///// <param name="voteId"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public int GetCanUseVoteCount(int voteId,string userId) {
        //    VoteInfo VoteInfo = GetSingleVoteInfo(voteId);
        //    int logcount = GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}'", voteId, userId));
        //    if (VoteInfo.IsFree.Equals(1))//免费
        //    {
        //        int count=VoteInfo.FreeVoteCount - logcount;
        //        if (count<0)
        //        {
        //            count=0;
        //        }
        //        return count;
        //    }
        //    else//收费
        //    {
        //        UserInfo userInfo = new BLLUser("").GetUserInfo(userId);
        //        if (userInfo.AvailableVoteCount==null)
        //        {
        //            userInfo.AvailableVoteCount = 0;
        //        }
        //        int count = VoteInfo.FreeVoteCount - logcount;//免费剩余票数
        //        if (count<0)
        //        {
        //            count = 0;
        //        }

        //        count += (int)userInfo.AvailableVoteCount;

        //        return count;

        //    }


        //}


        /// <summary> 
        /// 获取用户可用的投票数 20140917
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetCanUseVoteCount(int voteId, string userId)
        {
            VoteInfo voteInfo = GetVoteInfo(voteId);
            int logCount = 0;
            object objLogCount = ZentCloud.ZCDALEngine.DALEngine.GetSingle(string.Format("select sum(ISNULL(VoteCount,0)) from ZCJ_VoteLogInfo where VoteID={0} And UserID='{1}'", voteId, userId));
            if (objLogCount != null)
            {
                logCount = (int)objLogCount;
            }
            if (voteInfo.IsFree.Equals(1))//免费
            {
                int count = voteInfo.FreeVoteCount - logCount;
                if (count < 0)
                {
                    count = 0;
                }
                return count;
            }
            else//收费
            {
                UserInfo userInfo = bllUser.GetUserInfo(userId);
                if (userInfo.AvailableVoteCount == null)
                {
                    userInfo.AvailableVoteCount = 0;
                }
                int count = voteInfo.FreeVoteCount - logCount;//免费剩余票数
                if (count < 0)
                {
                    count = 0;
                }

                count += (int)userInfo.AvailableVoteCount;

                return count;

            }


        }
        //        /// <summary>
        ///// 获取投票记录数
        ///// </summary>
        ///// <param name="voteId"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public int GetVoteLogCount(int voteId, string userId) { 

        // return GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}'", voteId, userId));

        //}
        /// <summary>
        /// 获取投票记录数 20140917
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetVoteLogCount(int voteId, string userId)
        {
            int logCount = 0;
            object objLogCount = ZentCloud.ZCDALEngine.DALEngine.GetSingle(string.Format("select sum(ISNULL(VoteCount,0)) from ZCJ_VoteLogInfo where VoteID={0} And UserID='{1}'", voteId, userId));
            if (objLogCount != null)
            {
                logCount = (int)objLogCount;
            }
            return logCount;

        }



        /// <summary>
        /// 检查是否可以投票
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsCanVote(int voteId, string userId, int count)
        {
            VoteInfo voteInfo = GetVoteInfo(voteId);
            if (voteInfo.IsFree.Equals(1))//免费
            {
                if (GetCanUseVoteCount(voteId, userId) - count < 0)
                {
                    return false;
                }
            }
            else//收费
            {
                if (GetCanUseVoteCount(voteId, userId) - count < 0)
                {
                    return false;
                }
            }

            return true;

        }


        /// <summary>
        /// 获取某个投票的所有充值设置
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public List<VoteRecharge> GetVoteRechargeList(int voteId)
        {

            return GetList<VoteRecharge>(string.Format("VoteId={0}", voteId));
        }
        /// <summary>
        /// 获取最后的投票信息
        /// </summary>
        /// <returns></returns>
        public VoteInfo GetLastVoteInfo()
        {
            return Get<VoteInfo>(string.Format(" WebsiteOwner='{0}' Order by AutoID DESC", WebsiteOwner));

        }
        ///// <summary>
        ///// 重置用户剩余投票数(投票为免费的情况下有效)
        ///// </summary>
        ///// <returns></returns>
        //public void UpdateUserVoteCount(int voteId, UserInfo userInfo)
        //{
        //    DateTime dtNow = DateTime.Now;
        //    if (userInfo.LastUpdateVoteCountTime == null)
        //    {
        //        userInfo.LastUpdateVoteCountTime = dtNow;
        //        Update(userInfo, string.Format(" LastUpdateVoteCountTime='{0}'", userInfo.LastUpdateVoteCountTime), string.Format(" AutoID={0}", userInfo.AutoID));
        //    }
        //    TimeSpan ts = (Convert.ToDateTime(dtNow.ToString("yyyy-MM-dd")) - Convert.ToDateTime(((DateTime)userInfo.LastUpdateVoteCountTime).ToString("yyyy-MM-dd")));
        //    if (ts.TotalDays >= 1)//超过一天，投票记录清空
        //    {
        //        //清空投票记录
        //        int count = Delete(new VoteLogInfo(), string.Format("VoteID ={0} And UserID='{1}'", voteId, userInfo.UserID));
        //        userInfo.LastUpdateVoteCountTime = dtNow;//
        //        Update(userInfo, string.Format(" LastUpdateVoteCountTime='{0}'", userInfo.LastUpdateVoteCountTime), string.Format(" AutoID={0}", userInfo.AutoID));
        //    }




        //}

        /// <summary>
        /// 根据投票ID 与创建用户查询
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VoteObjectInfo GetVoteObjectInfo(int voteId, string userId)
        {
            return Get<VoteObjectInfo>(string.Format(" VoteID={0} And CreateUserId='{1}'", voteId, userId));
        }
        /// <summary>
        /// 根据选手编号查询选手
        /// </summary>
        /// <param name="voteid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public VoteObjectInfo GetVoteObjectInfo(int autoId)
        {
            return Get<VoteObjectInfo>(string.Format(" AutoID={0} ", autoId));
        }

        /// <summary>
        /// 查询排名第一的选手
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public VoteObjectInfo GetFirstRankVoteObjectInfo(int voteId)
        {
            return Get<VoteObjectInfo>(string.Format(" VoteID={0} And Rank=1", voteId));
        }

        /// <summary>
        /// 更新选手排名
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public bool UpdateVoteObjectRank(int voteId, string status = "")
        {
            StringBuilder sbWhere = new StringBuilder(string.Format(" VoteID={0}", voteId));
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status={0}", status);
            }
            sbWhere.AppendFormat(" Order by VoteCount DESC,AutoID ASC ");
            List<VoteObjectInfo> List = GetList<VoteObjectInfo>(sbWhere.ToString());
            for (int i = 0; i < List.Count; i++)
            {
                List[i].Rank = i + 1;
                if (!Update(List[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取TOTEMA 报名链接或我的报名
        /// </summary>
        /// <returns></returns>
        public string GetTotemaApplyLingOrMyClassLink()
        {
            string link = "Apply.aspx";
            if (IsLogin)
            {
                var model = GetVoteObjectInfo(TotemaVoteID, GetCurrUserID());
                if (model != null)
                {
                    return "MyClass.aspx";
                }
            }
            return link;

        }
        /// <summary>
        /// 获取沙滩宝贝报名链接或我的资料链接
        /// </summary>
        /// <returns></returns>
        public string GetBeachHoneySignUpLink()
        {
            string link = "SignUp.aspx";
            if (IsLogin)
            {
                var model = GetVoteObjectInfo(BeachHoneyVoteID, GetCurrUserID());
                if (model != null)
                {
                    return "MySignUp.aspx";
                }
            }
            return link;

        }
        /// <summary>
        /// 获取沙滩宝贝报名图片或我的报名图片
        /// </summary>
        /// <returns></returns>
        public string GetBeachHoneySignImg()
        {
            string img = "signup.jpg";
            if (IsLogin)
            {
                var model = GetVoteObjectInfo(BeachHoneyVoteID, GetCurrUserID());
                if (model != null)
                {
                    return "signuped.jpg";
                }
            }
            return img;

        }
        /// <summary>
        /// 获取选题投票详情
        /// </summary>
        public TheVoteInfo GetTheVoteInfo(string wesiteOwner, int id)
        {
            return Get<TheVoteInfo>(string.Format(" WebsiteOwner='{0}' AND AutoID={1} ", WebsiteOwner, id));
        }

        /// <summary>
        /// 新建PK模式
        /// </summary>
        /// <param name="voteGroupInfo"></param>
        /// <returns></returns>
        public bool AddVoteGroupInfo(VoteGroupInfo voteGroupInfo)
        {
            return Add(voteGroupInfo);
        }
        /// <summary>
        /// 根据参与者获取PK组
        /// </summary>
        /// <param name="voteGroupMembers"></param>
        /// <returns></returns>
        public VoteGroupInfo GetVoteGroupInfoByGroupMembers(string voteId, string voteGroupMembers)
        {
            return Get<VoteGroupInfo>(string.Format(" VoteId={0} AND VoteGroupMembers='{1}' ", int.Parse(voteId), voteGroupMembers));
        }
        public VoteGroupInfo GetVoteGroupInfoByGroupName(string voteId, string GroupName)
        {
            return Get<VoteGroupInfo>(string.Format(" VoteId={0} AND VoteGroupName='{1}' ", int.Parse(voteId), GroupName));
        }
        public VoteGroupInfo GetVoteGroupInfo(int groupId)
        {
            return Get<VoteGroupInfo>(string.Format(" VoteGroupId={0} ", groupId));
        }
        public int DeleteVoteGroupInfo(VoteGroupInfo model)
        {
            return Delete(model);
        }

        public bool UpdateVoteGroupInfo(VoteGroupInfo model)
        {
            return Update(model);
        }

        /// <summary>
        /// 获取所有组
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public List<VoteGroupInfo> GetVoteAllGroup(int voteId)
        {
            return GetList<VoteGroupInfo>(10000, string.Format(" VoteId = {0} ", voteId), " Sort DESC ");
        }


        ////<summary>
        ////TOTEMA 投票编号 本地
        ////</summary>
        //public int TotemaVoteID = 14;

        ///// <summary>
        ///// totema 投票编号 preview
        ///// </summary>
        //public int TotemaVoteID = 1;

        /// <summary>
        /// totema 投票编号 正式
        /// </summary>
        public int TotemaVoteID = 116;

        /// <summary>
        /// 沙滩宝贝投票ID
        /// </summary>
        public int BeachHoneyVoteID { get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["BeachHoneyVoteID"]); } }

    }
}
