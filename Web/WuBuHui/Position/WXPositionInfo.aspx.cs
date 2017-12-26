using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Position
{
    public partial class WXPositionInfo : System.Web.UI.Page
    {
        public string AutoId;
        public BLLJIMP.Model.PositionInfo pInfo;
        public BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        private BLLJIMP.Model.UserInfo uInfo;
        public string TagStr;
        public bool isUserRegistered = true;
        public List<ArticleCategory> TradeList = new List<ArticleCategory>();
        public List<ArticleCategory> ProfessionalList = new List<ArticleCategory>();
        /// <summary>
        /// 是否已申请标识
        /// </summary>
        public bool IsApply;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BLLJIMP.Model.UserInfo uinfo = bll.GetCurrentUserInfo();
                if (string.IsNullOrEmpty(uinfo.TrueName) || string.IsNullOrEmpty(uinfo.Phone))
                {
                   isUserRegistered = false;
                }
           
                AutoId = Request["id"];
                if (!string.IsNullOrEmpty(AutoId))
                {
                    GetPosition();

                    var List = bll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType in ('trade','Professional')", bll.WebsiteOwner));
                    TradeList = List.Where(p => p.CategoryType.Equals("trade")).ToList();
                    ProfessionalList = List.Where(p => p.CategoryType.Equals("Professional")).ToList();
                    IsApply = bll.GetCount<ApplyPositionInfo>(string.Format("UserId='{0}' And PositionId={1}",DataLoadTool.GetCurrUserID(), AutoId)) > 0 ? true : false;
                }
            }
            catch (Exception)
            {

                Response.End();
            }
        }

        private void GetPosition()
        {
           
            pInfo = bll.Get<BLLJIMP.Model.PositionInfo>(string.Format(" WebsiteOwner='{0}' AND AutoId='{1}'", bll.WebsiteOwner, AutoId));
            if (pInfo != null)
            {
                pInfo.Pv = pInfo.Pv + 1;
                bll.Update(pInfo);
                txtTitle.Text = pInfo.Title;
                txtPersonal.Text = pInfo.Personal;
                txtTime.Text = pInfo.InsertDate.ToString();
                txtAddress.Text = pInfo.Address;
                txtEnterpriseScale.Text = pInfo.EnterpriseScale;
                txtContent.Text = pInfo.Context;

            }
        }
    }
}