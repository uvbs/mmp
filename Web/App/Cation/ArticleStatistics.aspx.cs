using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public static class MyEnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element))) { yield return element; }
            }
        }
    }
    public partial class ArticleStatistics : System.Web.UI.Page
    {
        
        public string RootNodes = "";
        public string articleId = "";
        /// <summary>
        /// 当前用户信息
        /// </summary>
        private UserInfo currentUserInfo;
        /// <summary>
        /// 当前站点所有者
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new ZentCloud.BLLJIMP.BLLWeixin("");
        protected void Page_Load(object sender, EventArgs e)
        {
            articleId = Request["articleId"];
            if (articleId == null)
            {
                Response.End();
            }
            int articleIdint;
            if (!int.TryParse(articleId, out articleIdint))
            {
                Response.End();
            }
            currentUserInfo = DataLoadTool.GetCurrUserModel();
            currWebSiteUserInfo=bll.Get<UserInfo>(string.Format("UserID='{0}'",DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));//
            JuActivityInfo articleInfo = bll.Get<JuActivityInfo>(string.Format("JuActivityID={0}", articleIdint));
            if (articleInfo == null)
            {
                Response.End();
            }
            if (!currentUserInfo.UserType.Equals(1))
            {
                if (!articleInfo.WebsiteOwner.Equals(currentUserInfo.WebsiteOwner))
                {
                    Response.End();
                }

            }
            
            var articleIdHex = Convert.ToString(articleIdint, 16);//文章活动ID十六进制
            string pageUrl = string.Format("http://{0}/{1}/details.chtml", Request.Url.Host, articleIdHex);
            var rootList = bll.GetList<WebAccessLogsInfo>(string.Format("(Ex_PreSpreadUserID is null or Ex_PreSpreadUserID='') And (Ex_PreShareTimestamp is null or Ex_PreShareTimestamp='') And Ex_SpreadUserID !='' And Ex_SpreadUserID is not null  And Ex_ShareTimestamp !=''  And Ex_ShareTimestamp is not null  And PageUrl like '{0}%'  Order by AccessDate ASC", pageUrl));//根节点


            if (rootList.Count > 0)
            {
                rootList = rootList.DistinctBy(p => p.Ex_ShareTimestamp).ToList();
                System.Text.StringBuilder sbRoot = new System.Text.StringBuilder();
                for (int i = 0; i < rootList.Count; i++)
                {
                    var item = rootList[i];

                    //int count = bll.GetCount<WebAccessLogsInfo>(string.Format("Ex_PreSpreadUserID='{0}' And Ex_PreShareTimestamp='{1}' Order by AccessDate ASC", item.Ex_SpreadUserID, item.Ex_ShareTimestamp));
                    var subList = bll.GetList<WebAccessLogsInfo>(string.Format("Ex_PreSpreadUserID='{0}' And Ex_PreShareTimestamp='{1}' Order by AccessDate ASC ", item.Ex_SpreadUserID, item.Ex_ShareTimestamp));
                    subList = subList.DistinctBy(p => p.Ex_ShareTimestamp).ToList();
                    int count = subList.Count;
                    var isParent = false;
                    if (count > 0)
                    {
                        isParent = true;
                    }
                    string wxNickName = "无昵称";
                    string wxHeadImg = "/zTree/css/zTreeStyle/img/diy/user.png";
                    string icon = "/zTree/css/zTreeStyle/img/diy/user.png";
                    var userInfo = bll.Get<UserInfo>(string.Format("UserID='{0}'", item.Ex_SpreadUserID));
                    if (userInfo != null)
                    {
                        if (!string.IsNullOrEmpty(userInfo.WXNickname))
                        {
                            wxNickName = userInfo.WXNickname;
                        }
                        if (!string.IsNullOrEmpty(userInfo.WXHeadimgurlLocal))
                        {
                            wxHeadImg = userInfo.WXHeadimgurlLocal;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(userInfo.WXOpenId))
                            {
                                //拉取用户信息并更新数据库
                                
                                ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo weixinInfo = bllWeixin.GetWeixinUserInfo(currWebSiteUserInfo.UserID, currWebSiteUserInfo.WeixinAppId, currWebSiteUserInfo.WeixinAppSecret, userInfo.WXOpenId);
                                if (weixinInfo != null)
                                {
                                    if (!string.IsNullOrEmpty(weixinInfo.NickName))
                                    {
                                        userInfo.WXNickname = weixinInfo.NickName;
                                    }
                                    if (!string.IsNullOrEmpty(weixinInfo.HeadImgUrl))
                                    {
                                        userInfo.WXHeadimgurl = weixinInfo.HeadImgUrl;
                                    }
                                    //bll.Update(userInfo);

                                }

                            }


                        }


                    }
                    string tip = string.Format("<img src='{0}' align='absmiddle' width='100px' height='100px'/><br/>{1}<br/>被<span style='color:red;'>{2}</span>次转发", wxHeadImg, wxNickName, count);
                    var title = string.Format("<span style='color:blue;'>{0}</span>  <span style='color:red;'>{1}</span>转发 [{2}]", wxNickName, count, string.Format("{0:f}", item.AccessDate));
                    sbRoot.Append("{");
                    sbRoot.AppendFormat("name: \"{0}\", id: \"{1}\", count:{2}, times: 1, isParent:\"{3}\",Ex_SpreadUserID:\"{4}\",Ex_ShareTimestamp:\"{5}\",icon:\"{6}\",tip:\"{7}\"", title, item.AutoID, "1", isParent.ToString().ToLower(), item.Ex_SpreadUserID, item.Ex_ShareTimestamp, icon, tip);
                    sbRoot.Append("}");

                    if (i < rootList.Count - 1)//追加分隔符
                    {
                        sbRoot.Append(",");
                    }

                }
                RootNodes = sbRoot.ToString();
            }
            else
            {
                RootNodes = "{ name: \"暂时没有转发记录\", id: \"0\", count: 0, times: 1, isParent: false }";
            }



        }
    }
}