using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CompanyWebsite
{
    public partial class ToolBarManage : System.Web.UI.Page
    {
        BLLJIMP.BLLCompanyWebSite bll = new BLLJIMP.BLLCompanyWebSite();
        protected string groups;
        ///protected string iconclasses;
        protected string icoScript;
        protected int userType;
        protected void Page_Load(object sender, EventArgs e)
        {
            userType = bll.GetCurrentUserInfo().UserType;
            string isSystem = this.Request["is_system"];
            string use_type = this.Request["use_type"];
            string isPc=Request["isPc"];
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbWhere1 = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bll.WebsiteOwner);
            sbWhere1.AppendFormat(" WebsiteOwner Is null");
            if (!string.IsNullOrWhiteSpace(use_type))
            {
                sbWhere.AppendFormat(" And UseType = '{0}'", use_type);
                sbWhere1.AppendFormat(" And UseType = '{0}'", use_type);
            }
            if (!string.IsNullOrWhiteSpace(isPc))
            {
                sbWhere.AppendFormat(" And IsPC = '{0}'", isPc);
                sbWhere1.AppendFormat(" And IsPC = '{0}'", isPc);
            }
            else
            {
                sbWhere.AppendFormat(" And IsNull(IsPc,0)=0");
                sbWhere1.AppendFormat(" And IsNull(IsPc,0)=0");
            }
            List<CompanyWebsite_ToolBar> dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,KeyType,BaseID");
            if (isSystem != "1")
            {
                List<CompanyWebsite_ToolBar> dataList1 = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere1.ToString(), "AutoID,KeyType");
                List<int> nList = dataList.Select(p => p.BaseID).Distinct().ToList();
                foreach (CompanyWebsite_ToolBar item in dataList1.Where(p => !nList.Contains(p.AutoID)))
                {
                    dataList.Add(item);
                }
            }
            List<string> result = dataList.OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();
            groups = JsonConvert.SerializeObject(result);

            //图标引用js
            icoScript = bll.GetIcoScript();
        }
    }
}