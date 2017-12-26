using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote.VoteObject
{
    /// <summary>
    /// Get 的摘要说明
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
            string voteObjectId = context.Request["vote_object_id"];
            if (string.IsNullOrEmpty(voteObjectId))
            {
                resp.errmsg = "vote_object_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            VoteObjectInfo model = bllVote.GetVoteObjectInfo(int.Parse(voteObjectId));
            if (model == null)
            {
                resp.errmsg = "投票对象不存在";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            resp.isSuccess = true;
            List<string> showImgList = new List<string>();
            if (!string.IsNullOrEmpty(model.ShowImage1))
            {
                showImgList.Add(bllVote.GetImgUrl(model.ShowImage1));
            }
            if (!string.IsNullOrEmpty(model.ShowImage2))
            {
                showImgList.Add(bllVote.GetImgUrl(model.ShowImage2));
            }
            if (!string.IsNullOrEmpty(model.ShowImage3))
            {
                showImgList.Add(bllVote.GetImgUrl(model.ShowImage3));
            }
            if (!string.IsNullOrEmpty(model.ShowImage4))
            {
                showImgList.Add(bllVote.GetImgUrl(model.ShowImage4));
            }
            if (!string.IsNullOrEmpty(model.ShowImage5))
            {
                showImgList.Add(bllVote.GetImgUrl(model.ShowImage5));
            }
            resp.returnObj = new 
            {
                vote_id = model.VoteID,
                vote_object_id = model.AutoID,
                vote_object_number = model.Number,
                vote_object_name = model.VoteObjectName,
                head_img_url = bllVote.GetImgUrl(model.VoteObjectHeadImage),
                rank = model.Rank,
                vote_count = model.VoteCount,
                vote_object_status = model.Status,
                vote_object_introduction = model.Introduction,
                show_img_list = showImgList,
                vote_object_phone = model.Phone,
                vote_area=model.Area,
                ex1 = model.Ex1,
                ex2 = model.Ex2,
                ex3 = model.Ex3,
                ex4 = model.Ex4,
                ex5 = model.Ex5,
                ex6 = model.Ex6,
                ex7 = model.Ex7,
                ex8 = model.Ex8,
                ex9 = model.Ex9,
                ex10 = model.Ex10
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}