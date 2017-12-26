using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Vote.VoteObject
{
    /// <summary>
    ///投票
    /// </summary>
    public class AddVoteCount : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string voteObjectId = context.Request["vote_object_id"];
            if (string.IsNullOrEmpty(voteObjectId))
            {
                resp.errcode = 1;
                resp.errmsg = "vote_object_id 必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            VoteObjectInfo voteObjInfo = bllVote.GetVoteObjectInfo(int.Parse(voteObjectId));
            if (voteObjInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "vote_object_id 不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            VoteInfo voteInfo = bllVote.GetVoteInfo(voteObjInfo.VoteID);
            if (voteInfo.VoteStatus.Equals(0))
            {
                resp.errcode = 1;
                resp.errmsg = "投票停止";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;


            }
            if (!string.IsNullOrEmpty(voteInfo.StopDate))
            {
                if (DateTime.Now > (DateTime.Parse(voteInfo.StopDate)))
                {
                    resp.errcode = 1;
                    resp.errmsg = "投票结束";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;


                }
            }
            if (voteObjInfo.Status.Equals(0))
            {
                resp.errcode = 1;
                resp.errmsg = "审核未通过";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }
            string dateTimeToDay = DateTime.Now.ToString("yyyy-MM-dd");
            string dateTimeTomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //检查是否可以投票 每天1票
            int logCount = bllVote.GetCount<VoteLogInfo>(string.Format("VoteID={0} And UserID='{1}' And InsertDate>='{2}' And InsertDate<'{3}'", voteInfo.AutoID, CurrentUserInfo.UserID, dateTimeToDay, dateTimeTomorrow));
            if (logCount >= 1)
            {
                resp.errcode = 1;
                resp.errmsg = "今天已经投过票了,明天再来吧";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }

            if (bllVote.UpdateVoteObjectVoteCount(voteInfo.AutoID, voteObjInfo.AutoID, CurrentUserInfo.UserID, 1))
            {

                if (!bllVote.UpdateVoteObjectRank(voteInfo.AutoID, "1"))
                {
                    resp.errcode = 1;
                    resp.errmsg = "更新排名失败";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;

                }
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "投票失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }


    }
}