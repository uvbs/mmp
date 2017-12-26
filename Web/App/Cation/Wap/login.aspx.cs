using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
	public partial class login : System.Web.UI.Page
    {
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string redirecturl;
		protected void Page_Load(object sender, EventArgs e)
		{
            

            redirecturl = Request.Url.PathAndQuery.Replace(string.Format("{0}?redirecturl=", Request.FilePath), null).Replace("/App/Cation/Wap/Login.aspx",null);


            


		}
	}
}