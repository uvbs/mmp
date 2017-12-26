using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.User
{
    /// <summary>
    /// DistributionTree 的摘要说明
    /// </summary>
    public class DistributionTree : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string userId = context.Request["userId"];
            List<UserInfo> userList = bllUser.GetList<UserInfo>(string.Format("DistributionOffLinePreUserId='{0}'", userId));
            if (userList.Count > 0)
            {

                System.Text.StringBuilder sbJson = new System.Text.StringBuilder();
                sbJson.Append("[");
                for (int i = 0; i < userList.Count; i++)
                {
                    var item = userList[i];
                    var subList = bllUser.GetList<UserInfo>(string.Format("DistributionOffLinePreUserId='{0}'", item.UserID));//直接下级
                    var isParent = false;//是否有下级
                    if (subList.Count > 0)
                    {
                        isParent = true;
                    }
                    string showName = item.UserID;//显示名称
                    string headImg = "/Plugins/zTree/css/zTreeStyle/img/diy/user.png";//头像
                    string icon = "/Plugins/zTree/css/zTreeStyle/img/diy/user.png";//图标
                    if (!string.IsNullOrEmpty(item.WXNickname))
                    {
                        showName = item.WXNickname;
                    }
                    if (!string.IsNullOrEmpty(item.TrueName))
                    {
                        showName = item.TrueName;
                    }
                    if (!string.IsNullOrEmpty(item.WXHeadimgurl))
                    {
                        headImg = item.WXHeadimgurl;
                        icon = item.WXHeadimgurl;
                    }
                    string tip = string.Format("<img src='{0}' align='absmiddle' width='100px' height='100px'/><br/>{1}<br/>一级会员<span style='color:red;'>&nbsp;{2}</span><br/>二级会员<span style='color:red;'>&nbsp;{3}</span><br/>三级会员<span style='color:red;'>&nbsp;{4}</span>", headImg, showName, bll.GetDownUserCount(item.UserID, 1), bll.GetDownUserCount(item.UserID, 2), bll.GetDownUserCount(item.UserID, 3));//提示

                    var title = string.Format("<span style='color:blue;'>{0}</span>&nbsp;<a href='ProjectList.aspx?userId={1}' target='_blank'>查看项目</a>", showName, item.UserID);
                    sbJson.Append("{");
                    sbJson.AppendFormat("name: \"{0}\", id: \"{1}\", count:{2}, times: 1, isParent:\"{3}\",open:false,icon:\"{4}\",tip:\"{5}\"", title, item.UserID, subList.Count, isParent.ToString().ToLower(), icon, tip);
                    sbJson.Append("}");

                    if (i < userList.Count - 1)//追加分隔符
                    {
                        sbJson.Append(",");
                    }

                }
                sbJson.Append("]");
                context.Response.Write( sbJson.ToString());
            }
            else
            {
                context.Response.Write("");
            }
        }


    }
}