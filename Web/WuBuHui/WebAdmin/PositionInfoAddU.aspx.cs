using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin
{
    public partial class PositionInfoAddU : System.Web.UI.Page
    {
        public string AutoId;
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public List<ArticleCategory> TradeList = new List<ArticleCategory>();
        public List<ArticleCategory> ProfessionalList = new List<ArticleCategory>();
        public PositionInfo model = new PositionInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["AutoId"];
                if (!string.IsNullOrEmpty(AutoId))
                {
                    model = bll.Get<PositionInfo>(string.Format("AutoId={0}",AutoId));
                }
                List<ArticleCategory> CategoryList=bll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType in ('trade','Professional')", bll.WebsiteOwner));
                TradeList = CategoryList.Where(p => p.CategoryType.Equals("trade")).ToList();
                ProfessionalList = CategoryList.Where(p => p.CategoryType.Equals("Professional")).ToList();
            }
        }
    }
}