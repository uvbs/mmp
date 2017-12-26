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
    /// CouponRegister 的摘要说明 优惠券支付
    /// </summary>
    public class CouponRegister : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        BLLCardCoupon bllCardCoupon = new BLLCardCoupon();

        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.API.User.PayRegisterUser requestUser = bll.ConvertRequestToModel<BLLJIMP.Model.API.User.PayRegisterUser>(new BLLJIMP.Model.API.User.PayRegisterUser());
            string websiteOwner = bll.WebsiteOwner;
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
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
            decimal levelAmount = Convert.ToDecimal(levelConfig.FromHistoryScore);

            UserInfo spreadUser = bllUser.GetSpreadUser(requestUser.spreadid, websiteOwner);
            if (spreadUser == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "推荐人未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            requestUser.spreadid = spreadUser.UserID; //推荐人
            UserInfo regUser = bllUser.GetUserInfoByPhone(requestUser.phone, websiteOwner);
            if (regUser != null && regUser.MemberLevel >= 10)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该手机已注册会员";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (regUser != null && regUser.MemberLevel >= requestUser.level)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该会员有更高级别";
                bll.ContextResponse(context, apiResp);
                return;
            }

            string configV1CouponId = ZentCloud.Common.ConfigHelper.GetConfigString("YGBV1CouponId");
            string configV2CouponId = ZentCloud.Common.ConfigHelper.GetConfigString("YGBV2CouponId");
            string v1LevelNumber = ZentCloud.Common.ConfigHelper.GetConfigString("V1LevelNumber");
            string v2LevelNumber = ZentCloud.Common.ConfigHelper.GetConfigString("V2LevelNumber");
            string couponId = string.Empty;

            CardCoupons cardModel = null;
            MyCardCoupons myCardCoupon = null;
            if (requestUser.vType == "V1")
            {
                if (string.IsNullOrEmpty(configV1CouponId))
                {
                    apiResp.code = (int)APIErrCode.ContentNotFound;
                    apiResp.msg = "V1优惠券未配置";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                couponId = configV1CouponId;
                cardModel = bllCardCoupon.GetCardCoupon(Convert.ToInt32(couponId));
                myCardCoupon = bllCardCoupon.GetMyCardCouponMainId(Convert.ToInt32(couponId),CurrentUserInfo.UserID);
                if (cardModel == null || myCardCoupon == null)
                {
                    apiResp.code = (int)APIErrCode.ContentNotFound;
                    apiResp.msg = "优惠券不存在";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                if (requestUser.level.ToString() != v1LevelNumber)
                {
                    apiResp.code = (int)APIErrCode.ContentNotFound;
                    apiResp.msg = "优惠券不能用于此升级";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
            }
            if (requestUser.vType == "V2")
            {
                if (string.IsNullOrEmpty(configV2CouponId))
                {
                    apiResp.code = (int)APIErrCode.ContentNotFound;
                    apiResp.msg = "V2优惠券未配置";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                couponId = configV2CouponId;
                cardModel = bllCardCoupon.GetCardCoupon(Convert.ToInt32(couponId));
                myCardCoupon = bllCardCoupon.GetMyCardCouponMainId(Convert.ToInt32(couponId), CurrentUserInfo.UserID);
                if (cardModel == null || myCardCoupon == null)
                {
                    apiResp.code = (int)APIErrCode.ContentNotFound;
                    apiResp.msg = "优惠券不存在";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                if (requestUser.level.ToString() != v2LevelNumber)
                {
                    apiResp.code = (int)APIErrCode.ContentNotFound;
                    apiResp.msg = "优惠券不能用于此升级";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
            }
            if (regUser == null)
            {
                regUser = new UserInfo();
                regUser.UserID = string.Format("ZYUser{0}{1}", DateTime.Now.ToString("yyyyMMdd"), Guid.NewGuid().ToString("N").ToUpper());
                regUser.UserType = 2;
                regUser.WebsiteOwner = websiteOwner;
                regUser.Regtime = DateTime.Now;
                regUser.LastLoginDate = DateTime.Parse("1970-01-01");
            }
            regUser.TrueName = requestUser.truename;
            regUser.DistributionOwner = requestUser.spreadid;
            regUser.Phone = requestUser.phone;
            regUser.MemberLevel = requestUser.level;
            regUser.MemberApplyTime = DateTime.Now;
            regUser.MemberStartTime = DateTime.Now;
            regUser.MemberApplyStatus = 9;
            regUser.IdentityCard = requestUser.idcard;
            regUser.Province = requestUser.province;
            regUser.City = requestUser.city;
            regUser.District = requestUser.district;
            regUser.Town = requestUser.town;
            regUser.ProvinceCode = requestUser.provinceCode;
            regUser.CityCode = requestUser.cityCode;
            regUser.DistrictCode = requestUser.districtCode;
            regUser.TownCode = requestUser.townCode;
            regUser.RegIP = context.Request.UserHostAddress;//ip
            regUser.Password = ZentCloud.Common.Rand.Number(6);
            regUser.RegUserID = CurrentUserInfo.UserID;
            regUser.EmptyBill = 0;
            regUser.RegisterWay = "线上";
            regUser.IsDisable = 0;
            int disLevel = 1;
            if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;

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

            string projectId = bll.GetGUID(TransacType.PayRegisterOrder);
            //计算分佣
            bll.ComputeTransfers(disLevel, regUser, projectId, levelAmount, websiteOwner, "他人代替注册", ref sbSql, ref upUserLevel1,
                ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                levelConfig.LevelString);

            BLLTransaction tran = new BLLTransaction();

            #region 注册会员

            if (regUser.AutoID == 0)
            {
                regUser.AutoID = Convert.ToInt32(bllUser.AddReturnID(regUser, tran));
                if (regUser.AutoID <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "注册用户出错";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            else
            {
                if (!bllUser.Update(regUser, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "注册用户出错";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            #endregion

            #region  更新优惠券状态
            myCardCoupon.UseDate = DateTime.Now;
            myCardCoupon.Status = 1;
            if (!bllCardCoupon.Update(myCardCoupon, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "更新优惠券状态失败";
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            #region 消耗报单人金额

            if (bllUser.AddScoreDetail(CurrentUserInfo.UserID, websiteOwner, (double)(0 - levelAmount),
                string.Format("替{0}[{1}]注册{2}", regUser.TrueName, regUser.Phone, levelConfig.LevelString),
                "TotalAmount", (double)(CurrentUserInfo.TotalAmount - levelAmount),
                "", "替他人注册", "", "", (double)levelAmount, 0, regUser.UserID,
                tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString) <= 0)
            {
                tran.Rollback();
                apiResp.msg = "记录明细出错";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion

            #region 注册账号余额明细
            //自己的消费记录
            if (bllUser.AddScoreDetail(regUser.UserID, websiteOwner, (double)(levelAmount),
                string.Format("{0}[{1}]转入", CurrentUserInfo.TrueName, CurrentUserInfo.Phone, (double)levelAmount),
                "TotalAmount", (double)(levelAmount),
                "", "他人注册转入", "", "", (double)levelAmount, 0, CurrentUserInfo.UserID,
                tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString) <= 0)
            {
                tran.Rollback();
                apiResp.msg = "他人注册转入记录出错";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            int mainDetailId = bllUser.AddScoreDetail(regUser.UserID, websiteOwner, (double)(0 - levelAmount),
                string.Format("{0}[{1}]替您注册{2}", CurrentUserInfo.TrueName, CurrentUserInfo.Phone, levelConfig.LevelString),
                "TotalAmount", 0,
                "", "他人代替注册", "", "", (double)levelAmount, 0, CurrentUserInfo.UserID,
                tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString);
            if (mainDetailId <= 0)
            {
                tran.Rollback();
                apiResp.msg = "他人注册记录出错";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            bool hasProjectCommission = false;
            #region 记录分佣信息
            if (modelLevel1.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel1Id = Convert.ToInt32(bll.AddReturnID(modelLevel1, tran));
                if (modelLevel1Id <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "一级返利失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                scoreLockLevel1Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel1Info, tran));
                if (scoreLockLevel1Info.AutoId <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "一级返利冻结失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, websiteOwner, (double)scoreLockLevel1Info.Score,
                    scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                    scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", projectId, (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                    modelLevel1.CommissionUserId,
                    tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
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
                    tran.Rollback();
                    apiResp.msg = "一级返购房补助失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();

                scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel1ex1Info, tran));
                if (scoreLockLevel1ex1Info.AutoId <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "一级返购房补助冻结失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, websiteOwner, (double)scoreLockLevel1ex1Info.Score,
                    scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                    scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", projectId, (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                    modelLevel1ex1.CommissionUserId,
                    tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
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
                    tran.Rollback();
                    apiResp.msg = "二级返利失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();

                scoreLockLevel2Info.AutoId = Convert.ToInt32(bll.AddReturnID(scoreLockLevel2Info, tran));
                if (scoreLockLevel2Info.AutoId <= 0)
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
                    modelLevel2.CommissionUserId,
                    tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
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
                if (scoreLockLevel3Info.AutoId <= 0)
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
                    modelLevel3.CommissionUserId,
                    tran, ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                    ex5: modelLevel3.CommissionLevel) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "三级返利明细记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
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

            #region 记录业绩明细
            TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
            perDetail.AddType = "注册";
            perDetail.AddNote = "注册" + levelConfig.LevelString;
            perDetail.AddTime = DateTime.Now;
            perDetail.DistributionOwner = regUser.DistributionOwner;
            perDetail.UserId = regUser.UserID;
            perDetail.UserName = regUser.TrueName;
            perDetail.UserPhone = regUser.Phone;
            perDetail.Performance = levelAmount;
            string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
            int yearMonth = Convert.ToInt32(yearMonthString);
            perDetail.YearMonth = yearMonth;
            perDetail.WebsiteOwner = websiteOwner;

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
                    bll.BuildCurMonthPerformanceByUserID(websiteOwner, regUser.UserID);
                });
                th1.Start();
            }
            string msg = "";

            #region 短信发送密码
            BLLSMS bllSms = new BLLSMS("");
            bool smsBool = false;
            string smsString = string.Format("恭喜您成功注册为天下华商月供宝：{1}，您的初始密码为：{0}。您可关注公众号：songhetz，登录账户修改密码，并设置支付密码。", regUser.Password, levelConfig.LevelString);
            bllSms.SendSmsMisson(regUser.Phone, smsString, "", website.SmsSignature, out smsBool, out msg);
            #endregion

            if (string.IsNullOrWhiteSpace(msg)) msg = "注册成功";
            apiResp.msg = msg;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = new
            {
                password = regUser.Password
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}