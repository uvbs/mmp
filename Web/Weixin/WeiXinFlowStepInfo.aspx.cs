using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class WeiXinFlowStepInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 传入的流程ID
        /// </summary>
        public string FlowID;
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName;
        private BLL bll = new BLL("");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                //获取传入的流程ID
                FlowID = Request.QueryString["FlowID"];
                //流程名称
               
                   var flowinfo= bll.Get<WXFlowInfo>(string.Format(" FlowID={0}", FlowID));
                   if (flowinfo==null)
                   {
                     Response.Redirect("/Weixin/WeiXinFlowInfoManage.aspx");
                       
                   }
                   FlowName = flowinfo.FlowName;
                var userid = Comm.DataLoadTool.GetCurrUserID();
                if (bll.Get<WXFlowInfo>(string.Format("FlowID={0} and UserID='{1}'", FlowID, userid)) == null)
                {
                    Response.Redirect("/Weixin/WeiXinFlowInfoManage.aspx");

                }
            }



        }
    }
}
