<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Questionnaire.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Questionnaire.wap.Questionnaire" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=QuestionnaireModel.QuestionnaireName %></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="http://static-files.socialcrmyun.com/css/Questionnaire/style.css" rel="stylesheet" type="text/css" />
    <link href="http://static-files.socialcrmyun.com/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .submit1 {
            display: block;
            height: 40px;
            margin: 0px auto;
            border-radius: 5px;
            text-align: center;
            line-height: 40px;
            font-size: 16px;
            font-family: '微软雅黑';
            margin-top: 10px;
            color: #fffbfb !important;
            background-color: #c5c3c3;
        }
    </style>
</head>
<body>
    <section class="box">
        <div class="header"><%=QuestionnaireModel.QuestionnaireName %></div>
        <div class="cont"><%=QuestionnaireModel.QuestionnaireContent%></div>
    </section>
    <section class="box questionnaire bottom50">
        <%ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();%>
        <%List<ZentCloud.BLLJIMP.Model.Question> QuestionList = bll.GetList<ZentCloud.BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", QuestionnaireModel.QuestionnaireID), "Sort Asc");
          if (QuestionnaireModel.EachPageNum <= 0)
          {

              QuestionnaireModel.EachPageNum = QuestionList.Count;
          }
          bool hasPage = !(QuestionnaireModel.EachPageNum <= 0 || QuestionnaireModel.EachPageNum >= QuestionList.Count);

          List<string> ABCDList = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "H", "M", "N" };
          Dictionary<string, string> dirYearDate = new Dictionary<string, string>();
          dirYearDate.Add("month", "年月");
          dirYearDate.Add("date", "年月日");
          dirYearDate.Add("time", "时间");
          dirYearDate.Add("datetime", "日期时间");
          for (int i = 0; i < QuestionList.Count; i++)
          {
              List<ZentCloud.BLLJIMP.Model.Answer> AnswerList = bll.GetList<ZentCloud.BLLJIMP.Model.Answer>(string.Format("QuestionID={0}", QuestionList[i].QuestionID));
              switch (QuestionList[i].QuestionType)
              {
                  case 0:
                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\" data-type=\"singleselect\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下选项为单选&nbsp;{0}</p>", QuestionList[i].IsRequired == 1 ? "(<font color='#ffc853'>必选</font>)" : "(选填)"));

                      for (int k = 0; k < AnswerList.Count; k++)
                      {
                          ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetali = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} AND AnswerID='{3}' ", bll.GetCurrUserID(), QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID, AnswerList[k].AnswerID));
                          Response.Write("<div>");
                          if (recordDetali != null)
                          {
                              Response.Write(string.Format("<input name=\"radiocheck{0}\"  type=\"radio\" checked=\"checked\" class=\"radioinput\" id=\"rd{1}\" value=\"{2}\">", i, (i.ToString() + k.ToString()), AnswerList[k].AnswerID));
                          }
                          else
                          {
                              if (isSubmit)
                              {
                                  Response.Write(string.Format("<input name=\"radiocheck{0}\" disabled=\"disabled\" type=\"radio\" class=\"radioinput\" id=\"rd{1}\" value=\"{2}\">", i, (i.ToString() + k.ToString()), AnswerList[k].AnswerID));
                              }
                              else
                              {
                                  Response.Write(string.Format("<input name=\"radiocheck{0}\"  type=\"radio\" class=\"radioinput\" id=\"rd{1}\" value=\"{2}\">", i, (i.ToString() + k.ToString()), AnswerList[k].AnswerID));
                              }
                          }
                          Response.Write(string.Format("<div class=\"mainconcent {0}\">", (k == AnswerList.Count - 1) ? "" : "borderBottom"));
                          Response.Write(string.Format("<label class=\"inputlabel\" for=\"rd{0}\"></label>", i.ToString() + k.ToString()));
                          Response.Write("<span class=\"inputicon\"><span class=\"icon\"></span></span>");
                          Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>", AnswerList[k].AnswerName));
                          Response.Write("</div>");
                          Response.Write("</div>");//结束
                      }
                      Response.Write("</div>");//结束
                      break;
                  case 1:
                      Response.Write(string.Format("<div  class=\"question toupiaobox {3} duoxuan\" data-type=\"muselect\" data-required=\"{0}\" data-questionid=\"{1}\" data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下选项为多选&nbsp;{0}</p>", QuestionList[i].IsRequired == 1 ? "(<font color='#ffc853'>必填</font>)" : "(选填)"));

                      for (int l = 0; l < AnswerList.Count; l++)
                      {
                          ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetali = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} AND AnswerID='{3}' ", bll.GetCurrUserID(), QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID, AnswerList[l].AnswerID));
                          Response.Write("<div>");
                          if (recordDetali != null)
                          {
                              Response.Write(string.Format("<input name=\"checkboxcheck{0}\"  checked=\"checked\" type=\"checkbox\" disabled=\"disabled\" class=\"radioinput\" id=\"cb{1}\" value=\"{2}\">", i, (i.ToString() + l.ToString()), AnswerList[l].AnswerID));
                          }
                          else
                          {
                              if (isSubmit)
                              {
                                  Response.Write(string.Format("<input name=\"checkboxcheck{0}\"  type=\"checkbox\" class=\"radioinput\" disabled=\"disabled\" id=\"cb{1}\" value=\"{2}\">", i, (i.ToString() + l.ToString()), AnswerList[l].AnswerID));
                              }
                              else
                              {
                                  Response.Write(string.Format("<input name=\"checkboxcheck{0}\"  type=\"checkbox\" class=\"radioinput\" id=\"cb{1}\" value=\"{2}\">", i, (i.ToString() + l.ToString()), AnswerList[l].AnswerID));
                              }

                          }
                          Response.Write(string.Format("<div class=\"mainconcent {0}\">", (l == AnswerList.Count - 1) ? "" : "borderBottom"));
                          Response.Write(string.Format("<label class=\"inputlabel\" for=\"cb{0}\"></label>", i.ToString() + l.ToString()));
                          Response.Write("<span class=\"inputicon\"><span class=\"icon\"></span></span>");
                          Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>", AnswerList[l].AnswerName));
                          Response.Write("</div>");
                          Response.Write(" </div>");
                      }
                      Response.Write("</div>");//结束
                      break;
                  case 2:
                      ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetaliModel = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2}  ", bll.GetCurrUserID(), QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));

                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\"  data-type=\"text\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下选项为填空&nbsp;{0}</p>", QuestionList[i].IsRequired == 1 ? "(<font color='#ffc853'>必填)</font>" : "(选填)"));
                      Response.Write("<div class=\"mainconcent\">");

                      if (recordDetaliModel != null)
                      {
                          Response.Write("<input type=\"text\" class=\"text\" disabled=\"disabled\" value=" + recordDetaliModel.AnswerContent + " />");
                      }
                      else
                      {
                          Response.Write("<input type=\"text\" class=\"text\" />");
                      }

                      Response.Write("</div>");
                      Response.Write("</div>");//结束
                      break;
                  case 3:

                      Response.Write(string.Format("<div class=\"question toupiaobox {3}\" data-type=\"groupselect\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下选项为&nbsp;{0}</p>", QuestionList[i].IsRequired == 1 ? "(<font color='#ffc853'>必填</font>)" : "(选填)"));
                      Response.Write("<div class=\"grouptype\">");
                      Response.Write("<table class=\"grouptable\">");
                      List<string> gNameList = QuestionList[i].AnswerGroupName.Split(',').ToList();
                      Response.Write("<tr>");
                      Response.Write("<td class=\"groupanswertd\">");
                      for (int k = 0; k < AnswerList.Count; k++)
                      {
                          Response.Write(string.Format("<div>{0} {1}</div>", ABCDList[k], AnswerList[k].AnswerName));
                      }
                      Response.Write(string.Format("<div style=\"display:none;\"><select class=\"groupselecttemp\" data-group=\"temp\">"));
                      Response.Write(string.Format("<option value=\"{0}\" data-use=\"0\">{1}</option>", "", ""));
                      for (int ki = 0; ki < AnswerList.Count; ki++)
                      {
                          Response.Write(string.Format("<option value=\"{0}\" data-use=\"0\">{1}</option>", AnswerList[ki].AnswerID, ABCDList[ki]));
                      }
                      Response.Write("</select></div>");
                      Response.Write("</td>");

                      if (gNameList.Count > 0)
                      {
                          List<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail> recordDetaliList = bll.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} ", bll.GetCurrUserID(), QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));

                          foreach (var item in gNameList)
                          {
                              Response.Write("<td class=\"groupselecttd\">");
                              if (isSubmit)
                                  Response.Write(string.Format("{0}<br /><select class=\"groupselect\" disabled=\"disabled\" data-group=\"{0}\">", item));
                              else
                                  Response.Write(string.Format("{0}<br /><select class=\"groupselect\" data-group=\"{0}\">", item));

                              Response.Write(string.Format("<option value=\"{0}\">{1}</option>", "", ""));
                              for (int ki = 0; ki < AnswerList.Count; ki++)
                              {
                                  ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetaliCount = recordDetaliList.FirstOrDefault(p => p.AnswerContent == item && p.AnswerID == AnswerList[ki].AnswerID);
                                  if (recordDetaliCount != null)
                                  {
                                      Response.Write(string.Format("<option value=\"{0}\" selected=\"selected\"  data-use=\"1\">{1}</option>", AnswerList[ki].AnswerID, ABCDList[ki]));
                                  }
                                  else if (!recordDetaliList.Exists(p => p.AnswerID == AnswerList[ki].AnswerID))
                                  {
                                      Response.Write(string.Format("<option value=\"{0}\" data-use=\"0\">{1}</option>", AnswerList[ki].AnswerID, ABCDList[ki]));
                                  }
                              }
                              Response.Write("</select>");
                              Response.Write("</td>");

                          }
                      }
                      Response.Write("</tr>");
                      Response.Write(" </table>");
                      Response.Write("</div>");
                      Response.Write("</div>");//结束
                      break;
                  case 4:
                      ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail yearModel = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} ", bll.GetCurrUserID(), QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));
                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\" data-type=\"provincecity\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下为省市区地址填写&nbsp;{0}</p>", QuestionList[i].IsRequired == 1 ? "(<font color='#ffc853'>必填</font>)" : "(选填)"));
                      Response.Write(string.Format("<div class=\"provinceconcent\">"));
                      if (yearModel != null)
                      {
                          string[] kkk = yearModel.AnswerContent.Split(' ');
                          string province = kkk[0];
                          string city = kkk[1];
                          string district = kkk[2];
                          string text = kkk[3];
                          Response.Write(string.Format("<select class=\"province\" placeholder=\"省份\"><option>" + province + "</option></select>"));
                          Response.Write(string.Format("<select class=\"city\" placeholder=\"城市\"><option>" + city + "</option></select>"));
                          Response.Write(string.Format("<select class=\"district\" placeholder=\"地区\"><option>" + district + "</option></select>"));
                      }
                      else
                      {
                          Response.Write(string.Format("<select class=\"province\" placeholder=\"省份\"></select>"));
                          Response.Write(string.Format("<select class=\"city\" placeholder=\"城市\"></select>"));
                          Response.Write(string.Format("<select class=\"district\" placeholder=\"地区\"></select>"));
                      }
                      Response.Write(string.Format("</div>"));
                      if (yearModel != null)
                      {
                          string[] kkk = yearModel.AnswerContent.Split(' ');
                          string province = kkk[0];
                          string city = kkk[1];
                          string district = kkk[2];
                          string text = "";
                          if (province.Length > 0 && city.Length > 0 && district.Length > 0 && kkk[3].Length > 0)
                          {
                              text = kkk[3];
                          }
                          Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"text\" disabled=\"disabled\" value=" + text + " class=\"text\" placeholder=\"详细地址\" /></div>"));
                      }
                      else
                      {
                          Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"text\" class=\"text\" placeholder=\"详细地址\" /></div>"));
                      }
                      Response.Write("</div>");//结束
                      break;
                  case 5:
                      ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail dateModel = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} ", bll.GetCurrUserID(), QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));

                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\" data-type=\"yeardate\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\"  data-datetype=\"{4}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show"), QuestionList[i].AnswerGroupName));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下为{1}选择&nbsp;{0}</p>", (QuestionList[i].IsRequired == 1 ? "(<font color='#ffc853'>必填</font>)" : "(选填)"), dirYearDate[QuestionList[i].AnswerGroupName]));
                      if (QuestionList[i].AnswerGroupName == "datetime")
                      {
                          if (dateModel != null)
                          {
                              string[] dates = dateModel.AnswerContent.Split(' ');
                              string date = dates[0];
                              string time = dates[1];
                              Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"date\" disabled=\"disabled\" value=\"{0}\"  class=\"text\" style=\"width:50%;\" /><input type=\"time\" disabled=\"disabled\" class=\"text\" value=\"{1}\" style=\"width:50%;\" /></div>", !string.IsNullOrEmpty(dateModel.AnswerContent) ? date : "", !string.IsNullOrEmpty(dateModel.AnswerContent) ? time : ""));
                          }
                          else
                          {
                              Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"date\" class=\"text\" style=\"width:50%;\" /><input type=\"time\" class=\"text\" style=\"width:50%;\" /></div>", QuestionList[i].AnswerGroupName));
                          }

                      }
                      else
                      {
                          if (dateModel != null)
                          {
                              Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"{0}\" disabled=\"disabled\" class=\"text\" value=\"{1}\" /></div>", QuestionList[i].AnswerGroupName, dateModel.AnswerContent));
                          }
                          else
                          {
                              Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"{0}\" class=\"text\" /></div>", QuestionList[i].AnswerGroupName));
                          }
                      }
                      Response.Write("</div>");//结束
                      break;
                  case 6:
                      Response.Write(string.Format("<div class=\"question {3}\" data-type=\"show\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(QuestionList[i].QuestionName);
                      Response.Write("</div>");//结束
                      break;
              }
          } %>
        <% if (hasPage)
           {
        %>
        <table style="width: 100%; margin-top: 10px;">
            <tr>
                <td style="width: 40%; vertical-align: middle; text-align: center;"><a id="btnPrev" href="javascript:void(0)" class="submit" style="width: 100%; margin: 0px; display: none;">上一页</a></td>
                <td style="vertical-align: middle; text-align: center;"><span class="curPageInfo">1</span> / <span class="toalPageInfo"><%= Math.Ceiling(QuestionList.Count * 1.0 /QuestionnaireModel.EachPageNum) %> </span></td>
                <td style="width: 40%; vertical-align: middle; text-align: center;">
                    <a id="btnNext" href="javascript:void(0)" class="submit" style="width: 100%; margin: 0px;">下一页</a>
                    <a id="btnSumbit" href="javascript:void(0)" class="submit" style="width: 100%; margin: 0px; display: none;">提交</a>
                    <%--  <%
                                 if(isSubmit){
                                 %>
                                    <a id="btnSumbit1"  href="javascript:void(0)" class="submit1" style="width:100%; margin:0px;display:none;">提交</a>
                                <%
                                }else
                                {
                                     %>
                                    <a id="btnSumbit" href="javascript:void(0)" class="submit" style="width:100%; margin:0px;display:none;">提交</a>
                                    <%
                                }
                            %>--%>


                    <%-- if(isSubmit){
                         %>
                         <a  href="javascript:void(0)" class="submit1" style="margin-bottom: 20px; width: 90%;">提交</a>
                        <%
                    }
                    else
                    {
                         %>
                         <a id="btnSumbit" href="javascript:void(0)" class="submit" style="margin-bottom: 20px; width: 90%;">提交</a>
                        <%
                    }--%>


                </td>
            </tr>
        </table>
        <%  }
           else
           { 
                        
                    
        %>
        <a id="btnSumbit" href="javascript:void(0)" class="submit" style="margin-bottom: 20px; width: 90%;">提交</a>
        <%
              }%>




        <%
            if (!string.IsNullOrWhiteSpace(QuestionnaireModel.ButtonText) && !string.IsNullOrWhiteSpace(QuestionnaireModel.ButtonLink))
            {
                if (isSubmit)
                {
        %>
        <a href="<%=QuestionnaireModel.ButtonLink %>" class="submit" style="margin-bottom: 20px; width: 90%;"><%=QuestionnaireModel.ButtonText %></a>
        <%
            }
            else
            {
        %>
        <a href="<%=QuestionnaireModel.ButtonLink %>" disabled="true" id="alink" class="submit1" style="margin-bottom: 20px; width: 90%;"><%=QuestionnaireModel.ButtonText %>&nbsp;&nbsp;<span style="color: #9c9494; font-size: 12px;">(提交问卷后有效)</span></a>

        <%
            }

        }     
        %>





        <%
            if (spreadUser != null)
            {
                if (!string.IsNullOrEmpty(spreadUser.WXNickname) && !string.IsNullOrEmpty(spreadUser.WXHeadimgurl))
                {
        %>
        <div style="/* background-color: #5a8ece; */color: #2d2929; vertical-align: middle; width: auto; margin-bottom: 10px; text-shadow: none; width: 80%; margin: 0 auto; padding-bottom: 10px; border-bottom: 1px solid #cec7c7;">
            <table>
                <tbody>
                    <tr>
                        <td style="width: 55px;">
                            <img width="50" height="50" src="<%=spreadUser.WXHeadimgurl %>" style="width: auto; margin-top: 2px; margin-left: 2px; margin-bottom: 2px; vertical-align: middle; border-radius: 50px; width: 60px; height: 60px;"></td>
                        <td style="vertical-align: middle; padding-left: 12px;">来自&nbsp;<%=spreadUser.WXNickname %>&nbsp;的分享&nbsp; ☆☆☆☆☆</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <%
                }

            }
        %>
    </section>
    <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("wubuhui") || ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("xixinxian"))
      {%>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:history.go(-1)">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="/WubuHui/MyCenter/Index.aspx"><span class="iconfont icon-b11"></span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <!-- footerbar -->
    <%}%>
</body>
<script src="http://static-files.socialcrmyun.com/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="http://static-files.socialcrmyun.com/Scripts/json2.min.js" type="text/javascript"></script>
<script src="http://static-files.socialcrmyun.com/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script src="http://static-files.socialcrmyun.com/Scripts/wxshare/wxshare1.1.0/wxshare.js"></script>
<script src="http://static-files.socialcrmyun.com/Scripts/Common.js"></script>
<script src="http://static-files.socialcrmyun.com/Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">
    var oldselect = '';
    var title = "<%=QuestionnaireModel.QuestionnaireName%>";
    var desc = "<%=QuestionnaireModel.QuestionnaireSummary%>";
    var domain = "<%=Request.Url.Authority%>";
    var imgUrl = "<%=QuestionnaireModel.QuestionnaireImage%>";
    var hasSubmitUrl='<%=QuestionnaireModel.QuestionnaireSubmitUrl%>';
    var hasRepeatSubmitUrl='<%=QuestionnaireModel.QuestionnaireRepeatSubmitUrl%>';
    //var shareUrl = window.location.href;
    var curStartNum = 0;
    var eachPageNum = <%=QuestionnaireModel.EachPageNum%>;
    var questionNum = <%=QuestionList.Count%>;
    var provinces = [];
    var layerIndex;
    var uid='<%=uid%>';
    wx.ready(function () {
        var tempImgUrl  = imgUrl;
        ''.inon
        if(imgUrl.indexOf('http')<0){ 
            tempImgUrl = 'http://'+domain;
            if(imgUrl.indexOf('/')==0){
                tempImgUrl+=imgUrl;
            }
            else{
                tempImgUrl+='/'+imgUrl;
            }
        }

        var pageData = {
            currUserOpenId: '<%=currUserInfo.WXOpenId%>', //当前用户的wxopenId
            currUserId: '<%=currUserInfo.UserID%>', //当前用户的userId
            title: title, //标题
            summary: desc, //描述
            shareImgUrl: tempImgUrl, //分享缩略图
            shareUrl: window.location.href, //分享链接
            tempShareId: CreateGUID(),
            preShareId: GetParm('comeonshareid'),
            callback: callback
        };

        var shareCallBackFunc = {
            timeline_s: function () {
                submitShare('timeline_s');
                shareComeplete();
            },
            timeline_c: function () {
                //朋友圈分享取消
            },
            message_s: function () {
                //分享给朋友
                submitShare('message_s');
                shareComeplete();
            },
            message_c: function () {
                //朋友分享取消
            }
        }

        var processUrl = function (url) {
            url = DelUrlParam(url, 'comeonshareid');
            url = DelUrlParam(url, 'from');
            url = DelUrlParam(url, 'isappinstalled');
            return url;
        }

        var callback = function (data) { }

        var submitShare = function (WxMsgType) {
            var reqData = {
                Action: 'ShareSubmit',
                url: processUrl(pageData.shareUrl),
                shareId: pageData.tempShareId,
                preId: pageData.preShareId,
                userId: pageData.currUserId,
                userWxOpenId: pageData.currUserOpenId,
                wxMsgType: WxMsgType
            }

            //分享到朋友圈
            $.ajax({
                type: 'post',
                url: '/serv/pubapi.ashx',
                data: reqData,
                dataType: 'jsonp',
                success: function (data) {
                    pageData.tempShareId = CreateGUID();
                }
            });
        }

        //Url处理
        //移除原有参数 comeonshareid from isappinstalled
        pageData.shareUrl = processUrl(pageData.shareUrl);

        if (pageData.shareUrl.indexOf('?') >= -1) {
            pageData.shareUrl = pageData.shareUrl + '&comeonshareid=' + pageData.tempShareId;
        }else{
            pageData.shareUrl = pageData.shareUrl + '?comeonshareid=' + pageData.tempShareId;
        }

        wxapi.wxshare({
            title: pageData.title,
            desc: pageData.summary,
            link: pageData.shareUrl,
            imgUrl: pageData.shareImgUrl
        }, shareCallBackFunc)
    });
    $(function () {
        window.alert = window.Alert = function (msg) {
            layerIndex =layer.open({
                content: msg,
                time: 2
            });
        };
        window.progress = function () {
            layerIndex = layer.open({
                type: 2,
                shadeClose:false
            });
        };

        $("#alink").click(function(){
            var alink=$(this).attr("disabled");
            if(alink=='disabled'){
                return false;
            }
            return true;
        });
        <%
    if (!isSubmit)
    {
            %>
        initArea();
        GetStroage();
        <%
        }
       
        %>


        //-----1.问卷答到一半没提交的，下次进来继续答卷（注意有分页的形式也需要翻到离开的页面）;
        //-----存值
        
        $(".toupiaobox .radioinput").click(function(){saveStorage();});
        $(".toupiaobox .groupselect,.toupiaobox .province,.toupiaobox .city,.toupiaobox .district").bind('change',function(){saveStorage();});
        $(".toupiaobox .text").blur(function(){saveStorage();});
        //-------赋值
        
        //console.log(JSON.parse(window.localStorage.getItem("data")));
        //console.log(JSON.parse(window.localStorage.getItem("total")));
        $('.toupiaobox .province').bind('change',function(){
            var prekey = $(this).val();
            var nl = $.grep(provinces, function (cur, i) {
                return cur['id'] == prekey;
            });
            if(nl[0].citys){
                var thtml = new StringBuilder();
                thtml.AppendFormat('<option value=""></option>');
                for (var i = 0; i < nl[0].citys.length; i++) {
                    thtml.AppendFormat('<option value="{0}">{1}</option>',nl[0].citys[i].id,nl[0].citys[i].name);
                }
                $(this).closest(".toupiaobox").find('.city').html(thtml.ToString());
                $(this).closest(".toupiaobox").find('.district').val('');
            }
            else{
                var nobj = this;
                progress();
                $.ajax({
                    type: 'post',
                    url: '/serv/pubapi.ashx',
                    data: { action: "GetGetKeyVauleDatas", type: 'city',prekey: prekey},
                    dataType:'json',
                    success: function (resp) {
                        layer.close(layerIndex);
                        if (resp.list){
                            if( resp.list.length>0) {
                                nl[0].citys = resp.list;
                                var thtml = new StringBuilder();
                                thtml.AppendFormat('<option value=""></option>');
                                for (var i = 0; i < resp.list.length; i++) {
                                    thtml.AppendFormat('<option value="{0}">{1}</option>',resp.list[i].id,resp.list[i].name);
                                }
                                $(nobj).closest(".toupiaobox").find('.city').html(thtml.ToString());
                                $(nobj).closest(".toupiaobox").find('.district').val('');
                            }
                            else{
                                nl[0].citys = [];
                                $(nobj).closest(".toupiaobox").find('.city').html('');
                                $(nobj).closest(".toupiaobox").find('.district').val('');
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("获取城市数据失败");
                    }
                });
            }
        });
        $('.toupiaobox .city').bind('change',function(){
            var provincekey = $(this).closest(".toupiaobox").find('.province').val();
            var nl = $.grep(provinces, function (cur, i) {return cur['id'] == provincekey;});

            var prekey = $(this).val();
            var ncl = $.grep(nl[0].citys, function (cur, i) {return cur['id'] == prekey;});

            if(ncl[0].districts){
                var thtml = new StringBuilder();
                thtml.AppendFormat('<option value=""></option>');
                for (var i = 0; i < ncl[0].districts.length; i++) {
                    thtml.AppendFormat('<option value="{0}">{1}</option>',ncl[0].districts[i].id,ncl[0].districts[i].name);
                }
                $(this).closest(".toupiaobox").find('.district').html(thtml.ToString());
            }
            else{
                var nobj = this;
                progress();
                $.ajax({
                    type: 'post',
                    url: '/serv/pubapi.ashx',
                    data: { action: "GetGetKeyVauleDatas", type: 'district',prekey: prekey},
                    dataType:'json',
                    success: function (resp) {
                        layer.close(layerIndex);
                        if (resp.list) {
                            if(resp.list.length>0) {
                                ncl[0].districts = resp.list;
                                var thtml = new StringBuilder();
                                thtml.AppendFormat('<option value=""></option>');
                                for (var i = 0; i < resp.list.length; i++) {
                                    thtml.AppendFormat('<option value="{0}">{1}</option>',resp.list[i].id,resp.list[i].name);
                                }
                                $(nobj).closest(".toupiaobox").find('.district').html(thtml.ToString());
                            }
                            else{
                                ncl[0].districts = [];
                                $(nobj).closest(".toupiaobox").find('.district').html('');
                            }
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("获取地区数据失败");
                    }
                });
            }
        });
        $('.grouptype .grouptable .groupselecttd .groupselect').live('click',function(){
            oldselect = $.trim($(this).val());
        });
        $('.grouptype .grouptable .groupselecttd .groupselect').bind('change',function(){
            var nval = $.trim($(this).val());
            var oldselecttemp = $(this).closest('.grouptable').find('.groupselecttemp');
            $(oldselecttemp).find('option[value="'+oldselect+'"]').attr('data-use','0');
            $(oldselecttemp).find('option[value="'+nval+'"]').attr('data-use','1');
            //console.log(oldselecttemp);
            $(this).closest('.grouptable').find('.groupselect').each(function(){
                var nnval = $.trim($(this).val());
                var nmodel = $(oldselecttemp).clone();
                $(nmodel).find('option[value="'+nnval+'"]').attr('data-use','0');
                $(this).html('');
                $(this).append($(nmodel).find('option[data-use="0"]'));
                $(this).val(nnval);
                // console.log($(this));
            });
        });
        
        $("#btnNext").click(function () {
            //检查必填项
            if(!checkContent()) return;
            $('.question.show').addClass('hide').removeClass('show');
            curStartNum = curStartNum + eachPageNum;
            for (var i = curStartNum; i < curStartNum + eachPageNum; i++) {
                $("[data-questionindex='"+i+"']").addClass('show').removeClass('hide');
            }
            $(document.body).scrollTop($('.questionnaire').offset().top -100);
            $("#btnPrev").show();
            $(".curPageInfo").html(parseInt(curStartNum/eachPageNum+1));

            if(curStartNum + eachPageNum>= questionNum){
                $("#btnNext").hide();
                $("#btnSumbit").show();
                $("#btnSumbit1").show();
            }
            saveStorage();
        });
        $("#btnPrev").click(function () {
            $('.question.show').addClass('hide').removeClass('show');
            curStartNum = curStartNum - eachPageNum;
            for (var i = curStartNum; i < curStartNum + eachPageNum; i++) {
                $("[data-questionindex='"+i+"']").addClass('show').removeClass('hide');
            }
            $("#btnNext").show();
            $("#btnSumbit").hide();
            $("#btnSumbit1").hide();
            $(".curPageInfo").html(parseInt(curStartNum/eachPageNum+1));
            if(curStartNum == 0){
                $("#btnPrev").hide();
            }
            saveStorage();
        });

        $("#btnSumbit").click(function () {
            if($("#btnSumbit").html() == "提交成功") return;
            //检查必填项
            if(!checkContent()) return;
            var DataObj={
                Data:[]
            }
            DataObj.Data= GetSubData();
            var JsonData = JSON.stringify(DataObj);
            $("#btnPrev").hide();
            $("#btnSumbit").html("正在提交...");
            $.ajax({
                type: 'post',
                url: '/Handler/OpenGuestHandler.ashx',
                data: { Action: "SumbitQuestionnaire", JsonData: JsonData,spread_userid:uid },
                dataType:'json',
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("提交成功");
                        $("#btnSumbit").html("提交成功");

                        if(hasSubmitUrl){
                            window.location.href = hasSubmitUrl;//提交问卷跳转链接
                        }else{
                            window.location.href = window.location.href+'&xx='+new Date().getTime();
                              <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("wubuhui") || ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("xixinxian"))
                                {
                                      %>
                            if (confirm("提交成功，是否立即回到首页")) {
                                window.location="/WuBuhui/MyCenter/Index.aspx";
                            }
                                    <%
                                }
                               %>
                        }


                      


                    }
                    else {
                        if(hasRepeatSubmitUrl&&resp.Status==10013){
                            window.location.href=hasRepeatSubmitUrl;//重复提交
                        }else{
                            alert(resp.Msg);
                            $("#btnPrev").show();
                            $("#btnSumbit").html("提交");
                        }
                    }
                },
                timeout: 60000,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#btnPrev").show();
                    $("#btnSumbit").html("提交");
                    if (textStatus == "timeout") {
                        alert("提交超时，请重新提交");
                    }
                }
            });

        });
    });
    function initArea(){
        if($('.question .province').length>0){
            progress();
            $.ajax({
                type: 'post',
                url: '/serv/pubapi.ashx',
                data: { action: "GetGetKeyVauleDatas", type: 'province' },
                dataType:'json',
                success: function (resp) {
                    layer.close(layerIndex);
                    if (resp.list &&  resp.list.length>0) {
                        provinces = resp.list;
                        var thtml = new StringBuilder();
                        thtml.AppendFormat('<option value=""></option>');
                        for (var i = 0; i < resp.list.length; i++) {
                            thtml.AppendFormat('<option value="{0}">{1}</option>',resp.list[i].id,resp.list[i].name);
                        }
                        $('.question .province').html(thtml.ToString());
                        $('.question .province').each(function(){
                            var dataprovince = $.trim($(this).attr('data-province'));
                            var datacity = $.trim($(this).attr('data-city'));
                            var datadistrict = $.trim($(this).attr('data-district'));
                            if(dataprovince == "") return;
                            $(this).val(dataprovince);
                            getRowCitys($(this).closest(".toupiaobox").find('.city'),dataprovince, datacity, datadistrict)
                        });
                        //
                        //
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("获取省份数据失败");
                }
            });
        }
    }
    //检查当前页填写内容
    function checkContent(){
        var checkresult=true;
        var focusObj =null;
        $(".toupiaobox.show[data-required='1']").each(function () {
            if(!checkresult) return;
            var questiontype = $(this).attr("data-type");
            switch (questiontype) {
                case "singleselect":
                    if ($(this).find("input[type='radio']:checked").val() == undefined) {
                        //alert($(this).find(".title").first().text() + "必选");
                        if(focusObj == null) focusObj = $(this).find("input[type='radio']").first();
                        checkresult=false;
                        return;
                    }
                    break;
                case "muselect":
                    if ($(this).find("input[type='checkbox']:checked").val() == undefined) {
                        //alert($(this).find(".title").first().text() + "必选");
                        if(focusObj == null) focusObj = $(this).find("input[type='checkbox']").first();
                        checkresult=false;
                        return;
                    }
                    break;
                case "text":
                    if ($.trim($(this).find("input[type='text']").first().val()) == "") {
                        //alert($(this).find(".title").first().text() + "必填");
                        if(focusObj == null) focusObj = $(this).find("input[type='text']").first();
                        checkresult=false;
                        return;
                    }
                    break;
                case "groupselect":
                    $(this).find(".groupselect").each(function(){
                        if ($.trim($(this).val()) == "") {
                            if(focusObj == null) focusObj = $(this);
                            checkresult=false;
                            return;
                        }
                    });
                    break;
                case "provincecity":
                    if($.trim($(this).find(".province").val()) == "") {
                        if(focusObj == null) focusObj = $(this).find(".province");
                        checkresult=false;
                        return;
                    }
                    if($.trim($(this).find(".city").val()) == "" && $(this).find(".city option").length>0) {
                        if(focusObj == null) focusObj = $(this).find(".city");
                        checkresult=false;
                        return;
                    }
                    if($.trim($(this).find(".district").val()) == "" && $(this).find(".district option").length>0) {
                        if(focusObj == null) focusObj = $(this).find(".district");
                        checkresult=false;
                        return;
                    }
                    if($.trim($(this).find(".text").val()) == "") {
                        if(focusObj == null) focusObj = $(this).find(".text");
                        checkresult=false;
                        return;
                    }
                    break;
                case "yeardate":
                    if($(this).attr('data-datetype') == 'datetime'){
                        if($.trim($($(this).find(".text").get(0)).val()) == "") {
                            if(focusObj == null) focusObj = $(this).find(".text").get(0);
                            checkresult=false;
                            return;
                        }
                        if($.trim($($(this).find(".text").get(1)).val()) == "") {
                            if(focusObj == null) focusObj = $(this).find(".text").get(1);
                            checkresult=false;
                            return;
                        }
                    }
                    else{
                        if($.trim($(this).find(".text").first().val()) == "") {
                            if(focusObj == null) focusObj = $(this).find(".text").first();
                            checkresult=false;
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }
        })
        //检查必填项
        if (!checkresult) {
            $(focusObj).focus();
            alert("必填项不完整");
            return false;
        }
        return true;
    }

    function saveStorage(){
        var data= {start:curStartNum,data:GetSubData()};
        window.localStorage.setItem("data",JSON.stringify(data));
    }



    function GetSubData(){
        var Data=[];
        $(".toupiaobox").each(function () {
            var dataType = $(this).attr("data-type");
            //投票记录模型
            var record ={
                QuestionnaireID:<%=QuestionnaireModel.QuestionnaireID%>,
                QuestionID:parseInt($(this).attr("data-questionid")),
                AnswerID:null,
                AnswerContent:null
            }
            var nli = [];
            switch ($(this).attr("data-type")) {
                case "singleselect"://单选
                    if ($(this).find("input[type='radio']:checked").val()!= undefined) {
                        record.AnswerID=parseInt($(this).find("input[type='radio']:checked").val());
                        Data.push(record);
                    }
                    break;
                case "muselect"://多选
                    if ($(this).find("input[type='checkbox']:checked").val() != undefined) {
                        $(this).find("input[type='checkbox']:checked").each(function () {
                            var murecord =
                            {
                                QuestionnaireID:record.QuestionnaireID,
                                QuestionID:record.QuestionID,
                                AnswerID:parseInt($(this).val()),
                                AnswerContent:null
                            }
                            Data.push(murecord);
                        })
                    }
                    break;
                case "text"://填空
                    if ($.trim($(this).find("input[type='text']").first().val())!= "") {
                        record.AnswerContent=$.trim($(this).find("input[type='text']").first().val());
                        Data.push(record);
                    }
                    break;
                case "groupselect":
                    record.QuestionID = parseInt($(this).attr("data-questionid"));
                    $(this).find(".groupselect").each(function(){
                        var murecord =
                        {
                            QuestionnaireID:record.QuestionnaireID,
                            QuestionID:record.QuestionID,
                            AnswerID:null,
                            AnswerContent:null
                        }
                        var rAnswerID = $.trim($(this).val());
                        if (rAnswerID != "") {
                            murecord.AnswerID = rAnswerID;
                            murecord.AnswerContent = $.trim($(this).attr('data-group'));
                            Data.push(murecord);
                        }
                    });
                    break;
                case "provincecity":
                    var cityCodeLi = []
                    if($.trim($(this).find(".province").val()) != "") {
                        nli.push($.trim($(this).find(".province option:selected").text()));
                        cityCodeLi.push($.trim($(this).find(".province").val()));
                    }
                    if($.trim($(this).find(".city").val()) != ""){ 
                        nli.push($.trim($(this).find(".city option:selected").text()));
                        cityCodeLi.push($.trim($(this).find(".city").val()));
                    }
                    if($.trim($(this).find(".district").val()) != "") {
                        nli.push($.trim($(this).find(".district option:selected").text()));
                        cityCodeLi.push($.trim($(this).find(".district").val()));
                    }
                    if($.trim($(this).find(".text").val()) != "") nli.push($.trim($(this).find(".text").val()));
                    if(nli.length>0){
                        record.AnswerContent=nli.join(' ');
                        record.AnswerCityCode=cityCodeLi.join(' ');
                        Data.push(record);
                    }
                    break;
                case "yeardate":
                    if($(this).attr('data-datetype') == 'datetime'){
                        if($.trim($($(this).find(".text").get(0)).val()) != "") {
                            nli.push($($(this).find(".text").get(0)).val());
                        }
                        if($.trim($($(this).find(".text").get(1)).val()) != "") {
                            nli.push($($(this).find(".text").get(1)).val());
                        }
                        if(nli.length == 2){
                            record.AnswerContent=nli.join(' ');
                            Data.push(record);
                        }
                    }
                    else{
                        if ($.trim($(this).find(".text").first().val())!= "") {
                            record.AnswerContent=$.trim($(this).find(".text").first().val());
                            Data.push(record);
                        }
                    }
                    break;
                default:
                    break;
            }
        })
        return Data;
    }

    function GetStroage(){
        if(window.localStorage.getItem("data")!=null){
            var storage=JSON.parse(window.localStorage.getItem("data"));
            for (var i = 0; i < storage.data.length; i++) {
                var lidata = storage.data[i];
                var qdiv = $('.question[data-questionid="'+lidata.QuestionID+'"]');
                var type = $(qdiv).attr('data-type');
                switch (type) {
                    case 'singleselect':
                    case 'muselect':
                        $(qdiv).find('.radioinput[value="'+lidata.AnswerID+'"]').attr('checked',true);
                        break;
                    case 'text':
                        $(qdiv).find('.text').val(lidata.AnswerContent);
                        break;
                    case 'groupselect':
                        $(qdiv).find('.groupselect[data-group="'+lidata.AnswerContent+'"]').val(lidata.AnswerID);
                        break;
                    case 'provincecity':
                        var awn=lidata.AnswerContent.split(' ');
                        var province=lidata.AnswerCityCode.split(' ');
                        $(qdiv).find('.province').html('<option value="'+province[0]+'">'+awn[0]+'</option>')
                        $(qdiv).find('.province').val(province[0]);
                        $(qdiv).find('.province').attr('data-province',province[0]);
                        if(province.length>1) {
                            $(qdiv).find('.city').html('<option value="'+province[0]+'">'+awn[1]+'</option>')
                            $(qdiv).find('.city').val(province[1]);
                            $(qdiv).find('.province').attr('data-city',province[1]);
                        }
                        if(province.length>2) {
                            $(qdiv).find('.district').html('<option value="'+province[2]+'">'+awn[2]+'</option>')
                            $(qdiv).find('.district').val(province[2]);
                            $(qdiv).find('.province').attr('data-district',province[2]);
                        }

                        $(qdiv).find('.text').val(awn[province.length]);
                        break;
                    case 'yeardate':
                        var datatype = $(qdiv).attr('data-datetype');
                        if(datatype =="datetime"){
                            var dt = lidata.AnswerContent.split(' ');
                            qdiv.find('.mainconcent .text[type="date"]').val(dt[0]);
                            if(dt.length>0) qdiv.find('.mainconcent .text[type="time"]').val(dt[1]);
                        }
                        else{
                            qdiv.find('.mainconcent .text').val(lidata.AnswerContent);
                        }
                        break;
                }
            } 
            curStartNum = storage.start;
            $('.question.show').addClass('hide').removeClass('show');
            for (var i = curStartNum; i < curStartNum + eachPageNum; i++) {
                $("[data-questionindex='"+i+"']").addClass('show').removeClass('hide');
            }
            $(".curPageInfo").html(parseInt(curStartNum/eachPageNum+1));
            if(curStartNum == 0){
                $("#btnPrev").hide();
            }
            else{
                $("#btnPrev").show();
            }
            if(curStartNum + eachPageNum>= questionNum){
                $("#btnNext").hide();
                $("#btnSumbit").show();
                $("#btnSumbit1").show();
            }else{
                $("#btnNext").show();
                $("#btnSumbit").hide();
                $("#btnSumbit1").hide();
            }
            $(".curPageInfo").html(parseInt(curStartNum/eachPageNum+1));
        }
    }

    function  getRowCitys(ob,prekey,nval, cval){
        $.ajax({
            type: 'post',
            url: '/serv/pubapi.ashx',
            data: { action: "GetGetKeyVauleDatas", type: 'city',prekey: prekey},
            dataType:'json',
            success: function (resp) {
                if (resp.list){
                    if( resp.list.length>0) {
                        var thtml = new StringBuilder();
                        thtml.AppendFormat('<option value=""></option>');
                        for (var i = 0; i < resp.list.length; i++) {
                            thtml.AppendFormat('<option value="{0}">{1}</option>',resp.list[i].id,resp.list[i].name);
                        }
                        $(ob).html(thtml.ToString());
                        if($(nval)!=""){
                            $(ob).val(nval);
                            getRowDistricts($(ob).closest(".toupiaobox").find('.district'),nval,cval);
                        }
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
    
    function  getRowDistricts(ob,prekey,nval){
        $.ajax({
            type: 'post',
            url: '/serv/pubapi.ashx',
            data: { action: "GetGetKeyVauleDatas", type: 'district',prekey: prekey},
            dataType:'json',
            success: function (resp) {
                if (resp.list) {
                    if(resp.list.length>0) {
                        var thtml = new StringBuilder();
                        thtml.AppendFormat('<option value=""></option>');
                        for (var i = 0; i < resp.list.length; i++) {
                            thtml.AppendFormat('<option value="{0}">{1}</option>',resp.list[i].id,resp.list[i].name);
                        }
                        $(ob).html(thtml.ToString());
                        $(ob).val(nval);
                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
</script>

</html>
