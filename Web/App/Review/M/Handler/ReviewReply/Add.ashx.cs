using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Review.M.Handler.ReviewReply
{
    /// <summary>
    /// 回复话题
    /// </summary>
    public class Add : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginNoAction
    {

        BLLReview bllReview = new BLLJIMP.BLLReview();
        BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoID"]);
            string content = context.Request["Context"];
            ReviewInfo reviewInfo = bllReview.Get<ReviewInfo>(string.Format("AutoId={0}", autoId));
            ReplyReviewInfo replyReview = new ReplyReviewInfo()
            {
                ReviewID = Convert.ToInt32(autoId),
                InsertDate = DateTime.Now,
                ReplyContent = content,
                UserId = CurrentUserInfo.UserID,
                UserName = CurrentUserInfo.LoginName,
                PraentId = 0,
                WebSiteOwner = bllReview.WebsiteOwner

            };
            bool isSuccess = bllReview.Add(replyReview);
            if (isSuccess)
            {
                
                reviewInfo.NumCount++;
                reviewInfo.ReplyDateTiem = DateTime.Now;
                if (bllReview.Update(reviewInfo))
                {
                    

                    //给回复者加分
                    int replyCount = bllReview.GetCount<ReplyReviewInfo>(string.Format("ReviewID={0} And  UserId='{1}'", autoId, CurrentUserInfo.UserID));
                    if (replyCount <= 1)//第一次回答才得分
                    {

                        bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.AnswerQuestions), bllUser.WebsiteOwner, null, null);

                    }
                    apiResp.status = true;
                }
                else
                {
                   
                }
            }
            else
            {
               
            }
            BLLRedis.ClearReviewList(bllReview.WebsiteOwner);
            context.Response.Write( Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}