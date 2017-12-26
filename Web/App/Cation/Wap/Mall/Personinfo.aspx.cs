using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class Personinfo : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo userInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bll.IsLogin)
            {
                userInfo = DataLoadTool.GetCurrUserModel();

            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }


        }

    }
}