using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using ZentCloud.Common.Model;

namespace ZentCloud.JubitIMP.Web.Admin.User
{
    public partial class SupplierChannelList : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();

        /// <summary>
        /// 等级列表
        /// </summary>
        public List<UserLevelConfig> LevelList = new List<UserLevelConfig>();


        protected void Page_Load(object sender, EventArgs e)
        {

           try
            {

                LevelList = bllDis.QueryUserLevelList(bll.WebsiteOwner, "DistributionSupplierChannel");
            }
            catch (Exception ex)
            {

                Response.Write(ex.ToString());
            }
           


        }
    }
}