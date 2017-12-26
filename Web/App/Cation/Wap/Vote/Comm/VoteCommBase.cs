using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Text;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm
{
    /// <summary>
    /// 基类
    /// </summary>
    public class VoteCommBase : System.Web.UI.Page
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo currentUserInfo = new UserInfo();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLJIMP.BLLCompanyWebSite bllWebsite = new BLLJIMP.BLLCompanyWebSite();
        public List<CompanyWebsite_ToolBar> toolbarList = new List<CompanyWebsite_ToolBar>(); 
        protected BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        protected VoteInfo currVote = new VoteInfo();
        public StringBuilder footerHtml = new StringBuilder();

        /// <summary>
        /// 样式自定义
        /// </summary>
        public StringBuilder styleCustomize = new StringBuilder();

        protected override void OnInit(EventArgs e)
        {
            if (!bllUser.IsLogin)
            {
                Response.Write("请用微信打开");
                Response.End();
            }
            else
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                currVote = bllVote.GetVoteInfo(Convert.ToInt32(Request["vid"]));

                if (currVote == null)
                {
                    Response.Redirect("/Error/CommonMsg.aspx?msg=投票活动不存在");
                    Response.End();
                }

                //if (currVote.VoteCountAutoUpdate.Equals(1))
                //{
                //    //检查是否更新票数
                //    bllVote.UpdateUserVoteCount(currVote.AutoID, currentUserInfo);

                //}

                #region footerHtml
                toolbarList = bllWebsite.GetToolBarList(int.MaxValue, 1, bllUser.WebsiteOwner, "nav", currVote.FooterMenuGroup, false);

                if (string.IsNullOrWhiteSpace(currVote.FooterMenuGroup))
                {
                    //默认导航
                    footerHtml.AppendFormat("<div id =\"footer\">");
                    footerHtml.AppendFormat("     <div class=\"menu3 font14\">");
                    footerHtml.AppendFormat("        <ul>");
                    footerHtml.AppendFormat("            <li id=\"li0\" >");
                    footerHtml.AppendFormat("                <a href=\"Index.aspx?vid={0} \">", currVote.AutoID);
                    //footerHtml.AppendFormat("                   <img src = \"images/tab_00.jpg\" alt=\"\" title=\"\" class=\"mTop4\" />");
                    footerHtml.AppendFormat("首页");
                    footerHtml.AppendFormat("                </a>");
                    footerHtml.AppendFormat("            </li>");
                    footerHtml.AppendFormat("            <li id=\"li4\" >");
                    footerHtml.AppendFormat("                <a href=\"Rule.aspx?vid={0}\" >", currVote.AutoID);
                    footerHtml.AppendFormat("                   <div class=\"arrow-right\"></div>海选规则");
                    footerHtml.AppendFormat("                </a>");
                    footerHtml.AppendFormat("            </li>");
                    footerHtml.AppendFormat("            <li id=\"li5\" >");
                    footerHtml.AppendFormat("                <a href=\"SignUp.aspx?vid={0}\" >", currVote.AutoID);
                    footerHtml.AppendFormat("                   <div class=\"arrow-right\"></div>参与海选");
                    footerHtml.AppendFormat("                </a>");
                    footerHtml.AppendFormat("            </li>");
                    footerHtml.AppendFormat("            <li id=\"li6\" >");
                    footerHtml.AppendFormat("                <a href=\"List.aspx?vid={0}\" >", currVote.AutoID);
                    footerHtml.AppendFormat("                   <div class=\"arrow-right\"></div>为TA投票");
                    footerHtml.AppendFormat("                </a>");
                    footerHtml.AppendFormat("            </li>");
                    footerHtml.AppendFormat("        </ul>");
                    footerHtml.AppendFormat("    </div>");
                    footerHtml.AppendFormat("</div>");

                }
                else
                {
                    //自定义导航
                    footerHtml.AppendFormat("<div id =\"footer\">");
                    footerHtml.AppendFormat("     <div class=\"row\">");

                    for (int i = 0; i < toolbarList.Count; i++)
                    {
                        //            < div class="col">
                        //    <i class="iconfont icon-shouyeshixin"></i>
                        //    <a href = "#" > 首页 </ a >
                        //</ div >

                        footerHtml.AppendFormat("<div class=\"col\">");

                        if (!string.IsNullOrWhiteSpace(toolbarList[i].ToolBarImage) && toolbarList[i].ToolBarImage.IndexOf("iconfont") > -1)
                        {
                            footerHtml.AppendFormat("<i class=\"{0}\"></i>", toolbarList[i].ToolBarImage);
                        }

                        footerHtml.AppendFormat("<a href=\"{0}\">", toolbarList[i].ToolBarTypeValue);
                        footerHtml.AppendFormat(toolbarList[i].ToolBarName);
                        footerHtml.AppendFormat("</a>");


                        footerHtml.AppendFormat("</div>");

                    }

                    footerHtml.AppendFormat("    </div>");
                    footerHtml.AppendFormat("</div>");

                }
                #endregion

                styleCustomize.AppendFormat("<style type=\"text/css\">");


                //主题色 主题字色
                if (!string.IsNullOrWhiteSpace(currVote.ThemeColor) && !string.IsNullOrWhiteSpace(currVote.ThemeFontColor))
                {
                    if (currVote.ThemeColor.IndexOf("#") > -1)
                    {
                        currVote.ThemeColor = currVote.ThemeColor.Replace("#", "");
                        currVote.ThemeColor = "#" + currVote.ThemeColor;
                    }
                    if (currVote.ThemeFontColor.IndexOf("#") > -1)
                    {
                        currVote.ThemeFontColor = currVote.ThemeFontColor.Replace("#", "");
                        currVote.ThemeFontColor = "#" + currVote.ThemeFontColor;
                    }
                    styleCustomize.AppendFormat("#footer{1}border-top: 1px {0} solid;background: {0};{2}", currVote.ThemeColor,"{","}");
                    styleCustomize.AppendFormat("#footer a{1}color: {0};{2}", currVote.ThemeFontColor, "{", "}");
                    styleCustomize.AppendFormat(".red{0} background-color:{2} {1} ", "{", "}", currVote.ThemeColor);
                    styleCustomize.AppendFormat(".form_a{0} color:{2} {1} ","{","}",currVote.ThemeFontColor);
                    //.top-search
                    styleCustomize.AppendFormat(".top-search{0} -webkit-box-shadow: 1px 1px 4px {2}, -1px 1px 4px {2},1px -1px 4px {2};     box-shadow: 1px 1px 4px {2}, -1px 1px 4px {2},1px -1px 4px {2}; {1} ", "{", "}", currVote.ThemeColor);
                    //.form_div
                    styleCustomize.AppendFormat(".form_div{0} -webkit-box-shadow: 1px 1px 4px {2}, -1px 1px 4px {2},1px -1px 4px {2};     box-shadow: 1px 1px 4px {2}, -1px 1px 4px {2},1px -1px 4px {2}; {1} ", "{", "}", currVote.ThemeColor);
                    //.btnToVote
                    styleCustomize.AppendFormat(".btnToVote{0} background-color:{2}; color: {3}; {1} ", "{", "}",currVote.ThemeColor, currVote.ThemeFontColor);

                    //.wrapList .btnSort
                    styleCustomize.AppendFormat(".wrapList .btnSort{0} background-color:{2}; color: {3}; {1} ", "{", "}", currVote.ThemeColor, currVote.ThemeFontColor);

                    styleCustomize.AppendFormat(".left_maskbar{0} background:{2}; color: {3}; {1} ", "{", "}", currVote.ThemeColor, currVote.ThemeFontColor);
                    styleCustomize.AppendFormat(".bottom_maskbar{0} background:{2}; color: {3}; {1} ", "{", "}", currVote.ThemeColor, currVote.ThemeFontColor);

                    //.flex-control-nav li a.active {background: #f1698a; cursor: default;}
                    styleCustomize.AppendFormat(".flex-control-nav li a.active{0} background:{2}; {1} ", "{", "}", currVote.ThemeColor);
                    
                    //.votedetail
                    styleCustomize.AppendFormat(".votedetail{0} background:{2}; color: {3}; {1} ", "{", "}", currVote.ThemeColor, currVote.ThemeFontColor);

                    styleCustomize.AppendFormat(".themeColor{0} color:{2} !important; {1} ", "{", "}", currVote.ThemeColor);

                    styleCustomize.AppendFormat(".themeBgColor{0} background-color:{2} !important; {1} ", "{", "}", currVote.ThemeColor);

                    styleCustomize.AppendFormat(".themeFontColor{0} color:{2} !important; {1} ", "{", "}", currVote.ThemeFontColor);

                    //.swiper-pagination-bullet-active
                    styleCustomize.AppendFormat(".swiper-pagination-bullet-active{0} background:{2}; {1} ", "{", "}", currVote.ThemeColor);

                    //.wrapList .btnSearch
                    //styleCustomize.AppendFormat(".wrapList .btnSearch{0} background-color:{2}; color: {3}; {1} ", "{", "}", currVote.ThemeColor, currVote.ThemeFontColor);

                }

                styleCustomize.AppendFormat("</style>");
            }
           
        }
    }
}