using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class Index :DistributionBase
    {
        /// <summary>
        /// 分销BLL
        /// </summary>
        public ZentCloud.BLLJIMP.BLLDistribution bllDis = new ZentCloud.BLLJIMP.BLLDistribution();
        /// <summary>
        /// 用户
        /// </summary>
        public BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        ///权限BLL
        /// </summary>
        public BLLPermission.BLLMenuPermission bllPms;
        /// <summary>
        /// ICO图标
        /// </summary>
        protected string icoimport;
        /// <summary>
        /// 是否分销员
        /// </summary>
        public int isDistributionMember = 0;
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string websiteOwner = string.Empty;
        /// <summary>
        /// 未下单下级用户数
        /// </summary>
        public int DownUserCountHaveOrder;
        /// <summary>
        /// 已经下单下级 用户数
        /// </summary>
        public int DownUserCountUnHaveOrder;

        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo websiteInfo = new WebsiteInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            bllPms = new BLLPermission.BLLMenuPermission(bllUser.WebsiteOwner);


            if (bllDis.WebsiteOwner == "songhe")
            {
                this.Title = "我要推广";
            }
            else
            {
                this.Title = "我是代言人";
            }

            if (!bllDis.IsLogin)
            {
                this.Response.Write("<span style=\"font-size:40px;\">您还没有登录！</span>");
                this.Response.End();
                return;
            }
            
            isDistributionMember = bllUser.IsDistributionMember(CurrentUserInfo)? 1:0;
            websiteOwner = bllUser.WebsiteOwner;

            //if (isDistributionMember == 0 && websiteOwner == "songhe" && CurrentUserInfo.MemberLevel == 0)
            //{
            //    //如果是非月供宝会员，则直接跳到会员注册页面
            //    //http://www.songhebao.com/app/wap/ApplyMember.aspx

            //    this.Response.Redirect("/app/wap/ApplyMember.aspx");
            //    this.Response.End();
            //    return;
            //}

            DownUserCountHaveOrder = bllUser.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And DistributionOwner='{1}' And DistributionSaleAmountLevel0>0", CurrentUserInfo.WebsiteOwner, CurrentUserInfo.UserID));
            DownUserCountUnHaveOrder = bllUser.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And DistributionOwner='{1}' And (DistributionSaleAmountLevel0=0 Or DistributionSaleAmountLevel0 Is NULL)", CurrentUserInfo.WebsiteOwner, CurrentUserInfo.UserID));

            //头部图标引用
            string iconfontPath = Common.ConfigHelper.GetConfigString("iconfont_comeoncloud");
            if (!string.IsNullOrWhiteSpace(iconfontPath))
            {
                string iconJson = File.ReadAllText(this.Server.MapPath(iconfontPath));
                JToken jToken = JToken.Parse(iconJson);
                
                StringBuilder icoSbu = new StringBuilder();
                icoSbu.AppendLine("@charset \"utf-8\";");
                if (jToken["css_file"] != null) icoSbu.AppendLine(string.Format("@import url(\"{0}\");", jToken["css_file"].ToString()));
                icoSbu.AppendLine(string.Format("@font-face {0}", "{"));
                icoSbu.AppendLine("font-family: \"iconfont\";");
                if (jToken["svg_file"] != null) icoSbu.AppendLine(string.Format("src:url(\"{0}#iconfont\") format('svg');", jToken["svg_file"].ToString()));
                icoSbu.AppendLine(string.Format("{0}", "}"));
                icoimport = icoSbu.ToString();
            }
            websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
                        
        }
    }
}