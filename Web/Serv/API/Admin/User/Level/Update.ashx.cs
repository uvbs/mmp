using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Level
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

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
            if (requestModel.level_id <= 0)
            {
                resp.errmsg = "level_id参数必填,请检查";
                resp.errcode = 1;
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
            UserLevelConfig userlevel = bll.Get<UserLevelConfig>(string.Format(" WebsiteOwner='{0}' and AutoID={1}",bll.WebsiteOwner,requestModel.level_id));
            if (userlevel == null)
            {
                resp.errcode = -1;
                resp.errmsg = "用户等级对象不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            userlevel.LevelNumber = requestModel.level_number;
            userlevel.LevelString = requestModel.level_string;
            userlevel.LevelIcon = requestModel.level_icon;
            userlevel.FromHistoryScore = requestModel.level_fromhistory_score;
            userlevel.ToHistoryScore = requestModel.level_tohistory_score;
            if (bll.Update(userlevel))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "修改用户等级出错";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;
        }
        public class RequestModel
        {
            /// <summary>
            /// id
            /// </summary>
            public int level_id { get; set; }

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