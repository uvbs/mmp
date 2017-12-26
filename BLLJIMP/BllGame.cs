using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 游戏BLL
    /// </summary>
    public class BllGame : BLL
    {
        public BllGame()
            : base()
        {

        }



        /// <summary>
        /// 插入游戏事件表 点击
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddGameEventDetailClick(GameEventDetailInfoClick model) {

            return Add(model);
        
        
        }
        /// <summary>
        /// 插入游戏事件表 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddGameEventDetail(GameEventDetailInfo model)
        {

            return Add(model);


        }

        /// <summary>
        /// 插入任务计划表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddGameAdvertPlan(GameAdvertPlan model) {

            return Add(model);
        
        }
        /// <summary>
        /// 删除任务计划表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteGameAdvertPlan(GameAdvertPlan model)
        {

            return Delete(model)>0;

        }
        /// <summary>
        /// 获取游戏任务分页
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="rows">每页行数</param>
        /// <param name="totalCount">总数</param>
        /// <param name="planName">任务名称</param>
        /// <returns></returns>
        public List<GameAdvertPlan> GetGameAdvertPlanList(int page, int rows, out int totalCount, string planName = "")
        {
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'",WebsiteOwner));
            if (!string.IsNullOrEmpty(planName))
            {
                sbWhere.AppendFormat(" And PlanName like '%{0}%'",planName);
            }
             totalCount = GetCount<GameAdvertPlan>(sbWhere.ToString());
             return  GetLit<GameAdvertPlan>(rows, page, sbWhere.ToString(),"AutoID DESC");
        }

        /// <summary>
        /// 获取单个游戏任务
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public GameAdvertPlan GetSingleGameAdvertPlan(int planId) {

            return Get<GameAdvertPlan>(string.Format("AutoID={0}",planId));
        
        
        }
        /// <summary>
        /// 更新游戏任务 IP PV
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public bool UpdateGamePlanIPPV(int planId) {

            var planInfo = GetSingleGameAdvertPlan(planId);
            planInfo.PV++;
            planInfo.IP = GetGamePlanIP(planId);
            return Update(planInfo);

        }
        /// <summary>
        /// 更新广告点击数
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public bool UpdateGamePlanClickCount(int planId) {

            var planInfo = GetSingleGameAdvertPlan(planId);
            planInfo.AdvertClickCount++;
           return  Update(planInfo);
        
        }

        /// <summary>
        /// 获取任务IP 数
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        public int GetGamePlanIP(int planId) {

            return GetCount<GameEventDetailInfo>("SourceIP", string.Format("GamePlanID={0}", planId));
        
        }

        /// <summary>
        /// 添加游戏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddGameInfo(GameInfo model) {

            return Add(model);
        
        }

        /// <summary>
        /// 添加游戏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateGameInfo(GameInfo model)
        {

            return Update(model);

        }
        /// <summary>
        /// 删除游戏信息
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public bool DeleteGameInfo(int autoId )
        {
            return Delete(new GameInfo(), string.Format("AutoID={0}",autoId))>0;
        
        }
        /// <summary>
        /// 获取单个游戏信息
        /// </summary>
        /// <param name="autoid"></param>
        /// <returns></returns>
        public GameInfo GetSingleGameInfo(int autoid) {


            return Get<GameInfo>(string.Format("AutoID={0}",autoid));
        
        }
        /// <summary>
        /// 获取游戏数量
        /// </summary>
        /// <returns></returns>
        public List<GameInfo> GetTopGameList(int rows) {

            return GetLit<GameInfo>(rows, 1, "", "GameSort ASC");
        
        
        }
        /// <summary>
        /// 删除游戏任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteGamePlan(string ids) {

           
            int count =Delete(new GameAdvertPlan(), string.Format("AutoID in ({0})", ids));
            Delete(new GameEventDetailInfo(), string.Format("GamePlanID in ({0})", ids));
            Delete(new GameEventDetailInfoClick(), string.Format("GamePlanID in ({0})", ids));
            return count;

        
        }



    }
}
