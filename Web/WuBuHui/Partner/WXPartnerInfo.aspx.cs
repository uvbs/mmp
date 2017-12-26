using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Partner
{
    public partial class WXPartnerInfo : System.Web.UI.Page
    {
        public string AutoId;
        public string PartnerStr;
        public BLLJIMP.Model.WBHPartnerInfo PInfo = new BLLJIMP.Model.WBHPartnerInfo();
        public bool zan = false;
        BLLJIMP.BLL bll;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                bll = new BLLJIMP.BLL();
                AutoId = Request["id"];
                GetPartnerInfo(AutoId);
                try
                {
                    BLLJIMP.Model.ForwardingRecord frecord = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName='五伴会赞'", bll.GetCurrentUserInfo().UserID, int.Parse(Request["id"]), bll.WebsiteOwner));
                    if (frecord != null)
                    {
                        zan = true;
                    }
                }
                catch (Exception)
                {
                     
                    
                }

            }
            catch (Exception)
            {

                Response.End();
            }

            
        }

        /// <summary>
        /// 获取五伴会详情
        /// </summary>
        /// <param name="AutoId"></param>
        private void GetPartnerInfo(string AutoId)
        {
          
             PInfo = bll.Get<BLLJIMP.Model.WBHPartnerInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, AutoId));
            if (PInfo != null)
            {
                GetPartnerStr(PInfo);
                PInfo.PartnerPv++;
                bll.Update(PInfo);
            }
        }

        private void GetPartnerStr(BLLJIMP.Model.WBHPartnerInfo PInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(PInfo.PartnerType))
                {
                    PInfo.PartnerType = PInfo.PartnerType.TrimEnd(',');
                    foreach (var item in PInfo.PartnerType.Split(','))
                    {
                        var category = bll.Get<ArticleCategory>(string.Format(" AutoID={0}",item));
                        if (category!=null)
                        {
                            PartnerStr += " <span class=\"wbtn_tag wbtn_main\">" + category.CategoryName + "</span>";
                        }
                        
                    }
                    //PInfo.Ctype = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("  CategoryType='Partner' AND AutoID in ({0})", PInfo.PartnerType));

                    //foreach (BLLJIMP.Model.ArticleCategory item in PInfo.Ctype)
                    //{
                    //    PartnerStr = " <span class=\"wbtn_tag wbtn_main\">" + item.CategoryName + "</span>";
                    //}


                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}