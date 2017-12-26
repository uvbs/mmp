using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class ScoreConfig : System.Web.UI.Page
    {
        public ZentCloud.BLLJIMP.Model.ScoreConfig model = new BLLJIMP.Model.ScoreConfig();
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        //public string OrderDateStr = "";
        //public string OrderDateTotalAmountStr = "";
        //public string OrderScoreStr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllScore.GetScoreConfig();
            if (model==null)
            {
                model=new BLLJIMP.Model.ScoreConfig();
            }
            else 
            {
                //if (model.OrderDate!=null)
                //{
                //    OrderDateStr = Convert.ToDateTime(model.OrderDate).ToString("yyyy-MM-dd");
                //}
                //if (model.OrderDateTotalAmount != null)
                //{
                //    OrderDateTotalAmountStr = model.OrderDateTotalAmount.ToString();
                //}
                //if (model.OrderScore != null)
                //{
                //    OrderScoreStr = model.OrderScore.ToString();
                //}
                
            }

        }
    }
}