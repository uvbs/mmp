using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucUserType : System.Web.UI.UserControl
    {
        private string selectValue = "";

        public string SelectValue
        {
            get { return this.ddlData.SelectedValue; }
            set { selectValue = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BLLControl bllControl = new BLLControl(Session[Comm.SessionKey.UserID].ToString());

                this.ddlData.Items.Clear();
                foreach (CodeListInfo item in bllControl.GetList<CodeListInfo>(" CodeType = 'userType' "))
                {
                    this.ddlData.Items.Add(new ListItem(item.CodeName, item.CodeValue));
                }
                this.ddlData.SelectedValue = "2";
                this.ddlData.SelectedValue = this.selectValue;
            }
        }

    }
}