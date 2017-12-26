using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class QuestionList : System.Web.UI.Page
    {
        /// <summary>
        /// 专家ID
        /// </summary>
        public string MasterID = "";
        /// <summary>
        /// 回复状态
        /// </summary>
        public string FeedBackStatus = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Comm.DataLoadTool.currIsHFAdmin() && !Comm.DataLoadTool.currIsHFVipUser())
            //{
            //    Response.Redirect("/Error/novippms.htm");
            //    Response.End();
            //    return;
            //}

            if (Request["masterid"] != null)
            {
                MasterID = Request["masterid"];
            }
            if (Request["feedbackstatus"] != null)
            {
                FeedBackStatus = Request["feedbackstatus"];
            }



        }
    }
}