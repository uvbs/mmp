using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.game
{
    public partial class Game : System.Web.UI.Page
    {
        BLLJIMP.BllGame bllGame = new BLLJIMP.BllGame();
        /// <summary>
        /// 广告代码
        /// </summary>
        private System.Text.StringBuilder sbAdvert = new StringBuilder();
        /// <summary>
        /// 游戏信息
        /// </summary>
        public GameInfo GameInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int planId=int.Parse(Request["pid"]);
                var PlanInfo = bllGame.GetSingleGameAdvertPlan(planId);
                GameInfo=bllGame.GetSingleGameInfo(PlanInfo.GameID);
                string Html = GameInfo.GameCode;
                sbAdvert.AppendLine("<div id=\"slides\" class=\"divslides\">");
                if (!string.IsNullOrEmpty(PlanInfo.AdvertImage1))
                {
                    sbAdvert.AppendLine("<div class=\"slider slider1\">");
                    sbAdvert.AppendFormat(" <img src=\"{0}\"  data-url=\"{1}\" data-pid=\"{2}\" class=\"pic\" >", PlanInfo.AdvertImage1, PlanInfo.AdvertUrl1, PlanInfo.AutoID);
                    sbAdvert.AppendLine("</div>");
        
                }
                if (!string.IsNullOrEmpty(PlanInfo.AdvertImage2))
                {
                    sbAdvert.AppendLine("<div class=\"slider slider2\">");
                    sbAdvert.AppendFormat(" <img src=\"{0}\"  data-url=\"{1}\" data-pid=\"{2}\" class=\"pic\" >", PlanInfo.AdvertImage2, PlanInfo.AdvertUrl2, PlanInfo.AutoID);
                    sbAdvert.AppendLine("</div>");

                }
                if (!string.IsNullOrEmpty(PlanInfo.AdvertImage3))
                {

                    sbAdvert.AppendLine("<div class=\"slider slider3\">");
                    sbAdvert.AppendFormat(" <img src=\"{0}\"  data-url=\"{1}\" data-pid=\"{2}\" class=\"pic\" >", PlanInfo.AdvertImage3, PlanInfo.AdvertUrl3, PlanInfo.AutoID);
                    sbAdvert.AppendLine("</div>");


                }
                sbAdvert.AppendLine("</div>");
                if (Html.Contains("$AD$"))
                {
                    Html = Html.Replace("$AD$", sbAdvert.ToString());//替换广告代码

                }
                if (Html.Contains("$SHAREIMAGE$"))
                {
                    Html = Html.Replace("$SHAREIMAGE$", string.Format("http://{0}{1}", Request.Url.Host, GameInfo.GameImage));//替换分享图片

                }
                if (Html.Contains("$SHAREURL$"))
                {
                    Html = Html.Replace("$SHAREURL$", string.Format("http://{0}/Game/Game.aspx?pid={1}", Request.Url.Host, Request["pid"]));//替换分享链接

                }

                body.InnerHtml = Html;
                bool rusult=InsertGameEventDetail();
                bllGame.UpdateGamePlanIPPV(int.Parse(Request["pid"]));

            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }


        }

        /// <summary>
        /// 事件记录
        /// </summary>
        /// <returns></returns>
        private bool InsertGameEventDetail (){
            GameEventDetailInfo model = new GameEventDetailInfo();
            model.GamePlanID = int.Parse(Request["pid"]);
            model.SourceUrl =Request.Url.ToString();
            model.EventBrowserUserAgent = Request.UserAgent;
            model.EventBrowser = Request.Browser.Browser;
            model.EventDate = DateTime.Now;
            model.EventSysPlatform = Request.Browser.Platform;
            model.SourceIP = Common.MySpider.GetClientIP();
            model.IPLocation = Common.MySpider.GetIPLocation(model.SourceIP);
            model.WebsiteOwner = bllGame.WebsiteOwner;
            if (bllGame.AddGameEventDetail(model))
            {
                return true;
            }
            else
            {
                return false;
            }


    
    
    
    }
    }
}