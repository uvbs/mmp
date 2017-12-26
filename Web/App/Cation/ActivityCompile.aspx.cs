using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class ActivityCompile : System.Web.UI.Page
    {
        /// <summary>
        /// 自动 ID
        /// </summary>
        public int aid = 0;
        /// <summary>
        /// 当前操作 add edit
        /// </summary>
        public string webAction = "add";
        /// <summary>
        /// 当前操作中文
        /// </summary>
        public string actionStr = "";
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity(DataLoadTool.GetCurrUserID());
        /// <summary>
        /// 当前活动
        /// </summary>
        public JuActivityInfo model = new JuActivityInfo();
        //public bool Pms_Advanced;
        /// <summary>
        /// 客服列表
        /// </summary>
        public System.Text.StringBuilder sbActivityNoticeKeFuList=new System.Text.StringBuilder();
        /// <summary>
        /// 分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        /// <summary>
        /// 收费选项
        /// </summary>
        public List<CrowdFundItem> ActivityItems = new List<CrowdFundItem>();
        /// <summary>
        /// 相关活动
        /// </summary>
        public List<JuActivityInfo> RelationArticle = new List<JuActivityInfo>();
        public List<WXMallProductInfo> RelationProduct = new List<WXMallProductInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            aid = Convert.ToInt32(Request["aid"]);
            webAction = Request["Action"];
           // Pms_Advanced = DataLoadTool.CheckCurrUserPms(ZentCloud.BLLPermission.PermissionKey.Pms_JuActivity_Advanced);
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bllJuactivity.GetJuActivity(aid);

                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    if (model.ActivityEndDate != null)
                    {
                        if ((DateTime)model.ActivityEndDate == new DateTime(1970, 1, 1))
                        {
                            model.ActivityEndDate = null;
                        }
                    }
                }

                if (model.IsFee==1)
                {
                    ActivityItems = bllJuactivity.GetList<CrowdFundItem>(string.Format(" CrowdFundID='{0}'",model.JuActivityID));
                }

            }
            else
            {
                model.WebsiteOwner = bllJuactivity.WebsiteOwner;
            }
            foreach (var item in bllJuactivity.GetList<WXKeFu>(string.Format("WebsiteOwner='{0}'", bllJuactivity.WebsiteOwner)))
            {
                sbActivityNoticeKeFuList.AppendFormat("<option value=\"{0}\">{1}</option>",item.AutoID,item.TrueName);


            }
            //foreach (var item in juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='activity'", juActivityBll.WebsiteOwner)))
            //{
            //    sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.CategoryName);

            //}
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bllJuactivity.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", bllJuactivity.WebsiteOwner, "activity")), "AutoID", "PreID", "CategoryName", 0, "ddlcategory", "width:200px", ""));

            if (!string.IsNullOrEmpty(model.RelationArticles))
            {
                string ids = "'" + model.RelationArticles.Replace(",", "','") + "'";
                //相关文章和活动
                RelationArticle = bllJuactivity.GetList<JuActivityInfo>(string.Format(" JuActivityID in ({0}) And IsHide=0 And ArticleType in('article','activity')", ids));
               
            }
            if (!string.IsNullOrEmpty(model.RelationProducts))
            {
                string ids = "'" + model.RelationProducts.Replace(",", "','") + "'";
                //相关商品
                RelationProduct = bllJuactivity.GetList<WXMallProductInfo>(string.Format(" WebsiteOwner='{0}' AND PID in ({1}) ", bllJuactivity.WebsiteOwner, ids));
            }


        }
    }
}