using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Monitor
{
    public partial class MonitorEventDetails : System.Web.UI.Page
    {
        public StringBuilder strSearchHtml = new StringBuilder();
        public StringBuilder strGrid = new StringBuilder();
        string planId = string.Empty;
        string eventType = string.Empty;
        string linkId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                planId = Request["planId"];
                eventType = Request["EventType"];
                linkId = Request["linkid"];
                if (linkId==null)
                {
                    linkId = "";
                }
                this.ViewState["planId"] = planId;
                this.ViewState["eventType"] = "";
                this.ViewState["linkid"] = linkId;
                this.CreateGrid();


            }
        }

        private void CreateGrid()
        {

            strGrid.AppendLine("{ field: 'SourceIP', title: 'IP地址', width: 100, align: 'left' },");
            strGrid.AppendLine("{ field: 'IPLocation', title: 'IP所在地', width: 100, align: 'left' },");
            strGrid.AppendLine("{ field: 'EventBrowserID', title: '浏览器', width: 100, align: 'left' },");
            strGrid.AppendLine("{ field: 'EventDate', title: '触发时间', width: 100, align: 'left',formatter:FormatDate }");
           
            if (this.eventType == "1")
            {
                strGrid.AppendLine(", { field: 'ClickUrl', title: '点击URL', width: 100, align: 'left' }");
            }


        }

    }
}