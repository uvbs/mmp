using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 刮刮奖 BLL
    /// </summary>
    public class BllLottery : BLL
    {
        public BllLottery()
            : base()
        {

        }

        /// <summary>
        ///  记录抽奖日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddWXLotteryLogV1(WXLotteryLogV1 model)
        {
            return Add(model);
        }
        
        /// <summary>
        /// 插入中奖记录
        /// </summary>
        /// <returns></returns>
        public bool AddWXLotteryRecordV1(WXLotteryRecordV1 model)
        {
            
            WebsiteInfo websiteInfo=GetWebsiteInfoModelFromDataBase();
            var award = model.WXAward;
            
            switch (award.AwardsType)
            {
                case 1://给用户增加积分
                    var isSyncYike = new BLLJIMP.BLLCommRelation().ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, WebsiteOwner, "");
                    Open.EZRproSDK.Client zrClient = new Open.EZRproSDK.Client();
                    if (isSyncYike)
                    {
                        var resp = zrClient.BonusUpdate(GetCurrentUserInfo().Ex2, Convert.ToInt32(award.Value), "中奖加积分");
                    }
                    else if (websiteInfo.IsUnionHongware==1)
                    {

                        Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(GetCurrentUserInfo().WebsiteOwner);
                        var hongWareMemberInfo = hongWareClient.GetMemberInfo(GetCurrentUserInfo().WXOpenId);
                        if (hongWareMemberInfo.member != null)
                        {
                            hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile,GetCurrentUserInfo().WXOpenId, float.Parse(award.Value));

                        }
                    
                    
                    }
                    //else
                    //{
                        string tempMsg = "";
                        new BLLUser().AddUserScoreDetail(GetCurrUserID(), CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ScoreDefineType.Lottery), WebsiteOwner
                            , out tempMsg, Convert.ToInt32(award.Value), "中奖加积分","", false);
                    //}
                    break;
                case 2://给用户增加优惠券

                    new BLLCardCoupon().SendCardCoupon(int.Parse(model.WXAward.Value), model.UserId);

                    break;
                default:
                    break;
            }


            return Add(model);
        }
        /// <summary>
        /// 获取中奖记录V1
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lotteryId"></param>
        /// <returns></returns>        
        public WXLotteryRecordV1 GetWXLotteryRecordV1(string userId, int lotteryId,int awardId)
        {
            return Get<WXLotteryRecordV1>(string.Format("LotteryId={0} And UserId='{1}' And WXAwardsId = {2} ", lotteryId, userId, awardId));
        }
        public WXLotteryRecordV1 GetWXLotteryRecordV1(string userId, int lotteryId)
        {
            return Get<WXLotteryRecordV1>(string.Format("LotteryId={0} And UserId='{1}'", lotteryId, userId));
        }
        public WXLotteryRecordV1 GetWXLotteryRecordByRecordIdV1(string userId, int lotteryId, int recordId)
        {
            return Get<WXLotteryRecordV1>(string.Format(" UserId='{0}' And LotteryId={1} And  AutoId={2} ", userId, lotteryId,recordId));
        }

        /// <summary>
        /// 获取中奖记录V1
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<WXLotteryRecordV1> GetWXLotteryRecordList(int lotteryId,string userId = "")
        {
            StringBuilder sbWhere = new StringBuilder();

            sbWhere.AppendFormat(" LotteryId={0} ", lotteryId);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                sbWhere.AppendFormat(" AND UserId = '{0}' ",userId);
            }

            return GetList<WXLotteryRecordV1>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取中奖记录V1 分页
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<WXLotteryRecordV1> GetWXLotteryRecordV1(int lotteryId, int pageIndex, int pageSize)
        {
            return GetLit<WXLotteryRecordV1>(pageSize, pageIndex, string.Format("LotteryId={0} ", lotteryId), " AutoID DESC");
        }
        /// <summary>
        /// 获取奖项
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<WXAwardsV1> GetAwardsListV1(int lotteryId)
        {
            var list = GetList<WXAwardsV1>(string.Format("LotteryId={0} Order by AutoID ASC", lotteryId));

            if (list != null)
            {
                list = FilterAwards(list);
            }

            return list;
        }
        public List<WXAwardsV1> FilterAwards(List<WXAwardsV1> dataList)
        {
            BLLCardCoupon bllCardCoupon = new BLLCardCoupon();

            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].AwardsType == 2)
                {
                    int cardCouponId = 0;
                    if (int.TryParse(dataList[i].Value,out cardCouponId))
                    {
                        var cardCoupon = bllCardCoupon.GetCardCoupon(cardCouponId);
                        dataList[i].ValueName = cardCoupon == null ? "" : cardCoupon.Name;
                    }

                }//优惠券读取优惠券名称
            }
            return dataList;
        }
        /// <summary>
        /// 获取奖项
        /// </summary>
        /// <param name="awardsId"></param>
        /// <returns></returns>
        public WXAwardsV1 GetAwards(int awardsId)
        {
            return Get<WXAwardsV1>(string.Format(" AutoID = {0} ", awardsId));
        }
        /// <summary>
        /// 获取抽奖活动信息
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public WXLotteryV1 GetLottery(int lotteryId)
        {
            return Get<WXLotteryV1>(string.Format("LotteryID={0}", lotteryId));
        }
        /// <summary>
        /// 查询指定用户抽奖日志
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WXLotteryLogV1> GetUserLotteryLog(int lotteryId, string userId)
        {
            return GetList<WXLotteryLogV1>(string.Format(" LotteryId = '{0}' AND UserId = '{1}' ", lotteryId, userId));
        }

        /// <summary>
        /// 查询指定用户最新的抽奖日志
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WXLotteryLogV1> GetUserLotteryLog(int top,int lotteryId, string userId)
        {
            return GetList<WXLotteryLogV1>(top, string.Format(" LotteryId = '{0}' AND UserId = '{1}' ", lotteryId, userId),"AutoID Desc");
        }

        /// <summary>
        /// 获取抽奖活动列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="lotteryName"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="total"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<WXLotteryV1> GetLotteryList(int rows, int page, string lotteryName,string lotteryType,string websiteOwner,out int total,string sort = "0"){
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            if (!string.IsNullOrEmpty(lotteryName))
            {
                sbWhere.AppendFormat(" And LotteryName like '%{0}%'", lotteryName);
            }
            if (!string.IsNullOrEmpty(lotteryType))
            {
                sbWhere.AppendFormat(" And LotteryType = '{0}'", lotteryType);
            }
            total = GetCount<WXLotteryV1>(sbWhere.ToString());
            string order = "";
            if (string.IsNullOrWhiteSpace(sort) || sort == "0") order = " Status DESC, LotteryID DESC";
            return GetLit<WXLotteryV1>(rows, page, sbWhere.ToString(), order);
        }
        /// <summary>
        /// 是否已经添加中奖用户
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ExistsWinningData(string lotteryId, string userId)
        {
            WXLotteryWinningDataV1 winningData = Get<WXLotteryWinningDataV1>(string.Format("LotteryId={0} And UserId='{1}'", lotteryId, userId));
            if (winningData == null) return false;
            return true;
        }

        /// <summary>
        /// 根据抽奖活动 用户名 获取抽奖记录
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<WXLotteryLogV1> GetWXLotteryLogListV1(int pageSize, int pageIndex, int lotteryId, string userId = null, DateTime? desDate = null)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("LotteryId={0}", lotteryId);
            if (!string.IsNullOrWhiteSpace(userId)) strWhere.AppendFormat(" And UserId='{0}'", userId);
            if (desDate != null)
            {
                strWhere.AppendFormat(" AND DATEDIFF(D,insertdate,'{0}') = 0", desDate.Value);
            }
            return GetLit<WXLotteryLogV1>(pageSize, pageIndex, strWhere.ToString());
        }

        /// <summary>
        /// 根据抽奖活动 用户名 获取抽奖记录数
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public int GetWXLotteryLogCountV1(int lotteryId, string userId = null, DateTime? desDate = null)
        {
            List<WXLotteryLogV1> list = GetWXLotteryLogListV1(int.MaxValue, 1, lotteryId, userId, desDate);
            return list.Count();
        }

        /// <summary>
        /// 根据抽奖活动 用户名 获取抽奖记录数
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public int GetWXLotteryLogUserCountV1(int lotteryId, string userId = null, DateTime? desDate = null)
        {
            List<WXLotteryLogV1> list = GetWXLotteryLogListV1(int.MaxValue, 1, lotteryId, userId, desDate);
            return list.GroupBy(p => p.UserId).Count();
        }

        /// <summary>
        /// 根据抽奖活动 用户名 奖项 获取中奖奖记录
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="userId"></param>
        /// <param name="awardsId"></param>
        /// <param name="desDate"></param>
        /// <returns></returns>
        public List<WXLotteryRecordV1> GetWXLotteryRecordListV1(int pageSize,int pageIndex, int lotteryId, string userId = null, int? isGetPrize = null
            , int? awardsId = null, DateTime? desDate = null)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("LotteryId={0}", lotteryId);
            if (!string.IsNullOrWhiteSpace(userId)) sbWhere.AppendFormat(" And UserId='{0}'", userId);
            if (isGetPrize.HasValue) sbWhere.AppendFormat(" And IsGetPrize={0} ", isGetPrize.Value);
            if (awardsId.HasValue) sbWhere.AppendFormat(" And WXAwardsId={0} ", awardsId.Value);
            if (desDate != null)
            {
                sbWhere.AppendFormat(" AND DATEDIFF(D,insertdate,'{0}') = 0", desDate.Value);
            }
            return GetLit<WXLotteryRecordV1>(pageSize, pageIndex, sbWhere.ToString());
        }
        /// <summary>
        /// 根据抽奖活动 用户名 获取抽奖记录数
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public int GetWXLotteryRecordCountV1(int lotteryId, string userId = null, int? isGetPrize = null
            , int? awardsId = null, DateTime? desDate = null)
        {
            List<WXLotteryRecordV1> list = GetWXLotteryRecordListV1(int.MaxValue,1,lotteryId, userId, isGetPrize, awardsId, desDate);
            return list.Count;
        }

        /// <summary>
        /// 检查是否领过奖品
        /// </summary>
        /// <param name="UserId">当前用户的id</param>
        /// <param name="autoId">活动编号</param>
        /// <returns>返回是否领奖成功</returns>
        public bool IsUserGetPrizeV1(string UserId, int autoId, int awardsId=0)
        {
            WXLotteryRecordV1 wxlRecord = Get<WXLotteryRecordV1>(string.Format(" UserID='{0}' AND LotteryId={1} AND WXAwardsId={2}", UserId, autoId, awardsId));
            if (wxlRecord != null)
            {
                if (wxlRecord.IsGetPrize == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsUserGetPrizeByRecordIdV1(int recordId)
        {
            WXLotteryRecordV1 wxlRecord = Get<WXLotteryRecordV1>(string.Format(" AutoID={0} ", recordId));
            if (wxlRecord != null)
            {
                if (wxlRecord.IsGetPrize == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 中奖数据
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public DataTable QueryLotteryData(int lotteryId)
        {
            DataTable dt = new DataTable();
            try
            {
                



                StringBuilder strSql = new StringBuilder();
                strSql.Append("select a.Name as 姓名,a.Phone as 手机 , c.wxnickname as 昵称,b.PrizeName 奖品, ");
                strSql.Append(" a.InsertDate as 中奖时间,(case WHEN a.IsGetPrize=0 THEN '未领奖' WHEN a.IsGetPrize=1 THEN '已领奖' ELSE '' END ) as 是否已领奖, ");
                strSql.Append(" a.UserID ");
                strSql.Append(" from ZCJ_WXLotteryRecordV1 a ");
                strSql.Append(" inner join  ZCJ_WXAwardsV1 b on a.wxawardsid=b.autoid");
                strSql.Append(" inner join  ZCJ_UserInfo c on a.userid=c.userid");
                strSql.AppendFormat(" where a.LotteryID={0}  ",lotteryId);
                dt = Query(strSql.ToString()).Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        /// <summary>
        /// 判断签到抽奖是否可以使用
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool signInLotteryIsCanUse(int lotteryId,out string msg)
        {
            msg = "";
            
            var signInModel = new BLLJIMP.BLLSignIn().Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND Type='Sign' ", WebsiteOwner));
            if (signInModel!=null&&signInModel.LotteryId == lotteryId.ToString())
            {
                var dt = DateTime.Now;

                if (dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    msg = "周日才可以抽奖";                   
                    return false;
                }

                //获取本周签到记录，倒退七天是否有七条
                var signInCount = GetCount<SignInLog>(string.Format("  cast(SignInDate as date) between '{0}' and '{1}' and  WebsiteOwner = '{2}' and UserID = '{3}' ",
                        dt.AddDays(-6).ToString("yyyy-MM-dd"),
                        dt.ToString("yyyy-MM-dd"),
                        WebsiteOwner,
                        GetCurrUserID()
                    ));

                if (signInCount < 7)
                {
                    msg = "一周连续签到才可以抽奖";
                    return false;
                }

            }

            return true;
        }


    }
}
