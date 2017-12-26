using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallScoreProductCompile : System.Web.UI.Page
    {
        public int pid = 0;
        public string webAction = "add";
        BLLMall bll = new BLLMall();
        public WXMallScoreProductInfo ProductModel = new WXMallScoreProductInfo();
        public string HeadTitle = "添加商品";
        //public System.Text.StringBuilder sbStores = new System.Text.StringBuilder();
        public System.Text.StringBuilder sbTypeInfo = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

            webAction = Request["Action"];
            GetScoreTypeInfo();
            if (webAction == "edit")
            {
                pid = Convert.ToInt32(Request["pid"]);
                ProductModel = bll.GetScoreProduct(pid);
                if (ProductModel == null)
                {
                    Response.End();
                }
                else
                {
                    HeadTitle = string.Format("{0}", ProductModel.PName);

                }
            }
            
        }


        public void GetScoreTypeInfo()
        {
            List<BLLJIMP.Model.WXMallScoreTypeInfo> stInfos = bll.GetList<BLLJIMP.Model.WXMallScoreTypeInfo>(string.Format("websiteOwner='{0}'",bll.WebsiteOwner));
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