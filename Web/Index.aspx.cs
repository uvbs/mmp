using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web
{
    public partial class Index : System.Web.UI.Page
    {
        public BLLCommRelation bllCommRelation = new BLLCommRelation();
        public bool isCustomMenu = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            var re = ZentCloud.Common.ValidatorHelper.PhoneNumLogicJudge("18521562432");
            //if (DataLoadTool.GetCurrUserID().ToLower().StartsWith("wxuser"))
            //{
            //    Response.Redirect("/Home/logout.aspx");
            //}
            //if (!IsPostBack)
            //{
            //    isCustomMenu = bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.WebsiteOwnerIsCustomMenu, bllCommRelation.WebsiteOwner, "");    
            //}
            if (!IsPostBack)
            {
                isCustomMenu = bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.WebsiteOwnerIsCustomMenu, bllCommRelation.WebsiteOwner, "");
            }
        }
    }
}