using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucSelectDateTime : System.Web.UI.UserControl
    {
        private string selectValue = string.Empty;

        public string SelectValue
        {
            get { return this.txtSelectDate.Value; }
            set { selectValue = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtSelectDate.Value = this.selectValue;
            }
        }
    }
}