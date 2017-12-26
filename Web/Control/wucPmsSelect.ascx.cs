using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucPmsSelect : System.Web.UI.UserControl
    {
        private List<string> selectValue;

        public List<string> SelectValue
        {
            get
            {
                return MySpider.ASPNET.CheckBoxListHelper.GetCheckedList(this.chkData);
            }

            set { selectValue = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ZentCloud.BLLPermission.BLLMenuPermission menuBll = new BLLPermission.BLLMenuPermission("");

                this.ddlData.Items.Clear();
                this.ddlData.Items.Add(new ListItem("无所属", "0"));
                foreach (ZentCloud.BLLPermission.Model.PermissionInfo item in menuBll.GetList<ZentCloud.BLLPermission.Model.PermissionInfo>(""))
                {
                    this.ddlData.Items.Add(new ListItem(item.PermissionName, item.PermissionID.ToString()));
                    this.chkData.Items.Add(new ListItem(item.PermissionName, item.PermissionID.ToString()));
                }
                //this.ddlData.SelectedValue = this.selectValue.ToString();
            }

        }
    }
}