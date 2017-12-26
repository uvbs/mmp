using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Member
{
    public partial class MemberList : System.Web.UI.Page
    {
        List<string> selectMemberList;
        private string selectedMobiles;
        public string SelectedMobiles
        {
            get { return selectedMobiles; }
        }

        BLLMember bllMember;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.bllMember = new BLLMember(this.Session[Comm.SessionKey.UserID].ToString());
            selectMemberList = new List<string>();
            if (!IsPostBack)
            {
                this.lbtnDelete.Attributes.Add("onclick", "return confirm(\"确认删除勾选项？\")");
                this.LoadData();
            }

        }


        #region 显示提示框
        private void ShowMessge(string str)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), str);
        }

        private void ShowMessge(string str, string url)
        {
            Tool.AjaxMessgeBox.ShowMessgeBoxForAjax(this.UpdatePanel1, this.GetType(), str, url);
        }
        #endregion


        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("UserID = '{0}'", this.bllMember.UserID);

            //构造筛选条件
            if (this.wucMemberGroup.SelectValue.Count > 0)
            {
                strWhere.AppendFormat("AND GroupID IN ({0})", Common.StringHelper.ListToStr<string>(this.wucMemberGroup.SelectValue, "'", ","));
            }

            this.ViewState["strWhere"] = strWhere;

            //设置总数
            this.AspNetPager1.RecordCount = bllMember.GetCount<MemberInfo>(strWhere.ToString());

            this.grvData.DataSource = bllMember.GetLit<MemberInfo>(this.AspNetPager1.PageSize, this.AspNetPager1.CurrentPageIndex, strWhere.ToString());
            this.grvData.DataBind();

            //设置分页显示
            this.AspNetPager1.CustomInfoHTML = string.Format("当前第{0}/{1}页 共{2}条记录 每页{3}条", this.AspNetPager1.CurrentPageIndex, this.AspNetPager1.PageCount, this.AspNetPager1.RecordCount, this.AspNetPager1.PageSize);

            //加载分组
            this.ddlGroup.Items.Clear();
            this.ddlGroup.Items.Add(new ListItem("无分组", "0"));
            foreach (ZentCloud.BLLJIMP.Model.MemberGroupInfo item in bllMember.GetList<ZentCloud.BLLJIMP.Model.MemberGroupInfo>(string.Format(" UserID = '{0}' AND GroupType = 1 ", Comm.DataLoadTool.GetCurrUserID())))
            {
                this.ddlGroup.Items.Add(new ListItem(item.GroupName, item.GroupID.ToString()));
            }

        }

        /// <summary>
        /// 获取选择的会员ID
        /// </summary>
        private void GetSelectMemberID()
        {
            int i = 0;
            foreach (GridViewRow row in grvData.Rows)
            {
                if (((CheckBox)(row.Cells[0].FindControl("cbRow"))).Checked)
                {
                    this.selectMemberList.Add(this.grvData.DataKeys[i].Value.ToString());
                }
                i++;
            }
        }

        protected void lbtnBatchImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Member/MemberUpload.aspx");
        }

        protected void grvData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cbAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = ((CheckBox)(grvData.HeaderRow.Cells[0].FindControl("cbAll"))).Checked;
            foreach (GridViewRow row in grvData.Rows)
            {
                ((CheckBox)(row.Cells[0].FindControl("cbRow"))).Checked = isChecked;
            }
        }

        protected void lbtnSendSMS_Click(object sender, EventArgs e)
        {
            selectedMobiles = string.Empty;

            if (this.ddlSetSendSMS.SelectedValue.Equals("0"))
            {
                foreach (GridViewRow row in grvData.Rows)
                {
                    if (((CheckBox)(row.Cells[0].FindControl("cbRow"))).Checked)
                    {
                        selectedMobiles += row.Cells[2].Text + "\n";
                    }
                }
            }
            else if (this.ddlSetSendSMS.SelectedValue.Equals("1"))
            {
                foreach (MemberInfo item in this.bllMember.GetList<MemberInfo>(this.ViewState["strWhere"].ToString()))
                {
                    selectedMobiles += item.Mobile + "\n";
                }
            }

            Session[Comm.SessionKey.PageRedirect] = "/Member/MemberList.aspx";
            Session[Comm.SessionKey.PageCacheName] = "cache" + bllMember.GetGUID(TransacType.CacheGet);
            Comm.DataCache.SetCache(Session[Comm.SessionKey.PageCacheName].ToString(), selectedMobiles);
            Response.Redirect("/SMS/Send.aspx");
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void btnSaveSetGroup_Click(object sender, EventArgs e)
        {
            string groupID = this.ddlGroup.SelectedValue;

            if (this.ddlSetGroup.SelectedValue.Equals("0"))
            {
                this.GetSelectMemberID();
                if (this.selectMemberList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (string item in this.selectMemberList)
                    {
                        sb.AppendFormat("'{0}',", item);
                    }

                    this.bllMember.Update(new MemberInfo(), string.Format(" GroupID = '{0}' ", groupID), string.Format(" MemberID IN ({0})", sb.ToString().Trim(',')));

                    this.LoadData();
                }
            }
            else if (this.ddlSetGroup.SelectedValue.Equals("1"))
            {
                if (this.ViewState["strWhere"] == null)
                {
                    this.ShowMessge("您还没有筛选数据，不能进行该设置!");
                    return;
                }

                this.bllMember.Update(new MemberInfo(), string.Format(" GroupID = '{0}' ", groupID), this.ViewState["strWhere"].ToString());

                this.LoadData();
            }


        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            this.GetSelectMemberID();

            if (this.selectMemberList.Count > 0)
            {
                int i = this.bllMember.Delete(new MemberInfo(), string.Format(" MemberID IN ({0}) ", Common.StringHelper.ListToStr<string>(this.selectMemberList, "'", ",")));
                this.ShowMessge(string.Format("成功删除{0}条数据!", i.ToString()));
                this.LoadData();
            }
            else
            {
                this.ShowMessge("未选择任何项！");
            }
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Member/Add.aspx"); 
        }

   

    }
}