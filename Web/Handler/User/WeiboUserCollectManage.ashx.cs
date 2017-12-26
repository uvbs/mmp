using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.User
{
    /// <summary>
    /// WeiboUserCollectManage 的摘要说明
    /// </summary>
    public class WeiboUserCollectManage : IHttpHandler, IRequiresSessionState
    {


        static BLLJIMP.BLL bll;

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

            bll = new BLLJIMP.BLL("");

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
                case "Delete":
                    result = Delete(context);
                    break;
                case "Query":
                    result = GetAllByAny(context);
                    break;
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {
            string ids = context.Request["id"];
            var count = 0;
            count = bll.Delete(new WeiboUserCollect(), string.Format("CollectID in({0}) And UserID='{1}'", ids, Comm.DataLoadTool.GetCurrUserID()));
            if (count == ids.Split(',').Length)
            {
                return "true";
            }
            return "false";


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
            var searchCondition =string.Format( "UserID='{0}'",Comm.DataLoadTool.GetCurrUserID());
            if (!string.IsNullOrEmpty(searchtitle))
            {
                searchCondition += "And WeiboName like '%" + searchtitle + "%'";
            }

            List<WeiboUserCollect> list = bll.GetLit<WeiboUserCollect>(rows, page, searchCondition, "AddDate DESC");

            int totalCount = bll.GetCount<WeiboUserCollect>(searchCondition);

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