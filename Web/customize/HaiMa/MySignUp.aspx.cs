using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.HaiMa
{
    public partial class MySignUp : HaiMaBase
    {
        BLLJIMP.BLLHaiMa bllHaiMa = new BLLJIMP.BLLHaiMa();
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((CurrentUserInfo.UserType.Equals(2)) && (bllHaiMa.IsReg(CurrentUserInfo))&&(!string.IsNullOrEmpty(CurrentUserInfo.Ex5)))
            {

            }
            else
            {
                Response.Write("报名过的销售服务店人员才能进入");
                Response.End();
            }
        }
    }
}