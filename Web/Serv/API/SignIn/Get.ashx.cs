using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.SignIn
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLSignIn bllSignIn = new BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
           // bllSignIn.ToLog("API.SignIn", "D:\\log\\UserSignInLog.txt");

            string id = context.Request["id"];
            string type = context.Request["type"];
            SignInAddress signInAddress = new SignInAddress();
            if (string.IsNullOrEmpty(type))
            {
                signInAddress = bllSignIn.GetByKey<SignInAddress>("AutoID", id);
            }
            else
            {
                signInAddress = bllSignIn.Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND Type='{1}'", bllSignIn.WebsiteOwner, type));
            }
            if (signInAddress == null)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "地址未找到";
                bllSignIn.ContextResponse(context, apiResp);
                return;
            }
            
            DateTime dt = DateTime.Now;  //当前时间
            DateTime startWeek = GetMondayDate(dt);

            DateTime endWeek = startWeek.AddDays(6);
            
            string dayStartTime = DateTime.Now.ToString("yyyy/MM/dd");//一天开始时间 算出当天是否签到
            
            string starkWeekStr = startWeek.ToString("yyyy/MM/dd");//本周周一
            string endWeekStr = endWeek.ToString("yyyy/MM/dd");//本周周日

            List<SignInLog> signLogs = new List<SignInLog>();
            bool isSignin = false;
            int signInDay = 0;

            List<Weeks> weekList = new List<Weeks>();
           
            int rscore = 0;
            
            if (!string.IsNullOrEmpty(type) && type == "Sign")
            {

                string[] weekNames = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                int[] scores = new int[] { signInAddress.MondayScore, signInAddress.TuesdayScore, signInAddress.WednesdayScore, signInAddress.ThursdayScore, signInAddress.FridayScore, signInAddress.SaturdayScore, signInAddress.SundayScore };

                int[] retroactives = new int[] { signInAddress.RetroactiveToOne, signInAddress.RetroactiveToTwo, signInAddress.RetroactiveToThree, signInAddress.RetroactiveToFour, signInAddress.RetroactiveToFive, signInAddress.RetroactiveToSix, signInAddress.RetroactiveToSeven };

                int count = bllSignIn.GetCount<SignInLog>(string.Format(" WebsiteOwner='{0}' AND Status=1 AND SignInDate>='{1}' AND UserId='{2}' AND Type='{3}' ", bllSignIn.WebsiteOwner, starkWeekStr, bllSignIn.GetCurrUserID(), type));
                
                if (count > 6) count = 6;

                rscore = retroactives[count];

                signLogs = bllSignIn.GetList<SignInLog>(string.Format(" WebsiteOwner='{0}' AND Type='{1}' AND UserID='{2}' AND SignInDate>='{3}'", bllSignIn.WebsiteOwner, type, CurrentUserInfo.UserID, starkWeekStr));

                if (signLogs.Count > 7)
                {
                    signInDay = 7;
                }
                else
                {
                    signInDay = signLogs.Count;
                }

                
                
                if (signLogs.Exists(p => p.SignInDate == dayStartTime)) isSignin = true;

                for (int i = 0; i < 7; i++)
			    {
                    Weeks week = new Weeks();
                    DateTime rDate = startWeek.AddDays(i);
                    short d = Convert.ToInt16(rDate.DayOfWeek.ToString("D"));
                    week.week_name = weekNames[d];
                    week.is_signin = signLogs.Exists(p => p.SignInDate == rDate.ToString("yyyy/MM/dd"));
                    week.month_name = rDate.ToString("M月d号");
                    week.score = scores[i];
                    week.is_show = rDate >= dt ? false : true;
                    week.class_name = rDate.DayOfWeek.ToString();
                    week.week_name_date = rDate.ToString("yyyy/MM/dd");
                    week.week_day = d;
                    weekList.Add(week);
			    }
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                id = signInAddress.AutoID,
                address = signInAddress.Address,
                longitude = signInAddress.Longitude,
                latitude = signInAddress.Latitude,
                range = signInAddress.Range,
                isdelete = signInAddress.IsDelete,
                monday_score = signInAddress.MondayScore,
                tuesday_score = signInAddress.TuesdayScore,
                wednesday_score = signInAddress.WednesdayScore,
                thursday_score = signInAddress.ThursdayScore,
                friday_score = signInAddress.FridayScore,
                saturdayScore = signInAddress.SaturdayScore,
                sundayScore = signInAddress.SundayScore,
                lottery_type = signInAddress.LotteryType,
                lottery_id = signInAddress.LotteryId,
                desc = signInAddress.Description,
                type = signInAddress.Type,
                is_signin = isSignin,//false未签到 true已签到
                sign_day = signInDay,//签到天数
                button_color=signInAddress.ButtonColor,
                thumbnail=signInAddress.Thumbnail,
                retroactive_score = rscore,
                weeks=weekList,
                slide_name = signInAddress.SlideGroupName,
                background_image=signInAddress.BackGroundImage,//背景图片
                have_sign_image=signInAddress.HaveSignImage,//已签到显示图片
                no_have_sign_image=signInAddress.NoHaveSignImage,//未签到显示图片
                monday_ads=signInAddress.MondayAds,
                tuesday_ads=signInAddress.TuesdayAds,
                wednesday_ads=signInAddress.WednesdayAds,
                thursday_ads=signInAddress.ThursdayAds,
                friday_ads=signInAddress.FridayAds,
                saturday_ads=signInAddress.SaturdayAds,
                sunday_ads=signInAddress.SundayAds
            };
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

   


    public class Weeks
    {
        /// <summary>
        /// 如周一、周二
        /// </summary>
        public string week_name { get; set; }


        /// <summary>
        /// 如1月28号
        /// </summary>
        public string month_name { get; set; }

        /// <summary>
        /// 是否已签到
        /// </summary>
        public bool is_signin { get; set; }

        /// <summary>
        /// 当日积分
        /// </summary>
        public int score { get; set; }

        /// <summary>
        /// 未到时间不显示内容
        /// </summary>
        public bool is_show { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string class_name { get; set; }

        /// <summary>
        ///日期
        /// </summary>
        public string week_name_date { get; set; }

        /// <summary>
        /// 1，2，3，4
        /// </summary>
        public int week_day { get; set; }

    }
}