using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using ZentCloud.Common.Model;

namespace ZentCloud.JubitIMP.Web.Admin.User
{
    public partial class ChildChannelSupplier : System.Web.UI.Page
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //BLLJIMP.BLL bll = new BLLJIMP.BLL();
        ///// <summary>
        ///// 
        ///// </summary>
        //BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        ///// <summary>
        ///// 渠道列表
        ///// </summary>
        //public List<ListItem> AllChannelList = new List<ListItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string permissionGroupId = bllDis.GetChannelPermissionGroupId();//渠道用户组
            //if (string.IsNullOrEmpty(permissionGroupId))
            //{
            //    permissionGroupId = "0";
            //}
            //var allChannelList = bll.GetList<UserInfo>(string.Format(" WebsiteOwner = '{0}' And PermissionGroupID={1} ", bll.WebsiteOwner, permissionGroupId));
            //try
            //{
            //    MyCategoriesV2 myCategories = new MyCategoriesV2();
            //    List<MyCategoryV2Model> myCategoryModel = myCategories.GetCommCateModelList("UserID", "ParentChannel", "ChannelName", allChannelList);
            //    AllChannelList = myCategories.GetCateListItem(myCategoryModel, "");

            //}
            //catch (Exception ex)
            //{

            //    Response.Write(ex.ToString());
            //}


        }
    }
}