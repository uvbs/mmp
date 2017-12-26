using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SignIn.Address
{
    /// <summary>
    /// 添加签到地址
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLSignIn bllSignIn = new BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
            string address = context.Request["address"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            string range = context.Request["range"]; //范围(米)
            string sueccessUrl = context.Request["successurl"];
            string signintime = context.Request["signintime"];
            string type=context.Request["type"];
            string desc=context.Request["desc"];
            string lotteryId=context.Request["lottery_id"];
            string lotteryType = context.Request["lottery_type"];
            string mondayScore = context.Request["monday_score"];
            string tuesdayScore = context.Request["tuesday_score"];
            string wednesdayScore = context.Request["wednesday_score"];
            string thursdayScore = context.Request["thursday_score"];
            string fridayScore = context.Request["friday_score"];
            string saturdayScore = context.Request["saturday_score"];
            string sundayScore = context.Request["sunday_score"];
            string buttonColor=context.Request["button_color"];
            string thumbnail = context.Request["thumbnail"];
            string backgroundImg = context.Request["background_img"];
            string havaSignImage = context.Request["have_sign_image"];
            string noHavaSignImage = context.Request["no_have_sign_image"];
            string retroactiveToOne = context.Request["retroactive_one"];
            string retroactiveToTwo = context.Request["retroactive_two"];
            string retroactiveToThree = context.Request["retroactive_three"];
            string retroactiveToFour = context.Request["retroactive_four"];
            string retroactiveToFive = context.Request["retroactive_five"];
            string retroactiveToSix = context.Request["retroactive_six"];
            string retroactiveToSeven = context.Request["retroactive_seven"];
            string slideName = context.Request["slide_name"];
            string mondayAds = context.Request["monday_ads"];
            string tuesdayAds = context.Request["tuesday_ads"];
            string wednesdayAds = context.Request["wednesday_ads"];
            string thursdayAds = context.Request["thursday_ads"];
            string fridayAds = context.Request["friday_ads"];
            string saturdayAds = context.Request["saturday_ads"];
            string sundayAds = context.Request["sunday_ads"];

            SignInAddress signInAddress = new SignInAddress();
            signInAddress.Address = address;
            signInAddress.Longitude = longitude;
            signInAddress.Latitude = latitude;
            signInAddress.Range = Convert.ToDouble(range);
            signInAddress.WebsiteOwner = bllSignIn.WebsiteOwner;
            signInAddress.SignInSuccessUrl = sueccessUrl;
            signInAddress.SignInTime = signintime;
            signInAddress.Type = type;
            signInAddress.Description=desc;
            signInAddress.LotteryId=lotteryId;
            signInAddress.LotteryType = lotteryType;
            signInAddress.MondayScore = Convert.ToInt32(mondayScore);
            signInAddress.TuesdayScore = Convert.ToInt32(tuesdayScore);
            signInAddress.WednesdayScore = Convert.ToInt32(wednesdayScore);
            signInAddress.ThursdayScore = Convert.ToInt32(thursdayScore);
            signInAddress.FridayScore = Convert.ToInt32(fridayScore);
            signInAddress.SaturdayScore = Convert.ToInt32(saturdayScore);
            signInAddress.SundayScore = Convert.ToInt32(sundayScore);
            signInAddress.ButtonColor = buttonColor;
            signInAddress.Thumbnail = thumbnail;
            signInAddress.BackGroundImage = backgroundImg;
            signInAddress.HaveSignImage = havaSignImage;
            signInAddress.NoHaveSignImage = noHavaSignImage;
            signInAddress.RetroactiveToOne = Convert.ToInt32(retroactiveToOne);
            signInAddress.RetroactiveToTwo = Convert.ToInt32(retroactiveToTwo);
            signInAddress.RetroactiveToThree = Convert.ToInt32(retroactiveToThree);
            signInAddress.RetroactiveToFour = Convert.ToInt32(retroactiveToFour);
            signInAddress.RetroactiveToFive = Convert.ToInt32(retroactiveToFive);
            signInAddress.RetroactiveToSix = Convert.ToInt32(retroactiveToSix);
            signInAddress.RetroactiveToSeven = Convert.ToInt32(retroactiveToSeven);
            signInAddress.MondayAds = mondayAds;
            signInAddress.TuesdayAds = tuesdayAds;
            signInAddress.WednesdayAds = wednesdayAds;
            signInAddress.ThursdayAds = thursdayAds;
            signInAddress.FridayAds = fridayAds;
            signInAddress.SaturdayAds = saturdayAds;
            signInAddress.SundayAds = sundayAds;
            signInAddress.SlideGroupName = slideName;

            if (bllSignIn.Add(signInAddress))
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "添加完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "添加失败";
            }
            bllSignIn.ContextResponse(context, apiResp);
        }

    }
}