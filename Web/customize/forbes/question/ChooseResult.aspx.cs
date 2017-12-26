using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    public partial class ChooseResult : ForbesQuestionBase
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 第一次答题分数
        /// </summary>
        public int scoreFirst;
        /// <summary>
        /// 第二次答题分数
        /// </summary>
        public int scoreSecond;
        /// <summary>
        /// 是否显示第二次分数
        /// </summary>
        public bool isShowSecond;
        protected void Page_Load(object sender, EventArgs e)
        {

            int correctCountFirst = bll.GetCount<ForbesQuestionPersonal>(string.Format(" UserId='{0}' And Status=1  And Count={1} And IsCorrect=1", CurrentUserInfo.UserID, 1));
            scoreFirst = (int)Math.Round(correctCountFirst * 3.33);


            int secondCount = bll.GetCount<ForbesQuestionPersonal>(string.Format(" UserId='{0}' And Status=1  And Count={1}", CurrentUserInfo.UserID, 2));
            if (secondCount==30)
            {
                isShowSecond = true;
            }
            int correctCountSecond = bll.GetCount<ForbesQuestionPersonal>(string.Format(" UserId='{0}' And Status=1  And Count={1} And IsCorrect=1", CurrentUserInfo.UserID,2));
            scoreSecond = (int)Math.Round(correctCountSecond * 3.33);




        }
    }
}