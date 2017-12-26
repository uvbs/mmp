using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class ActivityDetails : System.Web.UI.Page
    {
        BLLJuActivity juActivityBll;  //活动数据
        public string Id;// 活动编号
        public BLLJIMP.Model.JuActivityInfo jActivityeInfo;
        public List<BLLJIMP.Model.ActivityDataInfo> aDataInfos;
        public string websiteOwner;
        public BLLJIMP.Model.ActivityConfig aconfig;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
                Id = Request["id"];
                juActivityBll = new BLLJuActivity("");
                if (!string.IsNullOrEmpty(Id))
                {
                    aconfig = juActivityBll.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
                    if (aconfig == null)
                    {
                        aconfig = new BLLJIMP.Model.ActivityConfig() { TheOrganizers = "" };
                    }
                    GetActivityInfo(Id);
                }
            }
        }

        public string GetImg(object openId)
        {
            BLLJIMP.Model.UserInfo userInfo = juActivityBll.Get<BLLJIMP.Model.UserInfo>(string.Format(" WXOpenId='{0}'", openId));
            if (userInfo != null)
            {
                if (string.IsNullOrEmpty(userInfo.WXHeadimgurl))
                {
                    return "style/images/6.png";
                }
                else
                {
                    return userInfo.WXHeadimgurl;
                }

            }
            return "style/images/6.png";
        }

        public string GetData(object data)
        {
            return Convert.ToDateTime(data).ToString("HH:mm");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        private void GetActivityInfo(string Id)
        {
            jActivityeInfo = juActivityBll.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" SignUpActivityID='{0}'", Id));
            if (jActivityeInfo != null)
            {
                liTitle.Text = jActivityeInfo.ActivityName;
                LiName.Text = jActivityeInfo.ActivityName;
                liOpen.Text = Convert.ToString(jActivityeInfo.OpenCount);
                liShareTotal.Text = Convert.ToString(jActivityeInfo.ShareTotalCount);
                //Imgsrc.Src = jActivityeInfo.ThumbnailsPath;
                LiAddress.Text = jActivityeInfo.ActivityAddress;
                if (jActivityeInfo.ActivityStartDate != null)
                {
                    LiStartTime.Text = jActivityeInfo.ActivityStartDate.Value.ToString("yyyy年MM月dd日 HH:mm");
                    if (jActivityeInfo.ActivityEndDate != null)
                    {
                        LiEndTiem.Text = "至" + jActivityeInfo.ActivityEndDate.Value.ToString("yyyy年MM月dd日 HH:mm");
                    }
                }
                liContext.Text = jActivityeInfo.ActivityDescription;

                aDataInfos = juActivityBll.GetLit<BLLJIMP.Model.ActivityDataInfo>(6, 1, string.Format("ActivityID='{0}'", jActivityeInfo.SignUpActivityID));
                liPCount.Text = aDataInfos.Count.ToString();
             
            }
        }
    }
}