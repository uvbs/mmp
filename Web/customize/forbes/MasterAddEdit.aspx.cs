using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.forbes
{
    public partial class MasterAddEdit : System.Web.UI.Page
    {
        public string AutoId;
        //public string tradeStr;
        public string Tags;
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public BLLJIMP.Model.TutorInfo tInfo = new BLLJIMP.Model.TutorInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["id"];
                if (!string.IsNullOrEmpty(AutoId))
                {
                    tInfo = bll.Get<BLLJIMP.Model.TutorInfo>(string.Format(" AutoId={0}", AutoId));
                    //Gettrade();//获取行业
                    GetProfessional();//获取行业
                }
                else
                {
                    //Gettrade1();//获取行业
                    GetProfessional1();//获取行业
                }

            }
        }

        private void GetProfessional1()//获取行业
        {
            try
            {
                List<BLLJIMP.Model.ArticleCategory> actegorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='Professional'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));

                if (actegorys != null)
                {
                    foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
                    {

                        Tags += string.Format("<input type=\"checkbox\" id=\"{0}\"  name=\"Professional\" value=\"{1}\" />", "Professional" + item.AutoID, item.AutoID);
                        Tags += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "Professional" + item.AutoID, item.CategoryName);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        ///// <summary>
        ///// 获取行业
        ///// </summary>
        //private void Gettrade1()
        //{
        //    try
        //    {
        //        List<BLLJIMP.Model.ArticleCategory> actegorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='trade'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));

        //        if (actegorys != null)
        //        {
        //            foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
        //            {
        //                tradeStr += string.Format("<input type=\"checkbox\" id=\"{0}\"  name=\"trade\" value=\"{1}\" />", "trade" + item.AutoID, item.AutoID);
        //                tradeStr += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "trade" + item.AutoID, item.CategoryName);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// 获取专业
        /// </summary>
        private void GetProfessional()
        {
            try
            {
                List<BLLJIMP.Model.ArticleCategory> actegorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='Professional'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));

                if (actegorys != null)
                {
                    foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
                    {

                        if (tInfo.ProfessionalStr.Contains(item.AutoID.ToString()))
                        {
                            Tags += string.Format("<input type=\"checkbox\" id=\"{0}\" checked=\"checked\"  name=\"Professional\" value=\"{1}\" />", "Professional" + item.AutoID, item.AutoID);
                            Tags += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "trade" + item.AutoID, item.CategoryName);
                        }
                        else
                        {
                            Tags += string.Format("<input type=\"checkbox\" id=\"{0}\"  name=\"Professional\" value=\"{1}\" />", "Professional" + item.AutoID, item.AutoID);
                            Tags += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "Professional" + item.AutoID, item.CategoryName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        ///// <summary>
        ///// 获取行业
        ///// </summary>
        //private void Gettrade()
        //{
        //    try
        //    {
        //        List<BLLJIMP.Model.ArticleCategory> actegorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  websiteOwner='{0}' AND CategoryType='trade'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));

        //        if (actegorys != null)
        //        {
        //            foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
        //            {
        //                if (tInfo.TradeStr.Contains(item.AutoID.ToString()))
        //                {
        //                    tradeStr += string.Format("<input type=\"checkbox\" id=\"{0}\" checked=\"checked\"  name=\"trade\" value=\"{1}\" />", "trade" + item.AutoID, item.AutoID);
        //                    tradeStr += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "trade" + item.AutoID, item.CategoryName);
        //                }
        //                else
        //                {
        //                    tradeStr += string.Format("<input type=\"checkbox\" id=\"{0}\"  name=\"trade\" value=\"{1}\" />", "trade" + item.AutoID, item.AutoID);
        //                    tradeStr += string.Format("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "trade" + item.AutoID, item.CategoryName);
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

    }
}