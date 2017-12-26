using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Flow
{
    public partial class List : System.Web.UI.Page
    {
        protected string module_name = "流程";
        protected string flow_key = "";
        protected string hide_status = "";
        protected string def_status = "0,11";
        protected string hide_status_str = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request["module_name"])) module_name = this.Request["module_name"];
            if (!string.IsNullOrWhiteSpace(this.Request["flow_key"])) flow_key = this.Request["flow_key"];
            if (!string.IsNullOrWhiteSpace(this.Request["hide_status"])) {
                hide_status = this.Request["hide_status"];
                if (hide_status == "all")
                {
                    def_status = "";
                }
                else
                {
                    hide_status_str = "," + hide_status + ",";
                }
            }
        }
    }
}