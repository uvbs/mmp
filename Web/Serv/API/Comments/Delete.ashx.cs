using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Comments
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["id"];
            if (string.IsNullOrEmpty(autoId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "id 为必填项,请检查";
                bllReview.ContextResponse(context, apiResp);
                return;
            }
            ReviewInfo model = bllReview.GetReviewByAutoId(int.Parse(autoId));
            if (model == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "评论已经删除";
                bllReview.ContextResponse(context, apiResp);
                return;
            }
            if (bllReview.Delete(new ReviewInfo(), string.Format(" WebsiteOwner='{0}' AND AutoID={1}", bllReview.WebsiteOwner, int.Parse(autoId))) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
                int reviewCount = bllReview.GetReviewCount(BLLJIMP.Enums.ReviewTypeKey.AppointmentComment,model.ForeignkeyId,model.UserId);
                bllJuActivity.Update(new JuActivityInfo(), string.Format(" CommentCount={0} AND CommentAndReplayCount='{0}' ",reviewCount), string.Format(" JuActivityID={0}", model.ForeignkeyId));
            }
            else
            {
                apiResp.msg = "删除评论失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllReview.ContextResponse(context,apiResp);
        }
    }
}