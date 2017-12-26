using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLScoreDefine : BLL
    {
        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Model.ScoreDefineInfo GetScoreDefineInfo(string scoreType, string websiteOwner)
        {
            return Get<Model.ScoreDefineInfo>(string.Format(" ScoreType='{0}' AND WebsiteOwner='{1}' ", scoreType, websiteOwner));
        }
        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="scoreId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Model.ScoreDefineInfo GetScoreDefineInfo(int scoreId,string websiteOwner)
        {
            return Get<Model.ScoreDefineInfo>(string.Format(" ScoreId='{0}' AND WebsiteOwner='{1}' ", scoreId, websiteOwner));
        }
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="scoreDefineInfo"></param>
        /// <returns></returns>
        public bool ExistsScoreDefine(Model.ScoreDefineInfo scoreDefineInfo)
        {
            return GetScoreDefineInfo(scoreDefineInfo.ScoreType, scoreDefineInfo.WebsiteOwner) == null ? false : true;
        }

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="scoreDefineInfo"></param>
        /// <returns></returns>
        public bool ExistsScoreDefine(string scoreType, string websiteOwner)
        {
            return GetScoreDefineInfo(scoreType, websiteOwner) == null ? false : true;
        }

        /// <summary>
        /// 添加编辑积分规则
        /// </summary>
        /// <param name="scoreDefineInfo"></param>
        /// <returns></returns>
        public bool PutScoreDefine(Model.ScoreDefineInfo scoreDefineInfo)
        {
            //    if (ExistsScoreDefine(scoreDefineInfo))
            //    {
            //        Model.ScoreDefineInfo oldScoreDefineInfo = GetScoreDefineInfo(scoreDefineInfo.ScoreType, scoreDefineInfo.WebsiteOwner);
            //        oldScoreDefineInfo.Score = scoreDefineInfo.Score;
            //        oldScoreDefineInfo.IsHide = scoreDefineInfo.IsHide;
            //        oldScoreDefineInfo.DayLimit = scoreDefineInfo.DayLimit;
            //        oldScoreDefineInfo.TotalLimit = scoreDefineInfo.TotalLimit;
            //        oldScoreDefineInfo.Description = scoreDefineInfo.Description;
            //        oldScoreDefineInfo.OrderNum = scoreDefineInfo.OrderNum;
            //        oldScoreDefineInfo.Ex1 = scoreDefineInfo.Ex1;
            //        return Update(oldScoreDefineInfo);
            //    }
            //    else
            //    {
            //        scoreDefineInfo.ScoreId = int.Parse(GetGUID(TransacType.CommAdd));
            //        return Add(scoreDefineInfo);
            //    }

             Model.ScoreDefineInfo oldScoreDefineInfo=GetScoreDefineInfo(scoreDefineInfo.ScoreId,scoreDefineInfo.WebsiteOwner);
             if (oldScoreDefineInfo == null)//新增
            {
                if (scoreDefineInfo.ScoreType=="Customize")
                {
                    if (GetCount<ScoreDefineInfo>(string.Format(" ScoreEvent='{0}' And WebsiteOwner='{1}'", scoreDefineInfo.ScoreEvent, WebsiteOwner)) > 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (GetCount<ScoreDefineInfo>(string.Format(" ScoreType='{0}' And WebsiteOwner='{1}'", scoreDefineInfo.ScoreType, WebsiteOwner)) > 0)
                    {
                        return false;
                    }
                }


                scoreDefineInfo.ScoreId = int.Parse(GetGUID(TransacType.CommAdd));
                return Add(scoreDefineInfo);
            }
            else//编辑
            {
                oldScoreDefineInfo.Score = scoreDefineInfo.Score;
                oldScoreDefineInfo.IsHide = scoreDefineInfo.IsHide;
                oldScoreDefineInfo.DayLimit = scoreDefineInfo.DayLimit;
                oldScoreDefineInfo.TotalLimit = scoreDefineInfo.TotalLimit;
                oldScoreDefineInfo.Description = scoreDefineInfo.Description;
                oldScoreDefineInfo.OrderNum = scoreDefineInfo.OrderNum;
                oldScoreDefineInfo.Ex1 = scoreDefineInfo.Ex1;
                oldScoreDefineInfo.ScoreEvent = scoreDefineInfo.ScoreEvent;
                oldScoreDefineInfo.BaseRateValue = scoreDefineInfo.BaseRateValue;
                oldScoreDefineInfo.BaseRateScore = scoreDefineInfo.BaseRateScore;
                return Update(oldScoreDefineInfo);
            }


        }
        /// <summary>
        /// 获取积分规则列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<Model.ScoreDefineInfo> GetScoreDefineList(string websiteOwner, int? isHide = 0, int? top = 100)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isHide.HasValue) sbSql.AppendFormat(" AND IsHide={0} ", isHide);

            return GetList<Model.ScoreDefineInfo>(top.Value, sbSql.ToString(), "OrderNum,ScoreId");
        }
        /// <summary>
        /// 获取积分规则列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<ScoreDefineInfo> GetScoreDefineList(int pageSize, int pageIndex, string websiteOwner, out int total, int? isHide = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isHide.HasValue) sbSql.AppendFormat(" AND IsHide={0} ", isHide);
            total = GetCount<ScoreDefineInfo>(sbSql.ToString());
            return GetLit<ScoreDefineInfo>(pageSize, pageIndex, sbSql.ToString(), "OrderNum,ScoreId");
        }

        /// <summary>
        /// 删除积分规则
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool DeleteScoreDefine(string scoreType, string websiteOwner)
        {
            if (Delete(new ScoreDefineInfo(), string.Format(" ScoreType='{0}' AND WebsiteOwner='{1}' ", scoreType, websiteOwner)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除积分规则
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool DeleteScoreDefine(int scoreId, string websiteOwner)
        {
            if (Delete(new ScoreDefineInfo(), string.Format(" ScoreId='{0}' AND WebsiteOwner='{1}' ", scoreId, websiteOwner)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除积分规则
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool DeleteScoreDefineEx(int id, string websiteOwner)
        {
            if (Delete(new ScoreDefineInfoExt(), string.Format(" AutoId={0} AND WebsiteOwner='{1}' ", id, websiteOwner)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
        /// <summary>
        /// 获取规则根据积分事件
        /// </summary>
        /// <param name="scoreEvent">积分事件</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public Model.ScoreDefineInfo GetScoreDefineInfoByScoreEvent(string scoreEvent, string websiteOwner)
        {
            return Get<Model.ScoreDefineInfo>(string.Format(" ScoreEvent='{0}' AND WebsiteOwner='{1}' ", scoreEvent, websiteOwner));
        }

        /// <summary>
        /// 获取积分扩展列表
        /// </summary>
        /// <param name="scoreId"></param>
        /// <returns></returns>
        public List<ScoreDefineInfoExt> GetScoreDefineExList(int scoreId) {

            return GetList<ScoreDefineInfoExt>(string.Format(" ScoreId={0}", scoreId)); 
        
        
        }


    }
}
