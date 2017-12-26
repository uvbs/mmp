using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class AnswerRank : System.Web.UI.Page
    {
        BLLJIMP.BLLQuestion bllQuestion = new BLLJIMP.BLLQuestion();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public string ActivityName = string.Empty;
        public string spreadUserId = string.Empty;
        protected WebsiteInfo website = new WebsiteInfo();
        public  List<UserModel> UserList;
        protected void Page_Load(object sender, EventArgs e)
        {
            string activityId = Request["activityId"];//答题编号
            spreadUserId = Request["sid"];
            if (string.IsNullOrEmpty(activityId))
            {
                Response.Write("问卷ID 必传");
                Response.End();
            }
            BLLJIMP.Model.Questionnaire model = bllQuestion.Get<BLLJIMP.Model.Questionnaire>(string.Format(" WebsiteOwner='{0}' AND QuestionnaireID='{1}' ", bllQuestion.WebsiteOwner, activityId));

            if (model == null)
            {
                Response.Write("问卷不存在");
                Response.End();
            }
            ActivityName = model.QuestionnaireName;
            website = bllQuestion.GetWebsiteInfoModelFromDataBase();
            List<MonitorLinkInfo> monitorLinkList;
            if (website.SortType == 0)
            {
                monitorLinkList = bllQuestion.GetList<MonitorLinkInfo>(string.Format(" ActivityId={0}", activityId));
            }
            else
            {
                monitorLinkList = bllQuestion.GetList<MonitorLinkInfo>(100,string.Format(" ActivityId={0}", activityId)," OpenCount DESC ");
            }

            List<QuestionnaireRecord> recordList = bllQuestion.GetList<QuestionnaireRecord>(string.Format(" QuestionnaireID={0} ",int.Parse(activityId)));

            UserList = new List<UserModel>();

            foreach (var item in monitorLinkList)
            {
                if (!string.IsNullOrEmpty(item.LinkName))
                {
                    UserInfo user = bllUser.GetUserInfo(item.LinkName);
                    if (user != null)
                    {
                        UserModel userModel = new UserModel();
                        userModel.HeadImg = bllUser.GetUserDispalyAvatar(user);
                        userModel.ShowName = user.TrueName;
                        if (website.SortType == 0)
                        {
                            userModel.SpreadCount = recordList.Where(p => p.PreUserId == user.UserID).Count();
                        }
                        else
                        {
                            userModel.SpreadCount = item.OpenCount;
                        }
                        UserList.Add(userModel);
                    }
                }
            }
            UserList = UserList.OrderByDescending(p => p.SpreadCount).ToList();


        }

        /// <summary>
        /// 用户显示模型
        /// </summary>
        public class UserModel
        {

            /// <summary>
            ///头像
            /// </summary>
            public string HeadImg { get; set; }

            /// <summary>
            /// 显示名称
            /// </summary>
            public string ShowName { get; set; }

            /// <summary>
            /// 转发报名数量
            /// </summary>
            public int SpreadCount { get; set; }

        }
    }
}