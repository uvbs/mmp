using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Vote
{
    /// <summary>
    /// 修改投票信息
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
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
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.vote_id <= 0)
            {
                resp.errmsg = "vote_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
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
            if (requestModel.vote_stop_time <= 0)
            {
                resp.errmsg = "vote_stop_time 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            VoteInfo model = new VoteInfo();
            model.AutoID = requestModel.vote_id;
            model.VoteName = requestModel.vote_name;
            model.VoteImage = requestModel.vote_img_url;
            model.Summary = requestModel.vote_summary;
            model.StopDate = bllVote.GetTime(requestModel.vote_stop_time).ToString();
            model.VoteStatus = requestModel.vote_status;
            model.BottomContent = requestModel.vote_bottom_content;
            if (bllVote.UpdateVoteInfo(model))
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "修改投票出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel
        {
            /// <summary>
            /// 投票id
            /// </summary>
            public int vote_id { get; set; }

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