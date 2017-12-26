using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud;
using ZentCloud.BLLJIMP;
using ZentCloud.JubitIMP.Web.Tool;
using ZentCloud.BLLJIMP.Model;
using System.Text;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class List : System.Web.UI.Page
    {
        ZentCloud.BLLPermission.BLLMenuPermission pmsBll;
        ZentCloud.BLLJIMP.BLLUser userBll;
        ZentCloud.BLLJIMP.BLLSMS smsBll;

        protected void Page_Load(object sender, EventArgs e)
        {

            this.userBll = new BLLUser(Comm.DataLoadTool.GetCurrUserID());
            this.pmsBll = new BLLPermission.BLLMenuPermission(Comm.DataLoadTool.GetCurrUserID());
            this.smsBll = new BLLSMS(Comm.DataLoadTool.GetCurrUserID());

            if (!IsPostBack)
            {
                this.LoadData();
            }
        }

        private void ShowMessge(string msg)
        {
            AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), msg);
        }

        private void ShowMessge(string msg, string url)
        {
            AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), msg, url);
        }

        private void LoadData()
        {
            StringBuilder strWhere = new StringBuilder();

            if (this.txtSearchUserID.Text.Trim() != "")
            {
                strWhere.AppendFormat(" UserID like '{0}%' ", this.txtSearchUserID.Text.Trim());
            }


            //设置总数
            this.AspNetPager1.RecordCount = this.userBll.GetCount<UserInfo>(strWhere.ToString());

            //this.grvData.DataSource = weiboBll.GetList<WeiboDetails>(strWhere.ToString());
            this.grvData.DataSource = this.userBll.GetLit<UserInfo>(this.AspNetPager1.PageSize, this.AspNetPager1.CurrentPageIndex, strWhere.ToString(), "Regtime DESC");
            this.DataBind();

            //设置分页显示
            this.AspNetPager1.CustomInfoHTML = string.Format("当前第{0}/{1}页 共{2}条记录 每页{3}条", this.AspNetPager1.CurrentPageIndex, this.AspNetPager1.PageCount, this.AspNetPager1.RecordCount, this.AspNetPager1.PageSize);

            this.ddlChargePipe.Items.Clear();
            foreach (BLLJIMP.Model.SMSChargePipeInfo item in this.smsBll.GetList<BLLJIMP.Model.SMSChargePipeInfo>())
            {
                this.ddlChargePipe.Items.Add(new ListItem(item.PipeID, item.PipeID));
            }

            this.ddlSendPipe.Items.Clear();
            foreach (BLLJIMP.Model.SMSSendPipeInfo item in this.smsBll.GetList<BLLJIMP.Model.SMSSendPipeInfo>())
            {
                this.ddlSendPipe.Items.Add(new ListItem(item.PipeID, item.PipeID));
            }

        }

        protected void grvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.panelSet.Visible = true;

            string userID = this.grvData.DataKeys[e.NewSelectedIndex].Value.ToString();

            this.ViewState["userID"] = userID;

            this.wucGroup.SetValue(this.pmsBll.GetPmsGroupIDByUser(userID));
            UserInfo selectUser = this.userBll.Get<UserInfo>(string.Format(" UserID = '{0}' ", this.ViewState["userID"].ToString()));
            this.lbBalance.Text = selectUser.Points.ToString();

            this.grvUserPipeSet.DataSource = this.smsBll.GetUserPipeList(userID);
            this.grvUserPipeSet.DataBind();

        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.pmsBll.SetUserPmsGroup(this.ViewState["userID"].ToString(), this.wucGroup.SelectValue, true);
                this.ShowMessge("设置成功!");
            }
            catch (Exception ex)
            {
                this.ShowMessge("设置失败：" + ex.Message);
            }
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            this.panelSet.Visible = false;
            this.LoadData();
        }

        protected void btnReCharge_Click(object sender, EventArgs e)
        {
            int addPoit = 0;
            if (!int.TryParse(this.txtReCharge.Text, out addPoit))
            {
                this.ShowMessge("输入点数格式必须为数字!");
                this.txtReCharge.Focus();
                return;
            }

            if (addPoit.Equals(0))
            {
                this.ShowMessge("充值点数必须大于0!");
                return;
            }

            UserInfo selectUser = this.userBll.Get<UserInfo>(string.Format(" UserID = '{0}' ", this.ViewState["userID"].ToString()));

            UTransactionInfo trac = new UTransactionInfo();
            trac.UserID = selectUser.UserID;
            trac.TracMoney = addPoit;
            trac.TracTime = DateTime.Now;

            switch (this.rdoReCharge.SelectedItem.Text)
            {
                case "短信":
                    trac.Tractype = "addSmsPoit";
                    trac.TracNote = string.Format("短信充值{0}条,操作员{1}", addPoit, Comm.DataLoadTool.GetCurrUserID());
                    selectUser.Points += addPoit;
                    break;
                case "邮件":
                    trac.Tractype = "addEmailPoit";
                    trac.TracNote = string.Format("邮件充值{0}条,操作员{1}", addPoit, Comm.DataLoadTool.GetCurrUserID());
                    selectUser.EmailPoints += addPoit;
                    break;
                default:
                    break;
            }

            ZentCloud.ZCBLLEngine.BLLTransaction affair = new ZCBLLEngine.BLLTransaction();

            try
            {

                if (!this.userBll.Add(trac, affair))
                {
                    this.ShowMessge("充值失败!");
                    affair.Rollback();
                    return;
                }

                if (!this.userBll.Update(selectUser, affair))
                {
                    this.ShowMessge("充值失败!");
                    affair.Rollback();
                    return;
                }

                affair.Commit();
                this.txtReCharge.Text = "";
                this.ShowMessge("充值成功!");

                this.LoadData();
            }
            catch (Exception ex)
            {
                this.ShowMessge("充值失败:" + ex.Message);
                affair.Commit();
            }
        }

        protected void rdoReCharge_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserInfo selectUser = this.userBll.Get<UserInfo>(string.Format(" UserID = '{0}' ", this.ViewState["userID"].ToString()));

            switch (this.rdoReCharge.SelectedItem.Text)
            {
                case "短信":
                    this.lbBalance.Text = selectUser.Points.ToString();
                    break;
                case "邮件":
                    this.lbBalance.Text = selectUser.EmailPoints.ToString();
                    break;
                default:
                    break;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void btnAddUserPipeSet_Click(object sender, EventArgs e)
        {
            BLLJIMP.Model.SMSUserPipeSet model = new SMSUserPipeSet();

            model.UserID = this.ViewState["userID"].ToString();
            model.UserPipe = this.ddlChargePipe.SelectedValue;
            model.SendPipe = this.ddlSendPipe.SelectedValue;

            if (this.smsBll.Exists(model, new List<string>() { "UserID", "UserPipe" }))
            {
                this.smsBll.Update(
                    new BLLJIMP.Model.SMSUserPipeSet(),
                    string.Format(" SendPipe = '{0}' ", model.SendPipe),
                    string.Format(" UserID ='{0}' and UserPipe = '{1}'", model.UserID, model.UserPipe)
                    );
                this.ShowMessge("已成功更新通道：" + model.UserPipe);
            }
            else
            {
                this.smsBll.Add(model);
                this.ShowMessge("已成功添加新通道");
            }

            this.grvUserPipeSet.DataSource = this.smsBll.GetUserPipeList(model.UserID);
            this.grvUserPipeSet.DataBind();
        }

        protected void grvUserPipeSet_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            BLLJIMP.Model.SMSUserPipeSet model = new SMSUserPipeSet();

            model.UserID = this.grvUserPipeSet.DataKeys[e.RowIndex].Values["UserID"].ToString();
            model.UserPipe = this.grvUserPipeSet.DataKeys[e.RowIndex].Values["UserPipe"].ToString();
            model.SendPipe = this.grvUserPipeSet.DataKeys[e.RowIndex].Values["SendPipe"].ToString();

            this.smsBll.Delete(model);

            this.ShowMessge("成功删除!");

            this.grvUserPipeSet.DataSource = this.smsBll.GetUserPipeList(model.UserID);
            this.grvUserPipeSet.DataBind();
        }

        protected void grvUserPipeSet_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                    lbtnDelete.Attributes.Add("onclick", "return confirm(\"确认删除？\")");
                }
                catch { }
            }
        }



    }
}