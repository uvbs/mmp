<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;专家预约&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>专家管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

            <a href="Edit.aspx?type=<%=Request["type"] %>" class="easyui-linkbutton"
                iconcls="icon-add2" plain="true" id="btnAdd">添加</a>


             <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除</a>


             <a href="javascript:;" onclick="ShowQRcode()" style="color:blue;">获取手机端链接</a>&nbsp;&nbsp;
            <br />
            <label style="margin-left: 8px;">
                科室</label>
            <%=sbCategory.ToString()%>
           
            <label style="margin-left: 8px;">
                关键字</label>
            <input type="text" id="txtKeyWord" style="width: 200px;" placeholder="姓名,身份,预约地址"/>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" src="" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/Handler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=Request.Url.Host %>';
        var type = "<%=Request["type"]%>";
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "DoctorList",type:type },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'PID', title: '编号', width: 50, align: 'left' },
                                {
                                    field: 'RecommendImg', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                        return str.ToString();
                                    }
                                },
                            {
                                field: 'PName', title: '专家姓名', width: 160, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a target="_blank"  title="" )">{1}</a>', rowData.PID, rowData.PName);
                                    return str.ToString();
                                }
                            },
                           { field: 'ExArticleTitle_1', title: '职务/职称', width: 100, align: 'center', formatter: FormatterTitle },
                           { field: 'ExArticleTitle_2', title: '身份', width: 100, align: 'center', formatter: FormatterTitle },
                           { field: 'ExArticleTitle_4', title: '预约地址', width: 100, align: 'center', formatter: FormatterTitle },
                           { field: 'ExArticleTitle_3', title: '所属科室', width: 100, align: 'center', formatter: FormatterTitle },
                           { field: 'SaleCount', title: '已经预约数量', width: 100, align: 'center', formatter: FormatterTitle },
                           { field: 'Stock', title: '可预约数量', width: 100, align: 'left', formatter: function (value, rowData) {
                                                              var newvalue = "";
                                                              if (value != null) {
                                                                  newvalue = value;
                                                              }
                                                              var str = new StringBuilder();
                                                              str.AppendFormat('<input type="text" value="{0}" id="txtStock{1}" style="width:50px;" maxlength="5"> <a title="点击保存排序号"  onclick="UpdateStock({1})" href="javascript:void(0);">保存</a>', newvalue, rowData.PID);
                                                              return str.ToString();
                                                          }
                                                      },
                           { field: 'Sort', title: '排序', width: 100, align: 'left', formatter: function (value, rowData) {
                                                                   var newvalue = "";
                                                                   if (value != null) {
                                                                       newvalue = value;
                                                                   }
                                                                   var str = new StringBuilder();
                                                                   str.AppendFormat('<input type="text" value="{0}" id="txtSort{1}" style="width:50px;" maxlength="5"> <a title="点击保存排序号"  onclick="UpdateSortIndex({1})" href="javascript:void(0);">保存</a>', newvalue, rowData.PID);
                                                                   return str.ToString();
                                                               }
                                                           },
                            {
                                field: 'EditCloum', title: '操作', width: 50, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击修改" href="Edit.aspx?id={0}&type={1}">修改</a>', rowData.PID,type);
                                    return str.ToString();
                                }
                            }
	                ]]
	            }
            );

            $("#ddlCategory").change(function () {

                Search();

            })


        });






        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DoctorDelete", ids: GetRowsIds(rows).join(',') },
                        success: function (resp) {
                            Alert("成功删除了" + resp.result + "条记录");
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });


        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].PID
                 );
            }
            return ids;
        }


        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "DoctorList", keyWord: $("#txtKeyWord").val(), CategoryId: $("#ddlCategory").val(),type:"<%=Request["type"]%>" }
	            });
        }


        //更新排序号
        function UpdateSortIndex(id) {

            var sortindex = $("#txtSort" + id).val();
            if ($.trim(sortindex) == "") {
                $("#txtSort" + id).focus();
                return false;
            }



            var re = /^[1-9]+[0-9]*]*$/;
            if (!re.test(sortindex)) {
                alert("请输入正整数");
                $("#txtSort" + id).val("");
                $("#txtSort" + id).focus();
                return false;
            }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "UpdateDoctorSortIndex", id: id, Sort: sortindex },
                dataType: "json",
                success: function (resp) {
                    try {
                        if (resp.status ==true) {
                            Show("修改成功");
                            //$('#grvData').datagrid("reload");
                        }
                        else {
                            Alert("修改失败");
                        }
                    } catch (e) {
                        alert(e);
                    }

                }
            });


        }

        //更新排序号
        function UpdateStock(id) {

            var sortindex = $("#txtStock" + id).val();
            if ($.trim(sortindex) == "") {
                $("#txtStock" + id).focus();
                return false;
            }



            var re = /^[0-9]+[0-9]*]*$/;
            if (!re.test(sortindex)) {
                alert("请输入正整数");
                $("#txtStock" + id).val("");
                $("#txtStock" + id).focus();
                return false;
            }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "UpdateDoctorStock", id: id, Stock: sortindex },
                dataType: "json",
                success: function (resp) {
                    try {
                        if (resp.status == true) {
                            Show("修改成功");
                            
                        }
                        else {
                            Alert("修改失败");
                        }
                    } catch (e) {
                        alert(e);
                    }

                }
            });


        }

        function ShowQRcode() {
            


            $('#dlgSHowQRCode').dialog('open');

            var linkUrl = "http://" + domain + "/customize/bookingdoctor/index.aspx";
            if ("<%=Request["type"]%>" == "BookingDoctorFuYou") {
               
                linkUrl = "http://" + domain + "/customize/bookingdoctorfuyou/index.aspx";
            }

            $.ajax({
                type: 'post',
                url: "/Handler/QCode.ashx",
                data: { code: linkUrl },
                success: function (result) {
                    $("#imgQrcode").attr("src", result);
                }
            });
            $("#alinkurl").html(linkUrl).attr("href", linkUrl);
        }
    </script>
</asp:Content>
