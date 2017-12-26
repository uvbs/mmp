using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// WeixinMsgDetailsManage 的摘要说明
    /// </summary>
    public class WeixinMsgDetailsManage : IHttpHandler, IRequiresSessionState
    {

        static BLLJIMP.BLL bll;

        /// <summary>
        /// 增删改权限
        /// </summary>
        private static bool isEdit;
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

            bll = new BLLJIMP.BLL("");
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
            var strWhere = "";
            var userinfo = DataLoadTool.GetCurrUserModel();
            if (userinfo.UserType != 1)
            {
                strWhere = string.Format("UserID='{0}'", userinfo.UserID);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere += " UserID like '%" + keyWord + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.WeixinMsgDetails> list = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinMsgDetails>(pageSize, pageIndex, strWhere, "ReplyDate DESC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinMsgDetails>(strWhere);

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = list
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