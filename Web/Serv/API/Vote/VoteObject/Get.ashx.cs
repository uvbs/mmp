using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Vote.VoteObject
{
    /// <summary>
    /// 选手详情
    /// </summary>
    public class Get : BaseHandlerNoAction
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
                resp.errmsg = " vote_object_id 参数必填";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var voteObjectInfo = bllVote.GetVoteObjectInfo(int.Parse(voteObjectId));
            if (voteObjectInfo==null)
            {
                resp.errcode = 1;
                resp.errmsg = " vote_object_id 不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            List<string> showImgList = new List<string>();
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage1))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage1));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage2))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage2));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage3))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage3));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage4))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage4));
            }
            if (!string.IsNullOrEmpty(voteObjectInfo.ShowImage5))
            {
                showImgList.Add(bllVote.GetImgUrl(voteObjectInfo.ShowImage5));
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                vote_id = voteObjectInfo.VoteID,
                vote_object_id = voteObjectInfo.AutoID,
                vote_object_number = voteObjectInfo.Number,
                vote_object_name = voteObjectInfo.VoteObjectName,
                head_img_url = bllVote.GetImgUrl(voteObjectInfo.VoteObjectHeadImage),
                rank = voteObjectInfo.Rank,
                vote_count = voteObjectInfo.VoteCount,
                vote_object_status = voteObjectInfo.Status,
                vote_object_introduction = voteObjectInfo.Introduction,
                show_img_list = showImgList,
                ex1 = voteObjectInfo.Ex1,
                ex2 = voteObjectInfo.Ex2,
                ex3 = voteObjectInfo.Ex3,
                ex4 = voteObjectInfo.Ex4,
                ex5 = voteObjectInfo.Ex5,
                ex6 = voteObjectInfo.Ex6,
                ex7 = voteObjectInfo.Ex7,
                ex8 = voteObjectInfo.Ex8,
                ex9 = voteObjectInfo.Ex9,
                ex10 = voteObjectInfo.Ex10
            }));



        }

        
    }
}