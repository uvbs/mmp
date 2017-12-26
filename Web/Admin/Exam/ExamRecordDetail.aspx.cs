using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Exam
{
    public partial class ExamRecordDetail : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        public BLLJIMP.Model.Questionnaire QuestionnaireModel = new BLLJIMP.Model.Questionnaire();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        public UserInfo currUserInfo = new UserInfo();


        /// <summary>
        /// 
        /// </summary>
        public bool isSubmit = false;
        /// <summary>
        /// 记录
        /// </summary>
        public QuestionnaireRecord record = new QuestionnaireRecord();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!bll.IsLogin)
            {
                Response.Write("请用微信打开");
                Response.End();
                return;
            }
            string id = Request["id"];
            currUserInfo = bllUser.GetUserInfo(Request["uid"],bll.WebsiteOwner);
            record = bll.Get<QuestionnaireRecord>(string.Format("UserId='{0}' And QuestionnaireID={1}", currUserInfo.UserID, id));
            if (record != null)
            {
                isSubmit = true;
            }

            if (string.IsNullOrEmpty(id))
            {
                Response.Write("无参数");
                Response.End();
                return;
            }

            QuestionnaireModel = bll.Get<BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}", id));
            if (QuestionnaireModel == null)
            {
                Response.Write("试卷不存在");
                Response.End();
                return;
            }

        }
    }
}