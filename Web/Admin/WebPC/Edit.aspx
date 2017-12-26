<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.WebPC.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            font-family: 微软雅黑;
            background-color: white !important;
        }

        .tdTitle {
            font-weight: bold;
        }



        .title {
            font-size: 12px;
        }

        input[type=text], select {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        .question {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 98%;
            position: relative;
        }

        .fieldsort {
            float: left;
            margin-left: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .deletequestion {
            float: right;
            right: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .question input[type=text] {
            width: 90%;
        }

        #fileLogo {
            display: none;
        }
        .centent_r_btm {
            height:auto !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;&nbsp;&nbsp;页面>&nbsp;&gt;&nbsp;&nbsp;<span>编辑<%=model.PageName %></span> <a title="返回管理" style="float: right; margin-right: 20px;" href="PageList.aspx" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%;">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">页面名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPageName" maxlength="100" value="<%=model.PageName %>" style="width: 100%;" placeholder="页面名称(必填)" />
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">顶部内容：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtTopContent" style="width: 100%; height: 30px;"><%=model.TopContent %></textarea>
                    </td>
                </tr>
                                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">Logo：
                    </td>
                    <td width="*" align="left">
                        <img alt="Logo" src="<%=model.Logo %>" width="80px" id="imgLogo" />
                        <br />
                        <a id="auploadThumbnails"
                            href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileLogo.click()">上传</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG,PNG 格式图片
                        <input type="file" id="fileLogo" name="file1" />
                    </td>
                </tr>
                                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">顶部菜单导航：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlTopMenu">
                            <option value="">无</option>
                            <%foreach (var item in MenuList)
                              {%>
                                <option value="<%=item%>"><%=item%></option>
                              <%} %>
                        </select>
                    </td>
                </tr>
                 <tr>
                     <td style="width: 100px;" align="right" class="tdTitle">底部内容：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtBottomContent" style="width: 100%; height: 30px;"><%=model.BottomContent %></textarea>
                    </td>
                </tr>



            </table>



            <strong style="font-size: 20px;">中部内容:</strong>






            <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddItem">添加内容</a>

            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 10px;">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary" style="width: 200px;">保存</a>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/WebPC/Update.ashx";
        var editor;//顶部内容
        var editorBottom;//底部内容
        var editors = {};
        var slideListJson=<%=slideListJson%>;//幻灯片列表
        var middList=<%=model.MiddContent%>;//中部内容
        var itemCount =middList.length; //数量
        var editorsEditIndex=[];
        var slideTypeList=[{name:"默认",value:""},{name:"3D方块",value:"cube"},{name:"3D覆盖流",value:"coverflow"},{name:"分组显示",value:"perview"}];
        $(function () {

           
            //上移
            $(".upfield").live("click", function () {
                if ($(this).closest("div").prev(".question").length > 0) {
                    var type =$(this).closest("div").find("input[type='radio']:checked").val();
                    if (type != "html") {
                        $(this).closest("div").prev(".question").before($(this).closest("div").clone());
                        $(this).closest("div").remove();
                    }
                    else {
                        var indexNum = $(this).closest("div").attr('data-item-index');
                        var nhtml = editors['editor' + indexNum].html();
                        KindEditor.remove('#txtEditor' + indexNum);
                        editors['editor' + indexNum] = null;
                        $(this).closest("div").prev(".question").before($(this).closest("div").clone());
                        $(this).closest("div").remove();
                        KindEditor.ready(function (K) {
                            $('#txtEditor' + indexNum).html(nhtml);
                            editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                                items: [
                                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                                filterMode: false,
                                width: '93%'
                            });
                        });
                    }
                }
            });

            //下移
            $(".downfield").live("click", function () {
                if ($(this).closest("div").next(".question").length > 0) {
                    var type = $(this).closest("div").find("input[type='radio']:checked").val();
                    if (type != "html") {
                        $(this).closest("div").next(".question").after($(this).closest("div").clone());
                        $(this).closest("div").remove();
                    }
                    else {
                        var indexNum = $(this).closest("div").attr('data-item-index');
                        var nhtml = editors['editor' + indexNum].html();
                        KindEditor.remove('#txtEditor' + indexNum);
                        editors['editor' + indexNum] = null;
                        $(this).closest("div").next(".question").after($(this).closest("div").clone());
                        $(this).closest("div").remove();
                        KindEditor.ready(function (K) {
                            $('#txtEditor' + indexNum).html(nhtml);
                            editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                                items: [
                                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                                filterMode: false,
                                width: '93%'
                            });
                        });
                    }
                }
            });
            //删除


            //删除
            $('.deletequestion').live("click", function () {
                //if ($('.deletequestion').length <= 1) {
                //    Alert("至少添加一个题目");
                //    return false;
                //}
                if (confirm("确定要删除?")) {
                    var indexNum = $(this).closest("div").attr('data-item-index');
                    if (editors['editor' + indexNum]) {
                        KindEditor.remove('#txtEditor' + indexNum);
                        editors['editor' + indexNum] = null;
                    }
                    $(this).parent().remove();
                }



            });
            //删除

            //添加
            $("#btnAddItem").click(function () {
                itemCount++;
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat("<div class=\"question\" data-item-index=\"{0}\" data-item-id=\"0\">", itemCount);
                appendhtml.AppendFormat("<img src=\"/img/icons/up.png\" class=\"upfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/icons/down.png\" class=\"downfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/delete.png\" class=\"deletequestion\"/>");
                appendhtml.AppendFormat("<table style=\"width:100%;margin-left:10px;\">");

                appendhtml.AppendFormat("<tr><td style=\"width:100px;\">类型:</td><td>", itemCount);

                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"slide\" id=\"rd{0}slide\" checked=\"checked\"/><label for=\"rd{0}slide\">幻灯片</label>", itemCount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"html\" id=\"rd{0}html\"/><label for=\"rd{0}html\">自定义内容</label>", itemCount);

                appendhtml.AppendFormat("</td></tr>", itemCount);

                appendhtml.AppendFormat("<tr class=\"trslide\" ><td>选择幻灯片:</td><td><select class=\"ddlSlide\">");
                    
                    
                for (var i = 0; i < slideListJson.length; i++) {
                    appendhtml.AppendFormat('<option value="{0}">{0}</option>',slideListJson[i]);
                }
                    

                appendhtml.AppendFormat("</select>幻灯片效果: <select class=\"ddlSlideType\">")
                    
                for (var l = 0; l < slideTypeList.length; l++) {
                    
                        appendhtml.AppendFormat('<option value="{0}">{1}</option>',slideTypeList[l].value,slideTypeList[l].name);
                    
                   
                }
                appendhtml.AppendFormat("</select></td></tr>");




                appendhtml.AppendFormat("<tr class=\"trKindeditor\" style=\"display:none;\"><td>内容:</td><td><div id=\"txtEditor{0}\" style=\"height: 200px;\"></div></td></tr>", itemCount);
                appendhtml.AppendFormat("</table>");
                appendhtml.AppendFormat("</div>");
                $(this).before(appendhtml.ToString());
                
            });
            //添加


            //上传Logo
            $("#fileLogo").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Questionnaire',
                         secureuri: false,
                         fileElementId: 'fileLogo',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgLogo').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    Alert(e);
                }
            });

            //类型筛选
            $("input[type='radio'][name^='rdtype']").live("click", function () {

                $(this).closest(".question").find(".trslide").hide();
                $(this).closest(".question").find(".trKindeditor").hide();

                if ($.inArray($(this).val(), ['slide', 'html']) >= 0) {

                    if ($(this).val() == "slide") {//幻灯片类型
                        $(this).closest(".question").find(".trslide").show();

                    }
                    else if ($(this).val() == "html") {//自定义内容类型
                      
                        var indexNum = $(this).closest("div").attr('data-item-index');
                        if (!editors['editor' + indexNum]) {
                            KindEditor.ready(function (K) {
                                editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                                    uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                                    items: [
                                        'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                        'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                                    filterMode: false,
                                    width: '93%'
                                });
                            });
                        }
                        $(this).closest(".question").find(".trKindeditor").show();

                    }
                }

            });

            //
            $("#ddlTopMenu").val("<%=model.TopMenu%>");
            //

            //保存
            $('#btnSave').click(function () {

               

                    try {

                        if ($.trim($("#txtPageName").val()) == "") {
                            $("#txtPageName").focus();
                            return false;
                        }
                        var model = GetData();
                        var jsonData = JSON.stringify(model);
                        $.messager.progress({ text: '正在更新...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { jsonData: jsonData },
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.status ==true) {
                                    alert("编辑成功");
                                    window.location.href = "PageList.aspx";
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }
                        });

                    } catch (e) {
                        Alert(e);
                    }

                



            });

            //加载中部内容
            loadMidd();
            //加载中部内容

        });



        //顶部内容
        KindEditor.ready(function (K) {
            editor = K.create('#txtTopContent', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist'],
                filterMode: false
            });
        });

        //顶部内容
        KindEditor.ready(function (K) {
            editorBottom = K.create('#txtBottomContent', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist'],
                filterMode: false
            });
        });

        //获取模型
        function GetData() {
            //模型
            var dataModel = {
                PageId:<%=model.PageId%>,
                PageName: $("#txtPageName").val(),
                Logo:$("#imgLogo").attr("src"),
                TopContent: editor.html(),
                BottomContent: editorBottom.html(),
                TopMenu:$(ddlTopMenu).val(),
                MiddList: []
            }
            
            //中间内容
            $(".question").each(function () {
               
                var middModel = {
                    Type: '',//类型 幻灯片,html
                    Content:'',// html内容
                    SlideName:'',//幻灯片名称
                    SlideType:''//幻灯片效果
                   
                }; 
                middModel.Type = $(this).find("input[type='radio']:checked").val();
                if (middModel.Type == "slide") {//幻灯片模型
                    middModel.SlideName = $(this).find(".ddlSlide").first().val();
                    middModel.SlideType = $(this).find(".ddlSlideType").first().val();
                    middModel.Type = "slide";
                }
                else if (middModel.Type == "html") {//自定义内容
                    var indexNum = $(this).closest("div").attr('data-item-index');
                    middModel.Content = editors['editor' + indexNum].html();
                    middModel.Type = "html";
                }
                dataModel.MiddList.push(middModel);
            });
            //中间内容
           
            return dataModel;
        }

        //加载中部内容
        function loadMidd(){
           
            
            var appendhtml = new StringBuilder();
            for (var i = 0; i <middList.length; i++) {
                
                appendhtml.AppendFormat("<div class=\"question\" data-item-index=\"{0}\" data-item-id=\"0\">", i);
                appendhtml.AppendFormat("<img src=\"/img/icons/up.png\" class=\"upfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/icons/down.png\" class=\"downfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/delete.png\" class=\"deletequestion\"/>");
                appendhtml.AppendFormat("<table style=\"width:100%;margin-left:10px;\">");
                appendhtml.AppendFormat("<tr><td style=\"width:100px;\">类型:</td><td>", i);

                var trSlideDisplay="none";
                var trHtmlDisplay="none";

                switch (middList[i].Type) {
                    case "slide":
                        appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"slide\" id=\"rd{0}slide\" checked=\"checked\"/><label for=\"rd{0}slide\">幻灯片</label>", i);
                        appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"html\" id=\"rd{0}html\"/><label for=\"rd{0}html\">自定义内容</label>", i);
                        trSlideDisplay="inline-grid";
                        break;
                    case "html":
                        appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"slide\" id=\"rd{0}slide\" /><label for=\"rd{0}slide\">幻灯片</label>", i);
                        appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"html\" id=\"rd{0}html\" checked=\"checked\"/><label for=\"rd{0}html\">自定义内容</label>", i);
                        trHtmlDisplay="inline-grid";
                        editorsEditIndex.push(i.toString());
                    default:
        
                }


                appendhtml.AppendFormat("</td></tr>", i);

                

                appendhtml.AppendFormat("<tr class=\"trslide\" style=\"display:{0}\" ><td>选择幻灯片:</td><td><select class=\"ddlSlide\">",trSlideDisplay);
                    
                    
                for (var k = 0; k < slideListJson.length; k++) {
                    if (middList[i].SlideName==slideListJson[k]) {
                        appendhtml.AppendFormat('<option value="{0}" selected=\"selected\">{0}</option>',slideListJson[k]);
                    }
                    else {
                        appendhtml.AppendFormat('<option value="{0}">{0}</option>',slideListJson[k]);
                    }
                   
                }
                    
                appendhtml.AppendFormat("</select>幻灯片效果: <select class=\"ddlSlideType\">")
                    
                for (var l = 0; l < slideTypeList.length; l++) {
                    if (middList[i].SlideType==slideTypeList[l].value) {
                        appendhtml.AppendFormat('<option value="{0}" selected=\"selected\">{1}</option>',slideTypeList[l].value,slideTypeList[l].name);
                    }
                    else {
                        appendhtml.AppendFormat('<option value="{0}">{1}</option>',slideTypeList[l].value,slideTypeList[l].name);
                    }
                   
                }
                    ("</select></td></tr>");

                appendhtml.AppendFormat("<tr class=\"trKindeditor\" style=\"display:{1};\"><td>内容:</td><td><div id=\"txtEditor{0}\" style=\"height: 200px;\">{2}</div></td></tr>", i,trHtmlDisplay,middList[i].Content);
                appendhtml.AppendFormat("</table>");
                appendhtml.AppendFormat("</div>");
                

              
               
            }
           
            $("#btnAddItem").before(appendhtml.ToString());

           //生成编辑器
            for (var i = 0; i < editorsEditIndex.length; i++) {
                $("#rd"+editorsEditIndex[i]+"html").click();

            }




        
        }
    </script>
</asp:Content>
