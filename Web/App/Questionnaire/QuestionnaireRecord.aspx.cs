using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Questionnaire
{
    public partial class QuestionnaireRecord : System.Web.UI.Page
    {
        protected string type;
        protected string typeName;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            type = string.IsNullOrWhiteSpace(Request["type"]) ? "0" : Request["type"];
            typeName = type == "0" ? "题库" : "问卷";
        }
    }
}