using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// WeixinReplyRuleInfoTextManage 的摘要说明
    /// </summary>
    public class WeixinReplyRuleInfoTextManage : IHttpHandler, IRequiresSessionState
    {
        static BLLJIMP.BLLWeixin bll;

        ///// <summary>
        ///// 增删改权限
        ///// </summary>
        //private static bool _isedit;
        ///// <summary>
        ///// 查看权限
        ///// </summary>
        //private static bool _isview;

        private static string websiteOwner;//设定该站点所有者

        public void ProcessRequest(HttpContext context)
        {
            websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
            bll = new BLLJIMP.BLLWeixin("");
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = "false";
            switch (action)
            {
                case "Add":
                    result = Add(context);
                    break;
                case "Edit":
                    result = Edit(context);
                    break;
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
        /// 添加
        /// </summary>
        private static string Add(HttpContext context)
        {

            //if (!_isedit)
            //{
            //    return null;
            //}

            try
            {
                //var userid = websiteOwner;
                //if (string.IsNullOrEmpty(userid))
                //{
                //    return "请重新登录";
                //}
                var keyword = context.Request["MsgKeyword"];
                var matchType = context.Request["MatchType"];
                if (!bll.CheckUserKeyword(websiteOwner, keyword))
                {
                    return "关键字重复";
                }
                var model = new WeixinReplyRuleInfo();
                model.MsgKeyword = keyword;
                model.MatchType = matchType;
                model.ReplyContent = context.Request["ReplyContent"];
                model.ReceiveType = "text";
                model.ReplyType = "text";
                model.CreateDate = DateTime.Now;
                model.RuleType = 1;
                model.UID = bll.GetGUID(BLLJIMP.TransacType.WeixinReplyRuleAdd);
                model.UserID = websiteOwner;
                return bll.Add(model).ToString().ToLower();

            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            //return "false";

        }


        /// <summary>
        /// 修改
        /// </summary>
        public static string Edit(HttpContext context)
        {
            var uid = context.Request["UID"];
            //var userId = websiteOwner;//Comm.DataLoadTool.GetCurrUserID();
            //if (string.IsNullOrEmpty(userid))
            //{
            //    return "请重新登录";
            //}
            var keyword = context.Request["MsgKeyword"];
            var matchType = context.Request["MatchType"];
            var oldInfo = bll.Get<WeixinReplyRuleInfo>(string.Format("UID={0}", uid));
            if (oldInfo.MsgKeyword != keyword)//对比关键字已经改变
            {
                //关键字改变,检查关键字是否重复
                if (!bll.CheckUserKeyword(websiteOwner, keyword))
                {
                    return "关键字重复";
                }

            }

            var model = new WeixinReplyRuleInfo();
            model.UID = uid;
            model.MsgKeyword = keyword;
            model.MatchType = matchType;
            model.ReplyContent = context.Request["ReplyContent"];
            model.ReceiveType = "text";
            model.ReplyType = "text";
            model.CreateDate = DateTime.Now;
            model.RuleType = 1;
            model.UserID = websiteOwner;
            return bll.Update(model).ToString().ToLower();
        }


        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {

            try
            {
                string ids = context.Request["id"];
                bll.Delete(new WeixinReplyRuleInfo(), string.Format("UID in({0}) ", ids));
                return "true";

            }
            catch (Exception ex)
            {

                return ex.Message;
            }


        }





        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        private static string GetAllByAny(HttpContext context)
        {

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            var strWhere = string.Format("UserID='{0}'  And ReplyType='text'And RuleType=1", websiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere += " And MsgKeyword like '%" + keyWord + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo> dataList = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(pageSize, pageIndex, strWhere, "UID");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(strWhere);

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