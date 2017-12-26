using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class ScoreProductDetail : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public WXMallScoreProductInfo model;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int Pid = int.Parse(Request["pid"]);
                model = bllMall.GetScoreProduct(Pid);
                if (model == null)
                {
                    
                    Response.End();
                }
                if (model.DiscountScore > 0)
                {
                    model.Score = model.DiscountScore;

                }
                model.PV++;
                bllMall.Update(model);
                if (!bllMall.IsLogin)
                {
                    if (bllMall.IsMobile)
                    {
                        if (bllMall.WebsiteOwner.Equals("forbes"))
                        {
                            Response.Redirect("/customize/forbes/#/login");
                        }
                        else
                        {
                            Response.Redirect(string.Format("/App/Cation/Wap/login.aspx?redirecturl={0}", Request.Url.PathAndQuery.ToString()));
                        }

                    }
                }


            }
            catch (Exception)
            {

                Response.End();
            }

        }
    }
}