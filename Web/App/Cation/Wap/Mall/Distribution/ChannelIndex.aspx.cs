using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class ChannelIndex : DistributionBase
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
        /// ICO图标
        /// </summary>
        protected string icoimport;
        /// <summary>
        /// 站点信息
        /// </summary>
        public WebsiteInfo websiteInfo;
        /// <summary>
        /// 等级
        /// </summary>
        public string LevelName;
        protected void Page_Load(object sender, EventArgs e)
        {

            UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", CurrentUserInfo.UserID));
            if (channelUserInfo == null)
            {
                Response.Redirect("/error/commonmsg.aspx?msg=您还不是渠道身份，无法访问，请联系商家升级为渠道。");
                return;
            }
            CurrentUserInfo = channelUserInfo;
            websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
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
            LevelName = bllUser.Get<UserLevelConfig>(string.Format("AutoId={0}", CurrentUserInfo.ChannelLevelId)).LevelString;
        }
    }
}