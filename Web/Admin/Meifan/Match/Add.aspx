<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Match.Add" %>
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
        .amount {
             height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        .items {
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

        .delete-item {
            float: right;
            right: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .items input[type=text] {
            width: 90%;
        }

        #fileActivity {
            display: none;
        }

        .centent_r_btm {
            height: auto !important;
        }
        #divItems {
            display:none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;&nbsp;&nbsp;竞赛>&nbsp;&gt;&nbsp;&nbsp;<span>添加</span> <a title="返回管理" style="float: right; margin-right: 20px;" href="List.aspx" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%;">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityName" maxlength="100" value="" style="width: 100%;" placeholder="名称(必填)" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" maxlength="100" value="" style="width: 100%;" placeholder="描述(选填)" />
                    </td>
                </tr>
                <tr style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">要点：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMainPoints" maxlength="100" value="" style="width: 100%;" placeholder="要点(选填)" />
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="" width="80px" id="imgActivity" />
                        <br />
                        <a id="auploadThumbnails"
                            href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileActivity.click()">上传</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG,PNG 格式图片
                        <input type="file" id="fileActivity" name="file1" />
                    </td>
                </tr>

                 

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">详细内容：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtDescription" style="width: 100%; height: 500px;"></textarea>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">免费/收费：
                    </td>
                    <td width="*" align="left">
                       <input id="rdoIsNeedPay0" name="rdoIsNeedPay"  type="radio" value="0" checked="checked"/><label for="rdoIsNeedPay0">免费</label>
                       <input id="rdoIsNeedPay1" name="rdoIsNeedPay"  type="radio" value="1"/><label for="rdoIsNeedPay1">收费</label>
                    </td>
                </tr>


            </table>

            <div id="divItems">
            <strong style="font-size: 20px;">收费选项:</strong>
            <div class="items" data-item-index="0" data-item-id="">
                <%--<img src="/img/icons/up.png" class="upfield fieldsort" />
                <img src="/img/icons/down.png" class="downfield fieldsort" />--%>
                <img src="/img/delete.png" class="delete-item" />
                <table style="width: 100%; margin-left: 10px;">

                    <tr>
                        <td>开始日期: 
                            <input type="date" class="begin-date" value="" />
                            结束日期:
                            <input type="date" class="end-date" />
                            组别:<select class="group-type">
                                <%foreach (var item in GroupList){%>
                                 <option value="<%=item.TagName%>"><%=item.TagName %></option>
                                 <% } %>

                            </select>
                            是否会员:
                        <select class="member-type">
                            <option value="全部">全部</option>
                            <option value="会员">会员</option>
                            <option value="非会员">非会员</option>
                        </select>

                            金额:<input type="number" class="amount" />
                        </td>
                    </tr>

                </table>
            </div>
            <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddItem">添加选项</a>
            </div>


            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 10px;">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary" style="width: 200px;">添加</a>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/serv/api/admin/meifan/match/add.ashx";
        var editor;
        var itemCount = 1; //数量
        var goupList = <%=GroupListJson%>;
        var memberTypeList = ["全部","会员", "非会员"];
        $(function () {



            $("input[name='rdoIsNeedPay']").click(function () {

                if ($(this).val() == "0") {
                    $("#divItems").hide();

                } else {
                    $("#divItems").show();
                }


            })
            //上移
            //$(".upfield").live("click", function () {
            //    if ($(this).closest("div").prev(".items").length > 0) {
            //        $(this).closest("div").prev(".items").before($(this).closest("div").clone());
            //        $(this).closest("div").remove();


            //    }
            //});

            ////下移
            //$(".downfield").live("click", function () {
            //    if ($(this).closest("div").next(".items").length > 0) {
            //        $(this).closest("div").next(".items").after($(this).closest("div").clone());
            //        $(this).closest("div").remove();


            //    }
            //});

            //删除


            //删除
            $('.delete-item').live("click", function () {

                if (confirm("确定要删除?")) {

                    $(this).parent().remove();
                }


            });
            //删除

            //添加
            $("#btnAddItem").click(function () {

                var count=$(".items").length;
                var beginDate=$($(".items")[count-1]).find(".begin-date").first().val();
                var endDate=$($(".items")[count-1]).find(".end-date").first().val();

                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<div class="items" data-item-index="{0}" data-item-id="">', itemCount);
                //appendhtml.AppendFormat('<img src="/img/icons/up.png" class="upfield fieldsort" />');
                //appendhtml.AppendFormat('<img src="/img/icons/down.png" class="downfield fieldsort" />');
                appendhtml.AppendFormat('<img src="/img/delete.png" class="delete-item" />');
                appendhtml.AppendFormat('<table style="width: 100%; margin-left: 10px;">');
                appendhtml.AppendFormat('<tr>');
                appendhtml.AppendFormat('<td>');
                appendhtml.AppendFormat('开始日期:  <input type="date" class="begin-date" value="{0}" />',beginDate);
                appendhtml.AppendFormat('结束日期: <input type="date" class="end-date" value="{0}"/>',endDate);
                appendhtml.AppendFormat('组别:');
                appendhtml.AppendFormat('<select class="group-type">');

                //appendhtml.AppendFormat('<option value="无">无</option>');
                //appendhtml.AppendFormat('<option value="男子团体">男子团体</option>');
                //appendhtml.AppendFormat(' <option value="女子团体">女子团体</option>');
                for (var l = 0; l < goupList.length; l++) {

                    appendhtml.AppendFormat('<option value="{0}">{1}</option>', goupList[l], goupList[l]);


                }


                appendhtml.AppendFormat('</select>');
                appendhtml.AppendFormat('是否会员:');
                appendhtml.AppendFormat('<select class="member-type">');

                //appendhtml.AppendFormat('<option value="会员">会员</option>');
                //appendhtml.AppendFormat('<option value="非会员">非会员</option>');
                for (var k = 0; k < memberTypeList.length; k++) {

                    appendhtml.AppendFormat('<option value="{0}">{1}</option>', memberTypeList[k], memberTypeList[k]);


                }

                appendhtml.AppendFormat('</select>');
                appendhtml.AppendFormat('金额:<input type="number" class="amount"/>');
                appendhtml.AppendFormat('</td>');
                appendhtml.AppendFormat('</tr>');
                appendhtml.AppendFormat('</table>');
                appendhtml.AppendFormat('</div>');

                $(this).before(appendhtml.ToString());

                itemCount++;


            });
            //添加


            //上传图片
            $("#fileActivity").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/serv/api/common/file.ashx?action=add',
                         secureuri: false,
                         fileElementId: 'fileActivity',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.errcode == 0) {

                                 $('#imgActivity').attr('src', resp.file_url_list[0]);
                             }
                             else {
                                 Alert(resp.errmsg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    Alert(e);
                }
            });


            //保存
            $('#btnSave').click(function () {

                if (confirm("确定添加?")) {

                    try {
                        var model = getModel();
                        if (model.activity_name == "") {
                            $("#txtActivityName").focus();
                            return false;
                        }
                        if (model.activity_img == "") {
                            Alert("请上传图片");
                            return false;
                        }
                        var jsonData = JSON.stringify(model);
                        $.messager.progress({ text: '正在添加...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { data: jsonData },
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.status == true) {
                                    alert("添加成功");
                                    window.location.href = "List.aspx";
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }
                        });

                    } catch (e) {
                        Alert(e);
                    }

                }



            });


        });



        KindEditor.ready(function (K) {
            editor = K.create('#txtDescription', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
    'insertunorderedlist', '|', 'importword','video', 'image', 'multiimage', 'link', 'unlink','lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                filterMode: false
            });
        });

        //获取模型
        function getModel() {
            //模型
            var model = {

                activity_name: $("#txtActivityName").val(),
                activity_img: $("#imgActivity").attr("src"),
                summary: $("#txtSummary").val(),
                main_points: $("#txtMainPoints").val(),
                is_need_pay: $("input[name='rdoIsNeedPay']:checked").val(),
                description: editor.html(),
                items: []
            }

            //中间内容
            $(".items").each(function () {

                var item = {
                    item_id: '',//类型 幻灯片,html
                    from_date: '',// html内容
                    to_date: '',//幻灯片名称
                    group_type: '',//幻灯片效果
                    is_member: '',
                    amount: ''

                };
                item.item_id = $(this).closest("div").attr('data-item-id');
                item.from_date = $(this).find(".begin-date").first().val();
                item.to_date = $(this).find(".end-date").first().val();
                item.group_type = $(this).find(".group-type").first().val();
                item.is_member = $(this).find(".member-type").first().val();
                item.amount = $(this).find(".amount").first().val();
                model.items.push(item);
            });
            return model;
        }

        function checkInput() {





        }
    </script>
</asp:Content>

