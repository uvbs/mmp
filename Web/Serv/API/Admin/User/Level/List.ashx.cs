using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Level
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            StringBuilder sbWhere = new StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            string level_type = context.Request["level_type"];
            string hide_count = context.Request["hide_count"];
            string level_num = context.Request["level_num"];
            string min_level = context.Request["min_level"];
            
            if (!string.IsNullOrEmpty(level_type))
            {
                sbWhere.AppendFormat(" AND LevelType ='{0}'", level_type);
            }
            else
            {
                sbWhere.AppendFormat(" AND LevelType ='{0}'", "DistributionOnLine");
            }
            if (!string.IsNullOrEmpty(min_level))
            {
                sbWhere.AppendFormat(" AND LevelNumber>={0}", min_level);
            }
            if (!string.IsNullOrEmpty(level_num))
            {
                sbWhere.AppendFormat(" AND LevelNumber={0}", level_num);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND LevelString like '%{0}%'", keyWord);
            }
            int totalCount = 0;
            if (hide_count != "1") totalCount =bll.GetCount<UserLevelConfig>(sbWhere.ToString());
            var levelData = bll.GetLit<UserLevelConfig>(pageSize, pageIndex, sbWhere.ToString());
            var list = from p in levelData
                       select new
                       {
                           level_id=p.AutoId,
                           level_number = p.LevelNumber,
                           level_string = p.LevelString,
                           level_icon = p.LevelIcon,
                           level_fromhistory_score=p.FromHistoryScore,
                           level_tohistory_score=p.ToHistoryScore
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list//列表
            };
            bll.ContextResponse(context, data);
        }


        public class RequestModel
        {
            /// <summary>
            /// 等级id
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