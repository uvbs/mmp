<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="QuestionnaireRecordDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Questionnaire.QuestionnaireRecordDetail" %>
<!DOCTYPE html>
<html>
<head>
<title></title>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
<link href="/css/Questionnaire/style.css" rel="stylesheet" type="text/css" />
<script>
</script>
</head>
<body>
    <section class="box">
        <%
         ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();
         ZentCloud.BLLJIMP.Model.Questionnaire Questionnaire = bll.Get<ZentCloud.BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}", Request["id"]));
         List<ZentCloud.BLLJIMP.Model.Question> QuestionList = bll.GetList<ZentCloud.BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", Questionnaire.QuestionnaireID), "Sort Asc");
         Response.Write(string.Format("<div><a href=\"QuestionnaireRecord.aspx?id={0}&type=" + Request["type"] + "\" style=\"float:right;\">返回</a></div>", Questionnaire.QuestionnaireID));
         Response.Write(string.Format("<h1 style=\"text-align:center;\">{0}</h1>", Questionnaire.QuestionnaireName));
         ZentCloud.BLLJIMP.Model.QuestionnaireRecord rec = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecord>(string.Format("UserId='{0}' And QuestionnaireID={1}", Request["uid"], Request["id"]));
         //Response.Write(string.Format("<h5 style=\"text-align:right;\">用户:{0}</h5>", rec.UserId.StartsWith("AnonymousUser") ? "匿名用户" : rec.UserId));
         Response.Write(string.Format("<h5 style=\"text-align:right;\">{0}</h5>", rec.InsertDate.ToString("yyyy-MM-dd HH:mm")));
         Response.Write("<div style=\"margin-bottom:100px;\">");
         Dictionary<string, string> dirYearDate = new Dictionary<string, string>();
         dirYearDate.Add("month", "年月");
         dirYearDate.Add("date", "年月日");
         dirYearDate.Add("time", "时间");
         dirYearDate.Add("datetime", "日期时间");
         foreach (var Question in QuestionList.Where(p => p.QuestionType != 6))
         {
             Response.Write("<div  class=\"toupiaobox\">");
             Response.Write(string.Format("<div class=\"title\">{0}</div>",Question.QuestionName));
             Response.Write("<p class=\"note\">");
             switch (Question.QuestionType)
	        {
                 case 0:
                     Response.Write("单选");
                     break;
                 case 1:
                     Response.Write("多选");
                     break;
                 case 2:
                     Response.Write("填空");
                     break;
                 case 3:
                     Response.Write("分组");
                     break;
                 case 4:
                     Response.Write("(省市区地址)");
                     break;
                 case 5:
                     Response.Write("(" + dirYearDate[Question.AnswerGroupName] + ")");
                     break;
		       
	        }
             Response.Write("</p>");

             List<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail> RecordList = bll.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format("QuestionID={0} And UserID='{1}'", Question.QuestionID,Request["uid"]));
             switch (Question.QuestionType)
             {
                 case 0:
                    if (RecordList.Count>0)
	                {
		              Response.Write("<div class=\"mainconcent\">");
                      Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>", bll.Get<ZentCloud.BLLJIMP.Model.Answer>(string.Format("AnswerID={0}", RecordList[0].AnswerID)).AnswerName));
                      Response.Write("</div>");
	                }

                     break;
                 case 1:
                     if (RecordList.Count > 0)
                     {
                         foreach (var Record in RecordList)
                         {
                             Response.Write("<div class=\"mainconcent\">");
                             Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>", bll.Get<ZentCloud.BLLJIMP.Model.Answer>(string.Format("AnswerID={0}", Record.AnswerID)).AnswerName));
                             Response.Write("</div>"); 
                         }

                     }
                     break;
                 case 2:
                     if (RecordList.Count > 0)
                     {
                         Response.Write("<div class=\"mainconcent\">");
                         Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>",RecordList[0].AnswerContent));
                         Response.Write("</div>");
                     }
                     break;
                 case 3:
                     if (RecordList.Count > 0)
                     {
                         foreach (var Record in RecordList)
                         {
                             Response.Write("<div class=\"mainconcent\">");
                             Response.Write(string.Format("<div class=\"inputtext\" >（{1}）{0}</div>", bll.Get<ZentCloud.BLLJIMP.Model.Answer>(string.Format("AnswerID={0}", Record.AnswerID)).AnswerName, Record.AnswerContent));
                             Response.Write("</div>");
                         }
                     }
                     break;
                 case 4:
                     if (RecordList.Count > 0)
                     {
                         Response.Write("<div class=\"mainconcent\">");
                         Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>", RecordList[0].AnswerContent));
                         Response.Write("</div>");
                     }
                     break;
                 case 5:
                     if (RecordList.Count > 0)
                     {
                         Response.Write("<div class=\"mainconcent\">");
                         Response.Write(string.Format("<div class=\"inputtext\" >{0}</div>", RecordList[0].AnswerContent));
                         Response.Write("</div>");
                     }
                     break;
             }
            
           Response.Write("</div>");
             
         }

         Response.Write("</div>");
       %>
    </section>
</body>
<script>
    // $("body").text(navigator.userAgent)

    
</script>
</html>

