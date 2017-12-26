using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Admin.User
{
    public partial class SupplierList : System.Web.UI.Page
    {
        /// <summary>
        /// 等级列表
        /// </summary>
        public List<UserLevelConfig> LevelList = new List<UserLevelConfig>();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        protected void Page_Load(object sender, EventArgs e)
        {
            LevelList = bllDis.QueryUserLevelList(bllDis.WebsiteOwner, "DistributionSupplier");

        }
    }
}