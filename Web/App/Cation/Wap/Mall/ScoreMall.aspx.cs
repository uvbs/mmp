using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class ScoreMall : System.Web.UI.Page
    {
        /// <summary>
        /// 分类字符串
        /// </summary>
        public string CategoryStr;
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<BLLJIMP.Model.WXMallScoreTypeInfo> data = bllMall.GetList<BLLJIMP.Model.WXMallScoreTypeInfo>(string.Format("WebsiteOwner='{0}'", bllMall.WebsiteOwner));

            foreach (BLLJIMP.Model.WXMallScoreTypeInfo item in data)
                {
                    CategoryStr += string.Format("<li class=\"catli\" data-categorid=\"{0}\"><a >{1}</a></li>",item.AutoId,item.TypeName); 
                }
            

        }
    }
}