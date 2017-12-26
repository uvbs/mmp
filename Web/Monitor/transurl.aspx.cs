using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Monitor
{
    public partial class transurl : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string request = Request["s"];//接收的参数     
                int eventType = 0;//默认打开
                //数据验证
                if (!string.IsNullOrEmpty(request))
                {
                    if (request.EndsWith("teq1"))
                    {
                        eventType = 1;//点击khlf
                        //request = request.TrimEnd('teq1');
                        request = request.Substring(0, request.IndexOf("teq1"));

                    }

                    MonitorLinkInfo linkInfo = bll.Get<MonitorLinkInfo>(string.Format("EncryptParameter='{0}'", request));
                    if (linkInfo != null)
                    {
                        MonitorPlan planInfo = bll.Get<MonitorPlan>(string.Format("MonitorPlanID={0}", linkInfo.MonitorPlanID));
                        if (planInfo.PlanStatus.Equals("1"))
                        {
                            MonitorEventDetailsInfo model = new MonitorEventDetailsInfo();
                            //model.DetailID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorDetailID));
                            model.MonitorPlanID = planInfo.MonitorPlanID;
                            model.EventType = eventType;
                            model.LinkID = linkInfo.LinkID;
                            model.EventBrowser = HttpContext.Current.Request.Browser.ToString();
                            model.EventBrowserID = HttpContext.Current.Request.Browser.Id;
                            model.EventBrowserIsBata = HttpContext.Current.Request.Browser.Beta.ToString();
                            model.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;
                            model.EventDate = DateTime.Now;
                         
                            if (HttpContext.Current.Request.Browser.Win32)
                                model.EventSysByte = "32位系统";
                            else
                                model.EventSysByte = "64位系统";


                            model.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
                            model.SourceIP = Common.MySpider.GetClientIP();
                            model.IPLocation = Common.MySpider.GetIPLocation(model.SourceIP);
                            if (Request.UrlReferrer != null)
                            {
                                model.SourceUrl = Request.UrlReferrer.ToString();
                            }
                            bll.Add(model);
                            if (eventType.Equals(1))
                            {
                                if (linkInfo != null)
                                {
                                    Response.Redirect(linkInfo.RealLink, false);
                                }


                            }


                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        Response.End();
                    }


                }



            }
            catch (Exception ex)
            {

                Response.End();
            }

        }
    }
}
