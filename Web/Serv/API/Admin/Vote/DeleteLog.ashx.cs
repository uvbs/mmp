using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote
{
    /// <summary>
    /// 删除投票记录
    /// </summary>
    public class DeleteLog : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string voteId = context.Request["vote_id"];
            if (string.IsNullOrEmpty(voteId))
            {
                resp.errmsg = "vote_id 为必填,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            VoteInfo vote = bllVote.GetVoteInfo(int.Parse(voteId));
            if (vote == null)
            {
                resp.errmsg = "不存在投票";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var logList = bllVote.GetList<VoteLogInfo>(string.Format(" WebsiteOwner='{0}' AND VoteID in ({1})", bllVote.WebsiteOwner, voteId));
            if (logList.Count == 0)
            {
                resp.errmsg = "日志已经为空";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllVote.Delete(new VoteLogInfo(), string.Format(" WebsiteOwner='{0}' AND VoteID in ({1})", bllVote.WebsiteOwner, voteId))>0)
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = "清空投票日志出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}