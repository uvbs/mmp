using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class MyAddressList : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 收货地址列表
        /// </summary>
        public List<WXConsigneeAddress> AddressList;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (bllMall.IsLogin)
            {
                AddressList=bllMall.GetConsigneeAddressList(DataLoadTool.GetCurrUserID());

            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }

        }
    }
}