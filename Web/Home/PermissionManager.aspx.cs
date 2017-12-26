using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class PermissionManager : System.Web.UI.Page
    {
        ZentCloud.BLLPermission.BLLMenuPermission menuBll;

        protected void Page_Load(object sender, EventArgs e)
        {
            menuBll = new BLLPermission.BLLMenuPermission(Comm.DataLoadTool.GetCurrUserID());

            this.LoadData();

        }

        private void LoadData()
        {
            this.grvData.DataSource = this.menuBll.GetList<ZentCloud.BLLPermission.Model.PermissionInfo>("");
            this.grvData.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            string Url = this.txtUrl.Text;
            string PermissionName = this.txtPermissionName.Text;
            string PermissionDescription = this.txtPermissionDescription.Text;

            ZentCloud.BLLPermission.Model.PermissionInfo model = new BLLPermission.Model.PermissionInfo();
            model.PermissionID = long.Parse(menuBll.GetGUID(Common.TransacType.PermissionAdd));
            model.Url = Url;
            model.MenuID = this.wucMenu.SelectValue;
            model.PermissionName = PermissionName;
            model.PermissionDescription = PermissionDescription;

            if (menuBll.Add(model))
                Common.WebMessageBox.ShowAndRedirect(this, "添加成功!", Request.RawUrl);
            else
                Common.WebMessageBox.Show(this, "添加失败!");

        }

    }

}