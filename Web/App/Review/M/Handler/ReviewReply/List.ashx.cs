using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Review.M.Handler.ReviewReply
{
    /// <summary>
    ///话题回复列表
    /// </summary>
    public class List : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginNoAction
    {
        BLLReview bllReview = new BLLJIMP.BLLReview();
        BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            List<ReplyReviewInfo> dataList = bllReview.GetLit<ReplyReviewInfo>(pageSize, pageIndex, string.Format(" ReviewID={0} And Status=1", autoId), " AutoId DESC");
            foreach (ReplyReviewInfo item in dataList)
            {
                UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                item.HTNum = bllReview.GetCount<ReviewInfo>(string.Format(" ForeignkeyId='{0}' AND websiteOwner='{1}'", item.UserId, bllReview.WebsiteOwner));
                if (userInfo != null)
                {

                    item.Img = bllUser.GetUserDispalyAvatar(userInfo);
                    item.NickName = userInfo.WXNickname;
                    item.UserName = userInfo.TrueName;

                }

            }
            apiResp.status = true;
            apiResp.result = dataList;
            context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}