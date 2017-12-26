<%@ Page Title="" Language="C#" MasterPageFile="~/customize/forbes/question/Master.Master"
    AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.question.Question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="css/question.css" rel="stylesheet" type="text/css" />
    <style>
        #divPreQuestion
        {
            font-size: 14px;
            text-align: left;
            margin-left: 5px;
            margin-right: 5px;
            line-height: 20px;
        }
        .wrapForbesQuestion .baseKnowledge
        {
            text-align: left;
            padding: 30px 30px 20px;
        }
        .wrapForbesQuestion
        {
            height: 100%;
            background-image: url(images/bg1_02.png);
            text-align: center;
            min-height: 800px;
        }
        .chooseRegion{font-weight:bold;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapForbesQuestion">
        <div id="divPreQuestion">
        </div>
        <div class="baseKnowledge">
            <h2 id="txtcategory">
            </h2>
            <p class="pTop8 colorC48826" id="txtknowledge_lv1">
            </p>
            <p class="pTop8 colorC48826" id="txtknowledge_lv2">
            </p>
        </div>
        <div class="queTitle" id="txtquestion">
        </div>
        <div class="chooseRegion">
            <div class="row">
                <div class="col" id="answera" data-answer="A">
                </div>
                <div class="col" id="answerb" data-answer="B">
                </div>
            </div>
            <div class="row">
                <div class="col" id="answerc" data-answer="C">
                </div>
                <div class="col" id="answerd" data-answer="D">
                </div>
            </div>
        </div>
        <div class="chooseAnsBtn">
            <button id="btnAnswer" class="button button-block button-positive">
                就选它了
            </button>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script>
        var handlerUrl = "Handler.ashx";
        var myQuestionId = 0; //我的问题编号
        var answer = ""; //选择的答案
        var count =<%=Request["count"]%>;//第一道题
        var isFinish = false;
        $(function () {

            $("[data-answer]").click(function () {

                $("[data-answer]").removeClass("selected");
                $(this).addClass("selected");
                answer = $(this).data("answer");
            })

            //先答案
            $("#btnAnswer").click(function () {

                if (isFinish) {
                    //layermsg("答题已经完成<br/>请点击查看成绩");
                    window.location.href = 'Result.aspx?count=' + count;
                    return false;
                }
                if (!$("[data-answer]").hasClass("selected")) {
                    layermsg("请选择一个答案");
                    return false;
                };


                //提交答案
                SumbitAnswer();



            })

            //获取我的问题
            GetMyQuestion();





        })


        //获取我的题目
        function GetMyQuestion() {
        $(divPreQuestion).html("");
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { action: "GetMyQuestion", count: count },
                success: function (resp) {
                    if (resp.errcode == 0) {
                        if (resp.is_finish == 1) {
                            isFinish = true;
                            GetMyLastQuestion();
                            $(btnAnswer).html("您已答题完成<br/>查看上次答题成绩");
                            return false;
                        }
                        SetQuestion(resp);

                    }


                }
            });

        }


        //设置题目内容
        function SetQuestion(obj) {

            if (obj.is_finish==0) {//未完成题目
                $(txtcategory).html(obj.category_name);
                $(txtknowledge_lv1).html("一级知识点：" + obj.knowledge_lv1);
                $(txtknowledge_lv2).html("二级知识点：" + obj.knowledge_lv2);
                $(txtquestion).html(obj.question);
                $(answera).html(obj.answer_a);
                $(answerb).html(obj.answer_b);
                $(answerc).html(obj.answer_c);
                $(answerd).html(obj.answer_d);
                myQuestionId = obj.myquestion_id;

                if (obj.pre_question!=""&&obj.pre_question!=null) {

                
    $(divPreQuestion).html("<br/>上一题:<br/>"+obj.pre_question+"<br/>"+obj.pre_answer_a+"<br/>"+obj.pre_answer_b+"<br/>"+obj.pre_answer_c+"<br/>"+obj.pre_answer_d+"<br/>正确答案:"+obj.pre_correctanswer);
}


            }
            else {
               //已完成题目
            }




        }

        //提示消息
        function layermsg(msg) {
            layer.open({
                content: msg,
                btn: ['OK']
            });
        }

        //提交答案
        function SumbitAnswer() {

            if (isFinish) {
                layermsg("答题已经完成<br/>请点击查看成绩");
                return false;
            }
            $(btnAnswer).html("正在提交...");
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { action: "SumbitAnswer", myquestionid: myQuestionId, answer: answer },
                success: function (resp) {
                    if (resp.errcode == 0) {
                        //提交答案成功
                        answer = ""; //答案初始
                        $("[data-answer]").removeClass("selected");

                        if (resp.is_finish == 1) {
                            //layermsg("恭喜答案完成");
                            return false;
                        }

                        //获取下一首题目
                        GetMyQuestion();

                    }
                    else {
                        layermsg(resp.errmsg);
                    }



                },
                complete: function () {

                    $(btnAnswer).html("就选它了");

                }
            });



        }

        //获取最后一道题的正确答案
        function GetMyLastQuestion() {
        $(divPreQuestion).html("");
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { action: "GetMyLastQuestion", count: 1 },
                success: function (resp) {
                    if (resp.errcode == 0) {
                        $(divPreQuestion).html("<br/>正确答案:"+resp.correctanswer);
}
                       

                    }


                
            });

        }
    </script>
</asp:Content>
