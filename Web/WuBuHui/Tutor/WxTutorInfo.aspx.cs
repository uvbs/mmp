using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Tutor
{
    public partial class WxTutorInfo : System.Web.UI.Page
    {
        public string UserId;
        public BLLJIMP.Model.TutorInfo tInfo;
        public BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public string TagStr;
        public bool zan = false;
        public bool isUserRegistered = true;
        public string IsFollowedString = string.Empty;
        public int UserLevel =1;
        BLLJIMP.BLLUserScore bllUserScore;
        public int FansNumInt = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UserId = Request["UserId"];
                if (!string.IsNullOrEmpty(UserId))
                {
                    BLLJIMP.Model.UserInfo uinfo = bllUser.GetCurrentUserInfo();
                    bllUserScore = new BLLUserScore(uinfo.UserID);
                    //uInfo = DataLoadTool.GetCurrUserModel();
                    GetTutor(UserId);
                    GetTagStr();
                   // BLLJuActivity juActivityBll = new BLLJuActivity();

                    if (string.IsNullOrEmpty(uinfo.TrueName) || string.IsNullOrEmpty(uinfo.Phone))
                    {
                        isUserRegistered = false;
                    }
                    
                }
            }
            catch (Exception ex)
            {

                Response.End();
            }
            //if (!IsPostBack)
            //{

           // }
        }

        /// <summary>
        /// 获取话题标签
        /// </summary>
        private void GetTagStr()
        {

            List<BLLJIMP.Model.ArticleCategory> actegorys = bllUser.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='word' AND WebsiteOwner='{0}'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));
            foreach (BLLJIMP.Model.ArticleCategory item in actegorys)
            {
                TagStr += "<input class=\"checkbox\" type=\"checkbox\" Name=\"word\" value=\"" + item.AutoID + "\" id=\"word" + item.AutoID + "\">";
                TagStr += "<label for=\"word" + item.AutoID + "\" class=\"discusstag\">";
                TagStr += "<span class=\"wbtn wbtn_gary\"><span class=\"iconfont\"></span></span>" + item.CategoryName;
                TagStr += "</label>";
            }

        }

        /// <summary>
        /// 获取导师详情
        /// </summary>
        /// <param name="UserId"></param>
        private void GetTutor(string UserId)
        {
            tInfo = bllUser.Get<BLLJIMP.Model.TutorInfo>(string.Format(" AutoId='{0}'", UserId));
            if (tInfo != null)
            {
                BLLJIMP.Model.UserInfo uInfo = bllUser.Get<BLLJIMP.Model.UserInfo>(string.Format("UserId='{0}'", tInfo.UserId));
                IheadImg.Src = tInfo.TutorImg;
                txtExplain.Text = tInfo.TutorExplain;
                txtTrade.Text = GetTradeStr(tInfo.TradeStr);//行业标签
                txtProfessiona.Text = GettxtProfessionaStr(tInfo.ProfessionalStr);//专业标签
                IsFollowedString = bllUser.CheckFollow(bllUser.GetCurrentUserInfo().UserID, tInfo.UserId) ? "关注" : "已关注";
                FansNum.InnerText = uInfo.FansCount.ToString();
                FansNumInt = uInfo.FansCount;
                BLLJIMP.Model.ForwardingRecord frecord = bllUser.Get<BLLJIMP.Model.ForwardingRecord>(string.Format(" FUserID='{0}' AND RUserID='{1}' AND websiteOwner='{2}' AND TypeName='导师赞'", DataLoadTool.GetCurrUserModel().UserID, tInfo.UserId, bllUser.WebsiteOwner));
                if (frecord != null)
                {
                    zan = true;
                }
                UserLevel = bllUserScore.GetUserLevelByTotalScore(uInfo.HistoryTotalScore);
            }
        }




       

        /// <summary>
        /// 获取专业标签
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string GettxtProfessionaStr(string ProfessionalStr)
        {
            StringBuilder sbStr = new StringBuilder("");
            if (!string.IsNullOrEmpty(ProfessionalStr) && ProfessionalStr.Length > 1)
            {
                List<BLLJIMP.Model.ArticleCategory> aCategorys = bllUser.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='professional' AND AutoID in ({0}) AND WebsiteOwner='{1}'", ProfessionalStr.Substring(0, ProfessionalStr.Length - 1), bllUser.WebsiteOwner));
                if (aCategorys.Count > 0)
                {
                    foreach (BLLJIMP.Model.ArticleCategory item in aCategorys)
                    {
                        sbStr.AppendFormat("<span class=\"wbtn_tag wbtn_main\">{0}</span>", item.CategoryName);
                    }
                }
            }
            return sbStr.ToString();
        }

        /// <summary>
        /// 获取行业标签
        /// </summary>
        /// <param name="p"></param>
        private string GetTradeStr(string TradeStr)
        {
            StringBuilder sbStr = new StringBuilder("");
            if (!string.IsNullOrEmpty(TradeStr) && TradeStr.Length > 1)
            {
                List<BLLJIMP.Model.ArticleCategory> aCategorys = bllUser.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='trade' AND AutoID in ({0}) AND WebsiteOwner='{1}'", TradeStr.Substring(0, TradeStr.Length - 1), bllUser.WebsiteOwner));
                if (aCategorys.Count > 0)
                {
                    foreach (BLLJIMP.Model.ArticleCategory item in aCategorys)
                    {
                        sbStr.AppendFormat("<span class=\"wbtn_tag wbtn_main\">{0}</span>", item.CategoryName);
                    }
                }
            }
            return sbStr.ToString();
        }
    }
}