using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    public partial class Share : ForbesQuestionBase
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 等级 A- A A+ A++
        /// </summary>
        public string level;
        /// <summary>
        /// 等级描述
        /// </summary>
        public string levelDesc;
        public int percent = 60;
        protected void Page_Load(object sender, EventArgs e)
        {
            int count = int.Parse(Request["count"]);

            int correctCountFirst = bll.GetCount<ForbesQuestionPersonal>(string.Format(" UserId='{0}' And Status=1  And Count={1} And IsCorrect=1", CurrentUserInfo.UserID, count));
            int score = (int)Math.Round(correctCountFirst * 3.33);

            GetLevel(score);



        }

        /// <summary>
        /// 计算等级
        /// </summary>
        /// <param name="score">分数</param>
        /// <returns></returns>
        private void GetLevel(int score)
        {
            if (score >= 95)
            {
                levelDesc = "太棒了！！你在模拟答题中击败了大多数理财师，期待你在正式答题和比赛中的瞩目表现！";
                percent = 99;
                level= "A++";

            }
            if (score >= 90 && score < 95)
            {
                percent = 90;
                levelDesc = "你的专业能力过硬，案例经验丰富，相信你一定能够在正式的比赛中一展身手！";
                level = "A+";

            }
            if (score >= 80 && score < 90)
            {
                percent =85;
                levelDesc = "恭喜你，你已经具备一个优秀理财师所具备的专业素养，继续努力，你会在正式比赛中大放异彩！";
                level = "A";
            }
            percent = new Random().Next(60,79);
            levelDesc = "其实你离优秀只有一点距离。多一点点努力，你就是下一个优选理财师。继续加油！";
            level = "A-";


        }
    }
}