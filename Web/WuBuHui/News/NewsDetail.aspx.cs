using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZentCloud.JubitIMP.Web.WuBuHui.News
{
    public partial class NewsDetail : System.Web.UI.Page
    {
        public BLLJIMP.Model.JuActivityInfo model = new BLLJIMP.Model.JuActivityInfo();
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity("");
       public bool zan = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                model = bll.GetJuActivity(int.Parse(Request["id"]));
                model.PV++;
                bll.Update(model);
                txtPraiseNum.Text = model.UpCount.ToString();
                try
                {
                    BLLJIMP.Model.ForwardingRecord frecord = bll.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName = '文章赞'", bll.GetCurrentUserInfo().UserID, int.Parse(Request["id"]), bll.WebsiteOwner));
                    if (frecord != null)
                    {
                        zan = true;
                    }

                    #region 加投票功能
                    if (model.ActivityDescription.Contains("$TOUPIAO@"))
                    {
                        int start = model.ActivityDescription.IndexOf("$TOUPIAO@");
                        int end = model.ActivityDescription.LastIndexOf("TOUPIAO$");
                        int length = end - start - 1;
                        string Voteid = model.ActivityDescription.Substring(model.ActivityDescription.IndexOf("$TOUPIAO@") + 1, length).Replace("TOUPIAO@", null);
                        string str = "$TOUPIAO@" + Voteid + "TOUPIAO$";
                        model.ActivityDescription = model.ActivityDescription.Replace(str, bll.GetTheVoteInfo(Voteid));

                    }
                    #endregion


                }
                catch (Exception)
                {
                    
                   
                }


            }
            catch (Exception)
            {

                Response.End();
            }

        }


       
    }
}