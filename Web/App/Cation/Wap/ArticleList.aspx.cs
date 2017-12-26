using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class ArticleList : System.Web.UI.Page
    {
        public BLLJIMP.Model.UserPersonalizeDataInfo cateModel;
        public int cateId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            cateId = Convert.ToInt32(Request["cateId"]);
            //cateModel = new BLLJIMP.BLLUserPersonalize("").Get(cateId);

        }

    }
}