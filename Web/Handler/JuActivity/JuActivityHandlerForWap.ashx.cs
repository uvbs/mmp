using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using System.Text;
using System.Data;

namespace ZentCloud.JubitIMP.Web.Handler.JuActivity
{
    /// <summary>
    /// JuActivityHandlerForWap 的摘要说明
    /// </summary>
    public class JuActivityHandlerForWap : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLJuActivity juActivityBll;
        BLLUser userBll;
        BLLWeixin weixinBll;
        SystemSet systemSet;
        UserInfo userModel;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                userBll = new BLLUser("");
                weixinBll = new BLLWeixin("");
                this.juActivityBll = new BLLJuActivity("");
                this.systemSet = this.juActivityBll.GetSysSet();
                string Action = context.Request["Action"];

                switch (Action)
                {
                    case "QueryJuActivityForWapCommon"://活动查询
                        result = QueryJuActivityForWapCommon(context);
                        break;
                    case "QueryActivityListForSpreadRank"://活动排名列表
                        result = QueryActivityListForSpreadRank(context);
                        break;

                    case "QueryJuActivityForWap":
                        result = QueryJuActivityForWap(context);
                        break;
                    case "AddJuActivityPraise"://增加赞
                        result = AddJuActivityPraise(context);
                        break;
                    //case "QueryJuMaster"://查询专家库
                    //    result = QueryJuMaster(context);
                    //    break;
                    //case "QueryJuMasterFeedBack"://查询专家库留言//所有人可见
                    //    result = QueryJuMasterFeedBack(context);
                    //    break;
                    //case "AddJuMasterUserLinkerInfo"://提交联系信息
                    //    result = AddJuMasterUserLinkerInfo(context);
                    //    break;
                    //case "AddJuMasterFeedBack"://提交留言
                    //    result = AddJuMasterFeedBack(context);
                    //    break;
                    case "QueryJuActivitySpreadRank"://提交留言
                        result = QueryJuActivitySpreadRank(context);
                        break;

                 

                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            context.Response.Write(result);
        }

       
        private string QueryJuActivitySpreadRank(HttpContext context)
        {

            int monitorPlanID = Convert.ToInt32(context.Request["MonitorPlanID"]);
            string sort = context.Request["sort"];

            DataTable dt = new DataTable();
            StringBuilder sbContent = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(sort) && !monitorPlanID.Equals(0))
            {
                dt = this.juActivityBll.QueryJuActivitySpreadRank(monitorPlanID, sort);
            }

            if (dt != null)
            {

                sbContent.AppendLine("<table style=\"width:100%;\">");
                sbContent.AppendLine("<thead>");
                sbContent.AppendLine("<tr>");
                sbContent.AppendLine("<th style=\"width:15%\">排行</th>");
                sbContent.AppendLine("<th style=\"width:28%\">姓名</th>");
                sbContent.AppendLine("<th style=\"width:30%\">独立IP</th>");
                sbContent.AppendLine("<th style=\"width:34%\">访问量(PV)</th>");
                sbContent.AppendLine("</tr>");
                sbContent.AppendLine("</thead>");
                sbContent.AppendLine("<tbody>");
                sbContent.AppendLine("<tr>");
                sbContent.AppendFormat("<td colspan=\"4\"><hr/></td>");
                sbContent.AppendLine(" </tr>");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbContent.AppendLine("<tr>");
                    sbContent.AppendFormat("<th style=\"width:10%;text-align:center;\">{0}</th>", i + 1);
                    sbContent.AppendFormat("<td style=\"width:30%;text-align:center;\">{0}</td>", dt.Rows[i]["LinkName"].ToString().Split('-')[0]);
                    sbContent.AppendFormat("<td style=\"width:30%;text-align:center;\">{0}</td>", dt.Rows[i]["IP"].ToString());
                    sbContent.AppendFormat("<td style=\"width:30%;text-align:center;\">{0}</td>", dt.Rows[i]["PV"].ToString());
                    sbContent.AppendLine(" </tr>");
                    if (i < dt.Rows.Count - 1)
                    {
                        sbContent.AppendLine("<tr>");
                        sbContent.AppendFormat("<td colspan=\"4\"><hr/></td>");
                        sbContent.AppendLine(" </tr>");
                    }
                }
                sbContent.AppendLine("</tbody>");
                sbContent.AppendLine("</table>");

                //divContentList.InnerHtml = sbContent.ToString();
            }
            return sbContent.ToString();
        }

        private string QueryJuActivityForWeb(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);

            StringBuilder strWhere = new StringBuilder();

            List<JuActivityInfo> dataList = this.juActivityBll.GetLit<JuActivityInfo>(rows, page, strWhere.ToString(), "Sort DESC,ActivityStartDate DESC");

            int totalCount = this.juActivityBll.GetCount<JuActivityInfo>(strWhere.ToString());

            return Common.JSONHelper.ListToEasyUIJson(totalCount, dataList);
        }

        /// <summary>
        /// 增加赞
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddJuActivityPraise(HttpContext context)
        {
            string juActivityID = context.Request["JuActivityID"];
            JuActivityInfo model = juActivityBll.Get<JuActivityInfo>(string.Format("JuActivityID='{0}'", juActivityID));
            if (model.UpCount == null)
            {
                model.UpCount = 0;
            }
            model.UpCount++;
            return juActivityBll.Update(model).ToString().ToLower();

        }


        /// <summary>
        /// wap 活动查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityForWap(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string queryType = context.Request["queryType"];
            string activityAddress = context.Request["ActivityAddress"];
            string activityStartDate = context.Request["ActivityStartDate"];//传入的是最近天数
            string recommendCate = context.Request["RecommendCate"];
            string isFee = context.Request["IsFee"];
            string keyWord = context.Request["KeyWord"];
            string fromSearchHtml = context.Request["fromSearchHtml"];
            List<JuActivityInfo> dataList = new List<JuActivityInfo>();
            int count = 0;
            dataList = juActivityBll.QueryJuActivityData(queryType, out count, activityAddress, activityStartDate, recommendCate, isFee, keyWord, page, rows);
            int totalpage = this.juActivityBll.GetTotalPage(count, rows);
            if (string.IsNullOrEmpty(activityAddress) && string.IsNullOrEmpty(activityStartDate) && string.IsNullOrEmpty(recommendCate) && string.IsNullOrEmpty(isFee) && string.IsNullOrEmpty(keyWord) && (page == 1) && (fromSearchHtml == "0"))
            {

                return ConverHtmlFormateTop(dataList, rows, ((totalpage > page) && page == 1), page);//最新活动

            }
            if ((recommendCate.Equals("精选活动")) && string.IsNullOrEmpty(activityAddress) && (string.IsNullOrEmpty(activityStartDate)) && (string.IsNullOrEmpty(isFee)) && (string.IsNullOrEmpty(keyWord)) && (page == 1) && (string.IsNullOrEmpty(fromSearchHtml)))
            {
                return ConverHtmlFormateTop(dataList, rows, ((totalpage > page) && page == 1), page);//精选活动
            }
            if (recommendCate.Equals("聚比特活动") && page.Equals(1))
            {
                return ConverHtmlFormateTop(dataList, rows, ((totalpage > page) && page == 1), page);//聚比特活动

            }

            return ConverHtmlFormate(dataList, rows, keyWord, ((totalpage > page) && page == 1), page);//

        }



        /// <summary>
        /// wap 活动查询 所有用户可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryJuActivityForWapCommon(HttpContext context)
        {
            string aid = context.Request["aid"];//ZCJ_UserInfo AutoID 十六进制
            string m = context.Request["m"]; /// ZCJ_WXMemberInfo  MemberID 十六进制
            string type = context.Request["type"];
            UserInfo userInfo = juActivityBll.Get<UserInfo>(string.Format("AutoID={0}", Convert.ToInt32(aid, 16)));
            if (userInfo == null)
            {
                return "";
            }

            WXMemberInfo regInfo = juActivityBll.Get<WXMemberInfo>(string.Format("MemberID={0}", Convert.ToInt32(m, 16)));
            if (m == null)
            {
                return "";
            }

            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            List<JuActivityInfo> dataList = new List<JuActivityInfo>();
            //int count = 0;
            int totalCount = 0;
            dataList = juActivityBll.QueryJuActivityData("search", out totalCount, null, null, null, null, null, page, rows, userInfo.UserID);
            int totalpage = this.juActivityBll.GetTotalPage(totalCount, rows);
            if (type.Equals("rank"))
            {
                ConverHtmlFormateJuactivityRankCommon(m, dataList, rows, ((totalpage > page) && page == 1), page);//
            }
            return ConverHtmlFormateJuactivityCommon(m, dataList, rows, ((totalpage > page) && page == 1), page, aid);//

        }

        /// <summary>
        /// 活动影响力排行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryActivityListForSpreadRank(HttpContext context)
        {
            string userId = context.Request["uid"];
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            List<JuActivityInfo> dataList = new List<JuActivityInfo>();

            int totalCount = 0;

            dataList = juActivityBll.QueryJuActivityData("search", out totalCount, null, null, null, null, null, page, rows, userId);
            int totalpage = this.juActivityBll.GetTotalPage(totalCount, rows);

            return ConverHtmlFormateActivityListForSpreadRank(dataList, rows, ((totalpage > page) && page == 1), page);//

        }


        ///// <summary>
        ///// 专家库查询
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMaster(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    List<JuMasterInfo> dataList = new List<JuMasterInfo>();
        //    dataList = juActivityBll.GetList<JuMasterInfo>("");
        //    return ConverHtmlFormateMaster(dataList);




        //}

        ///// <summary>
        ///// 查询专家库留言列表
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string QueryJuMasterFeedBack(HttpContext context)
        //{
        //    int page = Convert.ToInt32(context.Request["page"]);
        //    int rows = Convert.ToInt32(context.Request["rows"]);
        //    string MasterID = context.Request["MasterID"];
        //    string FeedBackStatus = context.Request["FeedBackStatus"];
        //    StringBuilder sbWhere = new StringBuilder();

        //    sbWhere.AppendFormat("MasterID='{0}'", MasterID);
        //    if (!string.IsNullOrEmpty(FeedBackStatus))
        //    {
        //        if (FeedBackStatus.Equals("0"))
        //        {
        //            FeedBackStatus = "未处理";
        //        }
        //        if (FeedBackStatus.Equals("1"))
        //        {
        //            FeedBackStatus = "已回复";
        //        }

        //        sbWhere.AppendFormat(" And ProcessStatus='{0}'", FeedBackStatus);


        //    }
        //    int count = juActivityBll.GetCount<JuMasterFeedBack>(sbWhere.ToString());
        //    int totalpage = this.juActivityBll.GetTotalPage(count, rows);

        //    List<JuMasterFeedBack> dataList = new List<JuMasterFeedBack>();
        //    dataList = juActivityBll.GetLit<JuMasterFeedBack>(rows, page, sbWhere.ToString(), "FeedBackID DESC");

        //    return ConverHtmlFormateMasterFeedBack(dataList, rows, ((totalpage > page) && page == 1));




        //}

        ///// <summary>
        ///// 提交联系信息
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddJuMasterUserLinkerInfo(HttpContext context)
        //{
        //    try
        //    {
        //        JuMasterUserLinkerInfo model = new JuMasterUserLinkerInfo();
        //        model.MasterID = context.Request["MasterID"];
        //        //model.SubUserID = context.Request["SubUserID"];
        //        model.Name = context.Request["Name"];
        //        model.Company = context.Request["Company"];
        //        model.Title = context.Request["Title"];
        //        model.Mobile = context.Request["Mobile"];
        //        model.Email = context.Request["Email"];
        //        model.OtherDescription = context.Request["OtherDescription"];

        //        model.SubmitDate = DateTime.Now;
        //        model.SubmitIP = Common.MySpider.GetClientIP();

        //        if (string.IsNullOrWhiteSpace(model.Name))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "请输入您的姓名！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }

        //        if (string.IsNullOrWhiteSpace(model.Mobile) && string.IsNullOrWhiteSpace(model.Email))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "手机或者邮箱至少填入一个！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }



        //        if (!Common.ValidatorHelper.PhoneNumLogicJudge(model.Mobile) && !string.IsNullOrWhiteSpace(model.Mobile))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "手机号码格式有误！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }
        //        if (!Common.ValidatorHelper.EmailLogicJudge(model.Email) && !string.IsNullOrWhiteSpace(model.Email))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "邮箱地址格式有误！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }

        //        model.LinkerID = int.Parse(this.juActivityBll.GetGUID(BLLJIMP.TransacType.AddJuMasterUserLinkerInfo));

        //        if (juActivityBll.Add(model))
        //        {
        //            resp.Status = 1;
        //            resp.Msg = "添加成功！";
        //        }
        //        else
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "添加失败！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "特殊异常：" + ex.Message;
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);


        //}



        ///// <summary>
        ///// 提交留言信息
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string AddJuMasterFeedBack(HttpContext context)
        //{

        //    try
        //    {
        //        JuMasterFeedBack model = new JuMasterFeedBack();

        //        model.MasterID = context.Request["MasterID"];
        //        //model.UserID = context.Request["UserID"];
        //        model.UserNickName = context.Request["UserNickName"];
        //        model.FeedBackContent = context.Request["FeedBackContent"];
        //        model.FeedBackType = context.Request["FeedBackType"];
        //        model.SubmitDate = DateTime.Now;
        //        model.SubmitIP = Common.MySpider.GetClientIP();
        //        model.ProcessStatus = "未处理";

        //        if (string.IsNullOrWhiteSpace(model.UserNickName))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "请输入您的姓名！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }

        //        if (string.IsNullOrWhiteSpace(model.FeedBackContent))
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "请输入留言内容！";
        //            return Common.JSONHelper.ObjectToJson(resp);
        //        }

        //        model.FeedBackID = int.Parse(this.juActivityBll.GetGUID(BLLJIMP.TransacType.AddJuMasterFeedBackInfo));

        //        if (juActivityBll.Add(model))
        //        {
        //            resp.Status = 1;
        //            resp.Msg = "添加成功！";
        //        }
        //        else
        //        {
        //            resp.Status = 0;
        //            resp.Msg = "添加失败！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "特殊异常：" + ex.Message;
        //    }

        //    return Common.JSONHelper.ObjectToJson(resp);

        //}


        /// <summary>
        /// 格式化查询活动列表
        /// </summary>
        /// <param name="memberIDHex">ZCJ_WXMemberInfo MemberID 十六进制</param>
        /// <param name="dataList"></param>
        /// <param name="rows"></param>
        /// <param name="isShowBtnNext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string ConverHtmlFormateJuactivityCommon(string memberIDHex, List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1, string userAutoIDHex = "0")
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            if (pageIndex == 1)
            {
                sb.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10px 10px 0 10px;width:100%;\">");
            }
            else
            {
                sb.AppendLine("<table  style=\"margin-top :0;margin-left:5;padding:0 10px 0 10px;width:100%;\">");

            }

            sb.AppendLine("<tbody>");
            foreach (var item in dataList)
            {

                sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/wxad/{0}/{1}/detail.chtml?{2}={3}\" onclick=\"GotoRel(this)\"><td >",
                    Convert.ToString(item.JuActivityID, 16),
                    memberIDHex,
                    this.systemSet.UserAutoIDHexKey,
                    userAutoIDHex
                    );
                sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", item.ActivityName);
                sb.AppendLine("<br />");
                sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", item.ActivityStartDate));
                sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", item.ThumbnailsPath);
                sb.AppendLine("</td></tr>");
                sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            if (isShowBtnNext)
            {
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化活动排名表
        /// </summary>
        /// <param name="memberIDHex"></param>
        /// <param name="dataList"></param>
        /// <param name="rows"></param>
        /// <param name="isShowBtnNext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string ConverHtmlFormateActivityListForSpreadRank(List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }

            foreach (var item in dataList)
            {
                sb.AppendFormat("<li data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"div\" data-icon=\"arrow-r\" data-iconpos=\"right\" data-theme=\"c\" class=\"ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-li-has-thumb ui-btn-up-c\">");
                sb.AppendLine("<div class=\"ui-btn-inner ui-li\">");
                sb.AppendLine("<div class=\"ui-btn-text\">");
                sb.AppendFormat("<a href=\"/JuActivity/Wap/JuActivitySpreadRank.aspx?sort=pv&monitorPlanID={0}&userID={1}\" data-ajax=\"false\" class=\"ui-link-inherit\">", item.MonitorPlanID, item.UserID);
                sb.AppendFormat("<img src=\"{0}\" width=\"80\" height=\"80\" class=\"ui-li-thumb\">  ", item.ThumbnailsPath);
                sb.AppendLine("<h2 class=\"ui-li-heading\">");
                sb.AppendLine(item.ActivityName);
                sb.AppendLine("</h2>");
                sb.AppendFormat("<p class=\"ui-li-desc\">  {0}</p> ", string.Format("{0:f}", item.ActivityStartDate));
                sb.AppendLine("</a>");
                sb.AppendLine("</div>");
                sb.AppendLine("<span class=\"ui-icon ui-icon-arrow-r ui-icon-shadow\">&nbsp;</span>");
                sb.AppendLine("</div>");
                sb.AppendLine(" </li>");


                //sb.AppendFormat(string.Format(@"<li><a href=""/JuActivity/Wap/JuActivitySpreadRank.aspx?sort=pv&monitorPlanID={3}&userID={4}""            data-ajax=""false"">            <img src=""{0}"" width=""80"" height=""80"" />            <h2>                {1}</h2>            <p>                {2}</p>        </a></li>",
                //      item.ThumbnailsPath,
                //      item.ActivityName,
                //      item.ActivityStartDate,
                //      item.MonitorPlanID,
                //      item.UserID
                //    ));
            }
            if (isShowBtnNext)
            {
                //sb.AppendFormat("<li data-corners=\"false\" data-shadow=\"false\" data-iconshadow=\"true\" data-wrapperels=\"div\" data-icon=\"arrow-r\" data-iconpos=\"right\" data-theme=\"c\" class=\"ui-btn ui-btn-icon-right ui-li-has-arrow ui-li ui-li-has-thumb ui-btn-up-c\">");   
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);
                //sb.AppendLine(" </li>"); 
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化查询活动列表-排行
        /// </summary>
        /// <param name="memberIDHex">ZCJ_WXMemberInfo MemberID 十六进制</param>
        /// <param name="dataList"></param>
        /// <param name="rows"></param>
        /// <param name="isShowBtnNext"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        private string ConverHtmlFormateJuactivityRankCommon(string memberIDHex, List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            if (pageIndex == 1)
            {
                sb.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10px 10px 0 10px;width:100%;\">");
            }
            else
            {
                sb.AppendLine("<table  style=\"margin-top :0;margin-left:5;padding:0 10px 0 10px;width:100%;\">");

            }

            sb.AppendLine("<tbody>");
            foreach (var item in dataList)
            {

                //sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/wxad/{0}/{1}/detail.chtml\" onclick=\"GotoRel(this)\"><td >", Convert.ToString(item.JuActivityID, 16), memberIDHex);

                sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\"><td>", Convert.ToString(item.JuActivityID, 16), memberIDHex);

                sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", item.ActivityName);
                sb.AppendLine("<br />");
                sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", item.ActivityStartDate));
                sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", item.ThumbnailsPath);
                sb.AppendLine("</td></tr>");
                sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");



            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            if (isShowBtnNext)
            {
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sb.ToString();
        }


        //查询聚活动
        private string ConverHtmlFormate(List<JuActivityInfo> dataList, int rows, string highLight = "", bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            if (pageIndex == 1)
            {
                sb.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10px 10px 0 10px;width:100%;\">");
            }
            else
            {
                sb.AppendLine("<table  style=\"margin-top :0;margin-left:5;padding:0 10px 0 10px;width:100%;\">");

            }

            sb.AppendLine("<tbody>");
            foreach (var item in dataList)
            {
                if (!string.IsNullOrWhiteSpace(highLight))
                    item.ActivityName = item.ActivityName.Replace(highLight, "<span style=\"color:red;\">" + highLight + "</span>");

                sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/JuActivity/Wap/ArticleDetail.aspx?id={0}\" onclick=\"GotoRel(this)\"><td >", item.JuActivityID);
                //sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"GotoRel(this)\" ><td >", item.ActivityWebsite);
                sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", item.ActivityName);
                sb.AppendLine("<br />");
                sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", item.ActivityStartDate));
                sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", item.ThumbnailsPath);
                sb.AppendLine("</td></tr>");
                sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");



            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            if (isShowBtnNext)
            {
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sb.ToString();
        }

        private string ConverHtmlFormateTop(List<JuActivityInfo> dataList, int rows, bool isShowBtnNext = false, int pageIndex = 1)
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            if (pageIndex == 1)
            {
                sb.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10px 10px 0 10px;width:100%;\">");
            }
            else
            {
                sb.AppendLine("<table  style=\"margin-top :0;margin-left:5;padding:0 10px 0 10px;width:100%;\">");

            }

            sb.AppendLine("<tbody>");
            for (int i = 0; i < dataList.Count; i++)
            {
                if (i == 0)
                {
                    //sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"GotoRel(this)\" style=\"background:url(/img/hb/hb1.jpg );width:100%;height:100px; repeat-x;\")\" >", list[i].ActivityWebsite);
                    // sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"GotoRel(this)\">", dataList[i].ActivityWebsite);
                    sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/JuActivity/Wap/ArticleDetail.aspx?id={0}\" onclick=\"GotoRel(this)\">", dataList[0].JuActivityID);
                    sb.AppendFormat("<td colspan=\"2\" valign=\"bottom\" >");
                    sb.AppendFormat("<div style=\"position:relative;\">");

                    //sb.AppendFormat("<img src=\"{0}\" class=\"topimg\" width:100% style=\"height:80px;margin-right:10px;\" />", list[0].TopImgPath);

                    sb.AppendFormat("<img src=\"{0}\" class=\"topimg\" style=\"width:100%;height:auto;margin-right:10px;\" onload=\"settopimage()\" />", dataList[0].TopImgPath);

                    sb.AppendFormat("<div style=\"width:100%;filter:alpha(Opacity=100);-moz-opacity:0.8;opacity: 0.8; background-color:#3C3C3C;position:absolute; bottom: 0px;\">");
                    sb.AppendFormat("<span style=\"color:#FFFFFF;margin-left:5px;text-shadow:none;\">");

                    sb.AppendFormat(dataList[0].ActivityName);
                    sb.AppendFormat("</span>");
                    sb.AppendFormat("</div>");
                    sb.AppendFormat("</div>");
                    sb.AppendFormat("</td>");
                    sb.AppendFormat(" </tr>");

                    sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");


                }

                else
                {


                    // sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"GotoRel(this)\" ><td >", dataList[i].ActivityWebsite);
                    sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/JuActivity/Wap/ArticleDetail.aspx?id={0}\" onclick=\"GotoRel(this)\"><td >", dataList[i].JuActivityID);
                    sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", dataList[i].ActivityName);
                    sb.AppendLine("<br />");
                    sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", dataList[i].ActivityStartDate));
                    sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                    sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", dataList[i].ThumbnailsPath);
                    sb.AppendLine("</td></tr>");
                    sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");
                }


            }
            //            for (int i=0;i<dataList.Count;i++)
            //            {
            //                if (i==0)
            //                {
            //                  sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"GotoRel(this)\" >", dataList[i].ActivityWebsite);
            //    sb.AppendFormat("<td colspan=\"2\">") ;
            //    sb.AppendFormat("<div style=\"width:100%;\" >");
            //     sb.AppendFormat("<img src=\"{0}\" style=\"width:100%;height:80px;margin-right:10px;\" />",dataList[0].TopImgPath) ;
            //sb.AppendFormat("</div>");

            //    sb.AppendFormat("<div style=\"filter:alpha(Opacity=100);-moz-opacity:0.8;opacity: 0.8; background-color:#3C3C3C;overflow:hidden; \">");
            //    sb.AppendFormat("<span style=\"color:#FFFFFF;margin-left:5;\">");

            //     sb.AppendFormat(dataList[0].ActivityName);
            //    sb.AppendFormat("</span>");
            //     sb.AppendFormat("</div>");
            //      sb.AppendFormat("</td>");
            //    sb.AppendFormat(" </tr>");

            //  sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");


            //                }

            //                else
            //    {


            //                sb.AppendFormat(" <tr rel=\"{0}\" onclick=\"GotoRel(this)\" ><td >", dataList[i].ActivityWebsite);
            //                sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", dataList[i].ActivityName);
            //                sb.AppendLine("<br />");
            //                sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;\" >{0}</div>", string.Format("{0:f}", dataList[i].ActivityStartDate));
            //                sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
            //                sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", dataList[i].ThumbnailsPath);
            //                sb.AppendLine("</td></tr>");
            //                sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");
            //            }


            //            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            if (isShowBtnNext)
            {
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sb.ToString();
        }


        private string ConverHtmlFormateJubit(List<JuActivityInfo> dataList, int rows, string highLight = "", bool isShowBtnNext = false)
        {

            StringBuilder sb = new StringBuilder();
            if (dataList.Count.Equals(0))
            {
                return "";
            }
            sb.AppendLine("<table  style=\"margin-top :5;margin-left:5;padding:10px;width:100%;\">");
            sb.AppendLine("<tbody>");
            foreach (var item in dataList)
            {
                if (!string.IsNullOrWhiteSpace(highLight))
                    item.ActivityName = item.ActivityName.Replace(highLight, "<span style=\"color:red;\">" + highLight + "</span>");

                sb.AppendFormat(" <tr onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/JuActivity/Wap/ArticleDetail.aspx?id={0}\" onclick=\"GotoRel(this)\" ><td >", item.JuActivityID);
                sb.AppendFormat("<span style=\"font-size:100%;line-height:100%;color:Black\" >{0}</span>", item.ActivityName);
                sb.AppendLine("<br />");
                sb.AppendFormat("<div  style=\"font-size: 90%;line-height: 100%;margin-top:5px;\" >{0}</div>", string.Format("{0:f}", item.ActivityStartDate));
                sb.AppendLine("</td><td valign=\"top\" align=\"right\"  >");
                sb.AppendFormat("<img src=\"{0}\" style=\"width:50px;height:50px;margin-right:10px;\" /> ", item.ThumbnailsPath);
                sb.AppendLine("</td></tr>");
                sb.AppendFormat("<tr><td colspan=\"2\"><hr /></td></tr>");



            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            if (isShowBtnNext)
            {
                sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\" ></div>");
                sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);


            }
            return sb.ToString();
        }

        //        /// <summary>
        //        /// 专家库查询
        //        /// </summary>
        //        /// <param name="dataList"></param>
        //        /// <returns></returns>
        //     private string ConverHtmlFormateMaster(List<JuMasterInfo> dataList){
        //      StringBuilder sb=new StringBuilder();
        //      if (dataList.Count==0)
        //    {
        //         return "";
        //    }
        //     foreach (var item in dataList)
        //    {
        //         sb.AppendLine("<div class=\"msg-item-wrapper-expert\">"); 
        //         sb.AppendLine("<div style=\"margin-left:5px;margin-top:5px;margin-bottom:5px;\">");
        //         sb.AppendLine("<table style=\"width:100%;\">");
        //         sb.AppendLine("<tbody>");
        //         sb.AppendFormat("<tr rel=\"/JuActivity/Wap/JuMasterDetails.aspx?masterid={0}\" onclick=\"GotoRel(this)\">",item.MasterID);
        //  sb.AppendLine(" <td valign=\"top\" style=\"width:90px;\" >");
        //sb.AppendFormat("<img src=\"{0}\" style=\"width:75.75px;height:100px;\" />",item.HeadImg);
        //sb.AppendLine("</td>");
        //    sb.AppendLine("<td >");
        //sb.AppendLine(" <div>");
        //sb.AppendLine("<font style=\"font-weight:bold;font-size: 16px;font-weight: 700;text-overflow: ellipsis;font-family: Helvetica,Arial,sans-serif;\">");
        //    sb.AppendLine(item.MasterName);
        //         sb.AppendLine("</font>");
        //         sb.AppendLine("&nbsp;");
        //         sb.AppendLine(item.Title);




        //         sb.AppendLine("</div>");
        //         sb.AppendLine(" <div style=\"font-size:100%;line-height:100%;color:Black;margin-top:5px;font-family: Helvetica,Arial,sans-serif;\" >");
        //sb.AppendLine(item.Company);
        //         sb.AppendLine("</div>");
        //         sb.AppendLine("</td>");
        //         sb.AppendLine("</tr>");

        //          sb.AppendLine("</tbody>");

        //          sb.AppendLine("</table>");

        //          sb.AppendLine(" <div style=\"font-family: Helvetica,Arial,sans-serif;color:##5B5B5B;margin-left:5px;\">");
        //          sb.AppendLine("简介: ");
        //          string sum = item.Summary;
        //          if (sum!= null)
        //    {
        //        if (sum.Length > 21)
        //    {
        //        sum = sum.Substring(0, 20);
        //    }
        //    }
        //         sb.AppendLine(sum);
        //          sb.AppendLine("</div>");
        //         sb.AppendLine("</div>");
        //         sb.AppendLine("</div>");


        //    }
        //     return sb.ToString();


        //      }


        ///// <summary>
        ///// 专家库查询
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMaster(List<JuMasterInfo> dataList)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        sb.AppendLine("<div >");
        //        sb.AppendFormat("<div style=\"margin-left:5px;margin-top:5px;margin-bottom:5px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\" rel=\"/JuActivity/Wap/JuMasterDetails.aspx?masterid={0}\" onclick=\"GotoRel(this)\" >", item.MasterID);
        //        sb.AppendLine("<table style=\"width:100%;\">");
        //        sb.AppendLine("<tbody>");
        //        //sb.AppendFormat("<tr rel=\"/JuActivity/Wap/JuMasterDetails.aspx?masterid={0}\" onclick=\"GotoRel(this)\"  >", item.MasterID);
        //        sb.AppendFormat("<tr>");
        //        sb.AppendLine(" <td valign=\"top\" style=\"width:90px;\" >");
        //        sb.AppendFormat("<img src=\"{0}\" style=\"width:75.75px;height:100px;\" />", item.HeadImg);
        //        sb.AppendLine("</td>");
        //        sb.AppendLine("<td>");
        //        sb.AppendLine(" <div>");
        //        sb.AppendLine("<label style=\"font-weight:bold;font-size: 20px;font-weight:700;font-family: Helvetica,Arial,sans-serif;\">");
        //        sb.AppendLine(item.MasterName);
        //        sb.AppendLine("</label>");



        //        sb.AppendLine("</div>");
        //        sb.AppendLine("<div style=\"margin-top:5px;color:#8E8E8E;\">");
        //        sb.AppendLine("<img src=\"/JuActivity/Wap/image/master/postion.gif\" width=\"12px\" height=\"12px\">");
        //        sb.AppendLine(item.Title);

        //        sb.AppendLine("</div>");

        //        sb.AppendLine(" <div style=\"font-size:100%;line-height:100%;color:#8E8E8E;margin-top:5px;font-family: Helvetica,Arial,sans-serif;margin-top:5px;\" >");
        //        sb.AppendLine("<img src=\"/JuActivity/Wap/image/master/company.gif\" width=\"12px\" height=\"12px\">");
        //        sb.AppendLine(item.Company);
        //        sb.AppendLine("</div>");

        //        sb.AppendLine("</td>");
        //        sb.AppendLine("</tr>");

        //        sb.AppendLine("</tbody>");

        //        sb.AppendLine("</table>");

        //        sb.AppendLine(" <div style=\"font-family: Helvetica,Arial,sans-serif;color:#8E8E8E;margin-left:5px;\">");
        //        sb.AppendLine("<img src=\"/JuActivity/Wap/image/master/summary.gif\" width=\"12px\" height=\"12px\">");
        //        sb.AppendLine("简介: ");
        //        string sum = item.Summary;
        //        if (sum != null)
        //        {
        //            if (sum.Length > 81)
        //            {
        //                sum = sum.Substring(0, 80) + "...";
        //            }
        //        }
        //        sb.AppendLine(sum);
        //        sb.AppendLine("</div>");
        //        sb.AppendLine("</div>");
        //        sb.AppendLine("</div>");
        //        sb.AppendLine("<hr noshade size=1 align=center width=100%");


        //    }
        //    return sb.ToString();


        //}




        ///// <summary>
        ///// 专家库留言查询
        ///// </summary>
        ///// <param name="dataList"></param>
        ///// <returns></returns>
        //private string ConverHtmlFormateMasterFeedBack(List<JuMasterFeedBack> dataList, int rows, bool isShowBtnNext = false)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (dataList.Count == 0)
        //    {
        //        return "";
        //    }
        //    foreach (var item in dataList)
        //    {
        //        sb.AppendLine("<div style=\"border: 1px solid #CCC;margin-top:10px;\">");
        //        sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");

        //        sb.AppendFormat("{0} 发表于 {1}", item.UserNickName, item.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"));

        //        sb.AppendLine("</div>");

        //        sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");

        //        sb.Append(item.FeedBackContent);
        //        // 请问因学习需要可以自带笔记本电脑在图书馆无线上网查资料吗?（总馆和分馆都行吗）
        //        sb.AppendLine("</div>");



        //        var FeedBackDialogue = juActivityBll.Get<JuMasterFeedBackDialogue>(string.Format("FeedBackID='{0}'", item.FeedBackID));
        //        if (FeedBackDialogue != null)
        //        {
        //            sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;text-align: left;font-weight: bold;font-size: 16px;color:#930;\"> ");
        //            sb.AppendFormat("专家回复 {0}", FeedBackDialogue.SubmitDate.ToString("yyyy-MM-dd HH:mm:ss"));
        //            //管理员回复 2013-10-21 13:12:25
        //            sb.AppendLine("</div>");
        //            sb.AppendLine("<div style=\"margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");
        //            sb.Append(FeedBackDialogue.DialogueContent);
        //            //可以的。

        //            sb.AppendLine("</div>");



        //        }


        //        sb.AppendLine("</div>");


        //    }
        //    if (isShowBtnNext)
        //    {
        //        sb.AppendFormat("<div class=\"progressBar\" id=\"progressBar\" style=\"display:none;\"></div>");
        //        sb.AppendFormat("<div id=\"btnNext\" isshow=\"yes\" style=\"display:block;text-align:center;font-family: Helvetica,Arial,sans-serif;font-size: 12px;color: #666666;line-height: 18px;\"> 向上拉动显示下{0}条</div>", rows);

        //    }


        //    return sb.ToString();


        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
