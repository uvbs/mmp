using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web
{
    public partial class Main2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session[Comm.SessionKey.UserID] == null || this.Session[Comm.SessionKey.UserType] == null)
            {
                ZentCloud.Common.WebMessageBox.Show(this.Page, "操作超时，请重新登录！");
                Response.Redirect("/Login.aspx");
            }
            if (this.Session[Comm.SessionKey.LoginStatu] == null)
                Response.Redirect("/Login.aspx");

            if (this.Session[Comm.SessionKey.LoginStatu].ToString() != "1")
                Response.Redirect("/Login.aspx");

            if (!IsPostBack)
            {
                this.lbtnQuit.Attributes.Add("onclick", "return confirm(\"确认要退出吗？\")");
            }
        }

        /// <summary>
        /// 短信发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSendSms_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "1";
            this.Response.Redirect("/SMS/Send.aspx");
            //this.Server.Transfer("/SMS/Send.aspx");
        }

        /// <summary>
        /// 账号信息历史按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnUserTrac_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "2";
            this.Response.Redirect("/Trac/TracHistory.aspx");
            //this.Server.Transfer("/Trac/List.aspx");
        }

        protected void lbtnRechargeManage_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "3";
            this.Response.Redirect("/User/RechargeHistory.aspx");
            //this.Server.Transfer("/User/RechargeHistory.aspx");
        }

        protected void lbtnUserManage_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "4";
            this.Response.Redirect("/User/List.aspx");
            //this.Server.Transfer("/User/List.aspx");
        }

        /// <summary>
        /// 发送历史查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnL2PlabList_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/SMS/PlanList.aspx");
        }

        /// <summary>
        /// 添加新用户按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAddNewUser_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/User/Add.aspx");
            //this.Server.Transfer("/User/Add.aspx");
        }


        protected void lbtnUserList_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/User/List.aspx");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnQuit_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.LoginStatu] = 0;
            this.Session[Comm.SessionKey.UserID] = null;
            this.Response.Redirect("/Login.aspx");
        }


        #region 微博操作按钮

        /// <summary>
        /// 发微博
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnWeiBo_Click(object sender, EventArgs e)
        {
            this.Session[ZentCloud.JubitIMP.Web.Comm.SessionKey.SelectMenu] = "5";
            this.Response.Redirect("/Weibo/Account.aspx");
        }
        
        #endregion

      

        protected void lbtnMeeting_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "6";
            this.Response.Redirect("/Meeting/MeetingList.aspx");
        }

        protected void lbtnMember_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "7";
            this.Response.Redirect("/Member/MemberList.aspx");
        }

        protected void lbtnWeixin_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "8";
            //this.Response.Redirect("/Member/MemberList.aspx");
        }

        protected void lbtnEmail_Click(object sender, EventArgs e)
        {
            this.Session[Comm.SessionKey.SelectMenu] = "9";
            //this.Response.Redirect("/Member/MemberList.aspx");
        }
    }
}
