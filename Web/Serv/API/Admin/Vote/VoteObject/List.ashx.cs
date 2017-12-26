using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote.VoteObject
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        /// <param name="context"></param>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int voteId=!string.IsNullOrEmpty(context.Request["vote_id"])?int.Parse(context.Request["vote_id"]):0;
            string voteObjectNumber = context.Request["vote_object_number"];
            string keyWord = context.Request["keyword"];//关键字
            string sort = context.Request["sort"];//排序
            string voteObjectStatus=context.Request["vote_object_status"];
            string voteObjectArea=context.Request["vote_object_area"];
            if(voteId<=0)
            {
                resp.errmsg="vote_id 为必填项,请检查";
                resp.errcode=(int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            int totalCount=0;
            var voteObjectList = bllVote.GetVoteObjectInfoList(voteId, pageIndex, pageSize, out totalCount, keyWord, voteObjectNumber, voteObjectStatus, sort, voteObjectArea);
            resp.isSuccess = true;
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in voteObjectList)
            {
                returnList.Add(new 
                {
                    vote_oject_id=item.AutoID,
                    vote_object_number=item.Number,
                    vote_object_name=item.VoteObjectName,
                    head_img_url=item.VoteObjectHeadImage,
                    vote_object_age=item.VoteObjectAge,
                    vote_object_gender=item.VoteObjectGender,
                    rank=item.Rank,
                    vote_area=item.Area,
                    vote_object_introduction=item.Introduction,
                    vote_count=item.VoteCount
                });
            }
            resp.returnObj = new 
            {
                totalcount=totalCount,
                list=returnList
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}