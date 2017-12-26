using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucPmsGroupSelect : System.Web.UI.UserControl
    {
        private List<long> selectValue = new List<long>();

        public List<long> SelectValue
        {
            get
            {
                this.selectValue.Clear();
                foreach (string item in MySpider.ASPNET.CheckBoxListHelper.GetCheckedList(this.chkData))
                {
                    this.selectValue.Add(long.Parse(item));
                }

                return this.selectValue;
            }

            set
            {
                this.selectValue = value;
            }

        }

        public void SetValue(List<long> groupID)
        {
            this.chkData.SelectedIndex = -1;
            if (groupID.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (long item in groupID)
                {
                    sb.Append(item.ToString() + ",");
                }

                MySpider.ASPNET.CheckBoxListHelper.SetChecked(this.chkData, sb.ToString(), ",");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ZentCloud.BLLPermission.BLLMenuPermission menuBll = new BLLPermission.BLLMenuPermission(Comm.DataLoadTool.GetCurrUserID());

                this.chkData.Items.Clear();

                foreach (ZentCloud.BLLPermission.Model.PermissionGroupInfo item in menuBll.GetList<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(""))
                {
                    this.chkData.Items.Add(new ListItem(item.GroupName, item.GroupID.ToString()));
                }



                if (this.selectValue.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (long item in this.selectValue)
                    {
                        sb.Append(item.ToString() + ",");
                    }

                    MySpider.ASPNET.CheckBoxListHelper.SetChecked(this.chkData, sb.ToString(), ",");
                }


            }
        }


    }
}