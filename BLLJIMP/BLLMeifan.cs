using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using System.Web;

namespace ZentCloud.BLLJIMP
{
    public class BLLMeifan : BLL
    {

        #region 会员卡
        /// <summary>
        /// 会员卡列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="cardType">类型</param>
        /// <param name="isDisable">是否禁用</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public List<MeifanCard> CardList(int pageIndex, int pageSize, string cardType, string isDisable, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}' And IsDelete=0 ", WebsiteOwner);

            if (!string.IsNullOrEmpty(cardType))
            {
                sbWhere.AppendFormat(" And CardType='{0}'", cardType);
            }
            if (!string.IsNullOrEmpty(isDisable))
            {
                sbWhere.AppendFormat(" And IsDisable={0}", isDisable);
            }
            totalCount = GetCount<MeifanCard>(sbWhere.ToString());
            return GetLit<MeifanCard>(pageSize, pageIndex, sbWhere.ToString(), " AutoID Asc");
        }

        /// <summary>
        /// 获取会员卡券详细信息
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public MeifanCard GetCard(string cardId)
        {

            return Get<MeifanCard>(string.Format("CardId='{0}'", cardId));
        }

        /// <summary>
        /// 获取实际金额
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public decimal GetTrueAmount(MeifanCard card)
        {

            var currentUserInfo = GetCurrentUserInfo();
            if (currentUserInfo != null)
            {
                if (GetCount<MeifanMyCard>(string.Format("UserId='{0}' And CardId='{1}'", currentUserInfo.UserID, card.CardId)) > 0)
                {
                    return card.Amount - card.ServerAmount;
                }
            }
            return card.Amount;

        }
        /// <summary>
        /// 获取实际服务费
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public decimal GetTrueServerAmount(MeifanCard card)
        {
            var currentUserInfo = GetCurrentUserInfo();
            if (currentUserInfo != null)
            {
                if (GetCount<MeifanMyCard>(string.Format("UserId='{0}' And CardId='{1}'", currentUserInfo.UserID, card.CardId)) > 0)
                {
                    return 0;
                }
            }
            return card.ServerAmount;


        }


        /// <summary>
        /// 我的会员卡列表
        /// </summary>
        /// <returns></returns>
        public List<MeifanMyCard> MyCardList(string userId)
        {

            return GetList<MeifanMyCard>(string.Format(" UserId='{0}'And DATEADD(month,ValidMonth,ValidDate)>GetDate()", userId));

        }
        /// <summary>
        /// 获取我的默认会员卡号
        /// </summary>
        /// <returns></returns>
        public string GetMyDefualtCardNumber(string userId)
        {

            string cardNumber = "";
            var myCardList = MyCardList(userId);
            List<MeifanCard> cardList = new List<MeifanCard>();
            if (myCardList.Count > 0)
            {
                foreach (var item in myCardList)
                {
                    MeifanCard card = GetCard(item.CardId);
                    cardList.Add(card);
                }
                string cardId = "";
               // cardId = cardList.OrderByDescending(p => p.Amount + p.ServerAmount).First().CardId;
                if (cardList.Count(p => p.CardType == "chuandong") > 0)
                {
                    cardId = cardList.First(p => p.CardType == "chuandong").CardId;

                }
                else if (cardList.Count(p => p.CardType == "family") > 0)
                {
                    cardId = cardList.First(p => p.CardType == "family").CardId;
                }

                else if (cardList.Count(p => p.CardType == "personal") > 0)
                {
                    cardId = cardList.First(p => p.CardType == "personal").CardId;
                }

                return myCardList.First(p => p.CardId == cardId).CardNum;
            }
            return cardNumber;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MeifanMyCard GetMyCardById(string id)
        {
            return Get<MeifanMyCard>(string.Format(" AutoId='{0}'", id));

        }
        /// <summary>
        /// 获取我的会员卡到期时间
        /// </summary>
        /// <param name="myCard"></param>
        /// <returns></returns>
        public string GetMyCardExpireDate(MeifanMyCard myCard)
        {

            return myCard.ValidDate.AddMonths(myCard.ValidMonth).ToString("yyyy-MM-dd");

        }
        /// <summary>
        /// 获取我的会员卡还剩多少天过期
        /// </summary>
        /// <param name="myCard"></param>
        /// <returns></returns>
        public int GetMyCardOverDays(MeifanMyCard myCard)
        {
            var days = (int)(myCard.ValidDate.AddMonths(myCard.ValidMonth) - DateTime.Now).TotalDays;
            return days > 0 ? days : 0;

        }


        #endregion




        #region 活动 竞赛 培训
        /// <summary>
        /// 获取 活动 竞赛 培训 列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="activityType">类型
        ///activity 活动
        ///match 比赛
        ///train 培训
        /// </param>
        /// <param name="keyWord"></param>
        /// <param name="isPublish"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<JuActivityInfo> ActivityList(int pageIndex, int pageSize, string activityType, string keyWord, string isPublish, out int totalCount, string orderBy = " CreateDate Desc")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}' And IsDelete=0 ", WebsiteOwner);

            if (!string.IsNullOrEmpty(activityType))
            {
                sbWhere.AppendFormat(" And ArticleType='{0}'", activityType);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And ActivityName like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(isPublish))
            {
                sbWhere.AppendFormat(" And IsPublish='{0}'", isPublish);
            }
            totalCount = GetCount<JuActivityInfo>(sbWhere.ToString());

            return GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
        }

        /// <summary>
        /// 获取活动报名数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <param name="activityType"></param>
        /// <returns></returns>
        public List<ActivityDataInfo> ActivityDataList(int pageIndex, int pageSize, out int totalCount, string activityId = "", string userId = "", string activityType = "", string keyWord = "", string activityName = "", string fromDate = "", string toDate = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}'  ", WebsiteOwner);
            if (!string.IsNullOrEmpty(activityId))
            {
                sbWhere.AppendFormat(" And ActivityId='{0}' ", activityId);
            }
            if (!string.IsNullOrEmpty(activityName))
            {
                sbWhere.AppendFormat(" And ActivityName like '%{0}%' ", activityName);
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                sbWhere.AppendFormat(" And InsertDate>='{0}' ", fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                string toDateNew = Convert.ToDateTime(toDate).AddDays(1).ToString();
                sbWhere.AppendFormat(" And InsertDate<'{0}' ", toDateNew);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId='{0}' ", userId);
            }
            if (!string.IsNullOrEmpty(activityType))
            {
                sbWhere.AppendFormat(" And ActivityType='{0}' ", activityType);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (Name like '%{0}%' Or Phone like '%{0}%') ", keyWord);
            }
            totalCount = GetCount<ActivityDataInfo>(sbWhere.ToString());
            return GetLit<ActivityDataInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate Desc");
        }

        ///// <summary>
        ///// 获取报名人数
        ///// </summary>
        ///// <returns></returns>
        //public int GetSignUpCount(string activityId)
        //{

        //    return GetCount<MeifanActivityData>(string.Format(" ActivityId='{0}'", activityId));

        //}



        /// <summary>
        /// 获取订单号报名数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActivityDataInfo GetActivityDataByOrderId(string orderId)
        {

            return Get<ActivityDataInfo>(string.Format(" OrderId='{0}'", orderId));

        }
        ///// <summary>
        ///// 获取id报名数据
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        //public ActivityDataInfo GetActivityDataById(string orderId)
        //{

        //    return Get<ActivityDataInfo>(string.Format(" OrderId='{0}'", atuoId));

        //}
        /// <summary>
        /// 活动 比赛 培训 详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public JuActivityInfo GetActivity(string activityId)
        {

            return Get<JuActivityInfo>(string.Format("JuActivityID='{0}'", activityId));
        }

        /// <summary>
        /// 获取活动状态
        /// 0 未开始
        /// 1 进行中
        /// 2 已经结束
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public string GetActivityStatus(JuActivityInfo activity)
        {

            if (activity.EndDate != null)
            {
                if (DateTime.Parse(activity.BeginDate) > DateTime.Now)
                {
                    return "0";
                }
                else if (DateTime.Now >= DateTime.Parse(activity.BeginDate) && DateTime.Parse(activity.EndDate) >= DateTime.Now)
                {
                    return "1";
                }

            }
            return "2";


        }
        /// <summary>
        /// 获取竞赛状态 列表
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public string GetMatchStatus(JuActivityInfo activity)
        {
            try
            {

                if (activity.IsFee==0)
                {
                    return "1";
                }
                DateTime dtNow = DateTime.Now;
                var beginDateList = ActivityItemBeginDateList(activity.JuActivityID.ToString());
                beginDateList.Sort();
                var endDateList = ActivityItemEndDateList(activity.JuActivityID.ToString());
                endDateList.Sort();

                if (Convert.ToDateTime(beginDateList[beginDateList.Count - 1]) > dtNow)
                {

                    return "0";

                }
                else if (Convert.ToDateTime(endDateList[endDateList.Count - 1]) < dtNow)
                {
                    return "2";
                }
            }
            catch (Exception)
            {


            }
            return "1";


        }

        /// <summary>
        /// 获取竞赛状态我的
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public string GetMyMatchStatus(ActivityDataInfo data)
        {


            if (!string.IsNullOrEmpty(data.DateRange))
            {
                DateTime dtNow = DateTime.Now;
                var fromDate = data.DateRange.Split('-')[0];
                var toDate = data.DateRange.Split('-')[1];

                if (Convert.ToDateTime(fromDate) > dtNow)
                {
                    return "0";
                }
                if (dtNow > Convert.ToDateTime(toDate))
                {
                    return "2";
                }
            }


            return "1";


        }
        /// <summary>
        /// 获取竞赛状态
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetMyMatchStatus(JuActivityInfo activity, UserInfo userInfo)
        {
            var data = Get<ActivityDataInfo>(string.Format("ActivityId={0} And UserId='{1}'", activity.JuActivityID, userInfo.UserID));

            return GetMyMatchStatus(data);

        }
        /// <summary>
        /// 获取活动 比赛 培训 收费选项
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<MeifanActivityItem> ActivityItemList(string activityId)
        {

            return GetList<MeifanActivityItem>(string.Format("ActivityId={0}", activityId));

        }

        /// <summary>
        /// 获取不重复的时间段
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<string> ActivityItemDateList(string activityId)
        {
            List<string> list = new List<string>();
            var itemList = ActivityItemList(activityId);
            foreach (var item in itemList)
            {
                string dateRange = string.Format("{0}-{1}", Convert.ToDateTime(item.FromDate).ToString("yyyy/MM/dd"), Convert.ToDateTime(item.ToDate).ToString("yyyy/MM/dd"));
                if (!list.Contains(dateRange))
                {
                    list.Add(dateRange);
                }

            }
            return list;

        }
        /// <summary>
        /// 获取不重复的开始时间段
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<string> ActivityItemBeginDateList(string activityId)
        {
            List<string> list = new List<string>();
            var itemList = ActivityItemList(activityId);
            foreach (var item in itemList)
            {
                string dateRange = Convert.ToDateTime(item.FromDate).ToString("yyyy/MM/dd");
                if (!list.Contains(dateRange))
                {
                    list.Add(dateRange);
                }

            }
            return list;

        }
        /// <summary>
        /// 获取不重复的时间段
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<string> ActivityItemEndDateList(string activityId)
        {
            List<string> list = new List<string>();
            var itemList = ActivityItemList(activityId);
            foreach (var item in itemList)
            {
                string dateRange = Convert.ToDateTime(item.ToDate).ToString("yyyy/MM/dd");
                if (!list.Contains(dateRange))
                {
                    list.Add(dateRange);
                }

            }
            return list;

        }
        /// <summary>
        /// 获取默认日期时间段 离当前时间最近的日期段
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public string GetDefaultActivityItemDate(string activityId)
        {
            try
            {
                var beginDateList = ActivityItemBeginDateList(activityId);
                var dateList = ActivityItemDateList(activityId);
                Dictionary<string, double> dic = new Dictionary<string, double>();
                DateTime dtNow = DateTime.Now;
                foreach (var item in beginDateList)
                {
                    double tempSec = Math.Abs((dtNow - Convert.ToDateTime(item)).TotalSeconds);
                    dic.Add(item, tempSec);

                }

                string defaultDate = dic.OrderBy(p => p.Value).First().Key;
                foreach (var item in dateList)
                {

                    if (item.StartsWith(defaultDate))
                    {
                        return item;
                    }

                }
            }
            catch (Exception)
            {


            }




            return "";

        }

        /// <summary>
        /// 获取组别
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<string> ActivityItemGroupList(string activityId)
        {

            return ActivityItemList(activityId).DistinctBy(p => p.GroupType).ToList().Select(p => p.GroupType).ToList();

        }
        /// <summary>
        /// 获取会员类型
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<string> ActivityItemMemberTypeList(string activityId)
        {

            return ActivityItemList(activityId).DistinctBy(p => p.IsMember).ToList().Select(p => p.IsMember).ToList();

        }
        /// <summary>
        /// 获取我的培训状态
        /// 0 未开始
        /// 1 进行中
        /// 2 已结束
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMyTrainStatus(ActivityDataInfo data)
        {

            DateTime dtNow = DateTime.Now;
            var fromDate = data.DateRange.Split('-')[0];
            var toDate = data.DateRange.Split('-')[1];

            if (Convert.ToDateTime(fromDate) > dtNow)
            {
                return "0";
            }
            if (dtNow > Convert.ToDateTime(toDate))
            {
                return "2";
            }
            return "1";


        }
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsExpire(JuActivityInfo model)
        {
            if (model.ArticleType == "activity")//活动
            {
                if (GetActivityStatus(model) == "2")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if (model.ArticleType == "match" || model.ArticleType == "train")//竞赛 培训
            {
                if (model.IsFee == 1)
                {
                    DateTime dtNow = DateTime.Now;
                    foreach (var item in ActivityItemList(model.JuActivityID.ToString()))
                    {

                        if (Convert.ToDateTime(item.ToDate) > dtNow)
                        {
                            return false;
                        }

                    }
                }
                else
                {
                    return false;
                }

            }

            return true;

        }

        /// <summary>
        /// 获取活动短名称
        /// </summary>
        /// <param name="activityName"></param>
        /// <returns></returns>
        public string GetActivityShortName(string activityName)
        {

            if (activityName.Length <= 12)
            {
                return activityName;
            }
            return activityName.Substring(0, 12) + "...";

        }


        #endregion


    }
}
