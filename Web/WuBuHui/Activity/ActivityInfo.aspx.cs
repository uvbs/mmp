using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Activity
{
    public partial class ActivityInfo : System.Web.UI.Page
    {

        public string Id;
        public BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public BLLJIMP.Model.JuActivityInfo model;
        public string classStr = "listbox";
        public UserInfo UserInfo = new UserInfo();
        /// <summary>
        ///是否报名标识
        /// </summary>
        public bool IsSubmit;
        public bool isUserRegistered = true;
        public bool isUserScoreEnough = false;
        public bool isActivityStopped = false;
        /// <summary>
        /// 活动权限
        /// </summary>
        public bool PerActivity = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UserInfo = bll.GetCurrentUserInfo();
                if (string.IsNullOrEmpty(UserInfo.TrueName) || string.IsNullOrEmpty(UserInfo.Phone))
                {

                    isUserRegistered = false;
                }


                Id = Request["id"];
                if (!string.IsNullOrEmpty(Id))
                {
                    BLLJIMP.Model.JuActivityInfo activity = GetActivity(Id);

                    if (activity != null)
                    {
                        if (activity.ActivityEndDate != null)
                        {
                            if (DateTime.Now >= (DateTime)activity.ActivityEndDate)
                            {
                                activity.IsHide = 1;
                            }
                        }
                        if (activity.ActivityIntegral < UserInfo.TotalScore)
                        {
                            isUserScoreEnough = true;
                        }

                        if (activity.IsHide == 1)
                        {
                            isActivityStopped = true;
                        }
                        #region 指定标签的用户可以报名
                        if (!string.IsNullOrEmpty(activity.Tags))
                        {

                            if (!string.IsNullOrEmpty(UserInfo.TagName))
                            {
                                foreach (string item in UserInfo.TagName.Split(','))
                                {
                                    if (activity.Tags.Contains(item))
                                    {
                                        PerActivity = true;
                                        break;
                                    }
                                }
                            }

                        }
                        else
                        {
                            PerActivity = true;
                        }
                        #endregion

                    }
                }

            }
            catch (Exception)
            {

                Response.End();
            }
            //if (!IsPostBack)
            //{

            //}

        }

        private BLLJIMP.Model.JuActivityInfo GetActivity(string id)
        {

            model = bll.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" JuActivityID='{0}'", id));
            if (model != null)
            {
                if (model.ActivityEndDate != null)
                {
                    if (DateTime.Now >= (DateTime)model.ActivityEndDate)
                    {
                        model.IsHide = 1;
                    }
                }
                model.PV = model.PV + 1;
                bll.Update(model);
                txtTitle.Text = model.ActivityName;
                txtStart.Text = model.IsHide == 0 ? "进行中" : "已结束";
                if (model.IsHide==-1)
                {
                    txtStart.Text = "待开始";
                }
                if ((model.MaxSignUpTotalCount > 0 )&& (model.SignUpTotalCount >= model.MaxSignUpTotalCount)&&(model.IsHide==0))
                {

                    txtStart.Text = "已满员";

                }
                classStr = model.IsHide == 0 ? "listbox" : "listbox partyover";
                if (model.IsHide==-1)
                {
                    classStr = "listbox";
                }
                if (model.MaxSignUpTotalCount > 0 && (model.SignUpTotalCount >= model.MaxSignUpTotalCount))
                {
                    classStr = "listbox partyfull";
                }
                txtTime.Text = model.ActivityStartDate.ToString();
                txtAddress.Text = model.ActivityAddress;
                if ((!string.IsNullOrEmpty(model.CategoryId))&&(!model.CategoryId.Equals("0")))
                {
                    txtType.Text = bll.Get<BLLJIMP.Model.ArticleCategory>(string.Format(" AutoID={0}", model.CategoryId)).CategoryName;
                }
                txtContent.Text = model.ActivityDescription;
                txtNum.Text = model.SignUpTotalCount.ToString();
                txtActivityIntegral.Text = model.ActivityIntegral.ToString();
                if (bll.GetCount<ActivityDataInfo>(string.Format("ActivityID={0} And WeixinOpenID='{1}' And IsDelete=0 ", model.SignUpActivityID, UserInfo.WXOpenId)) > 0)
                {
                    IsSubmit = true;
                }

            }
            return model;
        }
    }
}