using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Lottery.LotteryUserInfo
{
    /// <summary>
    /// Add 的摘要说明  添加抽奖参与者
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string autoIds = context.Request["uids"];
            string lotteryId = context.Request["lottery_id"];
            if (string.IsNullOrEmpty(lotteryId))
            {
                apiResp.msg = "活动抽奖id为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            string[] ids = autoIds.Split(',');
            bool result = false;
            foreach (var item in ids)
            {
                UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(item));
                if (userInfo == null) continue;
                BLLJIMP.Model.LotteryUserInfo model = bllUser.Get<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryId={1} AND UserId='{2}' ", bllUser.WebsiteOwner, lotteryId, userInfo.UserID));
                if (model == null)
                {
                    model = new BLLJIMP.Model.LotteryUserInfo();
                    model.WebsiteOwner = bllUser.WebsiteOwner;
                    model.WXHeadimgurl = !string.IsNullOrEmpty(userInfo.WXHeadimgurl) ? userInfo.WXHeadimgurl : "http://file.comeoncloud.net/img/europejobsites.png";
                    model.WXNickname = bllUser.GetUserDispalyName(userInfo);
                    model.UserId = userInfo.UserID;
                    model.CreateDate = DateTime.Now;
                    model.WinnerDate = DateTime.Now;
                    model.LotteryId = Convert.ToInt32(lotteryId);
                    result=bllUser.Add(model);
                }
                else
                    result = true;
                
            }
            if (result)
            {
                int count=bllUser.GetCount<BLLJIMP.Model.LotteryUserInfo>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}",bllUser.WebsiteOwner,lotteryId));
                bllUser.UpdateByKey<WXLotteryV1>("LotteryID", lotteryId, "WinnerCount", count.ToString());
                apiResp.msg = "添加参与者成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "添加参与者失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}