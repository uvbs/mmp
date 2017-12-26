using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZCJson;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Vote
{
    public partial class VoteInfoCompile : System.Web.UI.Page
    {

        public string aid = "0";
        public string webAction = "add";
        public string actionStr = "";
        BLLVote bll = new BLLVote();
        public VoteInfo model = new VoteInfo();
        protected string groups;
        protected void Page_Load(object sender, EventArgs e)
        {
            aid = Request["aid"];
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bll.GetVoteInfo(int.Parse(aid));
                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.StopDate))
                    {
                        model.StopDate = DateTime.Parse(model.StopDate).ToString("yyyy-MM-dd HH:mm");
                    }
                }
            }
            else
            {
                model.FreeVoteCount = 1;
                model.VoteObjectLimitVoteCount = 1;
            }
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bll.WebsiteOwner);
            sbWhere.AppendFormat(" And UseType = 'nav'");
            List<CompanyWebsite_ToolBar> dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,KeyType,BaseID");
            List<string> result = dataList.OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();
            groups = JsonConvert.SerializeObject(result);




        }

        
    }
}