using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.Help
{
    /// <summary>
    /// 帮助 处理文件
    /// </summary>
    public class HelpHandler : IHttpHandler, IRequiresSessionState
    {

        BLL bll = new BLL("");
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo useinfo;
        public void ProcessRequest(HttpContext context)
        {
            useinfo = Comm.DataLoadTool.GetCurrUserModel();
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string Action = context.Request["Action"];
            string result = "true";
            switch (Action)
            {
                case "AddCategory":
                    result = AddCategory(context);
                    break;
                case "EditCategory":
                    result = EditCategory(context);
                    break;
                case "DeleteCategory":
                    result = DeleteCategory(context);
                    break;
                case "QueryCategory":
                    result = QueryCategory(context);
                    break;
                case "GetMenuSelectList":
                    result = GetMenuSelectList(context);
                    break;



                case "QueryHelpContent":
                    result = QueryHelpContent(context);
                    break;

                case "AddHelpContent":
                    result = AddHelpContent(context);
                    break;
                case "GetSingleHelpContent":
                    result = GetSingleHelpContent(context);
                    break;


                case "EditHelpContent":
                    result = EditHelpContent(context);
                    break;
                case "DeleteHelpContent":
                    result = DeleteHelpContent(context);
                    break;

              
                //case "SubmitFeedBack":
                //    result = SubmitFeedBack(context);
                //    break;

                case "QueryFeedBack":
                    result = QueryFeedBack(context);
                    break;

                case "DeleteFeedBack":
                    result = DeleteFeedBack(context);
                    break;


                    

            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 获取菜单选择列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMenuSelectList(HttpContext context)
        {
            string result = string.Empty;
            result = new MySpider.MyCategories().GetSelectOptionHtml(bll.GetList<HelpCategory>(), "CategoryID", "PreID", "NodeName", 0, "ddlPreMenu", "width:200px", "");
            return result.ToString();
        }

        private string DeleteCategory(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_DeleteMenu))
            //{
            //    return "无权删除菜单";
            //}
            //#endregion

            string ids = context.Request["ids"];

            //TODO:删除菜单前，清除相关权限菜单关联

            int result = bll.Delete(new HelpCategory(), string.Format(" CategoryID in ({0})", ids));//pmsBll.DeleteUser(idsList);
            if (result>0)
            {
                bll.Delete(new HelpContents(),string.Format("CategoryID={0}",ids));
            }
            return result.ToString();
        }

        private string AddCategory(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_AddMenu))
            //{
            //    return "无权添加菜单";
            //}
            //#endregion

            string jsonData = context.Request["JsonData"];
            HelpCategory model = ZentCloud.Common.JSONHelper.JsonToModel<HelpCategory>(jsonData);
            model.CategoryID = long.Parse(bll.GetGUID(Common.TransacType.HelpCategoryAdd));
            bool result = bll.Add(model);
            return result.ToString().ToLower();
        }

        private string EditCategory(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Menu_EditMenu))
            //{
            //    return "无权修改菜单信息";
            //}
            //#endregion

            string jsonData = context.Request["JsonData"];
            HelpCategory model = ZentCloud.Common.JSONHelper.JsonToModel<HelpCategory>(jsonData);
            bool result = bll.Update(model);
            return result.ToString().ToLower();
        }
        /// <summary>
        /// 分类查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryCategory(HttpContext context)
        {
            List<HelpCategory> list;//分页去掉 = bll.GetLit<BLLMJCMS.Model.HelpCategory>(rows, page, searchCondition, "CategoryID");

            list = bll.GetList<HelpCategory>().OrderBy(p=>p.Sort).ToList();

            List<HelpCategory> showList = new List<HelpCategory>();

            MySpider.MyCategories m = new MySpider.MyCategories();

            foreach (ListItem item in m.GetCateListItem(m.GetCommCateModelList<HelpCategory>("CategoryID", "PreID", "NodeName", list), 0))
            {
                try
                {
                    HelpCategory tmpModel = list.Where(p => p.CategoryID.ToString().Equals(item.Value)).ToList()[0];
                    tmpModel.NodeName = item.Text;
                    showList.Add(tmpModel);
                }
                catch { }
            }
           
            int totalCount = showList.Count;

            string jsonResult = ZentCloud.Common.JSONHelper.ObjectToJson(totalCount, showList);

            return jsonResult;
        }


        private string QueryHelpContent(HttpContext context) {

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            var CategoryID = context.Request["CategoryID"];
            var searchCondition = string.Format("CategoryID='{0}'", CategoryID);
            List<HelpContents> list = bll.GetLit<HelpContents>(rows, page, searchCondition, "AddDate DESC");

            int totalCount = bll.GetCount<HelpContents>(searchCondition);
            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalCount, list);
            return jsonResult;

        
        
        
        }

        /// <summary>
        /// 添加内容页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddHelpContent(HttpContext context) {

            var model = GetContentModel(context);
            model.HelpContent = context.Request["Content"];
            model.AddDate = DateTime.Now;
            model.AddUserID = useinfo.UserID;
            if (bll.GetCount<HelpContents>(string.Format("Title='{0}'",model.Title))>0)
            {
                return "-1";
            }
            if (bll.Add(model))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        
        }

        /// <summary>
        /// 编辑内容页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditHelpContent(HttpContext context)
        {

            var model = GetContentModel(context);
            model.AutoID =int.Parse(context.Request["AutoID"]);
            model.HelpContent = context.Request["Content"];
            if (bll.Update(model, string.Format("Title='{0}',HelpContent='{1}',Sort={2},Status={3}",model.Title,model.HelpContent,model.Sort,model.Status),string.Format("AutoID={0}",model.AutoID))>0)
            {
                return "保存成功";
            }
            else
            {
                return "保存失败";
            }

        }
        /// <summary>
        ///获取单条内容信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSingleHelpContent(HttpContext context) {
            var autoID = context.Request["AutoID"];
            var HelpContentInfo = bll.Get<HelpContents>(string.Format("AutoID='{0}'",autoID));
            if (HelpContentInfo!=null)
            {
                return string.Format("{0}★{1}★{2}★{3}",HelpContentInfo.Title,HelpContentInfo.HelpContent,HelpContentInfo.Sort,HelpContentInfo.Status);
            }
            else
            {
                return "";
            }
        
        
        }

        private string DeleteHelpContent(HttpContext context) {

            var ids = context.Request["id"];
           int count= bll.Delete(new HelpContents(), string.Format("AutoID in ({0})",ids));
            if (count>0)
            {
                return string.Format("成功删除了{0}条记录",count);
            }
            else
            {
                return "删除失败";
            }
        
        }

        ///// <summary>
        ///// 提交反馈
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public string SubmitFeedBack(HttpContext context) {

        //    string Title = context.Request["Title"];
        //    string Content = context.Request["Content"];
        //    if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Content))
        //    {
        //        return "-1";
        //    }
        //    ZentCloud.BLLJIMP.Model.FeedBack model = new FeedBack();
        //    model.Title = Title;
        //    model.FeedBackContent = Content;
        //    model.AddDate = DateTime.Now;
        //    model.UserID = useinfo.UserID;
        //    model.Status = 0;
        //    if (bll.Add(model))
        //    {
        //        return "1";
        //    }
        //    else
        //    {
        //        return "0";
        //    }

        
        //}

        /// <summary>
        /// 反馈查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryFeedBack(HttpContext context)
        {

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
         
            var searchCondition = "";
            List<ZentCloud.BLLJIMP.Model.FeedBack> list = bll.GetLit<ZentCloud.BLLJIMP.Model.FeedBack>(rows, page, searchCondition, "AddDate DESC");
            //for (int i = 0; i < list.Count; i++)
            //{
            //    list[i].FeedBackContent = null;
            //    list[i].ReplyContent = null;
            //}
            int totalCount = bll.GetCount<ZentCloud.BLLJIMP.Model.FeedBack>(searchCondition);
            string jsonResult = ZentCloud.Common.JSONHelper.ListToEasyUIJson(totalCount, list);
            return jsonResult;




        }
        /// <summary>
        /// 删除反馈
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteFeedBack(HttpContext context)
        {

            var ids = context.Request["id"];
            int count = bll.Delete(new ZentCloud.BLLJIMP.Model.FeedBack(), string.Format("AutoID in ({0})", ids));
            if (count > 0)
            {
                return string.Format("成功删除了{0}条记录", count);
            }
            else
            {
                return "删除失败";
            }

        }

        /// <summary>
        /// 获取传入的实体
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private HelpContents GetContentModel(HttpContext context)
        {
            return ZentCloud.Common.JSONHelper.JsonToModel<BLLJIMP.Model.HelpContents>(context.Request["JsonData"]);
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