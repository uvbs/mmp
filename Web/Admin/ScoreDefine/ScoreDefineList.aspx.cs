using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.ScoreDefine
{
    public partial class ScoreDefineList : System.Web.UI.Page
    {
        protected string selectOptionHtml;
        protected string moduleName = "积分";
        protected int isHideEx1 = 0;
        protected int isHideEvent = 0;
        protected BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        protected void Page_Load(object sender, EventArgs e)
        {
            var moduleNameReq = Request["moduleName"];
            isHideEx1 = Convert.ToInt32(Request["isHideEx1"]);
            isHideEvent = Convert.ToInt32(Request["isHideEvent"]);

            if (!string.IsNullOrWhiteSpace(moduleNameReq)) moduleName = moduleNameReq;

            List<KeyVauleDataInfo> list = bllKeyValueData.GetKeyVauleDataInfoList("ScoreDefineType", "0", bllKeyValueData.WebsiteOwner);
            if(list.Count ==0) list = bllKeyValueData.GetKeyVauleDataInfoList("ScoreDefineType", "0", "Common");
            selectOptionHtml = new ZentCloud.Common.MyCategoriesV2().GetSelectOptionHtml(list, "DataKey", "PreKey", "DataValue", "0", "ddlType", "width:200px", "");
        }
    }
}