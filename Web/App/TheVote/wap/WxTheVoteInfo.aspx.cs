using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.TheVote.wap
{
    public partial class WxTheVoteInfo : System.Web.UI.Page
    {
        public string AutoId = "";
        public BLLJIMP.Model.TheVoteInfo tvInfo;
        private BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string Select = "radio";
        ZentCloud.BLLJIMP.Model.UserInfo userInfo; //当前登陆的用户
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                AutoId = Request["AutoId"];
                if (!string.IsNullOrEmpty(AutoId))
                {
                    GetTheVoteInfo(AutoId);

                    MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                    detailInfo.MonitorPlanID =int.Parse(AutoId);
                    detailInfo.EventType = 0;
                    detailInfo.EventBrowser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
                    detailInfo.EventBrowserID = HttpContext.Current.Request.Browser.Id; ;
                    if (HttpContext.Current.Request.Browser.Beta)
                        detailInfo.EventBrowserIsBata = "测试版";
                    else
                        detailInfo.EventBrowserIsBata = "正式版";

                    detailInfo.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;
                    detailInfo.EventDate = DateTime.Now;
                    if (HttpContext.Current.Request.Browser.Win16)
                        detailInfo.EventSysByte = "16位系统";
                    else
                        if (HttpContext.Current.Request.Browser.Win32)
                            detailInfo.EventSysByte = "32位系统";
                        else
                            detailInfo.EventSysByte = "64位系统";
                    detailInfo.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
                    detailInfo.SourceIP = Common.MySpider.GetClientIP();
                    detailInfo.IPLocation = Common.MySpider.GetIPLocation(detailInfo.SourceIP);
                    detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
                    detailInfo.WebsiteOwner = bll.WebsiteOwner;
                    detailInfo.ModuleType = "thevote";
                    if (bll.IsLogin)
                    {
                        detailInfo.EventUserID = bll.GetCurrUserID();
                    }
                    detailInfo.ShareTimestamp = "3";
                    bll.Add(detailInfo);

                    int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='3'  ", bll.WebsiteOwner, int.Parse(AutoId)));
                    int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='3' ", bll.WebsiteOwner, int.Parse(AutoId)));
                    int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='3' ", bll.WebsiteOwner, int.Parse(AutoId)));

                    bll.Update(new TheVoteInfo(),string.Format(" IP={0},PV={1},UV={2} ",ipCount,pvCount,uvCount),string.Format(" AutoId={0} ",AutoId));
                }
                else
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendFormat("<li>");
                    str.AppendFormat("<div>{0}</div>", "没有数据");
                    str.AppendFormat("</li>");
                    needList.InnerHtml = str.ToString();
                }
            }
        }

        private void GetTheVoteInfo(string autoId)
        {
            StringBuilder str = new StringBuilder();
            tvInfo = bll.Get<BLLJIMP.Model.TheVoteInfo>(" autoId=" + autoId);
            if (tvInfo != null)
            {
                needList.InnerHtml = ToHtml(tvInfo);
            }
            else
            {
                str.AppendFormat("<li>");
                str.AppendFormat("<div>{0}</div>", "没有数据");
                str.AppendFormat("</li>");
                needList.InnerHtml = str.ToString();
            }
        }

        /// <summary>
        /// 返回投票内容
        /// </summary>
        /// <param name="tvInfo"></param>
        /// <returns></returns>
        private string ToHtml(BLLJIMP.Model.TheVoteInfo tvInfo)
        {
            StringBuilder htmlStr = new StringBuilder();
            string classStr = "";
            string selectStr = "";
            string str = "";
            BLLJIMP.Model.UserVoteInfo uvInfo = null;
            if (bll.IsLogin)
            {
                this.userInfo = DataLoadTool.GetCurrUserModel();
                string whereStr = string.Format(" UserId='{0}' and VoteId='{1}'", userInfo.UserID, tvInfo.AutoId);
                uvInfo = bll.Get<BLLJIMP.Model.UserVoteInfo>(whereStr);
            }
            //uvInfo = null;
            if (uvInfo != null)
            {
                classStr = "toupiaobox toupiaoover";
            }
            else
            {
                if (tvInfo.VoteSelect == "1")
                {
                    classStr = "toupiaobox";
                    selectStr = "radio";
                    str = "以下选项为单选";
                }
                else if (tvInfo.VoteSelect == "2")
                {
                    classStr = "toupiaobox duoxuan";
                    selectStr = "checkbox";
                    str = "以下选项为多选";
                }
            }
            htmlStr.AppendFormat("<link rel=\"stylesheet\" href=\"styles/css/style.css?v=0.0.1\">");
            htmlStr.AppendFormat("<div id=\"toupiao\" class=\"{0}\">", classStr);
            htmlStr.AppendFormat("<div><img src=\"{0}\"></div>", tvInfo.ThumbnailsPath);
            htmlStr.AppendFormat("<div class=\"title\">{0}</div>", tvInfo.VoteName);
            htmlStr.AppendFormat("<p class=\"note\">{0}</p>", str);
            htmlStr.AppendFormat("<input type=\"hidden\" id=\"SelectStr\"  value=\"{0}\" />", selectStr);
            htmlStr.AppendFormat("<input type=\"hidden\" id=\"AutoId\"  value=\"{0}\" />", tvInfo.AutoId);
            List<BLLJIMP.Model.DictionaryInfo> dInfos = bll.GetList<BLLJIMP.Model.DictionaryInfo>(" ForeignKey='" + tvInfo.AutoId + "'");
            foreach (var item in dInfos)
            {
                htmlStr.AppendFormat("<input name=\"radiocheck2\" type=\"{0}\" class=\"radioinput\" id=\"{1}\" value=\"\" v=\"{2}\">", selectStr, selectStr + item.AutoID, item.AutoID);
                htmlStr.AppendFormat("<div class=\"mainconcent\">");
                htmlStr.AppendFormat("<label class=\"inputlabel\" for=\"{0}\"></label>", selectStr + item.AutoID);
                htmlStr.AppendFormat("<span class=\"inputicon\"><span class=\"icon\"></span></span>");
                htmlStr.AppendFormat("<div class=\"inputtext\" >{0}</div>", item.ValueStr);
                htmlStr.AppendFormat("<div class=\"jindubar\">");
                double f = 0;
                if (tvInfo.VoteNumbers != 0)
                {
                    f = Math.Round((Convert.ToDouble(item.VoteNums) / Convert.ToDouble(tvInfo.VoteNumbers)) * 100, 0);
                }
                htmlStr.AppendFormat("<span class=\"jindu\" style=\"width:{0}%;\"></span>", f);
                htmlStr.AppendFormat("<span class=\"peoplenum\">{0}票数</span>", item.VoteNums);


                htmlStr.AppendFormat("<span class=\"pecent\">{0}%</span>", f);
                htmlStr.AppendFormat("</div></div>");
            }
            string dName = "";
            if (uvInfo != null)
            {
                dInfos = bll.GetList<BLLJIMP.Model.DictionaryInfo>(" AutoId in (" + uvInfo.DiInfoId + ")");

                if (dInfos != null)
                {
                    foreach (BLLJIMP.Model.DictionaryInfo item in dInfos)
                    {
                        dName += item.ValueStr + " ";
                    }
                }
            }

            htmlStr.AppendFormat("<span class=\"button\" id=\"btnSave\" onclick=\"SaveInfo()\">投票</span>");
            htmlStr.AppendFormat("<span class=\"toupiaoinfo\" >你已投过票，投票项为\"{0}\"</span>", dName);
            htmlStr.AppendFormat("</div>");
            return htmlStr.ToString();
        }
    }
}