using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using System.Data;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class WeixinFlowStepInfoData : System.Web.UI.Page
    {
        /// <summary>
        /// 传入的流程ID
        /// </summary>
        public string FlowID;
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName;
        /// <summary>
        /// 显示的列名
        /// </summary>
        public string Columns;
        private BLL bll = new BLL("");
        private BLLWeixin bllweixin = new BLLWeixin("");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //获取传入的流程ID
                FlowID = Request.QueryString["FlowID"];
                //流程名称

                var flowinfo = bll.Get<WXFlowInfo>(string.Format(" FlowID={0}", FlowID));
                if (flowinfo == null)
                {
                    Response.Redirect("/Weixin/WeiXinFlowInfoManage.aspx");

                }
                FlowName = flowinfo.FlowName;
                var userid = Comm.DataLoadTool.GetCurrUserID();
                if (bll.Get<WXFlowInfo>(string.Format("FlowID={0} and UserID='{1}'", FlowID, userid)) == null)
                {
                    Response.Redirect("/Weixin/WeiXinFlowInfoManage.aspx");

                }
                List < WXFlowDataInfo > list = bll.GetList<WXFlowDataInfo>(1,string.Format("FlowID={0}", FlowID),"");
                DataTable datatable = bllweixin.GetMemberFlowDataView(list);
                foreach (DataColumn item in datatable.Columns)
                {
                  Columns+=string.Format(" <th field=\"{0}\" width=\"100\">{1} </th>",item.ColumnName,item.ColumnName); 
                            
                }

            }

        }
    }
}
