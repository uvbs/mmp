using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Level
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNoAction
    {


        //BLL
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string historyTotalScore = context.Request["history_total_score"];
            if (string.IsNullOrEmpty(historyTotalScore))
            {
                resp.errcode = 1;
                resp.errmsg = "参数错误";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserLevelConfig userLevel = bll.Get<UserLevelConfig>(string.Format(" WebSiteOwner='{0}' AND  FromHistoryScore<={1} AND ToHistoryScore>={1}",bll.WebsiteOwner, historyTotalScore));
            if (userLevel == null)
            {
                resp.errcode = 1;
                resp.errmsg = "没有查到用户等级";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            RequestModel requesrModel = new RequestModel();
            requesrModel.level_number = userLevel.LevelNumber;
            requesrModel.level_string = userLevel.LevelString;
            requesrModel.level_icon = userLevel.LevelIcon;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requesrModel));
           
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
        }

    }
}