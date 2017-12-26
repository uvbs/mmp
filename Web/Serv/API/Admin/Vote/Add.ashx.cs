using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote
{
    /// <summary>
    /// 添加投票
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception)
            {
                resp.errcode=(int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.vote_name))
            {
                resp.errmsg = "vote_name 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.vote_stop_time<=0)
            {
                resp.errmsg = "vote_stop_time 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            VoteInfo model = new VoteInfo();
            model.VoteName = requestModel.vote_name;
            model.VoteImage = requestModel.vote_img_url;
            model.Summary = requestModel.vote_summary;
            model.StopDate = bllVote.GetTime(requestModel.vote_stop_time).ToString();
            model.VoteStatus = requestModel.vote_status;
            model.WebsiteOwner = bllVote.WebsiteOwner;
            model.CreateUserID = bllVote.GetCurrUserID();
            model.BottomContent = requestModel.vote_bottom_content;
            if (bllVote.AddVoteInfo(model))
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "添加投票出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));


        }

        public class RequestModel 
        {
            /// <summary>
            /// 投票名称
            /// </summary>
            public string vote_name { get; set; }

            /// <summary>
            /// 缩略图
            /// </summary>
            public string vote_img_url { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string vote_summary { get; set; }

            /// <summary>
            /// 投票状态  0 关闭  1开启
            /// </summary>
            public int vote_status { get; set; }

           /// <summary>
           /// 截至日期
           /// </summary>
            public long vote_stop_time { get; set; }

            /// <summary>
            /// 底部内容
            /// </summary>
            public string vote_bottom_content { get; set; }
        }
    }
}