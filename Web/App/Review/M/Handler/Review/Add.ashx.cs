using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Review.M.Handler.Review
{
    /// <summary>
    /// 发布话题
    /// </summary>
    public class Add : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginNoAction
    {

        BLLReview bllReview = new BLLJIMP.BLLReview();
        BLLUser bllUser = new BLLJIMP.BLLUser();
        WebsiteInfo currentWebsteInfo = new WebsiteInfo();
        public void ProcessRequest(HttpContext context)
        {
            string title = context.Request["Title"];
            string content = context.Request["Context"];

            if (string.IsNullOrEmpty(title))
            {
                apiResp.msg = "标题不能为空";
                context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
                return ;
            }
            if (string.IsNullOrEmpty(content))
            {
                apiResp.msg = "内容不能为空";
                context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            currentWebsteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
            if (currentWebsteInfo.IsEnableUserReleaseReview==0)
            {
                apiResp.msg = "暂不开放发布话题功能";
                context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            ReviewInfo model = new ReviewInfo
            {
                ForeignkeyId = bllUser.WebsiteOwner,
                ForeignkeyName = bllUser.WebsiteOwner,
                UserId = CurrentUserInfo.UserID,
                UserName = CurrentUserInfo.TrueName,
                ReviewPower = 0,
                InsertDate = DateTime.Now,
                ReviewTitle = title,
                ReviewContent = content,
                WebsiteOwner = bllUser.WebsiteOwner,
                PraiseNum = 0,
                StepNum = 0,
                ReviewType = "话题",
                CategoryType = "",
                ReplyDateTiem = DateTime.Now
            };

            if (bllReview.Add(model))
            {
                BLLRedis.ClearReviewList(bllUser.WebsiteOwner);
                apiResp.status = true;   
            }
            else
            {
                apiResp.msg = "发布失败";
            }
            context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
        }

    }
}