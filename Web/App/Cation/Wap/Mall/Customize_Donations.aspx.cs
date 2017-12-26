using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    /// <summary>
    /// 捐款
    /// </summary>
    public partial class Customize_Donations : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        BLLJIMP.BLLJuActivity bllArticle = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 是否已登录
        /// </summary>
        public bool IsLogin;
        /// <summary>
        /// 捐款人数
        /// </summary>
        public int PeopleCount;
        /// <summary>
        /// 捐款总金额
        /// </summary>
        public decimal TotalAmount;
        public UserInfo CurrentUserInfo = new UserInfo();
        public JuActivityInfo ArticleModel = new JuActivityInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            IsLogin = bll.IsLogin;
            if (bll.IsLogin)
            {
                CurrentUserInfo = bll.GetCurrentUserInfo();
            }
            PeopleCount = bll.GetCount<WXMallOrderInfo>(string.Format("WebsiteOwner='qianwei' And OrderUserID='system' And PaymentStatus=1"));
            TotalAmount = bll.GetList<WXMallOrderInfo>(string.Format("WebsiteOwner='qianwei' And OrderUserID='system' And PaymentStatus=1")).Sum(p=>p.TotalAmount);

            ArticleModel = bllArticle.GetJuActivity(274374);
            if (ArticleModel==null)
            {
                ArticleModel = new JuActivityInfo();
            }

        }
    }
}