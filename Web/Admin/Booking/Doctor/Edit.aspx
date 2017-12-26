<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
            font-size: 12px;
        }
        .return
        {
            float: right;
            margin-right: 5px;
        }
        input[type=text], select
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        .rmb
        {
            color: Red;
            font-weight: bold;
        }
        #txtExArticleTitle_1,#txtExArticleTitle_2,#txtExArticleTitle_4,#txtSummary {
            width:100%;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;预约&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="List.aspx" title="返回列表">专家管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=headTitle%></span>
        <a href="List.aspx?type=<%=Request["type"] %>" style="float: right; margin-right: 20px;" title="返回列表"
            class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <% 
       ZentCloud.BLLJIMP.BLL bll= new ZentCloud.BLLJIMP.BLL();
       System.Text.StringBuilder sbWhere = new StringBuilder();
       sbWhere.AppendFormat(" WebsiteOwner='{0}'",bll.WebsiteOwner);
       if (!string.IsNullOrEmpty(Request["type"]))
       {
           sbWhere.AppendFormat(" And TagType='{0}' ", Request["type"]);
       }
       else
       {
           sbWhere.AppendFormat(" And TagType='Booking' ");
       }
       var tagList = bll.GetList<ZentCloud.BLLJIMP.Model.MemberTag>(sbWhere.ToString()); %>
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        专家姓名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPName" value="<%=productModel.PName%>" style="width: 100%;" placeholder="请输入专家姓名"/>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        头像：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.RecommendImg == null ? "" : productModel.RecommendImg%>"
                            width="80px" height="80px" id="imgThumbnailsPath" /><br />
                      <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>

                    <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        宣传图：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="<%=productModel.ShowImage1 == null ? "" : productModel.ShowImage1%>"
                            width="300px" height="150px" id="imgThumbnailsPathShow1" /><br />
                      <a id="auploadThumbnailsShow1"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPathShow1.click()">上传</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片图片最佳显示效果为640*275
                        <input type="file" id="txtThumbnailsPathShow1" name="file2" style="display:none;" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        分享描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" value="<%=productModel.Summary%>"  placeholder="请输入分享描述"/>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        职务/职称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtExArticleTitle_1" value="<%=productModel.ExArticleTitle_1%>"  placeholder="请输入职务/职称"/>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        身份：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtExArticleTitle_2" value="<%=productModel.ExArticleTitle_2%>" placeholder="请输入身份"/>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        预约地址：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtExArticleTitle_4" value="<%=productModel.ExArticleTitle_4%>" placeholder="请输入预约地址"/>
                    </td>
                </tr>
                    <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        所属科室：
                    </td>
                    <td width="*" align="left">
                        <%=sbCategory.ToString()%>
                        
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        可预约数量：
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="color: Red; font-weight: bold; font-size: 20px; padding-left: 10px;"
                            id="txtStock" value="<%=productModel.Stock%>" onkeyup="this.value=this.value.replace(/[^\.\d]/g,'');this.value=this.value.replace('.','');" placeholder="请输入数字" maxlength="5">
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        已预约数量：
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="color: Red; font-weight: bold; font-size: 20px; padding-left: 10px;"
                            id="txtSaleCount" value="<%=productModel.SaleCount%>" onkeyup="this.value=this.value.replace(/[^\.\d]/g,'');this.value=this.value.replace('.','');" placeholder="请输入数字" maxlength="5">
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        排序(数值越大越靠前)：
                    </td>
                    <td width="*" align="left">
                        <input type="text" style="color: Red; font-weight: bold; font-size: 20px; padding-left: 10px;"
                            id="txtSort" value="<%=productModel.Sort%>" onkeyup="this.value=this.value.replace(/[^\.\d]/g,'');this.value=this.value.replace('.','');" placeholder="请输入数字" maxlength="5"/>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        预约：
                    </td>
                    <td width="*" align="left">
                       <input type="radio" name="rdoIsOnSale" id="rdoIsOnSale1" checked="checked" value="1" /><label for="rdoIsOnSale1">正常预约</label>
                       <input type="radio" name="rdoIsOnSale" id="rdoIsOnSale0" value="0" /><label for="rdoIsOnSale0">停止预约</label>
                    </td>
                </tr>

                                <tr >
                    <td style="width: 50px; vertical-align: top;" align="right" class="tdTitle">
                        标签：
                    </td>
                    <td align="left">
                        
                        <%
                            System.Text.StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < tagList.Count; i++)
                            {

                              sb.AppendFormat("<input type=\"checkbox\" name=\"cbtags\" id=\"cb{0}\" value=\"{1}\"><label for=\"cb{0}\">{1}</label>&nbsp;&nbsp;", i, tagList[i].TagName);
                                
                            }
                            Response.Write(sb.ToString());
                          %>
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            专家介绍：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                                <%=productModel.PDescription == null ? "" : productModel.PDescription%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px; text-decoration: underline;"
                            class="button button-rounded button-primary">确定</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/Handler.ashx";
        var currAction = '<%=webAction %>';
        var editor;
        var type = "<%=Request["type"]%>";
        $(function () {

            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }
            if (currAction == 'add') {
              
            }
            else { //编辑
                var categoryId = "<%=productModel.CategoryId%>"; //分类编号
                $("#ddlCategory").val(categoryId);

                var isOnSale = "<%=productModel.IsOnSale%>"; 
                if (isOnSale=="0") {
                    rdoIsOnSale0.checked = true;
                }
                else {
                    rdoIsOnSale1.checked = true;
                }

                SetTags("<%=productModel.Tags%>");

            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {

                        PID: "<%=id%>",
                        PName: $.trim($('#txtPName').val()),
                        RecommendImg: $('#imgThumbnailsPath').attr('src'),
                        ShowImage1: $('#imgThumbnailsPathShow1').attr('src'),
                        Stock: $("#txtStock").val(),
                        SaleCount: $("#txtSaleCount").val(),
                        ExArticleTitle_1: $.trim($("#txtExArticleTitle_1").val()),
                        ExArticleTitle_2: $.trim($("#txtExArticleTitle_2").val()),
                        ExArticleTitle_3: $("#ddlCategory").find("option:selected").text().replace('└', ''),
                        ExArticleTitle_4: $.trim($("#txtExArticleTitle_4").val()),
                        Sort: $.trim($('#txtSort').val()),
                        CategoryId: $.trim($("#ddlCategory").val()),
                        Summary: $("#txtSummary").val(),
                        PDescription: editor.html(),
                        IsOnSale: $("input[name='rdoIsOnSale']:checked").val(),
                        Tags: GetTags(),
                        ArticleCategoryType:"<%=Request["type"]%>",
                        Action:"DoctorAddEdit"

                    };
                    if (model.PName == '') {
                        $('#txtPName').focus();
                        //Alert('请输入姓名！');
                        return;
                    }
                    if (model.Stock == '') {
                        $('#txtStock').focus();
                        return;
                    }
                    if (model.Sort == '') {
                        $('#txtSort').focus();
                        return;
                    }
                    if (model.RecommendImg == '') {
                       
                        Alert('请上传专家头像');
                        return;
                    }
                    if (model.CategoryId == '') {
                        alert("请选择科室");
                        return;
                    }
                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.status ==true) {
                                if (currAction == 'add')
                                {
                                    ResetCurr();
                                } else {
                                    window.location.href = "List.aspx?type="+type;
                               
                                }
                                    
                                Alert(resp.msg);
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

            $('#btnReset').click(function () {
                ResetCurr();

            });

            //头像
            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {

                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    alert(e);
                }
            });

            //头像
            $("#txtThumbnailsPathShow1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=file2',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPathShow1',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgThumbnailsPathShow1').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    alert(e);
                }
            });

            //



        });

        function ResetCurr() {
            //ClearAll();
            $("input[type='text']").val("");
            $("#txtStock").val("0");
            $("#txtSaleCount").val("0");
            $("#txtSort").val("0");
            $("#ddlCategory").val("");
            
            $("#imgThumbnailsPath").attr("src", "");
            $("#imgThumbnailsPathShow1").attr("src", "");
            $("input[name='cbtags']").each(
             function () {
             $(this).removeAttr("checked");

             }

     )
            editor.html('');
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'importword', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                filterMode: false
            });
        });

        function GetTags() {
            var tags = [];
            $("input[name='cbtags']:checked").each(
                function () {

                    tags.push($(this).val());

                }


                )

            return tags.join(',');

        }

        function SetTags(tags) {
           
            for (var i = 0; i < tags.split(',').length ; i++) {
               
                $("input[name='cbtags']").each(
                    function () {
                        console.log(tags.split(',')[i]);
                        console.log($(this).val());
                   if (tags.split(',')[i]==$(this).val()) {

                        $(this).attr("checked", "checked");
        }



    }


    )


            }
           




           

        }


    </script>
</asp:Content>