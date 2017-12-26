using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.mmpadmin
{
    public partial class Index : System.Web.UI.Page
    {
        /// <summary>
        /// 站点配置
        /// </summary>
        System.Text.StringBuilder sbConfig = new System.Text.StringBuilder();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWebsite = new BLLWebSite();
        /// <summary>
        /// 菜单权限BLL
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");

        protected void Page_Load(object sender, EventArgs e)
        {
            WebsiteInfo currentWebsiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
            UserInfo currUser = bllUser.GetCurrentUserInfo();
            if (currentWebsiteInfo.WebsiteExpirationDate.HasValue && 
                currentWebsiteInfo.WebsiteExpirationDate.Value.AddDays(1).AddSeconds(-1) < DateTime.Now &&
                currUser.UserType != 1)           {
                this.Response.Redirect("/Error/expire.htm");
            }
            string indexStr = File.ReadAllText(this.Server.MapPath("index.html"));
            //处理首页的 lib 引用路径
            indexStr = indexStr.Replace("files.comeoncloud.net", "static-files.socialcrmyun.com");
            indexStr = indexStr.Replace("http://static-files.socialcrmyun.com/lib/summernote/", "http://files.comeoncloud.net/lib/summernote/");//summernote有跨域问题

            this.Response.Clear();
            var companyConfig = bllWebsite.GetCompanyWebsiteConfig();
            
            var websiteConfig = new
            {
                is_enable_limit_product_buy_time = currentWebsiteInfo.IsEnableLimitProductBuyTime==0?false:true,//是否启用限制购买时间
                is_show_product_pv = bllMenuPermission.CheckUserAndPmsKey(bllUser.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.IsShowProductPv),//是否显示商品访问量列
                is_show_article_pv=bllMenuPermission.CheckUserAndPmsKey(bllUser.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.IsShowArticlePv),//是否显示文章访问量列
                is_show_activity_pv = bllMenuPermission.CheckUserAndPmsKey(bllUser.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.IsShowActivityPv),//是否显示活动访问量列
                weixin_bind_domain=!string.IsNullOrEmpty(currentWebsiteInfo.WeiXinBindDomain)?currentWebsiteInfo.WeiXinBindDomain:"",//微信绑定域名
                curr_user_id=currUser.UserID,
                is_open_group=currentWebsiteInfo.IsOpenGroup==1?true:false,
                stock_type=companyConfig.StockType//库存模式 
            };
            sbConfig.AppendFormat("</title>");
            sbConfig.AppendFormat("<script>");
            sbConfig.AppendFormat("var WEBSITE_CONFIG=");
            sbConfig.Append(ZentCloud.Common.JSONHelper.ObjectToJson(websiteConfig));
            sbConfig.AppendFormat("</script>");
            indexStr = indexStr.Replace("</title>", sbConfig.ToString());//图标文件
            string icoScript = bllUser.GetIcoScript();
            if (!string.IsNullOrWhiteSpace(icoScript))
            {
                //indexStr += icoScript;
            }
            this.Response.Write(indexStr);
            
            

        }
    }
}