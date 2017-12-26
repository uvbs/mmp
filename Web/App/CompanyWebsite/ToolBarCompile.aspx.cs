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
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CompanyWebsite
{
    public partial class ToolBarCompile : System.Web.UI.Page
    {

        public string aid = "0";
        public string webAction = "add";
        public string actionStr = "";
        BLLCompanyWebSite bll = new BLLCompanyWebSite();
        public CompanyWebsite_ToolBar model = new CompanyWebsite_ToolBar();
        protected string groups;
        protected string limitPres;
        protected string iconclasses;
        protected string icoScript;
        protected int userType;
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Request["aid"];
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";

            List<int> limitPreList = new List<int>();
            if (webAction == "edit")
            {
                model = bll.GetCompanyWebsiteToolBarById(aid);

                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    limitPreList.Add(model.AutoID);
                    limitPreList.AddRange(bll.GetChildToolBarIDList(model.AutoID, bll.WebsiteOwner));
                }
            }
            userType = bll.GetCurrentUserInfo().UserType;

            string is_system = this.Request["is_system"];
            string isPc=Request["isPc"];
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbWhere1 = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bll.WebsiteOwner);
            sbWhere1.AppendFormat(" WebsiteOwner Is null");
            if (!string.IsNullOrEmpty(isPc))
            {
              sbWhere.AppendFormat(" And IsPc = '{0}'",isPc);
             sbWhere1.AppendFormat(" And IsPc = '{0}'",isPc);
            }
            else
            {
                sbWhere.AppendFormat(" And IsNull(IsPc,0)=0");
                sbWhere1.AppendFormat(" And IsNull(IsPc,0)=0");
            }
            List<CompanyWebsite_ToolBar> dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,KeyType,BaseID");
            if (is_system != "1")
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

            limitPres = JsonConvert.SerializeObject(limitPreList);

            //图标引用js
            icoScript = bll.GetIcoScript();
            iconclasses = bll.GetIcoClassArray();
        }
    }
}