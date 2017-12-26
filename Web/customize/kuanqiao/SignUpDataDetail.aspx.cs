using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.kuanqiao
{
	public partial class SignUpDataDetail : System.Web.UI.Page
	{

        public ActivityDataInfo model;
        BLLJIMP.BLLActivity bll = new BLLJIMP.BLLActivity("");
		protected void Page_Load(object sender, EventArgs e)
		{
            try
            {
                model = bll.GetActivityDataInfo("130725", int.Parse(Request["uid"]));
                if (model == null)
                {
                    Response.End();
                }
            }
            catch (Exception)
            {

                Response.End();
            }


		}
	}
}