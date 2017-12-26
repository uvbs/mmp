using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallScoreProductMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        public System.Text.StringBuilder sbTypeInfo = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetScoreTypeInfo();
        }
        public void GetScoreTypeInfo()
        {
            List<BLLJIMP.Model.WXMallScoreTypeInfo> stInfos = bll.GetList<BLLJIMP.Model.WXMallScoreTypeInfo>();
            if (stInfos.Count > 0)
            {
                foreach (BLLJIMP.Model.WXMallScoreTypeInfo item in stInfos)
                {
                    sbTypeInfo.AppendFormat("<option value='{0}'>{1}</option>", item.AutoId, item.TypeName);
                }
            }

        }
    }
}