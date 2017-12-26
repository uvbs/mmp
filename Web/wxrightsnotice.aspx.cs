using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web
{
    public partial class wxrightsnotice : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            //for (int i =1; i <=2000; i++)
            //{
            //    //if ((i%2)==0)
            //    //{
            //        WXLotteryWinningData model = new WXLotteryWinningData();
            //        model.LotteryId =8;
            //        model.WinningIndex = i;
            //        model.WXAwardsId =12;
            //        bll.Add(model);


            //    //}
                
            //}
            //BLLJIMP.BLLWeixin bllweixin=new BLLJIMP.BLLWeixin("");
            //WXQiyeConfig config=bllweixin.Get<BLLJIMP.Model.WXQiyeConfig>(string.Format("WebsiteOwner='{0}'",bll.WebsiteOwner));
            //bllweixin.SendMessageTextQiye(config.CorpID, config.Secret, config.AppId, "vincent", "hello word");
        }
    }
}