using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote
{
    /// <summary>
    /// 投票详情接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        /// <param name="context"></param>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string voteId = context.Request["vote_id"];
            if (string.IsNullOrEmpty(voteId))
            {
                resp.errmsg = "vote_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            VoteInfo model = bllVote.GetVoteInfo(int.Parse(voteId));
            if (model == null)
            {
                resp.errmsg = "不存在该条投票信息";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            resp.isSuccess = true;
            resp.returnObj=new
            {
                vote_name = model.VoteName,
                vote_img_url = model.VoteImage,
                vote_summary = model.Summary,
                vote_status = model.VoteStatus,
                vote_stop_time = model.StopDate,
                vote_bottom_content = model.BottomContent
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}