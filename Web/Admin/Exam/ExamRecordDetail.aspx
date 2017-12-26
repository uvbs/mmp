<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ExamRecordDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Exam.ExamRecordDetail" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=QuestionnaireModel.QuestionnaireName %></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/Questionnaire/style.css?v=20160719" rel="stylesheet" type="text/css" />
    <link href="/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .box {
            
        }

            .box .header {
                background-color: #FFFAE9;
                color: black;
                font-weight: normal;
            }

        label {
            font-weight: normal;
        }

        textarea {
            border-radius: 5px;
        }

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

        .btn-save {
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
            background-color: #FF7D00;
        }

        .exammin {
            color: red;
        }

        #divtime {
            display: none;
        }

        .header {
            position: relative;
            top: 0;
            /*z-index: 1000000;
            margin-bottom: 150px;*/
            height: auto;
            line-height: normal;
        }

        #secQuestionList {
            margin-top:250px;
            margin-bottom: 100px;
        }

        .box .submit {
            background-color: #FF7D00;
        }

        .examname {
            text-align: center;
            font-size: 18px;
            font-weight: bold;
        }

        .notice {
            text-align: left;
            margin-left: 5px;
            font-size: 14px;
            padding-left: 5px;
            padding-right: 5px;
        }

        textarea {
            width: 100%;
            height: 300px;
        }

        .result {
            background-color: white;
            padding: 5px 5px 5px 5px;
        }

        .textarea-result {
            width: 100%;
            border-radius: 5px;
            height: 50px;
        }
        #divTop {

           position:fixed;
           top:0;
           
           z-index:1000000;

        }
    </style>
</head>
<body>



    <section class="box" id="divTop">
        <div class="header">
            <label class="examname"><%=QuestionnaireModel.QuestionnaireName %></label>


        </div>

        <div class="result">
            <div class="result-content">
                <h5>考试结果:</h5>
                <textarea class="textarea-result" placeholder="请输入考试结果" id="txtResult"><%=record.Result %></textarea>
                <label class="btn-save" id="btnSave">保存考试结果</label>
            </div>
        </div>



    </section>

    <section class="box questionnaire bottom50" id="secQuestionList">
        <%ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();%>
        <%List<ZentCloud.BLLJIMP.Model.Question> QuestionList = bll.GetList<ZentCloud.BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", QuestionnaireModel.QuestionnaireID), "Sort Asc");
          QuestionnaireModel.EachPageNum = 10;
          //if (QuestionnaireModel.EachPageNum <= 0)
          //{

          //    QuestionnaireModel.EachPageNum = QuestionList.Count;
          //}
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
                      //Response.Write(string.Format("<p class=\"note\">以下选项为单选&nbsp;</p>", QuestionList[i].IsRequired == 1 ? "(<font color='red'>必选</font>)" : "(选填)"));

                      for (int k = 0; k < AnswerList.Count; k++)
                      {
                          ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetali = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} AND AnswerID='{3}' ",currUserInfo.UserID, QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID, AnswerList[k].AnswerID));
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
                      // Response.Write(string.Format("<p class=\"note\">以下选项为多选&nbsp;</p>", QuestionList[i].IsRequired == 1 ? "(<font color='red'>必填</font>)" : "(选填)"));

                      for (int l = 0; l < AnswerList.Count; l++)
                      {
                          ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetali = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} AND AnswerID='{3}' ", currUserInfo.UserID, QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID, AnswerList[l].AnswerID));
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
                      ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail recordDetaliModel = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2}  ", currUserInfo.UserID, QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));

                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\"  data-type=\"text\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      //Response.Write(string.Format("<p class=\"note\">以下选项为填空&nbsp;</p>", QuestionList[i].IsRequired == 1 ? "(<font color='red'>必填)</font>" : "(选填)"));
                      Response.Write("<div class=\"mainconcent\">");

                      if (recordDetaliModel != null)
                      {
                          //Response.Write("<input type=\"text\" class=\"text\" disabled=\"disabled\" value=" + recordDetaliModel.AnswerContent + " />");
                          Response.Write("<textarea class=\"area\" disabled=\"disabled\">" + recordDetaliModel.AnswerContent + "</textarea>");
                      }
                      else
                      {
                          // Response.Write("<input type=\"text\" class=\"text\" />");

                          Response.Write("<textarea class=\"area\" ></textarea>");
                      }

                      Response.Write("</div>");
                      Response.Write("</div>");//结束
                      break;
                  case 3:

                      Response.Write(string.Format("<div class=\"question toupiaobox {3}\" data-type=\"groupselect\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      //Response.Write(string.Format("<p class=\"note\">以下选项为&nbsp;</p>", QuestionList[i].IsRequired == 1 ? "(<font color='red'>必填</font>)" : "(选填)"));
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
                          List<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail> recordDetaliList = bll.GetList<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} ", currUserInfo.UserID, QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));

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
                      ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail yearModel = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} ", currUserInfo.UserID, QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));
                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\" data-type=\"provincecity\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show")));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下为省市区地址填写&nbsp;</p>", QuestionList[i].IsRequired == 1 ? "(<font color='red'>必填</font>)" : "(选填)"));
                      Response.Write(string.Format("<div class=\"provinceconcent\">"));
                      if (yearModel != null)
                      {
                          string[] kkk = yearModel.AnswerContent.Split(' ');
                          string province = kkk[0];
                          string city = "";
                          string district = "";
                          string text = "";

                          if (kkk.Length >= 2)
                          {
                              city = kkk[1];
                          }
                          if (kkk.Length >= 3)
                          {
                              district = kkk[2];
                          }
                          if (kkk.Length >= 4)
                          {
                              text = kkk[3];
                          }
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
                          string city = "";
                          string district = "";
                          string text = "";
                          if (kkk.Length >= 2)
                          {
                              city = kkk[1];
                          }
                          if (kkk.Length >= 3)
                          {
                              district = kkk[2];
                          }

                          if (province.Length > 0 && city.Length > 0 && district.Length > 0 && kkk[3].Length > 0)
                          {
                              text = kkk[3];
                          }
                          Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"text\" disabled=\"disabled\" value=\"" + text + "\" class=\"text\" placeholder=\"详细地址\" /></div>"));
                      }
                      else
                      {
                          Response.Write(string.Format("<div class=\"mainconcent\"><input type=\"text\" class=\"text\" placeholder=\"详细地址\" /></div>"));
                      }
                      Response.Write("</div>");//结束
                      break;
                  case 5:
                      ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail dateModel = bll.Get<ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail>(string.Format(" UserID='{0}' AND QuestionnaireID={1} AND [QuestionID]={2} ", currUserInfo.UserID, QuestionnaireModel.QuestionnaireID, QuestionList[i].QuestionID));

                      Response.Write(string.Format("<div  class=\"question toupiaobox {3}\" data-type=\"yeardate\" data-required=\"{0}\" data-questionid=\"{1}\"  data-questionindex=\"{2}\"  data-datetype=\"{4}\">",
                          QuestionList[i].IsRequired, QuestionList[i].QuestionID, i, ((hasPage && i >= QuestionnaireModel.EachPageNum) ? "hide" : "show"), QuestionList[i].AnswerGroupName));//开始
                      Response.Write(string.Format("<div class=\"title\">{0}</div>", QuestionList[i].QuestionName));
                      Response.Write(string.Format("<p class=\"note\">以下为{1}选择&nbsp;</p>", (QuestionList[i].IsRequired == 1 ? "(<font color='red'>必填</font>)" : "(选填)"), dirYearDate[QuestionList[i].AnswerGroupName]));
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
           { %>

        <table style="width: 100%; margin-top: 10px;">
            <tr>
                <td style="width: 40%; vertical-align: middle; text-align: center;"><a id="btnPrev" href="javascript:void(0)" class="submit" style="width: 100%; margin: 0px; display: none;">上一页</a></td>
                <td style="vertical-align: middle; text-align: center;"><span class="curPageInfo">1</span> / <span class="toalPageInfo"><%= Math.Ceiling(QuestionList.Count * 1.0 /QuestionnaireModel.EachPageNum) %> </span></td>
                <td style="width: 40%; vertical-align: middle; text-align: center;">
                    <a id="btnNext" href="javascript:void(0)" class="submit" style="width: 100%; margin: 0px;">下一页</a>



                </td>
            </tr>
        </table>
        <%  }%>
    </section>


</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script type="text/javascript">

    var curStartNum = 0;
    var eachPageNum = <%=QuestionnaireModel.EachPageNum%>;
    var questionNum = <%=QuestionList.Count%>;

   
    $(function () {
        
        $("#btnNext").click(function () {
            //检查必填项
            //if(!checkContent()) return;
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
                //$("#btnSumbit").show();
                
            }
           
        });

        $("#btnPrev").click(function () {
            $('.question.show').addClass('hide').removeClass('show');
            curStartNum = curStartNum - eachPageNum;
            for (var i = curStartNum; i < curStartNum + eachPageNum; i++) {
                $("[data-questionindex='"+i+"']").addClass('show').removeClass('hide');
            }
            $("#btnNext").show();
            //$("#btnSumbit").hide();
            
            $(".curPageInfo").html(parseInt(curStartNum/eachPageNum+1));
            if(curStartNum == 0){
                $("#btnPrev").hide();
            }
          
        });

        $("#btnSave").click(function(){
            var result=$("#txtResult").val();
            if (result=="") {
                alert("请输入考试结果");
                return false;
            }
            $.ajax({
                type: "Post",
                url: "/Handler/App/CationHandler.ashx",
                data: { Action: "SaveExamResult", id:"<%=Request["id"]%>",userId: "<%=Request["uid"]%>",result:result},
                dataType:"json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("保存成功");
                       
                    } else {
                        alert(resp.Msg);
                    }


                }
            });


        
        
        });
       

    });
  

</script>


</html>

