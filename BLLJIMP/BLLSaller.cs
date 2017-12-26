using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 基于用户模块扩展的商家模块
    /// </summary>
    public class BLLSaller:BLLUser
    {
        BLLCommRelation bllCommRelation = new BLLCommRelation();

        /// <summary>
        /// 获取商家平均分返回结果
        /// </summary>
        public class SallerRateScoreAvgResult
        {
            /// <summary>
            /// 信誉评分
            /// </summary>
            public double ReputationScore { get; set; }
            /// <summary>
            /// 服务态度评分
            /// </summary>
            public double ServiceAttitudeScore { get; set; }
        }

        /// <summary>
        /// 获取商家平均分
        /// </summary>
        /// <param name="sallerId"></param>
        /// <returns></returns>
        public SallerRateScoreAvgResult GetSallerRateScoreAvg(string sallerId)
        {
            SallerRateScoreAvgResult result = new SallerRateScoreAvgResult();

            /*
            获取所有评分
            计算平均分（减轻数据库负担先取出所有数据由程序内存计算）
            */

            var dataList = bllCommRelation.GetRelationList(Enums.CommRelationType.SallerRateScore, "", sallerId, 1, 1000000);

            if (dataList.Count(data => !string.IsNullOrWhiteSpace(data.Ex1) && Common.ValidatorHelper.IsNumber(data.Ex1)) > 0)
            {
                result.ReputationScore = dataList.Where(data => !string.IsNullOrWhiteSpace(data.Ex1)).Average(p => Convert.ToDouble(p.Ex1));
            }
            if (dataList.Count(data => !string.IsNullOrWhiteSpace(data.Ex2) && Common.ValidatorHelper.IsNumber(data.Ex2)) > 0)
            {
                result.ServiceAttitudeScore = dataList.Where(data => !string.IsNullOrWhiteSpace(data.Ex2)).Average(p => Convert.ToDouble(p.Ex2));
            }
            
            return result;
        }
        
        /// <summary>
        /// 评价商家
        /// </summary>
        /// <param name="sallerId">商家id</param>
        /// <param name="doRateUserId">评价者id</param>
        /// <param name="reputationScore">信誉分数</param>
        /// <param name="serviceAttitudeScore">服务态度分数</param>
        /// <param name="comment">评价内容</param>
        /// <returns></returns>
        public bool RateSaller(string sallerId,string doRateUserId,double reputationScore,double serviceAttitudeScore,string comment)
        {
            return bllCommRelation.AddCommRelation(Enums.CommRelationType.SallerRateScore, doRateUserId, sallerId,"", reputationScore.ToString(), serviceAttitudeScore.ToString(), comment);
        }

        /// <summary>
        /// 获取商家收到的评价列表
        /// </summary>
        /// <param name="sallerId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Model.CommRelationInfo> GetSallerRateList(string sallerId,int pageSize,int pageIndex,out int totalCount)
        {
            List<Model.CommRelationInfo> result = new List<Model.CommRelationInfo>();

            result = bllCommRelation.GetRelationListDesc(Enums.CommRelationType.SallerRateScore, "", sallerId, pageIndex, pageSize,out totalCount);

            return result;
        }
    }
}
