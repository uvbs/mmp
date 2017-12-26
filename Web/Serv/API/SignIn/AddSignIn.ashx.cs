using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.SignIn
{
    /// <summary>
    /// AddSignIn 的摘要说明
    /// </summary>
    public class AddSignIn : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 签到逻辑层
        /// </summary>
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];

            string id = context.Request["id"];

            string address = context.Request["address"];

            if (string.IsNullOrEmpty(type))
            {
                apiResp.msg = "未找到签到";
                bllSignIn.ContextResponse(context, apiResp);
                return;
            }

            UserInfo currUser = bllSignIn.GetCurrentUserInfo();

            DateTime dt = DateTime.Now;

            string dtStr = dt.ToString("yyyy/MM/dd");

            string startTime = GetMondayDate(dt).ToString("yyyy/MM/dd");
            
            SignInAddress signInAddress = bllSignIn.Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND Type='{1}'", bllSignIn.WebsiteOwner, type));

            int[] scores = new int[] { signInAddress.SundayScore, signInAddress.MondayScore, signInAddress.TuesdayScore, signInAddress.WednesdayScore, signInAddress.ThursdayScore, signInAddress.FridayScore, signInAddress.SaturdayScore };


            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            SignInLog model = bllSignIn.Get<SignInLog>(string.Format(" WebsiteOwner='{0}' AND UserId='{1}' AND Type='{2}' AND SignInDate='{3}'", bllSignIn.WebsiteOwner, bllSignIn.GetCurrUserID(), type, dtStr),tran);

            if (model == null)
            {
                model = new SignInLog();
                model.AddressId = Convert.ToInt32(id);
                model.Address = address;
                model.WebsiteOwner = bllSignIn.WebsiteOwner;
                model.UserID = bllSignIn.GetCurrUserID();
                model.CreateDate = dt;
                model.Remark = "周签到";
                model.Type = type;
                model.SignInDate = dt.ToString("yyyy/MM/dd");
                model.Status = 0;
                int addScore = scores[Convert.ToInt32(dt.DayOfWeek)];

                if (model.Type == "Sign")
                {
                    model.Ex1 = "签到获得" + addScore.ToString() + "积分,日期：" + dt.ToString("yyyy/MM/dd");
                }

                DateTime startWeek = GetMondayDate(dt);

                string starkWeekStr = startWeek.ToString("yyyy/MM/dd");//本周周一

                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                scoreRecord.UserID = currUser.UserID;
                scoreRecord.AddTime = dt;
                scoreRecord.TotalScore = currUser.TotalScore;
                scoreRecord.Score = addScore;
                scoreRecord.ScoreType = "SignIn";
                scoreRecord.AddNote = "签到";
                scoreRecord.RelationID = bllSignIn.GetCurrUserID();
                scoreRecord.WebSiteOwner = bllSignIn.WebsiteOwner;
                scoreRecord.Ex6 = "签到获得" + addScore + "积分,日期：" + dt.ToString("yyyy/MM/dd");

                try
                {
                    if (!bllSignIn.Add(model, tran))
                    {
                        tran.Rollback();
                        apiResp.msg = "签到失败";
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        bllSignIn.ContextResponse(context, apiResp);
                        return;
                    }
                    if (bllSignIn.Update(currUser, string.Format(" TotalScore=IsNull(TotalScore,0)+{0}", addScore), string.Format(" AutoId={0}",  currUser.AutoID), tran) <= 0)
                    {

                        tran.Rollback();
                        apiResp.msg = "添加积分失败";
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        bllSignIn.ContextResponse(context, apiResp);
                        return;
                    }
                    if (!bllSignIn.Add(scoreRecord, tran))
                    {
                        tran.Rollback();
                        apiResp.msg = "添加积分详情失败";
                        apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                        bllSignIn.ContextResponse(context, apiResp);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    apiResp.msg = "异常："+ex.Message;
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    bllSignIn.ContextResponse(context, apiResp);
                    return;
                }



                tran.Commit();
                apiResp.status = true;
                apiResp.msg = "签到成功";
            }
            else
            {
                apiResp.msg = "已签到";
            }
            bllSignIn.ContextResponse(context, apiResp);


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