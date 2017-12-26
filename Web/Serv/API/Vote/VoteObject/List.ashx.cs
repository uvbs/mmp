using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Vote.VoteObject
{
    /// <summary>
    ///选手列表
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        /// <summary>
        /// 投票逻辑
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string voteId = context.Request["vote_id"];
            string voteObjectStatus=context.Request["vote_object_status"];
            if (string.IsNullOrEmpty(voteId))
            {
                resp.errcode = 1;
                resp.errmsg = " vote_id 参数必填";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string sort = context.Request["sort"];
            int totalCount = 0;
            var sourceData = bllVote.GetVoteObjectInfoList(int.Parse(voteId), pageIndex, pageSize, out totalCount, keyWord, "", voteObjectStatus, sort);
            var list = from p in sourceData
                       select new
                       {
                           vote_object_id = p.AutoID,
                           vote_object_number = p.Number,
                           vote_object_name = p.VoteObjectName,
                           head_img_url = bllVote.GetImgUrl(p.VoteObjectHeadImage),
                           rank = p.Rank,
                           vote_count = p.VoteCount,
                           vote_object_status = p.Status,
                           vote_object_introduction = p.Introduction

                       };

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
             {
                 totalcount = totalCount,
                 list = list

             }));



        }


    }
}