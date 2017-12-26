using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions
{
    public partial class MessageBox : System.Web.UI.Page
    {
        public int SystemMessageCount = 0;
        public int ReviewCount = 0;
        public int QuestionaryCount = 0;
        public string SysteMessageTime = string.Empty;
        public string ReviewTime = string.Empty;
        public string QuestionaryTime = string.Empty;
        public string IsHaveUnReadMessage = "false";
        BLLJIMP.BLLSystemNotice bllNotice = new BLLJIMP.BLLSystemNotice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetMsgCount();
                IsHaveUnReadMessage = bllNotice.IsHaveUnReadMessage(bllNotice.GetCurrentUserInfo().UserID).ToString();
            }
        }

        public void GetMsgCount()
        {
            try
            {
              
               BLLJIMP.BLLSystemNotice bll = new BLLJIMP.BLLSystemNotice();
               BLLJIMP.Model.UserInfo userInfo = bll.GetCurrentUserInfo();
               ReviewCount = bll.GetUnReadMsgCount(userInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.ReviewReminder);
               SystemMessageCount = bll.GetUnReadMsgCount(userInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage);
               QuestionaryCount = bll.GetUnReadMsgCount(userInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.QuestionaryReminder);

               SysteMessageTime = bll.GetUnReadMsgTime(userInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage).ToString();
               ReviewTime = bll.GetUnReadMsgTime(userInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.ReviewReminder).ToString();
               QuestionaryTime = bll.GetUnReadMsgTime(userInfo.UserID, BLLJIMP.BLLSystemNotice.NoticeType.QuestionaryReminder).ToString();
                
            }
            catch
            {

            }
        }
    }
}