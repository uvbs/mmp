using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Level
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {


        //BLL
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string levelId = context.Request["level_id"];
            if (string.IsNullOrEmpty(levelId))
            {
                resp.errcode = 1;
                resp.errmsg = "参数错误";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserLevelConfig userLevel = bll.Get<UserLevelConfig>(string.Format(" WebSiteOwner='{0}' AND AutoID={1} ",bll.WebsiteOwner,levelId));
            if (userLevel == null)
            {
                resp.errcode = 1;
                resp.errmsg = "没有查到用户等级";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requesrModel = new RequestModel();
            requesrModel.level_id = userLevel.AutoId;
            requesrModel.level_number = userLevel.LevelNumber;
            requesrModel.level_string = userLevel.LevelString;
            requesrModel.level_icon = userLevel.LevelIcon;
            requesrModel.level_fromhistory_score = userLevel.FromHistoryScore;
            requesrModel.level_tohistory_score = userLevel.ToHistoryScore;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requesrModel));
            return;
        }
        public class RequestModel
        {
            /// <summary>
            /// 等级id
            /// </summary>
            public int? level_id { get; set; }
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
            /// 累计积分--开始
            /// </summary>
            public double level_fromhistory_score { get; set; }

            /// <summary>
            /// 累计积分---结束
            /// </summary>
            public double level_tohistory_score { get; set; }
        }
    }
}