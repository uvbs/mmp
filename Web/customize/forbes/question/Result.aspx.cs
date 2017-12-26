using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    public partial class Result : ForbesQuestionBase
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 是否显示第二道题目
        /// </summary>
        public bool isShowSencondQuestion;
        /// <summary>
        /// 总分
        /// </summary>
        public int Score;
        protected void Page_Load(object sender, EventArgs e)
        {
            int count=int.Parse(Request["count"]);
            if (count==1)
            {

                if (bll.GetCount<ForbesQuestionPersonal>(string.Format(" UserId='{0}' And Status=0  And Count=2",CurrentUserInfo.UserID))==30)
                {

                    //第二道题还没有做，让用户选择第二道题目
                    isShowSencondQuestion = true;

                    
                }
                
                
            }
            int correctCount = bll.GetCount<ForbesQuestionPersonal>(string.Format(" UserId='{0}' And Status=1  And Count={1} And IsCorrect=1", CurrentUserInfo.UserID, count));
            Score = (int)Math.Round(correctCount * 3.33);

        }
    }
}