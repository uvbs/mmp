<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/WapMain.Master"  CodeBehind="NewActivity.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.NewActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>

    <link href="/UMEditor/themes/default/_css/umeditor.css" rel="stylesheet" type="text/css" />
    <script src="/UMEditor/umeditor.config.js" type="text/javascript"></script>
    <script src="/UMEditor/editor_api.js" type="text/javascript"></script>
    <script src="/UMEditor/lang/zh-cn/zh-cn.js" type="text/javascript"></script>
    <link href="/Scripts/mobiscroll/mobiscroll-2.6.2/css/mobiscroll.custom-2.6.2.min.css"
        rel="stylesheet" type="text/css" />
    <script src="/Scripts/mobiscroll/mobiscroll-2.6.2/js/mobiscroll.custom-2.6.2.min.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/JuActivity/JuActivityHandler.ashx";
        var openId = '<%=currUser.WXOpenId %>';
        var currAcvityID = 0;
        var currAction = 'add';
        var ue;
        var fieldid = 4;
        $(function () {


            $('#txtStartDate').mobiscroll().datetime();

            var opt = {
                preset: 'datetime', //日期
                theme: 'jqm', //皮肤样式
                display: 'modal', //显示方式 
                mode: 'Scroller', //日期选择模式
                dateFormat: 'yy-MM-dd HH:mm', // 日期格式
                setText: '确定', //确认按钮名称
                cancelText: '取消', //取消按钮名籍我
                dateOrder: 'yymmdd', //面板中日期排列格式
                dayText: '日',
                monthText: '月',
                yearText: '年', //面板中年月日文字
                endYear: 2030 //结束年份

            };

            $('#txtStartDate').mobiscroll(opt);

            ue = UM.getEditor('myEditor', {
                toolbar: ['undo redo | bold italic underline forecolor backcolor insertorderedlist insertunorderedlist justifyleft justifycenter justifyright justifyjustify'],
                //focus时自动清空初始化时的内容
                autoClearinitialContent: true,
                //关闭字数统计
                wordCount: false,
                //关闭elementPath
                elementPathEnabled: false,
                //默认的编辑区域高度
                initialFrameHeight: 300,
                //是否保持toolbar的位置不动
                autoFloatEnabled: false
            });

            $('#btnSave').live('click', function () {
                try {
                    var model = {
                        IsSignUpJubit: 1,
                        ActivityName: $.trim($('#txtTitle').val()),
                        ActivityWebsite: "",
                        ActivityStartDate: $('#txtStartDate').val(),
                        ActivityDescription: ue.getContent(), //$('#txtContent').val(),
                        Action: currAction == 'add' ? 'AddJuActivity' : 'EditJuActivity',
                        JuActivityID: currAcvityID,
                        ThumbnailsPath: GetRandomHb(),
                        IsHide: 0,
                        IsByWebsiteContent: 0,
                        ArticleType: 'activity',
                        ArticleTemplate: 1,
                        IsSpread: 1,
                        FieldNameList: GetFieldNameList(),
                        IsHideRecommend: 0
                    };


                    if (model.ActivityName == '') {
                        $('#lbDlgMsg').html('请输入活动主题');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    if (model.ActivityStartDate == '') {
                        $('#lbDlgMsg').html('请输入活动时间');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    if (model.ActivityDescription == '') {
                        $('#lbDlgMsg').html('请输入活动详情');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    $.mobile.loading('show', { textVisible: true, text: '正在提交...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            try {
                                $.mobile.loading('hide');
                                var resp = $.parseJSON(result);
                                if (resp.Status == 1) {

                                    ClearLocalData();

                                    $('#lbDlgMsg').html("已成功创建活动!<br />正在转到分享页面。。。");
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');

                                    //JuActivityIDHex
                                    //                                    setInterval(function () {
                                    //                                       // window.location.href = '/' + resp.ExObj.JuActivityIDHex + '/share.chtml';



                                    //                                    }, 1500);
                                    window.location.href = '/' + resp.ExObj.JuActivityIDHex + '/share.chtml';


                                }
                                else {
                                    $('#lbDlgMsg').html(resp.Msg);
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');
                                }
                            } catch (e) {
                                alert(e);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }

            });

            GetLocalData();

            try {

                //                $(document).keypress(function () {
                //                    SetLocalData();
                //                }
                //                );

                //                $(document).on("scrollstart", function () {
                //                    SetLocalData();
                //                });

                //                $(document).on("tap", function () {
                //                    SetLocalData();
                //                });

                setInterval(function () { SetLocalData() }, 500);

            } catch (e) {
                alert(e);
            }

            $("#btnShowAddField").click(function () {

                if (fieldid >= 21) {
                    alert("最多新增17个字段");
                    return false;
                }
                $("#txtFieldName").val("");
                $("#txtFieldName").focus();
                $('#dlgNewField').popup();
                $('#dlgNewField').popup('open');




            });
            $("#btnAddField").click(function () {

                var fieldName = $.trim($("#txtFieldName").val());
                if (fieldName == "") {
                    $("#txtFieldName").val("");
                    $("#txtFieldName").focus();
                    return false;
                }
                if (CheckFieldNameIsRepeat(fieldName)) {
                    alert("重复的报名字段");
                    $("#txtFieldName").val("");
                    $("#txtFieldName").focus();
                    return false;
                }
                if (fieldName.indexOf(',') > -1) {
                    alert("名称不能包含逗号");
                    return false;
                }
                var str = new StringBuilder();
                str.AppendFormat("<input type=\"checkbox\" data-mini=\"true\" name=\"cbfield\" id=\"cb{0}\" checked=\"checked\" value=\"{1}\"  ><label for=\"cb{0}\">{1}</label>", fieldid, fieldName);
                $("#btnShowAddField").before(str.ToString());
                $("#ulfield").trigger("create");
                fieldid++;
                $('#dlgNewField').popup('close');




            });



        });


        function SetLocalData() {
            SetCookie('activityCnt', ue.getContent());
            SetCookie('activityTitle', $('#txtTitle').val());
            SetCookie('activityStartDate', $('#txtStartDate').val());
            //txtStartDate

        }

        function GetLocalData() {

            var articleCnt = getCookie('activityCnt');
            var articleTitle = getCookie('activityTitle');
            var activityStartDate = getCookie('activityStartDate');
            if (articleTitle != null)
                $('#txtTitle').val(articleTitle);
            if (articleCnt != null)
                ue.setContent(articleCnt);
            if (activityStartDate != null)
                $('#txtStartDate').val(activityStartDate);
        }

        function ClearLocalData() {
            //情空cookie数据
            SetCookie('articleCnt', '');
            SetCookie('articleTitle', '');
            SetCookie('activityStartDate', '');
        }


        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 21);
            return "/img/hb/hb" + randInt + ".jpg";
        }

        //获取添加的字段名称列表
        function GetFieldNameList() {
            var fieldnamelist = [];
            $('input[name="cbfield"]:checked').each(function () {
                var fieldname = $(this).val();
                fieldnamelist.push(fieldname);
            });
            return fieldnamelist.join(',');

        }
        //检查报名字段是否重复
        function CheckFieldNameIsRepeat(name) {
            var result = false;
            $('input[name="cbfield"]').each(function () {
                var fieldname = $(this).val();
                if (fieldname == name) {
                    result = true;
                }
            });
            return result;

        }
    </script>
     <style type="text/css">

  .th
  {
     white-space:nowrap; 
    font-size: 16px;
   line-height : 36px;
   color:Black;
   font: 12px/1.5 "microsoft yahei",arial,\5b8b\4f53;
   font-weight:bold;

   }
   
   .btnField
   {
font-size: 12px;
font: 12px/1.5 "microsoft yahei",arial,\5b8b\4f53;
font-weight: bold;
color: #2489ce /*{b-body-link-hover}*/;
background: #00b5e5;
-webkit-box-shadow: none;
-moz-box-shadow: none;
box-shadow: none;
-webkit-transition-property: background;
-moz-transition-property: background;
-o-transition-property: background;
transition-property: background;
-webkit-transition-duration: 0.3s;
-moz-transition-duration: 0.3s;
-o-transition-duration: 0.3s;
transition-duration: 0.3s;
color: white;
text-shadow: none;
border: none; 
-webkit-border-radius: 3px;
-moz-border-radius: 3px;
-ms-border-radius: 3px;
-o-border-radius: 3px;
border-radius: 3px; 
-webkit-box-shadow: inset 0px 1px 0px rgba(255, 255, 255, 0.5), 0px 1px 2px rgba(0, 0, 0, 0.15);
-moz-box-shadow: inset 0px 1px 0px rgba(255, 255, 255, 0.5), 0px 1px 2px rgba(0, 0, 0, 0.15);
box-shadow: inset 0px 1px 0px rgba(255, 255, 255, 0.5), 0px 1px 2px rgba(0, 0, 0, 0.15);
background-color: #eeeeee;
background: -webkit-gradient(linear, 50% 0%, 50% 100%, color-stop(0%, #fbfbfb), color-stop(100%, #e1e1e1));
background: -webkit-linear-gradient(top, #fbfbfb, #e1e1e1);
background: -moz-linear-gradient(top, #fbfbfb, #e1e1e1);
background: -o-linear-gradient(top, #fbfbfb, #e1e1e1);
background: linear-gradient(top, #fbfbfb, #e1e1e1);
display: -moz-inline-stack;
display: inline-block;
vertical-align: middle;
border: 1px solid #d4d4d4;
height: 30px;
line-height: 30px;
padding: 0px 25.6px;
color: #666666;
text-shadow: 0 1px 1px white;
margin: 0;
text-decoration: none;
text-align: center;
      


   }
   

  
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div data-role="page" id="page-title" data-theme="b" >

    <div style="margin-left:1%;margin-right:1%;">
    
   
        <div data-role="header" data-theme="f" data-tap-toggle="false" style="" id="divTop">
            <a href="/FShare/Wap/UserHub.aspx" data-icon="home" data-ajax="false">主页</a>
            <h1>
                组织活动
            </h1>
        </div>
        
   <label  class="th">
            活动主题:</label>
   
   
   
            
        <input type="text" id="txtTitle" value="" placeholder="请输入活动主题" />
        <label for="datetime-3" class="th">
            活动时间:</label>
         <%Response.Write(string.Format(@"<input type=""datetime-local"" data-clear-btn=""false"" id=""txtStartDate"" value=""{0}"" />",DateTime.Now.ToString())); %>
        <label for="txtContent" class="th">
            活动详情:</label>
        <%--<textarea cols="40" rows="8" style="height: 160px" id="txtContent" placeholder="请输入活动详情，请尽量生动、具体"></textarea>--%>
         <script type="text/plain" id="myEditor" style=" width:98%; height:200px; word-wrap: break-word;">
                <p>请输入文章内容</p>
            </script>     
<%--        <table width="100%">
            <tr>
                <td style=" width:1px"></td>
                <td style=" width:*">
                      
                </td>
                <td style=" width:1px"></td>
            </tr>
        </table>--%>

        <div  >
        <legend onclick="$('#ulfield').slideToggle();" class="th">报名字段设置∇</legend>
        <fieldset data-role="controlgroup">
        <div style="list-style:none;" id="ulfield">
      
         <input type="checkbox" id="cbnamefield" checked="checked" disabled="disabled" data-mini="true" /><label for="cbnamefield">姓名</label>
      
        <input type="checkbox" id="cbphonefield" checked="checked" disabled="disabled" data-mini="true" /><label for="cbphonefield">手机</label>

        <input type="checkbox" id="cbemailfield" data-mini="true" name="cbfield" value="Email" /><label for="cbemailfield">Email</label>

        <input type="checkbox" id="cbcompanynamefield" name="cbfield" data-mini="true" value="公司"   /><label for="cbcompanynamefield">公司</label>
        
        <input type="checkbox" id="cbpositionfield" data-mini="true" name="cbfield" value="职位"  /><label for="cbpositionfield">职位</label>
        
       
        <a href="javascript:;" id="btnShowAddField"  class="btnField">
                            新增字段</a>
        </div>
        </fieldset>
       </div>
        <a href="#" data-role="button" data-inline="false" data-theme="f" id="btnSave" data-ajax="false"
            data-mini="false">立即发布</a>
        
        <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <label id="lbDlgMsg">
            </label>
        </div>
        <div data-role="popup" id="dlgNewField" style="padding: 20px; text-align: center; ">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
           <input type="text" id="txtFieldName" placeholder="请输入字段名称"  /><input type="button" id="btnAddField" value="新增报名字段"/>
          
        </div>

         </div>
    </div>


</asp:Content>

