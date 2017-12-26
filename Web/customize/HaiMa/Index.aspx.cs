using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.HaiMa
{
    public partial class Index :HaiMaBase
    {
        /// <summary>
        /// 是否可以报名
        /// </summary>
        public string CanSignUp = "false";
        BLLJIMP.BLLHaiMa bllHaiMa = new BLLJIMP.BLLHaiMa();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (bllHaiMa.IsReg(CurrentUserInfo))
            {
                if (CurrentUserInfo.UserType.Equals(2))// 2表示是销售店人员
                {
                    CanSignUp = "true";
                }
            }
        }
    }
}