using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;


namespace ZentCloud.JubitIMP.Web.Admin.WebPC
{
    public partial class Edit : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<string> MenuList = new List<string>();
        /// <summary>
        ///幻灯片列表
        /// </summary>
        public List<string> SlideList = new List<string>();
        /// <summary>
        /// 幻灯片列表json
        /// </summary>
        protected string slideListJson;
        /// <summary>
        /// 模型
        /// </summary>
        public PcPage model = new PcPage();
        
        protected void Page_Load(object sender, EventArgs e)
        {

            model = bll.Get<PcPage>(string.Format(" WebsiteOwner='{0}' And PageId={1}",bll.WebsiteOwner,Request["pageId"]));

            #region 菜单列表
            string is_system = this.Request["is_system"];
            string use_type = this.Request["use_type"];
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbWhere1 = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bll.WebsiteOwner);
            sbWhere1.AppendFormat(" WebsiteOwner Is null");

            sbWhere.AppendFormat(" And  IsPc=1");
            sbWhere1.AppendFormat(" And  IsPc=1");
            if (!string.IsNullOrWhiteSpace(use_type))
            {
                sbWhere.AppendFormat(" And UseType = '{0}'", use_type);
                sbWhere1.AppendFormat(" And UseType = '{0}'", use_type);
            }
            var dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere.ToString(), "AutoID,KeyType,BaseID");
            if (is_system != "1")
            {
                List<CompanyWebsite_ToolBar> dataList1 = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, sbWhere1.ToString(), "AutoID,KeyType");
                List<int> nList = dataList.Select(p => p.BaseID).Distinct().ToList();
                foreach (CompanyWebsite_ToolBar item in dataList1.Where(p => !nList.Contains(p.AutoID)))
                {
                    dataList.Add(item);
                }
            }
            MenuList = dataList.OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();

            #endregion

            #region 幻灯片列表
            var slideData = bll.GetList<BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And IsPC=1  order by Sort DESC", bll.WebsiteOwner));
            foreach (var item in slideData)
            {
                if (!SlideList.Contains(item.Type))
                {
                    SlideList.Add(item.Type);
                }
            }
            slideListJson = ZentCloud.Common.JSONHelper.ObjectToJson(SlideList);
            #endregion

           

        }
    }
}