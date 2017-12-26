using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.User
{
    public partial class UserScoreConfig : System.Web.UI.Page
    {
        protected WebsiteInfo model = new WebsiteInfo();
        protected List<TableFieldMapping> tbFieldList = new List<TableFieldMapping>();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLWebSite bllWebsite = new BLLWebSite();
            string websiteOwner = bllWebsite.WebsiteOwner;
            model = bllWebsite.GetWebsiteInfo(websiteOwner);

            string fields = "'DistributionRateLevel0First','DistributionRateLevel1First','DistributionRateLevel0','DistributionRateLevel1','RebateScoreRate','DistributionRateLevel2','DistributionRateLevel3','FromHistoryScore','ToHistoryScore','Discount','DistributionRateLevel1Ex1','AccumulationFundRateLevel1','RebateMemberRate','AwardAmount'";
            tbFieldList = bllTableFieldMap.GetTableFieldMapByWebsite(websiteOwner, "ZCJ_UserLevelConfig", null, null, "0", "AutoId,Field,MappingName,IsShowInList", fields);
        }
        protected string GetFieldName(string field, string defName)
        {
           return bllTableFieldMap.GetTableFieldName(defName, field, tbFieldList);
        }
        protected bool GetIsShow(string field, bool defShow)
        {
            return bllTableFieldMap.GetTableFieldIsShow(defShow, field, tbFieldList);
        }
        protected string GetIsHideString(string field, bool defShow)
        {
            if(bllTableFieldMap.GetTableFieldIsShow(defShow, field, tbFieldList)) return "";
            return "hide";
        }
    }
}