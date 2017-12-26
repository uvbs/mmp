using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// WeixinMemberInfoManage 的摘要说明
    /// </summary>
    public class WeixinMemberInfoManage : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// BLL
        /// </summary>
        static BLLJIMP.BLL bll=new BLLJIMP.BLL();

        /// <summary>
        /// 增删改权限
        /// </summary>
        private static bool IsEdit;
        /// <summary>
        /// 查看权限
        /// </summary>
        private static bool isView;
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            //BLLMenuPermission perbll = new BLLMenuPermission("");
            //_isedit = perbll.CheckUserAndPms(DataLoadTool.GetCurrUserID(), 258);
            //_isview = perbll.CheckUserAndPms(DataLoadTool.GetCurrUserID(), 253);

            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = "false";
            switch (action)
            {
                   
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

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            var strWhere =string.Format("UserID='{0}'",DataLoadTool.GetCurrUserID());

            //if (!string.IsNullOrEmpty(searchtitle))
            //{
            //    searchCondition += "UserID like '%" + searchtitle + "%'";
            //}

            List<ZentCloud.BLLJIMP.Model.WeixinMemberInfo> dataList = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinMemberInfo>(pageSize, pageIndex, strWhere, "RegDate ASC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinMemberInfo>(strWhere);
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
