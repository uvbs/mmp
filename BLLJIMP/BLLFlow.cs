using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP
{
    public class BLLFlow:BLL
    {
        /// <summary>
        /// 通过id查流程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Flow GetFlowByID(int id, string websiteOwner)
        {
            return GetByKey<BLLJIMP.Model.Flow>("AutoID", id.ToString(), websiteOwner: websiteOwner);
        }
        /// <summary>
        /// 通过key查流程
        /// </summary>
        /// <param name="key"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Flow GetFlowByKey(string key, string websiteOwner)
        {
            return GetByKey<BLLJIMP.Model.Flow>("FlowKey", key, websiteOwner: websiteOwner);
        }
        /// <summary>
        /// 拼接环节查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public string GetStepParamString(string websiteOwner, int flowId, int? stepId = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            sbSql.AppendFormat(" And FlowID='{0}'", flowId);
            if (stepId.HasValue) sbSql.AppendFormat(" And AutoID='{0}'", stepId);
            return sbSql.ToString();
        }
        /// <summary>
        /// 查询环节列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public List<FlowStep> GetStepList(int rows, int page, string websiteOwner, int flowId)
        {
            return GetLit<FlowStep>(rows, page, GetStepParamString(websiteOwner, flowId), "Sort Asc");
        }
        /// <summary>
        /// 查询环节
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public FlowStep GetStep(string websiteOwner, int flowId, int stepId)
        {
            return Get<FlowStep>(GetStepParamString(websiteOwner, flowId, stepId));
        }


        /// <summary>
        /// 拼接执行流程查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public string GetActionParamString(string websiteOwner, string memberUserId = "", string flowKey = "", string status = "",
            string handleUserId = "", string handleGroupId = "", string isActionMe = "", int? actId = null, string memberKey = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            if (!string.IsNullOrWhiteSpace(memberUserId)) sbSql.AppendFormat(" And MemberID='{0}'", memberUserId);
            if (!string.IsNullOrWhiteSpace(flowKey)) sbSql.AppendFormat(" And FlowKey='{0}'", flowKey);
            if (!string.IsNullOrWhiteSpace(memberKey))
            {
                sbSql.AppendFormat(" And (MemberPhone='{0}' Or MemberName Like '{0}%')", memberKey);
            }
            if (!string.IsNullOrWhiteSpace(status)) sbSql.AppendFormat(" And Status In ({0})", status);
            if (actId.HasValue) sbSql.AppendFormat(" And AutoID={0}", actId.Value);
            if (isActionMe == "1")
            {
                if (!string.IsNullOrWhiteSpace(handleUserId))
                {
                    sbSql.AppendFormat(" And EXISTS( ");
                    sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_FlowActionDetail] ");
                    sbSql.AppendFormat("    WHERE [FlowID] = [ZCJ_FlowAction].[FlowID] ");
                    sbSql.AppendFormat("    AND [ActionID]=[ZCJ_FlowAction].[AutoID] ");
                    sbSql.AppendFormat("    AND [WebsiteOwner]=[ZCJ_FlowAction].[WebsiteOwner] ");
                    sbSql.AppendFormat("    AND [HandleUserID]='{0}' ", handleUserId);
                    sbSql.AppendFormat(" ) ");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(handleUserId) && !string.IsNullOrWhiteSpace(handleGroupId))
                {
                    sbSql.AppendFormat(" And ( EXISTS( ");
                    sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_FlowStepRelation] ");
                    sbSql.AppendFormat("    WHERE [FlowID] = [ZCJ_FlowAction].[FlowID] ");
                    sbSql.AppendFormat("    AND [StepID]=[ZCJ_FlowAction].[StepID] ");
                    sbSql.AppendFormat("    AND [WebsiteOwner]=[ZCJ_FlowAction].[WebsiteOwner] ");
                    sbSql.AppendFormat("    AND [RelationType]='HandleUsers' ");
                    sbSql.AppendFormat("    AND [RelationID]='{0}' ", handleUserId);
                    sbSql.AppendFormat(" ) OR EXISTS( ");
                    sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_FlowStepRelation] ");
                    sbSql.AppendFormat("    WHERE [FlowID] = [ZCJ_FlowAction].[FlowID] ");
                    sbSql.AppendFormat("    AND [StepID]=[ZCJ_FlowAction].[StepID] ");
                    sbSql.AppendFormat("    AND [WebsiteOwner]=[ZCJ_FlowAction].[WebsiteOwner] ");
                    sbSql.AppendFormat("    AND [RelationType]='HandleGroups' ");
                    sbSql.AppendFormat("    AND [RelationID]='{0}' ", handleGroupId);
                    sbSql.AppendFormat(" ) ) ");
                }
                else if (!string.IsNullOrWhiteSpace(handleUserId))
                {
                    sbSql.AppendFormat(" And EXISTS( ");
                    sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_FlowStepRelation] ");
                    sbSql.AppendFormat("    WHERE [FlowID] = [ZCJ_FlowAction].[FlowID] ");
                    sbSql.AppendFormat("    AND [StepID]=[ZCJ_FlowAction].[StepID] ");
                    sbSql.AppendFormat("    AND [WebsiteOwner]=[ZCJ_FlowAction].[WebsiteOwner] ");
                    sbSql.AppendFormat("    AND [RelationType]='HandleUsers' ");
                    sbSql.AppendFormat("    AND [RelationID]='{0}' ", handleUserId);
                    sbSql.AppendFormat(" ) ");
                }
                else if (!string.IsNullOrWhiteSpace(handleGroupId))
                {
                    sbSql.AppendFormat(" And EXISTS( ");
                    sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_FlowStepRelation] ");
                    sbSql.AppendFormat("    WHERE [FlowID] = [ZCJ_FlowAction].[FlowID] ");
                    sbSql.AppendFormat("    AND [StepID]=[ZCJ_FlowAction].[StepID] ");
                    sbSql.AppendFormat("    AND [WebsiteOwner]=[ZCJ_FlowAction].[WebsiteOwner] ");
                    sbSql.AppendFormat("    AND [RelationType]='HandleGroups' ");
                    sbSql.AppendFormat("    AND [RelationID]='{0}' ", handleGroupId);
                    sbSql.AppendFormat(" ) ");
                }
            }
            return sbSql.ToString();
        }
        /// <summary>
        /// 查询执行流程列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public List<FlowAction> GetActionList(int rows, int page, string websiteOwner, string memberUserId = "", string flowKey = "", string status = "",
            string handleUserId = "", string handleGroupId = "", string isActionMe = "", string memberKey = "")
        {
            return GetLit<FlowAction>(rows, page, GetActionParamString(websiteOwner, memberUserId, flowKey, status, handleUserId, handleGroupId, isActionMe, memberKey: memberKey), "AutoID Desc");
        }

        /// <summary>
        /// 查询执行流程数量
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public int GetActionCount(string websiteOwner, string memberUserId = "", string flowKey = "", string status = "",
            string handleUserId = "", string handleGroupId = "", string isActionMe = "", string memberKey = "")
        {
            return GetCount<FlowAction>(GetActionParamString(websiteOwner, memberUserId, flowKey, status, handleUserId, handleGroupId, isActionMe, memberKey: memberKey));
        }
        /// <summary>
        /// 是否存在手机在流程中
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="flowKey"></param>
        /// <param name="status"></param>
        /// <param name="memberPhone"></param>
        /// <returns></returns>
        public bool ExistsMemberPhoneAction(string websiteOwner, string flowKey = "", string status = "", string memberKey = "", string memberUserId="")
        {
            return Get<FlowAction>(GetActionParamString(websiteOwner, flowKey: flowKey, status: status, memberKey: memberKey, memberUserId: memberUserId)) == null ? false : true;
        }

        /// <summary>
        /// 查询是否可处理
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="actId"></param>
        /// <param name="handleUserId"></param>
        /// <param name="handleGroupId"></param>
        /// <returns></returns>
        public bool IsCanAction(string websiteOwner, int actId, string handleUserId = "", string handleGroupId = "")
        {
            return Get<FlowAction>(GetActionParamString(websiteOwner, handleUserId: handleUserId, handleGroupId: handleGroupId, actId: actId))==null?false:true;
        }
        /// <summary>
        /// 拼接环节查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="actId"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public string GetActionDetailParamString(string websiteOwner, int actId, int flowId, int? stepId=null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            sbSql.AppendFormat(" And ActionID='{0}'", actId);
            sbSql.AppendFormat(" And FlowID='{0}'", flowId);
            if (stepId.HasValue) sbSql.AppendFormat(" And StepID='{0}'", stepId.Value);
            return sbSql.ToString();
        }
        /// <summary>
        /// 查询执行明细
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="actId"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public FlowActionDetail GetActionDetail(string websiteOwner, int actId, int flowId, int stepId)
        {
            return Get<FlowActionDetail>(GetActionDetailParamString(websiteOwner, actId, flowId, stepId));
        }

        public FlowActionDetail GetEndActionDetail(string websiteOwner, int actId, int flowId, int? stepId=null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(GetActionDetailParamString(websiteOwner, actId, flowId, stepId));
            sbSql.AppendFormat(" Order by AutoID desc ");
            return Get<FlowActionDetail>(sbSql.ToString());
        }

        /// <summary>
        /// 查询执行明细列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="actId"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public List<FlowActionDetail> GetActionDetails(string websiteOwner, int actId, int flowId)
        {
            return GetList<FlowActionDetail>(GetActionDetailParamString(websiteOwner, actId, flowId));
        }
        /// <summary>
        /// 查询执行附件列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="actId"></param>
        /// <param name="flowId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public List<FlowActionFile> GetActionFiles(string websiteOwner, int actId, int flowId, int? stepId=null)
        {
            return GetList<FlowActionFile>(GetActionDetailParamString(websiteOwner, actId, flowId, stepId));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="audit">审核意见：1通过 2不通过 3对申请取消的审核同意 4对申请取消的审核不同意 </param>
        /// <param name="act"></param>
        /// <param name="nowAct"></param>
        /// <param name="files"></param>
        /// <param name="stepNext"></param>
        /// <returns></returns>
        public bool Action(out string msg, int audit,string websiteOwner, FlowAction act, FlowActionDetail nowAct, List<FlowActionFile> files, FlowStep stepNext)
        {
            msg = "";
            if (stepNext != null)
            {
                #region 中间环节

                act.StepID = stepNext.AutoID;
                act.StepName = stepNext.StepName;
                if (audit == 2) act.Status = 8;
                BLLTransaction tran = new BLLTransaction();
                if (!Update(act, tran))
                {
                    msg = "处理失败";
                    tran.Rollback();
                    return false;
                }
                if (!Add(nowAct, tran))
                {
                    msg = "记录处理失败";
                    tran.Rollback();
                    return false;
                }
                foreach (var item in files)
                {
                    if (!Add(item, tran))
                    {
                        msg = "保存附件失败";
                        tran.Rollback();
                        return false;
                    }
                }
                msg = "审核完成";
                tran.Commit();
                return true;
                #endregion
            }
            else
            {
                bool hasProjectCommission = false;//分佣是否存在

                #region 结束环节

                act.StepID = 0;
                act.StepName = null;
                if (audit == 2) {  act.Status = 8; }
                if (audit == 1) {   act.Status = 9; }
                if (audit == 3) {
                    if (act.Status != 11)
                    {
                        msg = "当申请取消时，才同意取消";
                        return false;
                    }
                    act.Status = 10;
                }
                if (audit == 4)
                {
                    if (act.Status != 11)
                    {
                        msg = "当申请取消时，才拒绝取消";
                        return false;
                    }
                    act.Status = 12;
                }

                act.EndDate = nowAct.HandleDate;

                BLLUser bllUser = new BLLUser();
                BLLLog bllLog = new BLLLog();
                BLLDistribution bllDis = new BLLDistribution();
                BLLUserScoreDetailsInfo bllScore = new BLLUserScoreDetailsInfo();
                UserInfo member = bllUser.GetUserInfo(act.MemberID, websiteOwner);
                WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);

                BLLTransaction tran = new BLLTransaction();
                string smsString = "";
                if (act.Status == 8 && act.FlowKey == "RegisterOffLine")
                {
                    #region 线下注册

                    member.MemberApplyStatus = 2;
                    member.MemberLevel = 0;

                    bllScore.DeleteScore(websiteOwner, "TotalAmount", "", member.UserID);
                    bllLog.Add(EnumLogType.ShMember, EnumLogTypeAction.Delete, nowAct.HandleUserID, "审核不通过，删除财务明细，加回报单金额", targetID: member.UserID);
                    
                    member.AccountAmountEstimate += act.Amount;
                    member.TotalAmount += act.Amount;

                    if (!bllUser.Update(member, tran))
                    {
                        msg = "更新会员审核状态失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion 线下注册
                }
                else if (act.Status == 9 && act.FlowKey == "RegisterOffLine")
                {
                    #region 线下注册
                    BLLDistribution bllDistribution = new BLLDistribution();
                    UserLevelConfig levelConfig = bllDistribution.QueryUserLevel(websiteOwner, "DistributionOnLine", member.MemberLevel.ToString());
                    if (levelConfig == null)
                    {
                        msg = "注册会员等级未找到";
                        return false;
                    }
                    member = bllUser.GetUserInfoByPhone(act.MemberPhone, websiteOwner);
                    if (member == null)
                    {
                        msg = "该会员账号未找到";
                        return false;
                    }
                    member.MemberStartTime = nowAct.HandleDate;
                    member.MemberApplyStatus = 9;
                    member.RegisterWay = "线下";

                    StringBuilder sbSql = new StringBuilder();

                    UserInfo upUserLevel1 = null;//分销上一级
                    UserInfo upUserLevel2 = null;//分销上二级
                    UserInfo upUserLevel3 = null;//分销上三级

                    UserLevelConfig levelConfig1 = null;//分销上一级规则
                    UserLevelConfig levelConfig2 = null;//分销上二级规则
                    UserLevelConfig levelConfig3 = null;//分销上三级规则

                    ProjectCommission modelLevel1 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel1Info = new ScoreLockInfo();
                    ProjectCommission modelLevel1ex1 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel1ex1Info = new ScoreLockInfo();
                    ProjectCommission modelLevel2 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel2Info = new ScoreLockInfo();
                    ProjectCommission modelLevel3 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel3Info = new ScoreLockInfo();

                    int disLevel = 1;
                    if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;

                    //计算分佣
                    bllDistribution.ComputeTransfers(disLevel, member, act.AutoID.ToString(), act.Amount, websiteOwner, "线下注册", ref sbSql, ref upUserLevel1,
                        ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                        ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                        levelConfig.LevelString);

                    #region 记录分佣信息
                    if (modelLevel1.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel1Id = Convert.ToInt32(AddReturnID(modelLevel1, tran));
                        if (modelLevel1Id <= 0)
                        {
                            msg = "一级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                        scoreLockLevel1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1Info, tran));
                        if (scoreLockLevel1Info.AutoId <= 0)
                        {
                            msg = "一级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, websiteOwner, (double)scoreLockLevel1Info.Score,
                            scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                            scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(),
                            (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                            modelLevel1.CommissionUserId, tran,
                            ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel1.CommissionLevel) <= 0)
                        {
                            msg = "一级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel1ex1.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel1ex1Id = Convert.ToInt32(AddReturnID(modelLevel1ex1, tran));
                        if (modelLevel1ex1Id <= 0)
                        {
                            msg = "一级返购房补助失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
                        scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1ex1Info, tran));
                        if (scoreLockLevel1ex1Info.AutoId <= 0)
                        {
                            msg = "一级返购房补助冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, websiteOwner, (double)scoreLockLevel1ex1Info.Score,
                            scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                            scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(),
                            (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                            modelLevel1ex1.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel1ex1.CommissionLevel) <= 0)
                        {
                            msg = "一级返购房补助明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel2.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel2Id = Convert.ToInt32(AddReturnID(modelLevel2, tran));
                        if (modelLevel2Id <= 0)
                        {
                            msg = "二级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
                        scoreLockLevel2Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel2Info, tran));
                        if (scoreLockLevel2Info.AutoId <= 0)
                        {
                            msg = "二级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, websiteOwner, (double)scoreLockLevel2Info.Score,
                            scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
                            scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(), (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
                            modelLevel2.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel2.CommissionLevel) <= 0)
                        {
                            msg = "二级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel3.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel3Id = Convert.ToInt32(AddReturnID(modelLevel3, tran));
                        if (modelLevel3Id<=0)
                        {
                            msg = "三级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
                        scoreLockLevel3Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel3Info, tran));
                        if (scoreLockLevel3Info.AutoId <= 0)
                        {
                            msg = "三级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, websiteOwner, (double)scoreLockLevel3Info.Score,
                            scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
                            scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(), (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
                            modelLevel3.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel3.CommissionLevel) <= 0)
                        {
                            msg = "三级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (hasProjectCommission)
                    {
                        int result = BLLBase.ExecuteSql(sbSql.ToString(), tran);
                        if (result <= 0)
                        {
                            msg = "更新分佣账面金额失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    #endregion

                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)(act.Amount),
                        string.Format("线下注册审核到账", act.Amount), "TotalAmount", (double)(member.TotalAmount + act.Amount),
                        act.AutoID.ToString(), "线下注册充值", "", "", (double)act.Amount, 0, "",
                        tran, ex1:act.StartEx1,
                        ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                        ex5:"offline",isPrint:1) <= 0)
                    {
                        msg = "注册充值审核到账明细失败";
                        tran.Rollback();
                        return false;
                    }
                    member.AccountAmountEstimate += act.Amount;
                    member.TotalAmount += act.Amount;
                    if (!bllUser.Update(member, tran))
                    {
                        msg = "更新会员审核状态失败";
                        tran.Rollback();
                        return false;
                    }

                    #region 记录业绩明细
                    TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
                    perDetail.AddType = "注册";
                    perDetail.AddNote = "实单审核" + levelConfig.LevelString;
                    perDetail.AddTime = nowAct.HandleDate;
                    perDetail.DistributionOwner = member.DistributionOwner;
                    perDetail.UserId = member.UserID;
                    perDetail.UserName = member.TrueName;
                    perDetail.UserPhone = member.Phone;
                    perDetail.Performance = act.Amount;
                    string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
                    int yearMonth = Convert.ToInt32(yearMonthString);
                    perDetail.YearMonth = yearMonth;
                    perDetail.WebsiteOwner = websiteOwner;

                    if (!Add(perDetail, tran))
                    {
                        msg = "记录业绩明细失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion 记录业绩明细

                    #endregion 线下注册
                }
                else if (act.Status == 9 && act.FlowKey == "OfflineRecharge")
                {
                    #region 线下充值
                    if (bllUser.Update(member, string.Format("TotalAmount=ISNULL(TotalAmount,0)+{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0}", act.Amount),
                        string.Format("AutoID={0} And WebsiteOwner='{1}' ", member.AutoID, websiteOwner),
                        tran) <= 0)
                    {
                        msg = "更新用户金额失败";
                        tran.Rollback();
                        return false;
                    }
                    #region 记录余额明细
                    //自己的消费记录
                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)(act.Amount),
                        string.Format("线下充值{0}元", (double)act.Amount), "TotalAmount", (double)(member.TotalAmount + act.Amount),
                        act.AutoID.ToString(), "线下充值", "", "", (double)act.Amount, 0, "",
                        tran, ex1: act.StartEx1, ex5: "offline", isPrint: 1) <= 0)
                    {
                        msg = "充值明细失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion
                    #endregion
                }
                else if (act.Status == 9 && act.FlowKey == "OfflineUpgrade")
                {
                    #region 线下升级

                    if (member.MemberLevel != act.MemberLevel)
                    {
                        msg = "会员级别已变动请重新申请";
                        return false;
                    }
                    if (member.MemberLevel >= Convert.ToInt32(act.StartEx2))
                    {
                        msg = "所选级别低于会员当前级别";
                        return false;
                    }
                    decimal userTotalAmount = 0;
                    decimal needAmount = 0;
                    UserLevelConfig levelConfig = bllDis.QueryUserLevel(websiteOwner, "DistributionOnLine", member.MemberLevel.ToString());
                    userTotalAmount = Convert.ToDecimal(levelConfig.FromHistoryScore);
                    UserLevelConfig toLevelConfig = bllDis.QueryUserLevel(websiteOwner, "DistributionOnLine", act.StartEx2);
                    if (toLevelConfig == null)
                    {
                        msg = "等级未找到";
                        return false;
                    }
                    if (toLevelConfig.IsDisable == 1)
                    {
                        msg = "级别禁止升级";
                        return false;
                    }
                    needAmount = Convert.ToDecimal(toLevelConfig.FromHistoryScore);
                    decimal rechargeAmount = needAmount - userTotalAmount;
                    if (rechargeAmount < 0)
                    {
                        msg = "所选级别低于会员当前级别";
                        return false;
                    }

                    StringBuilder sbSql = new StringBuilder();

                    UserInfo upUserLevel1 = null;//分销上一级
                    UserInfo upUserLevel2 = null;//分销上二级
                    UserInfo upUserLevel3 = null;//分销上三级

                    UserLevelConfig levelConfig1 = null;//分销上一级规则
                    UserLevelConfig levelConfig2 = null;//分销上二级规则
                    UserLevelConfig levelConfig3 = null;//分销上三级规则

                    ProjectCommission modelLevel1 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel1Info = new ScoreLockInfo();
                    ProjectCommission modelLevel1ex1 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel1ex1Info = new ScoreLockInfo();
                    ProjectCommission modelLevel2 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel2Info = new ScoreLockInfo();
                    ProjectCommission modelLevel3 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel3Info = new ScoreLockInfo();

                    int disLevel = 1;
                    if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;

                    //计算分佣
                    bllDis.ComputeTransfers(disLevel, member, act.AutoID.ToString(), rechargeAmount, websiteOwner, "线下升级", ref sbSql, ref upUserLevel1,
                        ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                        ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                        levelConfig.LevelString);

                    #region 记录分佣信息
                    if (modelLevel1.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel1Id = Convert.ToInt32(AddReturnID(modelLevel1, tran));
                        if (modelLevel1Id <= 0)
                        {
                            msg = "一级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                        scoreLockLevel1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1Info, tran));
                        if (scoreLockLevel1Info.AutoId <= 0)
                        {
                            msg = "一级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, websiteOwner, (double)scoreLockLevel1Info.Score,
                            scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                            scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(),
                            (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                            modelLevel1.CommissionUserId, tran,
                            ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel1.CommissionLevel) <= 0)
                        {
                            msg = "一级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel1ex1.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel1ex1Id = Convert.ToInt32(AddReturnID(modelLevel1ex1, tran));
                        if (modelLevel1ex1Id <= 0)
                        {
                            msg = "一级返购房补助失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
                        scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1ex1Info, tran));
                        if (scoreLockLevel1ex1Info.AutoId <= 0)
                        {
                            msg = "一级返购房补助冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, websiteOwner, (double)scoreLockLevel1ex1Info.Score,
                            scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                            scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(),
                            (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                            modelLevel1ex1.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel1ex1.CommissionLevel) <= 0)
                        {
                            msg = "一级返购房补助明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel2.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel2Id = Convert.ToInt32(AddReturnID(modelLevel2, tran));
                        if (modelLevel2Id <= 0)
                        {
                            msg = "二级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
                        scoreLockLevel2Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel2Info, tran));
                        if (scoreLockLevel2Info.AutoId <= 0)
                        {
                            msg = "二级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, websiteOwner, (double)scoreLockLevel2Info.Score,
                            scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
                            scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(), (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
                            modelLevel2.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel2.CommissionLevel) <= 0)
                        {
                            msg = "二级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel3.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel3Id = Convert.ToInt32(AddReturnID(modelLevel3, tran));
                        if (modelLevel3Id <= 0)
                        {
                            msg = "三级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
                        scoreLockLevel3Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel3Info, tran));
                        if (scoreLockLevel3Info.AutoId <= 0)
                        {
                            msg = "三级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, websiteOwner, (double)scoreLockLevel3Info.Score,
                            scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
                            scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(), (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
                            modelLevel3.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel3.CommissionLevel) <= 0)
                        {
                            msg = "三级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (hasProjectCommission)
                    {
                        int result = BLLBase.ExecuteSql(sbSql.ToString(), tran);
                        if (result <= 0)
                        {
                            msg = "更新分佣账面金额失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    #endregion


                    if (bllUser.Update(member, string.Format("MemberLevel={0}", act.StartEx2),
                        string.Format("AutoID={0} And WebsiteOwner='{1}' ", member.AutoID, websiteOwner),
                        tran) <= 0)
                    {
                        msg = "更新用户级别失败";
                        tran.Rollback();
                        return false;
                    }
                    #region 记录余额明细
                    //自己的消费记录
                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)(rechargeAmount),
                        string.Format("线下充值{0}元", (double)rechargeAmount), "TotalAmount", (double)(member.TotalAmount + rechargeAmount),
                        act.AutoID.ToString(), "线下充值", "", "", (double)rechargeAmount, 0, "",
                        tran, ex1: act.StartEx1, ex5: "offline", isPrint: 1) <= 0)
                    {
                        msg = "充值明细失败";
                        tran.Rollback();
                        return false;
                    }

                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)(0 - rechargeAmount),
                        string.Format("{1}为{0}", toLevelConfig.LevelString, "线下升级"), "TotalAmount", (double)(member.TotalAmount),
                        "", "升级会员", "", "", (double)rechargeAmount, 0, "",
                        tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                        ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString) <= 0)
                    {
                        msg = "升级会员失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion

                    #endregion
                }
                else if (act.Status == 9 && act.FlowKey == "Withdraw")
                {
                    #region 修改明细可打印状态
                    if(bllUser.Update(new UserScoreDetailsInfo(),"IsPrint=1",
                        string.Format("WebsiteOwner='{0}' And UserID='{1}' And ScoreEvent='申请提现' And RelationID='{2}'",
                        websiteOwner, member.UserID, act.AutoID.ToString()),
                        tran) <= 0)
                    {
                        msg = "更新提现明细可打印状态失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion
                }
                else if (act.FlowKey == "Withdraw" && (act.Status == 8 || act.Status == 10))
                {
                    #region 提现
                    if (bllUser.Update(member, string.Format("TotalAmount=ISNULL(TotalAmount,0)+{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0}", act.Amount),
                        string.Format("AutoID={0} And WebsiteOwner='{1}' ", member.AutoID, websiteOwner),
                        tran) <= 0)
                    {
                        msg = "更新用户金额失败";
                        tran.Rollback();
                        return false;
                    }
                    #region 记录余额明细
                    //自己的消费记录
                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)(act.Amount),
                        string.Format("提现退款{0}{1}", (double)act.Amount, website.TotalAmountShowName), "TotalAmount", (double)(member.TotalAmount + act.Amount),
                        "", "提现退款", "", "", (double)act.Amount, (double)(0 - act.DeductAmount), "",
                        tran) <= 0)
                    {
                        msg = "提现退款明细失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion
                    #endregion
                }
                else if (act.Status == 9 && act.FlowKey == "CancelRegister")
                {
                    #region 撤单
                    if (member.MemberLevel == 0)
                    {
                        msg = "用户是会员或已撤单";
                        return false;
                    }
                    BLLDistribution bllDistribution = new BLLDistribution();
                    UserLevelConfig levelConfig = bllDistribution.QueryUserLevel(websiteOwner, "DistributionOnLine", member.MemberLevel.ToString());

                    if (!string.IsNullOrWhiteSpace(member.RegUserID) && member.RegisterWay=="余额" && nowAct.Ex1 == "1")
                    {
                        UserScoreDetailsInfo oRegLog = bllScore.GetNewScore(websiteOwner, "TotalAmount", "", member.UserID, "AutoID,Score", "他人注册转入");
                        if (oRegLog != null)
                        {
                            //记录撤单时会员金额
                            if (bllUser.AddScoreDetail(member.RegUserID, websiteOwner, oRegLog.Score,
                                string.Format("会员{0}[{1}]撤单退还注册费", member.TrueName,member.Phone),
                                "TotalAmount", (double)0,
                                "", "下级撤单", "", "", oRegLog.Score, (double)0, member.UserID,
                                tran) <= 0)
                            {
                                msg = "退积分给报单人记录明细失败";
                                tran.Rollback();
                                return false;
                            }
                            //积分加到账面
                            if (bllUser.Update(member, 
                                string.Format("TotalAmount=ISNULL(TotalAmount,0)+{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0}", oRegLog.Score),
                                string.Format("UserID='{0}' And WebsiteOwner='{1}'",
                                member.RegUserID, websiteOwner),
                                tran) <= 0)
                            {
                                msg = "退积分给报单人失败";
                                tran.Rollback();
                                return false;
                            }
                        }
                    }

                    //记录撤单时会员金额
                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)(0 - member.AccountAmountEstimate),
                        string.Format("会员余额清零"),
                        "TotalAmount", (double)0,
                        "", "撤单", "", "", (double)member.AccountAmountEstimate, (double)0, "",
                        tran) <= 0)
                    {
                        msg = "撤单金额清空";
                        tran.Rollback();
                        return false;
                    }

                    member.MemberApplyStatus = 0;
                    member.MemberLevel = 0;
                    //member.DistributionOwner = null;
                    member.TotalAmount = 0;
                    member.AccountAmountEstimate = 0;
                    member.AccumulationFund = 0;

                    if (!bllUser.Update(member, tran))
                    {
                        msg = "撤单";
                        tran.Rollback();
                        return false;
                    }
                    decimal logAmount = bllDistribution.GetPerformanceDetailSum(member.UserID, "", websiteOwner,0);
                    if (logAmount > 0)
                    {
                        hasProjectCommission = true;
                        #region 记录业绩明细
                        TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
                        perDetail.AddType = "撤单";
                        perDetail.AddNote = "撤单扣除历史累积";
                        perDetail.AddTime = nowAct.HandleDate;
                        perDetail.DistributionOwner = member.DistributionOwner;
                        perDetail.UserId = member.UserID;
                        perDetail.UserPhone = member.Phone;
                        perDetail.UserName = member.TrueName;
                        perDetail.Performance = (0 - logAmount);
                        perDetail.WebsiteOwner = websiteOwner;
                        string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
                        int yearMonth = Convert.ToInt32(yearMonthString);
                        perDetail.YearMonth = yearMonth;

                        if (!Add(perDetail, tran))
                        {
                            msg = "记录业绩明细失败";
                            tran.Rollback();
                            return false;
                        }

                        #endregion 记录业绩明细
                    }
                    #endregion
                }
                else if (act.Status == 8 && act.FlowKey == "CancelRegister")
                {
                    #region 撤单
                    if (bllUser.Update(member,
                        string.Format("IsDisable=0"),
                        string.Format("WebsiteOwner='{0}' And AutoID={1}", websiteOwner, member.AutoID),
                        tran) <= 0)
                    {
                        msg = "会员解冻失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion
                }
                else if (act.Status == 9 && act.FlowKey == "EmptyBilFill")
                {
                    #region 空单填满
                    BLLDistribution bllDistribution = new BLLDistribution();
                    UserLevelConfig levelConfig = bllDistribution.QueryUserLevel(websiteOwner, "DistributionOnLine", member.MemberLevel.ToString());
                    decimal amount = Convert.ToDecimal(levelConfig.FromHistoryScore);
                    if (levelConfig == null)
                    {
                        msg = "会员等级未找到";
                        return false;
                    }

                    member.MemberApplyStatus = 9;
                    member.MemberStartTime = nowAct.HandleDate;

                    StringBuilder sbSql = new StringBuilder();

                    UserInfo upUserLevel1 = null;//分销上一级
                    UserInfo upUserLevel2 = null;//分销上二级
                    UserInfo upUserLevel3 = null;//分销上三级

                    UserLevelConfig levelConfig1 = null;//分销上一级规则
                    UserLevelConfig levelConfig2 = null;//分销上二级规则
                    UserLevelConfig levelConfig3 = null;//分销上三级规则

                    ProjectCommission modelLevel1 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel1Info = new ScoreLockInfo();
                    ProjectCommission modelLevel1ex1 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel1ex1Info = new ScoreLockInfo();
                    ProjectCommission modelLevel2 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel2Info = new ScoreLockInfo();
                    ProjectCommission modelLevel3 = new ProjectCommission();
                    ScoreLockInfo scoreLockLevel3Info = new ScoreLockInfo();

                    int disLevel = 1;
                    if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;

                    //计算分佣
                    bllDistribution.ComputeTransfers(disLevel, member, act.AutoID.ToString(), amount, websiteOwner, "空单填满", ref sbSql, ref upUserLevel1,
                        ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                        ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                        levelConfig.LevelString);

                    #region 记录分佣信息
                    if (modelLevel1.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel1Id = Convert.ToInt32(AddReturnID(modelLevel1, tran));
                        if (modelLevel1Id <= 0)
                        {
                            msg = "一级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                        scoreLockLevel1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1Info, tran));
                        if (scoreLockLevel1Info.AutoId <= 0)
                        {
                            msg = "一级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, websiteOwner, (double)scoreLockLevel1Info.Score,
                            scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                            scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(),
                            (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                            modelLevel1.CommissionUserId, tran,
                            ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel1.CommissionLevel) <= 0)
                        {
                            msg = "一级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel1ex1.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel1ex1Id = Convert.ToInt32(AddReturnID(modelLevel1ex1, tran));
                        if (modelLevel1ex1Id <= 0)
                        {
                            msg = "一级返购房补助失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
                        scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1ex1Info, tran));
                        if (scoreLockLevel1ex1Info.AutoId <= 0)
                        {
                            msg = "一级返购房补助冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, websiteOwner, (double)scoreLockLevel1ex1Info.Score,
                            scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                            scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(),
                            (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                            modelLevel1ex1.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel1ex1.CommissionLevel) <= 0)
                        {
                            msg = "一级返购房补助明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel2.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel2Id = Convert.ToInt32(AddReturnID(modelLevel2, tran));
                        if (modelLevel2Id <= 0)
                        {
                            msg = "二级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
                        scoreLockLevel2Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel2Info, tran));
                        if (scoreLockLevel2Info.AutoId <= 0)
                        {
                            msg = "二级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, websiteOwner, (double)scoreLockLevel2Info.Score,
                            scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
                            scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(), (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
                            modelLevel2.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel2.CommissionLevel) <= 0)
                        {
                            msg = "二级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (modelLevel3.Amount > 0)
                    {
                        hasProjectCommission = true;
                        int modelLevel3Id = Convert.ToInt32(AddReturnID(modelLevel3, tran));
                        if (modelLevel3Id <= 0)
                        {
                            msg = "三级返利失败";
                            tran.Rollback();
                            return false;
                        }
                        scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
                        scoreLockLevel3Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel3Info, tran));
                        if (scoreLockLevel3Info.AutoId <= 0)
                        {
                            msg = "三级返利冻结失败";
                            tran.Rollback();
                            return false;
                        }
                        string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                        if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, websiteOwner, (double)scoreLockLevel3Info.Score,
                            scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
                            scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "", act.AutoID.ToString(), (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
                            modelLevel3.CommissionUserId,
                            tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                            ex5: modelLevel3.CommissionLevel) <= 0)
                        {
                            msg = "三级返利明细记录失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    if (hasProjectCommission)
                    {
                        int result = BLLBase.ExecuteSql(sbSql.ToString(), tran);
                        if (result <= 0)
                        {
                            msg = "更新分佣账面金额失败";
                            tran.Rollback();
                            return false;
                        }
                    }
                    #endregion

                    if (!bllUser.Update(member, tran))
                    {
                        msg = "修改会员状态出错";
                        tran.Rollback();
                        return false;
                    }

                    #region 记录业绩明细
                    TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
                    perDetail.AddType = "注册";
                    perDetail.AddNote = "空单填满" + levelConfig.LevelString;
                    perDetail.AddTime = nowAct.HandleDate;
                    perDetail.DistributionOwner = member.DistributionOwner;
                    perDetail.UserId = member.UserID;
                    perDetail.UserName = member.TrueName;
                    perDetail.UserPhone = member.Phone;
                    perDetail.Performance = amount;
                    perDetail.WebsiteOwner = websiteOwner;
                    string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
                    int yearMonth = Convert.ToInt32(yearMonthString);
                    perDetail.YearMonth = yearMonth;

                    if (!Add(perDetail, tran))
                    {
                        msg = "记录业绩明细失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion 记录业绩明细
                    #endregion 空单填满
                }
                else if (act.Status == 9 && act.FlowKey == "PerformanceReward")
                {
                    #region 管理奖票据
                    decimal trueAmount = nowAct.Ex1 == "1"?act.TrueAmount:0;

                    //记录撤单时会员金额
                    if (bllUser.AddScoreDetail(member.UserID, websiteOwner, (double)trueAmount,
                        string.Format("获得{0}管理奖", act.StartEx1),
                        "TotalAmount", (double)0,
                        "", "管理奖", "", "", (double)act.Amount, (double)act.DeductAmount, "",
                        tran) <= 0)
                    {
                        msg = "管理奖记录明细失败";
                        tran.Rollback();
                        return false;
                    }
                    //金额积分加到账面
                    if (bllUser.Update(member,
                        string.Format("TotalAmount=ISNULL(TotalAmount,0)+{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0},AccumulationFund=ISNULL(AccumulationFund,0)+{1}", 
                        trueAmount, act.DeductAmount),
                        string.Format("UserID='{0}' And WebsiteOwner='{1}'",
                        member.UserID, websiteOwner),
                        tran) <= 0)
                    {
                        msg = "管理奖发放失败";
                        tran.Rollback();
                        return false;
                    }
                    if (bllUser.Update(new TeamPerformance(),
                        string.Format("FlowActionStatus={0},Status=9", act.Status),
                        string.Format("AutoID={0}", act.RelationId), tran) <= 0)
                    {
                        msg = "业绩表票据审核状态更新失败";
                        tran.Rollback();
                        return false;
                    }
                    #endregion
                }

                if (!Update(act, tran))
                {
                    msg = "处理失败";
                    tran.Rollback();
                    return false;
                }
                if (!Add(nowAct, tran))
                {
                    msg = "记录处理失败";
                    tran.Rollback();
                    return false;
                }
                foreach (var item in files)
                {
                    if (!Add(item, tran))
                    {
                        msg = "保存附件失败";
                        tran.Rollback();
                        return false;
                    }
                }
                msg = "审核完成";
                tran.Commit();
                if (hasProjectCommission)
                {
                    //计算相关业绩
                    Thread th2 = new Thread(delegate()
                    {
                        bllDis.BuildCurMonthPerformanceByUserID(website.WebsiteOwner, member.UserID);
                    });
                    th2.Start();
                }
                if (act.Status == 9 && act.FlowKey == "RegisterOffLine")
                {
                    BLLSMS bllSms = new BLLSMS("");
                    bool smsBool = false;
                    bllSms.SendSmsMisson(member.Phone, smsString, "", website.SmsSignature, out smsBool, out msg);
                    if (!smsBool)
                    {
                        msg = "短信密码发送失败：" + msg;
                    }
                }
                else if (act.Status == 9 && act.FlowKey == "OfflineRecharge")
                {
                    //线下充值异步计算金额
                    Thread th1 = new Thread(delegate()
                    {
                        bllDis.CheckTotalAmount(member.AutoID,websiteOwner, 7);
                    });
                    th1.Start();
                }
                else if (act.Status == 9 && act.FlowKey == "CancelRegister")
                {
                    //异步修改积分明细表
                    Thread th1 = new Thread(delegate()
                    {
                        #region 撤单扣分佣
                        decimal unLockAmount = 0;
                        string rmsg = "";
                        bllDis.CancelLockAmount(member, website, out rmsg, out unLockAmount);
                        #endregion
                    });
                    th1.Start();
                }
                return true;
                #endregion
            }

        }
    }
}
