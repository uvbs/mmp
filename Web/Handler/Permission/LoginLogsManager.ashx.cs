using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler.Permission
{
    /// <summary>
    /// LoginLogsManager 的摘要说明
    /// </summary>
    public class LoginLogsManager : IHttpHandler, IRequiresSessionState
    {


        BLL bll = new BLL("");
         public void ProcessRequest(HttpContext context)
        {
            string result = "false";
            try
            {

                context.Response.ContentType = "text/plain";
                context.Response.Expires = 0;
                string action = context.Request["Action"];

                switch (action)
                {
                    case "Query":
                        result = Query(context);
                        break;

                }
            }
            catch (Exception ex)
            {
                result = ex.Message;

            }

            context.Response.Write(result);
        }





       

        //private string Delete(HttpContext context)
        //{

        //    string ids = context.Request["ids"];
        //    List<string> idsList = ids.Split(',').ToList();
        //    string strIds = Common.StringHelper.ListToStr<string>(idsList, "'", ",");
        //    int count = bll.Delete(new BLLWB.Model.WBAccount(), string.Format(" AccName in ({0}) ", strIds)); ;
        //    return count.ToString();
        //}


        ///// <summary>
        ///// 添加微博账号
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string Add(HttpContext context)
        //{


        //    string jsonData = context.Request["JsonData"];
        //    BLLWB.Model.WBAccount model = ZentCloud.Common.JSONHelper.JsonToModel<BLLWB.Model.WBAccount>(jsonData);

        //    bool result = bll.Add(model);

        //    return result.ToString().ToLower();
        //}
        ///// <summary>
        ///// 编辑账户信息
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string Edit(HttpContext context)
        //{

        //    string jsonData = context.Request["JsonData"];
        //    BLLWB.Model.WBAccount model = ZentCloud.Common.JSONHelper.JsonToModel<BLLWB.Model.WBAccount>(jsonData);
        //    return bll.Update(model).ToString().ToLower();
        //}

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Query(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["UserID"];//搜索参数
            //string ScreenName = context.Request["ScreenName"];//昵称
            string from = context.Request["From"];
            string to = context.Request["To"];

            var sbCondition = new StringBuilder("1=1 ");//搜索条件

            //时间范围查询
            if (!string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to))
            {
                sbCondition.AppendFormat("And InsertDate>='{0}'", from);
            }
            else if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                sbCondition.AppendFormat("And InsertDate<='{0}'", to);
            }
            else if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                sbCondition.AppendFormat("And InsertDate between '{0}' And '{1}'", from, to);
            }
            //时间范围查询
            if (!string.IsNullOrWhiteSpace(userId))
            {
                sbCondition.AppendFormat("And UserID ='{0}'", userId);
            }
            //if (!string.IsNullOrWhiteSpace(ScreenName))
            //{
            //    if (!string.IsNullOrWhiteSpace(searchReq))
            //    {

            //        searchCondition += string.Format("Or ScreenName='{0}'", ScreenName);
            //    }
            //    else
            //    {
            //        searchCondition += string.Format("ScreenName='{0}'", ScreenName);
            //    }

            //}


            List<UserLoginLogs> dataList = bll.GetLit<UserLoginLogs>(pageSize, pageIndex, sbCondition.ToString(), "insertdate desc");

            int totalCount = bll.GetCount<UserLoginLogs>(sbCondition.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });

        }


    


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}