using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CrowdFund.Mobile
{
    public partial class CrowdFundInfoShow : MobileBase
    {
        BLLJIMP.BLL bllBase = new BLLJIMP.BLL();
        /// <summary>
        /// 众筹信息
        /// </summary>
        public  CrowdFundInfo model = new CrowdFundInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
            {
                Response.End();
            }
            model = bllBase.Get<CrowdFundInfo>(string.Format(" AutoID={0}",Request["id"]));
            if (model==null)
            {
                Response.End();
            }
            model.PV++;
            bllBase.Update(model);

        }
    }
}