using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using System.Data;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using ZentCloud.Common;
using AliOss;

namespace ZentCloud.BLLJIMP
{
    public class BLLJuActivity : BLL
    {
        BLLActivity bllActivity = new BLLActivity("");
        BLLUser bllUser = new BLLUser("");
        BLLJIMP.BLLLog bllLog = new BLLLog();
        BLLWeixin bllWeixin = new BLLWeixin();
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        public BLLJuActivity(string userID)
            : base(userID)
        {

        }

        public BLLJuActivity()
            : base()
        {

        }

        /// <summary>
        /// 获取聚活动数据
        /// </summary>
        /// <param name="queryType">查找类型:search、new(已遗弃)</param>
        /// <param name="totalCount">总数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="activityAddress">活动地址</param>
        /// <param name="activityFutureDay">未来天数</param>
        /// <param name="recommendCate">推荐类型</param>
        /// <param name="isFee">是否免费</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userID">用户ID</param>
        /// <param name="isSpread">是否推广</param>
        /// <param name="articleType">文章类型：article、activity</param>
        /// <returns></returns>
        public List<JuActivityInfo> QueryJuActivityData(
                string queryType,
                out int totalCount,
                string activityAddress = "",
                string activityFutureDay = "",
                string recommendCate = "",
                string isFee = "",
                string keyWord = "",
                int pageIndex = 0,
                int pageSize = 0,
                string userID = "",

                int? isSpread = null,
                string articleType = "",
                string websiteOwner = "",
                string articleTypeEx1 = "",
                string categoryId = "",
                string preId = "",
                string provinceCode = "",
                string cityCode = "",
                string districtCode = "",
                string tags = "",//标签筛选，可以逗号分隔多个标签
                bool isNoCommentAndReplayCount = false,//是否查询没有评论内容的列表
                string orderBy = "",//排序
                bool isHasCommentAndReplayCount = false,
                bool showHide = true,
                bool hasSys = false,
                string isDelete = "",
                string status = null,
                bool isShowEndDateData = false,
                string column = "",
                bool isForward = false,
                string createDateStart = "",//创建时间开始
                string createDateEnd = "",//创建时间结束
                bool keywordSearchAuthor = false,//是否关键字查询作者
                string orderByAll = "", //完全替换排序条件
                string rootId=""
            )
        {
            // TODO： 关键字查询类型 string keyType="1", //1全部，2标题，3内容

            List<JuActivityInfo> result = new List<JuActivityInfo>();

            StringBuilder strWhere = new StringBuilder();
            if (string.IsNullOrEmpty(isDelete))
            {
                strWhere.Append(" IsDelete = 0 ");
            }
            else
            {
                strWhere.Append(" IsDelete = 1 ");
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                strWhere.AppendFormat(" AND TStatus In ({0}) ", status);
            }
            if (!showHide) strWhere.Append(" AND IsHide = 0 ");

            if (!hasSys) strWhere.Append(" AND IsSys = 0 ");

            #region 构造筛选条件
            if (isNoCommentAndReplayCount)
            {
                strWhere.AppendFormat(" AND CommentAndReplayCount = 0 ");
            }

            if (isHasCommentAndReplayCount)
            {
                strWhere.AppendFormat(" AND CommentAndReplayCount > 0 ");
            }

            if (!string.IsNullOrWhiteSpace(tags))
            {
                var tagArr = tags.Split(',').ToList();
                strWhere.Append("AND ( ");
                for (int i = 0; i < tagArr.Count; i++)
                {
                    if (i > 0)
                        strWhere.Append(" OR ");
                    strWhere.AppendFormat(" Tags LIKE '%{0}%' ", tagArr[i]);
                }
                strWhere.Append(" ) ");
            }

            if (!string.IsNullOrWhiteSpace(districtCode))
            {
                strWhere.AppendFormat(" AND DistrictCode = '{0}' ", districtCode);
            }

            if (!string.IsNullOrWhiteSpace(cityCode))
            {
                strWhere.AppendFormat(" AND CityCode = '{0}' ", cityCode);
            }

            if (!string.IsNullOrWhiteSpace(provinceCode))
            {
                strWhere.AppendFormat(" AND (ProvinceCode = '{0}' OR ProvinceCode = '0') ", provinceCode);
            }

            if (!string.IsNullOrWhiteSpace(preId))
            {
                strWhere.AppendFormat(" AND PreId = {0} ", preId);
            }

            if (!string.IsNullOrWhiteSpace(articleTypeEx1))
            {
                strWhere.AppendFormat(" AND ArticleTypeEx1 = '{0}' ", articleTypeEx1);
            }
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", websiteOwner);
            }

            if (!string.IsNullOrWhiteSpace(articleType))
            {
                strWhere.AppendFormat(" AND ArticleType = '{0}' ", articleType);
            }
            else
            {
                strWhere.AppendFormat(" AND ArticleType in('article','activity') ");
            }

            if (isSpread != null)
            {
                strWhere.AppendFormat(" AND IsSpread = {0} ", isSpread);
            }

            if (!string.IsNullOrWhiteSpace(userID))
            {
                strWhere.AppendFormat(" AND UserID = '{0}' ", userID);
            }

            if (!string.IsNullOrWhiteSpace(activityAddress))
            {
                strWhere.AppendFormat(" AND ActivityAddress Like '%{0}%' ", activityAddress);
            }

            if (!string.IsNullOrWhiteSpace(activityFutureDay))
            {
                int activityStartDateInt = 0;

                if (int.TryParse(activityFutureDay, out activityStartDateInt))
                {
                }
                if (activityStartDateInt > 0)
                {
                    DateTime searchDate = DateTime.Now.AddDays(activityStartDateInt);
                    strWhere.AppendFormat(" AND ActivityStartDate between '{0}' and '{1}' ", DateTime.Now, searchDate);
                }
            }

            if (!string.IsNullOrWhiteSpace(recommendCate))
            {
                if (recommendCate == "无分类" || string.IsNullOrWhiteSpace(recommendCate))
                {
                    strWhere.AppendFormat(" AND( RecommendCate = '' or RecommendCate is null )");
                }
                else
                {
                    foreach (var item in recommendCate.Split(','))
                    {
                        strWhere.AppendFormat(" AND RecommendCate Like '%{0}%' ", item);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(isFee))
            {
                strWhere.AppendFormat(" AND IsFee = '{0}' ", isFee);
            }

           
            //int jid = 0;
            //if (int.TryParse(keyWord, out jid))
            //{
            //    strWhere.AppendFormat(" AND JuActivityID={0} ", jid);
            //}else if(!string.IsNullOrWhiteSpace(keyWord)){
            //     strWhere.AppendFormat(" AND (ActivityName like '%{0}%' or Summary like '%{0}%' )", keyWord);
            //}
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                if (keywordSearchAuthor) //模糊查询作者昵称
                {
                    int uTotal = 0;
                    List<UserInfo> authors = new BLLUser().FindList(out uTotal, int.MaxValue, 1, "", keyWord, websiteOwner, null, "AutoID,UserID");
                    if (authors.Count > 0)
                    {
                        string authorStrings = ZentCloud.Common.MyStringHelper.ListToStr(authors.Select(p => p.UserID).ToList(), "'", ",");
                        strWhere.AppendFormat(" AND (UserID In({1}) or ActivityName like '%{0}%' or Convert(Nvarchar(50),JuActivityID)='{0}' or Summary like '%{0}%' )", keyWord, authorStrings);
                    }
                    else
                    {
                        strWhere.AppendFormat(" AND (ActivityName like '%{0}%' or Convert(Nvarchar(50),JuActivityID)='{0}' or Summary like '%{0}%' )", keyWord);
                    }
                }
                else
                {
                    strWhere.AppendFormat(" AND (ActivityName like '%{0}%' or Convert(Nvarchar(50),JuActivityID)='{0}' or Summary like '%{0}%' )", keyWord);
                }
            }

            if (!string.IsNullOrEmpty(categoryId) && categoryId != "0" && !categoryId.Contains(","))
            {
                categoryId = new BLLArticleCategory().GetCateAndChildIds(int.Parse(categoryId));//获取下面的子分类
                if (string.IsNullOrEmpty(categoryId)) categoryId = "-1";
                strWhere.AppendFormat(" AND ( CategoryId in ({0})  OR RootCateId IN ({0})  )", categoryId);
            }
            else if (!string.IsNullOrEmpty(categoryId) && categoryId.Contains(","))
            {
                categoryId = "'" + categoryId.Replace(",", "','") + "'";
                strWhere.AppendFormat(" AND CategoryId in ({0}) ", categoryId);
            }
            if (isShowEndDateData)
            {
                strWhere.AppendFormat(" AND (ActivityEndDate>getdate() or ActivityEndDate is null )");
            }
                        if (isForward)
            {
                strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM ZCJ_ActivityForwardInfo WHERE ");
                if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat(" WebsiteOwner='{0}' AND", websiteOwner);
                strWhere.AppendFormat(" ZCJ_ActivityForwardInfo.ActivityId=JuActivityID)");
            }

            if (!string.IsNullOrWhiteSpace(createDateStart))
            {
                strWhere.AppendFormat(" AND CreateDate>='{0}'", createDateStart);
            }
            if (!string.IsNullOrWhiteSpace(createDateEnd))
            {
                strWhere.AppendFormat(" AND CreateDate<='{0}'", createDateEnd);
            }
            if (!string.IsNullOrEmpty(rootId))
            {
                strWhere.AppendFormat(" AND RootCateId='{0}' ", rootId);
            }
            #endregion

            //排序顺序
            //1.排序号降序 
            //2.状态（是否隐藏） 
            //3.时间 （计算最接近的时间，取当前时间小时差的绝对值，进行中优先，未开始次(修正-5年)，已结束最末(修正-20年))
            //4.活动编号
            string strOrderBy = string.Format(@"ISNULL(Sort,0) DESC,IsHide asc,ISNULL(abs(datediff(hh,GetDate(),case when GetDate() between [ActivityStartDate] and ISNULL([ActivityEndDate],[ActivityStartDate]) then ISNULL([ActivityEndDate],[ActivityStartDate]) when GetDate() < [ActivityStartDate] then dateadd(year,5,[ActivityStartDate]) when GetDate() > ISNULL([ActivityEndDate],[ActivityStartDate]) then dateadd(year,-20,ISNULL([ActivityEndDate],[ActivityStartDate])) end)),0) asc,[JuActivityID] desc");

            if (string.IsNullOrWhiteSpace(orderByAll)) //完全替换排序条件
            {
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    strOrderBy = string.Format("{0},{1}", orderBy, strOrderBy);
                }
            }
            else
            {
                strOrderBy = orderByAll;
            }

            totalCount = GetCount<JuActivityInfo>(strWhere.ToString());

            if (pageIndex != 0)
            {
                if (!string.IsNullOrEmpty(column))
                {
                    result = GetColList<JuActivityInfo>(pageSize, pageIndex, strWhere.ToString(), strOrderBy, column);
                }
                else
                {
                    result = GetLit<JuActivityInfo>(pageSize, pageIndex, strWhere.ToString(), strOrderBy);
                }

            }
            else
            {
                result = GetList<JuActivityInfo>(int.MaxValue, strWhere.ToString(), strOrderBy);
            }
            return result;
        }


        /// <summary>
        /// 获取聚活动数据
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<JuActivityInfo> QueryJuActivityData(string userID, int pageIndex = 0, int pageSize = 0)
        {
            int totalCount = 0;
            return QueryJuActivityData("search", out totalCount, null, null, null, null, null, pageIndex, pageSize, userID);
        }
        /// <summary>
        /// 类型Id获取该类文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int QueryJuActivityCountByCategoryId(string categoryId, string articleType, bool showHide)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" IsDelete = 0 ", categoryId);
            if (!showHide) strWhere.AppendFormat(" and IsHide=0 ");
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                strWhere.AppendFormat(" AND CategoryId = '{0}' ", categoryId);
            }
            if (!string.IsNullOrWhiteSpace(articleType))
            {
                strWhere.AppendFormat(" AND ArticleType = '{0}' ", articleType);
            }
            int result = GetCount<JuActivityInfo>(strWhere.ToString());
            return result;
        }

        /// <summary>
        /// 获取JuActivity内容列表
        /// </summary>
        /// <param name="contentType">内容类型</param>
        /// <param name="preId"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="currUserId"></param>
        /// <param name="cateId"></param>
        /// <returns>
        /// [{文章id，评论id，评论时间，回复数，点赞数，当前用户是否点赞，当前用户是否已收藏， {回复了哪条回复} }]
        /// </returns>
        public List<JuActivityInfo> GetJuActivityList(
                string contentType,
                string preId,
                out int totalCount,
                int pageIndex,
                int pageSize,
                string ownerUserId,
                string currUserId,
                string cateId,
                string websiteOwner,
                string keyWord = "",
                string tags = "",
                string provinceCode = "",
                string cityCode = "",
                string districtCode = "",
                bool isNoCommentAndReplayCount = false,//是否查询没有评论内容的列表
                string orderBy = "",//排序
                bool isHasCommentAndReplayCount = false,
                bool showHide = true,
                string status = null,
                bool isShowEndDateData = false,
                string column = "",
                bool hasStatistics = true,
                bool hasAuthor = true,
                bool isForward = false, //是否微吸粉
                string createDateStart = "",//创建时间开始
                string createDateEnd = "",//创建时间结束
                bool keywordSearchAuthor = false,//是否关键字查询作者
                string orderByAll = "", //完全替换排序条件
                bool hideSubCount = false,//不查子内容数
                bool hideReplyUser = false,//不查回复某人
                bool hideProvince = false,//不查省份
                string rootId=""
            )
        {
            List<JuActivityInfo> orgDataList =
                QueryJuActivityData(null, out totalCount, null, null, null, null, keyWord, pageIndex, pageSize
                , ownerUserId, null, contentType, websiteOwner, null, cateId, preId, provinceCode, cityCode, districtCode
                , tags, isNoCommentAndReplayCount, orderBy, isHasCommentAndReplayCount, showHide, false, null, status, isShowEndDateData, column
                ,isForward, createDateStart, createDateEnd, keywordSearchAuthor, orderByAll, rootId);

            List<JuActivityInfo> dataList = new List<JuActivityInfo>();

            foreach (var item in orgDataList)
            {
                if (hasStatistics || hasAuthor)
                {
                    dataList.Add(FilterJuActivityExInfo(item, currUserId, hasStatistics,
                        hasAuthor, hideSubCount, hideReplyUser, hideProvince));
                }
                else
                {
                    dataList.Add(item);
                }
            }
            return dataList;
        }


        /// <summary>
        /// 填充单个JuActivity扩展对象
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currUserId"></param>
        /// <returns></returns>
        public JuActivityInfo FilterJuActivityExInfo(JuActivityInfo item, string currUserId = "",
                bool hasStatistics = true,
                bool hasAuthor = true,
                bool hideSubCount = false,
                bool hideReplyUser = false,
                bool hideProvince = false)
        {
            /*
             * 单个评论对象：
             * {文章id，评论id，评论时间，回复数，点赞数，当前用户是否点赞，当前用户是否已收藏，{ 发布人对象 }， {回复了哪条回复} }
             * 
             */

            BLLUser userBll = new BLLUser();
            JuActivityInfo result = item;
            if (hasStatistics)
            {
                BLLCommRelation commRelationBll = new BLLCommRelation();

                if (!hideSubCount)
                {
                    result.SubCount = GetJuActivitySubCount(item.JuActivityID, false);
                }

                //result.CommentCount = new BLLReview().GetReviewCount(Enums.ReviewTypeKey.ArticleComment, item.JuActivityID.ToString(),null);

                //result.PraiseCount = commRelationBll.GetRelationCount(Enums.CommRelationType.JuActivityPraise, item.JuActivityID.ToString(), null);
                //result.FavoriteCount = commRelationBll.GetRelationCount(Enums.CommRelationType.JuActivityFavorite, item.JuActivityID.ToString(), null);
                //result.FollowCount = commRelationBll.GetRelationCount(Enums.CommRelationType.JuActivityFollow, item.JuActivityID.ToString(), null);

                var currUserIsPraise = false;
                var currUserIsFavorite = false;
                var currUserIsFollow = false;

                if (!string.IsNullOrWhiteSpace(currUserId))
                {
                    //判断当前用户是否已赞
                    if (commRelationBll.ExistRelation(Enums.CommRelationType.JuActivityPraise, item.JuActivityID.ToString(), currUserId)) currUserIsPraise = true;
                    //判断是否已收藏
                    if (commRelationBll.ExistRelation(Enums.CommRelationType.JuActivityFavorite, item.JuActivityID.ToString(), currUserId)) currUserIsFavorite = true;
                    //判断是否已关注
                    if (commRelationBll.ExistRelation(Enums.CommRelationType.JuActivityFollow, item.JuActivityID.ToString(), currUserId)) currUserIsFollow = true;
                }

                //回复的回复查询
                if (!hideReplyUser)
                {
                    var replyRelation = commRelationBll.GetRelationInfo(Enums.CommRelationType.JuActivityReplyToReply, item.JuActivityID.ToString(), null);
                    string replyToUserId = string.Empty, replyToUserName = string.Empty;

                    if (replyRelation != null)
                    {
                        var replyToObj = GetJuActivity(Convert.ToInt32(replyRelation.RelationId), false);
                        if (replyToObj != null)
                        {
                            result.ReplayToUser = userBll.GetUserInfo(replyToObj.UserID);
                        }
                    }
                }

                result.CurrUserIsFavorite = currUserIsFavorite;
                result.CurrUserIsPraise = currUserIsPraise;
                result.CurrUserIsFollow = currUserIsFollow;

                if (!hideProvince)
                {
                    result.Province = new BLLKeyValueData().GetDataDefVaule("Province", item.ProvinceCode);
                }
            }
            if (hasAuthor) result.PubUser = userBll.GetUserInfo(item.UserID);
            return result;
        }


        /// <summary>
        /// 获取内容的子数
        /// </summary>
        /// <param name="preId"></param>
        /// <returns></returns>
        public int GetJuActivitySubCount(int preId, bool showHide)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("PreId = '{0}'", preId);
            if (!showHide) sbSql.AppendFormat(" and IsHide=0 ");
            int result = GetCount<JuActivityInfo>(sbSql.ToString());
            return result;
        }

        /// <summary>
        /// 查询聚活动报名数据
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public List<ActivityDataInfo> QueryJuActivitySignUpData(int jid, bool showHide = true)
        {
            return GetList<ActivityDataInfo>(string.Format(" ActivityID = '{0}' AND IsDelete = 0 order by InsertDate DESC  ", GetJuActivity(jid, showHide).SignUpActivityID));
        }

        /// <summary>
        /// 根据活动ID获取活动
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public JuActivityInfo GetJuActivity(int jid, bool showHide = true, string websiteOwner = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("JuActivityID = '{0}' and IsDelete=0 ", jid);
            if (!showHide) sbSql.AppendFormat(" and IsHide=0 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" and WebsiteOwner='{0}' ", websiteOwner);

            return Get<JuActivityInfo>(sbSql.ToString());
        }

        /// <summary>
        /// 根据活动报名表ID获取活动
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        public JuActivityInfo GetJuActivityByActivityID(string aid, bool showHide = true)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("SignUpActivityID = '{0}'", aid);
            if (!showHide) sbSql.AppendFormat(" and IsHide=0 ");
            return Get<JuActivityInfo>(sbSql.ToString());
        }

        /// <summary>
        /// 根据专家UserID获取最新动态
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JuActivityInfo GetNewActivityByUserId(string UserID, bool showHide)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner = '{0}'", WebsiteOwner);
            sbSql.AppendFormat(" AND UserID = '{0}'", UserID);
            if (!showHide) sbSql.AppendFormat(" and IsHide=0 ");
            sbSql.AppendFormat(" AND IsDelete = 0 ");
            List<JuActivityInfo> JuActivityInfos = GetList<JuActivityInfo>(1, sbSql.ToString(), " JuActivityID Desc");
            return JuActivityInfos.Count > 0 ? JuActivityInfos[0] : null;
        }
        /// <summary>
        /// 获取分类最新的一条
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        public JuActivityInfo GetNewActivityByCateId(string cateId, bool showHide)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner = '{0}'", WebsiteOwner);
            sbSql.AppendFormat(" AND CategoryId = '{0}'", cateId);
            if (!showHide) sbSql.AppendFormat(" and IsHide=0 ");
            sbSql.AppendFormat(" AND IsDelete = 0 ");
            List<JuActivityInfo> JuActivityInfos = GetList<JuActivityInfo>(1, sbSql.ToString(), " JuActivityID Desc");
            return JuActivityInfos.Count > 0 ? JuActivityInfos[0] : null;
        }

        ///// <summary>
        ///// 获取指定专家的统计信息
        ///// </summary>
        ///// <param name="masterID">专家ID</param>
        ///// <param name="feedBackTotalCount">留言总数</param>
        ///// <param name="feedBackTodayNewCount">今日留言总数</param>
        ///// <param name="linkerInfoTotalCount">联系人信息总数</param>
        ///// <param name="linkerInfoTodayNewCount">今日联系人信息总数</param>
        //public void GetJuMasterStatis(
        //    string masterID,
        //    out int feedBackTotalCount,
        //    out int feedBackTodayNewCount,
        //    out int linkerInfoTotalCount,
        //    out int linkerInfoTodayNewCount
        //    )
        //{
        //    feedBackTotalCount = GetCount<JuMasterFeedBack>(string.Format("MasterID = '{0}'", masterID));
        //    feedBackTodayNewCount = GetCount<JuMasterFeedBack>(string.Format("MasterID = '{0}' AND SubmitDate >= '{1}' ", masterID, DateTime.Now.ToShortDateString()));
        //    linkerInfoTotalCount = GetCount<JuMasterUserLinkerInfo>(string.Format("MasterID = '{0}'", masterID));
        //    linkerInfoTodayNewCount = GetCount<JuMasterUserLinkerInfo>(string.Format("MasterID = '{0}' AND SubmitDate >= '{1}' ", masterID, DateTime.Now.ToShortDateString()));



        //}

        /// <summary>
        /// 分查询转发排名
        /// </summary>
        /// <param name="monitorPlanID">监测ID(由JuActivity表可找到)</param>
        /// <param name="orderBy">排序：IP、PV</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable QueryJuActivitySpreadRank(int monitorPlanID, string orderBy, int pageIndex, int pageSize)
        {
            DataTable dt = new DataTable();

            try
            {
                string tmpTableName = "#" + Guid.NewGuid().ToString().Replace("-", "");

                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("select b.LinkName,c.Name,count(SourceIP) as PV,count(distinct(SourceIP)) as IP,a.LinkID into {0}  ", tmpTableName);
                strSql.AppendFormat("from  ");
                strSql.AppendFormat("ZCJ_MonitorEventDetailsInfo a left join ZCJ_MonitorLinkInfo b on a.LinkID = b.LinkID  ");
                strSql.AppendFormat("left join dbo.ZCJ_WXMemberInfo c on c.MemberID = b.WXMemberID  ");
                strSql.AppendFormat("where b.MonitorPlanID = {0}  ", monitorPlanID);
                strSql.AppendFormat("group by a.LinkID,b.LinkName,c.Name  ");
                strSql.AppendFormat("order by {0} desc;  ", orderBy);

                strSql.AppendFormat("select top {0} * from   ", pageSize);
                strSql.AppendFormat("(select top {0} row_number() over(order by {1} desc) as COL_ROWNUMBER, * from {2} )   ", pageSize * pageIndex, orderBy, tmpTableName);
                strSql.AppendFormat("TABLE_ORDERDATA  ");
                strSql.AppendFormat("where COL_ROWNUMBER > {0};", (pageIndex - 1) * pageSize);

                strSql.AppendFormat("drop table {0}", tmpTableName);

                dt = ZCDALEngine.DbHelperSQL.Query(strSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// 查询全部转发排名
        /// </summary>
        /// <param name="monitorPlanID"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataTable QueryJuActivitySpreadRank(int monitorPlanID, string orderBy)
        {
            DataTable dt = new DataTable();

            try
            {
                string tmpTableName = "#" + Guid.NewGuid().ToString().Replace("-", "");

                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("select b.LinkName,c.Name,count(SourceIP) as PV,count(distinct(SourceIP)) as IP,a.LinkID ");
                strSql.AppendFormat("from  ");
                strSql.AppendFormat("ZCJ_MonitorEventDetailsInfo a left join ZCJ_MonitorLinkInfo b on a.LinkID = b.LinkID  ");
                strSql.AppendFormat("left join dbo.ZCJ_WXMemberInfo c on c.MemberID = b.WXMemberID  ");
                strSql.AppendFormat("where b.MonitorPlanID = {0}  ", monitorPlanID);
                strSql.AppendFormat("group by a.LinkID,b.LinkName,c.Name  ");
                strSql.AppendFormat("order by {0} desc;  ", orderBy);

                dt = ZCDALEngine.DbHelperSQL.Query(strSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// 根据聚活动创建报名任务实体
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Model.ActivityInfo CreateSignUpActivityModelByJuActivity(Model.JuActivityInfo model, string userID)
        {
            //自动创建报名活动
            ActivityInfo signUpActivityModel = new ActivityInfo();
            signUpActivityModel.ActivityID = GetGUID(TransacType.ActivityAdd);
            signUpActivityModel.UserID = userID;
            signUpActivityModel.ActivityName = model.ActivityName;
            signUpActivityModel.ActivityDate = model.ActivityStartDate;
            signUpActivityModel.ActivityAddress = model.ActivityAddress;
            signUpActivityModel.ActivityWebsite = model.ActivityWebsite;
            signUpActivityModel.ActivityStatus = 1;
            signUpActivityModel.LimitCount = 100;
            signUpActivityModel.ActivityDescription = string.Format("该任务为活动{0}自动创建", model.JuActivityID);

            return signUpActivityModel;
        }



        /// <summary>
        /// 过滤没权限的组ID列表串，组-用户 验证处理，防止恶意获取到别人的发送列表
        /// </summary>
        /// <param name="strGroupIds"></param>
        /// <returns></returns>
        //private string FilterNoPmsJuActivityIds(string strJuactivityIds)
        //{

        ////聚活动- 验证处理，防止恶意获取到别人的活动
        //List<string> groupList = GetList<JuActivityInfo>(string.Format(" UserID = '{0}',
        //        this.userInfo.UserID,
        //        strJuactivityIds
        //    )).Select(p => p.).ToList();

        //if (strGroupIds.Contains(",0,") || strGroupIds.StartsWith("0,") || strGroupIds == "0")
        //{
        //    groupList.Add("0");
        //}

        //strGroupIds = Common.StringHelper.ListToStr<string>(groupList, "'", ",");
        //return strGroupIds;


        // }


        ///// <summary>
        ///// 根据文章ID返回文章Html
        ///// </summary>
        ///// <param name="juActivityID">文章ID</param>
        ///// <param name="currOpenerOpenID">当前打开者的OpenID</param>
        ///// <param name="currUrl">当前打开的链接地址</param>
        ///// <returns>Html</returns>
        //public string GetArticleHtmlByArticleID(int juActivityID, string currOpenerOpenID = "", string currUrl = "")
        //{
        //    try
        //    {
        //        ///最终输出的Html
        //        string Html = string.Empty; ;
        //        JuActivityInfo articleInfo = Get<JuActivityInfo>(string.Format("JuActivityID={0}", juActivityID));
        //        if (articleInfo != null)//检查文章是否存在
        //        {
        //            if (!articleInfo.IsDelete.Equals(1))//检查文章是否删除
        //            {
        //                if (articleInfo.IsHide.Equals(0))
        //                {

        //                    UserInfo userInfo = Get<UserInfo>(string.Format("UserID='{0}'", articleInfo.UserID));//文章发布者信息
        //                    SystemSet systemset = Get<SystemSet>("");//系统配置信息

        //                    #region 更新打开人次
        //                    if (articleInfo.OpenCount == null)
        //                    {
        //                        articleInfo.OpenCount = 0;
        //                    }
        //                    articleInfo.OpenCount++;
        //                    Update(articleInfo);


        //                    #endregion

        //                    #region 内部链接
        //                    if (articleInfo.IsByWebsiteContent.Equals(0))//内部链接
        //                    {
        //                        if (articleInfo.ArticleTemplate.Equals(0))// 空模板
        //                        {

        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit0.htm"), Encoding.UTF8);



        //                        }
        //                        if (articleInfo.ArticleTemplate.Equals(1))// 微信官方模板
        //                        {
        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/weixin/ArticleTemplate/jubit1.htm"), Encoding.UTF8);

        //                        }

        //                        if (articleInfo.ArticleTemplate.Equals(2))// 聚比特模板
        //                        {

        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit2.htm"), Encoding.UTF8);

        //                        }


        //                        if (articleInfo.IsSignUpJubit > 0)
        //                        {
        //                            //读取个性化模板
        //                            Html = Html.Replace("$JUTP-TPSignForm$", Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/TPSignForm.htm"), Encoding.UTF8));
        //                        }
        //                        else
        //                        {
        //                            Html = Html.Replace("$JUTP-TPSignForm$", "");

        //                        }



        //                    }
        //                    #endregion

        //                    #region 外部链接
        //                    else//外部链接
        //                    {
        //                        //下载外部链接源代码
        //                        Html = Common.MySpider.GetPageSourceForUTF8(articleInfo.ActivityWebsite);

        //                        if (!string.IsNullOrWhiteSpace(currUrl))
        //                        {
        //                            List<string> tmpArr = currUrl.Split('&').ToList();
        //                            string tmpStr = "";

        //                            int i = 0;

        //                            foreach (var item in tmpArr)
        //                            {
        //                                if (item.Contains("message=") || item.Contains("status="))//该两个参数一般为状态提醒码
        //                                    continue;
        //                                if (i > 0)
        //                                    tmpStr += "&";
        //                                tmpStr += item;
        //                                i++;
        //                            }

        //                            //针对微信网页转发分享链接，需要把网页分享地址替换成当前页面地址，否则分享出去就是微信自己原来的页面了（目的是分享出去是我们自己的链接）
        //                            Html = Html.Replace(articleInfo.ActivityWebsite, tmpStr);//转发的时候需要把msg去除
        //                        }
        //                        //document.domain，这个也必须替换掉

        //                        Html = Html.Replace("document.domain", " var documentDomainTeml");
        //                    }
        //                    #endregion

        //                    #region 追加报名表单
        //                    //追加报名表单                
        //                    // 符合条件的情况
        //                    //1:使用外部链接并使用报名.
        //                    //2:使用内部链接并使用微信官方模板并使用报名.
        //                    //if ((articleInfo.IsByWebsiteContent.Equals(1) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)))//使用外部链接并使用报名.
        //                    //    ||
        //                    //    //使用内部链接并使用微信官方模板并使用报名.
        //                    //    (articleInfo.IsByWebsiteContent.Equals(0) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)) && articleInfo.ArticleTemplate.Equals(1))
        //                    //    )

        //                    if (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2))//有报名
        //                    {
        //                        StringBuilder sbCheckForm = new StringBuilder();//追加检查form
        //                        StringBuilder sbAppend = new StringBuilder();//Html后追加的html
        //                        StringBuilder sbSignInHtml = new StringBuilder();//报名表单 格式<tr></tr>
        //                        StringBuilder sbcurrMemberInfo = new StringBuilder();//当前会员隐藏信息
        //                        //FORMDATA
        //                        ActivityInfo planactivityInfo = Get<ActivityInfo>(string.Format("ActivityID='{0}'", articleInfo.SignUpActivityID));

        //                        List<ActivityFieldMappingInfo> activityFieldMappingInfoList; //= GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' Order by ExFieldIndex ASC", planactivityInfo.ActivityID));
        //                        activityFieldMappingInfoList = new BLLActivity("").GetActivityFieldMappingList(planactivityInfo.ActivityID);

        //                        WXMemberInfo currOpenerMemberInfo = new WXMemberInfo();
        //                        //Dictionary<string, string> currOpenerInfo = new Dictionary<string, string>();
        //                        bool currOpenerIsMember = false;
        //                        if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
        //                        {
        //                            //如果有openID传入，则检查是否是会员，是的话则查询出姓名 手机 邮箱数据
        //                            currOpenerMemberInfo = Get<WXMemberInfo>(string.Format(" UserID = '{0}' and WeixinOpenID = '{1}' ", articleInfo.UserID, currOpenerOpenID));
        //                            if (currOpenerMemberInfo != null)
        //                                currOpenerIsMember = true;

        //                        }

        //                        if (!string.IsNullOrWhiteSpace(currOpenerOpenID) && !currOpenerIsMember)
        //                        {
        //                            sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td>{0}</td></tr>", systemset.IsNotWXMemberSignInTipMsg);
        //                        }

        //                        //if (!currOpenerIsMember)
        //                        //{
        //                        //    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td>{0}</td></tr>", systemset.IsNotWXMemberSignInTipMsg);
        //                        //}

        //                        //其它报名字段
        //                        foreach (ActivityFieldMappingInfo item in activityFieldMappingInfoList)
        //                        {
        //                            if (item.FieldType != 1)//普通字段
        //                            {
        //                                if (currOpenerIsMember)
        //                                {
        //                                    bool tmpIsMp = false;//是否对应
        //                                    string tmpValue = "";

        //                                    //姓名
        //                                    if (item.FieldName.Equals("name", StringComparison.OrdinalIgnoreCase) && !tmpIsMp)
        //                                    {
        //                                        tmpValue = currOpenerMemberInfo.Name;
        //                                        tmpIsMp = true;
        //                                    }

        //                                    //手机
        //                                    if (item.FieldName.Equals("phone", StringComparison.OrdinalIgnoreCase) && !tmpIsMp)
        //                                    {
        //                                        tmpValue = currOpenerMemberInfo.Phone;
        //                                        tmpIsMp = true;
        //                                    }

        //                                    //判断是不是公司，是的话则隐藏并赋值
        //                                    if (item.MappingName == "公司" && !tmpIsMp)
        //                                    {
        //                                        tmpValue = currOpenerMemberInfo.Company;
        //                                        tmpIsMp = true;
        //                                    }

        //                                    //判断是不是职位，是的话则隐藏并赋值
        //                                    if (item.MappingName == "职位" && !tmpIsMp)
        //                                    {
        //                                        tmpValue = currOpenerMemberInfo.Postion;
        //                                        tmpIsMp = true;
        //                                    }

        //                                    if (!tmpIsMp)
        //                                    {
        //                                        //判断是不是邮箱，是的话则隐藏并赋值
        //                                        List<string> tmpEmailMp = new List<string>() { "邮箱", "邮件", "email" };
        //                                        foreach (var i in tmpEmailMp)
        //                                        {
        //                                            if (item.MappingName.ToLower().Contains(i))
        //                                            {
        //                                                tmpValue = currOpenerMemberInfo.Email;
        //                                                tmpIsMp = true;
        //                                                break;
        //                                            }
        //                                        }
        //                                    }

        //                                    if (tmpIsMp)
        //                                    {
        //                                        sbcurrMemberInfo.AppendFormat("<input type=\"hidden\" value=\"{0}\"  name=\"{1}\" id=\"{1}\"  />", tmpValue, item.FieldName);//邮箱
        //                                        continue;
        //                                    }

        //                                }
        //                                sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"{0}\">{1}</label></td></tr>", item.FieldName, item.MappingName);

        //                                if (item.IsMultiline.Equals(1))//<textarea name="K1" id="txtContent" style="height: 100px;" placeholder="请详细描述您的建议、意见、问题等。"></textarea>
        //                                    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><textarea rows=\"5\" type=\"text\" name=\"{0}\" id=\"{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;height: 200px;\"></textarea> </td></tr>", item.FieldName, item.MappingName);
        //                                else
        //                                    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"{0}\" id=\"{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;\"/> </td></tr>", item.FieldName, item.MappingName);

        //                            }
        //                            else//微信推广字段
        //                            {
        //                                sbSignInHtml.AppendFormat("<tr><td><input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"$CCWXTG-LINKID$\" /></td></tr> ", item.FieldName);
        //                            }

        //                            if (item.FieldIsNull.Equals(1))
        //                            {

        //                                sbCheckForm.AppendFormat("if (!document.getElementById(\"{0}\").value)", item.FieldName);

        //                                sbCheckForm.AppendLine("{");
        //                                sbCheckForm.AppendFormat("alert(\"请输入{0}\");", item.MappingName);
        //                                sbCheckForm.AppendLine(" return false;");
        //                                sbCheckForm.AppendLine("}");
        //                            }


        //                        }
        //                        //其它报名字段

        //                        //其它报名信息
        //                        //sbSignInHtml.AppendLine("<div  style=\"margin-top: 10px;\">");
        //                        sbSignInHtml.AppendLine("<tr><td>");
        //                        sbSignInHtml.AppendFormat("<input id=\"activityID\" type=\"hidden\" value=\"{0}\" name=\"ActivityID\" />", planactivityInfo.ActivityID);//活动ID
        //                        sbSignInHtml.AppendFormat("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\" />", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID));//外部登录名
        //                        sbSignInHtml.AppendFormat("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\" />", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password));//外部登录密码

        //                        if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
        //                        {
        //                            //如果当前微信OpenID不为空，则添加到页面去
        //                            sbSignInHtml.AppendFormat("<input id=\"{1}\" type=\"hidden\" value=\"{0}\" name=\"{1}\" />", currOpenerOpenID, systemset.WXCurrOpenerOpenIDKey);//当前的微信OpenID
        //                        }


        //                        sbSignInHtml.AppendLine(sbcurrMemberInfo.ToString());//添加会员信息


        //                        //不允许重复字段
        //                        if (planactivityInfo.DistinctKeys != null)
        //                        {
        //                            if (!string.IsNullOrEmpty(planactivityInfo.DistinctKeys))
        //                            {
        //                                sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"DistinctKeys\">", planactivityInfo.DistinctKeys);
        //                            }
        //                        }
        //                        sbSignInHtml.AppendLine("</td></tr>");
        //                        //不允许重复字段

        //                        if (articleInfo.IsByWebsiteContent.Equals(1))//外部链接报名
        //                        {
        //                            sbAppend.Append(Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/wx_externalsignuptemplate.htm"), Encoding.UTF8));//外部报名表单模板
        //                            sbAppend = sbAppend.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //                            if (Html.Contains("</body>"))
        //                            {
        //                                Html = Html.Replace("</body>", string.Format("{0}</body>", sbAppend));
        //                            }
        //                            else
        //                            {
        //                                Html += sbAppend.ToString();
        //                            }

        //                            Html = Html.Replace("$CCWX-CHECKFORM$", sbCheckForm.ToString());


        //                        }
        //                        else
        //                        {

        //                            //内部链接报名
        //                            Html = Html.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //                        }

        //                    }


        //                    //追加报名表单 
        //                    #endregion

        //                    #region 替换微信远程图片为本地路径

        //                    System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //                    System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(Html);
        //                    foreach (System.Text.RegularExpressions.Match match in matches)
        //                    {
        //                        String name = match.ToString();
        //                        string reomoteurl = match.Groups["imgUrl"].Value;
        //                        //微信特殊处理"<img src=\"http://res.wx.qq.com/mmbizwap/zh_CN/htmledition/images/big_loading19d82d.gif\" onerror=\"this.parentNode.removeChild(this)\" data-src=\"http://mmbiz.qpic.cn/mmbiz/lybKlMLvO2WzibKRgibMx82Zsg2Lhts4dibN9JNicHeTrzUJfiarSia2GaIjtlPyictzVxPmEUH4iaeTuXszSbO7CbTibdQ/0\" />"
        //                        if (name.Contains("data-src="))
        //                        {
        //                            string tmp = Common.StringHelper.CutByStarTag(name, "data-src=", true);// name.Substring(name.IndexOf("data-src"));

        //                            tmp = tmp.Substring(1, tmp.IndexOf(" ") - 2);
        //                            reomoteurl = tmp;
        //                        }

        //                        if (reomoteurl.StartsWith("http://mmsns.qpic.cn/") || reomoteurl.StartsWith("http://mmbiz.qpic.cn/") || reomoteurl.StartsWith("http://res.wx.qq.com/"))//替换微信官方图片地址为本地 
        //                        {
        //                            Html = Html.Replace(name, string.Format("<img src=\"http://{0}:{1}{2}\">", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, DownLoadRemoteImage(reomoteurl)));

        //                        }


        //                    }

        //                    #endregion

        //                    #region 替换文章标签

        //                    if (Html.Contains("$CCWX-ARTICLEIMAGE$"))//替换分享图片地址
        //                    {
        //                        if (articleInfo.ThumbnailsPath.ToLower().StartsWith("http://"))
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLEIMAGE$", articleInfo.ThumbnailsPath);
        //                        }
        //                        else
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLEIMAGE$", string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, articleInfo.ThumbnailsPath));

        //                        }
        //                    }

        //                    if (Html.Contains("$CCWXCALLBACKURL$"))//替换回调地址 
        //                    {
        //                        Html = Html.Replace("$CCWXCALLBACKURL$", string.Format("http://{0}:{1}{2}?id={3}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.FilePath, articleInfo.JuActivityID));
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLETITLE$"))//替换标题
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLETITLE$", articleInfo.ActivityName);//替换标题

        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLETIME$"))//替换时间
        //                    {
        //                        if (articleInfo.IsSignUpJubit.Equals(0))//普通文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", string.Format("{0:f}", articleInfo.CreateDate));//替换时间
        //                        }
        //                        else//活动文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", "开始时间:" + string.Format("{0:f}", articleInfo.ActivityStartDate));//替换时间
        //                        }


        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEENDTIME$"))//替换时间
        //                    {
        //                        if (articleInfo.IsSignUpJubit.Equals(0))//普通文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLEENDTIME$", "");
        //                        }
        //                        else//活动文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", "结束时间:" + string.Format("{0:f}", articleInfo.ActivityEndDate));//替换时间
        //                        }


        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEADDRESS$"))//替换地址
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEADDRESS$", articleInfo.ActivityAddress == null || articleInfo.ActivityAddress == "" ? userInfo.WeixinPublicName : articleInfo.ActivityAddress);//替换地址
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLECONTENT$"))//替换内容
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLECONTENT$", articleInfo.ActivityDescription);//替换内容
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEOPENCOUNT$"))//替换打开人次
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEOPENCOUNT$", articleInfo.PV.ToString());//替换打开人次
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEUPCOUNT$"))//替换赞人数
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEUPCOUNT$", articleInfo.UpCount.ToString());//替换赞人数
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEID$"))//替换文章ID
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEID$", articleInfo.JuActivityID.ToString());//替换文章ID
        //                    }

        //                    #endregion

        //                }
        //                else
        //                {
        //                    Html = "该文章不显示";
        //                }
        //            }
        //            else
        //            {
        //                Html = "该文章已经删除";
        //            }
        //        }
        //        else
        //        {
        //            Html = "不存在的文章";
        //        }


        //        return Html;//返回最终Html
        //    }
        //    catch (Exception ex)
        //    {

        //        return ex.ToString();
        //    }


        //}


        /// <summary>
        /// 生成文章活动html
        /// </summary>
        /// <param name="JuActivityInfo">文章活动</param>
        /// <param name="currOpenerOpenID">传进来的OpenId</param>
        /// <param name="currUrl">当前地址</param>
        /// <param name="spreadUser">推广人</param>
        /// <returns>文章活动html</returns>
        public string GetJuactivityHtml(JuActivityInfo juActivityInfo, string currOpenerOpenID = "", string currUrl = "",UserInfo spreadUser = null,UserInfo shareUser = null)
        {
            try
            {
                //是否在PC下访问
                bool isInPC = false;
                if (HttpContext.Current.Request.Browser.Platform.ToLower().StartsWith("win"))
                {
                    isInPC = true;
                }

                ///最终输出的Html
                string html = string.Empty;
                //JuActivityInfo juActivityInfo = Get<JuActivityInfo>(string.Format("JuActivityID={0}", juActivityID));
                UserInfo currentUserInfo = new UserInfo();

                if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
                {
                    currentUserInfo = bllUser.GetUserInfoByOpenId(currOpenerOpenID);
                }
                else
                {
                    //if (HttpContext.Current.Session[Common.SessionKey.UserID] != null)
                    //{
                    //    currUser = Get<ZentCloud.BLLJIMP.Model.UserInfo>(string.Format("UserID = '{0}'", HttpContext.Current.Session[Common.SessionKey.UserID]));
                    //}
                    currentUserInfo = bllUser.GetCurrentUserInfo();

                }
                ///收费
                string signUpbutton = string.Empty;
                ActivityDataInfo signUpData = null;
                if (currentUserInfo!=null)
                {

                    signUpData = bllActivity.GetActivityDataInfo(juActivityInfo.SignUpActivityID, currentUserInfo.UserID);
                    if (signUpData == null)
                    {
                        signUpData = bllActivity.GetActivityDataInfoByOpenId(juActivityInfo.SignUpActivityID, currentUserInfo.WXOpenId);
                    }

                }

                CompanyWebsite_Config companyWebsiteConfig = bllWebsite.GetCompanyWebsiteConfig();

                //if (juActivityInfo != null)//检查文章是否存在
                //{
                    if (!juActivityInfo.IsDelete.Equals(1))//检查文章是否删除
                    {
                        if (juActivityInfo.IsHide.Equals(0) || (juActivityInfo.ArticleType == "activity"))
                        {

                            UserInfo userInfo = bllUser.GetUserInfo(juActivityInfo.UserID);
                            //Get<UserInfo>(string.Format("UserID='{0}'", articleInfo.UserID));//文章发布者信息
                           // SystemSet systemSet = Get<SystemSet>("");//系统配置信息
                            //#region 更新打开人次
                            //if (articleInfo.OpenCount == null)
                            //{
                            //    articleInfo.OpenCount = 0;
                            //}
                            //articleInfo.OpenCount++;
                            //Update(articleInfo);

                            //#endregion


                            //if (articleInfo.IsByWebsiteContent.Equals(0))//内部链接
                            //{
                            #region 读取初始模板
                            if (juActivityInfo.ArticleTemplate.Equals(0))// 空模板
                            {

                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit0.htm"), Encoding.UTF8);



                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(1))// 微信官方模板
                            {
                                if (isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/weixin/ArticleTemplate/pc1.htm"), Encoding.UTF8);
                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/weixin/ArticleTemplate/jubit1.htm"), Encoding.UTF8);
                                }

                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(2))// 聚比特模板
                            {

                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit2.htm"), Encoding.UTF8);

                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(3)) //活动模板(有微信高级认证)
                            {

                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit3.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc2.htm"), Encoding.UTF8);

                                }

                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(4))//活动模板(无微信高级认证)
                            {


                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit4.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc2.htm"), Encoding.UTF8);

                                }
                            }

                            else if (juActivityInfo.ArticleTemplate.Equals(5))//活动模板(无微信高级认证)
                            {


                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit5.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit5.htm"), Encoding.UTF8);

                                }
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(6))//文章新模板
                            {


                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit6.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc1.htm"), Encoding.UTF8);

                                }
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(7))//活动模板
                            {


                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit7.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc2.htm"), Encoding.UTF8);

                                }
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(8))//申请模板
                            {


                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit8.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc2.htm"), Encoding.UTF8);

                                }
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(9))//收费模板
                            {


                                //if (!isInPC)
                                //{
                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit9.htm"), Encoding.UTF8);

                                //}
                                //else
                                //{
                                //    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc2.htm"), Encoding.UTF8);

                                //}
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(10))//活动模板免费
                            {


                                if (!isInPC)
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit10.htm"), Encoding.UTF8);

                                }
                                else
                                {
                                    html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/pc2.htm"), Encoding.UTF8);

                                }
                            }


                            else if (juActivityInfo.ArticleTemplate.Equals(11))//微信模板
                            {
                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit11.html"), Encoding.UTF8);
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(12))//活动新模板 增加tab
                            {
                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit12.htm"), Encoding.UTF8);
                            }
                            else if (juActivityInfo.ArticleTemplate.Equals(13))//活动模板 提交信息
                            {
                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit13.htm"), Encoding.UTF8);
                            }
                            else if (juActivityInfo.ArticleType.Equals("greetingcard"))//贺卡模板
                            {
                                html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath(string.Format("/Weixin/ArticleTemplate/{0}.htm", juActivityInfo.ArticleTemplate)), Encoding.UTF8);
                            }
                            #endregion

                            if (juActivityInfo.IsSignUpJubit > 0)
                            {
                                //读取个性化模板
                                if (juActivityInfo.ActivityStatus == 0)
                                {
                                    html = html.Replace("$JUTP-TPSignForm$", Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/TPSignForm.htm"), Encoding.UTF8));
                                    if (html.Contains("$CCWX-PERSONNELLIST$") && (juActivityInfo.IsShowPersonnelList.Equals(1)))//报名人数标签
                                    {
                                        html = html.Replace("$CCWX-PERSONNELLIST$", Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/subtemplate/personnellist.htm"), Encoding.UTF8));
                                    }
                                    else
                                    {
                                        html = html.Replace("$CCWX-PERSONNELLIST$", null);

                                    }
                                }
                                html = html.Replace("$JUTP-TPSignForm$", "");
                                html = html.Replace("$CCWX-PERSONNELLIST$", "");

                            }
                            else
                            {
                                html = html.Replace("$JUTP-TPSignForm$", "");

                            }
                            //}



                            #region 外部链接 (注释且已经不使用)
                            ////else//外部链接
                            ////{
                            ////    //下载外部链接源代码
                            ////    Html = Common.MySpider.GetPageSourceForUTF8(articleInfo.ActivityWebsite);

                            ////    if (!string.IsNullOrWhiteSpace(currUrl))
                            ////    {
                            ////        List<string> tmpArr = currUrl.Split('&').ToList();
                            ////        string tmpStr = "";

                            ////        int i = 0;

                            ////        foreach (var item in tmpArr)
                            ////        {
                            ////            if (item.Contains("message=") || item.Contains("status="))//该两个参数一般为状态提醒码
                            ////                continue;
                            ////            if (i > 0)
                            ////                tmpStr += "&";
                            ////            tmpStr += item;
                            ////            i++;
                            ////        }

                            ////        //针对微信网页转发分享链接，需要把网页分享地址替换成当前页面地址，否则分享出去就是微信自己原来的页面了（目的是分享出去是我们自己的链接）
                            ////        Html = Html.Replace(articleInfo.ActivityWebsite, tmpStr);//转发的时候需要把msg去除
                            ////    }
                            ////    //document.domain，这个也必须替换掉

                            ////    Html = Html.Replace("document.domain", " var documentDomainTeml");
                            ////}
                            //#endregion

                            //#region 追加报名表单
                            ////追加报名表单                
                            //// 符合条件的情况
                            ////1:使用外部链接并使用报名.
                            ////2:使用内部链接并使用微信官方模板并使用报名.
                            ////if ((articleInfo.IsByWebsiteContent.Equals(1) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)))//使用外部链接并使用报名.
                            ////    ||
                            ////    //使用内部链接并使用微信官方模板并使用报名.
                            ////    (articleInfo.IsByWebsiteContent.Equals(0) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)) && articleInfo.ArticleTemplate.Equals(1))
                            ////    )

                            //if (juActivityInfo.IsSignUpJubit.Equals(1) || juActivityInfo.IsSignUpJubit.Equals(2))//有报名
                            //{
                            //    StringBuilder sbCheckForm = new StringBuilder();//追加检查form
                            //    StringBuilder sbAppend = new StringBuilder();//Html后追加的html
                            //    StringBuilder sbSignInHtml = new StringBuilder();//报名表单 格式<tr></tr>
                            //    StringBuilder sbcurrMemberInfo = new StringBuilder();//当前会员隐藏信息
                            //    //FORMDATA
                            //    ActivityInfo planActivityInfo = Get<ActivityInfo>(string.Format("ActivityID='{0}'", juActivityInfo.SignUpActivityID));

                            //    List<ActivityFieldMappingInfo> activityFieldMappingInfoList =
                            //   bllActivity.GetActivityFieldMappingList(planActivityInfo.ActivityID).Where(p => p.IsHideInSubmitPage != "1").ToList();
                            //    WXMemberInfo currOpenerMemberInfo = new WXMemberInfo();
                            //    //Dictionary<string, string> currOpenerInfo = new Dictionary<string, string>();
                            //    bool currOpenerIsMember = false;
                            //    if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
                            //    {
                            //        //如果有openID传入，则检查是否是会员，是的话则查询出姓名 手机 邮箱数据
                            //        currOpenerMemberInfo = Get<WXMemberInfo>(string.Format(" UserID = '{0}' and WeixinOpenID = '{1}' ", juActivityInfo.UserID, currOpenerOpenID));
                            //        if (currOpenerMemberInfo != null)
                            //            currOpenerIsMember = true;

                            //        if (currentUserInfo != null)
                            //            currOpenerIsMember = true;

                            //    }

                            //    //if (!string.IsNullOrWhiteSpace(currOpenerOpenID) && !currOpenerIsMember)
                            //    //{
                            //    //    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td>{0}</td></tr>", systemSet.IsNotWXMemberSignInTipMsg);
                            //    //}

                            //    //if (!currOpenerIsMember)
                            //    //{
                            //    //    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td>{0}</td></tr>", systemset.IsNotWXMemberSignInTipMsg);
                            //    //}

                            //    //其它报名字段
                            //    foreach (ActivityFieldMappingInfo item in activityFieldMappingInfoList)
                            //    {
                            //        if (item.FieldType != 1)//普通字段
                            //        {
                            //            string tmpValue = "";

                            //            if (currOpenerIsMember)
                            //            {
                            //                bool tmpIsMp = false;//是否对应


                            //                //姓名
                            //                if (item.FieldName.Equals("name", StringComparison.OrdinalIgnoreCase) && !tmpIsMp)
                            //                {
                            //                    //tmpValue = currOpenerMemberInfo.Name;
                            //                    tmpValue = currentUserInfo.TrueName;
                            //                    tmpIsMp = true;
                            //                }

                            //                //手机
                            //                if (item.FieldName.Equals("phone", StringComparison.OrdinalIgnoreCase) && !tmpIsMp)
                            //                {
                            //                    tmpValue = currentUserInfo.Phone;
                            //                    tmpIsMp = true;
                            //                }

                            //                //判断是不是公司，是的话则隐藏并赋值
                            //                if (item.MappingName == "公司" && !tmpIsMp)
                            //                {
                            //                    tmpValue = currentUserInfo.Company;
                            //                    tmpIsMp = true;
                            //                }

                            //                //判断是不是职位，是的话则隐藏并赋值
                            //                if (item.MappingName == "职位" && !tmpIsMp)
                            //                {
                            //                    tmpValue = currentUserInfo.Postion;
                            //                    tmpIsMp = true;
                            //                }

                            //                if (!tmpIsMp)
                            //                {
                            //                    //判断是不是邮箱，是的话则隐藏并赋值
                            //                    List<string> tmpEmailMp = new List<string>() { "邮箱", "邮件", "email" };
                            //                    foreach (var i in tmpEmailMp)
                            //                    {
                            //                        if (item.MappingName.ToLower().Contains(i))
                            //                        {
                            //                            tmpValue = currentUserInfo.Email;
                            //                            tmpIsMp = true;
                            //                            break;
                            //                        }
                            //                    }
                            //                }

                            //                //if (tmpIsMp)
                            //                //{
                            //                //    sbcurrMemberInfo.AppendFormat("<input type=\"hidden\" value=\"{0}\"  name=\"{1}\" id=\"{1}\"  />", tmpValue, item.FieldName);//邮箱
                            //                //    continue;
                            //                //}

                            //            }
                            //            sbSignInHtml.AppendFormat("<tr ><td><label for=\"{0}\">{1}</label></td></tr>", item.FieldName, item.MappingName);
                            //            switch (item.InputType)
                            //            {
                            //                case "combox":
                            //                    sbSignInHtml.AppendFormat("<tr ><td>");
                            //                    sbSignInHtml.AppendFormat("<select name=\"{0}\">", item.FieldName);
                            //                    for (var i = 0; i < item.Options.Split(',').Length; i++)
                            //                    {
                            //                        var optionValue = item.Options.Split(',')[i];
                            //                        sbSignInHtml.AppendFormat("<option value=\"{0}\">{0}</option>", optionValue);


                            //                    }
                            //                    sbSignInHtml.AppendFormat("</select>");
                            //                    sbSignInHtml.AppendFormat("</td></tr>");

                            //                    break;
                            //                case "checkbox":
                            //                    sbSignInHtml.AppendFormat("<tr ><td >");

                            //                    for (var i = 0; i < item.Options.Split(',').Length; i++)
                            //                    {
                            //                        var optionValue = item.Options.Split(',')[i];
                            //                        sbSignInHtml.AppendFormat("<input type=\"checkbox\" name=\"{0}\" value=\"{1}\" id=\"cb{2}\"/>", item.FieldName, optionValue, i);
                            //                        sbSignInHtml.AppendFormat("<label for=\"cb{0}\">{1}</label>", i, optionValue);


                            //                    }

                            //                    sbSignInHtml.AppendFormat("</td></tr>");

                            //                    break;
                            //                case "text":
                            //                default:
                            //                    if (item.IsMultiline.Equals(1))//<textarea name="K1" id="txtContent" style="height: 100px;" placeholder="请详细描述您的建议、意见、问题等。"></textarea>
                            //                    {
                            //                        sbSignInHtml.AppendFormat("<tr ><td ><textarea rows=\"5\" type=\"text\" name=\"{0}\" id=\"{0}\" value=\"{2}\" placeholder=\"请输入{1}\" ></textarea> </td></tr>", item.FieldName, item.MappingName, tmpValue);
                            //                    }
                            //                    else
                            //                    {
                            //                        sbSignInHtml.AppendFormat("<tr ><td ><input  type=\"text\" name=\"{0}\" id=\"{0}\" value=\"{2}\" placeholder=\"请输入{1}\" /> </td></tr>", item.FieldName, item.MappingName, tmpValue);
                            //                    }
                            //                    break;

                            //            }




                            //        }
                            //        else//微信推广字段
                            //        {
                            //            sbSignInHtml.AppendFormat("<tr><td><input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"$CCWXTG-LINKID$\" /></td></tr> ", item.FieldName);
                            //        }

                            //        if (item.FieldIsNull.Equals(1))
                            //        {

                            //            sbCheckForm.AppendFormat("if (!document.getElementById(\"{0}\").value)", item.FieldName);

                            //            sbCheckForm.AppendLine("{");
                            //            sbCheckForm.AppendFormat("alert(\"请输入{0}\");", item.MappingName);
                            //            sbCheckForm.AppendLine(" return false;");
                            //            sbCheckForm.AppendLine("}");
                            //        }


                            //    }
                            //    //其它报名字段

                            //    //其它报名信息
                            //    //sbSignInHtml.AppendLine("<div  style=\"margin-top: 10px;\">");
                            //    sbSignInHtml.AppendLine("<tr><td>");
                            //    sbSignInHtml.AppendFormat("<input id=\"activityID\" type=\"hidden\" value=\"{0}\" name=\"ActivityID\" />", planActivityInfo.ActivityID);//活动ID
                            //    sbSignInHtml.AppendFormat("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\" />", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID));//外部登录名
                            //    sbSignInHtml.AppendFormat("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\" />", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password));//外部登录密码

                            //    if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
                            //    {
                            //        //如果当前微信OpenID不为空，则添加到页面去
                            //        sbSignInHtml.AppendFormat("<input id=\"{1}\" type=\"hidden\" value=\"{0}\" name=\"{1}\" />", currOpenerOpenID, systemSet.WXCurrOpenerOpenIDKey);//当前的微信OpenID
                            //    }

                            //    //if (!string.IsNullOrWhiteSpace(spreadUserID))
                            //    //{
                            //    //如果当前推广人不为空，则添加到页面去,记录谁带来的报名
                            //    // sbSignInHtml.AppendFormat("<input id=\"{1}\" type=\"hidden\" value=\"{0}\" name=\"{1}\" />", Common.Base64Change.EncodeBase64ByUTF8(spreadUserID), "SpreadUserID");//当前的微信OpenID
                            //    //}
                            //    if (spreadUser != null)
                            //    {
                            //        sbSignInHtml.AppendFormat("<input name=\"SpreadUserID\"  type=\"hidden\" value=\"{0}\"  />", spreadUser.UserID);
                            //        sbSignInHtml.AppendFormat("<input name=\"MonitorPlanID\"  type=\"hidden\" value=\"{0}\"  />", juActivityInfo.MonitorPlanID);
                            //    }
                            //    sbSignInHtml.AppendLine(sbcurrMemberInfo.ToString());//添加会员信息


                            //    //不允许重复字段
                            //    if (planActivityInfo.DistinctKeys != null)
                            //    {
                            //        if (!string.IsNullOrEmpty(planActivityInfo.DistinctKeys))
                            //        {
                            //            sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"DistinctKeys\">", planActivityInfo.DistinctKeys);
                            //        }
                            //    }
                            //    sbSignInHtml.AppendLine("</td></tr>");
                            //    //不允许重复字段

                            //    if (juActivityInfo.IsByWebsiteContent.Equals(1))//外部链接报名
                            //    {
                            //        sbAppend.Append(Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/wx_externalsignuptemplate.htm"), Encoding.UTF8));//外部报名表单模板
                            //        sbAppend = sbAppend.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
                            //        if (html.Contains("</body>"))
                            //        {
                            //            html = html.Replace("</body>", string.Format("{0}</body>", sbAppend));
                            //        }
                            //        else
                            //        {
                            //            html += sbAppend.ToString();
                            //        }

                            //        html = html.Replace("$CCWX-CHECKFORM$", sbCheckForm.ToString());


                            //    }
                            //    else
                            //    {

                            //        //内部链接报名
                            //        html = html.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
                            //    }

                            //}


                            ////追加报名表单 
                            //#endregion

                            //#region 替换微信远程图片为本地路径[已经注释]

                            //System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            //System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(Html);
                            //foreach (System.Text.RegularExpressions.Match match in matches)
                            //{
                            //    String name = match.ToString();
                            //    string reomoteurl = match.Groups["imgUrl"].Value;
                            //    //微信特殊处理"<img src=\"http://res.wx.qq.com/mmbizwap/zh_CN/htmledition/images/big_loading19d82d.gif\" onerror=\"this.parentNode.removeChild(this)\" data-src=\"http://mmbiz.qpic.cn/mmbiz/lybKlMLvO2WzibKRgibMx82Zsg2Lhts4dibN9JNicHeTrzUJfiarSia2GaIjtlPyictzVxPmEUH4iaeTuXszSbO7CbTibdQ/0\" />"
                            //    if (name.Contains("data-src="))
                            //    {
                            //        string tmp = Common.StringHelper.CutByStarTag(name, "data-src=", true);// name.Substring(name.IndexOf("data-src"));

                            //        tmp = tmp.Substring(1, tmp.IndexOf(" ") - 2);
                            //        reomoteurl = tmp;
                            //    }

                            //    if (reomoteurl.StartsWith("http://mmsns.qpic.cn/") || reomoteurl.StartsWith("http://mmbiz.qpic.cn/") || reomoteurl.StartsWith("http://res.wx.qq.com/"))//替换微信官方图片地址为本地 
                            //    {
                            //        Html = Html.Replace(name, string.Format("<img src=\"http://{0}:{1}{2}\">", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, DownLoadRemoteImage(reomoteurl)));

                            //    }


                            //}

                            #endregion

                            #region 替换文章标签

                            if (html.Contains("$CCWX-currOpenerOpenID$"))//替换当前用户openid
                            {
                                html = html.Replace("$CCWX-currOpenerOpenID$", currentUserInfo == null ? "" : currentUserInfo.WXOpenId);
                            }
                            if (html.Contains("$CCWX-currUserID$"))//替换当前用户openid
                            {
                                html = html.Replace("$CCWX-currUserID$", currentUserInfo == null ? "" : currentUserInfo.UserID);
                            }

                            if (html.Contains("$CCWX-shareUserId$"))//替换微监测分享者id
                            {
                                html = html.Replace("$CCWX-shareUserId$", shareUser == null ? "" : shareUser.UserID);
                            }

                            if (html.Contains("$CCWX-signUpActivityID$"))//替换真实报名id
                            {
                                html = html.Replace("$CCWX-signUpActivityID$", juActivityInfo.SignUpActivityID == null ? "" : juActivityInfo.SignUpActivityID);
                            }
                            if (signUpData != null)
                            {
                                if (html.Contains("$CCWX-currUserOrderId$"))//当前用户报名下单的订单id
                                {
                                    html = html.Replace("$CCWX-currUserOrderId$", signUpData.OrderId == null ? "" : signUpData.OrderId);
                                }
                            }

                            if (html.Contains("$CCWX-SPREADUSERINFO$"))
                            {
                                if (spreadUser != null)
                                {

                                    if (companyWebsiteConfig != null)
                                    {
                                        if (!string.IsNullOrEmpty(spreadUser.WXHeadimgurl) && !string.IsNullOrEmpty(spreadUser.WXNickname) && !string.IsNullOrEmpty(companyWebsiteConfig.WeixinAccountNickName))
                                        {

                                            //html = html.Replace("$CCWX-SPREADUSERINFO$", "<div style=\"background-color:Gray;color:White;vertical-align:middle;width:auto;margin-bottom:10px;text-shadow:none;margin-top:20px;\"><table><tbody> <tr><td style=\"width:55px;\"><img width=\"50\" height=\"50\" src=\"" + spreadUser.WXHeadimgurl + "\" style=\"width:auto;margin-top:2px;margin-left:2px;margin-bottom:2px;vertical-align:middle;\" /></td> <td style=\"vertical-align:middle;\">来自&nbsp;" + spreadUser.WXNickname + "&nbsp;的推荐&nbsp; ☆☆☆☆☆“" + config.WeixinAccountNickName + "” </td></tr></tbody></table></div>");
                                            html = html.Replace("$CCWX-SPREADUSERINFO$", "<div style=\"background-color:Gray;color:White;vertical-align:middle;width:auto;margin-bottom:10px;text-shadow:none;margin-top:20px;\"><table><tbody> <tr><td style=\"width:55px;\"><img width=\"50\" height=\"50\" src=\"" + spreadUser.WXHeadimgurl + "\" style=\"width:auto;margin-top:2px;margin-left:2px;margin-bottom:2px;vertical-align:middle;\" /></td> <td style=\"vertical-align:middle;\"><div style=\" padding-left: 6px; font-size: 12px;\">来自&nbsp;" + spreadUser.WXNickname + "&nbsp;的分享&nbsp; ☆☆☆☆☆   <br>扫码关注公众号 “" + companyWebsiteConfig.WeixinAccountNickName + "”  </div></td></tr></tbody></table></div>");

                                        }
                                    }
                                }

                                html = html.Replace("$CCWX-SPREADUSERINFO$", "");
                            }



                            if (html.Contains("$CCWX-WXHEADIMGURL$"))//头像
                            {
                                html = html.Replace("$CCWX-WXHEADIMGURL$", spreadUser == null ? "" : spreadUser.WXHeadimgurl);
                            }
                            if (html.Contains("$CCWX-WXNICKNAME$"))//昵称
                            {
                                html = html.Replace("$CCWX-WXNICKNAME$", spreadUser == null ? "" : spreadUser.WXNickname);
                            }
                            #region 生成吸粉二维码
                            if (html.Contains("$CCWX-DISTRIBUTIONWXQRCODELIMITURL"))
                            {

                                if (spreadUser != null)
                                {
                                    string qrCodeUrl = "";
                                    var wxQrcode = bllActivity.Get<WXQrCode>(string.Format(" WebsiteOwner='{0}' And UserId='{1}' And Id={2} And QrCodeType='WeiXinRecommendFans'", bllActivity.WebsiteOwner, spreadUser.UserID, juActivityInfo.JuActivityID));
                                    if (wxQrcode != null)
                                    {
                                        qrCodeUrl = wxQrcode.QrCodeUrl;
                                    }
                                    else
                                    {
                                        string scen = "ArticleId_" +juActivityInfo.JuActivityID.ToString() + "_" + spreadUser.UserID;
                                        qrCodeUrl = bllWeixin.GetWxQrcodeLimit(scen);

                                        WXQrCode qrCodeModel = new WXQrCode();
                                        qrCodeModel.WebsiteOwner = bllActivity.WebsiteOwner;
                                        qrCodeModel.UserId = spreadUser.UserID;
                                        qrCodeModel.Id = juActivityInfo.JuActivityID.ToString();
                                        qrCodeModel.QrCodeType = "WeiXinRecommendFans";
                                        qrCodeModel.QrCodeUrl = qrCodeUrl;
                                        bllActivity.Add(qrCodeModel);
                                    }
                                    #region 分销二维码图标
                                    if (!string.IsNullOrEmpty(companyWebsiteConfig.DistributionQRCodeIcon) && !string.IsNullOrEmpty(qrCodeUrl))
                                    {

                                        try
                                        {
                                            qrCodeUrl = bllWeixin.GetQRCodeImg(qrCodeUrl, companyWebsiteConfig.DistributionQRCodeIcon);

                                            qrCodeUrl = DownLoadImageToOss(HttpContext.Current.Server.MapPath(qrCodeUrl), WebsiteOwner, true);
                                        }
                                        catch { }

                                    }
                                    #endregion

                                    

                                    html = html.Replace("$CCWX-DISTRIBUTIONWXQRCODELIMITURL", "<img  src='" + qrCodeUrl + "'>");
                                }



                                html = html.Replace("$CCWX-DISTRIBUTIONWXQRCODELIMITURL", "");


                            }
                            #endregion


                            //UserInfo userInfo = Get<UserInfo>(string.Format("UserID='{0}'", articleInfo.UserID));//文章发布者信息
                            //$CCWX-NikiNameAndARTICLETIME$
                            if (html.Contains("$CCWX-NikiNameAndARTICLETIME$"))//替换发表时间和昵称
                            {
                                //Html = Html.Replace("$CCWX-NikiNameAndARTICLETIME$", string.Format("{0} 发表于 {1}", string.IsNullOrWhiteSpace(userInfo.WXNickname) ? userInfo.UserID : userInfo.WXNickname, articleInfo.CreateDate.ToShortDateString()));

                                html = html.Replace("$CCWX-NikiNameAndARTICLETIME$", string.Format("{0:f}", juActivityInfo.CreateDate));
                            }

                            #region 分享链接
                            if (html.Contains("$CCWXTG-SHAREURL$"))//替换分享链接地址
                            {
                                string shareUrl = "";
                                if (spreadUser == null)//默认推广
                                {
                                    //上一个分享人信息
                                    string lastSpreadU = HttpContext.Current.Request["SpreadU"] == null ? "" : HttpContext.Current.Request["SpreadU"];
                                    string lastShareTimestamp = HttpContext.Current.Request["ShareTimestamp"] == null ? "" : HttpContext.Current.Request["ShareTimestamp"];
                                    string lastSpreadUserId = ZentCloud.Common.Base64Change.DecodeBase64ByUTF8(lastSpreadU);
                                    //上一个分享人信息

                                    //新的分享信息
                                    //分享人用户id
                                    string spreadU = currentUserInfo == null ? "" : MySpider.Base64Change.EncodeBase64ByUTF8(currentUserInfo.UserID);
                                    //上一个分享人用户id
                                    string preSpreadU = lastSpreadU;
                                    //上一个人分享时间戳
                                    string preShareTimestamp = lastShareTimestamp;
                                    string shareTimestamp = MySpider.UnixTimestamp.GetStamp();

                                    //新的分享信息

                                    shareUrl = string.Format("http://{0}/{1}/Share.chtml?shareTimestamp={2}&spreadU={3}&PreSpreadU={4}&PreShareTimestamp={5}",
                                    HttpContext.Current.Request.Url.Host,
                                    juActivityInfo.JuActivityIDHex,
                                    shareTimestamp,
                                    currentUserInfo == null ? "" : MySpider.Base64Change.EncodeBase64ByUTF8(currentUserInfo.UserID),
                                    preSpreadU,
                                    preShareTimestamp

                                   );
                                }
                                else//微转发推广
                                {

                                    //
                                    //shareUrl = string.Format("http://{0}/{1}/{2}/detail.chtml", HttpContext.Current.Request.Url.Host,articleInfo.JuActivityIDHex,Convert.ToString(spreadUser.AutoID,16));
                                    //
                                    //上一个分享人信息
                                    string lastSpreadU = HttpContext.Current.Request["SpreadU"] == null ? "" : HttpContext.Current.Request["SpreadU"];
                                    string lastShareTimestamp = HttpContext.Current.Request["ShareTimestamp"] == null ? "" : HttpContext.Current.Request["ShareTimestamp"];
                                    string lastSpreadUserId = ZentCloud.Common.Base64Change.DecodeBase64ByUTF8(lastSpreadU);
                                    //上一个分享人信息
                                    //新的分享信息
                                    //分享人用户id
                                    string spreadU = spreadUser == null ? "" : MySpider.Base64Change.EncodeBase64ByUTF8(spreadUser.UserID);
                                    //上一个分享人用户id
                                    string preSpreadU = lastSpreadU;
                                    //上一个人分享时间戳
                                    string preShareTimestamp = lastShareTimestamp;
                                    string shareTimestamp = MySpider.UnixTimestamp.GetStamp();

                                    //新的分享信息

                                    shareUrl = string.Format("http://{0}/{1}/{2}/Share.chtml?shareTimestamp={3}&spreadU={4}&PreSpreadU={5}&PreShareTimestamp={6}",
                                    HttpContext.Current.Request.Url.Host,
                                    juActivityInfo.JuActivityIDHex,
                                    Convert.ToString(spreadUser.AutoID, 16),
                                    shareTimestamp,
                                    currentUserInfo == null ? "" : MySpider.Base64Change.EncodeBase64ByUTF8(spreadUser.UserID),
                                    preSpreadU,
                                    preShareTimestamp

                                   );

                                }
                                html = html.Replace("$CCWXTG-SHAREURL$", shareUrl);


                            } 
                            #endregion

                            #region 加投票功能
                            if (juActivityInfo.ActivityDescription.Contains("$TOUPIAO@"))
                            {
                                int startIndex = juActivityInfo.ActivityDescription.IndexOf("$TOUPIAO@");
                                int endIndex = juActivityInfo.ActivityDescription.LastIndexOf("TOUPIAO$");
                                int length = endIndex - startIndex - 1;
                                string voteId = juActivityInfo.ActivityDescription.Substring(juActivityInfo.ActivityDescription.IndexOf("$TOUPIAO@") + 1, length).Replace("TOUPIAO@", null);
                                string str = "$TOUPIAO@" + voteId + "TOUPIAO$";
                                juActivityInfo.ActivityDescription = juActivityInfo.ActivityDescription.Replace(str, GetTheVoteInfo(voteId));

                            }
                            #endregion




                            if (html.Contains("$CCWX-ARTICLEIMAGE$"))//替换分享图片地址
                            {
                                if (juActivityInfo.ThumbnailsPath.ToLower().Contains("http://"))
                                {
                                    html = html.Replace("$CCWX-ARTICLEIMAGE$", juActivityInfo.ThumbnailsPath);
                                }
                                else
                                {
                                    html = html.Replace("$CCWX-ARTICLEIMAGE$", string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, juActivityInfo.ThumbnailsPath));

                                }
                            }

                            //if (html.Contains("$CCWXCALLBACKURL$"))//替换回调地址 
                            //{
                            //    html = html.Replace("$CCWXCALLBACKURL$", string.Format("http://{0}:{1}{2}?id={3}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.FilePath, juActivityInfo.JuActivityID));
                            //}
                            if (html.Contains("$CCWX-ARTICLETITLE$"))//替换标题
                            {
                                html = html.Replace("$CCWX-ARTICLETITLE$", juActivityInfo.ActivityName);//替换标题

                            }
                            if (html.Contains("$CCWX-ARTICLESUMMARY$"))//替换描述
                            {
                                html = html.Replace("$CCWX-ARTICLESUMMARY$", juActivityInfo.Summary);//替换描述

                            }
                            if (html.Contains("$CCWX-ACTIVITYLECTURER$"))//替换作者
                            {
                                html = html.Replace("$CCWX-ACTIVITYLECTURER$", juActivityInfo.ActivityLecturer);//替换作者
                            }
                            if (html.Contains("$CCWX-ARTICLETIME$"))//替换时间
                            {
                                if (juActivityInfo.IsSignUpJubit.Equals(0))//普通文章
                                {
                                    html = html.Replace("$CCWX-ARTICLETIME$", string.Format("{0:f}", juActivityInfo.CreateDate));//替换时间
                                }
                                else if (juActivityInfo.ArticleTemplate.Equals(12))
                                {
                                    html = html.Replace("$CCWX-ARTICLETIME$", string.Format("{0}",Convert.ToDateTime(juActivityInfo.ActivityStartDate).ToString("yyyy-MM-dd")));//替换时间
                                }
                                else//活动文章
                                {
                                    html = html.Replace("$CCWX-ARTICLETIME$", "开始时间:" + string.Format("{0:f}", juActivityInfo.ActivityStartDate));//替换时间
                                }
                            }
                            if (html.Contains("$CCWX-ARTICLEENDTIME$"))//替换时间
                            {
                                if (juActivityInfo.IsSignUpJubit.Equals(0))//普通文章
                                {
                                    html = html.Replace("$CCWX-ARTICLEENDTIME$", "");
                                }
                                else if(juActivityInfo.ArticleTemplate.Equals(12))//活动文章
                                {
                                    html = html.Replace("$CCWX-ARTICLEENDTIME$", string.Format("{0}", Convert.ToDateTime(juActivityInfo.ActivityStartDate).ToString("yyyy-MM-dd")));//替换时间
                                }else{
                                     html = html.Replace("$CCWX-ARTICLEENDTIME$", "结束时间:" + string.Format("{0:f}", juActivityInfo.ActivityEndDate));//替换时间
                                }
                            }
                            if (html.Contains("$CCWX-ARTICLEADDRESS$"))//替换地址
                            {
                                html = html.Replace("$CCWX-ARTICLEADDRESS$", string.IsNullOrEmpty(juActivityInfo.ActivityAddress) ? "" : juActivityInfo.ActivityAddress);
                            }
                            if (html.Contains("$CCWX-ARTICLECONTENT$"))//替换内容
                            {
                                html = html.Replace("$CCWX-ARTICLECONTENT$", juActivityInfo.ActivityDescription);//替换内容
                            }
                            if (html.Contains("$CCWX-ARTICLEOPENCOUNT$"))//替换打开人次
                            {
                                html = html.Replace("$CCWX-ARTICLEOPENCOUNT$", juActivityInfo.PV.ToString());//替换打开人次
                            }
                            if (html.Contains("$CCWX-ARTICLEUPCOUNT$"))//替换赞人数
                            {
                                html = html.Replace("$CCWX-ARTICLEUPCOUNT$", juActivityInfo.UpCount.ToString());//替换赞人数
                            }
                            if (html.Contains("$CCWX-ARTICLEID$"))//替换文章ID
                            {
                                var nJuActivityID = 0;
                                if (juActivityInfo.ArticleType.ToLower() == "article")
                                {
                                    if (juActivityInfo.HaveComment == 0)
                                    {
                                        
                                        //CompanyWebsite_Config nWebsiteConfig = (new ZentCloud.BLLJIMP.BLLWebSite()).GetCompanyWebsiteConfig();
                                        if (companyWebsiteConfig != null && companyWebsiteConfig.HaveComment == 1)
                                        {
                                            nJuActivityID = juActivityInfo.JuActivityID;
                                        }
                                    }
                                    else if (juActivityInfo.HaveComment == 1)
                                    {
                                        nJuActivityID = juActivityInfo.JuActivityID;
                                    }
                                }
                                html = html.Replace("$CCWX-ARTICLEID$", nJuActivityID.ToString());//替换文章ID
                            }
                            if (html.Contains("$CCWX-ARTICLSHARETCOUNT$"))//分享人数
                            {
                                html = html.Replace("$CCWX-ARTICLSHARETCOUNT$", juActivityInfo.ShareTotalCount.ToString());//分享人数
                            }

                            if (html.Contains("$CCWX-ARTICLSUGID$"))//报名ID
                            {
                                html = html.Replace("$CCWX-ARTICLSUGID$", juActivityInfo.SignUpActivityID.ToString());//报名ID
                            }
                            if (html.Contains("$CCWX-JUACTIVITYID$"))//报名ID
                            {
                                html = html.Replace("$CCWX-JUACTIVITYID$", juActivityInfo.JuActivityID.ToString());//JuactivityId
                            }
                            if (html.Contains("$CCWX-ArticleSource$"))//替换文章来源
                            {
                                StringBuilder strSource = new StringBuilder();

                                //if (userInfo.ArticleSourceType == "AddFriend" && !string.IsNullOrWhiteSpace(userInfo.ArticleSourceWXHao))
                                //{
                                //    strSource.AppendFormat("来自&nbsp;<a href=\"weixin://contacts/profile/{0}\">{1}</a>", userInfo.ArticleSourceWXHao, userInfo.ArticleSourceName);
                                //}
                                //else if (userInfo.ArticleSourceType == "WebSite" && !string.IsNullOrWhiteSpace(userInfo.ArticleSourceWebSite))
                                //{
                                //    strSource.AppendFormat("来自&nbsp;<a target=\"_blank\" href=\"{0}\">{1}</a>", userInfo.ArticleSourceWebSite.ToLower().StartsWith("http://") ? userInfo.ArticleSourceWebSite : "http://" + userInfo.ArticleSourceWebSite, userInfo.ArticleSourceName);
                                //}
                                //else
                                //{
                                //    strSource.AppendFormat("<a href=\"javascript:;\">{0}</a>", string.IsNullOrWhiteSpace(userInfo.ArticleSourceName) ? "" : "来自&nbsp;" + userInfo.ArticleSourceName);
                                //}

                                html = html.Replace("$CCWX-ArticleSource$", strSource.ToString());//替换文章ID
                            }
                            //#region 替换作者的其它发布
                            //if (html.Contains("$CCWX-OTHERARTICLE$"))
                            //{

                            //    List<JuActivityInfo> otherArticleList = GetLit<JuActivityInfo>(5, 1, string.Format("WebsiteOwner='{0}'  And JuActivityID !={1} and  IsDelete=0 and (IsHideRecommend is null or IsHideRecommend='0') And ArticleType='{2}'  ", articleInfo.WebsiteOwner, articleInfo.JuActivityID, articleInfo.ArticleType), "CreateDate DESC");
                            //    if (otherArticleList.Count > 0)
                            //    {

                            //        StringBuilder sbOther = new StringBuilder("<div style=\"font-family: 微软雅黑;margin-top:10px;\">");
                            //        sbOther.AppendLine("<label style=\"font-weight:bold;fon-size:18px;margin-left:10px;\">作者的其它发布</label>");

                            //        //#8c8c8c
                            //        sbOther.AppendLine("<div style=\"border-top: 1px solid; border-bottom: 1px solid;border-color:#ddd;overflow:hidden;margin-top:10px;padding-bottom: 10px;margin-bottom: 30px;\">");

                            //        sbOther.Append("<table style=\"width:100%;margin-top:10px;margin-left:10px;\">");
                            //        sbOther.AppendLine("<tbody>");
                            //        for (int i = 0; i < otherArticleList.Count; i++)
                            //        {
                            //            sbOther.AppendLine(string.Format("<tr onclick=\"window.location.href=\'/{0}/share.chtml\'\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\">", otherArticleList[i].JuActivityIDHex));
                            //            sbOther.AppendLine("<td style=\"width:50px;\">");
                            //            sbOther.AppendFormat("<img src=\"{0}\" width=\"50px;\" height=\"50px;\" style=\"border-radius:5px;\" >", otherArticleList[i].ThumbnailsPath);
                            //            sbOther.AppendLine("</td>");
                            //            sbOther.AppendLine("<td style=\"padding: 0 12px 0 10px;\" >");
                            //            sbOther.AppendFormat("<label style=\"margin-left:10px;\">{0}</label>", otherArticleList[i].ActivityName);
                            //            sbOther.AppendLine("</td>");
                            //            sbOther.AppendLine("</tr>");
                            //            if (i < otherArticleList.Count - 1)
                            //            {
                            //                sbOther.AppendLine("<tr>");
                            //                sbOther.AppendLine("<td colspan=\"2\">");
                            //                //sbOther.AppendLine("<hr style=\"border-color:#CCCCCC;\"/>");
                            //                sbOther.AppendLine("<hr style=\"height:1px;border:0;border-bottom:1px solid #cccccc;\" />");
                            //                sbOther.AppendLine("</td>");
                            //                sbOther.AppendLine("</tr>");
                            //            }

                            //        }
                            //        sbOther.AppendLine("</tbody>");
                            //        sbOther.Append("</table>");
                            //        sbOther.Append("</div>");
                            //        sbOther.AppendLine("</div>");
                            //        html = html.Replace("$CCWX-OTHERARTICLE$", sbOther.ToString());
                            //    }
                            //    html = html.Replace("$CCWX-OTHERARTICLE$", null);

                            //}
                            //#endregion

                            #region 可见区域

                            if (html.Contains("$CCWX-VISIBLEAREA$"))
                            {
                                if (juActivityInfo.VisibleArea == 1)
                                {
                                    StringBuilder sbHtml = new StringBuilder("<div style='border: 5px dotted #ddd;margin: 8%;padding: 14%;text-align: center;color: #797979;font-size: 20px;'>");

                                    if (juActivityInfo.IsFee == 0)
                                    {
                                        if (juActivityInfo.ShowCondition == 0)
                                        {
                                            if (signUpData != null)
                                            {
                                                sbHtml.Clear();

                                                sbHtml.AppendFormat("<div style='margin-bottom:20px;'>{0}</div>", juActivityInfo.VisibleContext);
                                            }
                                            else
                                            {
                                                sbHtml.AppendFormat("<span>报名成功后可见</span>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (juActivityInfo.ShowCondition == 1)
                                        {
                                            if (signUpData != null)
                                            {
                                                if (signUpData.PaymentStatus == 1)
                                                {
                                                    sbHtml.Clear();
                                                    sbHtml.AppendFormat("<div style='margin-bottom:20px;'>{0}</div>", juActivityInfo.VisibleContext);
                                                }
                                                else
                                                {
                                                    sbHtml.AppendFormat("<span>支付成功后可见</span>");
                                                }

                                            }
                                            else
                                            {
                                                sbHtml.AppendFormat("<span>支付成功后可见</span>");
                                            }
                                        }
                                        else
                                        {
                                            if (signUpData != null)
                                            {
                                                sbHtml.Clear();
                                                sbHtml.AppendFormat("<div style='margin-bottom:20px;'>{0}</div>", juActivityInfo.VisibleContext);
                                            }
                                            else
                                            {
                                                sbHtml.AppendFormat("<span>报名成功后可见</span>");
                                            }
                                        }
                                    }
                                    sbHtml.AppendFormat("</div>");
                                    html = html.Replace("$CCWX-VISIBLEAREA$", sbHtml.ToString());
                                }
                                html = html.Replace("$CCWX-VISIBLEAREA$", null);
                            }





                            #endregion



                            #region 替换相关阅读
                            if (html.Contains("$CCWX-RELATIONARTICLE$"))
                            {
                                if (!string.IsNullOrEmpty(juActivityInfo.RelationArticles)||!string.IsNullOrEmpty(juActivityInfo.RelationProducts))
                                {
                                    List<JuActivityInfo> relationArticleList = new List<JuActivityInfo>();
                                    List<WXMallProductInfo> proList = new List<WXMallProductInfo>();
                                    if (!string.IsNullOrEmpty(juActivityInfo.RelationArticles))
                                    {
                                        relationArticleList = GetList<JuActivityInfo>(string.Format(" JuActivityID in({0}) And IsHide=0 And ArticleType in('article','activity')", juActivityInfo.RelationArticles));
                                    }
                                    if (!string.IsNullOrEmpty(juActivityInfo.RelationProducts))
                                    {
                                        proList = bllActivity.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' AND PID in ({1}) ", bllActivity.WebsiteOwner, juActivityInfo.RelationProducts));
                                    }
                                    if (relationArticleList.Count > 0 || proList.Count>0)
                                    {
                                        StringBuilder sbOther = new StringBuilder("<div class=\"warpRelationArticle$ \" style=\"font-family: 微软雅黑;margin-top:10px;\">");
                                        sbOther.AppendLine("<label style=\"font-weight:bold;font-size:18px;margin-left:10px;\">相关阅读</label>");

                                        sbOther.AppendLine("<div style=\"border-top: 1px solid; border-bottom: 1px solid;border-color:#ddd;overflow:hidden;margin-top:10px;padding-bottom: 10px;margin-bottom: 30px;\">");

                                        sbOther.Append("<table style=\"width:100%;margin-top:10px;margin-left:10px;\">");
                                        sbOther.AppendLine("<tbody>");
                                        //文章或活动
                                        for (int i = 0; i < relationArticleList.Count; i++)
                                        {
                                            sbOther.AppendLine(string.Format("<tr onclick=\"window.location.href=\'/{0}/detail.chtml\'\" >", relationArticleList[i].JuActivityIDHex));
                                            sbOther.AppendLine("<td style=\"width:50px;\">");
                                            sbOther.AppendFormat("<img class=\"imgArticle\" src=\"{0}\" width=\"50px;\" height=\"50px;\" style=\"border-radius:5px;position: relative;left: 6px;\" >", relationArticleList[i].ThumbnailsPath);
                                            sbOther.AppendLine("</td>");
                                            sbOther.AppendLine("<td style=\"padding: 0 14px 0 10px;\">");
                                            sbOther.AppendFormat("<label class=\"lbTitle\" style=\"margin-left:10px;\">{0}</label>", relationArticleList[i].ActivityName);

                                            sbOther.AppendFormat("<br/><label class=\"lbDescription\" style=\"margin-left:10px;color:#999;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;width: 248px;font-weight: normal;font-size: 12px;\">{0}</label>", relationArticleList[i].Summary);

                                            sbOther.AppendLine("</td>");
                                            sbOther.AppendLine("</tr>");
                                            if (i < relationArticleList.Count - 1)
                                            {
                                                sbOther.AppendLine("<tr>");
                                                sbOther.AppendLine("<td colspan=\"2\">");
                                                sbOther.AppendLine("<hr style=\"height:1px;border:0;border-bottom:1px solid #cccccc;\" />");
                                                sbOther.AppendLine("</td>");
                                                sbOther.AppendLine("</tr>");
                                            }

                                        }
                                        if (proList.Count > 0)
                                        {
                                            sbOther.AppendLine("<tr>");
                                            sbOther.AppendLine("<td colspan=\"2\">");
                                            sbOther.AppendLine("<hr style=\"height:1px;border:0;border-bottom:1px solid #cccccc;\" />");
                                            sbOther.AppendLine("</td>");
                                            sbOther.AppendLine("</tr>");
                                        }
                                        //商品
                                        for (int j = 0; j < proList.Count; j++)
                                        {
                                           
                                            sbOther.AppendLine(string.Format("<tr onclick=\"window.location.href='/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/427557#/productDetail/" + proList[j].PID + "'\">"));
                                            sbOther.AppendLine("<td style=\"width:50px;\">");
                                            sbOther.AppendFormat("<img class=\"imgArticle\" src=\"{0}\" width=\"50px;\" height=\"50px;\" style=\"border-radius:5px;position: relative;left: 6px;\" >", proList[j].ShowImage);
                                            sbOther.AppendLine("</td>");
                                            sbOther.AppendLine("<td style=\"padding: 0 14px 0 10px;\">");
                                            sbOther.AppendFormat("<label class=\"lbTitle\" style=\"margin-left:10px;\">{0}</label>", proList[j].PName);

                                            sbOther.AppendFormat("<br/><label class=\"lbDescription\" style=\"margin-left:10px;color:#999;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;width: 248px;font-weight: normal;font-size: 12px;\">{0}</label>", proList[j].Summary);

                                            sbOther.AppendLine("</td>");
                                            sbOther.AppendLine("</tr>");
                                            if (j < proList.Count - 1)
                                            {
                                                sbOther.AppendLine("<tr>");
                                                sbOther.AppendLine("<td colspan=\"2\">");
                                                sbOther.AppendLine("<hr style=\"height:1px;border:0;border-bottom:1px solid #cccccc;\" />");
                                                sbOther.AppendLine("</td>");
                                                sbOther.AppendLine("</tr>");
                                            }
                                        }
                                        sbOther.AppendLine("</tbody>");
                                        sbOther.Append("</table>");
                                        sbOther.Append("</div>");
                                        sbOther.AppendLine("</div>");
                                        html = html.Replace("$CCWX-RELATIONARTICLE$", sbOther.ToString());
                                    }
                                }
                    
                                html = html.Replace("$CCWX-RELATIONARTICLE$", null);

                            }
                            #endregion


                            #region 替换文件下载
                            if (html.Contains("$CCWX-FileDownload$"))
                            {

                                List<JuActivityFiles> juActivityFiles = new List<JuActivityFiles>();
                                juActivityFiles = GetColMultListByKey<JuActivityFiles>(int.MaxValue, 1, "JuActivityID", juActivityInfo.JuActivityID.ToString(), "AutoId,FileName,FilePath,FileClass");

                                if (juActivityFiles != null && juActivityFiles.Count > 0)
                                {


                                    StringBuilder sbOther = new StringBuilder("<div style=\"font-family: 微软雅黑;margin-top:10px;\">");
                                    sbOther.AppendLine("<label style=\"font-weight:bold;fon-size:18px;margin-left:10px;\">附件下载</label>");

                                    //#8c8c8c
                                    sbOther.AppendLine("<div style=\"border-top: 1px solid; border-bottom: 1px solid;border-color:#ddd;overflow:hidden;margin-top:10px;padding-bottom: 10px;margin-bottom: 30px;\">");

                                    sbOther.Append("<table style=\"width:100%;margin-top:10px;margin-left:10px;\">");
                                    sbOther.AppendLine("<tbody>");
                                    for (int i = 0; i < juActivityFiles.Count; i++)
                                    {
                                        sbOther.AppendLine(string.Format("<tr onclick=\"window.open(\'{0}\')\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\">", juActivityFiles[i].FilePath));

                                        sbOther.AppendLine("<td>");
                                        sbOther.AppendFormat("<label style=\"margin-left:10px;    color: #5A8ECE;\">{0}</label>", juActivityFiles[i].FileName);
                                        sbOther.AppendLine("</td>");
                                        sbOther.AppendLine("</tr>");
                                        if (i < juActivityFiles.Count - 1)
                                        {
                                            sbOther.AppendLine("<tr>");
                                            sbOther.AppendLine("<td colspan=\"2\">");
                                            //sbOther.AppendLine("<hr style=\"border-color:#CCCCCC;\"/>");
                                            sbOther.AppendLine("<hr style=\"height:1px;border:0;border-bottom:1px solid #eee;\" />");
                                            sbOther.AppendLine("</td>");
                                            sbOther.AppendLine("</tr>");
                                        }

                                    }
                                    sbOther.AppendLine("</tbody>");
                                    sbOther.Append("</table>");
                                    sbOther.Append("</div>");
                                    sbOther.AppendLine("</div>");
                                    html = html.Replace("$CCWX-FileDownload$", sbOther.ToString());

                                }
                                html = html.Replace("$CCWX-FileDownload$", null);

                            }
                            #endregion


                            if (html.Contains("$CCWX-ARTICLEHEADCODE$"))//替换顶部code
                            {
                                html = html.Replace("$CCWX-ARTICLEHEADCODE$", GetWebsiteInfoModel().ArticleHeadCode);
                            }

                            if (html.Contains("$CCWX-ARTICLEBOTTOMCODE$"))//替换底部code
                            {
                                html = html.Replace("$CCWX-ARTICLEBOTTOMCODE$", GetWebsiteInfoModel().ArticleBottomCode);
                            }
                            if (html.Contains("$CCWX-SIGNUPTOTALCOUNT$"))//替换已经报名人数
                            {
                                html = html.Replace("$CCWX-SIGNUPTOTALCOUNT$", juActivityInfo.SignUpTotalCount.ToString());
                            }
                            //
                            ActivityConfig activityConfig = Get<ActivityConfig>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
                            if (activityConfig == null)
                            {
                                activityConfig = new ActivityConfig();
                            }
                            if (html.Contains("$CCWX-ORGANIZERNAME$"))//替换主办方名称
                            {
                                html = html.Replace("$CCWX-ORGANIZERNAME$", string.IsNullOrEmpty(activityConfig.OrganizerName) ? "主办方" : activityConfig.OrganizerName);
                            }
                            if (html.Contains("$CCWX-MYREGISTRATIONNAME$"))//替换我的报名名称
                            {
                                html = html.Replace("$CCWX-MYREGISTRATIONNAME$", string.IsNullOrEmpty(activityConfig.MyRegistrationName) ? "我的报名" : activityConfig.MyRegistrationName);
                            }
                            if (html.Contains("$CCWX-ACTIVITIESNAME$"))//替换活动名称
                            {
                                html = html.Replace("$CCWX-ACTIVITIESNAME$", string.IsNullOrEmpty(activityConfig.ActivitiesName) ? "活动日历" : activityConfig.ActivitiesName);
                            }
                            if (html.Contains("$CCWX-ORGANIZERSURL$"))//替换主办方链接
                            {
                                html = html.Replace("$CCWX-ORGANIZERSURL$", string.IsNullOrEmpty(activityConfig.TheOrganizers) ? "javascript:void(0)" : activityConfig.TheOrganizers);
                            }
                            if (html.Contains("$CCWX-ACTIVITYID$"))//替换活动报名ID
                            {
                                html = html.Replace("$CCWX-ACTIVITYID$", juActivityInfo.SignUpActivityID);//替换活动报名ID
                            }
                            //
                            if (html.Contains("$CCWX-SHAREUSERINFO$"))
                            {
                                if (spreadUser != null)//有推广者
                                {
                                    StringBuilder sbSpreadUserInfo = new StringBuilder();
                                    sbSpreadUserInfo.Append("<div style=\"background-color:Gray;color:White;vertical-align:middle;width:auto;margin-bottom:10px;text-shadow:none;\">");
                                    sbSpreadUserInfo.AppendFormat("<table><td style=\"width:55px;\"><img width=\"50\" height=\"50\" src=\"{0}\" style=\"width:auto;margin-top:2px;margin-left:2px;margin-bottom:2px;vertical-align:middle;\"/></td>", bllUser.GetUserDispalyAvatar(spreadUser));
                                    if (!string.IsNullOrEmpty(spreadUser.TrueName))
                                    {
                                        spreadUser.WXNickname = spreadUser.TrueName;
                                    }
                                    sbSpreadUserInfo.AppendFormat("<td style=\"vertical-align:middle;\">来自&nbsp;{0}&nbsp;的推荐&nbsp; ☆☆☆☆☆</td></table>", spreadUser.WXNickname);
                                    sbSpreadUserInfo.Append("</div>");
                                    html = html.Replace("$CCWX-SHAREUSERINFO$", sbSpreadUserInfo.ToString());
                                }
                                else
                                {
                                    html = html.Replace("$CCWX-SHAREUSERINFO$", null);
                                }

                            }

                            if (html.Contains("$CCWX-WXNICKNAME$"))//微信昵称
                            {

                                if (currentUserInfo != null)
                                {
                                    if (!string.IsNullOrEmpty(currentUserInfo.TrueName))
                                    {
                                        html = html.Replace("$CCWX-WXNICKNAME$", currentUserInfo.TrueName);
                                    }
                                    else if (!string.IsNullOrEmpty(currentUserInfo.WXNickname))
                                    {
                                        html = html.Replace("$CCWX-WXNICKNAME$", currentUserInfo.WXNickname);
                                    }

                                }

                                html = html.Replace("$CCWX-WXNICKNAME$", null);

                            }
                            if (html.Contains("$CCWX-WXHEADIMGURLLOCAL$"))//微信头像地址替换(查看贺卡的人)
                            {

                                if (currentUserInfo != null)
                                {
                                    if (!string.IsNullOrEmpty(currentUserInfo.WXHeadimgurlLocal))
                                    {
                                        html = html.Replace("$CCWX-WXHEADIMGURLLOCAL$", currentUserInfo.WXHeadimgurlLocal);
                                    }

                                }

                                html = html.Replace("$CCWX-WXHEADIMGURLLOCAL$", "/img/offline_user.png");

                            }
                            if (html.Contains("$CCWX-WXHEADIMGURLLOCALLECTURER$"))//微信头像地址替换(发布贺卡的人)
                            {

                                if (userInfo != null)
                                {
                                    if (!string.IsNullOrEmpty(userInfo.WXHeadimgurlLocal))
                                    {
                                        html = html.Replace("$CCWX-WXHEADIMGURLLOCALLECTURER$", userInfo.WXHeadimgurlLocal);
                                    }

                                }

                                html = html.Replace("$CCWX-WXHEADIMGURLLOCALLECTURER$", "/img/offline_user.png");

                            }
                            #region tab标题
                            if (html.Contains("$CCWX-WXTABTITLES$"))//tab标题
                            {
                                var titls = "";
                                var col = string.Empty;
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle1) && !string.IsNullOrEmpty(juActivityInfo.TabExTitle2) && !string.IsNullOrEmpty(juActivityInfo.TabExTitle3) && !string.IsNullOrEmpty(juActivityInfo.TabExTitle4))
                                {
                                    col = "col-xs-3";
                                }
                                else if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle1) && !string.IsNullOrEmpty(juActivityInfo.TabExTitle2) && !string.IsNullOrEmpty(juActivityInfo.TabExTitle3))
                                {
                                    col = "col-xs-4";
                                }
                                else if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle1) && !string.IsNullOrEmpty(juActivityInfo.TabExTitle2))
                                {
                                    col = "col-xs-6";
                                }
                                else if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle1))
                                {
                                    col = "col-xs-12";
                                }

                                if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle1))
                                {
                                    titls = "<div class=\"course-tab-title-unselect " + col + "\" data-tab-title-index=\"1\">" + juActivityInfo.TabExTitle1 + "</div>";
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle2))
                                {
                                    titls += "<div class=\"course-tab-title-unselect " + col + "\" data-tab-title-index=\"2\">" + juActivityInfo.TabExTitle2 + "</div>";
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle3))
                                {
                                    titls += "<div class=\"course-tab-title-unselect " + col + "\" data-tab-title-index=\"3\">" + juActivityInfo.TabExTitle3 + "</div>";
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExTitle4))
                                {
                                    titls += "<div class=\"course-tab-title-unselect " + col + "\" data-tab-title-index=\"4\">" + juActivityInfo.TabExTitle4 + "</div>";
                                }                   
                                html = html.Replace("$CCWX-WXTABTITLES$", titls);

                            }
                            if (html.Contains("$CCWX-WXTABCONTENTS$"))//tab标题
                            {
                                var contents = "";
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExContent1))
                                {
                                    contents = "<div class=\"course-tab-content\" data-tab-content-index=\"1\">" + juActivityInfo.TabExContent1 + "</div>";
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExContent2))
                                {
                                    contents += "<div class=\"course-tab-content\" data-tab-content-index=\"2\">" + juActivityInfo.TabExContent2 + "</div>";
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExContent3))
                                {
                                    contents += "<div class=\"course-tab-content\" data-tab-content-index=\"3\">" + juActivityInfo.TabExContent3 + "</div>";
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.TabExContent4))
                                {
                                    contents += "<div class=\"course-tab-content\" data-tab-content-index=\"4\">" + juActivityInfo.TabExContent4 + "</div>";
                                }
                                html = html.Replace("$CCWX-WXTABCONTENTS$", contents);
                            }
                            if (html.Contains("$CCWX-ARTICLECONTENTS$"))
                            {
                                var content = string.Empty;
                                if (!string.IsNullOrEmpty(juActivityInfo.ActivityDescription))
                                {
                                    content = " <div class=\"course-detail\">" + juActivityInfo.ActivityDescription + "<div style=\"clear: both;\"></div></div>";
                                }
                                html = html.Replace("$CCWX-ARTICLECONTENTS$", content);
                            }

                            #endregion

                            if (html.Contains("$CCWX-WXAMOUNT$"))
                            {

                                CrowdFundItem activityItem = bllActivity.Get<CrowdFundItem>(string.Format(" WebsiteOwner='{0}' AND CrowdFunDID={1}",bllActivity.WebsiteOwner,juActivityInfo.JuActivityID));
                                var span=string.Empty;
                                if (activityItem != null)
                                {
                                    span += "<span class=\"price-icon\">￥</span>";
                                    span += "<span class=\"price\">"+Math.Round(activityItem.Amount,1)+"</span>";
                                    span += "<span class=\"price-icon\">元</span>";
                                    span += " <span class=\"pre-price\">￥"+Math.Round(activityItem.OriginalPrice,1)+"</span>";
                                }
                                html = html.Replace("$CCWX-WXAMOUNT$", span);

                            }
                            if (html.Contains("$CCWX-WXPRODUCTINFOS$"))
                            {
                                var proHtmls = string.Empty;

                                if (!string.IsNullOrEmpty(juActivityInfo.RelationProducts))
                                {
                                    List<WXMallProductInfo> proList = bllActivity.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' AND PID in ({1}) ", bllActivity.WebsiteOwner, juActivityInfo.RelationProducts));

                                    for (int i = 0; i < proList.Count; i++)
                                    {
                                        proHtmls += "<div class=\"recommend-item\" onclick=\"window.location.href='/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/427557#/productDetail/" + proList[i].PID + "'\"><img src=\"" + proList[i].ShowImage + "\">";
                                        proHtmls += "<label>" + proList[i].PName + "</label>";
                                        proHtmls += "</div>";
                                    }
                                }
                                if (!string.IsNullOrEmpty(juActivityInfo.RelationArticles))
                                {
                                    List<JuActivityInfo> jList = bllActivity.GetList<JuActivityInfo>(string.Format(" JuActivityID in ({0}) And IsHide=0 And ArticleType in('article','activity')", juActivityInfo.RelationArticles));

                                    for (int i = 0; i < jList.Count; i++)
                                    {
                                        proHtmls += "<div class=\"recommend-item\" onclick=\"window.location.href='http://" + HttpContext.Current.Request.Url.Authority + "/" + jList[i].JuActivityIDHex + "/" + "details.chtml'\"><img src=\"" + jList[i].ThumbnailsPath + "\">";
                                        proHtmls += "<label>" + jList[i].ActivityName + "</label>";
                                        proHtmls += "</div>";
                                    }
                                }
                                
                                html = html.Replace("$CCWX-WXPRODUCTINFOS$", proHtmls);
                            }
                            if (html.Contains("$CCWX-TEL$"))
                            {
                                html = html.Replace("$CCWX-TEL$", companyWebsiteConfig.Tel);
                            }
                            if (html.Contains("$CCWX-QQ$"))
                            {
                                html = html.Replace("$CCWX-QQ$", companyWebsiteConfig.QQ);
                            }
                           
                            #endregion

                            #region 我也要发布文章或活动标题
                            if (html.Contains("$CCWX-NEWARTICLEORACTIVITYTITLE$"))//发布文章或活动标题
                            {
                                if (juActivityInfo.ArticleType.Equals("article"))
                                {
                                    html = html.Replace("$CCWX-NEWARTICLEORACTIVITYTITLE$", "我也要发文章");
                                }
                                if (juActivityInfo.ArticleType.Equals("activity"))
                                {
                                    html = html.Replace("$CCWX-NEWARTICLEORACTIVITYTITLE$", "我也要组织活动");
                                }
                                html = html.Replace("$CCWX-NEWARTICLEORACTIVITYTITLE$", null);
                            }



                            if (html.Contains("$CCWX-ACTIVITYSTATUS$") || html.Contains("$CCWX-ACTIVITYSTATUS"))//活动
                            {

                                switch (juActivityInfo.ActivityStatus)
                                {
                                    case 0://进行中
                                        html = html.Replace("$CCWX-ACTIVITYSTATUS$", "<span class=\"text\">进行中 </span><svg class=\"sanjiao\" version=\"1.1\" viewbox=\"0 0 100 100\" ><polygon points=\"100,100 0.2,100 100,0.2\" /></svg>");
                                        html = html.Replace("$CCWX-ACTIVITYSTATUS", "" + juActivityInfo.ActivityStatus + "");
                                        break;
                                    case 1://已停止
                                        html = html.Replace("$CCWX-ACTIVITYSTATUS$", "<span class=\"text\">已停止 </span><svg class=\"sanjiao\" version=\"1.1\" viewbox=\"0 0 100 100\" ><polygon points=\"100,100 0.2,100 100,0.2\" fill=\"#ddd\" /></svg>");
                                        html = html.Replace("$CCWX-ACTIVITYSTATUS", "" + juActivityInfo.ActivityStatus + "");
                                        break;
                                    default:
                                        break;
                                }




                                html = html.Replace("$CCWX-ACTIVITYSTATUS$", null);
                            }
                            #endregion

                            #region 报名按钮
                            if (html.Contains("$CCWX-ACTIVITYISSIGNIN8$"))
                            {
                                switch (juActivityInfo.ActivityStatus)
                                {
                                    case 0://进行中
                                        if (signUpData == null)
                                        {
                                            html = html.Replace("$CCWX-ACTIVITYISSIGNIN8$", "<span class=\"wbtn wbtn_line_main\" id=\"applyactivebtn\"><span class=\"iconfont icon-b55 smallicon\"></span>立即申请</span>");
                                        }
                                        else
                                        {
                                            html = html.Replace("$CCWX-ACTIVITYISSIGNIN8$", "<span class=\"wbtn wbtn_line_main\" id=\"applyaend\" style=\"color: #ccc!important;\"><span class=\"iconfont icon-b55 smallicon\" style=\"color: #ccc!important;\"></span>您已申请</span>");
                                        }

                                        break;
                                    case 1://已停止
                                        html = html.Replace("$CCWX-ACTIVITYISSIGNIN8$", "<span class=\"wbtn wbtn_line_main\" style=\"color:#ccc!important;\" id=\"applyactivebtn1\"><span class=\"iconfont icon-b55 smallicon\" style=\"color: #ccc!important;\"></span>活动已结束</span>");
                                        break;
                                    default:
                                        break;
                                }
                                html = html.Replace("$CCWX-ACTIVITYISSIGNIN8$", null);
                            }

                            if (html.Contains("$CCWX-ACTIVITYISSIGNIN7$"))
                            {
                                switch (juActivityInfo.ActivityStatus)
                                {
                                    case 0://进行中
                                        if (juActivityInfo.IsFee == 1)
                                        {
                                            if (signUpData == null)
                                            {
                                                html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", "<span class=\"wbtn wbtn_line_main\" id=\"applyactivebtn\"><span class=\"iconfont icon-b55 smallicon\"></span>立即报名</span>");
                                            }
                                            else
                                            {
                                                if (signUpData.PaymentStatus == 0)
                                                {
                                                    html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", "<span class=\"wbtn wbtn_line_main\" id=\"gopay\" onclick=\"rePay()\"><span class=\"iconfont icon-b55 smallicon\"></span>您已报名,去支付</span>");
                                                }
                                                else
                                                {
                                                    html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", "<span class=\"wbtn wbtn_line_main\" id=\"applyend\"><span class=\"iconfont icon-b55 smallicon\"></span><a href=\"/App/Cation/Wap/MyActivityLlists.aspx\">您已报名,前往查看</a></span>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (signUpData == null)
                                            {
                                                html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", "<span class=\"wbtn wbtn_line_main\" id=\"applyactivebtn\"><span class=\"iconfont icon-b55 smallicon\"></span>立即报名</span>");
                                            }
                                            else
                                            {
                                                html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", "<span class=\"wbtn wbtn_line_main\" id=\"applyend\"><span class=\"iconfont icon-b55 smallicon\"></span><a href=\"/App/Cation/Wap/MyActivityLlists.aspx\">您已报名,前往查看</a></span>");
                                            }
                                        }
                                        break;
                                    case 1://已停止
                                        html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", "<span class=\"wbtn wbtn_line_main\" id=\"applyactivebtn1\" style=\"color: #ccc!important;\"><span class=\"iconfont icon-b55 smallicon\" style=\"color: #ccc!important;\"></span>活动已结束</span>");
                                        break;
                                    default:
                                        break;
                                }
                                html = html.Replace("$CCWX-ACTIVITYISSIGNIN7$", null);
                            }
                            #endregion

                            #region 登录链接
                            if (html.Contains("$CCWX-APPLOGINURL$"))
                            {
                                string appLoginUrl = Common.ConfigHelper.GetConfigString("appLoginUrl").ToLower();
                                html = html.Replace("$CCWX-APPLOGINURL$", appLoginUrl);
                            }
                            #endregion

                            #region 文章活动底部
                            if (html.Contains("$CCWX-ARTICLEBOTTOM$"))
                            {

                                StringBuilder sbBottom = new StringBuilder();
                                if (companyWebsiteConfig != null)
                                {
                                    List<CompanyWebsite_ToolBar> navigationList = new List<CompanyWebsite_ToolBar>();
                                    switch (juActivityInfo.ArticleType)
                                    {
                                        case "article":
                                            if (!string.IsNullOrEmpty(companyWebsiteConfig.ArticleToolBarGrous))
                                            {
                                                navigationList = bllUser.GetList<CompanyWebsite_ToolBar>(string.Format(" WebsiteOwner='{0}' And KeyType='{1}' Order By PlayIndex ASC", bllUser.WebsiteOwner, companyWebsiteConfig.ArticleToolBarGrous));
                                            }
                                            break;
                                        case "activity":
                                            if (!string.IsNullOrEmpty(companyWebsiteConfig.ActivityToolBarGrous))
                                            {
                                                navigationList = bllUser.GetList<CompanyWebsite_ToolBar>(string.Format(" WebsiteOwner='{0}' And KeyType='{1}' Order By PlayIndex ASC", bllUser.WebsiteOwner, companyWebsiteConfig.ActivityToolBarGrous));
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    sbBottom.AppendFormat("<footer>");
                                    sbBottom.AppendFormat("<ul>");
                                    foreach (var item in navigationList)
                                    {
                                        sbBottom.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", item.ToolBarTypeValue, item.ToolBarName);
                                    }

                                    sbBottom.AppendFormat("</ul>");
                                    sbBottom.AppendFormat("<div class=\"copyright-box\">{0}</div>", companyWebsiteConfig.Copyright);
                                    sbBottom.AppendFormat("</footer>");
                                    html = html.Replace("$CCWX-ARTICLEBOTTOM$", sbBottom.ToString());


                                }


                                html = html.Replace("$CCWX-ARTICLEBOTTOM$", null);
                            }
                            #endregion

                            #region 阅读加积分
                            if (currentUserInfo != null)
                            {
                                try
                                {

                                    string tempMsg = "";

                                    bllUser.AddUserScoreDetail(currentUserInfo.UserID, "ReadType", bllUser.WebsiteOwner, out tempMsg, null, null, juActivityInfo.ArticleType,true, juActivityInfo.ArticleType);
                                    
                                    bllUser.AddUserScoreDetail(currentUserInfo.UserID, "ReadCategory", bllUser.WebsiteOwner, out tempMsg, null, null, juActivityInfo.CategoryId,true, juActivityInfo.CategoryId);
                                    
                                    bllUser.AddUserScoreDetail(currentUserInfo.UserID, "ReadArticle", bllUser.WebsiteOwner, out tempMsg, null,"《" + juActivityInfo.ActivityName + "》", juActivityInfo.JuActivityID.ToString(),true, juActivityInfo.JuActivityID.ToString(),juActivityInfo.ArticleType);
                                
                                }
                                catch (Exception)
                                {


                                }

                            }
                            #endregion

                            #region 是否报名
                            if (html.Contains("$CCWX-ISSIGNUP$"))
                            {
                                if (signUpData!=null)
                                {
                                    html = html.Replace("$CCWX-ISSIGNUP$","1");
                                }
                                html = html.Replace("$CCWX-ISSIGNUP$","0");
                            } 
                            #endregion
                            
                            #region 替换浏览器小图标
                            if (html.Contains("$CCWX-WEBSITEWEIXINICO$"))//替换浏览器小图标
                            {

                                if (!string.IsNullOrWhiteSpace(companyWebsiteConfig.DistributionQRCodeIcon))
                                {
                                    html = html.Replace("$CCWX-WEBSITEWEIXINICO$", string.Format("<link type=\"image/x-icon\" rel=\"shortcut icon\" href=\"{0}\" />", companyWebsiteConfig.DistributionQRCodeIcon));
                                }
                                html = html.Replace("$CCWX-WEBSITEWEIXINICO$", "");
                            }
                            #endregion

                        }
                        else
                        {
                            //if (!string.IsNullOrEmpty(articleInfo.SignUpActivityID))
                            //{
                            //    html = "报名已结束，有疑问请联系我们";
                            //}
                            //else
                            //{

                            //} 
                            html = "该文章不显示";

                        }
                    }
                    else
                    {
                        html = "该文章已经删除";
                    }
                //}
                //else
                //{
                //    html = "不存在的文章";
                //}

                ToLog("GetJuactivityHtml执行完毕");
                return html;//返回最终Html
            }
            catch (Exception ex)
            {
                //ToLog(ex.ToString());
                return ex.ToString();
            }


        }
        /// <summary>
        /// 嵌入投票
        /// </summary>
        /// <param name="voteId"></param>
        /// <returns></returns>
        public string GetTheVoteInfo(string voteId)
        {
            StringBuilder str = new StringBuilder();
            BLLJIMP.Model.TheVoteInfo theVoteInfo = Get<BLLJIMP.Model.TheVoteInfo>(string.Format(" TheVoteGUID='{0}'", voteId));
            if (theVoteInfo != null)
            {
                List<BLLJIMP.Model.DictionaryInfo> dInfos = GetList<BLLJIMP.Model.DictionaryInfo>(" ForeignKey='" + theVoteInfo.AutoId + "'");
                int height = 133 + (40 * dInfos.Count);
                //return "<iframe src=\"http://xixinxian.comeoncloud.net/App/TheVote/wap/WxTheVoteInfo.aspx?AutoId=" + theVoteInfo.AutoId + "\" width=\"100%\" height=\"" + height + "px\" style=\"border:none\"></iframe>";

                BLLJIMP.Model.TheVoteInfo tvInfo = Get<BLLJIMP.Model.TheVoteInfo>(" autoId=" + theVoteInfo.AutoId);
                if (tvInfo != null)
                {
                    return ToHtml(tvInfo);
                }
                else
                {
                    str.AppendFormat("<li>");
                    str.AppendFormat("<div>{0}</div>", "没有数据");
                    str.AppendFormat("</li>");
                    return str.ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// 返回投票内容
        /// </summary>
        /// <param name="tvInfo"></param>
        /// <returns></returns>
        public string ToHtml(BLLJIMP.Model.TheVoteInfo tvInfo)
        {
            StringBuilder sbHtml = new StringBuilder();
            string classStr = "";
            string selectStr = "";
            string str = "";
            UserInfo userInfo;
            BLLJIMP.Model.UserVoteInfo uvInfo = null;
            if (IsLogin)
            {
                userInfo = GetCurrentUserInfo();
                string whereStr = string.Format(" UserId='{0}' and VoteId='{1}'", userInfo.UserID, tvInfo.AutoId);
                uvInfo = Get<BLLJIMP.Model.UserVoteInfo>(whereStr);
            }

            if (uvInfo != null)
            {
                classStr = "toupiaobox toupiaoover";
            }
            else
            {
                if (tvInfo.VoteSelect == "1")
                {
                    classStr = "toupiaobox";
                    selectStr = "radio";
                    str = "以下选项为单选";
                }
                else if (tvInfo.VoteSelect == "2")
                {
                    classStr = "toupiaobox duoxuan";
                    selectStr = "checkbox";
                    str = "以下选项为多选";
                    if (tvInfo.MaxSelectItemCount > 0)
                    {
                        str += string.Format("(最多可选{0}项）", tvInfo.MaxSelectItemCount);
                    }

                }
            }
            sbHtml.Append("<section class=\"box\"><ul class=\"mainlist articlelist currentlist\" id=\"needList\" runat=\"server\">");


            sbHtml.AppendFormat("<link rel=\"stylesheet\" href=\"styles/css/style.css?v=0.0.1\">");
            sbHtml.AppendFormat("<div id=\"toupiao\" class=\"{0}\">", classStr);
            sbHtml.AppendFormat("<div class=\"title\">{0}</div>", tvInfo.VoteName);
            sbHtml.AppendFormat("<p class=\"note\">{0}</p>", str);
            sbHtml.AppendFormat("<input type=\"hidden\" data-role=\"none\" id=\"SelectStr\"  value=\"{0}\" />", selectStr);
            sbHtml.AppendFormat("<input type=\"hidden\" id=\"AutoId\" data-role=\"none\" value=\"{0}\" />", tvInfo.AutoId);
            List<BLLJIMP.Model.DictionaryInfo> dInfos = GetList<BLLJIMP.Model.DictionaryInfo>(" ForeignKey='" + tvInfo.AutoId + "'");
            foreach (var item in dInfos)
            {
                sbHtml.AppendFormat("<input name=\"radiocheck2\" data-role=\"none\" type=\"{0}\" class=\"radioinput\" id=\"{1}\" value=\"\" v=\"{2}\">", selectStr, selectStr + item.AutoID, item.AutoID);
                sbHtml.AppendFormat("<div class=\"mainconcent\">");
                sbHtml.AppendFormat("<label class=\"inputlabel\" for=\"{0}\"></label>", selectStr + item.AutoID);
                sbHtml.AppendFormat("<span class=\"inputicon\"><span class=\"icon\"></span></span>");
                sbHtml.AppendFormat("<div class=\"inputtext\" >{0}</div>", item.ValueStr);
                sbHtml.AppendFormat("<div class=\"jindubar\">");
                double f = 0;
                if (tvInfo.VoteNumbers != 0)
                {
                    f = Math.Round((Convert.ToDouble(item.VoteNums) / Convert.ToDouble(tvInfo.VoteNumbers)) * 100, 0);
                }
                sbHtml.AppendFormat("<span class=\"jindu\" style=\"width:{0}%;\"></span>", f);
                sbHtml.AppendFormat("<span class=\"peoplenum\">{0}票</span>", item.VoteNums);


                sbHtml.AppendFormat("<span class=\"pecent\">{0}%</span>", f);
                sbHtml.AppendFormat("</div></div>");
            }
            string itemName = "";
            if (uvInfo != null)
            {
                dInfos = GetList<BLLJIMP.Model.DictionaryInfo>(" AutoId in (" + uvInfo.DiInfoId + ")");

                if (dInfos != null)
                {
                    foreach (BLLJIMP.Model.DictionaryInfo item in dInfos)
                    {
                        itemName += item.ValueStr + " ";
                    }
                }
            }

            sbHtml.AppendFormat("<span class=\"button\" id=\"btnSave\" onclick=\"SaveInfo()\">投票</span>");
            sbHtml.AppendFormat("<span class=\"toupiaoinfo\" >你已投过票，投票项为\"{0}\"</span>", itemName);
            sbHtml.AppendFormat("</div>");
            sbHtml.Append("</ul></section>");
            return sbHtml.ToString();
        }


        private void ToLog(string msg)
        {
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
            //    {
            //        sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
            //    }
            //}
            //catch { }
        }


        #region GetArticleHtmlByArticleID老方法备份 - 2013.11.26
        //public string GetArticleHtmlByArticleID(int juActivityID, string currOpenerOpenID = "")
        //{
        //    try
        //    {
        //        ///最终输出的Html
        //        string Html = string.Empty; ;
        //        JuActivityInfo articleInfo = Get<JuActivityInfo>(string.Format("JuActivityID={0}", juActivityID));
        //        if (articleInfo != null)//检查文章是否存在
        //        {
        //            if (!articleInfo.IsDelete.Equals(1))//检查文章是否删除
        //            {
        //                if (articleInfo.IsHide.Equals(0))
        //                {

        //                    UserInfo userInfo = Get<UserInfo>(string.Format("UserID='{0}'", articleInfo.UserID));//文章发布者信息
        //                    SystemSet systemset = Get<SystemSet>("");//系统配置信息

        //                    #region 更新打开人次
        //                    if (articleInfo.OpenCount == null)
        //                    {
        //                        articleInfo.OpenCount = 0;
        //                    }
        //                    articleInfo.OpenCount++;
        //                    Update(articleInfo);


        //                    #endregion

        //                    #region 内部链接
        //                    if (articleInfo.IsByWebsiteContent.Equals(0))//内部链接
        //                    {
        //                        if (articleInfo.ArticleTemplate.Equals(0))// 空模板
        //                        {

        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit0.htm"), Encoding.UTF8);



        //                        }
        //                        if (articleInfo.ArticleTemplate.Equals(1))// 微信官方模板
        //                        {
        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/weixin/ArticleTemplate/jubit1.htm"), Encoding.UTF8);

        //                        }

        //                        if (articleInfo.ArticleTemplate.Equals(2))// 聚比特模板
        //                        {

        //                            Html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/jubit2.htm"), Encoding.UTF8);



        //                        }


        //                        if (articleInfo.IsSignUpJubit > 0)
        //                        {
        //                            //读取个性化模板
        //                            Html = Html.Replace("$JUTP-TPSignForm$", Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/TPSignForm.htm"), Encoding.UTF8));
        //                        }
        //                        else
        //                        {
        //                            Html = Html.Replace("$JUTP-TPSignForm$", "");
        //                        }



        //                    }
        //                    #endregion

        //                    #region 外部链接
        //                    else//外部链接
        //                    {
        //                        //下载外部链接源代码
        //                        Html = Common.MySpider.GetPageSourceForUTF8(articleInfo.ActivityWebsite);

        //                    }
        //                    #endregion

        //                    #region 追加报名表单
        //                    //追加报名表单                
        //                    // 符合条件的情况
        //                    //1:使用外部链接并使用报名.
        //                    //2:使用内部链接并使用微信官方模板并使用报名.
        //                    //if ((articleInfo.IsByWebsiteContent.Equals(1) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)))//使用外部链接并使用报名.
        //                    //    ||
        //                    //    //使用内部链接并使用微信官方模板并使用报名.
        //                    //    (articleInfo.IsByWebsiteContent.Equals(0) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)) && articleInfo.ArticleTemplate.Equals(1))
        //                    //    )

        //                    if (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2))//有报名
        //                    {
        //                        StringBuilder sbCheckForm = new StringBuilder();//追加检查form
        //                        StringBuilder sbAppend = new StringBuilder();//Html后追加的html
        //                        StringBuilder sbSignInHtml = new StringBuilder();//报名表单 格式<tr></tr>
        //                        StringBuilder sbcurrMemberInfo = new StringBuilder();//当前会员隐藏信息
        //                        //FORMDATA
        //                        ActivityInfo planactivityInfo = Get<ActivityInfo>(string.Format("ActivityID='{0}'", articleInfo.SignUpActivityID));
        //                        List<ActivityFieldMappingInfo> activityFieldMappingInfoList = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' Order by ExFieldIndex ASC", planactivityInfo.ActivityID));

        //                        WXMemberInfo currOpenerMemberInfo = new WXMemberInfo();
        //                        //Dictionary<string, string> currOpenerInfo = new Dictionary<string, string>();
        //                        bool currOpenerIsMember = false;
        //                        if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
        //                        {
        //                            //如果有openID传入，则检查是否是会员，是的话则查询出姓名 手机 邮箱数据
        //                            currOpenerMemberInfo = Get<WXMemberInfo>(string.Format(" UserID = '{0}' and WeixinOpenID = '{1}' ", articleInfo.UserID, currOpenerOpenID));
        //                            if (currOpenerMemberInfo != null)
        //                                currOpenerIsMember = true;

        //                        }

        //                        if (!currOpenerIsMember)
        //                        {
        //                            sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"txtName\" >姓名:</label></td></tr>");
        //                            sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"Name\" id=\"txtName\" value=\"\" placeholder=\"请输入姓名\" style=\"width:100%;\"></td></tr>");

        //                            sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"txtPhone\"  >手机号码:</label></td></tr>");
        //                            sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"Phone\" id=\"txtPhone\" value=\"\" placeholder=\"请输入手机号码\" style=\"width:100%;\"></td></tr>");
        //                            //手机号
        //                        }
        //                        else
        //                        {
        //                            sbcurrMemberInfo.AppendFormat("<input id=\"txtName\" type=\"hidden\" value=\"{0}\" name=\"Name\" />", currOpenerMemberInfo.Name);//姓名
        //                            sbcurrMemberInfo.AppendFormat("<input id=\"txtPhone\" type=\"hidden\" value=\"{0}\" name=\"Phone\" />", currOpenerMemberInfo.Phone);//手机
        //                            //Phone
        //                        }
        //                        //其它报名字段
        //                        foreach (ActivityFieldMappingInfo item in activityFieldMappingInfoList)
        //                        {
        //                            if (item.FieldType != 1)//普通字段
        //                            {
        //                                if (currOpenerIsMember)
        //                                {
        //                                    bool tmpIsMp = false;//是否对应
        //                                    string tmpValue = "";

        //                                    //姓名

        //                                    //手机

        //                                    //判断是不是公司，是的话则隐藏并赋值
        //                                    if (item.MappingName == "公司" && !tmpIsMp)
        //                                    {
        //                                        tmpValue = currOpenerMemberInfo.Company;
        //                                        tmpIsMp = true;
        //                                    }

        //                                    //判断是不是职位，是的话则隐藏并赋值
        //                                    if (item.MappingName == "职位" && !tmpIsMp)
        //                                    {
        //                                        tmpValue = currOpenerMemberInfo.Postion;
        //                                        tmpIsMp = true;
        //                                    }

        //                                    if (!tmpIsMp)
        //                                    {
        //                                        //判断是不是邮箱，是的话则隐藏并赋值
        //                                        List<string> tmpEmailMp = new List<string>() { "邮箱", "邮件", "email" };
        //                                        foreach (var i in tmpEmailMp)
        //                                        {
        //                                            if (item.MappingName.ToLower().Contains(i))
        //                                            {
        //                                                tmpValue = currOpenerMemberInfo.Email;
        //                                                tmpIsMp = true;
        //                                                break;
        //                                            }
        //                                        }
        //                                    }

        //                                    if (tmpIsMp)
        //                                    {
        //                                        sbcurrMemberInfo.AppendFormat("<input type=\"hidden\" value=\"{0}\"  name=\"K{1}\" id=\"K{1}\"  />", tmpValue, item.ExFieldIndex);//邮箱
        //                                        continue;
        //                                    }





        //                                }
        //                                sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"K{0}\">{1}</label></td></tr>", item.ExFieldIndex, item.MappingName);

        //                                if (item.IsMultiline.Equals(1))//<textarea name="K1" id="txtContent" style="height: 100px;" placeholder="请详细描述您的建议、意见、问题等。"></textarea>
        //                                    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><textarea rows=\"5\" type=\"text\" name=\"K{0}\" id=\"K{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;height: 200px;\"></textarea> </td></tr>", item.ExFieldIndex, item.MappingName);
        //                                else
        //                                    sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"K{0}\" id=\"K{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;\"/> </td></tr>", item.ExFieldIndex, item.MappingName);


        //                            }
        //                            else//微信推广字段
        //                            {
        //                                sbSignInHtml.AppendFormat("<tr><td><input type=\"hidden\" name=\"K{0}\" id=\"K{0}\" value=\"$CCWXTG-LINKID$\" /></td></tr> ", item.ExFieldIndex);
        //                            }

        //                            if (item.FieldIsNull.Equals(1))
        //                            {

        //                                sbCheckForm.AppendFormat("if (!document.getElementById(\"K{0}\").value)", item.ExFieldIndex);

        //                                sbCheckForm.AppendLine("{");
        //                                sbCheckForm.AppendFormat("alert(\"请输入{0}\");", item.MappingName);
        //                                sbCheckForm.AppendLine(" return false;");
        //                                sbCheckForm.AppendLine("}");
        //                            }


        //                        }
        //                        //其它报名字段

        //                        //其它报名信息
        //                        //sbSignInHtml.AppendLine("<div  style=\"margin-top: 10px;\">");
        //                        sbSignInHtml.AppendLine("<tr><td>");
        //                        sbSignInHtml.AppendFormat("<input id=\"activityID\" type=\"hidden\" value=\"{0}\" name=\"ActivityID\" />", planactivityInfo.ActivityID);//活动ID
        //                        sbSignInHtml.AppendFormat("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\" />", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID));//外部登录名
        //                        sbSignInHtml.AppendFormat("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\" />", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password));//外部登录密码

        //                        sbSignInHtml.AppendLine(sbcurrMemberInfo.ToString());//添加会员信息

        //                        //不允许重复字段
        //                        if (planactivityInfo.DistinctKeys != null)
        //                        {
        //                            if (!string.IsNullOrEmpty(planactivityInfo.DistinctKeys))
        //                            {
        //                                sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"DistinctKeys\">", planactivityInfo.DistinctKeys);
        //                            }
        //                        }
        //                        sbSignInHtml.AppendLine("</td></tr>");
        //                        //不允许重复字段

        //                        if (articleInfo.IsByWebsiteContent.Equals(1))//外部链接报名
        //                        {
        //                            sbAppend.Append(Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/wx_externalsignuptemplate.htm"), Encoding.UTF8));//外部报名表单模板
        //                            sbAppend = sbAppend.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //                            if (Html.Contains("</body>"))
        //                            {
        //                                Html = Html.Replace("</body>", string.Format("{0}</body>", sbAppend));
        //                            }
        //                            else
        //                            {
        //                                Html += sbAppend.ToString();
        //                            }

        //                            Html = Html.Replace("$CCWX-CHECKFORM$", sbCheckForm.ToString());


        //                        }
        //                        else
        //                        {

        //                            //内部链接报名
        //                            Html = Html.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //                        }

        //                    }


        //                    //追加报名表单 
        //                    #endregion

        //                    #region 替换微信远程图片为本地路径

        //                    System.Text.RegularExpressions.Regex regImg = new System.Text.RegularExpressions.Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //                    System.Text.RegularExpressions.MatchCollection matches = regImg.Matches(Html);
        //                    foreach (System.Text.RegularExpressions.Match match in matches)
        //                    {
        //                        String name = match.ToString();
        //                        string reomoteurl = match.Groups["imgUrl"].Value;
        //                        if (reomoteurl.StartsWith("http://mmsns.qpic.cn/") || reomoteurl.StartsWith("http://mmbiz.qpic.cn/") || reomoteurl.StartsWith("http://res.wx.qq.com/"))//替换微信官方图片地址为本地 
        //                        {
        //                            Html = Html.Replace(name, string.Format("<img src=\"http://{0}:{1}{2}\">", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, DownLoadRemoteImage(reomoteurl)));

        //                        }


        //                    }

        //                    #endregion

        //                    #region 替换文章标签

        //                    if (Html.Contains("$CCWX-ARTICLEIMAGE$"))//替换分享图片地址
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEIMAGE$", string.Format("http://{0}:{1}{2}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, articleInfo.ThumbnailsPath));
        //                    }

        //                    if (Html.Contains("$CCWXCALLBACKURL$"))//替换回调地址 
        //                    {
        //                        Html = Html.Replace("$CCWXCALLBACKURL$", string.Format("http://{0}:{1}{2}?id={3}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.FilePath, articleInfo.JuActivityID));
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLETITLE$"))//替换标题
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLETITLE$", articleInfo.ActivityName);//替换标题

        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLETIME$"))//替换时间
        //                    {
        //                        if (articleInfo.IsSignUpJubit.Equals(0))//普通文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", string.Format("{0:f}", articleInfo.CreateDate));//替换时间
        //                        }
        //                        else//活动文章
        //                        {
        //                            Html = Html.Replace("$CCWX-ARTICLETIME$", string.Format("{0:f}", articleInfo.ActivityStartDate));//替换时间
        //                        }


        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEADDRESS$"))//替换地址
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEADDRESS$", articleInfo.ActivityAddress == null || articleInfo.ActivityAddress == "" ? userInfo.WeixinPublicName : articleInfo.ActivityAddress);//替换地址
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLECONTENT$"))//替换内容
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLECONTENT$", articleInfo.ActivityDescription);//替换内容
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEOPENCOUNT$"))//替换打开人次
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEOPENCOUNT$", articleInfo.OpenCount.ToString());//替换打开人次
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEUPCOUNT$"))//替换赞人数
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEUPCOUNT$", articleInfo.UpCount.ToString());//替换赞人数
        //                    }
        //                    if (Html.Contains("$CCWX-ARTICLEID$"))//替换文章ID
        //                    {
        //                        Html = Html.Replace("$CCWX-ARTICLEID$", articleInfo.JuActivityID.ToString());//替换文章ID
        //                    }

        //                    #endregion



        //                }
        //                else
        //                {
        //                    Html = "该文章不显示";
        //                }
        //            }
        //            else
        //            {
        //                Html = "该文章已经删除";
        //            }
        //        }
        //        else
        //        {
        //            Html = "不存在的文章";
        //        }


        //        return Html;//返回最终Html
        //    }
        //    catch (Exception ex)
        //    {

        //        return ex.ToString();
        //    }


        //}
        #endregion


        ///// <summary>
        ///// 创建数据提交表单
        ///// </summary>
        ///// <param name="activityID"></param>
        ///// <param name="resultHtml"></param>
        ///// <param name="currOpenerOpenID"></param>
        //public void CreareSignInFormHtml(string activityID, ref string resultHtml, string currOpenerOpenID = "", string formTpName = "")
        //{
        //    #region 追加报名表单
        //    //追加报名表单                
        //    // 符合条件的情况
        //    //1:使用外部链接并使用报名.
        //    //2:使用内部链接并使用微信官方模板并使用报名.
        //    //if ((articleInfo.IsByWebsiteContent.Equals(1) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)))//使用外部链接并使用报名.
        //    //    ||
        //    //    //使用内部链接并使用微信官方模板并使用报名.
        //    //    (articleInfo.IsByWebsiteContent.Equals(0) && (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2)) && articleInfo.ArticleTemplate.Equals(1))
        //    //    )

        //    if (articleInfo.IsSignUpJubit.Equals(1) || articleInfo.IsSignUpJubit.Equals(2))//有报名
        //    {
        //        StringBuilder sbCheckForm = new StringBuilder();//追加检查form
        //        StringBuilder sbAppend = new StringBuilder();//Html后追加的html
        //        StringBuilder sbSignInHtml = new StringBuilder();//报名表单 格式<tr></tr>
        //        //FORMDATA
        //        ActivityInfo planactivityInfo = Get<ActivityInfo>(string.Format("ActivityID='{0}'", articleInfo.SignUpActivityID));
        //        List<ActivityFieldMappingInfo> activityFieldMappingInfoList = GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}' Order by ExFieldIndex ASC", planactivityInfo.ActivityID));


        //        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"txtName\" >姓名:</label></td></tr>");
        //        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"Name\" id=\"txtName\" value=\"\" placeholder=\"姓名\" style=\"width:100%;\"></td></tr>");

        //        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"txtPhone\"  >手机号码:</label></td></tr>");
        //        sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"Phone\" id=\"txtPhone\" value=\"\" placeholder=\"手机号码\" style=\"width:100%;\"></td></tr>");
        //        //手机号

        //        //其它报名字段
        //        foreach (ActivityFieldMappingInfo item in activityFieldMappingInfoList)
        //        {

        //            if (item.FieldType != 1)//普通字段
        //            {

        //                sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td><label for=\"K{0}\">{1}</label></td></tr>", item.ExFieldIndex, item.MappingName);
        //                sbSignInHtml.AppendFormat("<tr style=\"width:100%;\"><td style=\"width:100%;\"><input style=\"width:100%\" type=\"text\" name=\"K{0}\" id=\"K{0}\" value=\"\" placeholder=\"{1}\" style=\"width:100%;\"/> </td></tr>", item.ExFieldIndex, item.MappingName);

        //            }
        //            else//微信推广字段
        //            {
        //                sbSignInHtml.AppendFormat("<tr><td><input type=\"hidden\" name=\"K{0}\" id=\"K{0}\" value=\"$CCWXTG-LINKID$\" /></td></tr> ", item.ExFieldIndex);
        //            }
        //            if (item.FieldIsNull.Equals(1))
        //            {

        //                sbCheckForm.AppendFormat("if (!document.getElementById(\"K{0}\").value)", item.ExFieldIndex);

        //                sbCheckForm.AppendLine("{");
        //                sbCheckForm.AppendFormat("alert(\"请输入{0}\");", item.MappingName);
        //                sbCheckForm.AppendLine(" return false;");
        //                sbCheckForm.AppendLine("}");



        //            }


        //        }
        //        //其它报名字段

        //        //其它报名信息
        //        //sbSignInHtml.AppendLine("<div  style=\"margin-top: 10px;\">");
        //        sbSignInHtml.AppendLine("<tr><td>");
        //        sbSignInHtml.AppendFormat("<input id=\"activityID\" type=\"hidden\" value=\"{0}\" name=\"ActivityID\">", planactivityInfo.ActivityID);//活动ID
        //        sbSignInHtml.AppendFormat("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\">", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID));//外部登录名
        //        sbSignInHtml.AppendFormat("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\">", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password));//外部登录密码

        //        //不允许重复字段
        //        if (planactivityInfo.DistinctKeys != null)
        //        {
        //            if (!string.IsNullOrEmpty(planactivityInfo.DistinctKeys))
        //            {
        //                sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"DistinctKeys\">", planactivityInfo.DistinctKeys);
        //            }
        //        }
        //        sbSignInHtml.AppendLine("</td></tr>");
        //        //不允许重复字段

        //        if (articleInfo.IsByWebsiteContent.Equals(1))//外部链接报名
        //        {
        //            sbAppend.Append(Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/Weixin/ArticleTemplate/tpdatabase/wx_externalsignuptemplate.htm"), Encoding.UTF8));//外部报名表单模板
        //            sbAppend = sbAppend.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //            if (Html.Contains("</body>"))
        //            {
        //                Html = Html.Replace("</body>", string.Format("{0}</body>", sbAppend));
        //            }
        //            else
        //            {
        //                Html += sbAppend.ToString();
        //            }

        //            Html = Html.Replace("$CCWX-CHECKFORM$", sbCheckForm.ToString());


        //        }
        //        else
        //        {

        //            //内部链接报名
        //            Html = Html.Replace("$CCWX-FORMDATA$", sbSignInHtml.ToString());
        //        }

        //    }


        //    //追加报名表单 
        //    #endregion

        //}

        /// <summary>
        /// 下载远程地址图片存到本地
        /// </summary>
        /// <param name="remoteUrl">远程图片地址</param>
        /// <returns></returns>
        public string DownLoadRemoteImage(string remoteUrl,int inOss = 1)
        {
            string result = string.Empty;

            bool hasPicGather = false;

            string absoluteDirectory = System.Web.HttpContext.Current.Server.MapPath("/FileUpload/ImageMapping/");
            if (!Directory.Exists(absoluteDirectory)) Directory.CreateDirectory(absoluteDirectory);

            JuActivityImageMapping model = Get<JuActivityImageMapping>(string.Format("RemoteUrl='{0}'", remoteUrl));

            if (model != null)
            {
                result = model.LocalPath;
                string imgOrgPath = System.Web.HttpContext.Current.Server.MapPath(model.LocalPath);

                if (!File.Exists(imgOrgPath)) hasPicGather = true;//找不到文件 重新下载

            }
            else
            {

                model = new JuActivityImageMapping();

                model.RemoteUrl = remoteUrl;
                model.LocalPath = string.Format("/FileUpload/ImageMapping/{0}.jpg", Guid.NewGuid().ToString());

                if (Add(model))
                {
                    hasPicGather = true;
                }
                else
                {
                    return "";
                }
            }
            if (hasPicGather) ZentCloud.Common.MySpider.PicGather(remoteUrl, System.Web.HttpContext.Current.Server.MapPath(model.LocalPath));

            return model.LocalPath;
        }

        public string DownLoadRemoteImageLocal(string remoteUrl)
        {
            string websitePath = Common.ConfigHelper.GetConfigString("WebSitePath");

            if (string.IsNullOrWhiteSpace(websitePath))
            {
                websitePath = "D:\\WebSite\\CommonPlatform";
            }

            string result = string.Empty;

            bool hasPicGather = false;

            string absoluteDirectory = websitePath + "/FileUpload/ImageMapping/";
            if (!Directory.Exists(absoluteDirectory)) Directory.CreateDirectory(absoluteDirectory);

            JuActivityImageMapping model = Get<JuActivityImageMapping>(string.Format("RemoteUrl='{0}'", remoteUrl));

            if (model != null)
            {
                result = model.LocalPath;
                string imgOrgPath = websitePath + model.LocalPath;

                if (!File.Exists(imgOrgPath)) hasPicGather = true;//找不到文件 重新下载

            }
            else
            {

                model = new JuActivityImageMapping();

                model.RemoteUrl = remoteUrl;
                model.LocalPath = string.Format("/FileUpload/ImageMapping/{0}.jpg", Guid.NewGuid().ToString());

                if (Add(model))
                {
                    hasPicGather = true;
                }
                else
                {
                    return "";
                }
            }
            if (hasPicGather) ZentCloud.Common.MySpider.PicGather(remoteUrl, websitePath + model.LocalPath);

            return model.LocalPath;
        }


        /// <summary>
        /// 下载远程地址图片存到OSS
        /// </summary>
        /// <param name="remoteUrl">远程图片地址</param>
        /// <returns></returns>
        public string DownLoadImageToOss(string remoteUrl,string websiteOwner = "",bool sourceImgIsLocal = false)
        {
            string result = string.Empty;
            bool hasPicGather = false;
            bool isNew = false;

            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                websiteOwner = WebsiteOwner;
            }

            JuActivityImageMapping model = Get<JuActivityImageMapping>(string.Format("RemoteUrl='{0}' and InOss=1", remoteUrl));
            
            if (model != null)
            {
                if (!new BLLUploadOtherServer().HasWindowsAzure(GetWebsiteInfoModelFromDataBase()))
                {
                    string bucket = OssHelper.GetBucket(websiteOwner);
                    if (!OssHelper.CheckFile(bucket, model.LocalPath))
                    {
                        hasPicGather = true;
                    }
                    else
                    {
                        result = model.LocalPath;
                    }
                }
                else
                {
                    hasPicGather = true;
                }
               
            }
            else
            {
                model = new JuActivityImageMapping();
                model.RemoteUrl = remoteUrl;
                model.InOss = 1;
                hasPicGather = true;
                isNew = true;
            }
            if (hasPicGather)
            {
                string ext = Path.GetExtension(remoteUrl.ToLower());
                if (string.IsNullOrWhiteSpace(ext)) ext = ".jpg";
                byte[] imgByte = null;

                if (sourceImgIsLocal)
                {
                    imgByte = ImageHelper.ImageClass.GetImageData(remoteUrl);
                }
                else
                {
                    imgByte = Common.MySpider.PicGather(remoteUrl);
                }

                if(imgByte!=null && imgByte.Length>0){
                    string dir = OssHelper.GetBaseDir(websiteOwner);
                    string key = string.Format("{0}/{1}/{2}/{3}/{4}{5}",
                        dir,"ImageMapping","image",DateTime.Now.ToString("yyyyMMdd"),Guid.NewGuid().ToString("N").ToUpper(),ext);

                    model.LocalPath = new BLLUploadOtherServer().uploadFromByte(imgByte, ext);//OssHelper.UploadFileFromByte(bucket, key, imgByte, ext);

                    if (isNew)
                    {
                        Add(model);
                    }
                    else
                    {
                        Update(model, string.Format("LocalPath='{0}'", model.LocalPath), string.Format("RemoteUrl='{0}' and InOss=1", model.RemoteUrl));
                    }
                }
            }
            return model.LocalPath;
        }
        public class ActivityItemList
        {

            public List<ActivityItem> ItemList { get; set; }

        }
        public class ActivityItem
        {
            /// <summary>
            /// 选项ID
            /// </summary>
            public string ItemId { get; set; }
            /// <summary>
            /// 选项名称
            /// </summary>
            public string ItemName { get; set; }
            /// <summary>
            /// 选项金额
            /// </summary>
            public decimal ItemAmount { get; set; }

            /// <summary>
            /// 原价
            /// </summary>
            public decimal ItemPrice { get; set; }
            /// <summary>
            /// 选项描述
            /// </summary>
            public string ItemDesc { get; set; }
        }

        /// <summary>
        /// 添加聚活动
        /// </summary>
        /// <param name="context">内容传输</param>
        /// <param name="addUserId">添加用户</param>
        /// <param name="websiteOwner">站点所有者(子站点)</param>
        /// <param name="Status">返回状态码</param>
        /// <param name="Msg">返回信息</param>
        /// <param name="ExObj"></param>
        /// <param name="ExStr"></param>
        /// <returns></returns>
        public bool AddJuActivity(HttpContext context, string addUserId, string websiteOwner, ref int Status, ref string Msg, ref object ExObj, ref string ExStr, List<JuActivityFiles> files)
        {

            string isReplaceN = context.Request["isReplaceN"];

            BLLJIMP.Model.JuActivityInfo model = new JuActivityInfo();
            model.UserID = addUserId;
            model.JuActivityID = int.Parse(GetGUID(BLLJIMP.TransacType.ActivityAdd));
            model.ActivityName = Common.StringHelper.GetReplaceStr(context.Request["ActivityName"]);
            if (model.ActivityName.Length > 150)
            {
                Status = 0;
                Msg = "标题字数过长!";
                return false;
            }
            string activityStartDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityStartDate))
            {
                model.ActivityStartDate = DateTime.Parse(activityStartDate);
            }
            string activityEndDate = context.Request["ActivityEndDate"];
            if (!string.IsNullOrEmpty(activityEndDate))
            {
                model.ActivityEndDate = DateTime.Parse(activityEndDate);
            }

            string IsHideRecommend = context.Request["IsHideRecommend"];
            if (!string.IsNullOrEmpty(IsHideRecommend))
            {
                model.IsHideRecommend = IsHideRecommend;
            }
            string relationArticle = context.Request["RelationArticle"];
            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];
            model.IsSignUpJubit = int.Parse(context.Request["IsSignUpJubit"]);
            model.SignUpActivityID = context.Request["SignUpActivityID"];
            model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            //model.Sort = Convert.ToInt32(context.Request["Sort"]);
            model.IsFee = Convert.ToInt32(context.Request["IsFee"]);
            model.ArticleTemplate = Convert.ToInt32(context.Request["ArticleTemplate"]);
            model.TopImgPath = context.Request["TopImgPath"];

            model.ActivityLecturer = context.Request["ActivityLecturer"];
            model.ArticleTypeEx1 = context.Request["ArticleTypeEx1"];
            model.ActivitySignuptUrl = context.Request["ActivitySinupUrl"];
            model.ActivityIntegral = int.Parse(string.IsNullOrEmpty(context.Request["ActivityIntegral"]) ? "0" : context.Request["ActivityIntegral"]);
            model.IsByWebsiteContent = Convert.ToInt32(context.Request["IsByWebsiteContent"]);
            model.Sort = 1;
            model.LastUpdateDate = DateTime.Now;
            model.Summary = Common.StringHelper.GetReplaceStr(context.Request["Summary"]);
           
            if (isReplaceN == "1")
                model.ActivityDescription = model.ActivityDescription.Replace("\n", "<br />");

            //如果 Summary为空截200个字内容
            if (string.IsNullOrWhiteSpace(model.Summary))
            {
                string summary = MySpider.MyRegex.RemoveHTMLTags(model.ActivityDescription);
                if (summary.Length > 200) { 
                    model.Summary = summary.Substring(0, 200) + "..."; 
                }
                else
                {
                    model.Summary = summary;
                }
            }


            model.CreateDate = DateTime.Now;
            model.IsSpread = Convert.ToInt32(context.Request["IsSpread"]);
            model.ArticleType = context.Request["ArticleType"];
            model.ActivityNoticeKeFuId = context.Request["ActivityNoticeKeFuId"];
            model.CategoryId = context.Request["CategoryId"];

            //没选择类型时判断时评时间设置类型
            if (string.IsNullOrWhiteSpace(model.CategoryId) && model.ArticleType.ToLower() == "comment")
            {
                int nHour = DateTime.Now.Hour;
                int tCount = 0;
                List<ArticleCategory> listCate = new BLLArticleCategory().GetCateList(out tCount, "Comment", 0, "");
                foreach (var nCate in listCate)
                {
                    if (!string.IsNullOrWhiteSpace(nCate.Summary) && nCate.Summary.Split('-').Count() == 2)
                    {
                        string[] sl = nCate.Summary.Split('-');
                        int s = 0;
                        int e = 0;
                        int.TryParse(sl[0], out s);
                        int.TryParse(sl[1], out e);
                        if (nHour >= s && nHour < e)
                        {
                            model.CategoryId = nCate.AutoID.ToString();
                            break;
                        }
                    }
                }
            }
            model.IsShowPersonnelList = int.Parse(string.IsNullOrEmpty(context.Request["IsShowPersonnelList"]) ? "0" : context.Request["IsShowPersonnelList"]);
            model.ShowPersonnelListType = int.Parse(string.IsNullOrEmpty(context.Request["ShowPersonnelListType"]) ? "0" : context.Request["ShowPersonnelListType"]);
            model.MaxSignUpTotalCount = int.Parse(context.Request["MaxSignUpTotalCount"]);

            model.ProvinceCode = context.Request["ProvinceCode"];
            model.CityCode = context.Request["CityCode"];
            model.DistrictCode = context.Request["DistrictCode"];

            model.Tags = context.Request["Tags"];

            model.K1 = context.Request["K1"];
            model.K2 = context.Request["K2"];
            model.K3 = context.Request["K3"];
            model.K4 = context.Request["K4"];
            model.K5 = context.Request["K5"];
            model.K6 = context.Request["K6"];
            model.K7 = context.Request["K7"];
            model.K8 = context.Request["K8"];
            model.K9 = context.Request["K9"];
            model.K10 = context.Request["K10"];
            model.RootCateId = context.Request["RootCateId"];
            model.RedirectUrl = context.Request["RedirectUrl"];
            model.TabExTitle1 = context.Request["TabExTitle1"];
            model.TabExTitle2 = context.Request["TabExTitle2"];
            model.TabExTitle3 = context.Request["TabExTitle3"];
            model.TabExTitle4 = context.Request["TabExTitle4"];
            model.TabExContent1 = context.Request["TabExContent1"];
            model.TabExContent2 = context.Request["TabExContent2"];
            model.TabExContent3 = context.Request["TabExContent3"];
            model.TabExContent4 = context.Request["TabExContent4"];
            model.RelationArticles = relationArticle;
            if (!string.IsNullOrEmpty(model.RelationArticles))
            {
                model.RelationArticles = model.RelationArticles.Replace("'",null);
            }
            int haveComment = 0;
            int.TryParse(context.Request["HaveComment"], out haveComment);
            model.HaveComment = haveComment;
            model.AccessLevel = !string.IsNullOrEmpty(context.Request["AccessLevel"]) ? int.Parse(context.Request["AccessLevel"]) : 0;
            model.IsFee = !string.IsNullOrEmpty(context.Request["IsFee"]) ? int.Parse(context.Request["IsFee"]) : 0;
            model.VisibleArea = !string.IsNullOrEmpty(context.Request["VisibleArea"]) ? int.Parse(context.Request["VisibleArea"]) : 0;
            model.ShowCondition = !string.IsNullOrEmpty(context.Request["ShowCondition"]) ? int.Parse(context.Request["ShowCondition"]) : 0;
            model.VisibleContext = context.Request["VisibleContext"];
            model.RelationProducts = context.Request["RelationProduct"];
            if (model.IsFee == 1)//收费活动
            {
                var itemList = ZentCloud.Common.JSONHelper.JsonToModel<ActivityItemList>(context.Request["ItemListJson"]);
                if (itemList.ItemList.Count == 0)
                {
                    Status = 0;
                    Msg = "收费选项不允许为空!";
                    return false;
                }
                foreach (var item in itemList.ItemList)
                {
                    CrowdFundItem itemModel = new CrowdFundItem();
                    if (item.ItemId == "0")//新增
                    {
                        itemModel.WebsiteOwner = bllActivity.WebsiteOwner;
                        itemModel.Amount = item.ItemAmount;
                        itemModel.OriginalPrice = item.ItemPrice;
                        itemModel.ProductName = item.ItemName;
                        itemModel.ItemType = "Activity";
                        itemModel.CrowdFundID = model.JuActivityID;
                        itemModel.ItemId = int.Parse(GetGUID(BLLJIMP.TransacType.CommAdd));
                        itemModel.Description = item.ItemDesc;
                        bllActivity.Add(itemModel);

                    }

                }

            }

            List<string> tagList = null;
            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                tagList = model.Tags.Split(',').ToList();
            }

            SetJuActivityContentTags(model.JuActivityID, tagList);


            model.PV = int.Parse(string.IsNullOrEmpty(context.Request["PV"]) ? "0" : context.Request["PV"]);
            if ((model.ActivityStartDate != null) && (model.ActivityEndDate != null))
            {
                if (model.ActivityEndDate <= model.ActivityStartDate)
                {
                    Status = 0;
                    Msg = "活动结束时间需晚于开始时间!";
                    return false;


                }
            }

            //如果ArticleType类型是article，则IsSignUpJubit都为0;
            if (model.ArticleType == "article")
                model.IsSignUpJubit = 0;

            model.WebsiteOwner = websiteOwner;

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {
                if (model.IsSignUpJubit == 1)
                {
                    //自动创建报名活动
                    ActivityInfo signUpActivityModel = new ActivityInfo();
                    signUpActivityModel.ActivityID = GetGUID(TransacType.ActivityAdd);
                    signUpActivityModel.UserID = addUserId;
                    signUpActivityModel.ActivityName = model.ActivityName;
                    signUpActivityModel.ActivityDate = model.ActivityStartDate;
                    signUpActivityModel.ActivityAddress = model.ActivityAddress;
                    signUpActivityModel.ActivityWebsite = model.ActivityWebsite;
                    signUpActivityModel.ActivityStatus = 1;
                    signUpActivityModel.LimitCount = 100;
                    signUpActivityModel.ActivityDescription = string.Format("该任务为活动{0}自动创建", model.JuActivityID);
                    signUpActivityModel.WebsiteOwner = websiteOwner;

                    //设置自动生成的ID
                    model.SignUpActivityID = signUpActivityModel.ActivityID;

                    if (!Add(signUpActivityModel, tran))
                    {
                        tran.Rollback();
                        Status = 0;
                        Msg = "添加失败!";
                        return false;
                    }

                    //添加默认字段
                    //添加自定义字段
                    List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>();

                    string FieldNameListStr = context.Request["FieldNameList"];

                    if (!string.IsNullOrEmpty(FieldNameListStr))
                    {
                        List<string> FieldNameList = FieldNameListStr.Split(',').ToList();
                        if (FieldNameList.Count <= 60)
                        {
                            for (int i = 0; i < FieldNameList.Count; i++)
                            {
                                fieldData.Add(new ActivityFieldMappingInfo()
                                {
                                    ActivityID = model.SignUpActivityID,
                                    ExFieldIndex = (i + 1),
                                    FieldIsDefauld = 0,
                                    FieldType = 0,
                                    MappingName = FieldNameList[i]
                                });
                            }
                        }
                        else
                        {
                            Status = 0;
                            Msg = "最多新增60个报名字段!";
                            return false;
                        }
                    }
                    else
                    {
                        fieldData = new List<ActivityFieldMappingInfo>()
                        {
                            new ActivityFieldMappingInfo()
                            { 
                                ActivityID = model.SignUpActivityID, 
                                ExFieldIndex = 1, 
                                FieldIsDefauld = 0,
                                FieldType = 0,
                                FormatValiFunc = "email",
                                MappingName = "邮箱"
                            },
                            new ActivityFieldMappingInfo()
                            { 
                                ActivityID = model.SignUpActivityID, 
                                ExFieldIndex = 2, 
                                FieldIsDefauld = 0,
                                FieldType = 0,
                                MappingName = "公司"
                            },
                            new ActivityFieldMappingInfo()
                            { 
                                ActivityID = model.SignUpActivityID, 
                                ExFieldIndex = 3, 
                                FieldIsDefauld = 0,
                                FieldType = 0,
                                MappingName = "职位"
                            }
                        };
                    }
                    if (!AddList(fieldData))
                    {
                        tran.Rollback();

                        Status = 0;
                        Msg = "添加报名字段失败!";
                        return false;
                    };
                }

                //自动创建推广活动
                MonitorPlan monitorPlanModel = new MonitorPlan();
                monitorPlanModel.MonitorPlanID = int.Parse(GetGUID(TransacType.MonitorPlanID));
                monitorPlanModel.PlanName = model.ActivityName;
                monitorPlanModel.PlanStatus = "1";
                monitorPlanModel.UserID = addUserId;
                monitorPlanModel.InsertDate = DateTime.Now;
                monitorPlanModel.Remark = "自动创建的监测任务";

                model.MonitorPlanID = monitorPlanModel.MonitorPlanID;

                if (Add(monitorPlanModel, tran) && Add(model, tran))
                {
                    tran.Commit();

                    //添加附件关系
                    foreach (var item in files)
                    {
                        item.JuActivityID = model.JuActivityID;
                        Add(item);
                    }

                    Status = 1;
                    ExObj = model;
                    ExStr = model.JuActivityIDHex;//将16进制ID传回去
                    if (context.Request["ArticleType"] == "article")
                    {
                        bllLog.Add(BLLJIMP.Enums.EnumLogType.Article, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "增加文章[id=" + model.JuActivityID + "]");
                    }
                    else
                    {
                        bllLog.Add(BLLJIMP.Enums.EnumLogType.Activity, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "增加活动[id=" + model.JuActivityID + "]");
                    }
                    Msg = "添加成功!";

                }
                else
                {
                    tran.Rollback();
                    Status = 0;
                    Msg = "添加失败!";
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Status = 0;
                Msg = ex.Message;
            }

            //try
            //{
            //    //积分处理
            //    if (!string.IsNullOrWhiteSpace(model.RecommendCate))
            //    {
            //        Dictionary<string, int> HFScoreMatch = GetHFScoreMatch();
            //        int score = 0;
            //        if (HFScoreMatch.TryGetValue(model.RecommendCate, out score))
            //        {
            //            new BLLUser("").AddUserScore(model.UserID, (float)score, "发表" + model.RecommendCate, model.RecommendCate);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Msg += "积分添加异常:" + ex.Message;
            //}

            return Status == 1;
        }

        /// <summary>
        /// juActivity设置内容关联标签
        /// </summary>
        /// <param name="juActivityID"></param>
        /// <param name="tagList"></param>
        /// <returns></returns>
        public bool SetJuActivityContentTags(int juActivityID, List<string> tagList)
        {

            bool result = true;

            var type = CommonPlatform.Helper.EnumStringHelper.ToString(Enums.CommRelationType.JuActivityTag);

            //清除该JuActivityContent下的原有标签
            var deleteResult = Delete(
                    new Model.CommRelationInfo(),
                    string.Format(" MainId = '{0}' AND RelationType = '{1}' ", juActivityID, type)
                );

            if (tagList != null && tagList.Count > 0)
            {
                List<CommRelationInfo> dataList = new List<CommRelationInfo>();

                foreach (var item in tagList)
                {
                    dataList.Add(new CommRelationInfo()
                    {
                        MainId = juActivityID.ToString(),
                        RelationId = item,
                        RelationTime = DateTime.Now,
                        RelationType = type
                    });
                }

                result = AddList<CommRelationInfo>(dataList);
            }

            return result;
        }

        /// <summary>
        /// 编辑聚活动
        /// </summary>
        /// <param name="context"></param>
        /// <param name="addUserId"></param>
        /// <param name="websiteOwner">站点所有者（已遗弃，防止管理员在其他站点修改别人网站导致数据混乱）</param>
        /// <param name="Status"></param>
        /// <param name="Msg"></param>
        /// <param name="ExObj"></param>
        /// <param name="ExStr"></param>
        /// <returns></returns>
        public bool EditJuActivity(HttpContext context, string addUserId, string websiteOwner, ref int Status, ref string Msg, ref object ExObj, ref string ExStr, List<JuActivityFiles> files, string noDeleteFileIds)
        {
            int juActivityID = Convert.ToInt32(context.Request["JuActivityID"]);

            BLLJIMP.Model.JuActivityInfo model = Get<BLLJIMP.Model.JuActivityInfo>("JuActivityID = " + juActivityID.ToString());

            UserInfo userInfo = new BLLUser("").GetUserInfo(addUserId);

            //if (userInfo != null)
            //{
            //    //if (userInfo.UserType != 1)
            //    //{
            //    model = Get<BLLJIMP.Model.JuActivityInfo>(string.Format("JuActivityID = {0}  AND UserID = '{1}' ", juActivityID.ToString(), userInfo.UserID));
            //    //}
            //}
            //else
            //{
            //    model = Get<BLLJIMP.Model.JuActivityInfo>(string.Format("JuActivityID = {0} ", juActivityID.ToString()));
            //}

            if (model == null)
            {
                Status = 0;
                Msg = "活动不存在";
                return false;
            }
            if (!model.WebsiteOwner.Equals(WebsiteOwner))
            {
                Status = 0;
                Msg = "无权修改";
                return false;
            }



            model.ActivityName = Common.StringHelper.GetReplaceStr(context.Request["ActivityName"]);
            if (model.ActivityName.Length > 150)
            {
                Status = 0;
                Msg = "标题字数过长!";
                return false;
            }
            string activityStartDate = context.Request["ActivityStartDate"];
            if (!string.IsNullOrEmpty(activityStartDate))
            {
                model.ActivityStartDate = DateTime.Parse(activityStartDate);
            }
            string activityEndDate = context.Request["ActivityEndDate"];
            if (!string.IsNullOrEmpty(activityEndDate))
            {
                model.ActivityEndDate = DateTime.Parse(activityEndDate);
            }
            else
            {
                model.ActivityEndDate = new DateTime(1970, 1, 1);
            }

            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];

            model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            //model.Sort = Convert.ToInt32(context.Request["Sort"]);
            model.IsFee = Convert.ToInt32(context.Request["IsFee"]);
            model.ArticleTemplate = Convert.ToInt32(context.Request["ArticleTemplate"]);
            model.ActivityLecturer = context.Request["ActivityLecturer"];
            model.IsByWebsiteContent = Convert.ToInt32(context.Request["IsByWebsiteContent"]);
            model.ActivitySignuptUrl = context.Request["ActivitySinupUrl"];
            model.TopImgPath = context.Request["TopImgPath"];
            model.ActivityIntegral = int.Parse(string.IsNullOrEmpty(context.Request["ActivityIntegral"]) ? "0" : context.Request["ActivityIntegral"]);
            //model.SignUpActivityID = context.Request["SignUpActivityID"];活动ID默认创建，不给编辑了
            model.IsSpread = Convert.ToInt32(context.Request["IsSpread"]);
            bool isAddSignUpplan = false;
            model.ArticleType = context.Request["ArticleType"];
            model.ArticleTypeEx1 = context.Request["ArticleTypeEx1"];
            model.LastUpdateDate = DateTime.Now;
            model.Summary = Common.StringHelper.GetReplaceStr(context.Request["Summary"]);

            //如果 Summary为空截200个字内容
            if (string.IsNullOrWhiteSpace(model.Summary))
            {
                string summary = MySpider.MyRegex.RemoveHTMLTags(model.ActivityDescription);
                if (summary.Length > 200) { 
                    model.Summary = summary.Substring(0, 200) + "...";
                }
                else
                {
                    model.Summary = summary;
                }
            }

            string IsHideRecommend = context.Request["IsHideRecommend"];
            if (!string.IsNullOrEmpty(IsHideRecommend))
            {
                model.IsHideRecommend = IsHideRecommend;
            }
            model.ActivityNoticeKeFuId = context.Request["ActivityNoticeKeFuId"];
            model.CategoryId = context.Request["CategoryId"];
            model.IsShowPersonnelList = int.Parse(string.IsNullOrEmpty(context.Request["IsShowPersonnelList"]) ? "0" : context.Request["IsShowPersonnelList"]);
            model.ShowPersonnelListType = int.Parse(string.IsNullOrEmpty(context.Request["ShowPersonnelListType"]) ? "0" : context.Request["ShowPersonnelListType"]);
            model.MaxSignUpTotalCount = int.Parse(context.Request["MaxSignUpTotalCount"]);


            model.ProvinceCode = context.Request["ProvinceCode"];
            model.CityCode = context.Request["CityCode"];
            model.DistrictCode = context.Request["DistrictCode"];

            model.Tags = context.Request["Tags"];

            model.K1 = context.Request["K1"];
            model.K2 = context.Request["K2"];
            model.K3 = context.Request["K3"];
            model.K4 = context.Request["K4"];
            model.K5 = context.Request["K5"];
            model.K6 = context.Request["K6"];
            model.K7 = context.Request["K7"];
            model.K8 = context.Request["K8"];
            model.K9 = context.Request["K9"];
            model.K10 = context.Request["K10"];
            model.RootCateId = context.Request["RootCateId"];
            model.RedirectUrl = context.Request["RedirectUrl"];
            model.RelationArticles = context.Request["RelationArticle"];
            model.TabExTitle1 = context.Request["TabExTitle1"];
            model.TabExTitle2 = context.Request["TabExTitle2"];
            model.TabExTitle3 = context.Request["TabExTitle3"];
            model.TabExTitle4 = context.Request["TabExTitle4"];
            model.TabExContent1 = context.Request["TabExContent1"];
            model.TabExContent2 = context.Request["TabExContent2"];
            model.TabExContent3 = context.Request["TabExContent3"];
            model.TabExContent4 = context.Request["TabExContent4"];

            if (!string.IsNullOrEmpty(model.RelationArticles))
            {
                model.RelationArticles = model.RelationArticles.Replace("'", null);
            }
            int haveComment = 0;
            int.TryParse(context.Request["HaveComment"], out haveComment);
            model.HaveComment = haveComment;

            model.AccessLevel = !string.IsNullOrEmpty(context.Request["AccessLevel"]) ? int.Parse(context.Request["AccessLevel"]) : 0;
            model.IsFee = !string.IsNullOrEmpty(context.Request["IsFee"]) ? int.Parse(context.Request["IsFee"]) : 0;
            model.VisibleArea = !string.IsNullOrEmpty(context.Request["VisibleArea"]) ? int.Parse(context.Request["VisibleArea"]) : 0;
            model.ShowCondition = !string.IsNullOrEmpty(context.Request["ShowCondition"]) ? int.Parse(context.Request["ShowCondition"]) : 0;
            model.VisibleContext = context.Request["VisibleContext"];
            model.RelationProducts = context.Request["RelationProduct"];
            if (model.IsFee == 1)//收费活动
            {

                var itemList = ZentCloud.Common.JSONHelper.JsonToModel<ActivityItemList>(context.Request["ItemListJson"]);

                #region 删除旧选项
                var oldItemList = bllActivity.GetList<CrowdFundItem>(string.Format(" CrowdFundID='{0}'", model.JuActivityID));//旧的选项
                if (itemList.ItemList.Where(p => p.ItemId != "0").Count() != oldItemList.Count)//有需要删除的选项
                {
                    var delItems = from req in oldItemList
                                   where !(from old in itemList.ItemList
                                           select old.ItemId).Contains(req.ItemId.ToString())
                                   select req;
                    if (delItems.Count() > 0)
                    {
                        string delItemIds = string.Format(" ItemId in ({0})", string.Join(",", delItems.SelectMany(p => new List<int>() { (int)p.ItemId })));
                        if (bllActivity.Delete(new CrowdFundItem(), delItemIds) != delItems.Count())
                        {


                        }

                    }


                }
                #endregion
                foreach (var item in itemList.ItemList)
                {
                    CrowdFundItem itemModel = new CrowdFundItem();
                    if (item.ItemId == "0")//新增
                    {
                        itemModel.WebsiteOwner = bllActivity.WebsiteOwner;
                        itemModel.Amount = item.ItemAmount;
                        itemModel.OriginalPrice = item.ItemPrice;
                        itemModel.ProductName = item.ItemName;
                        itemModel.ItemType = "Activity";
                        itemModel.CrowdFundID = model.JuActivityID;
                        itemModel.ItemId = int.Parse(GetGUID(BLLJIMP.TransacType.CommAdd));
                        itemModel.Description = item.ItemDesc;
                        bllActivity.Add(itemModel);

                    }
                    else//更新
                    {
                        itemModel = bllActivity.Get<CrowdFundItem>(string.Format(" ItemId={0}", item.ItemId));
                        itemModel.Amount = item.ItemAmount;
                        itemModel.OriginalPrice = item.ItemPrice;
                        itemModel.ProductName = item.ItemName;
                        itemModel.Description = item.ItemDesc;
                        bllActivity.Update(itemModel);

                    }
                }
            }

            List<string> tagList = null;

            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                tagList = model.Tags.Split(',').ToList();
            }

            SetJuActivityContentTags(model.JuActivityID, tagList);

            model.PV = int.Parse(string.IsNullOrEmpty(context.Request["PV"]) ? "0" : context.Request["PV"]);
            if ((model.ActivityStartDate != null) && (model.ActivityEndDate != null))
            {
                if ((model.ActivityEndDate <= model.ActivityStartDate) && (model.ActivityEndDate != new DateTime(1970, 1, 1)))
                {
                    Status = 0;
                    Msg = "活动结束时间需晚于开始时间!";
                    return false;


                }
            }
            //如果ArticleType类型是article，则IsSignUpJubit都为0;
            if (model.ArticleType == "article")
                model.IsSignUpJubit = 0;
            else
            {
                //如果由其他状态编辑更改为自动报名状态，则重新自动创建任务
                int isSignUpJubit = Convert.ToInt32(context.Request["IsSignUpJubit"]);
                if (model.IsSignUpJubit != 1 && isSignUpJubit == 1)
                {
                    isAddSignUpplan = true;
                }

                model.IsSignUpJubit = isSignUpJubit;
            }

            Status = 0;
            Msg = "更新失败!";

            if (isAddSignUpplan)
            {
                ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                try
                {
                    ActivityInfo signUpActivityModel = CreateSignUpActivityModelByJuActivity(model, userInfo.UserID);
                    model.SignUpActivityID = signUpActivityModel.ActivityID;
                    signUpActivityModel.WebsiteOwner = websiteOwner;

                    //添加默认字段
                    List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>() {
                         new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 1, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            FormatValiFunc = "email",
                            MappingName = "邮箱"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 2, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "公司"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 3, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "职位"
                        }
                    };
                    if (!AddList(fieldData))
                    {
                        tran.Rollback();
                    }
                    else
                    {

                        if (Update(model, tran) && Add(signUpActivityModel, tran))
                        {
                            tran.Commit();

                            //删除已删除的文件
                            Delete(new JuActivityFiles(), string.Format(" AutoID Not In ({0}) AND JuActivityID={1}", noDeleteFileIds, model.JuActivityID.ToString()));
                            //添加新文件
                            foreach (var item in files)
                            {
                                item.JuActivityID = model.JuActivityID;
                                Add(item);
                            }

                            Status = 1;
                            Msg = "更新成功!";

                        }
                        else
                            tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Msg = ex.Message;
                }

            }
            else
            {

                if (this.Update(model))
                {
                    //删除已删除的文件
                    Delete(new JuActivityFiles(), string.Format(" AutoID Not In ({0}) AND JuActivityID={1}", noDeleteFileIds, model.JuActivityID.ToString()));
                    //添加新文件
                    foreach (var item in files)
                    {
                        item.JuActivityID = model.JuActivityID;
                        Add(item);
                    }

                    Status = 1;
                    Msg = "更新成功!";
                    if (model.ArticleType == "article")
                    {
                        bllLog.Add(BLLJIMP.Enums.EnumLogType.Article, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "编辑文章[id=" + model.JuActivityID + "]");
                    }
                    else
                    {
                        bllLog.Add(BLLJIMP.Enums.EnumLogType.Activity, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "编辑活动[id=" + model.JuActivityID + "]");
                    }
                }
            }

            return Status == 1;

        }

        /// <summary>
        /// 清除Html标签
        /// </summary>
        /// <param name="strHtml">原始内容</param>
        /// <param name="subLength">截取长度，默认不截取</param>
        /// <returns></returns>
        public string ClearHtmlTag(string strHtml, int subLength = 0)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                Regex reg = null;
                Match match = null;

                reg = new Regex(@"<\/?[^>]*>", RegexOptions.IgnoreCase);
                for (match = reg.Match(strHtml); match.Success; match = match.NextMatch())
                {
                    strHtml = strHtml.Replace(match.Groups[0].ToString(), null);
                }
                strHtml = strHtml.Replace("\n", null).Replace("\t", null).Replace("\r", null);
                if (strHtml.Contains("&nbsp;"))
                {
                    strHtml = strHtml.Replace("&nbsp;", null);
                }
                if (!subLength.Equals(0))
                {
                    if (strHtml.Length > subLength)
                    {
                        strHtml = string.Format("{0}...", strHtml.Substring(0, subLength));
                    }
                }
                return strHtml.Trim();

            }

            return "";


        }

        /// <summary>
        /// 获取用户模板源代码
        /// </summary>
        /// <param name="websiteOwer">网站所有者</param>
        /// <param name="templateType">模样类型</param>
        /// <returns></returns>
        public string GetTemplateSource(string websiteOwer, string templateType)
        {
            try
            {
                UserTemplate userTemplate = Get<UserTemplate>(string.Format("WebsiteOwner='{0}' And TemplateType='{1}'", websiteOwer, templateType));
                if (userTemplate == null)
                {
                    return "模板文件不存在";
                }

                string path = HttpContext.Current.Server.MapPath(userTemplate.TemplatePath);
                if (!System.IO.File.Exists(path))
                {
                    return "模板路径不存在";
                }
                else
                {
                    return Common.IOHelper.GetFileStr(path, Encoding.UTF8);
                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }



        }

        /// <summary>
        /// 增加PV
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool UpdatePVCount(int articleId)
        {

            var model = GetJuActivity(articleId);
            model.PV++;
            return Update(model);

        }
        /// <summary>
        /// 增加PV
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool UpdatePVCount(JuActivityInfo juActivityInfo)
        {

            juActivityInfo.PV++;
            return Update(juActivityInfo);

        }
        /// <summary>
        /// 更新微转发UV
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool UpdateUVCount(int articleId)
        {
            var model = GetJuActivity(articleId);
            int count = GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" MonitorPlanID={0} ", model.MonitorPlanID));
            model.UV = count;
            return Update(model);
        }
        /// <summary>
        /// 更新IP
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool UpdateIPCount(int articleId)
        {

            var model = GetJuActivity(articleId);
            //if (model.IP == null)
            //{
            //    model.IP = 0;
            //}
            int count = GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID = {0} ", model.MonitorPlanID));
            model.IP = count;
            return Update(model);


        }
        /// <summary>
        /// 更新IP
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool UpdateIPCount(JuActivityInfo juActivityInfo)
        {

            //var model = GetJuActivity(articleId);
            //if (model.IP == null)
            //{
            //    model.IP = 0;
            //}
            int count = GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID = {0} ", juActivityInfo.MonitorPlanID));
            juActivityInfo.IP = count;
            return Update(juActivityInfo);


        }

        public bool UpdateTotalShareCount(int articleId)
        {

            var model = GetJuActivity(articleId);

            int shareTotalCount = GetCount<MonitorEventDetailsInfo>("ShareTimestamp", string.Format(" MonitorPlanID = {0} and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ", model.MonitorPlanID));
            model.ShareTotalCount = shareTotalCount;
            return Update(model);


        }
        /// <summary>
        /// 更新分享人数
        /// </summary>
        /// <param name="juActivityInfo"></param>
        /// <returns></returns>
        public bool UpdateTotalShareCount(JuActivityInfo juActivityInfo)
        {

            int shareTotalCount = GetCount<MonitorEventDetailsInfo>("ShareTimestamp", string.Format(" MonitorPlanID = {0} and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ", juActivityInfo.MonitorPlanID));
            juActivityInfo.ShareTotalCount = shareTotalCount;
            return Update(juActivityInfo);


        }

        /// <summary>
        /// 整加单个列值数量
        /// </summary>
        /// <param name="col"></param>
        /// <param name="value"></param>
        /// <param name="jid"></param>
        /// <returns></returns>
        public bool PlusNumericalCol(string col, int jid, int value = 1)
        {
            var result = Update(new JuActivityInfo(), string.Format(" {0} = {0} + ({1}) ", col, value), string.Format(" JuActivityID = {0} ", jid)) > 0;

            return result;
        }

        /// <summary>
        /// 更新文章活动IPPV
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public bool UpDateIPPVShareCount(JuActivityInfo juActivityInfo)
        {

            return UpdateIPCount(juActivityInfo) && UpdatePVCount(juActivityInfo) && UpdateTotalShareCount(juActivityInfo);

        }

        /// <summary>
        /// 更新微转发PVUV
        /// </summary>
        /// <param name="juactivityInfo"></param>
        /// <returns></returns>
        public void UpdateActivityForwardPVUV(JuActivityInfo juactivityInfo)
        {
            int uv = GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" MonitorPlanID={0} ", juactivityInfo.MonitorPlanID));
            int pv = GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID={0} ", juactivityInfo.MonitorPlanID));
            juactivityInfo.UV = uv;
            
            Update(new JuActivityInfo(), string.Format(" UV={0}", uv), string.Format(" JuActivityID='{0}'", juactivityInfo.JuActivityID));//更新文章活动表UV
            Update(new ActivityForwardInfo(), string.Format(" PV={0},UV={1}", pv,uv), string.Format(" ActivityId='{0}'", juactivityInfo.JuActivityID));//更新转发表UV


        }



        /// <summary>
        /// 更新报名人数
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public bool UpSignUpTotalCount(int activityId)
        {
            JuActivityInfo act = GetJuActivity(activityId);
            bool result = false;
            if (act != null && !string.IsNullOrWhiteSpace(act.SignUpActivityID))
            {
                int count = GetCount<ActivityDataInfo>(string.Format(" ActivityID = {0} and IsDelete = 0 ", act.SignUpActivityID));
                result = Update(new JuActivityInfo(), string.Format(" SignUpCount = {0} ", count), string.Format(" JuActivityID = {0} ", act.JuActivityID)) > 0;
                //#region 更新转发表报名数量
                //MonitorLinkInfo linkModel = Get<MonitorLinkInfo>(string.Format(" MonitorPlanID={0} AND LinkName='{1}' ", monitorPlanID, spreadUserId));
                //int signupCount = GetCount<ActivityDataInfo>(string.Format("MonitorPlanID={0} And SpreadUserID='{1}' And IsDelete=0", int.Parse(monitorPlanID), spreadUserId));
                //if (linkModel != null)
                //{
                //    linkModel.ActivitySignUpCount = signupCount;
                //    result=Update(linkModel);
                //}
                //#endregion
            }
            return result;
        }

        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <param name="preId"></param>
        /// <returns></returns>
        public int GetJuActivityCount(string type, string cateId, string userId, bool showHide)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" IsDelete = 0");
            if (!showHide) sbSql.AppendFormat(" and IsHide=0 ");
            if (!string.IsNullOrWhiteSpace(type)) sbSql.AppendFormat("AND ArticleType='{0}'", type);
            if (!string.IsNullOrWhiteSpace(userId)) sbSql.AppendFormat("AND UserID='{0}'", userId);
            if (string.IsNullOrWhiteSpace(cateId))
            {
                sbSql.AppendFormat(" AND ArticleType='{0}'", type);
            }
            else if (cateId.Contains(","))
            {
                string cateIds = "'" + cateId.Replace(",", "','") + "'";
                sbSql.AppendFormat(" AND CategoryId IN ({0})", cateIds);
            }
            else
            {
                sbSql.AppendFormat(" AND CategoryId='{0}'", cateId);
            }

            int result = GetCount<JuActivityInfo>(sbSql.ToString());

            return result;
        }

        public bool PutArticle(JuActivityInfo article)
        {
            if (article.JuActivityID == 0)
            {
                article.JuActivityID = int.Parse(GetGUID(TransacType.CommAdd));
                return Add(article);
            }
            else
            {
                JuActivityInfo oldArticle = GetJuActivity(article.JuActivityID);
                oldArticle.ActivityName = article.ActivityName;
                oldArticle.Summary = article.Summary;
                oldArticle.ActivityDescription = article.ActivityDescription;
                oldArticle.IsFee = article.IsFee;
                oldArticle.IsHide = article.IsHide;
                oldArticle.PV = article.PV;
                oldArticle.Tags = article.Tags;
                oldArticle.ActivityWebsite = article.ActivityWebsite;
                oldArticle.ActivityAddress = article.ActivityAddress;
                oldArticle.ActivityIntegral = article.ActivityIntegral;
                oldArticle.CategoryId = article.CategoryId;
                oldArticle.ThumbnailsPath = article.ThumbnailsPath;
                oldArticle.ProvinceCode = article.ProvinceCode;
                return Update(oldArticle);
            }
        }


        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="juActivityID"></param>
        /// <returns></returns>
        public bool DelArticle(string juActivityID)
        {
            return Update(new JuActivityInfo(), " IsDelete=1 ", string.Format(" JuActivityID={0} ", juActivityID)) > 0;
        }

        /// <summary>
        /// 被举报的文章列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="showHide"></param>
        /// <returns></returns>
        public System.Data.DataSet GetIllegalContentList(int pageSize, int pageIndex, string keyword, string websiteOwner, bool showHide = true)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WITH A AS( ");
            strWhere.AppendFormat(" SELECT ROW_NUMBER() OVER (ORDER BY A.[AutoId] DESC) NUM ");
            strWhere.AppendFormat(" ,B.[UserID],A.[RelationId] ");
            strWhere.AppendFormat(" ,B.[Summary],B.[ActivityDescription],A.[RelationTime] ");
            strWhere.AppendFormat(" ,B.[CommentCount],B.[PV],B.[JuActivityID] ");
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_JuActivityInfo] B ON A.[MainId]=B.[JuActivityID] ");
            strWhere.AppendFormat("     AND B.[IsDelete]=0  ");
            if (!showHide) strWhere.AppendFormat("     AND B.[IsHide]=0  ");
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     AND (B.[ActivityDescription] like '%{0}%' OR B.[Summary] like '%{0}%')  ", keyword);
            strWhere.AppendFormat("     AND A.[RelationType]='ReportJuActivityIllegalContent' ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat("     AND [WebsiteOwner]='{0}' ", websiteOwner);
            strWhere.AppendFormat(" WHERE  A.[RelationType] = 'ReportJuActivityIllegalContent' ");
            strWhere.AppendFormat(" ) ");
            strWhere.AppendFormat(" SELECT * FROM A WHERE NUM BETWEEN ({1}-1)* {0}+1 AND {1}*{0}; ", pageSize, pageIndex);

            strWhere.AppendFormat(" SELECT COUNT(1)[TOTALCOUNT] ");
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_JuActivityInfo] B ON A.[MainId]=B.[JuActivityID] ");
            strWhere.AppendFormat("     AND B.[IsDelete]=0  ");
            if (!showHide) strWhere.AppendFormat("     AND B.[IsHide]=0  ");
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     AND (B.[ActivityDescription] like '%{0}%' OR B.[Summary] like '%{0}%')  ", keyword);
            strWhere.AppendFormat("     AND A.[RelationType]='ReportJuActivityIllegalContent' ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat("     AND [WebsiteOwner]='{0}' ", websiteOwner);
            strWhere.AppendFormat(" WHERE A.[RelationType] = 'ReportJuActivityIllegalContent' ");
            System.Data.DataSet ds = Query(strWhere.ToString());
            return ds;
        }
        /// <summary>
        /// 保存附件信息
        /// </summary>
        /// <param name="ArticleId"></param>
        /// <param name="fileInfos"></param>
        /// <param name="curUserId"></param>
        /// <returns></returns>
        public void PutArticleFiles(int ArticleId, List<string> fileInfos, string curUserId)
        {
            Delete(new JuActivityFiles(), string.Format("JuActivityID={0}", ArticleId));
            for (int i = 0; i < fileInfos.Count; i++)
            {
                List<string> lst = fileInfos[i].Split(':').ToList();
                if (lst.Count != 2) continue;
                JuActivityFiles file = new JuActivityFiles();
                file.AddDate = DateTime.Now;
                file.FileName = lst[0];
                file.FilePath = lst[1];
                file.JuActivityID = ArticleId;
                file.UserID = curUserId;
                Add(file);
            }
        }
        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="ArticleId"></param>
        /// <returns></returns>
        public List<JuActivityFiles> GetFiles(int ArticleId)
        {
            return GetList<JuActivityFiles>(string.Format("JuActivityID={0}", ArticleId));
        }

        /// <summary>
        /// 判断用户是否报过名
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActivityDataInfo GetUserSingup(int activityID, string userId)
        {
            var activity = GetJuActivity(activityID);

            var data = Get<ActivityDataInfo>(string.Format(" ActivityID = {0} AND UserId = '{1}' AND IsDelete = 0 ", activity.SignUpActivityID, userId));
            return data;
        }


        /// <summary>
        /// 附近用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="userType"></param>
        /// <param name="city"></param>
        /// <param name="gender"></param>
        /// <param name="tag"></param>
        /// <param name="keyword"></param>
        /// <param name="range"></param>
        /// <param name="sort"></param>
        /// <param name="tatol"></param>
        /// <returns></returns>
        public List<JuActivityInfo> GetRangeUserList(int pageSize, int pageIndex, string websiteOwner, string userId, string articleType
            , string categoryId, string city, string longitude, string latitude, string ex1, string ex2, string ex3, string ex4, string ex5
            , string ex6, string ex7, string ex8, string ex9, string ex10, string keyword, string tag, string sort, string start_time
            , string stop_time, out int tatol, int? range = 3, string status = "0,1", string juActivityIds = null
            , string signUpActivityIds = null, string colName = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" IsDelete=0 ");
            sbSql.AppendFormat(" AND [ActivityName] IS NOT NULL ");
            if (!string.IsNullOrWhiteSpace(start_time) && !string.IsNullOrWhiteSpace(stop_time))
            {
                sbSql.AppendFormat(" AND [ActivityStartDate] >'{0}' AND [ActivityStartDate] < '{1}' "
                    , DateTimeHelper.UnixTimestampToDateTime(Convert.ToInt64(start_time)).ToString("yyyy-MM-dd HH:mm:ss")
                    , DateTimeHelper.UnixTimestampToDateTime(Convert.ToInt64(stop_time)).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND [WebsiteOwner]='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(userId)) sbSql.AppendFormat(" AND UserID='{0}' ", userId);
            if (!string.IsNullOrWhiteSpace(articleType)) sbSql.AppendFormat(" AND ArticleType='{0}' ", articleType);
            if (!string.IsNullOrWhiteSpace(categoryId)) sbSql.AppendFormat(" AND CategoryId='{0}' ", categoryId);
            if (!string.IsNullOrWhiteSpace(city)) sbSql.AppendFormat(" AND City='{0}' ", city);
            if (!string.IsNullOrWhiteSpace(ex1)) sbSql.AppendFormat(" AND K1='{0}' ", ex1);
            if (!string.IsNullOrWhiteSpace(ex2)) sbSql.AppendFormat(" AND K2='{0}' ", ex2);
            if (!string.IsNullOrWhiteSpace(ex3)) sbSql.AppendFormat(" AND K3='{0}' ", ex3);
            if (!string.IsNullOrWhiteSpace(ex4)) sbSql.AppendFormat(" AND K4='{0}' ", ex4);
            if (!string.IsNullOrWhiteSpace(ex5)) sbSql.AppendFormat(" AND K5='{0}' ", ex5);
            if (!string.IsNullOrWhiteSpace(ex6)) sbSql.AppendFormat(" AND K6='{0}' ", ex6);
            if (!string.IsNullOrWhiteSpace(ex7)) sbSql.AppendFormat(" AND K7='{0}' ", ex7);
            if (!string.IsNullOrWhiteSpace(ex8)) sbSql.AppendFormat(" AND K8='{0}' ", ex8);
            if (!string.IsNullOrWhiteSpace(ex9)) sbSql.AppendFormat(" AND K9='{0}' ", ex9);
            if (!string.IsNullOrWhiteSpace(ex10)) sbSql.AppendFormat(" AND K10='{0}' ", ex10);
            if (!string.IsNullOrWhiteSpace(status)) sbSql.AppendFormat(" AND TStatus In ({0}) ", status);
            if (!string.IsNullOrWhiteSpace(juActivityIds)) sbSql.AppendFormat(" AND JuActivityID In ({0}) ", juActivityIds);
            if (!string.IsNullOrWhiteSpace(signUpActivityIds)) sbSql.AppendFormat(" AND SignUpActivityID In ({0}) ", signUpActivityIds);
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                sbSql.AppendFormat(" AND [UserLongitude] IS NOT NULL ");
                sbSql.AppendFormat(" AND dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude])<{2} "
                    , longitude, latitude, range);
            }

            if (!string.IsNullOrEmpty(tag))
            {
                string[] tagNameArray = tag.Split(',');
                sbSql.AppendFormat(" AND( ");
                for (int i = 0; i < tagNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tagNameArray[i]))
                    {
                        if (i > 0)
                        {
                            sbSql.AppendFormat(" OR Tags like '%{0}%' ", tagNameArray[i]);
                        }
                        else
                        {
                            sbSql.AppendFormat(" Tags like '%{0}%' ", tagNameArray[i]);
                        }

                    }
                }
                sbSql.AppendFormat(") ");
            }
            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" AND ( ActivityName like '{0}%' OR Summary like '{0}%' OR ActivityDescription like '{0}%' ) ", keyword);


            string order1 = "[CreditAcount] desc,";
            string order2 = "";
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                order2 = string.Format("dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude]) asc,", longitude, latitude);
            }
            string order = order1 + order2;
            switch (sort)
            {
                case "1":
                    order = order2 + order1;
                    break;
                case "2":
                    order = "ActivityStartDate asc," + order1;
                    break;
                case "3":
                    order = "ActivityStartDate desc," + order1;
                    break;
                case "4":
                    order = "CreateDate desc," + order1;
                    break;
                case "99":
                    order = "SignUpActivityID desc";
                    break;
            }
            order = order.TrimEnd(',');
            tatol = GetCount<JuActivityInfo>(sbSql.ToString());

            string fieldStr = "*,-1 [Distance]";
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                fieldStr = string.Format("*,dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude]) [Distance]", longitude, latitude);
            }
            return GetColList<JuActivityInfo>(pageSize, pageIndex, sbSql.ToString(), order
                , fieldStr);
        }
        /// <summary>
        /// 通过报名
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="passIds"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool PassSignUp(int activityId, string passIds, string websiteOwner, string noticeSignupPass)
        {
            JuActivityInfo juAct = GetJuActivity(activityId);
            if (juAct == null || string.IsNullOrWhiteSpace(juAct.SignUpActivityID))
            {
                throw new Exception("活动未找到或未报名");
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" ActivityID='{0}' ", juAct.SignUpActivityID);
            sbSql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND IsDelete=0 ");
            sbSql.AppendFormat(" AND Status=0 ");
            sbSql.AppendFormat(" AND UID In ({0}) ", passIds);



            List<ActivityDataInfo> list = GetList<ActivityDataInfo>(sbSql.ToString());

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            if (Update(new ActivityDataInfo(), string.Format(" Status={0} ", 1), sbSql.ToString(), tran) < 0)
            {
                tran.Rollback();
                return false;
            }

            StringBuilder sbSql1 = new StringBuilder();
            sbSql1.AppendFormat(" ActivityID='{0}' ", juAct.SignUpActivityID);
            sbSql1.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            sbSql1.AppendFormat(" AND IsDelete=0 ");
            sbSql1.AppendFormat(" AND Status=1 ");

            if (list.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(juAct.ActivityAddress) && juAct.K5 == "1")
                {
                    juAct.ActivityAddress = list[0].K1;
                }
            }

            //通过数量大于1 ，主表地址
            juAct.TStatus = 1;
            if (!Update(juAct, tran))
            {
                tran.Rollback();
                return false;
            }

            tran.Commit();

            BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
            if (noticeSignupPass == "1")
            {
                for (int i = 0; i < list.Count; i++)
                {
                    bllSystemNotice.SendSystemMessage("您的约会报名被通过", juAct.ActivityName, BLLJIMP.BLLSystemNotice.NoticeType.AppointmentNotice, BLLJIMP.BLLSystemNotice.SendType.Personal, list[i].UserId, juAct.JuActivityID.ToString());
                }
            }
            return true;
        }

        public List<ActivityDataInfo> GetRangeSignUpList(int pageSize, int pageIndex, string activityId, string websiteOwner
            , string longitude, string latitude, string status, string sort, out int total, string userId = null
            , string colName = null, string articleType = null, string categoryId = null)
        {
            StringBuilder whereSql = new StringBuilder();
            whereSql.AppendFormat(" WebsiteOwner='{0}' AND IsDelete=0 ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(activityId))
            {
                whereSql.AppendFormat(" AND ActivityID='{0}' ", activityId);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                whereSql.AppendFormat(" AND Status In ({0}) ", status);
            }
            if (!string.IsNullOrWhiteSpace(userId))
            {
                whereSql.AppendFormat(" AND UserId='{0}' ", userId);
            }
            if (!string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                whereSql.AppendFormat(" AND [UserLongitude] IS NOT NULL ");
            }
            if (!string.IsNullOrWhiteSpace(articleType))
            {
                whereSql.AppendFormat(" AND ArticleType='{0}' ", articleType);
            }
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                whereSql.AppendFormat(" AND CategoryId='{0}' ", categoryId);
            }
            total = GetCount<ActivityDataInfo>(whereSql.ToString());
            string order = " InsertDate DESC";
            if (sort == "1") order = " GuaranteeCreditAcount DESC";
            if (sort == "99") order = " ActivityID DESC,UID ASC ";

            string fieldStr = "*";
            if (!string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                fieldStr = string.Format("*,dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude]) [Distance]", longitude, latitude);
            }
            if (!string.IsNullOrWhiteSpace(colName))
            {
                fieldStr = colName;
            }


            return GetColList<ActivityDataInfo>(pageSize, pageIndex
                , whereSql.ToString()
                , order
                , fieldStr);
        }

        /// <summary>
        /// 返回未通过报名的信用金
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public bool ReturnGuaranteeCreditAcount(int activityId, string websiteOwner)
        {
            JuActivityInfo juAct = GetJuActivity(activityId);
            if (juAct == null || string.IsNullOrWhiteSpace(juAct.SignUpActivityID))
            {
                throw new Exception("活动未找到或未报名");
            }

            List<ActivityDataInfo> list = GetActivityDataListByUId(juAct.SignUpActivityID, websiteOwner, "0");
            if (list.Count == 0) return false;

            BLLUser bllUser = new BLLUser();
            foreach (var item in list)
            {
                //返还信用金
                if (bllUser.AddUserCreditAcountDetails(item.UserId, "ApplyReturn", websiteOwner, item.GuaranteeCreditAcount
                    , string.Format("【{0}】报名未通过返回担保信用金{1}", juAct.ActivityName, item.GuaranteeCreditAcount)))
                {
                    //修改返还状态
                    UpdateActivityDataStatus(item.ActivityID, item.UID, -2);
                }
            }
            return true;
        }
        /// <summary>
        /// 是否已报名
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="signUserId"></param>
        /// <returns></returns>
        public bool ExistsSignUp(int activityId, string websiteOwner, string signUserId)
        {
            JuActivityInfo juAct = GetJuActivity(activityId);
            if (juAct == null || string.IsNullOrWhiteSpace(juAct.SignUpActivityID))
            {
                throw new Exception("活动未找到或未报名");
            }
            ActivityDataInfo result = GetActivityDataByUserId(juAct.SignUpActivityID, null, signUserId, websiteOwner);
            return result == null ? false : true;

        }
        /// <summary>
        /// 当前信用排名 当天
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="creditAcount"></param>
        /// <returns></returns>
        public int GetTodayCreditAcountRank(string articleType, string categoryId, string websiteOwner, decimal creditAcount)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" IsDelete=0 ");
            if (!string.IsNullOrWhiteSpace(articleType)) sbSql.AppendFormat(" AND ArticleType='{0}' ", articleType);
            if (!string.IsNullOrWhiteSpace(categoryId)) sbSql.AppendFormat(" AND CategoryId='{0}' ", categoryId);
            sbSql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" AND CreditAcount>{0} ", creditAcount);
            sbSql.AppendFormat(" AND CONVERT(VARCHAR(10),CreateDate,120)= CONVERT(VARCHAR(10),GETDATE(),120) ");
            int Count = GetCount<JuActivityInfo>(sbSql.ToString());
            return Count + 1;
        }
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="juActivityId"></param>
        /// <param name="uId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="pubUserId"></param>
        /// <returns></returns>
        public bool SignIn(int juActivityId, int uId, string websiteOwner, UserInfo pubUser)
        {
            JuActivityInfo juAct = GetJuActivity(juActivityId);
            if (juAct == null || string.IsNullOrWhiteSpace(juAct.SignUpActivityID))
            {
                throw new Exception("活动未找到或未报名");
            }
            if (juAct.UserID != pubUser.UserID)
            {
                throw new Exception("您不是活动发布人");
            }

            ActivityDataInfo result = GetActivityDataByUserId(juAct.SignUpActivityID, uId, null, websiteOwner);
            if (result == null)
            {
                throw new Exception("未找到报名信息");
            }
            else if (result.Status == 2)
            {
                throw new Exception("已签到");
            }
            long endTimestamp = 0;
            long.TryParse(result.K60, out endTimestamp);
            if (DateTimeHelper.UnixTimestampToDateTime(endTimestamp) < DateTime.Now)
            {
                throw new Exception("二维码已过期");
            }
            result.Status = 2;

            int nowSignCount = 0;
            List<ActivityDataInfo> nowSignData = GetRangeSignUpList(0, 0, juAct.SignUpActivityID, websiteOwner
                , null, null, "1", "99", out nowSignCount);
            if (nowSignCount == 1)
            {
                juAct.TStatus = 2;
            }
            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            if (Update(result, tran) && Update(juAct, tran))
            {
                tran.Commit();
                BLLUser bllUser = new BLLUser();

                //签到记录
                WXSignInInfo signInInfo = new WXSignInInfo();
                signInInfo.SignInUserID = pubUser.UserID;
                signInInfo.Name = result.Name;
                signInInfo.Phone = result.Phone;
                signInInfo.JuActivityID = juAct.JuActivityID;
                signInInfo.SignInOpenID = pubUser.WXOpenId;
                signInInfo.SignInTime = DateTime.Now;
                Add(signInInfo);

                //返还信用金给报名未通过的人
                ReturnGuaranteeCreditAcount(juAct.JuActivityID, websiteOwner);

                //返还参与人信用金
                if (bllUser.AddUserCreditAcountDetails(result.UserId, "SignReturn", websiteOwner, result.GuaranteeCreditAcount
                    , string.Format("【{0}】签到返还信用金{1}", juAct.ActivityName, result.GuaranteeCreditAcount)))
                {
                    //修改返还状态
                    UpdateActivityDataStatus(result.ActivityID, result.UID, 3);
                }

                //返还发布人信用金
                bllUser.AddUserCreditAcountDetails(juAct.UserID, "SignReturnPublisher", websiteOwner, juAct.CreditAcount
                    , string.Format("【{0}】签到返还发布者信用{1}", juAct.ActivityName, juAct.CreditAcount));

                return true;
            }
            else
            {
                tran.Rollback();
                throw new Exception("签到出错");
            }
        }
        /// <summary>
        /// 修改报名状态
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="uId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateActivityDataStatus(string activityId, int uId, int status)
        {
            return Update(new ActivityDataInfo(), string.Format("Status={0}", status)
                , string.Format(" ActivityID='{0}' AND UID={1} ", activityId, uId)) > 0;
        }

        public List<ActivityDataInfo> GetActivityDataListByUId(string activityId, string websiteOwner
            , string status = null, string uIds = null, string nUIds = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" ActivityID='{0}' ", activityId);
            sbSql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(status)) sbSql.AppendFormat(" AND Status In ({0}) ", status);
            if (!string.IsNullOrWhiteSpace(uIds)) sbSql.AppendFormat(" AND UID In ({0}) ", uIds);
            if (!string.IsNullOrWhiteSpace(nUIds)) sbSql.AppendFormat(" AND UID Not In ({0}) ", nUIds);
            sbSql.AppendFormat(" AND IsDelete=0 ");
            return GetList<ActivityDataInfo>(sbSql.ToString());
        }
        public ActivityDataInfo GetActivityDataByUserId(string activityId, int? uid, string userId, string websiteOwner, string status = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" ActivityID='{0}' ", activityId);
            sbSql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            if (uid.HasValue) sbSql.AppendFormat(" AND UID={0} ", uid.Value);
            if (!string.IsNullOrWhiteSpace(userId)) sbSql.AppendFormat(" AND UserId='{0}' ", userId);
            if (!string.IsNullOrWhiteSpace(status)) sbSql.AppendFormat(" AND Status In ({0}) ", status);
            sbSql.AppendFormat(" AND IsDelete=0 ");
            return Get<ActivityDataInfo>(sbSql.ToString());
        }
        /// <summary>
        /// 获取签到状态
        /// </summary>
        /// <param name="juActivityId"></param>
        /// <returns></returns>
        public int GetSignStatus(int juActivityId, string userId, string websiteOwner)
        {
            JuActivityInfo juAct = GetJuActivity(juActivityId, true, websiteOwner);
            if (juAct == null || string.IsNullOrWhiteSpace(juAct.SignUpActivityID)) throw new Exception("活动未找到");

            ActivityDataInfo actData = GetActivityDataByUserId(juAct.SignUpActivityID, null, userId, websiteOwner);
            if (actData == null) throw new Exception("报名未找到");

            return actData.Status;
        }
        /// <summary>
        /// 检查约会
        /// </summary>
        public void CheckEndAppointment()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" IsDelete=0 ");
            sbSql.AppendFormat(" AND [ActivityEndDate] < '{0}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sbSql.AppendFormat(" AND [TStatus]=0 ");
            sbSql.AppendFormat(" AND [ArticleType]='{0}' ", "Appointment");
            List<JuActivityInfo> listJuAct = GetList<JuActivityInfo>(sbSql.ToString());
            if (listJuAct.Count == 0) return;
            BLLUser bllUser = new BLLUser();
            foreach (JuActivityInfo itemJuAct in listJuAct)
            {
                int tempSignUpCount = 0;
                List<ActivityDataInfo> listSignUp = GetRangeSignUpList(0, 0, itemJuAct.SignUpActivityID, null, null, null, null, "99", out tempSignUpCount);
                if (tempSignUpCount == 0)
                {
                    //返还发布人信用金
                    bllUser.AddUserCreditAcountDetails(itemJuAct.UserID, "ReturnPublisher", itemJuAct.WebsiteOwner, itemJuAct.CreditAcount
                        , string.Format("【{0}】无人报名返还发布者信用{1}", itemJuAct.ActivityName, itemJuAct.CreditAcount));
                    itemJuAct.TStatus = 2;
                    bllUser.Update(itemJuAct);
                }
                else
                {
                    //返还信用金给报名未通过的人
                    List<ActivityDataInfo> listNoPass = listSignUp.Where(p => p.Status == 0).ToList();
                    foreach (var itemNoPass in listNoPass)
                    {
                        //返还信用金
                        if (bllUser.AddUserCreditAcountDetails(itemNoPass.UserId, "ApplyReturn", itemJuAct.WebsiteOwner, itemNoPass.GuaranteeCreditAcount
                            , string.Format("【{0}】报名未通过返回担保信用金{1}", itemJuAct.ActivityName, itemNoPass.GuaranteeCreditAcount)))
                        {
                            //修改返还状态
                            UpdateActivityDataStatus(itemNoPass.ActivityID, itemNoPass.UID, -2);
                        }
                    }
                    //返还信用金给报名通过的人 (如果存在，则记录违约)
                    List<ActivityDataInfo> listPass = listSignUp.Where(p => p.Status == 1).ToList();
                    foreach (var itemPass in listPass)
                    {
                        //修改返还状态
                        UpdateActivityDataStatus(itemPass.ActivityID, itemPass.UID, -3);
                    }
                    //完成签到的人
                    List<ActivityDataInfo> listEnd = listSignUp.Where(p => p.Status > 1).ToList();
                    itemJuAct.TStatus = listEnd.Count == 0 ? -1 : 2;
                    bllUser.Update(itemJuAct);
                }
            }
        }


        /// <summary>
        /// 查询服务网点列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public List<JuActivityInfo> GetOutletsList(int rows, int page, string cateId, string tags, string keyword, bool showHide, string colName, out int total
            , string longitude, string latitude, int? range, string sort, string websiteOwner, string k1, string k4,string k5,string city)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbsql.AppendFormat(" And ArticleType='{0}' ", "Outlets");
            if (!showHide) sbsql.Append(" AND IsHide = 0 ");
            sbsql.AppendFormat(" And IsDelete={0} ", 0);
            if (!string.IsNullOrWhiteSpace(cateId)) sbsql.AppendFormat(" And CategoryId ='{0}' ", cateId);
            if (!string.IsNullOrWhiteSpace(k1)) sbsql.AppendFormat(" And K1 ='{0}' ", k1);
            if (!string.IsNullOrWhiteSpace(k4)) sbsql.AppendFormat(" And K4 ='{0}' ", k4);
            if (!string.IsNullOrWhiteSpace(k5)) sbsql.AppendFormat(" And (K5 !='' And K5 IS NOT NULL) ", k5);
            if (!string.IsNullOrWhiteSpace(city)) sbsql.AppendFormat(" And City='{0}' ", city);
            if (!string.IsNullOrWhiteSpace(tags))
            {
                var tagArr = tags.Split(',').ToList();
                sbsql.Append("AND ( ");
                for (int i = 0; i < tagArr.Count; i++)
                {
                    if (i > 0) sbsql.Append(" OR ");
                    sbsql.AppendFormat(" Tags LIKE '%{0}%' ", tagArr[i]);
                }
                sbsql.Append(" ) ");
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbsql.Append("AND ( ");
                sbsql.AppendFormat(" ActivityName Like '%{0}%' ", keyword);
                sbsql.AppendFormat(" OR ActivityAddress Like '%{0}%' ", keyword);
                sbsql.AppendFormat(" OR ServerTimeMsg Like '%{0}%' ", keyword);
                sbsql.AppendFormat(" OR ServicesMsg Like '%{0}%' ", keyword);
                sbsql.AppendFormat(" OR K4 Like '%{0}%' ", keyword);
                sbsql.Append(" ) ");
            }
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                sbsql.AppendFormat(" AND [UserLongitude] IS NOT NULL ");
                sbsql.AppendFormat(" AND dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude])<{2} "
                    , longitude, latitude, range);
            }

            string order1 = "ISNULL(Sort,0) DESC, [JuActivityID] desc,";
            string order2 = "";
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                order2 = string.Format("dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude]) asc,", longitude, latitude);
            }
            string order = order1 + order2;
            switch (sort)
            {
                case "range":
                    order = order2 + order1;
                    break;
                default:
                    break;
            }
            order = order.TrimEnd(',');


            total = GetCount<JuActivityInfo>(sbsql.ToString());

            if (string.IsNullOrWhiteSpace(colName))
            {
                return GetLit<JuActivityInfo>(rows, page, sbsql.ToString(), order);
            }
            else
            {
                string fieldStr = colName + ",-1 [Distance]";
                if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
                {
                    fieldStr = string.Format("{0},dbo.fnGetDistance({1},{2},[UserLongitude],[UserLatitude]) [Distance]", colName, longitude, latitude);
                }
                return GetColList<JuActivityInfo>(rows, page, sbsql.ToString(), order, fieldStr);
            }
        }

        /// <summary>
        /// 查询政策列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="cateId"></param>
        /// <param name="tags"></param>
        /// <param name="keyword"></param>
        /// <param name="showHide"></param>
        /// <param name="colName"></param>
        /// <param name="total"></param>
        /// <param name="sort"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="k1"></param>
        /// <param name="policy_object">政策对象</param>
        /// <param name="policy_level">政策级别</param>
        /// <param name="domicile_place">户籍所在地</param>
        /// <param name="sex">性别</param>
        /// <param name="age">年龄</param>
        /// <param name="education">学历</param>
        /// <param name="graduation_year">毕业年限</param>
        /// <param name="employment_status">就业状态</param>
        /// <param name="current_job_life">目前岗位工作年限</param>
        /// <param name="unemployment_period">失业期限</param>
        /// <param name="company_type">单位类型</param>
        /// <param name="registered_capital">注册资金</param>
        /// <param name="personnel_size">人员规模</param>
        /// <param name="company_size">单位规模</param>
        /// <param name="industry">所属行业</param>
        /// <returns></returns>
        public List<JuActivityInfo> GetPolicyList(int rows, int page, string cateId, string tags, string keyword, bool showHide, string colName, out int total,
            string sort, string websiteOwner, string k1, string policy_object, string policy_level, string domicile_place, string sex, string age, string education,
            string graduation_year, string employment_status, string current_job_life, string unemployment_period, string company_type, string registered_capital,
            string personnel_size, string company_size, string industry)
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbsql.AppendFormat(" And ArticleType='{0}' ", "Policy");
            sbsql.AppendFormat(" And IsDelete={0} ", 0);
            if (!showHide) sbsql.Append(" AND IsHide = 0 ");
            if (!string.IsNullOrWhiteSpace(cateId)) sbsql.AppendFormat(" And CategoryId ='{0}' ", cateId);
            if (!string.IsNullOrWhiteSpace(k1)) sbsql.AppendFormat(" And K1 ='{0}' ", k1);
            if (!string.IsNullOrWhiteSpace(policy_object)) sbsql.AppendFormat(" And (K2 ='{0}' OR K2='无要求' OR IsNull(K2,'')='') ", policy_object);
            if (!string.IsNullOrWhiteSpace(policy_level)) sbsql.AppendFormat(" And (K6 ='{0}' OR K6='无要求' OR IsNull(K6,'')='') ", policy_level);
            if (!string.IsNullOrWhiteSpace(domicile_place)) sbsql.AppendFormat(" And (K7 Like '%{0}%' OR K7 Like '%无要求%' OR IsNull(K7,'')='')  ", domicile_place);
            if (!string.IsNullOrWhiteSpace(sex)) sbsql.AppendFormat(" And (K8 = '{0}' OR K8='无要求' OR IsNull(K8,'')='') ", sex);
            if (!string.IsNullOrWhiteSpace(age))
            {
                if (sex == "男") sbsql.AppendFormat(" And Convert(int,IsNull(K9,'0')) <= {0} And Convert(int,IsNull(K10,'9999')) >= {0} ", age);
                else if (sex == "女") sbsql.AppendFormat(" And Convert(int,IsNull(K11,'0')) <= {0} And Convert(int,IsNull(K12,'9999')) >= {0} ", age);
                else sbsql.AppendFormat(" And ((Convert(int,IsNull(K11,'0')) <= {0} And Convert(int,IsNull(K12,'9999')) >= {0}) Or (Convert(int,IsNull(K9,'0')) <= {0} And Convert(int,IsNull(K10,'9999')) >= {0}) )", age);
            }
            if (!string.IsNullOrWhiteSpace(education)) sbsql.AppendFormat(" And (K13 Like '%{0}%' OR K13 Like '%无要求%' OR IsNull(K13,'')='') ", education);
            if (!string.IsNullOrWhiteSpace(graduation_year))
            {
                sbsql.AppendFormat(" And Convert(int,IsNull(K14,'0')) <= {0} And Convert(int,IsNull(K15,'9999')) >= {0} ", graduation_year);
            }
            if (!string.IsNullOrWhiteSpace(employment_status))
            {
                sbsql.AppendFormat(" And (K16 Like '%{0}%' OR K16 Like '%无要求%' OR IsNull(K16,'')='') ", employment_status);

                if (!string.IsNullOrWhiteSpace(current_job_life) && employment_status == "就业")
                {
                    sbsql.AppendFormat(" And Convert(int,IsNull(K17,'0')) <= {0} And Convert(int,IsNull(K18,'9999')) >= {0} ", current_job_life);
                }
                if (!string.IsNullOrWhiteSpace(unemployment_period) && employment_status.Contains("失业"))
                {
                    sbsql.AppendFormat(" And Convert(int,IsNull(K19,'0')) <= {0} And Convert(int,IsNull(K20,'9999')) >= {0} ", unemployment_period);
                }
            }
            if (!string.IsNullOrWhiteSpace(company_type)) sbsql.AppendFormat(" And (K21 ='{0}' OR K21='无要求' OR IsNull(K21,'')='')  ", company_type);
            if (!string.IsNullOrWhiteSpace(registered_capital))
            {
                sbsql.AppendFormat(" And Convert(int,IsNull(K22,'0')) <= {0} And Convert(int,IsNull(K23,'999999999')) >= {0} ", registered_capital);
            }
            if (!string.IsNullOrWhiteSpace(personnel_size))
            {
                sbsql.AppendFormat(" And Convert(int,IsNull(K24,'0')) <= {0} And Convert(int,IsNull(K25,'99999')) >= {0} ", personnel_size);
            }
            if (!string.IsNullOrWhiteSpace(company_size)) sbsql.AppendFormat(" And (K26 Like '%{0}%' OR K26 Like '%无要求%' OR IsNull(K26,'')='') ", company_size);
            if (!string.IsNullOrWhiteSpace(industry)) sbsql.AppendFormat(" And (K27 ='{0}' OR K27='无要求' OR IsNull(K27,'')='')  ", industry);


            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbsql.Append("AND ( ");
                sbsql.AppendFormat(" ActivityName Like '%{0}%' ", keyword);
                sbsql.AppendFormat(" OR K5 Like '%{0}%' ", keyword);
                sbsql.Append(" ) ");
            }
            string order1 = "ISNULL(Sort,0) DESC, [JuActivityID] desc,";
            string order = order1;
            switch (sort)
            {
                default:
                    break;
            }
            order = order.TrimEnd(',');


            total = GetCount<JuActivityInfo>(sbsql.ToString());

            if (string.IsNullOrWhiteSpace(colName))
            {
                return GetLit<JuActivityInfo>(rows, page, sbsql.ToString(), order);
            }
            else
            {
                return GetColList<JuActivityInfo>(rows, page, sbsql.ToString(), order, colName);
            }
        }

        /// <summary>
        /// 获取门店信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JuActivityInfo GetStoreById(string id) {

            return Get<JuActivityInfo>(string.Format("WebsiteOwner='{0}' And ArticleType='Outlets' And K5='{1}'", WebsiteOwner, id));
        
        
        }

        /// <summary>
        /// 查询通用网点列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public List<JuActivityInfo> GetCommOutletsList(int rows, int page, string websiteOwner, string type, bool show_delete, int map_show, Dictionary<string, string> equals, Dictionary<string, string> contains,
            Dictionary<string, string> keywords, string longitude, string latitude, int? range, string colName, out int total, string onlyLngLatIsNull = "")
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbsql.AppendFormat(" AND ArticleType = '{0}' ", type);
            if (!show_delete) sbsql.Append(" AND IsDelete = 0 ");
            foreach (var item in equals)
            {
                if (item.Key == "CategoryId")
                {
                    if (!string.IsNullOrEmpty(item.Value) && item.Value != "0" && !item.Value.Contains(","))
                    {
                        string categoryId = new BLLArticleCategory().GetCateAndChildIds(int.Parse(item.Value));//获取下面的子分类
                        if (string.IsNullOrEmpty(categoryId)) categoryId = "-1";
                        sbsql.AppendFormat(" AND ( CategoryId in ({0})  OR RootCateId IN ({0})  )", categoryId);
                    }
                    else if (!string.IsNullOrEmpty(item.Value) && item.Value.Contains(","))
                    {
                        string categoryId = "'" + item.Value.Replace(",", "','") + "'";
                        sbsql.AppendFormat(" AND CategoryId in ({0}) ", categoryId);
                    }
                }
                else if (item.Key == "Tags")
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        var tagArr = item.Value.Split(',').ToList();
                        sbsql.Append("AND ( ");
                        for (int i = 0; i < tagArr.Count; i++)
                        {
                            if (i > 0)
                                sbsql.Append(" OR ");
                            sbsql.AppendFormat(" Tags LIKE '%{0}%' ", tagArr[i]);
                        }
                        sbsql.Append(" ) ");
                    }
                }
                else
                {
                    sbsql.AppendFormat(" And {0} = '{1}' ", item.Key, item.Value);
                }
            }
            foreach (var item in contains)
            {
                sbsql.AppendFormat(" And {0} Like '%{1}%' ", item.Key, item.Value);
            }
            if (keywords.Count > 0)
            {
                sbsql.Append("AND ( 1=2 ");
                foreach (KeyValuePair<string, string> item in keywords)
                {
                    sbsql.AppendFormat(" OR {0} Like '%{1}%' ", item.Key, item.Value);
                }
                sbsql.Append(" ) ");
            }
            if (onlyLngLatIsNull == "1")
            {
                sbsql.AppendFormat(" And IsNull([UserLongitude],'')='' ");
            }
            if ((map_show == 1 || map_show == 2) && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                sbsql.AppendFormat(" AND [UserLongitude] IS NOT NULL ");
                sbsql.AppendFormat(" AND dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude])<{2} ", longitude, latitude, range.HasValue ? range.Value : int.MaxValue);
            }
            string order = "ISNULL(Sort,0) DESC, [JuActivityID] desc";
            if ((map_show == 1 || map_show == 2) && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                order = string.Format("dbo.fnGetDistance({0},{1},[UserLongitude],[UserLatitude]) asc,", longitude, latitude) + order;
            }
            total = GetCount<JuActivityInfo>(sbsql.ToString());
            string fieldStr = colName + ",-1 [Distance]";
            if ((map_show == 1 || map_show == 2) && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                fieldStr = string.Format("{0},dbo.fnGetDistance({1},{2},[UserLongitude],[UserLatitude]) [Distance]", colName, longitude, latitude);
            }
            return GetColList<JuActivityInfo>(rows, page, sbsql.ToString(), order, fieldStr);
        }
    }

    

}
