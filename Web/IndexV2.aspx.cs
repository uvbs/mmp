using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLPermission;
using ZentCloud.Common;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web
{
    public partial class IndexV2 : System.Web.UI.Page
    {

        protected WebsiteInfo websiteInfo;
        /// <summary>
        /// 菜单
        /// </summary>
        protected string menuString = "";
        /// <summary>
        /// 版本
        /// </summary>
        protected string WebsiteVersion = "";
        /// <summary>
        /// 到期时间
        /// </summary>
        protected string ExpirationDate = "";
        ///// <summary>
        ///// 是否可用
        ///// </summary>
        //protected bool IsValid = true;
        /// <summary>
        /// 站点名称
        /// </summary>
        protected string WebsiteName = "";
        /// <summary>
        /// 退出登录链接
        /// </summary>
        protected string LogoutHref = "";
        /// <summary>
        /// 网站LOGO
        /// </summary>
        protected string WebsiteLogo = "";
        /// <summary>
        /// 当前账户
        /// </summary>
        protected string curUserID = "";
        /// <summary>
        /// 当前网站
        /// </summary>
        protected string websiteOwner = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            /// <summary>
            /// 菜单BLL
            /// </summary>
            BLLMenuPermission bllMenu = new BLLMenuPermission("");
            /// <summary>
            /// 权限BLL
            /// </summary>
            BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
            /// <summary>
            /// 站点BLL
            /// </summary>
            BLLWebSite bllWebSite = new BLLWebSite();

            UserInfo currUser = DataLoadTool.GetCurrUserModel();
            if (currUser == null)
            {
                this.Response.Redirect(ConfigHelper.GetConfigString("logoutUrl"));
                return;
            }

            websiteInfo = bllWebSite.GetWebsiteInfoModelFromDataBase();

            curUserID = currUser.UserID;
            websiteOwner = bllWebSite.WebsiteOwner;
            if (bllPms.IsActionPermissionV2(websiteOwner))
            {
                if (currUser.PermissionGroupID.HasValue)
                {
                    ZentCloud.BLLPermission.Model.PermissionGroupInfo perGroupInfo = bllPms.Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format(" GroupID={0}", currUser.PermissionGroupID));
                    if (perGroupInfo!=null&&perGroupInfo.GroupType == 3)//管理员权限跟站点所有者一致
                    {
                        menuString = bllMenu.GetNewUserMenuTreeHtmlV2(websiteOwner, websiteOwner);
                    }
                    else
                    {
                        menuString = bllMenu.GetNewUserMenuTreeHtmlV2(curUserID, websiteOwner);
                    }
                }
                else
                {
                    menuString = bllMenu.GetNewUserMenuTreeHtmlV2(curUserID, websiteOwner);
                }
                
            }
            else
            {
                menuString = bllMenu.GetNewUserMenuTreeHtml(curUserID, websiteOwner);
            }
            WebsiteVersion = bllWebSite.GetWebsiteVersion(websiteOwner);
            
            WebsiteLogo = websiteInfo != null ? websiteInfo.WebsiteLogo : "";
            if (string.IsNullOrEmpty(websiteInfo.WebsiteLogo))
            {
                WebsiteLogo = ConfigHelper.GetConfigString("WebsiteLogo");
            }
            ExpirationDate = websiteInfo.WebsiteExpirationDate.HasValue ? websiteInfo.WebsiteExpirationDate.Value.ToString("yyyy-MM-dd") : "";
            //IsValid = websiteInfo.WebsiteExpirationDate.HasValue && websiteInfo.WebsiteExpirationDate.Value.AddDays(1) < DateTime.Now ? false : true;
            WebsiteName = websiteInfo.WebsiteName;
            LogoutHref = ConfigHelper.GetConfigString("logoutUrl") + "?op=logout";



        }
    }
}