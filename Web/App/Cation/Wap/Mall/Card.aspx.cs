using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class Card : System.Web.UI.Page
    {

        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo currentWebsiteInfo;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo userInfo;
        public bool boolIsSupplementUserInfo = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bll.IsLogin)
            {
                currentWebsiteInfo = bll.GetWebsiteInfoModel();
                userInfo = DataLoadTool.GetCurrUserModel();
                boolIsSupplementUserInfo = IsSupplementUserInfo();
            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }


        }


        /// <summary>
        /// 是否补充个人资料
        /// </summary>
        /// <returns></returns>
        public bool IsSupplementUserInfo()
        {
            if (userInfo!=null)
            {
                if (!string.IsNullOrEmpty(userInfo.TrueName))
                {
                    if (!string.IsNullOrEmpty(userInfo.Gender))
                    {
                        if (!string.IsNullOrEmpty(userInfo.Phone))
                        {
                            if (!string.IsNullOrEmpty(userInfo.Email))
                            {
                                
                                    return false;
                                

                            }

                        }
                        
                    }
                }
            }
            return true;

        
        
        
        }


    }
}