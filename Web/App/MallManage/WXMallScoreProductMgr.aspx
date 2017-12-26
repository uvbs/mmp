<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallScoreProductMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallScoreProductMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微会员&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>积分商品管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="/App/MallManage/WXMallScoreProductCompile.aspx?Action=add" class="easyui-linkbutton"
                iconcls="icon-add2" plain="true" id="btnAdd">添加新商品</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除商品</a>
            <br />
            上架/下架:
            <select id="ddlisonsale">
                <option value="">全部</option>
                <option value="1">上架</option>
                <option value="0">下架</option>
            </select>
            线上/线下:
            <select id="OnLine" onchange="CHOnLine()">
                <option value="">全部</option>
                <option value="0">线上</option>
                <option value="1">线下</option>
            </select>
            <select id="TypeInfos" style="display: none;">
                <option value="">全部</option>
                <%=sbTypeInfo.ToString() %>
            </select>
            <label style="margin-left: 8px;">
                商品名称</label>
            <input type="text" id="txtPName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码即可打开本页面进行分享" modal="true" style="width: 300px; height: 300px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=Request.Url.Host %>';
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallScoreProductInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'RecommendImg', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                            { field: 'PName', title: '商品名称', width: 160, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a target="_blank"  title="点击查看商品详情" href="/App/Cation/wap/mall/ScoreProductDetail.aspx?pid={0}">{1}</a>', rowData.AutoID, rowData.PName);
                                return str.ToString();
                            }
                        },

                            { field: 'Score', title: '所需积分', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Pv', title: '浏览量', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Stock', title: '库存', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'IsOnSale', title: '上架/下架', width: 100, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                if (value == "1") {
                                    str.AppendFormat("<font color='green'>上架</font>");
                                }
                                else {
                                    str.AppendFormat("<font color='red'>下架</font>");
                                }
                                return str.ToString();
                            }
                            },

                            { field: 'InsertDate', title: '时间', width: 100, align: 'center', formatter: FormatDate },
                            { field: 'Sort', title: '排序', width: 100, align: 'left', formatter: function (value, rowData) {
                             var newvalue = "";
                              if (value != null) {
                               newvalue = value;
                               }
                              var str = new StringBuilder();
                              str.AppendFormat('<input type="text" value="{0}" id="txtScoreProduct{1}" style="width:50px;" maxlength="5"> <a title="点击保存排序号"  onclick="UpdateSortIndex({1})" href="javascript:void(0);">保存</a>', newvalue, rowData.AutoID);
                              return str.ToString();
                               }
                              },
                            { field: 'EditCloum', title: '操作', width: 50, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="点击修改商品信息" href="/App/MallManage/WXMallScoreProductCompile.aspx?Action=edit&pid={0}">编辑</a>', rowData.AutoID);
                                return str.ToString();
                            }
                            }




                             ]]
	            }
            );


        });




        function ShowQRcode(aid) {
            //dlgSHowQRCode
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/' + aid + '/' + 'details.chtml');
            $('#dlgSHowQRCode').dialog('open');
        }

        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中商品?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWXMallScoreProductInfo", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            Alert("成功删除了" + result + "件商品");
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
                ids.push(rows[i].AutoID
                 );
            }
            return ids;
        }


        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallScoreProductInfo", OnLine: $("#OnLine").val(), TypeId: $("#TypeInfos").val(), PName: $("#txtPName").val(), IsOnSale: $("#ddlisonsale").val() }
	            });
        }
        function CHOnLine() {
            var onLine = $("#OnLine").val()
            if (onLine == 1) {
                $("#TypeInfos").show();
            } else {
                $("#TypeInfos").hide();
                $("#TypeInfos").val("");
            }


        }

        //更新排序号
        function UpdateSortIndex(id) {

            var sortindex = $("#txtScoreProduct" + id).val();
            if ($.trim(sortindex) == "") {
                $("#txtScoreProduct" + id).focus();
                return false;
            }



//            var re = /^[1-9]+[0-9]*]*$/;
//            if (!re.test(sortindex)) {
//                alert("请输入正整数");
//                $("#txtScoreProduct" + id).val("");
//                $("#txtScoreProduct" + id).focus();
//                return false;
//            }


            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "UpdateScoreProductSortIndex", id: id, SortIndex: sortindex },
                dataType: "json",
                success: function (resp) {
                    try {
                        if (resp.Status == 1) {
                            Show(resp.Msg);
                            $('#grvData').datagrid("reload");
                        }
                        else {
                            Alert(resp.Msg);
                        }
                    } catch (e) {
                        alert(e);
                    }

                }
            });


        }
    </script>
</asp:Content>
