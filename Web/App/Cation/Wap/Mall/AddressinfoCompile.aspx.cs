using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class AddressinfoCompile : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public WXConsigneeAddress AddressInfo=new WXConsigneeAddress();
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (bllMall.IsLogin)
            {
                if (!string.IsNullOrEmpty(Request["action"]))
                {
                    if ((Request["action"].Equals("edit")) && (!string.IsNullOrEmpty(Request["id"])))
                    {

                        AddressInfo = bllMall.GetConsigneeAddress(Request["id"]);
                        if (AddressInfo != null)
                        {
                            if (AddressInfo.UserID != DataLoadTool.GetCurrUserID())
                            {
                                Response.End();
                            }
                        }
                    }

                }

            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.Url.PathAndQuery));
            }







        }
    }
}