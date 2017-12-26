using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Review
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLReview bllReview = new BLLReview();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        { 
            RequestModel requestModel = bllReview.ConvertRequestToModel<RequestModel>(new RequestModel());
            if (string.IsNullOrWhiteSpace(requestModel.for_id))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "评论对象Id为空";
                bllReview.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.ReviewInfo review = new BLLJIMP.Model.ReviewInfo();
            review.AuditStatus = 0;
            review.ForeignkeyId = requestModel.for_id;
            review.Expand1 = requestModel.ex_id;
            review.UserId = CurrentUserInfo.UserID;
            review.UserName = bllUser.GetUserDispalyName(CurrentUserInfo);
            review.ReviewScore = requestModel.score;
            review.ReviewContent = requestModel.content;
            review.InsertDate = DateTime.Now;
            review.ReviewType = requestModel.type;
            review.ReviewTitle = requestModel.title;
            review.WebsiteOwner = bllReview.WebsiteOwner;
            review.IsHideUserName = 0;
            review.ReviewMainId = int.Parse(bllReview.GetGUID(TransacType.CommAdd));
            review.CommentImg = requestModel.comment_img;
            review.Ex2 = requestModel.order_detail_id;
            if (bllReview.Add(review))
            {
                if (review.ReviewType == "OrderComment" && !string.IsNullOrWhiteSpace(review.Expand1)) //更新 平均分 评分人数
                {
                    var total = 0;
                    List<BLLJIMP.Model.ReviewInfo> sourceData = bllReview.GetActReviewList(out total, 1, 0, "", "", "", "OrderComment", review.Expand1, "0","AutoId");
                    double reviewAvgScore = bllReview.GetReviewAvgScore(bllReview.WebsiteOwner,"","OrderComment",review.Expand1,"0");
                    bllReview.Update(new BLLJIMP.Model.WXMallProductInfo(),
                        string.Format("ReviewCount='{0}',ReviewScore='{1}'", total, reviewAvgScore),
                        string.Format("PID='{0}' And WebsiteOwner='{1}' ", review.Expand1, bllReview.WebsiteOwner));

                    bllReview.Update(new BLLJIMP.Model.WXMallOrderInfo(),
                        string.Format("ReviewScore='{0}'", review.ReviewScore),
                        string.Format("OrderID='{0}' And WebsiteOwner='{1}' ", review.ForeignkeyId, bllReview.WebsiteOwner));
                }

                apiResp.status = true;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "评论完成";

                BLLRedis.ClearReviewList(bllReview.WebsiteOwner);

            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "评论失败";
            }
            bllReview.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class RequestModel
        {
            /// <summary>
            /// 评论对象Id
            /// </summary>
            public string for_id { get; set; }
            /// <summary>
            /// 相关冗余Id
            /// </summary>
            public string ex_id { get; set; }
            /// <summary>
            /// 评论内容
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 评分
            /// </summary>
            public float score { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 评论标题
            /// </summary>
            public string title { get; set; }

            /// <summary>
            /// 评论图片
            /// </summary>
            public string comment_img { get; set; }

            /// <summary>
            /// 订单详情id
            /// </summary>
            public string order_detail_id { get; set; }
        }
    }
}