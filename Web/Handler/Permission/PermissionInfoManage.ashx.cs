using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Handler.Permission
{
    /// <summary>
    /// PermissionInfoManage 权限管理
    /// </summary>
    public class PermissionInfoManage : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 权限BLL
        /// </summary>
        BLLMenuPermission bllPer = new BLLMenuPermission("");
        /// <summary>
        /// 菜单BLL
        /// </summary>
        BLLMenuInfo bllMenu = new BLLMenuInfo();
        /// <summary>
        /// 用户BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = "true";
            try
            {
                if (!bllUser.GetCurrentUserInfo().UserType.Equals(1))
                {

                    return;
                }
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
                        result = Query(context);
                        break;
                    case "GetMenuSelectList":
                        result = GetMenuSelectList(context);
                        break;
                    case "BatchSetCategory":
                        result = BatchSetCategory(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
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
            List<MenuInfo> menuList = bllMenu.GetWebsiteMenuList(null, 2, false, false);
            result = new MySpider.MyCategories().GetSelectOptionHtml(menuList, "MenuID", "PreID", "NodeName", 0, "ddlPreMenu", "width:150px", "");
            return result.ToString();
        }

        private string Delete(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Permission_DeletePermission))
            //{
            //    return "无权删除权限";
            //}
            //#endregion
            string ids = context.Request["ids"];
            //删除权限前 删除相关权限组关联
            //删除权限权限组对应关系
            if (bllPer.Delete(new PermissionRelationInfo(), string.Format("PermissionID in({0}) ", ids)) > 0)
            {

            }

            int result = bllPer.Delete(new PermissionInfo(), string.Format(" PermissionID in ({0})", ids));//pmsBll.DeleteUser(idsList);
            return result.ToString();

        }

        private string Add(HttpContext context)
        {


            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Permission_AddPermission))
            //{
            //    return "无权添加权限";
            //}
            //#endregion

            string jsonData = context.Request["JsonData"];
            PermissionInfo model = ZentCloud.Common.JSONHelper.JsonToModel<PermissionInfo>(jsonData);
            string msg = string.Empty;
            bool result = bllPer.AddPms(model, out msg);

            if (result)
                return result.ToString().ToLower();
            else
                return msg;
        }

        private string Edit(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Permission_EditPermission))
            //{
            //    return "无权编辑权限";
            //}
            //#endregion
            string jsonData = context.Request["JsonData"];
            PermissionInfo model = ZentCloud.Common.JSONHelper.JsonToModel<PermissionInfo>(jsonData);
            bool result = bllPer.Update(model);
            return result.ToString().ToLower();
        }

        private string BatchSetCategory(HttpContext context)
        {
            string ids = context.Request["ids"];
            string permissionCateId = context.Request["PermissionCateId"];
            bool result = bllPer.Update(new PermissionInfo(), string.Format("PermissionCateId={0}", permissionCateId), string.Format("PermissionID In ({0})", ids)) > 0;
            return result.ToString().ToLower();
        }

        

        private string Query(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);

            string keyWord = context.Request["searchReq"];
            string permissionKey = context.Request["key"];
            
            string type = context.Request["type"];
            string cateID = context.Request["cateID"];
            sbWhere.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(type))
                sbWhere.AppendFormat(" AND PermissionType={0}", type);
            if (!string.IsNullOrWhiteSpace(cateID) && cateID != "0")
            {
                sbWhere.AppendFormat(" AND PermissionCateId ={0}", cateID);
            }
            if (!string.IsNullOrWhiteSpace(permissionKey))
                sbWhere.AppendFormat(" AND PermissionKey like '%{0}%'", permissionKey);
            if (!string.IsNullOrWhiteSpace(keyWord))
                sbWhere.AppendFormat(" AND PermissionName like '%{0}%' Or Url like '%{0}%'", keyWord);

            List<PermissionInfo> list = bllPer.GetLit<PermissionInfo>(pageSize, pageIndex, sbWhere.ToString(), "PermissionName");
            int totalCount = bllPer.GetCount<PermissionInfo>(sbWhere.ToString());
            if (list.Count > 0)
            {
                BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
                List<ZentCloud.BLLJIMP.Model.ArticleCategory> cateList = new BLLJIMP.BLLArticleCategory().GetList<ZentCloud.BLLJIMP.Model.ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}' Order by Sort", "Common", "Permission"));
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].PermissionCateId == 0) continue;
                    ZentCloud.BLLJIMP.Model.ArticleCategory nowType = cateList.FirstOrDefault(p => p.AutoID == list[i].PermissionCateId);
                    if (nowType != null) list[i].PermissionCate = nowType.CategoryName;
                }
            }
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