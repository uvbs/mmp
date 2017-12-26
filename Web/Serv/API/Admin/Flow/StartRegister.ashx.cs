using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Flow
{
    /// <summary>
    /// Start 的摘要说明
    /// </summary>
    public class StartRegister : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.API.User.PayRegisterUser requestUser = bll.ConvertRequestToModel<BLLJIMP.Model.API.User.PayRegisterUser>(new BLLJIMP.Model.API.User.PayRegisterUser());
            string websiteOwner = bll.WebsiteOwner;

            BLLJIMP.Model.Flow flow = bllFlow.GetFlowByKey(requestUser.flow_key, websiteOwner);
            if (flow == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "流程未定义";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            if (flow.IsDelete == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = flow.FlowName + "已停用";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            List<BLLJIMP.Model.FlowStep> steps = bllFlow.GetStepList(2, 1, websiteOwner, flow.AutoID);
            if (steps.Count == 0)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = flow.FlowName + "环节未设置";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }

            if (string.IsNullOrWhiteSpace(requestUser.level.ToString()))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请选择会员级别";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestUser.phone))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入手机号码";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestUser.spreadid))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入推荐人编号";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(requestUser.phone))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "手机号码格式不正确";
                bll.ContextResponse(context, apiResp);
                return;
            }

            if (!ZentCloud.Common.MyRegex.IsIDCard(requestUser.idcard))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "身份证号码必须如实填写";
                bll.ContextResponse(context, apiResp);
                return;
            }
            UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", requestUser.level.ToString());
            if (levelConfig == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员级别未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (levelConfig.IsDisable == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员级别禁止注册";
                bll.ContextResponse(context, apiResp);
                return;
            }
            requestUser.levelname = levelConfig.LevelString;
            UserInfo spreadUser = bllUser.GetSpreadUser(requestUser.spreadid, websiteOwner);
            if (spreadUser == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "推荐人未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            requestUser.spreadid = spreadUser.UserID; //推荐人

            if (bllFlow.ExistsMemberPhoneAction(websiteOwner, flow.FlowKey, "0", requestUser.phone))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该手机用户正在申请会员";
                bll.ContextResponse(context, apiResp);
                return;
            }
            UserInfo oldUserInfo = bllUser.GetUserInfoByPhone(requestUser.phone, websiteOwner);
            if (oldUserInfo != null && oldUserInfo.MemberLevel >= 10)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该手机已注册会员";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (oldUserInfo != null && oldUserInfo.MemberLevel > requestUser.level)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该会员有更高级别";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (oldUserInfo == null)
            {
                oldUserInfo = new UserInfo();
                oldUserInfo.UserID = string.Format("ZYUser{0}{1}", DateTime.Now.ToString("yyyyMMdd"), Guid.NewGuid().ToString("N").ToUpper());
                oldUserInfo.Regtime = DateTime.Now;
            }
            oldUserInfo.RegIP = context.Request.UserHostAddress;//ip
            oldUserInfo.Password = ZentCloud.Common.Rand.Number(6);
            oldUserInfo.UserType = 2;
            oldUserInfo.TrueName = requestUser.truename;
            oldUserInfo.WebsiteOwner = websiteOwner;
            oldUserInfo.DistributionOwner = requestUser.spreadid;
            oldUserInfo.Phone = requestUser.phone;
            oldUserInfo.MemberApplyTime = DateTime.Now;
            oldUserInfo.MemberLevel = levelConfig.LevelNumber;
            oldUserInfo.LastLoginDate = DateTime.Parse("1970-01-01");
            oldUserInfo.IdentityCard = requestUser.idcard;
            oldUserInfo.Province = requestUser.province;
            oldUserInfo.City = requestUser.city;
            oldUserInfo.District = requestUser.district;
            oldUserInfo.Town = requestUser.town;
            oldUserInfo.ProvinceCode = requestUser.provinceCode;
            oldUserInfo.CityCode = requestUser.cityCode;
            oldUserInfo.DistrictCode = requestUser.districtCode;
            oldUserInfo.TownCode = requestUser.townCode;
            oldUserInfo.RegUserID = currentUserInfo.UserID;
            if (flow.FlowKey == "RegisterEmptyBill")
            {
                oldUserInfo.EmptyBill = 1;
                oldUserInfo.MemberApplyStatus = 0;
            }
            else
            {
                oldUserInfo.EmptyBill = 0;
                oldUserInfo.MemberApplyStatus = 1;
            }
            oldUserInfo.RegisterWay = "线下";
            oldUserInfo.IsDisable = 0;

            BLLJIMP.Model.FlowStep step1 = steps[0];
            BLLJIMP.Model.FlowStep step2 = null;
            if (steps.Count == 2) step2 = steps[1];

            BLLJIMP.Model.FlowAction action = new BLLJIMP.Model.FlowAction();
            action.CreateDate = DateTime.Now;
            action.CreateUserID = currentUserInfo.UserID;
            action.WebsiteOwner = websiteOwner;
            action.StartStepID = step1.AutoID;
            action.FlowID = flow.AutoID;
            action.FlowKey = flow.FlowKey;
            action.StartEx2 = JsonConvert.SerializeObject(requestUser);
            if (!string.IsNullOrWhiteSpace(requestUser.content)) action.StartContent = requestUser.content;
            if (!string.IsNullOrWhiteSpace(requestUser.ex1)) action.StartEx1 = requestUser.ex1;

            action.MemberAutoID = 0;
            action.MemberID = oldUserInfo.UserID;
            action.MemberName = oldUserInfo.TrueName;
            action.MemberPhone = oldUserInfo.Phone;
            action.MemberLevel = levelConfig.LevelNumber;
            action.MemberLevelName = levelConfig.LevelString;

            action.FlowName = flow.FlowName;
            action.Amount = Convert.ToDecimal(levelConfig.FromHistoryScore);
            if (step2 != null)
            {
                action.StepID = step2.AutoID;
                action.StepName = step2.StepName;
            }
            else
            {
                action.Status = 9;
                action.EndDate = DateTime.Now;
            }
            BLLJIMP.Model.FlowActionDetail actionDetail1 = new BLLJIMP.Model.FlowActionDetail();
            actionDetail1.WebsiteOwner = websiteOwner;
            actionDetail1.FlowID = flow.AutoID;
            actionDetail1.StepID = step1.AutoID;
            actionDetail1.StepName = step1.StepName;
            actionDetail1.HandleUserID =currentUserInfo.UserID;
            actionDetail1.HandleDate = DateTime.Now;
            actionDetail1.Ex2 = action.StartEx2;
            if (!string.IsNullOrWhiteSpace(action.StartContent)) actionDetail1.HandleContent = action.StartContent;
            if (!string.IsNullOrWhiteSpace(action.StartEx1)) actionDetail1.Ex1 = action.StartEx1;

            List<BLLJIMP.Model.FlowActionFile> files = new List<BLLJIMP.Model.FlowActionFile>();
            List<string> fileUrls = requestUser.files.Split(',').Where(p=> !string.IsNullOrWhiteSpace(p)).ToList();
            if (fileUrls.Count > 0)
            {
                foreach (var item in fileUrls)
                {
                    files.Add(new BLLJIMP.Model.FlowActionFile()
                    {
                        FlowID = flow.AutoID,
                        StepID = step1.AutoID,
                        WebsiteOwner = websiteOwner,
                        FilePath = item
                    });
                }
                if (fileUrls.Count > 0) oldUserInfo.Ex6 = fileUrls[0];
                if (fileUrls.Count > 1) oldUserInfo.Ex7 = fileUrls[1];
                if (fileUrls.Count > 2) oldUserInfo.Ex8 = fileUrls[2];
                if (fileUrls.Count > 3) oldUserInfo.Ex9 = fileUrls[3];
                if (fileUrls.Count > 4) oldUserInfo.Ex10 = fileUrls[4];
            }


            //WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            //int disLevel = 1;
            //if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;

            //StringBuilder sbSql = new StringBuilder();

            //bool hasProjectCommission = false;//分佣是否存在
            //UserInfo upUserLevel1 = null;//分销上一级
            //UserInfo upUserLevel2 = null;//分销上二级
            //UserInfo upUserLevel3 = null;//分销上三级

            //UserLevelConfig levelConfig1 = null;//分销上一级规则
            //UserLevelConfig levelConfig2 = null;//分销上二级规则
            //UserLevelConfig levelConfig3 = null;//分销上三级规则

            //ProjectCommission modelLevel1 = new ProjectCommission();
            //ScoreLockInfo scoreLockLevel1Info = new ScoreLockInfo();
            //ProjectCommission modelLevel1ex1 = new ProjectCommission();
            //ScoreLockInfo scoreLockLevel1ex1Info = new ScoreLockInfo();
            //ProjectCommission modelLevel2 = new ProjectCommission();
            //ScoreLockInfo scoreLockLevel2Info = new ScoreLockInfo();
            //ProjectCommission modelLevel3 = new ProjectCommission();
            //ScoreLockInfo scoreLockLevel3Info = new ScoreLockInfo();

            //string guId = bll.GetGUID(TransacType.OfflineRegister);
            ////计算分佣
            //bll.ComputeTransfers(disLevel, oldUserInfo, guId, action.Amount, websiteOwner, "线下注册", ref sbSql, ref upUserLevel1,
            //    ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
            //    ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
            //    levelConfig.LevelString);

            BLLTransaction tran = new BLLTransaction();
            //线下注册，扣钱，（实单审核后返）
            oldUserInfo.AccountAmountEstimate -= action.Amount;
            oldUserInfo.TotalAmount -= action.Amount;

            if (oldUserInfo.AutoID == 0)
            {
                oldUserInfo.AutoID = Convert.ToInt32(bllFlow.AddReturnID(oldUserInfo, tran));
                if (oldUserInfo.AutoID <= 0)
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "注册失败";
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
                action.MemberAutoID = oldUserInfo.AutoID;
            }
            else
            {
                if (!bllUser.Update(oldUserInfo, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "注册失败";
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
                action.MemberAutoID = oldUserInfo.AutoID;
            }
            int rId = Convert.ToInt32(bllFlow.AddReturnID(action, tran));
            if (rId <= 0)
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = flow.FlowName + "失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }

            #region 记录分佣信息
            //if (modelLevel1.Amount > 0)
            //{
            //    hasProjectCommission = true;
            //    modelLevel1.ProjectId = rId;
            //    int modelLevel1Id = Convert.ToInt32(bll.AddReturnID(modelLevel1, tran));
            //    if (modelLevel1Id <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "一级返利失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
            //    scoreLockLevel1Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel1Info, tran));
            //    if (scoreLockLevel1Info.AutoId<=0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "一级返利冻结失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
            //    if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, websiteOwner, (double)scoreLockLevel1Info.Score,
            //        scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
            //        scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", rId.ToString(), 
            //        (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
            //        modelLevel1.CommissionUserId, tran,
            //        ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
            //        ex5: modelLevel1.CommissionLevel) <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "一级返利明细记录失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //}
            //if (modelLevel1ex1.Amount > 0)
            //{
            //    hasProjectCommission = true;
            //    modelLevel1ex1.ProjectId = rId;
            //    int modelLevel1ex1Id = Convert.ToInt32(bll.AddReturnID(modelLevel1ex1, tran));
            //    if (modelLevel1ex1Id <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "一级返购房补助失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
            //    scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel1ex1Info, tran));
            //    if (scoreLockLevel1ex1Info.AutoId<=0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "一级返购房补助冻结失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
            //    if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, websiteOwner, (double)scoreLockLevel1ex1Info.Score,
            //        scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
            //        scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", rId.ToString(), 
            //        (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
            //        modelLevel1ex1.CommissionUserId,
            //        tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
            //        ex5: modelLevel1ex1.CommissionLevel) <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "一级返购房补助明细记录失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //}
            //if (modelLevel2.Amount > 0)
            //{
            //    hasProjectCommission = true;
            //    modelLevel2.ProjectId = rId;
            //    int modelLevel2Id = Convert.ToInt32(bll.AddReturnID(modelLevel2, tran));
            //    if (modelLevel2Id <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "二级返利失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
            //    scoreLockLevel2Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel2Info, tran));
            //    if (scoreLockLevel2Info.AutoId<=0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "二级返利冻结失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
            //    if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, websiteOwner, (double)scoreLockLevel2Info.Score,
            //        scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
            //        scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", rId.ToString(), (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
            //        modelLevel2.CommissionUserId,
            //        tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
            //        ex5: modelLevel2.CommissionLevel) <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "二级返利明细记录失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //}
            //if (modelLevel3.Amount > 0)
            //{
            //    hasProjectCommission = true;
            //    modelLevel3.ProjectId = rId;
            //    int modelLevel3Id = Convert.ToInt32(bll.AddReturnID(modelLevel3, tran));
            //    if (modelLevel3Id<=0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "三级返利失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
            //    scoreLockLevel3Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel3Info, tran));
            //    if (scoreLockLevel3Info.AutoId<=0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "三级返利冻结失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
            //    if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, websiteOwner, (double)scoreLockLevel3Info.Score,
            //        scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
            //        scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "" ,rId.ToString(), (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
            //        modelLevel3.CommissionUserId,
            //        tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
            //        ex5: modelLevel3.CommissionLevel) <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "三级返利明细记录失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //}
            //if (hasProjectCommission)
            //{
            //    int result = BLLBase.ExecuteSql(sbSql.ToString(), tran);
            //    if (result <= 0)
            //    {
            //        tran.Rollback();
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "更新分佣账面金额失败";
            //        bllFlow.ContextResponse(context, apiResp);
            //        return;
            //    }
            //}
            #endregion

            #region 记录余额明细
            //自己的消费记录
            string note = flow.FlowKey == "RegisterEmptyBill" ? "线下注册空单" : "线下注册";
            int mainDetailId = bllUser.AddScoreDetail(oldUserInfo.UserID, websiteOwner, (double)(0 - action.Amount),
                string.Format("{1}{0}", levelConfig.LevelString, note), "TotalAmount", (double)oldUserInfo.TotalAmount,
                "", "注册会员", "", rId.ToString(), (double)action.Amount, 0, "",
                tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString);
            if (mainDetailId <= 0)
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "注册明细失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            actionDetail1.ActionID = rId;
            if (!bllFlow.Add(actionDetail1, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = flow.FlowName + "，记录失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            foreach (var item in files)
            {
                item.ActionID = rId;
                if (!bllFlow.Add(item, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = flow.FlowName + "，保存附件失败";
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();

            string msg = "";
            #region 短信发送密码
            BLLSMS bllSms = new BLLSMS("");
            bool smsBool = false;
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            string smsString = string.Format("恭喜您成功注册为天下华商月供宝：{1}，您的初始密码为：{0}。您可关注公众号：songhetz，登录账户修改密码，并设置支付密码。", oldUserInfo.Password, levelConfig.LevelString);
            bllSms.SendSmsMisson(oldUserInfo.Phone, smsString, "", website.SmsSignature, out smsBool, out msg);
            #endregion

            apiResp.msg = "注册成功" + (smsBool ? "" : "，但短信发送失败：" + msg);
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllFlow.ContextResponse(context, apiResp);

        }
    }
}