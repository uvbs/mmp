using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine
{
    public partial class ProjectList : System.Web.UI.Page
    {
        /// <summary>
        /// 分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bllDis = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        ///商机状态管理 
        /// </summary>
        public List<WXMallOrderStatusInfo> statusList = new List<WXMallOrderStatusInfo>();
        /// <summary>
        /// 分佣状态
        /// </summary>
        public string CommissionStatus = "";
        /// <summary>
        /// DataGrid显示列
        /// </summary>
        public string Columns = "";
        /// <summary>
        /// 分销活动ID
        /// </summary>
        public string ActivityID;
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;

        public string moduleType = "";
        public string moduleName = "商机";
        public string tipMsg = "项目名称";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["moduleType"])) moduleType = Request["moduleType"];
            if (!string.IsNullOrEmpty(Request["moduleName"])) moduleName = Request["moduleName"];

            if (moduleType == "HouseRecommend") tipMsg = "楼盘名称、省份、城市、地区";
            if (moduleType == "CompanyBranchApply") tipMsg = "城市、联系人、联系方式";
            if (moduleType == "CompanyBranchRecommend") tipMsg = "城市、联系人、联系方式";
            if (moduleType == "HouseAppointment") tipMsg = "楼盘名称、联系人、联系方式、备注";
            if (moduleType == "HouseBuyerRecommend") tipMsg = "楼盘名称、联系人、联系方式、备注";

            statusList = bllDis.QueryProjectStatusList(moduleType);

            var statusModel = statusList.FirstOrDefault(p => p.StatusAction == "DistributionOffLineCommission");
            if (statusModel != null)
            {
                CommissionStatus = statusModel.OrderStatu;
            }
            var fieldList = bllDis.QueryProjectFieldMapList(moduleType);
            for (int i = 0; i < fieldList.Count; i++)
            {
                if (fieldList[i].IsShowInList == 0) continue;
                Columns += "{ field: '" + fieldList[i].Field + "', title: '" + fieldList[i].MappingName + "', width: 10, align: 'left' }";
                if (i < fieldList.Count - 1)
                {
                    Columns += ",";
                }
            }
            ActivityID = bllDis.GetDistributionOffLineApplyActivityID();
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
        }
    }
}