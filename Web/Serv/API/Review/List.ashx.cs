using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Review
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLReview bllReview = new BLLReview();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]),
                page = Convert.ToInt32(context.Request["page"]);
            string for_id = context.Request["for_id"],
                ex_id = context.Request["ex_id"],
                keyword = context.Request["keyword"],
                type = context.Request["type"],
                status = context.Request["status"];
            string websiteOwner = bllReview.WebsiteOwner;

            var total = 0;
            //评论
            List<BLLJIMP.Model.ReviewInfo> sourceData = bllReview.GetActReviewList(out total, page, rows, for_id, keyword, "", type, ex_id, status
                , "AutoId,ReviewMainId,UserId,ReviewContent,InsertDate,ReviewScore,CommentImg,ForeignkeyId,Expand1,Ex2");

            List<dynamic> returnList = new List<dynamic>();
            List<BLLJIMP.Model.UserInfo> users = new List<BLLJIMP.Model.UserInfo>();

            foreach (var item in sourceData)
            {
                BLLJIMP.Model.UserInfo pubUser = users.FirstOrDefault(p=>p.UserID == item.UserId);
                if(pubUser == null){
                    pubUser = bllUser.GetUserInfo(item.UserId, websiteOwner);
                    if(pubUser != null) users.Add(pubUser);
                }
                WXMallOrderDetailsInfo orderDetails = bllReview.Get<WXMallOrderDetailsInfo>(string.Format(" AutoID={0} ",!string.IsNullOrEmpty(item.Ex2)?item.Ex2:"0"));

                returnList.Add(new
                {
                    id = item.ReviewMainId,
                    content = item.ReviewContent,
                    time = ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(item.InsertDate),
                    review_score = item.ReviewScore,
                    comment_img=item.CommentImg,
                    showprops=orderDetails==null?"":orderDetails.SkuShowProp,
                    pub_user = pubUser == null? null: new {
                        id = pubUser.AutoID,
                        avatar = bllUser.GetUserDispalyAvatar(pubUser),
                        name = bllUser.GetUserDispalyName(pubUser)
                    }
                });
            }
            apiResp.status=true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.result = new
            {
                totalcount = total,
                list = returnList
            };
            bllReview.ContextResponse(context, apiResp);
        }


    }
}