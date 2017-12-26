using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// QRCodeAdd 的摘要说明  扫码加入抽奖活动
    /// </summary>
    public class QRCodeAdd : BaseHandlerNeedLoginNoAction
    {
        protected BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        UserInfo curUser;
        public void ProcessRequest(HttpContext context)
        {


            string lotteryId=context.Request["lottery_id"];

            curUser =  bllUser.GetCurrentUserInfo();

            if (string.IsNullOrEmpty(lotteryId))
            {
                apiResp.msg = "抽奖活动id为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            BLLJIMP.Model.LotteryUserInfo model = bllUser.Get<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryId={1} AND UserId='{2}' ", bllUser.WebsiteOwner, lotteryId, curUser.UserID));
            if (model != null)
            {
                apiResp.msg = "您已参加抽奖";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            model = new BLLJIMP.Model.LotteryUserInfo();
            model.WebsiteOwner = bllUser.WebsiteOwner;
            model.CreateDate = DateTime.Now;
            model.WinnerDate = DateTime.Now;
            model.IsWinning = 0;
            model.LotteryId =Convert.ToInt32(lotteryId);
            model.UserId = curUser.UserID;
            model.WXHeadimgurl = curUser.WXHeadimgurl;
            model.WXNickname = bllUser.GetUserDispalyName(curUser);
            if (model.WXNickname.Trim() == string.Empty)
            {
                apiResp.msg = "无昵称不能加入";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Add(model))
            {
                int count = bllUser.GetCount<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}", bllUser.WebsiteOwner, lotteryId));
                bllUser.UpdateByKey<WXLotteryV1>("LotteryID", lotteryId, "WinnerCount", count.ToString());
                apiResp.msg = "加入成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "加入失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context,apiResp);
        }
    }
}