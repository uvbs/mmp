using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// TotalAmountUpgrade 的摘要说明
    /// </summary>
    public class TotalAmountUpgrade : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            if (CurrentUserInfo.IsLock == 1)
            {
                apiResp.code = (int)APIErrCode.NoPms;
                apiResp.msg = "账号已被锁定";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.MemberApplyStatus != 9)
            {
                apiResp.code = (int)APIErrCode.NoPms;
                apiResp.msg = "您的账号正在审核中";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.API.User.PayUpgrade requestUpgrade = bll.ConvertRequestToModel<BLLJIMP.Model.API.User.PayUpgrade>(new BLLJIMP.Model.API.User.PayUpgrade());
            string websiteOwner = bll.WebsiteOwner;
            requestUpgrade.level=CurrentUserInfo.MemberLevel;
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", requestUpgrade.level.ToString());
            if (levelConfig == null)
            {
                requestUpgrade.userTotalAmount = 0;
            }
            else
            {
                requestUpgrade.userTotalAmount = Convert.ToDecimal(levelConfig.FromHistoryScore);
            }
            UserLevelConfig toLevelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", requestUpgrade.toLevel.ToString());
            if (toLevelConfig == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员等级未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (toLevelConfig.IsDisable == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员级别禁止升级";
                bll.ContextResponse(context, apiResp);
                return;
            }
            requestUpgrade.needAmount = Convert.ToDecimal(toLevelConfig.FromHistoryScore);
            requestUpgrade.amount = requestUpgrade.needAmount - requestUpgrade.userTotalAmount;
            if (requestUpgrade.amount < 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "暂时不支持降级";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.TotalAmount < requestUpgrade.amount)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = string.Format("您的可用{0}不足", website.TotalAmountShowName);
                bll.ContextResponse(context, apiResp);
                return;
            }

            BLLTransaction tran = new BLLTransaction();

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

            string projectId = bll.GetGUID(TransacType.PayRegisterOrder);
            CurrentUserInfo.MemberLevel = requestUpgrade.toLevel;
            CurrentUserInfo.MemberApplyStatus = 9;
            //计算分佣
            bll.ComputeTransfers(disLevel, CurrentUserInfo, projectId, requestUpgrade.amount, websiteOwner, "余额升级", ref sbSql, ref upUserLevel1,
                ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                toLevelConfig.LevelString);


            bool hasProjectCommission = false;
            #region 记录分佣信息
            if (modelLevel1.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel1Id = Convert.ToInt32(bll.AddReturnID(modelLevel1, tran));
                if (modelLevel1Id <= 0)
                {
                    apiResp.msg = "一级返利失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                scoreLockLevel1Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel1Info, tran));
                if (scoreLockLevel1Info.AutoId<=0)
                {
                    apiResp.msg = "一级返利冻结失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, websiteOwner, (double)scoreLockLevel1Info.Score,
                    scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                    scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", projectId, (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                    modelLevel1.CommissionUserId, tran,
                    ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel1.CommissionLevel) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "一级返利明细记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            if (modelLevel1ex1.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel1ex1Id = Convert.ToInt32(bll.AddReturnID(modelLevel1ex1, tran));
                if (modelLevel1ex1Id <= 0)
                {
                    apiResp.msg = "一级返购房补助失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
                scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel1ex1Info, tran));
                if (scoreLockLevel1ex1Info.AutoId<=0)
                {
                    apiResp.msg = "一级返购房补助冻结失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, websiteOwner, (double)scoreLockLevel1ex1Info.Score,
                    scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                    scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", projectId, (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                    modelLevel1ex1.CommissionUserId, tran,
                    ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel1ex1.CommissionLevel) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "一级返购房补助明细记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            if (modelLevel2.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel2Id = Convert.ToInt32(bll.AddReturnID(modelLevel2, tran));
                if (modelLevel2Id <= 0)
                {
                    apiResp.msg = "二级返利失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
                scoreLockLevel2Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel2Info, tran));
                if (scoreLockLevel2Info.AutoId<=0)
                {
                    tran.Rollback();
                    apiResp.msg = "二级返利冻结失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, websiteOwner, (double)scoreLockLevel2Info.Score,
                    scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
                    scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", projectId, (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
                    modelLevel2.CommissionUserId, tran,
                    ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel2.CommissionLevel) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "二级返利明细记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            if (modelLevel3.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel3Id = Convert.ToInt32(bll.AddReturnID(modelLevel3, tran));
                if (!bll.Add(modelLevel3, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "三级返利失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
                scoreLockLevel3Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel3Info, tran));
                if (scoreLockLevel3Info.AutoId<=0)
                {
                    tran.Rollback();
                    apiResp.msg = "三级返利冻结失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, websiteOwner, (double)scoreLockLevel3Info.Score,
                    scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
                    scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "", projectId, (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
                    modelLevel3.CommissionUserId, tran,
                    ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel3.CommissionLevel) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "三级返利明细记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            #endregion

            #region 更新分佣账面金额
            if (hasProjectCommission)
            {
                if (BLLBase.ExecuteSql(sbSql.ToString(), tran) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = string.Format("更新分佣账面{0}出错", website.TotalAmountShowName);
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            #endregion

            #region 记录余额明细
            //自己的消费记录
            if (bllUser.AddScoreDetail(CurrentUserInfo.UserID, websiteOwner, (double)(0 - requestUpgrade.amount),
                string.Format("{1}为{0}", toLevelConfig.LevelString, "升级"), "TotalAmount", (double)(CurrentUserInfo.TotalAmount - requestUpgrade.amount),
                "", "升级会员", "", "", (double)requestUpgrade.amount, 0, "",
                tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString) <= 0)
            {
                tran.Rollback();
                apiResp.msg = "升级会员明细出错";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }

            #endregion

            #region 修改金额 更新支付状态

            if (BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_UserInfo SET MemberLevel={0},IsDisable=0,MemberApplyStatus=9,TotalAmount=ISNULL(TotalAmount,0)-{3},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)-{3} FROM ZCJ_UserInfo WHERE UserID='{1}' And WebsiteOwner='{2}' And ISNULL(TotalAmount,0)-{3}>=0 ;",
                requestUpgrade.toLevel, CurrentUserInfo.UserID, websiteOwner, requestUpgrade.amount),
                tran) <= 0)
            {
                tran.Rollback();
                apiResp.msg = string.Format("扣除用户可用{0}出错", website.TotalAmountShowName);
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            #region 记录业绩明细
            TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
            perDetail.AddType = "升级";
            perDetail.AddNote = "由" + levelConfig.LevelString + "升级" + toLevelConfig.LevelString;
            perDetail.AddTime = DateTime.Now;
            perDetail.DistributionOwner = CurrentUserInfo.DistributionOwner;
            perDetail.UserId = CurrentUserInfo.UserID;
            perDetail.UserName = CurrentUserInfo.TrueName;
            perDetail.UserPhone = CurrentUserInfo.Phone;
            perDetail.Performance = requestUpgrade.amount;
            string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
            int yearMonth = Convert.ToInt32(yearMonthString);
            perDetail.WebsiteOwner = websiteOwner;
            perDetail.YearMonth = yearMonth;

            if (!bllUser.Add(perDetail, tran))
            {
                tran.Rollback();
                apiResp.msg = "记录业绩明细失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            tran.Commit();
            if (hasProjectCommission)
            {
                //异步修改积分明细表
                Thread th1 = new Thread(delegate()
                {
                    //计算相关业绩
                    bll.BuildCurMonthPerformanceByUserID(websiteOwner, CurrentUserInfo.UserID);
                });
                th1.Start();
            }
            apiResp.msg = "升级完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;

            bll.ContextResponse(context, apiResp);

        }
    }
}