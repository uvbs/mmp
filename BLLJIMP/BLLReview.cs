using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP
{
    //BLL for 话题，评论，帖子
    public class BLLReview : BLL
    {
        BLLCommRelation bLLCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bllJuActivity = new BLLJuActivity();

        public enum AuditStatus
        {
            UnAudit = 0,    //未审核
            Permitted = 1,   //审核通过
            RejectedNoReason = 2    //拒绝
        }

        public bool BatchAudit(AuditStatus status, string[] ids)
        {
            try
            {
                ReviewInfo rInfo = new ReviewInfo();
                string setPms = string.Format("AuditStatus = {0}", (int)status);
                string strWhere = string.Format("AutoId in ({0})", string.Join(",", ids));

                var result = this.Update(rInfo, setPms, strWhere) > 0;
                BLLRedis.ClearReviewList(WebsiteOwner);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 添加评论或评论回复
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="foreignkeyId">外键id</param>
        /// <param name="preReviewId">所属评论id</param>
        /// <param name="reviewerId">提交者id</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="reviewMainId">主id</param>
        /// <returns></returns>
        public bool AddReview(Enums.ReviewTypeKey type, string foreignkeyId, int preReviewId, string reviewerId, string title, string content, string websiteOwner, out int reviewMainId, int isHideUserName)
        {
            reviewMainId = int.Parse(GetGUID(TransacType.CommAdd));

            var typeKey = CommonPlatform.Helper.EnumStringHelper.ToString(type);

            //保存基本信息
            Model.ReviewInfo data = new ReviewInfo()
            {
                ReviewMainId = reviewMainId,
                ReviewType = typeKey,
                ForeignkeyId = foreignkeyId,
                ParentId = preReviewId,
                ReviewTitle = title,
                ReviewContent = content,
                WebsiteOwner = websiteOwner,
                IsHideUserName = isHideUserName,
                InsertDate = DateTime.Now,
                UserId = reviewerId
            };


            if (type != Enums.ReviewTypeKey.CommentReply)
            {
                data.Expand1 = foreignkeyId;

                bllJuActivity.PlusNumericalCol("CommentCount", int.Parse(foreignkeyId));
                bllJuActivity.PlusNumericalCol("CommentAndReplayCount", int.Parse(foreignkeyId));
            }
            else if (type == Enums.ReviewTypeKey.CommentReply)
            {
                int pre = 0;
                int.TryParse(foreignkeyId, out pre);
                var preData = GetReviewInfo(pre);
                if (preData != null)
                {
                    data.Expand1 = preData.ForeignkeyId;
                    bllJuActivity.PlusNumericalCol("CommentAndReplayCount", int.Parse(preData.ForeignkeyId));
                }
            }
            var result = Add(data);
            BLLRedis.ClearReviewList(websiteOwner);
            return result;
        }

        /// <summary>
        /// 获取评论数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="foreignkeyId"></param>
        /// <returns></returns>
        public int GetReviewCount(Enums.ReviewTypeKey type, string foreignkeyId, string userId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" ReviewType = '{0}'", CommonPlatform.Helper.EnumStringHelper.ToString(type));
            if(!string.IsNullOrWhiteSpace(foreignkeyId)) sbSql.AppendFormat(" AND ForeignkeyId = '{0}'", foreignkeyId);
            if (!string.IsNullOrWhiteSpace(userId)) sbSql.AppendFormat(" AND UserId = '{0}'", userId);
            int result = GetCount<ReviewInfo>(sbSql.ToString());
            return result;
        }

        /// <summary>
        /// 获取单条评论信息
        /// </summary>
        /// <param name="reviewMainId"></param>
        /// <returns></returns>
        public ReviewInfo GetReviewInfo(int reviewMainId)
        {
            return Get<ReviewInfo>(string.Format(" ReviewMainId = {0} ", reviewMainId));
        }


        /// <summary>
        /// 获取单条评论信息
        /// </summary>
        /// <param name="reviewMainId"></param>
        /// <returns></returns>
        public ReviewInfo GetReviewByAutoId(int autoId)
        {
            return Get<ReviewInfo>(string.Format(" AutoId = {0} ", autoId));
        }
        public List<ReviewInfo> GetReviewList(Enums.ReviewTypeKey type, out int totalCount, int pageIndex, int pageSize, 
            string foreignkeyId, string websiteOwner, string currUserId, string orderby = "", string UserId = "",
            string auditStatus = "", string expand1 = "", bool showStatistic = true, bool showReplayToUser = true)
        {
            List<ReviewInfo> result = new List<ReviewInfo>();

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" ReviewType = '{0}' ", CommonPlatform.Helper.EnumStringHelper.ToString(type));
            if (!string.IsNullOrWhiteSpace(foreignkeyId))
            {
                strWhere.AppendFormat(" AND ForeignkeyId = '{0}' ", foreignkeyId);
            }
            if(!string.IsNullOrWhiteSpace(UserId))
            {
                strWhere.AppendFormat(" AND UserId = '{0}' ", UserId);
            }
            //if (!string.IsNullOrWhiteSpace(websiteOwner))
            //{
            //    strWhere.AppendFormat(" AND websiteOwner = '{0}' ", websiteOwner);
            //}
            if (!string.IsNullOrWhiteSpace(expand1))
            {
                strWhere.AppendFormat(" AND Expand1 = '{0}' ", expand1);
            }
            if (!string.IsNullOrWhiteSpace(auditStatus))
            {
                strWhere.AppendFormat(" AND AuditStatus = {0} ", auditStatus);
            }
            if (string.IsNullOrWhiteSpace(orderby))
            {
                orderby = " AutoId DESC ";
            }

            result = GetLit<ReviewInfo>(pageSize, pageIndex, strWhere.ToString(), orderby);

            for (int i = 0; i < result.Count; i++)
            {
                result[i] = FilterReviewInfo(result[i], currUserId, showStatistic, showReplayToUser);
            }

            totalCount = GetCount<ReviewInfo>(strWhere.ToString());

            return result;
        }
        /// <summary>
        /// 评论,话题列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="foreignkeyId"></param>
        /// <param name="keyword"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public List<ReviewInfo> GetActReviewList(out int totalCount, int pageIndex, int pageSize, string foreignkeyId, string keyword,
            string orderby, string reviewType = "", string expand1 = "", string auditStatus="", string colName ="")
        {
            List<ReviewInfo> result = new List<ReviewInfo>();

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WebsiteOwner='{0}' ",WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(foreignkeyId))
            {
                strWhere.AppendFormat(" AND ForeignkeyId = '{0}' ", foreignkeyId);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strWhere.AppendFormat(" AND ReviewContent like '%{0}%' ", keyword);
            }
            if (!string.IsNullOrWhiteSpace(reviewType))
            {
                strWhere.AppendFormat(" AND ReviewType = '{0}' ", reviewType);
            }
            if (!string.IsNullOrWhiteSpace(expand1))
            {
                strWhere.AppendFormat(" AND Expand1 = '{0}' ", expand1);
            }
            if (!string.IsNullOrWhiteSpace(auditStatus))
            {
                strWhere.AppendFormat(" AND AuditStatus in ({0}) ", auditStatus);
            }
            if (string.IsNullOrWhiteSpace(orderby))
            {
                orderby = " AutoID DESC ";
            }
            
            try
            {
                var orginKey = string.Format("pageSize{0}pageIndex{1}{2}orderBy{3}colName{4}", pageSize, pageIndex, strWhere.ToString(), orderby,colName);//原始key
                var listKey = string.Format("{0}:{2}:{1}", WebsiteOwner, Common.DEncrypt.GetMD5(orginKey),Common.SessionKey.ReviewListKeys);

                var redisDataStr = string.Empty;
                
                if (!string.IsNullOrWhiteSpace(redisDataStr))
                {
                    var redisData = JsonConvert.DeserializeObject<Model.API.List.ReviewList>(redisDataStr);
                    totalCount = redisData.TotalCount;
                    result = redisData.List;
                }
                else
                {
                    totalCount = GetCount<ReviewInfo>(strWhere.ToString());
                    if (!string.IsNullOrWhiteSpace(colName))
                    {
                        result = GetColList<ReviewInfo>(pageSize, pageIndex, strWhere.ToString(), orderby, colName);
                    }
                    else
                    {
                        result = GetLit<ReviewInfo>(pageSize, pageIndex, strWhere.ToString(), orderby);
                    }

                    var redisData = new Model.API.List.ReviewList()
                    {
                        TotalCount = totalCount,
                        List = result
                    };

                    RedisHelper.RedisHelper.StringSet(listKey, JsonConvert.SerializeObject(redisData));

                    if (!RedisHelper.RedisHelper.SetContains(WebsiteOwner + ":" + Common.SessionKey.ReviewListKeys, listKey))
                        RedisHelper.RedisHelper.SetAdd(WebsiteOwner + ":" + Common.SessionKey.ReviewListKeys, listKey);
                }
            }
            catch (Exception ex)
            {
                ToLog("获取商品列表异常，改成直接读取数据库：" + ex.Message, @"D:\log\redisdata.txt");
                totalCount = GetCount<ReviewInfo>(strWhere.ToString());
                if (!string.IsNullOrWhiteSpace(colName))
                {
                    result = GetColList<ReviewInfo>(pageSize, pageIndex, strWhere.ToString(), orderby, colName);
                }
                else
                {
                    result = GetLit<ReviewInfo>(pageSize, pageIndex, strWhere.ToString(), orderby);
                }
            }

            return result;
        }
        //获取平均分
        public double GetReviewAvgScore(string websiteOwner,string foreignkeyId, string reviewType = "", string expand1 = "", string auditStatus = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" Select Avg(ReviewScore) From ZCJ_ReviewInfo Where");
            strSql.AppendFormat(" WebsiteOwner='{0}' ",websiteOwner);
            if (!string.IsNullOrWhiteSpace(foreignkeyId))
            {
                strSql.AppendFormat(" AND ForeignkeyId = '{0}' ", foreignkeyId);
            }
            if (!string.IsNullOrWhiteSpace(reviewType))
            {
                strSql.AppendFormat(" AND ReviewType = '{0}' ", reviewType);
            }
            if (!string.IsNullOrWhiteSpace(expand1))
            {
                strSql.AppendFormat(" AND Expand1 = '{0}' ", expand1);
            }
            if (!string.IsNullOrWhiteSpace(auditStatus))
            {
                strSql.AppendFormat(" AND AuditStatus = {0} ", auditStatus);
            }
            object result = GetSingle(strSql.ToString(), "ReviewInfo");
            return Convert.ToDouble(result);
        }
        /// <summary>
        /// 获取回复数量
        /// </summary>
        /// <param name="foreignkeyId"></param>
        /// <returns></returns>
        public int GetReviewCount(string foreignkeyId)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(foreignkeyId))
            {
                strWhere.AppendFormat(" AND ForeignkeyId = '{0}' ", foreignkeyId);
            }
            return GetCount<ReviewInfo>(strWhere.ToString());
        }
        /// <summary>
        /// 填充回复信息相关关系数据
        /// </summary>
        /// <param name="item">回复数据</param>
        /// <param name="currUserId">用户id</param>
        /// <returns></returns>
        public ReviewInfo FilterReviewInfo(ReviewInfo item, string currUserId, bool showStatistic = true, bool showReplayToUser = true)
        {
            if (item == null)
            {
                return null;
            }

            if (showStatistic)
            {
                if (!string.IsNullOrWhiteSpace(currUserId))
                {
                    //当前用户是否收藏和点赞
                    item.CurrUserIsFavorite = bLLCommRelation.ExistRelation(Enums.CommRelationType.ReviewFavorite, item.ReviewMainId.ToString(), currUserId);
                    item.CurrUserIsPraise = bLLCommRelation.ExistRelation(Enums.CommRelationType.ReviewPraise, item.ReviewMainId.ToString(), currUserId);
                }

                //回复数，点赞数，收藏数

                if (item.ReviewType == CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.ArticleComment))
                {
                    item.ReplyCount = GetCount<ReviewInfo>(string.Format(" ForeignkeyId = '{0}' AND ReviewType = '{1}' ",
                            item.ReviewMainId,
                            CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.CommentReply))
                        );
                }

                if (item.ReviewType == CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.CommentReply))
                {
                    item.ReplyCount = GetCount<ReviewInfo>(string.Format(" ParentId = '{0}' AND ReviewType = '{1}' ",
                            item.ReviewMainId,
                            CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.CommentReply))
                        );
                }

                item.PraiseCount = bLLCommRelation.GetRelationCount(Enums.CommRelationType.ReviewPraise, item.ReviewMainId.ToString(), "");
                item.FavoriteCount = bLLCommRelation.GetRelationCount(Enums.CommRelationType.ReviewFavorite, item.ReviewMainId.ToString(), "");
            }

            item.PubUser = bllUser.GetUserInfo(item.UserId);

            if (showReplayToUser && item.ParentId > 0)
            {
                var reviewInfo = GetReviewInfo(item.ParentId);

                if (reviewInfo != null)
                {
                    item.ReplayToUser = bllUser.GetUserInfo(reviewInfo.UserId);
                }
            }
            
            return item;
        }

        public ReviewInfo FilterReviewInfo(int reviewMainId,string currUserId)
        { 
            return FilterReviewInfo(GetReviewInfo(reviewMainId),currUserId);
        }

        /// <summary>
        /// 被举报的回答和回复列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="showHide"></param>
        /// <returns></returns>
        public System.Data.DataSet GetIllegalReviewList(int pageSize, int pageIndex, string keyword, string websiteOwner)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WITH A AS( ");
            strWhere.AppendFormat(" SELECT ROW_NUMBER() OVER (ORDER BY A.[AutoId] DESC) NUM ");
            strWhere.AppendFormat(" ,B.[UserId],B.[ReviewType],A.[RelationId] ");
            strWhere.AppendFormat(" ,B.[ReviewContent],A.[RelationTime] ");
            strWhere.AppendFormat(" ,B.[PV],B.[ReviewMainId] ");
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_ReviewInfo] B ON A.[MainId]=B.[ReviewMainId] ");
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     AND (B.[ReviewContent] like '%{0}%')  ", keyword);
            strWhere.AppendFormat("     AND A.[RelationType]='ReportReviewIllegalContent' ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat("     AND [WebsiteOwner]='{0}' ", websiteOwner);
            strWhere.AppendFormat(" WHERE  A.[RelationType] = 'ReportReviewIllegalContent' ");
            strWhere.AppendFormat(" ) ");
            strWhere.AppendFormat(" SELECT * FROM A WHERE NUM BETWEEN ({1}-1)* {0}+1 AND {1}*{0}; ", pageSize, pageIndex);

            strWhere.AppendFormat(" SELECT COUNT(1)[TOTALCOUNT] ");
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_ReviewInfo] B ON A.[MainId]=B.[ReviewMainId] ");
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     AND (B.[ReviewContent] like '%{0}%')  ", keyword);
            strWhere.AppendFormat("     AND A.[RelationType]='ReportReviewIllegalContent' ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat("     AND [WebsiteOwner]='{0}' ", websiteOwner);
            strWhere.AppendFormat(" WHERE A.[RelationType] = 'ReportReviewIllegalContent' ");
            System.Data.DataSet ds = Query(strWhere.ToString());
            return ds;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="juActivityID"></param>
        /// <returns></returns>
        public bool DelReview(string ReviewMainId)
        {
            var result = Delete(new ReviewInfo(), string.Format(" ReviewMainId={0} ", ReviewMainId)) > 0;
            BLLRedis.ClearReviewList(WebsiteOwner);
            return result;
        }


        //订单评价相关

        //获取指定订单指定商品的评价记录
        public ReviewInfo GetOrderProductReviewInfo(string orderId, string productId,string orderDetailId="")
        {

            StringBuilder sbSql = new StringBuilder();

          
            sbSql.AppendFormat(" ReviewType='{0}' ", CommonPlatform.Helper.EnumStringHelper.ToString(Enums.ReviewTypeKey.OrderComment));
           

            if (!string.IsNullOrEmpty(orderId))
            {
                sbSql.AppendFormat(" AND ForeignkeyId='{0}' ", orderId);
            }

            if (!string.IsNullOrEmpty(productId))
            {
                sbSql.AppendFormat(" AND Expand1='{0}' ", productId);
            }

            if (!string.IsNullOrEmpty(orderDetailId))
            {
                sbSql.AppendFormat(" AND Ex2='{0}' ", orderDetailId);
            }


            return Get<ReviewInfo>(sbSql.ToString());
        }

    }
}