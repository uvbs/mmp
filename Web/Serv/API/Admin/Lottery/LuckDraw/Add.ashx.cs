using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LuckDraw
{
    /// <summary>
    /// Add 的摘要说明 添加抽奖
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string lotteryName=context.Request["lottery_name"];
            string lotteryType=context.Request["lottery_type"];
            string lotteryContent=context.Request["lottery_content"];
            string lotteryStatus=context.Request["lottery_status"];
            string isHideWinningList = context.Request["is_hideWinningList"];
            string backgroudImg = context.Request["backgroud_img"];
            string isHideQRCode=context.Request["ishide_qrcode"];
            string isHideTitle = context.Request["ishide_title"];
            string bgColor = context.Request["bgcolor"];
            string fontColor = context.Request["fontcolor"];
            string userBgcolor = context.Request["user_bgcolor"];
            string oneWinnerCount = context.Request["one_winnercount"];
            string qrcode=context.Request["qrcode"];
            string disUserId=context.Request["dis_userid"];//分销员用户id
            string startMusic=context.Request["start_music"];
            string stopMusic=context.Request["stop_music"];
            if (string.IsNullOrEmpty(lotteryName))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "必填选项为空";
                bllUser.ContextResponse(context, apiResp);
            }
            WXLotteryV1 lottery = new WXLotteryV1();
            lottery.LotteryID = int.Parse(bllUser.GetGUID(TransacType.DialogueID));
            lottery.LotteryName = lotteryName;
            lottery.LotteryType = lotteryType;
            lottery.LotteryContent = lotteryContent;
            lottery.WebsiteOwner = bllUser.WebsiteOwner;
            lottery.InsertDate = DateTime.Now;
            lottery.Status = Convert.ToInt32(lotteryStatus);
            lottery.IsHideWinningList = Convert.ToInt32(isHideWinningList);
            lottery.BackGroudImg = backgroudImg;
            lottery.IsHideQRCode =Convert.ToInt32(isHideQRCode);
            lottery.IsHideTitle = Convert.ToInt32(isHideTitle);
            lottery.BackGroundColor = bgColor;
            lottery.TitleFontColor = fontColor;
            lottery.UserBackGroudColor = userBgcolor;
            lottery.OneWinnerCount =Convert.ToInt32(oneWinnerCount);
            lottery.QRCode =Convert.ToInt32(qrcode);
            lottery.DistributorUserId = disUserId;
            lottery.StartMusic = startMusic;
            lottery.StopMusic = stopMusic;

            if (bllUser.Add(lottery))
            {
                apiResp.status = true;
                apiResp.msg = "添加成功";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "添加失败";
            }
            bllUser.ContextResponse(context, apiResp);


        }

        
    }
}