using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.ZCDALEngine;

namespace ZentCloud.JubitIMP.Web.Admin.TableFieldMap
{
    public partial class List : System.Web.UI.Page
    {
        protected string module_name;
        protected string table_name = "ZCJ_UserInfo";
        protected List<string> limitForeach = new List<string>() { "AutoID", "Password" };
        protected List<string> fieldList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            module_name = Request["module_name"];
            if (!string.IsNullOrWhiteSpace(Request["table_name"])) table_name = Request["table_name"];
            MetaTable metaTable = DALEngine.GetMetas().Tables[table_name];
            fieldList = metaTable.Columns.Keys.Where(p => !limitForeach.Contains(p)).ToList();
        }
    }
}