using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WeiXinOpen
{
    /// <summary>
    /// 微信开放平台授权页
    /// </summary>
    public partial class OAuth : System.Web.UI.Page
    {
        /// <summary>
        /// 微信开放平台BLL
        /// </summary>
        BLLWeixinOpen bll = new BLLWeixinOpen();
        protected void Page_Load(object sender, EventArgs e)
        {

            string redirectUrl = string.Format("http://{0}/WeixinOpen/OAuthCallBack.ashx", ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenOAuthDoMain"));//回调地址
            string preAuthCode=bll.GetPreAuthCode();//预授权码
            if (!string.IsNullOrEmpty(preAuthCode))
            {
                string url = string.Format("https://mp.weixin.qq.com/cgi-bin/componentloginpage?component_appid={0}&pre_auth_code={1}&redirect_uri={2}", bll.ComponentAppId, preAuthCode, redirectUrl);
                Response.Redirect(url);//跳转到授权页
            }
            else
            {
                Response.Write("获取预授权码失败");
                Response.End();
            }

           


        }
    }
}