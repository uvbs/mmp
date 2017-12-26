using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class SignUp : System.Web.UI.Page
    {
        public BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public int PageActionType = 0;//0正常提交，1审核中，2已经是正式学员，3是教师
        public BLLJIMP.Model.UserInfo currUserInfo;
        public BLLJIMP.Model.WebsiteInfo currWebSiteModel;
        public BLLJIMP.Model.UserInfo currWebSiteOwnerModel;
        public string signUpLoginName = string.Empty;
        public string signUploginPwd = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            currUserInfo = DataLoadTool.GetCurrUserModel();
            currWebSiteModel = DataLoadTool.GetWebsiteInfoModel();
            currWebSiteOwnerModel = new BLLJIMP.BLLUser("").GetUserInfo(currWebSiteModel.WebsiteOwner);
            signUpLoginName = Common.Base64Change.EncodeBase64ByUTF8(currWebSiteOwnerModel.UserID);
            signUploginPwd = Common.DEncrypt.ZCEncrypt(currWebSiteOwnerModel.Password);
            GetPageActionType();

        }

        private void GetPageActionType()
        {
            //查询是否已经提交资料正在审核中

            string activityId = Common.ConfigHelper.GetConfigString("HfSignUpActivityID");
            string openId = currUserInfo.WXOpenId;

            //if (DataLoadTool.currIsHFVipUser())
            //{
            //    PageActionType = 2;
            //    return;
            //}

            //if (DataLoadTool.currIsHFTeacher())
            //{
            //    PageActionType = 3;
            //    return;
            //}

            if (bll.Get<BLLJIMP.Model.ActivityDataInfo>(string.Format(" ActivityID = '{0}' and WeixinOpenID = '{1}' and IsDelete = 0 ", activityId, openId)) != null)
            {
                PageActionType = 1;
                return;
            }

        }

    }
}