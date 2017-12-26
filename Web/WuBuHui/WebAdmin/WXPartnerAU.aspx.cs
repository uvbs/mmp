using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin
{
    public partial class WXPartnerAU : System.Web.UI.Page
    {
        public string AutoId;
        BLLUser bll = new BLLUser("");
        public string PartnerStr;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["AutoId"];
                if (!string.IsNullOrEmpty(AutoId))
                {
                    BLLJIMP.Model.WBHPartnerInfo PInfo = bll.Get<BLLJIMP.Model.WBHPartnerInfo>(string.Format(" AutoId={0}", AutoId));
                    GetPartnerStr(PInfo);//获取行业
                }
                else
                {
                    GetPartnerStr1();//获取行业

                }
            }
        }

        private void GetPartnerStr1()
        {
            try
            {
                List<BLLJIMP.Model.ArticleCategory> actegorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='Partner'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));

                if (actegorys != null)
                {
                    foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
                    {
                        PartnerStr += string.Format("<input type=\"checkbox\" id=\"{0}\"  name=\"Partner\" value=\"{1}\" />", "Partner" + item.AutoID, item.AutoID);
                        PartnerStr += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "Partner" + item.AutoID, item.CategoryName);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetPartnerStr(BLLJIMP.Model.WBHPartnerInfo PInfo)
        {
            try
            {
                List<BLLJIMP.Model.ArticleCategory> actegorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='Partner'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));

                if (actegorys != null)
                {
                    foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
                    {
                        if (PInfo.PartnerType.Contains(item.AutoID.ToString()))
                        {
                            PartnerStr += string.Format("<input type=\"checkbox\" id=\"{0}\" checked=\"checked\"  name=\"Partner\" value=\"{1}\" />", "Partner" + item.AutoID, item.AutoID);
                            PartnerStr += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "trade" + item.AutoID, item.CategoryName);
                        }
                        else
                        {
                            PartnerStr += string.Format("<input type=\"checkbox\" id=\"{0}\"  name=\"Partner\" value=\"{1}\" />", "Partner" + item.AutoID, item.AutoID);
                            PartnerStr += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "Partner" + item.AutoID, item.CategoryName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}