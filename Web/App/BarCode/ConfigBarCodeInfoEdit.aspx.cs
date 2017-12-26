using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.BarCode
{
    public partial class ConfigBarCodeInfoEdit : System.Web.UI.Page
    {

        public string Tag = "添加";
        public string AutoId ;
        public BLLJIMP.Model.BarCodeInfo model;
        public BLLJIMP.BLLJuActivity jubll;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["id"];
                jubll = new BLLJIMP.BLLJuActivity("");
                if (!string.IsNullOrEmpty(AutoId))
                {
                    Tag = "编辑";
                    GetBarCodeInfo(AutoId);
                }
            }
        }

        private void GetBarCodeInfo(string AutoId)
        {
            model = jubll.Get<BLLJIMP.Model.BarCodeInfo>("  AutoID=" + AutoId);
        }
    }
}