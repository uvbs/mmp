using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class PmsGroupManagerV2 : System.Web.UI.Page
    {
        ///// <summary>
        ///// 权限组管理-添加
        ///// </summary>
        //public bool Pms_PermissionGroup_AddPermissionGroup;
        ///// <summary>
        ///// 权限组管理-编辑
        ///// </summary>
        //public bool Pms_PermissionGroup_EditPermissionGroup;
        ///// <summary>
        ///// 权限组管理-删除
        ///// </summary>
        //public bool Pms_PermissionGroup_DeletePermissionGroup;
        ///// <summary>
        ///// 权限组管理-分配权限
        ///// </summary>
        //public bool Pms_PermissionGroup_AssignationPermissionGroup;
        protected string PermissionHtml;
        protected string MenuHtml;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Pms_PermissionGroup_AddPermissionGroup = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_AddPermissionGroup);

            //Pms_PermissionGroup_EditPermissionGroup = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_EditPermissionGroup);

            //Pms_PermissionGroup_DeletePermissionGroup = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_DeletePermissionGroup);

            //Pms_PermissionGroup_AssignationPermissionGroup = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_AssignationPermissionGroup);
            LoadPermission();
            LoadMenu();
        }
        private void LoadPermission()
        {

            BLLJuActivity juActivityBll = new BLLJuActivity();
            ZentCloud.BLLPermission.BLLMenuPermission permissionBll = new ZentCloud.BLLPermission.BLLMenuPermission("");
            StringBuilder sb = new StringBuilder();
            int columCount = 3;
            int _w = 740 / columCount;
            List<ArticleCategory> cateList = juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}' Order by Sort", juActivityBll.WebsiteOwner, "Permission"));
            foreach (var cateitem in cateList)
            {
                List<ZentCloud.BLLPermission.Model.PermissionInfo> pmsGroup = permissionBll.GetList<ZentCloud.BLLPermission.Model.PermissionInfo>(string.Format(" PermissionCateId={0} order by PermissionID DESC", cateitem.AutoID));
                if (pmsGroup.Count > 0)
                {
                    sb.AppendFormat("<fieldset style=\"padding: 0px 10px 10px 10px; margin-top:10px; \">");
                    sb.AppendFormat("<legend><input id=\"cb_{0}\" title=\"{1}\" type=\"checkbox\" name=\"checktype\" class=\"positionTop2\" value=\"{0}\" /> <label title=\"{1}\" for=\"cb_{0}\">{1}</label></legend>", cateitem.AutoID, cateitem.CategoryName);
                    sb.AppendFormat("<ul style=\"width:100%;\">");

                    for (int i = 0; i < pmsGroup.Count; i++)
                    {
                        sb.AppendFormat("<li style=\"width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;\">", _w);
                        sb.AppendFormat(string.Format(@"<input id=""cb_{0}"" title=""{2}"" type=""checkbox"" name=""checksingle"" class=""positionTop2"" value=""{0}"" /> <label title=""{2}"" for=""cb_{0}"">{1}</label><br />", pmsGroup[i].PermissionID, pmsGroup[i].PermissionName, string.IsNullOrWhiteSpace(pmsGroup[i].PermissionDescription) ? pmsGroup[i].PermissionName : pmsGroup[i].PermissionDescription));
                        sb.AppendFormat("</li>");
                    }
                    sb.AppendFormat("</ul>");
                    sb.AppendFormat("</fieldset>");
                }
            }
            string cateIds = ZentCloud.Common.MyStringHelper.ListToStr(cateList.Select(p => p.AutoID).ToList(), "", ",");
            if (string.IsNullOrWhiteSpace(cateIds)) cateIds = "-1";
            List<ZentCloud.BLLPermission.Model.PermissionInfo> pmsGroup1 = new ZentCloud.BLLPermission.BLLMenuPermission("").GetList<ZentCloud.BLLPermission.Model.PermissionInfo>(string.Format(" PermissionCateId Not In ({0}) order by PermissionID DESC", cateIds));
            if (pmsGroup1.Count > 0)
            {
                sb.AppendFormat("<fieldset style=\"0px 10px 10px 10px; margin-top:10px;\">");
                sb.AppendFormat("<legend><input id=\"cb_{0}\" title=\"{1}\" type=\"checkbox\" name=\"checktype\" class=\"positionTop2\" value=\"{0}\" /> <label title=\"{1}\" for=\"cb_{0}\">{1}</label></legend>", 0, "未分类");
                sb.AppendFormat("<ul style=\"width:100%;\">");

                for (int i = 0; i < pmsGroup1.Count; i++)
                {
                    sb.AppendFormat("<li style=\"width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;\">", _w);
                    sb.AppendFormat(string.Format(@"<input id=""cb_{0}"" title=""{2}"" type=""checkbox"" name=""checksingle"" class=""positionTop2"" value=""{0}"" /> <label title=""{2}"" for=""cb_{0}"">{1}</label><br />", pmsGroup1[i].PermissionID, pmsGroup1[i].PermissionName, string.IsNullOrWhiteSpace(pmsGroup1[i].PermissionDescription) ? pmsGroup1[i].PermissionName : pmsGroup1[i].PermissionDescription));
                    sb.AppendFormat("</li>");
                }
                sb.AppendFormat("</ul>");
                sb.AppendFormat("</fieldset>");

            }
            PermissionHtml = sb.ToString();
        }
        private void LoadMenu()
        {

            BLLMenuInfo bllMenu = new BLLMenuInfo();
            StringBuilder sb = new StringBuilder();
            int columCount = 5;
            int _w = 740 / columCount;
            List<MenuInfo> listMenu = bllMenu.GetMenus(0, 1, 1, null, false, 2, "2,3");
            foreach (var parentMenu in listMenu)
            {
                if (parentMenu.ChildMenus.Count > 0)
                {
                    sb.AppendFormat("<fieldset style=\"padding: 0px 10px 10px 10px; margin-top:10px; \">");
                    sb.AppendFormat("<legend><input id=\"cmb_{0}\" title=\"{1}\" type=\"checkbox\" name=\"checkmenutype\" class=\"positionTop2\" value=\"{0}\" /> <label title=\"{1}\" for=\"cmb_{0}\">{1}</label></legend>", parentMenu.MenuID, parentMenu.NodeName);
                    sb.AppendFormat("<ul style=\"width:100%;\">");

                    foreach (var childMenu in parentMenu.ChildMenus)
                    {
                        sb.AppendFormat("<li style=\"width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;\">", _w);
                        sb.AppendFormat(string.Format(@"<input id=""cmb_{0}"" title=""{2}"" type=""checkbox"" name=""checkmenu"" class=""positionTop2"" value=""{0}"" /> <label title=""{2}"" for=""cmb_{0}"">{1}</label><br />", childMenu.MenuID, childMenu.NodeName, childMenu.NodeName));
                        sb.AppendFormat("</li>");
                    }
                    sb.AppendFormat("</ul>");
                    sb.AppendFormat("</fieldset>");
                }
            }
            MenuHtml = sb.ToString();
        }
    }
}