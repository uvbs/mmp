using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.SportsShow
{
    public partial class beforCanceled : System.Web.UI.Page
    {
        BLLForbesQuestion bllQuestion = new BLLForbesQuestion();
        protected List<ForbesQuestionResult> ListQuestionResult = new List<ForbesQuestionResult>();
        private string ActivityName = "2015上海体博会体商测试";
        private string CancelCode = "2015";
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo user = bllQuestion.GetCurrentUserInfo();
            string QuestionResultListKey = "QuestionResultList" + user.AutoID.ToString() + Session.SessionID;
            if (IsPostBack)
            {
                ListQuestionResult = (List<ForbesQuestionResult>)Common.DataCache.GetCache(QuestionResultListKey);//写入缓存
                return;
            }
            if (user == null)
            {
                this.Response.Redirect(Common.ConfigHelper.GetConfigString("noPmsUrl").ToLower(), true);
                return;
            }
            ListQuestionResult = bllQuestion.GetQuestionResultList(user.UserID, bllQuestion.WebsiteOwner, ActivityName);
            
            #region 检查是否已填写用户信息 没有且有中奖 则跳转到填写页
            if (string.IsNullOrWhiteSpace(user.Phone) || string.IsNullOrWhiteSpace(user.Ex1)
                || string.IsNullOrWhiteSpace(user.Ex2) || string.IsNullOrWhiteSpace(user.Ex3))
            {
                bool haveGift = false;
                bool haveCancelCode = false;
                for (int i = 0; i < ListQuestionResult.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(ListQuestionResult[i].GiftCode))
                    {
                        haveGift = true;
                        break;
                    }
                    if (ListQuestionResult[i].CreateDate < Convert.ToDateTime("2015-11-05"))
                    {
                        haveCancelCode = true;
                    }
                }
                string Type = "0";
                if (haveGift)
                {
                    Type = "1";
                }
                else if (haveCancelCode)
                {
                    Type = "2";
                }
                if (Type == "1" || Type == "2")
                {
                    this.Response.Redirect("info.aspx?type=" + Type, true);
                    return;
                }
            }
            #endregion
            Common.DataCache.SetCache(QuestionResultListKey, ListQuestionResult, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(6));//写入缓存
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            string hdnValue = hdnIndex.Value;
            UserInfo user = bllQuestion.GetCurrentUserInfo();
            string QuestionResultListKey = "QuestionResultList" + user.AutoID.ToString() + Session.SessionID;
            string Code = hdnCode.Value;

            #region 检查 核销码 兑奖人 提交的内容
            if (string.IsNullOrWhiteSpace(hdnValue)) {
                Common.WebMessageBox.Show(this, "提交出错！");
                return;
            }
            int Index = Convert.ToInt32(hdnValue);

            if (ListQuestionResult[Index].CancelCode == CancelCode)
            {
                Common.WebMessageBox.Show(this, "已经核销过！");
                return;
            }
            if (Code.Trim().ToUpper() != CancelCode)
            {
                Common.WebMessageBox.Show(this, "核销码错误！");
                return;
            }
            if (user == null)
            {
                Common.WebMessageBox.Show(this, "登录失败");
                return;
            }
            if (user.UserID != ListQuestionResult[Index].UserId)
            {
                Common.WebMessageBox.Show(this, "用户验证未通过！");
                return;
            }
            #endregion

            ListQuestionResult[Index].ModifyDate = DateTime.Now;
            ListQuestionResult[Index].CancelCode = CancelCode;

            if (bllQuestion.Update(ListQuestionResult[Index]))
            {
                Common.DataCache.SetCache(QuestionResultListKey, ListQuestionResult, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(6));//写入缓存
            }
            else
            {
                Common.WebMessageBox.Show(this, "核销失败！");
                return;
            }
        }
        protected void btnToQuestionPage_Click(object sender, EventArgs e)
        {
            if (ListQuestionResult.Count < 2 && DateTime.Now < Convert.ToDateTime("2015-11-05")) {
                this.Response.Redirect("question.aspx", true);
            }
        }
        /// <summary>
        /// 查询第几次答题
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected string GetNumZn(int i)
        {
            switch (i)
            {
                case 0:
                    return "一";
                case 1:
                    return "二";
                case 2:
                    return "三";
            }
            return "";
        }
        /// <summary>
        /// 查询得分评论
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected string GetScoreReview(int score)
        {
            switch (score)
            {
                case 0:
                    return "没药救了，咋办呐，大家快点来救我！";
                case 20:
                    return "额，这点体商有点尴尬啊！";
                case 40:
                    return "额，这点体商有点尴尬啊！";
                case 60:
                    return "额，这点体商有点尴尬啊！";
                case 80:
                    return "额，这点体商有点尴尬啊！";
                case 100:
                    return "哇塞，体商爆棚啦！";
            }
            return "";
        }
    }
}