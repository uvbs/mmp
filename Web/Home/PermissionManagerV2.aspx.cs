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

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class PermissionManagerV2 : System.Web.UI.Page
    {
        ///// <summary>
        ///// 权限管理-添加
        ///// </summary>      
        //public bool Pms_Permission_AddPermission;
        ///// <summary>
        /////  权限管理-编辑
        ///// </summary>
        //public bool Pms_Permission_EditPermission;
        ///// <summary>
        ///// 权限管理-删除
        ///// </summary>
        //public bool Pms_Permission_DeletePermission;

        BLLJuActivity juActivityBll = new BLLJuActivity(DataLoadTool.GetCurrUserID());
        public int cateRootId = 0;
        public StringBuilder sbCategory = new StringBuilder();
        public StringBuilder sbCategory1 = new StringBuilder();
        public StringBuilder sbCategory2 = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Pms_Permission_AddPermission = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Permission_AddPermission);

            //Pms_Permission_EditPermission = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Permission_EditPermission);

            //Pms_Permission_DeletePermission = DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_Permission_DeletePermission);
            List<ArticleCategory> list = juActivityBll.GetList<ArticleCategory>(int.MaxValue, string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", "Common", "Permission"), "Sort");
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlPermissionCate", "width:200px", ""));
            sbCategory1.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlPermissionCate1", "width:200px", ""));
            sbCategory2.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlPermissionCate2", "width:200px", ""));
        }
    }
}