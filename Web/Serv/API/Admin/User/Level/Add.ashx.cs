using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Level
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        //BLL
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.level_number <= 0)
            {
                resp.errmsg = "level_number参数必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.level_string))
            {
                resp.errmsg = "requestModel参数必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.level_icon))
            {
                resp.errmsg = "level_icon参数必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.level_fromhistory_score <= 0)
            {
                resp.errmsg = "level_fromhistory_score参数必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.level_tohistory_score <= 0)
            {
                resp.errmsg = "level_tohistory_score参数必填,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserLevelConfig userLevel = new UserLevelConfig();
            userLevel.LevelNumber = requestModel.level_number;
            userLevel.LevelString = requestModel.level_string;
            userLevel.LevelIcon = requestModel.level_icon;
            userLevel.FromHistoryScore = requestModel.level_fromhistory_score;
            userLevel.ToHistoryScore = requestModel.level_tohistory_score;
            userLevel.WebSiteOwner = bll.WebsiteOwner;
            if (bll.Add(userLevel))
            {
                resp.isSuccess = true;
                resp.errcode = 0;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "添加用户等级出错";
                resp.errcode = -1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }

        public class RequestModel 
        {
            /// <summary>
            /// 等级编号
            /// </summary>
            public int level_number { get; set; }

            /// <summary>
            /// 等级名称
            /// </summary>
            public string level_string { get; set; }

            /// <summary>
            /// 等级图标
            /// </summary>
            public string level_icon { get; set; }

            /// <summary>
            /// 等级--开始
            /// </summary>
            public double level_fromhistory_score { get; set; }

            /// <summary>
            /// 等级---结束
            /// </summary>
            public double level_tohistory_score { get; set; }
        }
    }
}