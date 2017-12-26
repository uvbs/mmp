<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="QuestionnaireStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Questionnaire.QuestionnaireStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            font-family: 微软雅黑;
            text-align:center;
         }
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 40px;
        }
        

        .title
        {
         font-size:12px;   
         }


        .question
        {
            border:1px solid;
            border-radius:5px;
            border-color:#CCCCCC;
            margin-top:20px;
            width:600px;
            margin: 0 auto;
            margin-bottom:20px;
         }
         table
         {
          width:100%;
          
          text-align:left;
             
         }
         th
         {
          width:80%;    
             
         }
      
         
    </style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">当前位置：&nbsp;<a href="/App/Questionnaire/QuestionnaireMgr.aspx?type=<%=type %>" title="返回问卷列表" >问卷</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;问卷统计&nbsp;<a style="float:right;margin-right:20px;" title="返回问卷管理" href="/App/Questionnaire/QuestionnaireMgr.aspx?type=<%=type %>" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
    
    
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        
       
        <%
            ZentCloud.BLLJIMP.BLL bll=new ZentCloud.BLLJIMP.BLL();
            ZentCloud.BLLJIMP.Model.Questionnaire QuestionnaireModel = bll.Get<ZentCloud.BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}",Request["id"]));
            Response.Write(string.Format("<div><h2>{0}</h2></div>", QuestionnaireModel.QuestionnaireName));
            Response.Write(" <hr style=\"border: 1px dotted #036\" />");
            Response.Write(" <br/>");
            Dictionary<string, string> dirYearDate = new Dictionary<string, string>();
            dirYearDate.Add("month", "年月");
            dirYearDate.Add("date", "年月日");
            dirYearDate.Add("time", "时间");
            dirYearDate.Add("datetime", "日期时间");
            List<ZentCloud.BLLJIMP.Model.Question> QuestionList = bll.GetList<ZentCloud.BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", QuestionnaireModel.QuestionnaireID), "Sort Asc");
            foreach (var Question in QuestionList.Where(p=>p.QuestionType!=6))
            {
                Response.Write("<div class=\"question\">");//问题开始
                string QuestionType = "";
                switch (Question.QuestionType)
                {
                    case 0:
                        QuestionType = "(单选)";
                        break;
                    case 1:
                        QuestionType = "(多选)";
                        break;
                    case 2:
                        QuestionType = "(填空)";
                        break;
                    case 3:
                        QuestionType = "(分组)";
                        break;
                    case 4:
                        QuestionType = "(省市区地址)";
                        break;
                    case 5:
                        QuestionType = "(" + dirYearDate[Question.AnswerGroupName] + ")";
                        break;
                    default:
                        break;
                }
                Response.Write(string.Format("<h3>{0}{1}</h3>", Question.QuestionName, QuestionType));
                Response.Write("<tr><td colspan=\"2\"><hr/></td></tr>");
                List<ZentCloud.BLLJIMP.Model.Answer> AnswerList = bll.GetList<ZentCloud.BLLJIMP.Model.Answer>(string.Format("QuestionID={0}", Question.QuestionID));
                int TotalRecordCount = bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0}", Question.QuestionID));
                List<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail> RecordList;
                switch (Question.QuestionType)
                {
                    case 0://单选
                      Response.Write("<table >");
                      Response.Write("<tr><th>选项</th><th>数量</th><th>比例</th></tr>");
                      Response.Write("<tr><td colspan=\"3\"><hr/></td></tr>");
                      foreach (var Answer in AnswerList)
                      {
                          int AnswerRocordCount = bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("AnswerID={0}", Answer.AnswerID));
                          double Percent = 0;
                          if (TotalRecordCount>0)
                          {
                            
                              Percent = Math.Round((Convert.ToDouble(AnswerRocordCount) / Convert.ToDouble(TotalRecordCount)) * 100, 0);

                          }
                          Response.Write(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}%</td></tr>", Answer.AnswerName, AnswerRocordCount, Percent));
                          Response.Write("<tr><td colspan=\"3\"><hr/></td></tr>");

                      }
                      Response.Write(string.Format("<tr><td colspan=\"3\"><h3>受访人数:{0}人</h3></td></tr>", bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0}",Question.QuestionID))));
                      Response.Write("</table>");
                      break;
                    case 1://多选
                      Response.Write("<table >");
                      Response.Write("<tr><th>选项</th><th>数量</th><th>比例<th></tr>");
                      Response.Write("<tr><td colspan=\"3\"><hr/></td></tr>");
                      foreach (var Answer in AnswerList)
                      {
                          int AnswerRocordCount = bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("AnswerID={0}", Answer.AnswerID));
                          double PercentM = 0;
                          if (TotalRecordCount> 0)
                          {

                              PercentM = Math.Round((Convert.ToDouble(AnswerRocordCount) / Convert.ToDouble(TotalRecordCount)) * 100, 0);

                          }
                          Response.Write(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}%</td></tr>", Answer.AnswerName, AnswerRocordCount, PercentM));
                          Response.Write("<tr><td colspan=\"3\"><hr/></td></tr>");
 
                      }
                      Response.Write(string.Format("<tr><td colspan=\"3\" ><h3>受访人数:{0}人</h3></td></tr>", bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>("UserId",string.Format("QuestionID={0}",Question.QuestionID))));
                      Response.Write("</table>");
                        break;
                    case 2://填空
                        RecordList = bll.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0}",Question.QuestionID));
                        Response.Write("<div style=\"text-align:left;\">");
                        foreach (var Record in RecordList)
                        {
                            Response.Write(Record.AnswerContent);
                            Response.Write("<hr/>");
                        }
                        Response.Write(string.Format("<h3>受访人数{0}人</h3>", bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>("UserId", string.Format("QuestionID={0}", Question.QuestionID))));
                        Response.Write("</div>");
                        break;
                    case 3://分组
                        List<string> gNameList = Question.AnswerGroupName.Split(',').ToList();
                        int colsNum = (gNameList.Count * 2) + 2;
                        Response.Write("<table >");
                        Response.Write("<tr><th style=\"width:50%;\">选项</th>");
                        foreach (var item in gNameList)
                        {
                            Response.Write(string.Format("<th style=\"width:auto;\">({0})数量</th>", item));
                            Response.Write(string.Format("<th style=\"width:auto;\">({0})比例</th>", item));
                        }
                        Response.Write("</tr>");
                        Response.Write(string.Format("<tr><td colspan=\"{0}\"><hr/></td></tr>", colsNum));
                        foreach (var Answer in AnswerList)
                        {
                            Response.Write(string.Format("<tr><td>{0}</td>", Answer.AnswerName));
                            foreach (var item in gNameList)
                            {
                                int AnswerRocordCount = bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("AnswerID={0} And AnswerContent='{1}'", Answer.AnswerID, item));
                                int tTotalRecordCount = bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0} And AnswerContent='{1}'", Question.QuestionID, item));
                                double Percent = 0;
                                if (tTotalRecordCount > 0){Percent = Math.Round((Convert.ToDouble(AnswerRocordCount) / Convert.ToDouble(tTotalRecordCount)) * 100, 0);}
                                Response.Write(string.Format("<td>{0}</td><td>{1}%</td>",AnswerRocordCount, Percent));
                            }
                            Response.Write("</tr>");
                        }
                        Response.Write(string.Format("<tr><td colspan=\"{0}\"><hr/></td></tr>", colsNum));
                        Response.Write(string.Format("<tr><td colspan=\"{1}\"><h3>受访人数:{0}人</h3></td></tr>", 
                            bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>("UserId", string.Format("QuestionID={0}", Question.QuestionID)),
                            colsNum));
                        Response.Write("</table>");
                        break;
                    case 4://填空
                        RecordList = bll.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0}", Question.QuestionID));
                        Response.Write("<div style=\"text-align:left;\">");
                        foreach (var Record in RecordList)
                        {
                            Response.Write(Record.AnswerContent);
                            Response.Write("<hr/>");
                        }
                        Response.Write(string.Format("<h3>受访人数{0}人</h3>", bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>("UserId", string.Format("QuestionID={0}", Question.QuestionID))));
                        Response.Write("</div>");
                        break;
                    case 5://填空
                        RecordList = bll.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0}", Question.QuestionID));
                        Response.Write("<div style=\"text-align:left;\">");
                        foreach (var Record in RecordList)
                        {
                            Response.Write(Record.AnswerContent);
                            Response.Write("<hr/>");
                        }
                        Response.Write(string.Format("<h3>受访人数{0}人</h3>", bll.GetCount<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>("UserId", string.Format("QuestionID={0}", Question.QuestionID))));
                        Response.Write("</div>");
                        break;
                    default:
                        break;
                }

                Response.Write("</div>");//问题结束

                
            }
            
            
            
            
       %>




    </div>
</asp:Content>
