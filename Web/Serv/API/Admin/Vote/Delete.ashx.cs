using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote
{
    /// <summary>
    /// 删除投票
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string voteIds = context.Request["vote_ids"];
            if (string.IsNullOrEmpty(voteIds))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound ;
                resp.errmsg = "vote_ids 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            
            if (bllVote.Delete(new VoteInfo(), string.Format(" WebsiteOwner='{0}' AND AutoID in ({1})", bllVote.WebsiteOwner, voteIds))>0)
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "删除投票出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}