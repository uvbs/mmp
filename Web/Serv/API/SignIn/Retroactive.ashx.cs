using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.SignIn
{
    /// <summary>
    /// Retroactive 补签
    /// </summary>
    public class Retroactive : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLSignIn bllSignIn = new BLLSignIn();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();

        public void ProcessRequest(HttpContext context)
        {

            string weekNameNumber = context.Request["week_name_date"];
            string type = context.Request["type"];
            string id = context.Request["id"];
            string address = context.Request["address"];

            UserInfo curUser = bllUser.GetCurrentUserInfo();// 当前用户
            string msg = string.Empty;

            SignInAddress signInAddress = bllSignIn.Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND Type='Sign' ", bllSignIn.WebsiteOwner));

            //补签积分数组
            int[] retroactives = new int[] { signInAddress.RetroactiveToOne, signInAddress.RetroactiveToTwo, signInAddress.RetroactiveToThree, signInAddress.RetroactiveToFour, signInAddress.RetroactiveToFive, signInAddress.RetroactiveToSix, signInAddress.RetroactiveToSeven };

            DateTime dt = DateTime.Now;  //当前时间
            DateTime startWeek = GetMondayDate(dt);
            string starkWeekStr = startWeek.ToString("yyyy/MM/dd");//本周周一

            int count = 0;
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
               
                SignInLog log = bllSignIn.Get<SignInLog>(string.Format(" WebsiteOwner='{0}' AND Type='Sign' AND UserId='{1}' AND SignInDate='{2}'", bllSignIn.WebsiteOwner, curUser.UserID, weekNameNumber), tran);
                if (log != null)
                {
                    apiResp.msg = "已存在记录";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    bllSignIn.ContextResponse(context, apiResp);
                    return;
                }
                 count = bllSignIn.GetCount<SignInLog>(string.Format(" WebsiteOwner='{0}' AND Status=1 AND SignInDate>='{1}' AND UserId='{2}' and Type='Sign' ", bllSignIn.WebsiteOwner, starkWeekStr, curUser.UserID));

                if (count > 6) count = 6;

                int score = retroactives[count];

                if (curUser.TotalScore < score)
                {
                    apiResp.msg = "积分不足";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    bllSignIn.ContextResponse(context, apiResp);
                    return;
                }

                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                scoreRecord.UserID = curUser.UserID;
                scoreRecord.AddTime = dt;
                scoreRecord.TotalScore = curUser.TotalScore;
                scoreRecord.Score = -score;
                scoreRecord.ScoreType = "SignIn";
                scoreRecord.AddNote = "补签";
                scoreRecord.RelationID = bllSignIn.GetCurrUserID();
                scoreRecord.WebSiteOwner = bllSignIn.WebsiteOwner;
                scoreRecord.Ex6 = "补签扣除" + score + "积分,日期：" + (string.IsNullOrEmpty(weekNameNumber) ? dt.ToString("yyyy/MM/dd") : weekNameNumber);

                //签到积分数据
                int[] scores = new int[] { signInAddress.SundayScore, signInAddress.MondayScore, signInAddress.TuesdayScore, signInAddress.WednesdayScore, signInAddress.ThursdayScore, signInAddress.FridayScore, signInAddress.SaturdayScore };

                log = new SignInLog();
                log.AddressId = Convert.ToInt32(id);
                log.Address = address;
                log.WebsiteOwner = bllSignIn.WebsiteOwner;
                log.UserID = curUser.UserID;
                log.CreateDate = dt;
                log.Remark = "补签";
                log.Type = type;
                log.SignInDate = weekNameNumber;
                log.Status = 1;
                int addScore = scores[Convert.ToInt32(dt.DayOfWeek)];
                if (!string.IsNullOrEmpty(weekNameNumber))
                {
                    short d = Convert.ToInt16((DateTime.Parse(weekNameNumber)).DayOfWeek.ToString("D"));
                    addScore = scores[d];
                }
                log.Ex1 = "补签获得" + addScore + "积分,扣除" + score + "积分,日期:" + (string.IsNullOrEmpty(weekNameNumber) ? dt.ToString("yyyy/MM/dd") : weekNameNumber);


                UserScoreDetailsInfo scoreRecord1 = new UserScoreDetailsInfo();
                scoreRecord1.UserID = curUser.UserID;
                scoreRecord1.AddTime = dt;
                scoreRecord1.TotalScore = curUser.TotalScore;
                scoreRecord1.Score = addScore;
                scoreRecord1.ScoreType = "SignIn";
                scoreRecord1.AddNote = "补签";
                scoreRecord1.RelationID = bllSignIn.GetCurrUserID();
                scoreRecord1.WebSiteOwner = bllSignIn.WebsiteOwner;
                scoreRecord1.Ex6 = "补签获得" + addScore + "积分,日期：" + (string.IsNullOrEmpty(weekNameNumber) ? dt.ToString("yyyy/MM/dd") : weekNameNumber);


                if (bllUser.Update(curUser, string.Format(" TotalScore=TotalScore+({0})", (addScore - score)), string.Format(" WebsiteOwner='{0}' AND  AutoID={1}", bllUser.WebsiteOwner, curUser.AutoID), tran) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "补签失败";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                if (!bllSignIn.Add(scoreRecord, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "积分详情添加失败";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }

                if (!bllSignIn.Add(log, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "添加签到数据失败";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                //if (bllSignIn.Update(curUser, string.Format(" TotalScore=TotalScore+{0}", addScore), string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bllSignIn.WebsiteOwner, curUser.AutoID), tran) <= 0)
                //{
                //    tran.Rollback();
                //    apiResp.msg = "增加积分失败";
                //    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                //}
                if (!bllSignIn.Add(scoreRecord1, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "积分详情添加失败";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }


                tran.Commit();
                msg = retroactives[6].ToString();
                if (count + 1 < 6) msg = retroactives[count + 1].ToString();

            }
            catch (Exception ex)
            {
                tran.Rollback();
                apiResp.msg = "异常" + ex.Message;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;

            }

            apiResp.status = true;
            apiResp.msg = msg;
            bllUser.ContextResponse(context, apiResp);
        }
        public static DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。 
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }

    }
}