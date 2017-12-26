using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 积分 BLL
    /// </summary>
    public class BllScore : BLL
    {
        BLLUser bllUser = new BLLUser();
        public BllScore()
            : base()
        {

        }

        /// <summary>
        /// 获取积分配置信息
        /// </summary>
        /// <returns></returns>
        public ScoreConfig GetScoreConfig()
        {
            return GetScoreConfig(WebsiteOwner);

        }
        public ScoreConfig GetScoreConfig(string websiteOwner)
        {
            ScoreConfig scoreConfig = Get<ScoreConfig>(string.Format("WebsiteOwner='{0}'", websiteOwner));
            if (scoreConfig == null)
            {
                scoreConfig = new ScoreConfig();

            }
            return scoreConfig;

        }

        /// <summary>
        /// 修改积分配置
        /// </summary>
        /// <param name="orderAmount">满多少元</param>
        /// <param name="orderScore">送多少积分</param>
        /// <param name="exchangeScore">多少积分</param>
        /// <param name="exchangeAmount">抵扣多少元</param>
        /// <returns></returns>
        public bool UpdateScoreConfig(string orderAmount, string orderScore, string exchangeScore, string exchangeAmount)
        {
            ScoreConfig model = GetScoreConfig();
            if (model.AutoID > 0)
            {
                if (!string.IsNullOrEmpty(orderAmount))
                {
                    model.OrderAmount = int.Parse(orderAmount);
                }

                if (!string.IsNullOrEmpty(orderScore))
                {
                    model.OrderScore = int.Parse(orderScore);
                }

                if (!string.IsNullOrEmpty(exchangeAmount))
                {
                    model.ExchangeAmount = decimal.Parse(exchangeAmount);
                }
                if (!string.IsNullOrEmpty(exchangeScore))
                {
                    model.ExchangeScore = int.Parse(exchangeScore);
                }
                if (Update(model))
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                model = new ScoreConfig();

                model.WebsiteOwner = WebsiteOwner;

                if (!string.IsNullOrEmpty(orderAmount))
                {
                    model.OrderAmount = int.Parse(orderAmount);
                }

                if (!string.IsNullOrEmpty(orderScore))
                {
                    model.OrderScore = int.Parse(orderScore);
                }

                if (!string.IsNullOrEmpty(exchangeAmount))
                {
                    model.ExchangeAmount = decimal.Parse(exchangeAmount);
                }
                if (!string.IsNullOrEmpty(exchangeScore))
                {
                    model.ExchangeScore = int.Parse(exchangeScore);
                }
                if (Add(model))
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }



        }

        /// <summary>
        /// 获取积分记录
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="userId">用户Id</param>
        /// <param name="scoreType">积分类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns></returns>
        public List<UserScoreDetailsInfo> GetScoreRecord(out int totalCount, string websiteOwner, string userId = "", string openId = "", string scoreType = "", string serialNumber = "", int pageIndex = 1, int pageSize = 10)
        {


            StringBuilder sbWhere = new StringBuilder("1=1 ");
            if (!string.IsNullOrEmpty(websiteOwner))
            {
                sbWhere.AppendFormat(" And WebsiteOwner='{0}' And Score!=0", websiteOwner);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserID='{0}'", userId);

            }
            if (!string.IsNullOrEmpty(openId))
            {
                sbWhere.AppendFormat(" And OpenId='{0}'", openId);

            }
            if (!string.IsNullOrEmpty(scoreType))
            {
                sbWhere.AppendFormat(" And ScoreType='{0}'", scoreType);
            }
            if (!string.IsNullOrEmpty(serialNumber))
            {
                sbWhere.AppendFormat(" And SerialNumber='{0}'", serialNumber);
            }
            totalCount = GetCount<UserScoreDetailsInfo>(sbWhere.ToString());
            return GetLit<UserScoreDetailsInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoId DESC");


        }
        /// <summary>
        /// 积分转移
        /// </summary>
        /// <param name="fromOpenId">原OpenId</param>
        /// <param name="toOpenId">新OpenId</param>
        /// <returns></returns>
        public bool Move(string websiteOwner, string fromOpenId, string toOpenId, out string msg, string serialNumber, string remark = "")
        {

            msg = "";
            UserInfo fromUserInfo = bllUser.GetUserInfoByOpenId(fromOpenId);
            if (fromUserInfo == null && (websiteOwner == "dongwu" || websiteOwner == "dongwudev"))
            {
                fromUserInfo = bllUser.CreateNewUser(websiteOwner, fromOpenId, "");
            }
            if (fromUserInfo == null)
            {
                msg = "from_openid不存在";
                return false;
            }
            if (fromUserInfo.TotalScore == 0)
            {
                msg = string.Format("{0}积分为0,不能转移", fromOpenId);
                return false;
            }
            UserInfo toUserInfo = bllUser.GetUserInfoByOpenId(toOpenId);
            if (toUserInfo == null && (websiteOwner == "dongwu" || websiteOwner == "dongwudev"))
            {
                toUserInfo = bllUser.CreateNewUser(websiteOwner, toOpenId, "");
            }
            if (toUserInfo == null)
            {
                msg = "to_openid不存在";
                return false;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                string scoreTitle1 = string.Format("积分转移,减掉积分{0}", fromUserInfo.TotalScore);
                string scoreTitle2 = string.Format("积分转移,增加积分{0}", fromUserInfo.TotalScore);
                if (!string.IsNullOrEmpty(remark))
                {
                    scoreTitle1 = remark;
                    scoreTitle2 = remark;
                }
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalScore+=(Select TotalScore from ZCJ_UserInfo where AutoId={0}),HistoryTotalScore+=(Select HistoryTotalScore from ZCJ_UserInfo where AutoId={0}) Where AutoId={1};", fromUserInfo.AutoID, toUserInfo.AutoID);//把旧用户积分转移到新用户下
                sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalScore=0 Where AutoId={0};", fromUserInfo.AutoID);//旧用户积分清零
                sbSql.AppendFormat("Insert Into ZCJ_UserScoreDetailsInfo (UserID,OpenId,Score,AddTime,AddNote,ScoreType,WebSiteOwner,SerialNumber) values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}');", fromUserInfo.UserID, fromOpenId, -fromUserInfo.TotalScore, DateTime.Now.ToString(), scoreTitle1, "Move", WebsiteOwner, serialNumber);
                sbSql.AppendFormat("Insert Into ZCJ_UserScoreDetailsInfo (UserID,OpenId,Score,AddTime,AddNote,ScoreType,WebSiteOwner,SerialNumber) values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}');", toUserInfo.UserID, toOpenId, fromUserInfo.TotalScore, DateTime.Now.ToString(), scoreTitle2, "Move", WebsiteOwner, serialNumber);
                if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSql.ToString(), tran) != 4)
                {
                    msg = "操作失败";
                    tran.Rollback();
                    return false;

                }

                #region 日志记录
                BLLApiLog bllApiLog = new BLLApiLog();
                if (!bllApiLog.Add(fromUserInfo.WebsiteOwner, Enums.EnumApiModule.Score, string.Format("积分清零,清除积分:{0}", fromUserInfo.TotalScore), fromUserInfo.WXOpenId, fromUserInfo.UserID, serialNumber))
                {
                    msg = "日志记录失败";
                    tran.Rollback();
                    return false;
                }
                if (!bllApiLog.Add(toUserInfo.WebsiteOwner, Enums.EnumApiModule.Score, string.Format("积分转移,增加积分:{0}", fromUserInfo.TotalScore), toUserInfo.WXOpenId, toUserInfo.UserID, serialNumber))
                {

                    msg = "日志记录失败";
                    tran.Rollback();
                    return false;
                }
                #endregion

                tran.Commit();
                return true;


            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                tran.Rollback();
            }
            return false;

        }


        /// <summary>
        /// 积分变动
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool Update(string websiteOwner, string openId, double score, string remark, out string msg, string serialNumber = "")
        {
            msg = "";
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                score = Math.Ceiling(score);
                UserInfo userInfo = bllUser.GetUserInfoByOpenId(openId);
                if (userInfo == null && (websiteOwner == "dongwu" || websiteOwner == "dongwudev"))
                {
                    userInfo = bllUser.CreateNewUser(websiteOwner, openId, "");
                }
                if (userInfo == null)
                {

                    //创建新用户
                    msg = "openid不存在";
                    return false;
                }
                if ((userInfo.TotalScore + score) < 0)
                {
                    msg = "积分不足";
                    return false;
                }
                StringBuilder sbSql = new StringBuilder();
                double historyTotalScore = 0;
                if (score > 0)
                {
                    historyTotalScore = score;
                }
                if (string.IsNullOrEmpty(remark))
                {
                    remark = string.Format("积分变动,变动值{0}", score);
                }
                sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalScore+={0},HistoryTotalScore+={1} Where WxOpenId='{2}' And WebsiteOwner='{3}' ;", score, historyTotalScore, openId, userInfo.WebsiteOwner);//积分变动
                sbSql.AppendFormat(" Insert Into ZCJ_UserScoreDetailsInfo (UserID,OpenId,Score,AddTime,AddNote,ScoreType,WebSiteOwner,SerialNumber) values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}');", userInfo.UserID, openId, score, DateTime.Now.ToString(), remark, "Update", userInfo.WebsiteOwner, serialNumber);
                if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSql.ToString(), tran) < 2)
                {

                    msg = "操作失败";
                    tran.Rollback();
                    return false;

                }

                #region 日志记录
                BLLApiLog bllApiLog = new BLLApiLog();
                if (!bllApiLog.Add(userInfo.WebsiteOwner, Enums.EnumApiModule.Score, string.Format("积分变动,变动值{0}", score), userInfo.WXOpenId, userInfo.UserID, serialNumber))
                {
                    msg = "日志记录失败";
                    tran.Rollback();
                    return false;
                }

                #endregion

                tran.Commit();
                return true;

            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                tran.Rollback();

            }
            return false;

        }


        /// <summary>
        /// 积分事件触发
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="openId">openId</param>
        /// <param name="value">原始值</param>
        /// <param name="scoreEvent">事件名称</param>
        /// <param name="remark">备注</param>
        /// <param name="msg">提示信息</param>
        /// <param name="addScore">增加积分</param>
        /// <param name="showName">显示名称</param>
        /// <returns></returns>
        public bool EventUpdate(string websiteOwner, string openId, decimal value, string scoreEvent, string remark, out string msg, out int addScore, string showName = "", string serialNumber = "")
        {
            int score = 0;//最终添加的积分
            addScore = 0;
            msg = "";//返回消息
            BLLApiLog bllApiLog = new BLLApiLog();
            BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
            ScoreDefineInfo scoreDefineInfo = bllScoreDefine.GetScoreDefineInfoByScoreEvent(scoreEvent, WebsiteOwner);
            if (scoreDefineInfo == null)
            {
                msg = "积分规则不存在,请检查";
                return false;
            }
            if (scoreDefineInfo.IsHide == 1)
            {
                msg = "积分规则已停用";
                return false;
            }
            UserInfo userInfo = bllUser.GetUserInfoByOpenId(openId);
            if (userInfo == null && (websiteOwner == "dongwu" || websiteOwner == "dongwudev"))
            {
                userInfo = bllUser.CreateNewUser(websiteOwner, openId, showName);
            }
            if (userInfo == null)
            {
                msg = "openid不存在,请检查";
                return false;
            }
            List<ScoreDefineInfoExt> scoreDefineEx = bllScoreDefine.GetScoreDefineExList(scoreDefineInfo.ScoreId);
            //优先级: 扩展->基本比例-一般
            if (scoreDefineEx != null && scoreDefineEx.Count > 0)
            {
                DateTime dtNow = DateTime.Now;
                var item = scoreDefineEx.FirstOrDefault(p => p.BeginTime <= DateTime.Now && p.EndTime >= DateTime.Now);
                if (item != null)
                {
                    //score = (double)Math.Round(value / (item.RateValue / item.RateScore), 2);
                    score = (int)Math.Ceiling(value / (item.RateValue / item.RateScore));

                }

            }
            if (score == 0)
            {
                if (scoreDefineInfo.BaseRateScore > 0 && scoreDefineInfo.BaseRateValue > 0)//基础比例
                {
                    //score = (double)Math.Round(value / (scoreDefineInfo.BaseRateValue / scoreDefineInfo.BaseRateScore), 2);
                    score = (int)Math.Ceiling(value / (scoreDefineInfo.BaseRateValue / scoreDefineInfo.BaseRateScore));
                }
            }
            if (score == 0)
            {
                score = (int)scoreDefineInfo.Score;
            }

            //if (score == 0)
            //{
            //    msg = "增加积分不能为0";
            //    return false;
            //}
            if (scoreDefineInfo.DayLimit > 0)
            {
                double nTotal = bllUser.GetUserDayScoreSUMEvent(userInfo.UserID, scoreEvent, true);
                if (scoreDefineInfo.DayLimit < nTotal + score)
                {
                    msg = scoreDefineInfo.ScoreEvent + "每日所得积分超限制";
                    return false;
                }
            }
            if (scoreDefineInfo.TotalLimit > 0)
            {
                double nTotal = bllUser.GetUserDayScoreSUMEvent(userInfo.UserID, scoreEvent, false);
                if (scoreDefineInfo.TotalLimit < nTotal + score)
                {
                    msg = scoreDefineInfo.ScoreEvent + "所得总积分超限制";
                    return false;
                }

            }

            //积分记录
            UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
            scoreModel.AddNote = !string.IsNullOrEmpty(remark) ? remark : scoreDefineInfo.Description;
            scoreModel.AddTime = DateTime.Now;
            scoreModel.Score = score;
            scoreModel.UserID = userInfo.UserID;
            scoreModel.OpenId = userInfo.WXOpenId;
            scoreModel.ScoreType = scoreDefineInfo.ScoreType;
            scoreModel.WebSiteOwner = WebsiteOwner;
            scoreModel.ScoreEvent = scoreEvent;
            scoreModel.SerialNumber = serialNumber;
            BLLTransaction tran = new BLLTransaction();
            try
            {
                int historyTotalScore = 0;
                if (score > 0)
                {
                    historyTotalScore = score;
                }
                string sql = string.Format("TotalScore+={0},HistoryTotalScore+={1}", score, historyTotalScore);
                if (!string.IsNullOrEmpty(showName))
                {
                    sql += string.Format(",TrueName='{0}'", showName);
                }
                if (Update(userInfo, sql, string.Format(" AutoId={0}", userInfo.AutoID), tran) == 1 && Add(scoreModel, tran))
                {

                    #region 日志记录
                    if (!bllApiLog.Add(userInfo.WebsiteOwner, Enums.EnumApiModule.ScoreEvent, string.Format("积分事件,事件名称:{0}原始值{1}积分值:{2}", scoreDefineInfo.ScoreEvent, value, score), userInfo.WXOpenId, userInfo.UserID, serialNumber))
                    {
                        msg = "日志记录失败";
                        tran.Rollback();
                        return false;
                    }

                    #endregion
                    tran.Commit();
                    addScore = score;
                    return true;
                }
                else
                {
                    msg = "操作失败";
                    tran.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                tran.Rollback();
                return false;
            }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetUsedTotalScore(string userId)
        {

            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(string.Format(" Select Sum(Score) from ZCJ_UserScoreDetailsInfo Where UserId='{0}' And Score<0", userId));
            if (result != null)
            {
                return decimal.Parse(result.ToString());
            }
            return 0;

        }


        /// <summary>
        /// 积分排行
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<RankModel> ScoreRank(string year = "", string month = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("Select UserID,Sum(Score) as TotalScore from ZCJ_UserScoreDetailsInfo Where WebsiteOwner='{0}' And Score>0 ", WebsiteOwner);

            if (!string.IsNullOrEmpty(year))//本年数据
            {
                sbSql.AppendFormat(" And AddTime>='{0}' And AddTime<'{1}'", int.Parse(year), int.Parse(year) + 1);
            }
            if (!string.IsNullOrEmpty(month))//本月数据
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtThisMonth = new DateTime(dtNow.Year, int.Parse(month), 1);
                sbSql.AppendFormat(" And AddTime>='{0}' And AddTime<'{1}'", dtThisMonth.ToString(), dtThisMonth.AddMonths(1).ToString());
            }
            sbSql.AppendFormat(" Group by UserID Order By TotalScore Desc");
            List<UserScoreDetailsInfo> sourceList = ZentCloud.ZCBLLEngine.BLLBase.Query<UserScoreDetailsInfo>(sbSql.ToString());
            List<RankModel> list = new List<RankModel>();
            foreach (var item in sourceList)
            {
                UserInfo userInfo = bllUser.GetUserInfo(item.UserID);
                if (userInfo != null)
                {
                    RankModel model = new RankModel();
                    model.ShowName = userInfo.TrueName;
                    if (string.IsNullOrEmpty(userInfo.TrueName))
                    {
                        model.ShowName = userInfo.WXNickname;
                    }
                    model.Score = item.TotalScore;
                    model.HeadImg = bllUser.GetUserDispalyAvatar(userInfo);
                    list.Add(model);
                }
            }
            return list;

        }
        /// <summary>
        /// 积分排行榜模型
        /// </summary>
        public class RankModel
        {

            /// <summary>
            /// 显示名称
            /// </summary>
            public string ShowName { get; set; }
            /// <summary>
            /// 头像
            /// </summary>
            public string HeadImg { get; set; }
            /// <summary>
            /// 积分
            /// </summary>
            public double Score { get; set; }

        }

        #region 订单返积分相关

        /// <summary>
        /// 增加冻结积分记录：
        /// 真正返积分是确认收货7天退货期过后 
        /// 返积分按冻结记录去返，以解决：如果用户7天后升级，防止下单的时候冻结的数量跟真正分的数量不一样
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool AddLockScoreByOrder(WXMallOrderInfo orderInfo)
        {
            ToLog("into AddLockScoreByOrder");
            BLLWeixin bllWeiXin = new BLLWeixin();
            bool result = false;

            //获取是否已经存有的冻结积分
            var lockModel = GetLockScoreByOrder(orderInfo.OrderID);

            if (lockModel != null)
            {
                return true;
            }

            ToLog("AddLockScoreByOrder 计算获得积分");
            //计算获得积分
            var addScore = CalcOrderRebateScore(orderInfo);
            ToLog("AddLockScoreByOrder addScore:" + addScore);
            //如获得积分则存入冻结表
            if (addScore > 0)
            {
                ToLog("AddLockScoreByOrder 存入冻结表");
                ScoreLockInfo scoreLockInfo = new ScoreLockInfo()
                {
                    ForeignkeyId = orderInfo.OrderID,
                    LockStatus = 0,
                    LockTime = DateTime.Now,
                    LockType = 1,
                    Score = addScore,
                    UserId = orderInfo.OrderUserID,
                    WebsiteOwner = orderInfo.WebsiteOwner
                };

                if (Add(scoreLockInfo))
                {
                    ToLog("AddLockScoreByOrder 存入冻结表成功");
                    result = true;

                    //发送通知;
                    string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/lockscores#/lockscores", HttpContext.Current.Request.Url.Host);
                    bllWeiXin.SendTemplateMessageNotifyComm(GetCurrentUserInfo(), "您有一笔即将到账积分",
                        string.Format("下单获得 {0} 积分,点击查看", addScore),
                        url);

                }
                else
                {
                    ToLog("AddLockScoreByOrder 存入冻结表失败");
                }
            }

            return result;
        }

        /// <summary>
        /// 取消订单锁定积分
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public bool CancelLockScoreByOrder(string orderId, string memo)
        {
            //取出原记录
            var model = GetLockScoreByOrder(orderId);

            if (model == null)
            {
                return false;
            }

            //追加备注
            if (string.IsNullOrWhiteSpace(model.Memo))
            {
                memo = ";" + memo;
            }

            return Update(
                new ScoreLockInfo(),
                    string.Format(" Memo+='{0}',LockStatus=2,CancelTime=GETDATE() ", memo),
                    string.Format(" ForeignkeyId='{0}' AND LockType = 1 ", orderId)
                ) > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memo"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool CancelLockScoreByOrder(string orderId, string memo, BLLTransaction tran)
        {
            //取出原记录
            var model = GetLockScoreByOrder(orderId);

            if (model == null)
            {
                return false;
            }

            //追加备注
            if (string.IsNullOrWhiteSpace(model.Memo))
            {
                memo = ";" + memo;
            }

            return Update(
                new ScoreLockInfo(),
                    string.Format(" Memo+='{0}',LockStatus=2,CancelTime=GETDATE() ", memo),
                    string.Format(" ForeignkeyId='{0}' AND LockType = 1 ", orderId),
                    tran
                ) > 0;
        }

        /// <summary>
        /// 反取消积分
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public bool UnCancelLockScoreByOrder(string orderId, string memo)
        {
            //取出原记录
            var model = GetLockScoreByOrder(orderId);

            if (model == null)
            {
                return false;
            }

            //追加备注
            if (string.IsNullOrWhiteSpace(model.Memo))
            {
                memo = ";" + memo;
            }

            return Update(
                new ScoreLockInfo(),
                    string.Format(" Memo+='{0}',LockStatus=0,CancelTime=null ", memo),
                    string.Format(" ForeignkeyId='{0}' AND LockType = 1 ", orderId)
                ) > 0;
        }

        /// <summary>
        /// 获取订单对应的锁定积分
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns></returns>
        public ScoreLockInfo GetLockScoreByOrder(string orderId)
        {
            return Get<ScoreLockInfo>(string.Format(" ForeignkeyId = '{0}' AND LockType = 1 ", orderId));
        }

        /// <summary>
        /// 计算订单可以返多少积分
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public decimal CalcOrderRebateScore(WXMallOrderInfo orderInfo)
        {
            BLLMall bllMall = new BLLMall();
            decimal addScore = 0;

            //取出全部sku和数量，相加获取得到的总积分

            //判断当前是否只能全额支付才返积分
            //如果可以不全额，且实付跟应付不一致，则每个商品按照均摊价格去计算积分
            var websiteInfo = GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);

            if (websiteInfo.IsRebateScoreMustAllCash == 1 && !orderInfo.IsAllCash)
            {
                //如果没有使用优惠券、积分、余额抵扣则为全额付款
                return 0;
            }

            var detalList = bllMall.GetOrderDetailsList(orderInfo.OrderID);

            var rebateRate = GetUserRebateScoreRate(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            if (rebateRate > 0 && orderInfo.TotalAmount > 0)
            {
                foreach (var item in detalList)
                {
                    decimal payRate = 1;//实付比例

                    if (!orderInfo.IsAllCash)
                    {
                        // 没有全额付款，计算返积分均摊价

                        // 均摊价=单个商品价格/所有商品价格*（实付-运费）- 已废弃
                        //item.OrderPrice = item.OrderPrice.Value / orderInfo.Product_Fee * (orderInfo.TotalAmount - orderInfo.Transport_Fee);

                        /* 积分均摊价
                         * 
                         * 实付价格比例=商品实付/商品应付（不算邮费）
                         * 
                         * 单个sku应算积分差价比例=单个sku积分差价（一件）/积分差价总和（总件数）
                         * 
                         * 差价总额=每个sku的积分差价*数量 加起来
                         * 
                         * 单个sku积分差价（无均摊）= 单个sku应算积分差价比例 * 差价总额
                         * 
                         * 单个sku积分均摊价 = 实付价格比例 * 单个sku积分差价
                         *                   = 商品实付/商品应付（不算邮费） * 差价总额 * 单个sku应算积分差价比例
                         *                   
                         * --------------------------------------------------------------------------------------                  
                         *            
                         *                   
                         * 单个sku积分均摊价 = 实付价格比例 * 单个sku积分差价          
                         * 
                         * 
                         */

                        payRate = (orderInfo.TotalAmount - orderInfo.Transport_Fee) / orderInfo.Product_Fee;

                    }

                    addScore += CalcProductSkuRebateScore(item.SkuId.Value, rebateRate, payRate, item.TotalCount);

                }
            }

            addScore = RebateScoreGetInt(addScore);

            return addScore;
        }

        /// <summary>
        /// 计算每一个sku获得的积分
        /// </summary>
        /// <param name="skuId"></param>
        /// <param name="rebateRate"></param>
        /// <param name="payRate">
        /// 实付比例，
        /// 在需要均摊计算的时候用到
        /// </param>
        /// <param name="count">数量</param>
        /// <param name="isGetInt">
        /// 是否取整
        /// 如果多件商品计算获得积分，在最终总分里面做取整处理
        /// </param>
        /// <returns></returns>
        public decimal CalcProductSkuRebateScore(int skuId, decimal rebateRate, decimal? payRate = null, int count = 1, bool isGetInt = false)
        {
            BLLMall bllMall = new BLLMall();
            BLLDistribution bllDist = new BLLDistribution();
            decimal addScore = 0;

            var skuInfo = bllMall.GetProductSku(skuId);

            //判断如果是特卖中的商品则不给返积分
            if (bllMall.IsPromotionTime(skuInfo))
            {
                return 0;
            }

            var price = bllMall.GetSkuPrice(skuInfo);

            if (skuInfo.BasePrice <= 0)
            {
                var productInfo = bllMall.GetProduct(skuInfo.ProductId);
                skuInfo.BasePrice = productInfo.BasePrice;
            }

            price = price - skuInfo.BasePrice;

            if (payRate != null)
            {
                price = price * payRate.Value;
            }

            if (price < 0)
            {
                price = 0;
            }

            //全额获得的积分=售价*参与返积分的比例*积分返利比例  -- 已废除           
            //按价格比例获得的积分=均摊价*参与返积分的比例*积分返利比例 -- 已废除
            //addScore = (int)(rebateRate * (productInfo.RebateScoreRate * 0.01M) * skuPrice * count);

            //获得的积分=实际售价（如果非全额则需要计算均摊差价：售价-基础价）* 返积分比例 * 数量
            addScore = rebateRate * price * count;

            if (isGetInt)
            {
                addScore = RebateScoreGetInt(addScore);
            }

            return addScore;
        }

        /// <summary>
        /// 计算商品返积分，一般展示商品详情的时候用
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rebateRate"></param>
        /// <returns></returns>
        public decimal CalcProductRebateScore(string productId, decimal rebateRate, int count = 1, bool isGetInt = false)
        {
            BLLMall bllMall = new BLLMall();
            var productInfo = bllMall.GetProduct(productId);
            return CalcProductRebateScore(productInfo, rebateRate, count, isGetInt);
        }

        /// <summary>
        /// 计算商品获得的积分
        /// </summary>
        /// <param name="product"></param>
        /// <param name="rebateRate"></param>
        /// <param name="count"></param>
        /// <param name="isGetInt"></param>
        /// <returns></returns>
        public decimal CalcProductRebateScore(WXMallProductInfo product, decimal rebateRate, int count = 1, bool isGetInt = false)
        {
            BLLMall bllMall = new BLLMall();

            decimal addScore = 0;

            //如果是特卖中 则不返积分
            if (product.IsPromotionProduct == 1)
            {
                var productSkuList = bllMall.GetProductSkuList(int.Parse(product.PID));//源SKU 
                foreach (var item in productSkuList)
                {
                    if (bllMall.IsPromotionTime(item))
                    {
                        return 0;
                    }
                }
            }

            decimal price = product.Price - product.BasePrice;

            if (price < 0)
            {
                price = 0;
            }

            addScore = rebateRate * price * count;

            if (isGetInt)
            {
                addScore = RebateScoreGetInt(addScore);
            }

            return addScore;
        }


        /// <summary>
        /// 获取用户的积分返利比例
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public decimal GetUserRebateScoreRate(string userId, string websiteOwner)
        {
            BLLDistribution bllDist = new BLLDistribution();
            decimal rebateRate = 0;


            if (userId == null)
            {

                var defLevel = Get<UserLevelConfig>(string.Format(" [LevelType] = 'DistributionOnLine' AND [WebSiteOwner] = '{0}' AND [LevelNumber] = 0 ",
                        websiteOwner
                    ));

                if (defLevel != null)
                {
                    if (decimal.TryParse(defLevel.RebateScoreRate, out rebateRate))
                    {
                        rebateRate *= 0.01M;
                    }
                }

                return rebateRate;
            }


            var userInfo = bllUser.GetUserInfo(userId, websiteOwner);
            var userLevel = bllDist.GetUserLevel(userInfo);

            //获取积分返利比例
            //先取会员级别、没有会员级别则取全局配置的规则
            if (userLevel != null && userLevel.AutoId != null)
            {
                if (decimal.TryParse(userLevel.RebateScoreRate, out rebateRate))
                {
                    rebateRate *= 0.01M;
                }
            }
            else
            {
                ScoreConfig scoreConfig = GetScoreConfig();
                if (scoreConfig != null)
                {
                    if (scoreConfig.OrderScore == null)
                    {
                        rebateRate = 0;
                    }
                    else if (scoreConfig.OrderAmount > 0)
                    {
                        rebateRate = (decimal)scoreConfig.OrderScore / scoreConfig.OrderAmount;
                    }

                }
            }

            return rebateRate;
        }


        /// <summary>
        /// 返积分取整
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public int RebateScoreGetInt(decimal score)
        {
            int result = 0;
            var websiteOwner = GetWebsiteInfoModelFromDataBase(WebsiteOwner);
            switch (websiteOwner.RebateScoreGetIntType)
            {
                case 0:
                    result = (int)Math.Round(score, 0, MidpointRounding.AwayFromZero);
                    break;
                case 1:
                    result = (int)Math.Ceiling(score);
                    break;
                case 2:
                    result = (int)Math.Floor(score);
                    break;
                default:
                    result = (int)score;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据订单类型判断订单是否支持返积分
        /// </summary>
        /// <param name="orderType"></param>
        /// <param name="groupbuyType"></param>
        /// <returns></returns>
        public bool IsCanRebateScoreByOrderType(Enums.OrderType orderType, string groupbuyType = null)
        {
            bool result = false;


            var websiteInfo = GetWebsiteInfoModelFromDataBase();

            if (orderType == Enums.OrderType.Normal)
            {
                result = websiteInfo.IsOrderRebateScoreByMallOrder == 1;
            }

            if (orderType == Enums.OrderType.GroupBuy && groupbuyType == "leader")
            {
                result = websiteInfo.IsOrderRebateScoreByCreateGroupBuy == 1;
            }

            if (orderType == Enums.OrderType.GroupBuy && groupbuyType == "member")
            {
                result = websiteInfo.IsOrderRebateScoreByJoinGroupBuy == 1;
            }

            return result;
        }

        /// <summary>
        /// 获取即将到账积分总额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetTotalLockScore(string userId, string websiteOwner, string lockType = "1")
        {
            int result = 0;

            //result = GetCount<ScoreLockInfo>(string.Format(" UserId = '{0}' AND WebsiteOwner = '{1}' AND LockStatus = 0 AND LockType = 1 ", userId,websiteOwner));

            var totalCount = GetSingle(string.Format(" SELECT SUM([Score]) FROM [ZCJ_ScoreLockInfo] WHERE  UserId = '{0}' AND WebsiteOwner = '{1}' AND LockStatus = 0 AND LockType = {2} ",
                userId, websiteOwner, lockType));

            if (totalCount != null)
            {
                result = Convert.ToInt32(totalCount);
            }

            return result;
        }

        /// <summary>
        /// 获取即将到账积分列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<ScoreLockInfo> GetLockScoreList(int rows, int page, string userId, string websiteOwner, string lockType = "1", string lockStatus = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" UserId = '{0}' AND WebsiteOwner = '{1}' AND LockType = {2} ", userId, websiteOwner, lockType);
            if (!string.IsNullOrWhiteSpace(lockStatus)) sbSql.AppendFormat(" AND LockStatus = {0} ", lockStatus);
            return GetLit<ScoreLockInfo>(rows, page, sbSql.ToString(), " LockStatus ASC,LockTime DESC ");
        }

        /// <summary>
        /// 获取即将到账积分记录数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetLockScoreCount(string userId, string websiteOwner, string lockType = "1", string lockStatus = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" UserId = '{0}' AND WebsiteOwner = '{1}' AND LockType = {0} ", userId, websiteOwner, lockType);
            if (!string.IsNullOrWhiteSpace(lockStatus)) sbSql.AppendFormat(" AND LockStatus = {0} ", lockStatus);
            return GetCount<ScoreLockInfo>(sbSql.ToString());
        }

        /// <summary>
        /// 冻结积分结算
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public bool SettlementOrderLockScore(WXMallOrderInfo orderInfo, out string msg)
        {
            BLLMall bllMall = new BLLMall();
            BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();
            BLLWeixin bllWeixin = new BLLWeixin();

            msg = "";
            var lockModel = GetLockScoreByOrder(orderInfo.OrderID);

            int addScore = (int)lockModel.Score;

            if (addScore > 0)
            {

                UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                if (orderUserInfo == null)
                {
                    return false;
                }
                WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));

                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                try
                {

                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = addScore;
                    scoreRecord.TotalScore = orderUserInfo.TotalScore;
                    scoreRecord.ScoreType = "OrderSuccess";
                    scoreRecord.UserID = orderInfo.OrderUserID;
                    scoreRecord.AddNote = "微商城-交易成功获得积分";
                    scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                    scoreRecord.RelationID = orderInfo.OrderID;

                    if (!bllMall.Add(scoreRecord, tran))
                    {
                        tran.Rollback();
                        return false;
                    }
                    if (bllUser.Update(orderUserInfo, string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore), string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) <= 0)
                    {
                        tran.Rollback();
                        return false;
                    }

                    #region 宏巍加积分
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);
                        var hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
                        if (hongWareMemberInfo.member != null)
                        {
                            if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, addScore))
                            {
                                tran.Rollback();
                                return false;
                            }


                        }

                    }
                    #endregion

                    //积分解冻
                    if (!string.IsNullOrWhiteSpace(lockModel.Memo))
                    {
                        lockModel.Memo += ";交易成功获得积分解冻";
                    }
                    else
                    {
                        lockModel.Memo = "交易成功获得积分解冻";
                    }

                    if (Update(
                       new ScoreLockInfo(),
                           string.Format(" Memo+='{0}',LockStatus=1,UnLockTime=GETDATE() ", lockModel.Memo),
                           string.Format(" ForeignkeyId='{0}' AND LockType = 1 ", orderInfo.OrderID),
                           tran
                       ) > 0)
                    {

                        #region 微信通知
                        try
                        {

                            string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/myscores#/myscores", bllWebsiteDomain.GetWebsiteDoMain(orderInfo.WebsiteOwner));
                            bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "您有一笔积分已经到账", string.Format("积分:{0}分", (int)lockModel.Score), url, "", "", "", orderInfo.WebsiteOwner);

                        }
                        catch
                        {

                        }
                        #endregion

                    }
                    else
                    {
                        msg = string.Format("处理即将到账积分失败!订单号:{0}", orderInfo.OrderID);
                        tran.Rollback();
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    tran.Rollback();
                    return false;
                }

                tran.Commit();
            }

            return true;
        }

        /// <summary>
        /// 积分统计任务
        /// </summary>
        public bool ExceScoreStatis(TimingTask task)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat(" WebSiteOwner='{0}' ", task.WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(task.DistributionUserId)){
                sbSQL.AppendFormat(" And UserID='{0}' ", task.DistributionUserId);
            };
            //if (!string.IsNullOrWhiteSpace(type)) { sbSQL.AppendFormat(" And ScoreType='{0}' ", type); }
            sbSQL.AppendFormat(" And ScoreType!='AccountAmount' And ScoreType!='TotalAmount' ");

            if (task.FromDate != null) { 
                sbSQL.AppendFormat(" AND AddTime>='{0}' ", task.FromDate); 
            }
            if (task.ToDate != null)
            { 
                sbSQL.AppendFormat(" AND AddTime<='{0}' ", task.ToDate);
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("日期");
            dt.Columns.Add("用户");
            dt.Columns.Add("手机");
            dt.Columns.Add("积分");
            dt.Columns.Add("说明");
            List<UserScoreDetailsInfo> userScores = new List<UserScoreDetailsInfo>();

            int pageIndex = 1;
            do
            {

                userScores = GetLit<UserScoreDetailsInfo>(100, pageIndex, sbSQL.ToString(), " AddTime DESC");
               if (userScores.Count>0)
               {
                   for (int i = 0; i < userScores.Count; i++)
                   {
                       UserInfo user = bllUser.GetUserInfo(userScores[i].UserID,task.WebsiteOwner);
                       if (user == null) continue;
                       System.Data.DataRow newRow = dt.NewRow();
                       newRow["日期"] = userScores[i].AddTime;
                       newRow["用户"] = user.TrueName;
                       newRow["手机"] = user.Phone;
                       newRow["积分"] = userScores[i].Score;
                       newRow["说明"] = userScores[i].AddNote;
                       dt.Rows.Add(newRow);
                   }
                   pageIndex++;
                   System.Threading.Thread.Sleep(100);
               }

            } while (userScores.Count>0);

            dt.TableName = "积分统计";
            string filePath = string.Format("\\FileUpload\\ScoreStatis\\{0}_{1}积分统计.xls",Guid.NewGuid().ToString(),Convert.ToDateTime(task.InsertDate).ToString("yyyyMMddHHmm"));

            string filePathAbs = string.Format(Common.ConfigHelper.GetConfigString("WebSitePath") + filePath);

            Common.NPOIHelper.DtToXls(dt, filePathAbs);
            task.Url = filePath;

            bool result=Update(task);
            try
            {

                string dir = "ScoreStatis";
                string fileType = "excel";
              string ossUrl=  AliOss.OssHelper.UploadFile(AliOss.OssHelper.GetBucket(task.WebsiteOwner), dir, AliOss.OssHelper.GetBucket(task.WebsiteOwner), fileType,task.WebsiteOwner, filePathAbs);

              task.Url = ossUrl;
              Update(task);


            }
            catch (Exception ex)
            {
                task.TaskInfo += ex.ToString();
            }
            
            return result;

           

        }
        #endregion


    }
}
