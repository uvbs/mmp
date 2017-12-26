using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucMenuSelect : System.Web.UI.UserControl
    {
        private long selectValue;

        public long SelectValue 
        {
            set { this.selectValue = value; }
            get
            {
                return long.Parse(this.ddlData.SelectedValue);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ZentCloud.BLLPermission.BLLMenuPermission menuBll = new BLLPermission.BLLMenuPermission("");

                this.ddlData.Items.Clear();
                this.ddlData.Items.Add(new ListItem("无所属", "0"));
                foreach (ZentCloud.BLLPermission.Model.MenuInfo item in menuBll.GetList<ZentCloud.BLLPermission.Model.MenuInfo>(""))
                {
                    this.ddlData.Items.Add(new ListItem(item.NodeName, item.MenuID.ToString()));
                    
                }
                this.ddlData.SelectedValue = this.selectValue.ToString();
            }
           
        }
    }
}