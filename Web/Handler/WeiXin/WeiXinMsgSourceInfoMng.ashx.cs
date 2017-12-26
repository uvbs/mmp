using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Text.RegularExpressions;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// WeiXinMsgSourceInfoMng 的摘要说明
    /// </summary>
    public class WeiXinMsgSourceInfoMng : IHttpHandler, IRequiresSessionState
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

        private static string websiteOwner;//设定该站点所有者

        public void ProcessRequest(HttpContext context)
        {

            websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
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
            //验证网址

            var linkUrl = context.Request["LinkUrl"];
            var picUrl = context.Request["PicUrl"];
            string match = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            Regex reg = new Regex(match);
            if (!string.IsNullOrEmpty(linkUrl))
            {
                if (!reg.IsMatch(linkUrl))
                {
                    return "请输入正确的网址,格式如 http://www.baidu.com";
                }

            }
            //if (!reg.IsMatch(picurl))
            //{
            //     return "请输入正确的图片地址,格式如 http://www.baidu.com/icon/demo.png";
            //}

            WeixinMsgSourceInfo model = new WeixinMsgSourceInfo();
            model.UserID = websiteOwner;//Comm.DataLoadTool.GetCurrUserID();
            model.Title = context.Request["SourceName"];
            model.PicUrl = picUrl;
            model.Url = linkUrl;
            model.SourceID = bll.GetGUID(ZentCloud.BLLJIMP.TransacType.WeixinSourceAdd);
            model.Description = context.Request["Description"];
            return bll.Add(model).ToString().ToLower();


        }

        /// <summary>
        /// 修改
        /// </summary>
        public static string Edit(HttpContext context)
        {
            //if (!_isedit)
            //{
            //    return null;
            //}
            //验证网址
            var linkUrl = context.Request["LinkUrl"];
            if (!string.IsNullOrEmpty(linkUrl))
            {
                string match = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                Regex reg = new Regex(match);
                if (!reg.IsMatch(linkUrl))
                {
                    return "请输入正确的网址,格式如 http://www.baidu.com";
                }

            }
            WeixinMsgSourceInfo model = new WeixinMsgSourceInfo();
            model.UserID = websiteOwner; //Comm.DataLoadTool.GetCurrUserID();
            model.Title = context.Request["SourceName"];
            model.PicUrl = context.Request["PicUrl"];
            model.Url = context.Request["LinkUrl"];
            model.SourceID = context.Request["SourceId"];
            model.Description = context.Request["Description"];
            return bll.Update(model).ToString().ToLower();

        }

        /// <summary>
        /// 删除
        /// </summary>
        private static string Delete(HttpContext context)
        {
            //if (!_isedit)
            //{
            //    return null;
            //}

            string ids = context.Request["id"];

            if (bll.Delete(new WeixinMsgSourceInfo(), string.Format("SourceID in({0}) ", ids)) > 0)
                return "true";

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

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["SearchTitle"];
            var strWhere = string.Format("UserID='{0}'", websiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere += " AND Title like '%" + keyWord + "%'";
            }

            List<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo> list = bll.GetLit<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(pageSize, pageIndex, strWhere, "SourceID DESC");

            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.WeixinMsgSourceInfo>(strWhere);

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