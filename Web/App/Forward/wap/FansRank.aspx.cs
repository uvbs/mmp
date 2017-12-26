using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class FansRank : System.Web.UI.Page
    {
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity("");
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected List<UserModel> UserList;
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
            ActivityName = activityInfo.ActivityName;


            List<MonitorLinkInfo> linkInfoList = bllActivity.GetList<MonitorLinkInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1}", bllActivity.WebsiteOwner, activityInfo.MonitorPlanID));
            if (linkInfoList.Count==0)
            {
                Response.Write("没有转发");
                Response.End();
            }
            string uids = "";//转发人用户id
            foreach (var item in linkInfoList)
            {
                uids += "'" + item.LinkName + "'" + ",";
            }
            uids = uids.TrimEnd(',');

            List<UserInfo> userInfoList = bllUser.GetList<UserInfo>(string.Format(" UserId in ({0})",uids));
            UserList = new List<UserModel>();
            if (userInfoList.Count > 0)
            {
                foreach (UserInfo item in userInfoList)
                {
                    UserModel model = new UserModel();
                    model.HeadImg = item.WXHeadimgurl;
                    model.ShowName = item.TrueName;
                    model.SpreadCount = bllUser.GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND DistributionOwner='{2}' ", bllUser.WebsiteOwner, activityId, item.UserID));
                    UserList.Add(model);
                }
            }
            UserList = UserList.OrderByDescending(p => p.SpreadCount).ToList();





            //List<UserInfo> userModelList = bllUser.GetList<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND DistributionOwner!='' ", bllUser.WebsiteOwner, activityId));
            //string ids = "";
            //foreach (UserInfo item in userModelList)
            //{
            //    ids += "'" + item.DistributionOwner + "'" + ",";
            //}
            //ids = ids.TrimEnd(',');
            //List<UserInfo> userLit = new List<UserInfo>();
            //if (userModelList.Count > 0)
            //{
            //    userLit=bllUser.GetList<UserInfo>(string.Format(" UserID in ({0})", ids));
            //}

            //UserList = new List<UserModel>();
            //foreach (UserInfo item in userLit)
            //{
            //    UserModel user = new UserModel();
            //    user.HeadImg = item.WXHeadimgurl;
            //    user.ShowName = item.TrueName;
            //    user.SpreadCount = userModelList.Where(p => p.DistributionOwner == item.UserID).Count();
            //    UserList.Add(user);
            //}
            //UserList = UserList.OrderByDescending(p => p.SpreadCount).ToList();
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
            /// 粉丝数量
            /// </summary>
            public int SpreadCount { get; set; }

        }
    }
}