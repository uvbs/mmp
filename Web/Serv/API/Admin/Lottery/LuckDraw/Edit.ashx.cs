using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LuckDraw
{
    /// <summary>
    /// Edit 的摘要说明  编辑抽奖
    /// </summary>
    public class Edit : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string lotteryId = context.Request["lottery_id"];
            string lotteryName = context.Request["lottery_name"];
            string lotteryContent = context.Request["lottery_content"];
            string lotteryStatus = context.Request["lottery_status"];
            string isHideWinningList = context.Request["is_hideWinningList"];
            string backgroudImg = context.Request["backgroud_img"];
            string isHideQRCode = context.Request["ishide_qrcode"];
            string isHideTitle = context.Request["ishide_title"];
            string bgColor = context.Request["bgcolor"];
            string fontColor = context.Request["fontcolor"];
            string userBgcolor = context.Request["user_bgcolor"];
            string oneWinnerCount = context.Request["one_winnercount"];
            string qrcode = context.Request["qrcode"];
            string disUserId = context.Request["dis_userid"];//分销员用户id
            string startMusic = context.Request["start_music"];
            string stopMusic = context.Request["stop_music"];
            if (string.IsNullOrEmpty(lotteryId)||string.IsNullOrEmpty(lotteryName))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "必填选项为空";
                bllUser.ContextResponse(context, apiResp);
            }

            WXLotteryV1 lottery = bllUser.Get<WXLotteryV1>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}", bllUser.WebsiteOwner, lotteryId));
            if (lottery == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "活动抽奖不存在";
                bllUser.ContextResponse(context, apiResp);
            }
            lottery.LotteryName = lotteryName;
            lottery.LotteryContent = lotteryContent;
            lottery.WebsiteOwner = bllUser.WebsiteOwner;
            lottery.InsertDate = DateTime.Now;
            lottery.Status = Convert.ToInt32(lotteryStatus);
            lottery.IsHideWinningList = Convert.ToInt32(isHideWinningList);
            lottery.BackGroudImg = backgroudImg;
            lottery.IsHideQRCode = Convert.ToInt32(isHideQRCode);
            lottery.IsHideTitle = Convert.ToInt32(isHideTitle);
            lottery.BackGroundColor = bgColor;
            lottery.TitleFontColor = fontColor;
            lottery.UserBackGroudColor = userBgcolor;
            lottery.OneWinnerCount = Convert.ToInt32(oneWinnerCount);
            lottery.QRCode = Convert.ToInt32(qrcode);
            lottery.DistributorUserId = disUserId;
            lottery.StartMusic = startMusic;
            lottery.StopMusic = stopMusic;
            if (bllUser.Update(lottery))
            {
                apiResp.status = true;
                apiResp.msg = "编辑成功";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "编辑失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }

        
    }
}