using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web
{
    public partial class Main1 : System.Web.UI.Page
    {
        public StringBuilder menuHtml = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                BLLPermission.BLLMenuPermission menuBll = new BLLPermission.BLLMenuPermission(Comm.DataLoadTool.GetCurrUserID());

                menuHtml.Append(menuBll.GetUserMenuTreeHtml(Comm.DataLoadTool.GetCurrUserID()));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}