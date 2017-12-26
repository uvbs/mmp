using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.SignIn
{
    public partial class SignConfig : System.Web.UI.Page
    {
        /// <summary>
        /// 签到逻辑层
        /// </summary>
        BLLJIMP.BLLSignIn bllSignIn = new BLLJIMP.BLLSignIn();

        public List<Ads> adsList1 = new List<Ads>();
        public List<Ads> adsList2 = new List<Ads>();
        public List<Ads> adsList3 = new List<Ads>();
        public List<Ads> adsList4 = new List<Ads>();
        public List<Ads> adsList5 = new List<Ads>();
        public List<Ads> adsList6 = new List<Ads>();
        public List<Ads> adsList7 = new List<Ads>();

        /// <summary>
        /// 签到
        /// </summary>
        protected BLLJIMP.Model.SignInAddress model = new BLLJIMP.Model.SignInAddress();
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllSignIn.Get<SignInAddress>(string.Format(" WebsiteOwner='{0}' AND Type='Sign' ",bllSignIn.WebsiteOwner));
            if (model!=null)
            {
                if (!string.IsNullOrEmpty(model.MondayAds))
                {
                    adsList1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.MondayAds);
                }
                if (!string.IsNullOrEmpty(model.TuesdayAds))
                {
                    adsList2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.TuesdayAds);
                }
                if (!string.IsNullOrEmpty(model.WednesdayAds))
                {
                    adsList3 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.WednesdayAds);
                }
                if (!string.IsNullOrEmpty(model.ThursdayAds))
                {
                    adsList4 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.ThursdayAds);
                }
                if (!string.IsNullOrEmpty(model.FridayAds))
                {
                    adsList5 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.FridayAds);
                }
                if (!string.IsNullOrEmpty(model.SaturdayAds))
                {
                    adsList6 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.SaturdayAds);
                }
                if (!string.IsNullOrEmpty(model.SundayAds))
                {
                    adsList7 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ads>>(model.SundayAds);
                }
            }

        }

        public class Ads
        {
            /// <summary>
            /// 图片路径
            /// </summary>
            public string img { get; set; }
            /// <summary>
            /// 图片标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 图片跳转
            /// </summary>
            public string url { get; set; }
        }
    }
}