using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class MyFeedBack : System.Web.UI.Page
    {
        /// <summary>
        /// 当道用户ID
        /// </summary>
        public string currentUserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUserID = Comm.DataLoadTool.GetCurrUserID();
        }
    }
}