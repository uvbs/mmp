using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class MenuManager : System.Web.UI.Page
    {

        ZentCloud.BLLPermission.BLLMenuPermission menuBll;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            menuBll = new BLLPermission.BLLMenuPermission(Comm.DataLoadTool.GetCurrUserID());
            if (!IsPostBack)
            {
                this.ViewState["op"] = "add";
                this.LoadData();
            }
        }

        private void LoadData()
        {
            this.grvData.DataSource = menuBll.GetList<ZentCloud.BLLPermission.Model.MenuInfo>("").OrderByDescending(p => p.MenuID);
            this.grvData.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            ZentCloud.BLLPermission.Model.MenuInfo menu = new BLLPermission.Model.MenuInfo();

            switch (this.ViewState["op"].ToString())
            {
                case "add":
                    menu.MenuID = long.Parse(menuBll.GetGUID(Common.TransacType.MenuAdd));

                    //menu.NodeName = this.txtNodeName.Text;
                    //menu.Url = this.txtUrl.Text.ToLower();
                    //menu.PreID = this.wucPreMenu.SelectValue;
                    //menu.MenuSort = int.Parse(this.txtMenuSort.Text);
                    //menu.ICOCSS = this.txtICOCSS.Text;

                    menu = this.GetControlData(menu);

                    if (menuBll.Add(menu))
                        Common.WebMessageBox.ShowAndRedirect(this, "添加成功!", Request.RawUrl);
                    else
                        Common.WebMessageBox.Show(this, "添加失败!");
                    break;
                case "update":
                    menu = menuBll.Get<ZentCloud.BLLPermission.Model.MenuInfo>(string.Format(" MenuID = {0} ", this.ViewState["MenuID"].ToString()));

                    menu = this.GetControlData(menu);

                    if (menuBll.Update(menu))
                        Common.WebMessageBox.ShowAndRedirect(this, "更改成功!", Request.RawUrl);
                    else
                        Common.WebMessageBox.Show(this, "更改失败!");

                    break;
                default:
                    break;
            }

        }

        private ZentCloud.BLLPermission.Model.MenuInfo GetControlData(ZentCloud.BLLPermission.Model.MenuInfo menu)
        {
            menu.NodeName = this.txtNodeName.Text;
            menu.Url = this.txtUrl.Text.ToLower();
            menu.PreID = this.wucPreMenu.SelectValue;
            menu.MenuSort = int.Parse(this.txtMenuSort.Text);
            menu.ICOCSS = this.txtICOCSS.Text;
            return menu;
        }

        protected void grvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.ViewState["MenuID"] = this.grvData.DataKeys[e.NewSelectedIndex].Value;
            this.ViewState["op"] = "update";
            ZentCloud.BLLPermission.Model.MenuInfo menu = menuBll.Get<ZentCloud.BLLPermission.Model.MenuInfo>(string.Format(" MenuID = {0} ", this.ViewState["MenuID"].ToString()));
            this.txtNodeName.Text = menu.NodeName;
            this.txtUrl.Text = menu.Url;
            this.wucPreMenu.SelectValue = menu.PreID;
            this.txtMenuSort.Text = menu.MenuSort.ToString();
            this.txtICOCSS.Text = menu.ICOCSS;

        }
    }
}