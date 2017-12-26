using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// SMSTriggerDetails 的摘要说明
    /// </summary>
    public class SMSTriggerDetails : IHttpHandler, IRequiresSessionState
    {




        static BLLJIMP.BLLSMS bll;
        
        /// <summary>
        /// 增删改权限
        /// </summary>
        private static bool _isedit;
        /// <summary>
        /// 查看权限
        /// </summary>
        private static bool _isview;
        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "text/plain";
            //BLLMenuPermission perbll = new BLLMenuPermission("");
            //_isedit = perbll.CheckUserAndPms(Comm.DataLoadTool.GetCurrUserID(), 258);
            //_isview = perbll.CheckUserAndPms(Comm.DataLoadTool.GetCurrUserID(), 253);

            bll =new BLLJIMP.BLLSMS("");

            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string Action = context.Request["Action"];
            string result = "false";
            switch (Action)
            {
                //case "Add":
                //    result = Add(context);
                //    break;
                //case "Edit":
                //    result = Edit(context);
                //    break;
                //case "Delete":
                //    result = Delete(context);
                //    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
            }
            context.Response.Write(result);
        }



        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private static string GetAllByAny(HttpContext context)
        {
            //if (!_isview)
            //{
            //    return null;
            //}

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string searchtitle = context.Request["SearchTitle"];
            var searchCondition = "";
            var userinfo=Comm.DataLoadTool.GetCurrUserModel();
            if (userinfo.UserType != 1)
            {
                searchCondition = string.Format("UserID='{0}'",userinfo.UserID);
            }
            if (!string.IsNullOrEmpty(searchtitle))
            {
                searchCondition += " UserID like '%" + searchtitle + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.SMSTriggerDetails> list = bll.GetLit<ZentCloud.BLLJIMP.Model.SMSTriggerDetails>(rows, page, searchCondition, "SubmitDate DESC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.SMSTriggerDetails>(searchCondition);

            string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, list);

            return jsonResult;
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