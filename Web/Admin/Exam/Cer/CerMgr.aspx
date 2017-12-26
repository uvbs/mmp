<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CerMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Exam.Cer.CerMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .style1 {
            width: 29%;
        }

        #txtThumbnailsPath {
            display: none;
        }

        #txtCode, #txtCodeName, #txtModelCode {
            width: 100%;
            height: 30px;
            font-size: 16px;
            font-weight: bold;
        }

        #imgThumbnailsPath {
            width: 100px;
            height: 50px;
        }

        .cerimg {
            max-width: 100px;
        }

        .upload-item {
            width: 100px;
            height: 50px;
            float: left;
            margin-top: 35px;
            margin-left: 10px;
        }
            .upload-item img {
                width:100px;
                height:50px;

            }
        input[type='file'] {
            display:none;
        }
        /*img:hover{
            transform:scale(2.5);
}*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>证书管理 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd()" id="btnUpload">上传</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                onclick="ShowQrCode()">获取手机端链接</a>
            <br />
            <div>
                <span style="font-size: 12px; font-weight: normal">关键字：</span>
                <input type="text" style="width: 200px" id="txtKeyWord" placeholder="证书编号,姓名,身份证号" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search()">查询</a>
            </div>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th title="checkbox" checkbox="true" width="15"></th>
                <th field="BarCode" width="15">证书编号
                </th>
                <th field="CodeName" width="15">姓名
                </th>
                <th field="ModelCode" width="15">身份证号
                </th>
                <th field="ImageUrl" width="65" formatter="FormatterImage">证书图片
                </th>

            </tr>
        </thead>
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="证书" style="width: 550px; padding: 15px; line-height: 30px; height: 420px;">
      

        <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath1" onclick="txtThumbnailsPath1.click()" />
            <input type="file" id="txtThumbnailsPath1" name="file1" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath1.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath2" onclick="txtThumbnailsPath2.click()" />
            <input type="file" id="txtThumbnailsPath2" name="file2" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath2.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath3" onclick="txtThumbnailsPath3.click()" />
            <input type="file" id="txtThumbnailsPath3" name="file3" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath3.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath4" onclick="txtThumbnailsPath4.click()" />
            <input type="file" id="txtThumbnailsPath4" name="file4" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath4.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath5" onclick="txtThumbnailsPath5.click()" />
            <input type="file" id="txtThumbnailsPath5" name="file5" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath5.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath6" onclick="txtThumbnailsPath6.click()" />
            <input type="file" id="txtThumbnailsPath6" name="file6" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath6.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath7" onclick="txtThumbnailsPath7.click()" />
            <input type="file" id="txtThumbnailsPath7" name="file7" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath7.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath8" onclick="txtThumbnailsPath8.click()" />
            <input type="file" id="txtThumbnailsPath8" name="file8" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath8.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath9" onclick="txtThumbnailsPath9.click()" />
            <input type="file" id="txtThumbnailsPath9" name="file9" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath9.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath10" onclick="txtThumbnailsPath10.click()" />
            <input type="file" id="txtThumbnailsPath10" name="file9" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath10.click()">上传证书</a>
        </div>
                <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath11" onclick="txtThumbnailsPath11.click()" />
            <input type="file" id="txtThumbnailsPath11" name="file11" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath11.click()">上传证书</a>
        </div>

            <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPath12" onclick="txtThumbnailsPath12.click()" />
            <input type="file" id="txtThumbnailsPath12" name="file12" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath12.click()">上传证书</a>
        </div>

    </div>

        <div id="dlgEdit" class="easyui-dialog" closed="true" title="证书" style="width: 550px; padding: 15px; line-height: 30px; height: 420px;">
        <table width="100%" >

            <tr>
                <td>姓名:
                </td>
                <td>
                    <input id="txtCodeName" type="text" placeholder="姓名(选填)" />
                </td>

            </tr>
            <tr>
                <td>身份证号:
                </td>
                <td>
                    <input id="txtModelCode" type="text" placeholder="身份证号(选填)" />
                </td>

            </tr>
            <tr>
                <td>证书编号:
                </td>
                <td>
                    <input id="txtCode" type="text" placeholder="证书编号(如果不填,图片名称将作为证书编号)" />
                </td>

            </tr>

            <tr>

                <td></td>
                <td>
                            <div class="upload-item">
            <img alt="点击上传" src="" id="imgThumbnailsPathEdit" onclick="txtThumbnailsPathEdit.click()" />
            <input type="file" id="txtThumbnailsPathEdit" name="fileEdit" />
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPathEdit.click()">上传证书</a>
        </div>



                </td>
            </tr>
        </table>


               

    </div>

    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" src="" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/ajaxImgUploadNotNewName.js?v=2016111501" type="text/javascript"></script>
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/Exam/Cer/Cer.ashx";
        var currSelectID = 0;
        var currAction = 'Add';
        var grid;
        $(function () {

            $('#txtThumbnailsPath1').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath1',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath1.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath2').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath2',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath2.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath3').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath3',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath3.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath4').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath4',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath4.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });

            $('#txtThumbnailsPath5').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath5',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath5.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });

            $('#txtThumbnailsPath6').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath6',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath6.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath7').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath7',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath7.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath8').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath8',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath8.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath9').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath9',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath9.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPath10').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath10',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath10.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });

            $('#txtThumbnailsPath11').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath11',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath11.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });

            $('#txtThumbnailsPath12').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath12',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath12.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });


            $('#txtThumbnailsPathEdit').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=ExamCer&isnotnewfilename=1',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPathEdit',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPathEdit.src = resp.ExStr;

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    $.messager.progress('close');
                    //alert(e);
                }
            });
            //-----------------加载gridview
            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight - 120,
                toolbar: '#divToolbar',
                fitCloumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "List" }
            });
            //------------加载gridview

            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {

                        //var dataModel = GetDlgModel();

                        //if (dataModel.ImageUrl == '') {
                        //    alert('请上传证书图片', 3);
                        //    return;
                        //}
                        var dataModel = {
                            Action:"Add",
                            img1: $("#imgThumbnailsPath1").attr("src"),
                            img2: $("#imgThumbnailsPath2").attr("src"),
                            img3: $("#imgThumbnailsPath3").attr("src"),
                            img4: $("#imgThumbnailsPath4").attr("src"),
                            img5: $("#imgThumbnailsPath5").attr("src"),
                            img6: $("#imgThumbnailsPath6").attr("src"),
                            img7: $("#imgThumbnailsPath7").attr("src"),
                            img8: $("#imgThumbnailsPath8").attr("src"),
                            img9: $("#imgThumbnailsPath9").attr("src"),
                            img10: $("#imgThumbnailsPath10").attr("src"),
                            img11: $("#imgThumbnailsPath11").attr("src"),
                            img12: $("#imgThumbnailsPath12").attr("src")
                        }



                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status == 1) {
                                   
                                    $('#dlgInput').dialog('close');
                                    $('#grvData').datagrid('reload');

                                }
                                else {
                                    alert(resp.msg, 2);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });

            $('#dlgEdit').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {

                        var dataModel = GetDlgModel();

                        if (dataModel.ImageUrl == '') {
                            alert('请上传证书图片', 3);
                            return;
                        }
                       


                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status == 1) {
                                    alert("操作成功");
                                    $('#dlgEdit').dialog('close');
                                    $('#grvData').datagrid('reload');

                                }
                                else {
                                    alert(resp.msg, 2);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgEdit').dialog('close');
                    }
                }]
            });



        });
        function FormatterImage(value, row) {
            var str = new StringBuilder();
            str.AppendFormat('<img src="{0}" class="cerimg"/>', value);
            return str.ToString();
        }



        function ShowAdd() {

            
            $(imgThumbnailsPath1).attr("src", "");
            $(imgThumbnailsPath2).attr("src", "");
            $(imgThumbnailsPath3).attr("src", "");
            $(imgThumbnailsPath4).attr("src", "");
            $(imgThumbnailsPath5).attr("src", "");
            $(imgThumbnailsPath6).attr("src", "");
            $(imgThumbnailsPath7).attr("src", "");
            $(imgThumbnailsPath8).attr("src", "");
            $(imgThumbnailsPath9).attr("src", "");
            $(imgThumbnailsPath10).attr("src", "");
            $(imgThumbnailsPath11).attr("src", "");
            $(imgThumbnailsPath12).attr("src", "");
            $('#dlgInput').dialog('open');
            currAction = "Add";
        }

        function ShowEdit() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }



            $('#dlgEdit').dialog('open');
            currAction = "Update";
            //加载编辑数据
            currSelectID = rows[0].AutoId;
            $(txtCode).val(rows[0].BarCode);
            $(txtCodeName).val(rows[0].CodeName);
            $(txtModelCode).val(rows[0].ModelCode);
            imgThumbnailsPathEdit.src = rows[0].ImageUrl;
        }
        //批量删除
        function Delete() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确定删除选中？', function (o) {
                if (o) {
                    var ids = new Array();
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].AutoId);
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "Delete", ids: ids.join(',') },
                        success: function (resp) {
                            if (resp.status == true) {
                                alert("删除成功");
                                grid.datagrid('reload');
                            } else {
                                alert("删除失败");
                            }


                        }
                    });
                }
            });
        }

        //获取对话框数据实体
        function GetDlgModel() {
            var model =
              {
                  "AutoId": currSelectID,
                  "ImageUrl": $(imgThumbnailsPathEdit).attr("src"),
                  "BarCode": $(txtCode).val(),
                  "CodeName": $(txtCodeName).val(),
                  "ModelCode": $(txtModelCode).val(),
                  "Action": currAction

              }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['Code'] == '') {
                $(txtCode).val("");
                return false;
            }
            if (model['ImageUrl'] == '') {
                alert("请上传证书图片");
                return false;
            }
            return true;
        }



        function Search() {
            $('#grvData').datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { Action: "List", keyword: $("#txtKeyWord").val() }
                  });
        }

        function ShowQrCode() {
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + "<%=Request.Url.Host%>" + '/customize/youzheng/cer/query.aspx');
            $('#dlgSHowQRCode').dialog('open');
            var linkUrl = "http://" + "<%=Request.Url.Host%>" + "/customize/youzheng/cer/query.aspx";
            $("#alinkurl").html(linkUrl).attr("href", linkUrl);
        }



    </script>
</asp:Content>
