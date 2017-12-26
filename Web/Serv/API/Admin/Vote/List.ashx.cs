using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote
{
    /// <summary>
    /// 投票列表
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
            try
            {
                int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
                int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
                string keyWord = context.Request["keyword"];//关键字
                string sort = context.Request["sort"];//排序

                int totalCount = 0;
                var voteList = bllVote.GetVoteInfoList(pageIndex,pageSize,keyWord,sort,out totalCount);
                resp.isSuccess = true;
                List<dynamic> returnList = new List<dynamic>();
                foreach (var item in voteList)
                {
                    returnList.Add(new 
                    {
                        vote_id=item.AutoID,
                        vote_name=item.VoteName,
                        vote_img_url=item.VoteImage,
                        vote_summary=item.Summary,
                        vote_status=item.VoteStatus,
                        vote_stop_time=item.StopDate,
                        vote_bottom_content=item.BottomContent
                    });
                }
                resp.returnObj=new 
                {
                    totalcount=totalCount,
                    list=returnList
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}