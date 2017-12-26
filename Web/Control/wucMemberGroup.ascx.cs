using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucMemberGroup : System.Web.UI.UserControl
    {
        ZentCloud.BLLJIMP.BLLMember bll;



        ///// <summary>
        ///// 编辑状态
        ///// </summary>
        //public bool IsEditState
        //{
        //    set
        //    {
        //        if (value)
        //            this.ViewState["EditState"] = 1;
        //        else
        //            this.ViewState["EditState"] = 0;
        //    }

        //    get
        //    {
        //        if (this.ViewState["EditState"] != null)
        //            if (this.ViewState["EditState"].ToString() == "1")
        //                return true;

        //        return false;
        //    }
        //}

        private int groupType = 0;

        /// <summary>
        /// 组类型
        /// </summary>
        public int GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        }

        private List<string> selectValue = new List<string>();

        public List<string> SelectValue
        {
            get
            {
                this.selectValue.Clear();
                foreach (string item in MySpider.ASPNET.CheckBoxListHelper.GetCheckedList(this.chkData))
                {
                    this.selectValue.Add(item);
                }

                return this.selectValue;
            }

            set
            {
                this.selectValue = value;
            }

        }

        public void SetValue(List<string> groupID)
        {
            this.chkData.SelectedIndex = -1;
            if (groupID.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string item in groupID)
                {
                    sb.Append(item + ",");
                }

                MySpider.ASPNET.CheckBoxListHelper.SetChecked(this.chkData, sb.ToString(), ",");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bll = new BLLJIMP.BLLMember(Comm.DataLoadTool.GetCurrUserID());
            if (!IsPostBack)
            {
                //this.ViewState["EditState"] = 0;
                this.lbtnDeleteGroup.Attributes.Add("onclick", "return confirm(\" 删除分组后，该分组下的会员数据归为无分组数据，确定删除？ \")");
                //this.lbtnDeleteGroup.Attributes.Add("onclick", "return confirm(\"提交前系统会对提交号码再进行一次处理，确认添加到发送队列？\")");
                //获取当前用户的会员分组
                this.LoadData();

                if (this.selectValue.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in this.selectValue)
                    {
                        sb.Append(item.ToString() + ",");
                    }

                    MySpider.ASPNET.CheckBoxListHelper.SetChecked(this.chkData, sb.ToString(), ",");
                }

            }
        }

        private void LoadData()
        {
            this.chkData.Items.Clear();
            //this.chkData.Items.Add(new ListItem("所有", "00"));
            this.chkData.Items.Add(new ListItem("无分组", "0"));
            foreach (ZentCloud.BLLJIMP.Model.MemberGroupInfo item in bll.GetList<ZentCloud.BLLJIMP.Model.MemberGroupInfo>(string.Format(" UserID = '{0}' AND GroupType = '{1}' ", Comm.DataLoadTool.GetCurrUserID(), this.groupType)))
            {
                this.chkData.Items.Add(new ListItem(item.GroupName, item.GroupID.ToString()));
            }

        }

        private void HideAddPanel()
        {
            this.lbMsg.Text = "";
            this.txtGroupName.Text = "";
            this.panelAdd.Visible = false;
            this.panelBtn.Visible = true;
        }

        private void ShowAddPanel()
        {
            this.panelAdd.Visible = true;
            this.panelBtn.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ZentCloud.BLLJIMP.Model.MemberGroupInfo model = new BLLJIMP.Model.MemberGroupInfo()
            {
                GroupID = bll.GetGUID(BLLJIMP.TransacType.MemberGroupAdd),
                UserID = Comm.DataLoadTool.GetCurrUserID(),
                GroupName = this.txtGroupName.Text.Trim(),
                AddDate = DateTime.Now,
                GroupType = this.groupType
                
            };

            //判断是否为空
            if (this.bll.Exists(model, new List<string>() { "UserID", "GroupName", "GroupType" }))
            {
                this.lbMsg.Text = "该组已存在!";
                return;
            }

            bll.Add(model);
            this.LoadData();
            this.HideAddPanel();
            this.Response.Redirect(this.Request.RawUrl);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.HideAddPanel();
        }

        protected void lbtnAddGroup_Click(object sender, EventArgs e)
        {
            this.ShowAddPanel();
        }

        protected void lbtnDeleteGroup_Click(object sender, EventArgs e)
        {
            this.selectValue = MySpider.ASPNET.CheckBoxListHelper.GetCheckedList(this.chkData);

            if (this.selectValue.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string item in this.selectValue)
                {
                    sb.AppendFormat("'{0}',", item);
                }

                //删除客户信息分组
                if (this.groupType.Equals(1))
                {
                    //将分组会员数据分组设置为0
                    int i = bll.Update(new BLLJIMP.Model.MemberInfo(), " GroupID = '0' ", string.Format(" GroupID IN ({0}) AND UserID = '{1}' ", sb.ToString().Trim(','), Comm.DataLoadTool.GetCurrUserID()));
                }

                //删除邮箱库分组
                if (this.groupType.Equals(2))
                {
                    
                }

                //删除分组
                int j = bll.Delete(new ZentCloud.BLLJIMP.Model.MemberGroupInfo(), string.Format(" GroupID IN ({0}) AND UserID = '{1}' ", sb.ToString().Trim(','), Comm.DataLoadTool.GetCurrUserID()));

                this.Response.Redirect(this.Request.RawUrl);

               
            }

        }
    }
}