using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class ForwardRank : System.Web.UI.Page
    {
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity("");
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected List<UserModel> UserList;
        protected WebsiteInfo website = new WebsiteInfo();
        protected string ActivityName;
        protected void Page_Load(object sender, EventArgs e)
        {
            string activityId = Request["activityId"];//报名活动ID
            if (string.IsNullOrEmpty(activityId))
            {
                Response.Write("活动ID 必传");
                Response.End();
            }
            JuActivityInfo activityInfo = bllJuactivity.GetJuActivity(int.Parse(activityId));
            if (activityInfo == null)
            {
                Response.Write("活动不存在");
                Response.End();
            }
            website = bllActivity.GetWebsiteInfoModelFromDataBase();
            ActivityName = activityInfo.ActivityName;
            List<MonitorLinkInfo> monitorLinkList;
            if (website.SortType == 1)
            {
                monitorLinkList = bllActivity.GetList<MonitorLinkInfo>(100, string.Format(" ActivityId={0}", activityInfo.SignUpActivityID), " OpenCount DESC");
            }
            else
            {
                monitorLinkList = bllActivity.GetList<MonitorLinkInfo>(string.Format(" ActivityId={0}", activityInfo.SignUpActivityID));
            }
            int totalCount = 0;
            List<ActivityDataInfo> data = bllActivity.GetActivityDataInfoList(activityInfo.SignUpActivityID, "", out totalCount);

            UserList = new List<UserModel>();
            foreach (var monitorLink in monitorLinkList)
            {
                if (!string.IsNullOrEmpty(monitorLink.LinkName))
                {
                    UserInfo user = bllUser.GetUserInfo(monitorLink.LinkName);
                    if (user != null)
                    {
                        UserModel userModel = new UserModel();
                        userModel.HeadImg = bllUser.GetUserDispalyAvatar(user);
                        userModel.ShowName = user.TrueName;
                        if (website.SortType == 0)
                        {
                            userModel.SpreadCount = data.Where(p => p.SpreadUserID == user.UserID).Count();
                        }
                        else
                        {
                            userModel.SpreadCount = monitorLink.OpenCount;
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