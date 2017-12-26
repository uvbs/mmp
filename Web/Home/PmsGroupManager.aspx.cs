using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class PmsGroupManager : System.Web.UI.Page
    {
        ZentCloud.BLLPermission.BLLMenuPermission menuBll;

        protected void Page_Load(object sender, EventArgs e)
        {
            menuBll = new BLLPermission.BLLMenuPermission(Comm.DataLoadTool.GetCurrUserID());
            this.LoadData();
        }

        private void LoadData()
        {
            this.grvData.DataSource = this.menuBll.GetList<ZentCloud.BLLPermission.Model.PermissionGroupInfo>("");
            this.grvData.DataBind();

            this.grvGroupAndPms.DataSource = this.menuBll.GetList<ZentCloud.BLLPermission.Model.PermissionRelationInfo>("");
            this.grvGroupAndPms.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
           
            string GroupName = this.txtGroupName.Text;
            string GroupDescription = this.txtGroupDescription.Text;
           

            ZentCloud.BLLPermission.Model.PermissionGroupInfo model = new ZentCloud.BLLPermission.Model.PermissionGroupInfo();
            model.GroupID = long.Parse( menuBll.GetGUID(Common.TransacType.PermissionGroupAdd));
            model.GroupName = GroupName;
            model.GroupDescription = GroupDescription;
            model.PreID = 0;
            if (menuBll.Add(model))
                Common.WebMessageBox.ShowAndRedirect(this, "添加成功!", Request.RawUrl);
            else
                Common.WebMessageBox.Show(this, "添加失败!");

        }

        protected void grvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.ViewState["uid"] = this.grvData.DataKeys[e.NewSelectedIndex].Value;

            this.txtGroupName.Text = this.ViewState["uid"].ToString();

        }

        protected void btnSetPms_Click(object sender, EventArgs e)
        {


            foreach (string item in this.wucPmsSelect.SelectValue)
            {
                ZentCloud.BLLPermission.Model.PermissionRelationInfo model = new BLLPermission.Model.PermissionRelationInfo();

                model.RelationID = this.ViewState["uid"].ToString();
                model.RelationType = 0;
                model.PermissionID = long.Parse(item);

                this.menuBll.Add(model);
            }
        }



    }
}