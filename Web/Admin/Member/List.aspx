<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Member.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .search-status {
            color: #0face0;
            margin: 0px 5px;
        }

        .imgAlign {
            margin: 5px;
        }

        .paginationImgAlign {
            position: relative;
            top: 4px;
            margin: 0px 2px;
        }

        .pagination-info-ex {
            float: right;
            padding-right: 15px;
            font-size: 12px;
        }

        .table {
            margin: auto;
            width: 100%;
            border-collapse: collapse;
        }

            .table td {
                border: solid #dddddd 1px;
                padding: 0px 5px;
                line-height: 26px;
            }
            .table td li{
                line-height:20px;
            }
            .table td li+li{
                border-top: solid 1px #e1e1e1;
            }
            .window-mask{
                width:100% !important;
                height:100% !important;
            }
            .datagrid-wrap{
                width:100% !important;
            }
            .btn-update{
                display:none;
            }
            
            .ImgDiv {
                display: inline-block; 
                padding: 3px 0px;
            }
            .ImgDiv a{
                display: inline-block;
                margin:3px;
                position:relative;
                border: solid #f3f3f3 1px;
                width: 80px; 
                height: 80px;
            }
            .ImgDiv a .showImg{
                width: 80px; 
                height: 80px;
            }
            .ImgDiv a .addImg{
                position:absolute;
                top:32px;
                left:32px;
                width: 16px; 
                height: 16px;
            }
            .ImgDiv a .fileImg,.ImgDiv a .addFileImg{
                position:absolute;
                top:0px;
                left:0px;
                width: 80px; 
                height: 80px;
                opacity:0;
                z-index:1;
            }
            .ImgDiv a .delImg{
                position:absolute;
                top:-5px;
                right:-5px;
                width: 16px; 
                height: 16px;
                z-index:2;
            }
           .dlgUpdateMemberInfo .dialog-content{
               overflow-x:hidden;
           }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"会员列表" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/member/shlist.ashx',pagination:true,striped:true,loadFilter: pagerFilter,rownumbers:true,showFooter:true,
        onLoadSuccess:thisOnLoadSuccess,
        queryParams:{min_level:10 }">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <%--<th field="id" width="35" formatter="FormatterTitle">编号</th>--%>
                <th field="phone" width="35" formatter="FormatterTitle">会员手机</th>
                <th field="name" width="35" formatter="FormatterTitle">会员姓名</th>
                <th field="up_user" width="35" formatter="FormatterUpMember">推荐人</th>
                <th field="reg_user" width="35" formatter="FormatterUpMember">报单人</th>
                <th field="reg_time" width="35" align="center" formatter="FormatterRegTime">注册时间</th>
                <th field="lv" width="35" align="center" formatter="FormatterLevel">当前级别</th>
                <th field="status" width="35" align="center" formatter="FormatterStatus">状态</th>
                <%--<th field="way" width="35" align="center" formatter="FormatterTitle">报单方式</th>
                <th field="stock" width="30" align="center" formatter="FormatterTitle">股权</th>
                <th field="ex1" width="25" align="center" formatter="FormatterHasImg">执照</th>--%>
                <th field="action" width="40" formatter="FormatterAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            会员级别：
            <select id="sltMemberLevel" class="easyui-combobox" style="width: 90px;" editable="false" 
                data-options="url:'/serv/api/admin/user/level/list.ashx',valueField: 'level_number',
                textField: 'level_string',loader:levelLoader,onChange:function(){Search();}">
            </select>
            会员：<input id="txtMember" class="easyui-textbox" style="width: 90px;" placeholder="手机/姓名" value="<% = Request["member"] %>" />
            推荐人：<input id="txtUpMember" class="easyui-textbox" style="width: 115px;" placeholder="手机/姓名" value="" />
            报单人：<input id="txtRegMember" class="easyui-textbox" style="width: 115px;" placeholder="手机/姓名" value="" />
            注册时间：<input id="txtStartTime" class="easyui-datebox" style="width: 90px;" />至<input id="txtEndTime" class="easyui-datebox" style="width: 90px;" />
        </div>
        <div>
            <input id="chkEmptyBill" type="checkbox" class="positionTop3" /><label for="chkEmptyBill">空单</label>
            <input id="chkTrueBill" type="checkbox" class="positionTop3" /><label for="chkTrueBill">实单</label>
            <input id="chkApplyPass" type="checkbox" class="positionTop3" /><label for="chkApplyPass">已审</label>
            <input id="chkApplyOther" type="checkbox" class="positionTop3" /><label for="chkApplyOther">未审</label>
            <input id="chkHasImg" type="checkbox" class="positionTop3" /><label for="chkHasImg">执照已传</label>
            <input id="chkNoImg" type="checkbox" class="positionTop3" /><label for="chkNoImg">执照未传</label>
            <input id="chkApplyCancel" type="checkbox" class="positionTop3" /><label for="chkApplyCancel">申请撤单</label>
            <input id="chkIsCancel" type="checkbox" class="positionTop3" /><label for="chkIsCancel">已撤单</label>
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
            <%if (canMemberExport){ %>
            <a id="btnExport" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-excel" onclick="SearchExport()">导出</a>
            <%} %>
        </div>
    </div>
    <div id="dlgMemberInfo" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'会员信息',width:640,height:document.documentElement.clientHeight-50,modal:true,buttons:'#dlgMemberInfoButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:16%;">姓名：</td>
                <td>
                    <span class="memberName member-label"></span>
                </td>
                <td style="width:16%;">手机：</td>
                <td>
                    <span class="memberPhone member-label"></span>
                </td>
            </tr>
            <tr>
                <td>身份证号：</td>
                <td>
                    <span class="memberIdentityCard member-label"></span>
                </td>
                <td>联系手机：</td>
                <td>
                    <span class="memberPhone1 member-label"></span>
                </td>
            </tr>
            <tr class="trUpUser">
                <td>推荐人：</td>
                <td colspan="3" class="memberUpUser"></td>
            </tr>
            <tr class="trRegUser">
                <td>报单人：</td>
                <td class="memberRegUser"></td>
                <td>报单方式：</td>
                <td>
                    <span class="memberWay member-label"></span>
                </td>
            </tr>
            <tr>
                <td>当前级别：</td>
                <td class="memberLevel"></td>
                <td>状态：</td>
                <td class="memberStatus"></td>
            </tr>
            <tr>
                <td>报单时间：</td>
                <td class="memberRegtime"></td>
                <td>审核时间：</td>
                <td class="memberApplyStarttime"></td>
            </tr>
            <tr>
                <td>账面余额：</td>
                <td class="memberAccountAmountEstimate"></td>
                <td>可用佣金：</td>
                <td class="memberTotalAmount"></td>
            </tr>
            <%--<tr>
                <td>总收入：</td>
                <td colspan="3" class="memberWinAmountTotal">
                </td>
            </tr>
            <tr>
                <td>总支出：</td>
                <td colspan="3" class="memberLoseAmountTotal">
                </td>
            </tr>--%>
            <tr>
                <td>账面公积金：</td>
                <td>
                    <span class="memberAccumulationFund member-label"></span>
                </td>
                <td>股权数：</td>
                <td>
                    <span class="memberStock member-label"></span>
                </td>
            </tr>
            <tr>
                <td>归属地：</td>
                <td colspan="3">
                    <span class="memberProvinceCity member-label"></span>
                </td>
            </tr>
            <tr>
                <td>地址：</td>
                <td colspan="3">
                    <span class="memberAddress member-label"></span>
                </td>
            </tr>
            <tr>
                <td>银行卡：</td>
                <td colspan="3" class="memberBankCard">
                </td>
            </tr>
            <tr>
                <td>执照：</td>
                <td colspan="3">
                    <div class="ImgDiv memberEx12345">
                    </div>
                </td>
            </tr>
            <tr>
                <td>凭证：</td>
                <td colspan="3">
                    <div class="ImgDiv memberEx678910">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgMemberInfoButtons">
            <%if (canLockMember)
              { %>
        <a href="javascript:void(0);" class="easyui-linkbutton btnLock" onclick="LockRegister()">锁定</a>
            <%}%>
            <%if (canCancelMemberRegister)
              { %>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="OpenCancelRegisterShow()">撤单</a>
            <%}%>
            <%if (canUpdateLoginPhone)
              { %>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="OpenUpdateLoginPhoneShow()">修改登录手机</a>
            <%}%>
            <%if (canUpdateDistributionOwner)
              { %>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="OpenUpdateDistributionOwnerShow()">更改推荐人</a>
            <%}%>
            <%if (canUpdateMemberInfo)
              { %>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="OpenUpdateMemberShow()">修改信息</a>
            <%}%>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgMemberInfo').dialog('close');">关闭</a>
    </div>
    <div  id="dlgUpdateMemberInfo" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'修改会员信息',width:500,height:400,modal:true,buttons:'#dlgUpdateMemberInfoButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:20%;">姓名：</td>
                <td>
                    <input type="text" maxlength="12" class="easyui-textbox member-text txtMemberName" placeholder="姓名" />
                </td>
            </tr>
            <tr>
                <td>联系手机：</td>
                <td>
                    <input type="tel" maxlength="11" class="easyui-textbox member-text txtMemberPhone1" placeholder="联系手机" />
                </td>
            </tr>
            <tr>
                <td>身份证号：</td>
                <td>
                    <input type="text" maxlength="18" class="easyui-textbox member-text txtMemberIdentityCard" placeholder="身份证号" />
                </td>
            </tr>
            <tr>
                <td>归属地：</td>
                <td >
                    <select class="easyui-combobox sltProvince" editable="false" 
                        data-options="valueField: 'code',textField: 'name',onChange:selectProvince">
                    </select>
                    <select class="easyui-combobox sltCity" editable="false" 
                        data-options="valueField: 'code',textField: 'name',onChange:selectCity"></select>
                    <select class="easyui-combobox sltDistrict" editable="false" 
                        data-options="valueField: 'code',textField: 'name',onChange:selectDistrict"></select>
                    <select class="easyui-combobox sltTown" editable="false"
                        data-options="valueField: 'code',textField: 'name'"></select>
                </td>
            </tr>
            <tr>
                <td>股权数：</td>
                <td >
                    <input type="text" maxlength="100" class="easyui-textbox member-text txtMemberStock" style="width:260px;" placeholder="请输入股权数" />
                </td>
            </tr>
            <tr>
                <td>地址：</td>
                <td >
                    <input type="text" maxlength="100" class="easyui-textbox member-text txtMemberAddress" style="width:260px;" placeholder="请输入地址" />
                </td>
            </tr>
            <tr>
                <td>
                    执照：
                </td>
                <td colspan="3">
                    <div class="ImgDiv txtEx12345">
                    </div>
                </td>
            </tr>
            <tr>
                <td>凭证：</td>
                <td colspan="3">
                    <div class="ImgDiv txtEx678910">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgUpdateMemberInfoButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="UpdateMember()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgUpdateMemberInfo').dialog('close');">取消</a>
    </div>
    
    <div  id="dlgUpdateLoginPhone" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'修改登录手机',width:350,height:200,modal:true,buttons:'#dlgUpdateLoginPhoneButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" cellpadding="0" cellspacing="0">
            <tr>
                <td>新登录手机：</td>
                <td>
                    <input type="tel" maxlength="11" class="easyui-textbox member-text txtMemberPhone" placeholder="新登录手机" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgUpdateLoginPhoneButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="UpdateLoginPhone()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgUpdateLoginPhone').dialog('close');">取消</a>
    </div>
    <div  id="dlgCancelRegister" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'撤单原因',width:450,height:320,modal:true,buttons:'#dlgCancelRegisterButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" style="width:95%;" cellpadding="0" cellspacing="0">
            <tr>
                <td style="border:0px;width:70px;">撤单原因：</td>
                <td style="border:0px;">
                    <div class="txtContent" 
                        contenteditable="true" 
                        style="width: 100%;line-height:21px;min-height:212px;padding:5px;border:solid #d3d3d3 1px;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgCancelRegisterButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="CancelRegister()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgCancelRegister').dialog('close');">取消</a>
    </div>
    <div id="dlgUpdateDistributionOwner" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'修改推荐人',width:350,height:200,modal:true,buttons:'#dlgUpdateDistributionOwnerButtons'"
        style="padding: 10px; line-height: 30px;">
        <table class="table" cellpadding="0" cellspacing="0">
            <tr>
                <td>推荐人手机/编号：</td>
                <td>
                    <input class="easyui-searchbox" data-options="prompt:'推荐人手机/编号',searcher:SearchSpread" />
                </td>
            </tr>
            <tr>
                <td>推荐人姓名：</td>
                <td>
                    <span class="memberID member-label hidden"></span>
                    <span class="memberTrueName member-label"></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgUpdateDistributionOwnerButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="UpdateDistributionOwner()">提交</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgUpdateDistributionOwner').dialog('close');">取消</a>
    </div>
    <div class="exportDiv" style="width:0px; height:0px; position:absolute; top:-10px;">
        <iframe id="exportIframe" style="width:0px; height:0px; position:absolute; top:-10px;"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/member/';
        var module_name = "<%=Request["module_name"]%>";
        var curIndex = -1;
        var curEvent = '';
        var provinces = [];
        var citys = [];
        var districts = [];
        var towns = [];
        var searchModel = {
            min_level: 10,
            level_num: ''
        };
        var curProvinceCode = "";
        var curCityCode = "";
        var curDistrictCode = "";
        var curTownCode = "";

        $(function () {
            bindEvent();
        });
        function bindEvent() {
            $('#dlgUpdateMemberInfo').on('click', '.delImg', function () {
                $(this).closest('a').remove();
            });
            $('#dlgUpdateMemberInfo').on('change', '.fileImg,.addFileImg', function () {
                if (this.files.length > 0) {
                    var isAdd = $(this).hasClass('addFileImg');
                    UploadImg(this, this.files[0], isAdd);
                }
            });
        }
        function UploadImg(ob, file, isAdd) {
            if (file.type.lastIndexOf('image/') != 0) {
                alert('仅能上传图片');
                return;
            }
            var _this = this;
            var fd = new FormData();//创建表单数据对象
            fd.append('file1', file);//将文件添加到表单数据中
            fd.append('action', 'Add');
            fd.append('dir', 'image');
            var xhr = new XMLHttpRequest();
            xhr.ob = ob;
            xhr.isAdd = isAdd;
            xhr.upload.addEventListener("progress", UploadProgress, false);//监听上传进度
            xhr.addEventListener("load", UploadComplete, false);
            xhr.addEventListener("error", UploadError, false);

            $.messager.progress();
            xhr.open("POST", '/Serv/API/Common/File.ashx');
            xhr.send(fd);
        }
        function UploadProgress(progress) {
            //$.messager.progress('close');
        }
        function UploadComplete(complete) {
            $.messager.progress('close');
            var resp = JSON.parse(complete.target.responseText);
            if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                var url = resp.file_url_list[0];
                if (this.isAdd) {
                    var str = new StringBuilder();
                    str.AppendFormat('<a href="javascript:void(0)"><img alt="" class="showImg" src="{0}" /><input type="file" class="fileImg" /><img alt="" class="delImg" src="/MainStyle/Res/easyui/themes/icons/no.png" /></a>', url);
                    $(this.ob).closest('a').before(str.ToString());
                    if($(this.ob).closest('.ImgDiv').children().length>5){
                        $(this.ob).closest('a').remove();
                    }
                }else{
                    $(this.ob).closest('a').find('.showImg').attr('src',url);
                }
            }
            else {
                alert(resp.errmsg);
            }
        }
        function UploadError(error) {
            $.messager.progress('close');
        }
        function levelLoader(param, success, error) {
            var that = $(this);
            var opts = that.combobox("options");
            $.ajax({
                type: opts.method,
                url: opts.url,
                data: { pageindex: 1, pagesize: 1000, hide_count: 1, min_level: 10 },
                dataType: "json",
                success: function (resp) {
                    var result = [{level_number:'',level_string:'全部'}];
                    if (resp.list && resp.list.length > 0) {
                        result = result.concat(resp.list);
                        success(result);
                    }
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        }

        function thisOnLoadSuccess(data) {
            var str = new StringBuilder();
            str.AppendFormat('<div class="pagination-info-ex">');
            str.AppendFormat('<img class="paginationImgAlign" src="/MainStyle/Res/easyui/themes/icons/contacts.gif" />详情');
            <%if(canResetMemberPwd){ %>
            str.AppendFormat('，<img class="paginationImgAlign" src="/MainStyle/Res/easyui/themes/icons/reload.png" />重置密码');
            <%}%>
            //str.AppendFormat('，<img class="paginationImgAlign" src="/MainStyle/Res/easyui/themes/icons/user_edit.png" />修改');
            str.AppendFormat('，<img class="paginationImgAlign" src="/MainStyle/Res/easyui/themes/icons/list.png" />财务明细');
            str.AppendFormat('，<img class="paginationImgAlign" src="/MainStyle/Res/easyui/themes/icons/article_add.png" />日志');
            //str.AppendFormat('，<img class="paginationImgAlign" src="/MainStyle/Res/easyui/themes/icons/no.png" />撤单');
            str.AppendFormat('<div>');
            $('.pagination .pagination-info-ex').remove();
            $('.pagination .pagination-info').after(str.ToString());
        }

        function FormatterMember(value, rowData) {
            if (!value) return "";
            return '姓名：' + rowData.name + '<br />手机：' + rowData.phone;
        }
        function FormatterUpMember(value) {
            if (!value) return "";
            if (value.phone) return value.phone;
            return value.name;
            //var r = [];
            //if (value.name && value.name != '  ') r.push('姓名：' + value.name);
            //if (value.id) r.push('编号：' + value.id);
            //if (value.phone && value.phone.length == 11) r.push('手机：' + value.phone);
            //if (value.phone && value.phone.length != 11) r.push('账号：' + value.phone);
            //return r.join('<br />');
        }
        function FormatterStatus(value, rowData) {
            if (!value) return "";
            if (rowData.is_lock == 1) return value + "<br />（锁定）";
            return value;
        }
        function FormatterRegTime(value) {
            if (!value) return "";
            var li = value.split(' ');
            return li[0] + '<br />' + li[1];
        }
        function FormatterLevel(value) {
            if (!value) return "";
            var li = value.split('(');
            if (li.length > 1) return li[0] + '<br />(' + li[1];
            return value;
        }
        function FormatterHasImg(value, rowData) {
            if (!value) return "待传";
            return "已传";
        }

        function FormatterAction(value, rowData, rowIndex) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:OpenShow({0})"><img alt="详情" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/contacts.gif" title="详情" /></a>', rowIndex);
            //str.AppendFormat('<a href="javascript:OpenEidtShow({0})"><img alt="修改" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/user_edit.png" title="修改" /></a>', rowIndex);
            <%if(canResetMemberPwd){ %>
            str.AppendFormat('<a href="javascript:OpenResetPwdShow({0})"><img alt="重置密码" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/reload.png" title="重置密码" /></a>', rowIndex);
            <%}%>
            //str.AppendFormat('<br />');
            str.AppendFormat('<a href="javascript:OpenScorePage({0})"><img alt="财务明细" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/list.png" title="财务明细" /></a>', rowData.id);
            str.AppendFormat('<a href="javascript:OpenLogPage(\'{0}\')"><img alt="日志" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/article_add.png" title="日志" /></a>', rowData.uid);
            //str.AppendFormat('<a href="javascript:OpenDeleteShow({0})"><img alt="撤单" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/no.png" title="撤单" /></a>', rowData.id);
            return str.ToString();
        }
        function OpenShow(rowIndex) {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[rowIndex];
            curIndex = rowIndex;
            $('#dlgMemberInfoButtons .easyui-linkbutton.btn-update').hide();
            $('#dlgMemberInfo .member-text').hide();
            var btnLockText = row.is_lock == 1 ? '解锁' : '锁定';
            if (row.other) {
                SetDialogMemberInfo(row);
                $('#dlgMemberInfo .btnLock span:last').text(btnLockText);
                $('#dlgMemberInfo').dialog('open');
            } else {
                $('#dlgMemberInfo .btnLock span:last').text(btnLockText);
                $('#dlgMemberInfo').dialog('open');
                $.messager.progress();
                $.ajax({
                    type: 'post',
                    url: handlerUrl + 'shget.ashx',
                    data: { id: row.id },
                    dataType: 'json',
                    success: function (resp) {
                        $.messager.progress('close');
                        if (resp.status) {
                            row.other = resp.result;
                            SetDialogMemberInfo(row);
                        } else {
                            alert(resp.msg);
                        }
                    },
                    error: function () {
                        $.messager.progress('close');
                    }
                });
            }
        }
        function OpenLogPage(uid) {
            window.open('/Admin/Log/MemberLogList.aspx?module_name=会员日志&module=ShMember&target_id=' + uid, '_blank');
        }
        function SetDialogMemberInfo(row) {
            //$('#dlgMemberInfo .memberId').text(row.id);
            $('#dlgMemberInfo .memberName').text(row.name);
            $('#dlgMemberInfo .memberPhone').text(row.phone);
            if (row.up_user) {
                var ur = [];
                if (row.up_user.name && row.up_user.name != '  ') ur.push(row.up_user.name);
                //if (row.up_user.id) ur.push('(' + row.up_user.id + ')');
                if (row.up_user.phone) ur.push('[' + row.up_user.phone + ']');
                var ut = ur.join('');
                $('#dlgMemberInfo .memberUpUser').text(ut);
            } else {
                $('#dlgMemberInfo .memberUpUser').text('');
            }
            if (row.reg_user) {
                var ur = [];
                if (row.reg_user.name && row.reg_user.name != '  ') ur.push(row.reg_user.name);
                //if (row.reg_user.id) ur.push('(' + row.reg_user.id + ')');
                if (row.reg_user.phone) ur.push('[' + row.reg_user.phone + ']');
                var ut = ur.join('');
                $('#dlgMemberInfo .memberRegUser').text(ut);
            } else {
                $('#dlgMemberInfo .memberRegUser').text('');
            }
            $('#dlgMemberInfo .memberWay').text($.trim(row.way));
            $('#dlgMemberInfo .memberStock').text($.trim(row.stock));
            $('#dlgMemberInfo .memberLevel').text($.trim(row.lv));
            $('#dlgMemberInfo .memberStatus').text(row.status);
            $('#dlgMemberInfo .memberRegtime').text(row.reg_time);
            var str = new StringBuilder();
            if (row.ex1) {
                str.AppendFormat('<a href="{0}" target="_blank"><img alt="" class="showImg" src="{0}" /></a>', row.ex1);
            }
            if (row.other) {
                if (row.other.member_stime.indexOf('0001') < 0) {
                    $('#dlgMemberInfo .memberApplyStarttime').text(row.other.member_stime);
                } else {
                    $('#dlgMemberInfo .memberApplyStarttime').text('');
                }
                $('#dlgMemberInfo .memberPhone1').text($.trim(row.other.phone1));
                $('#dlgMemberInfo .memberAccountAmountEstimate').text(row.other.estimate);
                $('#dlgMemberInfo .memberAccumulationFund').text(row.other.fund);
                $('#dlgMemberInfo .memberTotalAmount').text(row.other.amout);
                $('#dlgMemberInfo .memberIdentityCard').text($.trim(row.other.idcard));

                var ur = [];
                if (row.other.province) ur.push(row.other.province);
                if (row.other.city) ur.push(row.other.city);
                if (row.other.district) ur.push(row.other.district);
                if (row.other.town) ur.push(row.other.town);
                var ut = ur.join(' ')
                $('#dlgMemberInfo .memberProvinceCity').text(ut);
                $('#dlgMemberInfo .memberAddress').text($.trim(row.other.address));


                $('#dlgMemberInfo .memberBankCard').html('');
                if (row.other.bank_cards.length > 0) {
                    var strCard = new StringBuilder();
                    strCard.AppendFormat('<ul>');
                    for (var i = 0; i < row.other.bank_cards.length; i++) {
                        strCard.AppendFormat('<li>');
                        strCard.AppendFormat(' {0} {1} {2}', row.other.bank_cards[i].bank_name,
                            row.other.bank_cards[i].account_name, row.other.bank_cards[i].bank_account);
                        strCard.AppendFormat('</li>');
                    }
                    strCard.AppendFormat('</ul>');
                    $('#dlgMemberInfo .memberBankCard').append(strCard.ToString());
                }

                for (var i = 2; i <= 5; i++) {
                    if (row.other['ex' + i]) {
                        str.AppendFormat('<a href="{0}" target="_blank"><img alt="" class="showImg" src="{0}" /></a>', row.other['ex' + i]);
                    }
                }
                var str1 = new StringBuilder();
                for (var i = 6; i <= 10; i++) {
                    if (row.other['ex' + i]) {
                        str1.AppendFormat('<a href="{0}" target="_blank"><img alt="" class="showImg" src="{0}" /></a>', row.other['ex' + i]);
                    }
                }
                $('#dlgMemberInfo .memberEx678910').html('');
                $('#dlgMemberInfo .memberEx678910').append(str1.ToString());
                
            }
            $('#dlgMemberInfo .memberEx12345').html('');
            $('#dlgMemberInfo .memberEx12345').append(str.ToString());
        }
        function OpenUpdateMemberShow() {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];
            SetUpdateDialogMemberInfo(row) 
            $('#dlgUpdateMemberInfo').dialog('open');
        }
        function SetUpdateDialogMemberInfo(row) {
            $('#dlgUpdateMemberInfo .txtMemberName').val(row.name);
            $('#dlgUpdateMemberInfo .txtMemberPhone1').val($.trim(row.other.phone1));
            $('#dlgUpdateMemberInfo .txtMemberIdentityCard').val($.trim(row.other.idcard));
            $('#dlgUpdateMemberInfo .txtMemberAddress').val($.trim(row.other.address));
            $('#dlgUpdateMemberInfo .txtMemberStock').val($.trim(row.stock));
            console.log('row.other',row.other);
            if (row.other) {
                curProvinceCode = row.other.province_code;
                curCityCode = row.other.city_code;
                curDistrictCode = row.other.district_code;
                curTownCode = row.other.town_code;
                GetProvinces();
                var str = new StringBuilder();
                var count = 0;
                if (row.ex1) {
                    count++;
                    str.AppendFormat('<a href="javascript:void(0)"><img alt="" class="showImg" src="{0}" /><input type="file" class="fileImg" /><img alt="" class="delImg" src="/MainStyle/Res/easyui/themes/icons/no.png" /></a>', row.ex1);
                }
                for (var i = 2; i <= 5; i++) {
                    if (row.other['ex' + i]) {
                        count++;
                        str.AppendFormat('<a href="javascript:void(0)"><img alt="" class="showImg" src="{0}" /><input type="file" class="fileImg" /><img alt="" class="delImg" src="/MainStyle/Res/easyui/themes/icons/no.png" /></a>', row.other['ex' + i]);
                    }
                }
                if (count > 0) {
                    $('#dlgUpdateMemberInfo .txtEx12345').html(str.ToString());
                } else {
                    $('#dlgUpdateMemberInfo .txtEx12345').html('');
                }
                str = new StringBuilder();
                if (count < 5) $('#dlgUpdateMemberInfo .txtEx12345').append('<a href="javascript:void(0)"><img alt="" class="showImg" /><input type="file" class="addFileImg" /><img alt="" class="addImg" src="/MainStyle/Res/easyui/themes/icons/add2.png" /></a>');
                count = 0;
                for (var i = 6; i <= 10; i++) {
                    if (row.other['ex' + i]) {
                        count++;
                        str.AppendFormat('<a href="javascript:void(0)"><img alt="" class="showImg" src="{0}" /><input type="file" class="fileImg" /><img alt="" class="delImg" src="/MainStyle/Res/easyui/themes/icons/no.png" /></a>', row.other['ex' + i]);
                    }
                }
                if (count > 0) {
                    $('#dlgUpdateMemberInfo .txtEx678910').html(str.ToString());
                } else {
                    $('#dlgUpdateMemberInfo .txtEx678910').html('');
                }
                if (count < 5) $('#dlgUpdateMemberInfo .txtEx678910').append('<a href="javascript:void(0)"><img alt="" class="showImg" /><input type="file" class="addFileImg" /><img alt="" class="addImg" src="/MainStyle/Res/easyui/themes/icons/add2.png" /></a>');
            }
        }
        function GetAreas(action, data, fn) {
            $.ajax({
                type: "Post",
                url: '/Serv/API/Mall/Area.ashx?action=' + action,
                data: data,
                dataType: "json",
                success: function (resp) {
                    if (resp.list) {
                        if (fn) fn(resp.list);
                    }
                }
            });
        }
        function selectProvince(newValue, oldValue) {
            var cCitys = _.where(citys, { pcode: newValue });
            if (cCitys.length > 0) {
                $('.sltCity').combobox('loadData', cCitys);
                $('.sltCity').combobox('setValue', curCityCode);
                return
            }
            GetAreas('Cities', { province_code: newValue }, function (list) {
                citys = citys.concat(list);
                $('.sltCity').combobox('loadData', list);
                $('.sltCity').combobox('setValue', curCityCode);
            });
        }
        function selectCity(newValue, oldValue) {
            var cDistricts = _.where(districts, { pcode: newValue });
            if (cDistricts.length > 0) {
                $('.sltDistrict').combobox('loadData', cDistricts);
                $('.sltDistrict').combobox('setValue', curDistrictCode);
                return
            }
            GetAreas('Districts', { city_code: newValue }, function (list) {
                districts = districts.concat(list);
                $('.sltDistrict').combobox('loadData', list);
                $('.sltDistrict').combobox('setValue', curDistrictCode);
            });
        }
        function selectDistrict(newValue, oldValue, code, isload) {
            var cTowns = _.where(towns, { pcode: newValue });
            if (cTowns.length > 0) {
                $('.sltTown').combobox('loadData', cTowns);
                $('.sltTown').combobox('setValue', curTownCode);
                return
            }
            GetAreas('Areas', { district_code: newValue }, function (list) {
                towns = towns.concat(list);
                $('.sltTown').combobox('loadData', list);
                $('.sltTown').combobox('setValue', curTownCode);
            });
        }
        function GetProvinces() {
            if (provinces.length > 0) {
                $('.sltProvince').combobox('setValue', curProvinceCode);
            } else {
                GetAreas('Provinces', {}, function (list) {
                    provinces = provinces.concat(list);
                    $('.sltProvince').combobox('loadData', provinces);
                    $('.sltProvince').combobox('setValue', curProvinceCode);
                });
            }
        }
        function UpdateMember() {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];
            var postData = {
                id: row.id,
                name: $.trim($('#dlgUpdateMemberInfo .txtMemberName').val()),
                phone1: $.trim($('#dlgUpdateMemberInfo .txtMemberPhone1').val()),
                idcard: $.trim($('#dlgUpdateMemberInfo .txtMemberIdentityCard').val()),
                address: $.trim($('#dlgUpdateMemberInfo .txtMemberAddress').val()),
                province_code: $.trim($('#dlgUpdateMemberInfo .sltProvince').combobox('getValue')),
                province: $.trim($('#dlgUpdateMemberInfo .sltProvince').combobox('getText')),
                city_code: $.trim($('#dlgUpdateMemberInfo .sltCity').combobox('getValue')),
                city: $.trim($('#dlgUpdateMemberInfo .sltCity').combobox('getText')),
                district_code: $.trim($('#dlgUpdateMemberInfo .sltDistrict').combobox('getValue')),
                district: $.trim($('#dlgUpdateMemberInfo .sltDistrict').combobox('getText')),
                town_code: $.trim($('#dlgUpdateMemberInfo .sltTown').combobox('getValue')),
                town: $.trim($('#dlgUpdateMemberInfo .sltTown').combobox('getText')),
                stock: $.trim($('#dlgUpdateMemberInfo .txtMemberStock').val())
            }
            var num = 1;
            $('#dlgUpdateMemberInfo .txtEx12345 .showImg').each(function () {
                var src = $.trim($(this).attr('src'));
                if (src) {
                    postData['ex' + num] = src;
                    num++;
                }
            })
            num = 6;
            $('#dlgUpdateMemberInfo .txtEx678910 .showImg').each(function () {
                var src = $.trim($(this).attr('src'));
                if (src) {
                    postData['ex' + num] = src;
                    num++;
                }
            })
            $.messager.confirm('友情提示', '确定修改 ' + row.name + '(' + row.id + ') 的信息？',
                function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: '/Serv/API/Admin/Member/ShUpdate.ashx',
                            data: postData,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#grvData').datagrid('reload');
                                    $('#dlgUpdateMemberInfo').dialog('close');
                                    $('#dlgMemberInfo').dialog('close');
                                } else {
                                    Alert(resp.msg);
                                }
                            }
                        });
                    }
                });
        }
        function OpenUpdateLoginPhoneShow() {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];
            $('#dlgUpdateLoginPhone .txtMemberPhone').val($.trim(row.phone));
            $('#dlgUpdateLoginPhone').dialog('open');
        }
        function UpdateLoginPhone() {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];

            var postData = {
                id: row.id,
                phone: $.trim($('#dlgUpdateLoginPhone .txtMemberPhone').val())
            }
            $.messager.confirm('友情提示', '<div>确定修改 ' + row.name + '[' + row.phone + '] 的登录手机为</div><div style="color:red;font-size:16px;text-algin:center;">' + postData.phone + '</div>',
                function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: '/Serv/API/Admin/Member/ShUpdateLoginPhone.ashx',
                            data: postData,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#grvData').datagrid('reload');
                                    $('#dlgUpdateLoginPhone').dialog('close');
                                    $('#dlgMemberInfo').dialog('close');
                                } else {
                                    Alert(resp.msg);
                                }
                            }
                        });
                    }
                });
        }
        function OpenUpdateDistributionOwnerShow() {
            $('#dlgUpdateDistributionOwner .txtMemberPhone').val('');
            $('#dlgUpdateDistributionOwner .memberID').text(0);
            $('#dlgUpdateDistributionOwner .memberTrueName').html('');
            $('#dlgUpdateDistributionOwner').dialog('open');
        }
        function SearchSpread(value) {
            var spreadid = $.trim(value);
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/GetSpreadUser.ashx',
                data: { spreadid: spreadid },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        $('#dlgUpdateDistributionOwner .memberID').text(resp.result.id);
                        $('#dlgUpdateDistributionOwner .memberTrueName').html('<span style="color:green">' + resp.result.name + '</span>');
                    } else {
                        $('#dlgUpdateDistributionOwner .memberID').text(0);
                        $('#dlgUpdateDistributionOwner .memberTrueName').html('<span style="color:red;">' + resp.msg + '</span>');
                    }
                }
            });
        }
        function UpdateDistributionOwner() {
            var spreadid = $.trim($('#dlgUpdateDistributionOwner .memberID').text());
            var msg = $.trim($('#dlgUpdateDistributionOwner .memberTrueName').html());
            if (!spreadid) {
                if (!msg) msg = '推荐人未找到';
                alert(msg);
                return;
            }
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];
            
            $.messager.confirm('友情提示', '确定修改推荐人为：' + msg,
                function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: '/Serv/API/Admin/Member/ShUpdateDistributionOwner.ashx',
                            data: { id: row.id, spreadid: spreadid },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#grvData').datagrid('reload');
                                    $('#dlgUpdateDistributionOwner').dialog('close');
                                    $('#dlgMemberInfo').dialog('close');
                                } else {
                                    Alert(resp.msg);
                                }
                            }
                        });
                    }
                });
        }
        function OpenCancelRegisterShow() {
            $('#dlgCancelRegister .txtContent').html('');
            $('#dlgCancelRegister').dialog('open');
        }
        function CancelRegister() {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];

            var content = $('#dlgCancelRegister .txtContent').html();
            if (content.length > 500) {
                alert("备注最多能输入500个字");
                return;
            }
            $.messager.confirm('友情提示', '<div>确定 ' + row.name + '[' + row.phone + '] 申请撤单</div><div style="color:red;text-algin:center;">注意：会禁止登陆和分佣</div>',
                function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: '/Serv/API/Admin/Flow/StartCancelRegister.ashx',
                            data: { id: row.id, flow_key: 'CancelRegister', content: content },
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#grvData').datagrid('reload');
                                    $('#dlgCancelRegister').dialog('close');
                                    $('#dlgMemberInfo').dialog('close');
                                } else {
                                    Alert(resp.msg);
                                }
                            }
                        });
                    }
                });
        }
        function OpenResetPwdShow(rowIndex) {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[rowIndex];
            $.messager.confirm('友情提示', '确定重置 ' + row.name + '(' + row.id + ') 的密码？',
                function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: '/Serv/API/Admin/User/ResetPhonePassword.ashx',
                            data: { id: row.id },
                            dataType: "json",
                            success: function (resp) {
                                Alert(resp.msg);
                            }
                        });
                    }
                });
        }
        function LockRegister() {
            var rows = $("#grvData").datagrid('getRows');
            var row = rows[curIndex];
            var btnLockText = row.is_lock == 1 ? '解锁' : '锁定';
            var is_lock = 1 - row.is_lock;
            $.messager.confirm('友情提示', '确定' + btnLockText + '会员 ' + row.name + '(' + row.id + ')？',
                 function (o) {
                     if (o) {
                         $.ajax({
                             type: "Post",
                             url: '/Serv/API/Admin/Member/ShUpdateLock.ashx',
                             data: { id: row.id, is_lock: is_lock },
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.status) {
                                     $('#grvData').datagrid('reload');
                                     $('#dlgMemberInfo').dialog('close');
                                 } else {
                                     Alert(resp.msg);
                                 }
                             }
                         });
                     }
                 });
        }
        function OpenScorePage(uid) {
            window.open('/Admin/Score/List.aspx?score_type=TotalAmount&module_name=财务明细&member=' + uid, '_blank');
        }
        function onSelectLevelSearch(record) {
            Search();
        }
        //搜索会员
        function Search() {
            searchModel.level_num = $.trim($('#sltMemberLevel').combobox('getValue'));
            searchModel.member = $.trim($('#txtMember').val());
            searchModel.up_member = $.trim($('#txtUpMember').val());
            searchModel.reg_member = $.trim($('#txtRegMember').val());
            searchModel.start = $.trim($('#txtStartTime').datebox('getValue'));
            searchModel.end = $.trim($('#txtEndTime').datebox('getValue'));
            searchModel.empty_bill = $('#chkEmptyBill').get(0).checked?1:0;
            searchModel.true_bill = $('#chkTrueBill').get(0).checked?1:0;
            searchModel.apply_pass = $('#chkApplyPass').get(0).checked?1:0;
            searchModel.apply_other = $('#chkApplyOther').get(0).checked?1:0;
            searchModel.has_img = $('#chkHasImg').get(0).checked?1:0;
            searchModel.no_img = $('#chkNoImg').get(0).checked ? 1 : 0;
            searchModel.apply_cancel = $('#chkApplyCancel').get(0).checked ? 1 : 0;
            searchModel.is_cancel = $('#chkIsCancel').get(0).checked ? 1 : 0;
            $('#grvData').datagrid('load', searchModel);
        }

        function SearchExport() {
            searchModel.level_num = $.trim($('#sltMemberLevel').combobox('getValue'));
            searchModel.member = $.trim($('#txtMember').val());
            searchModel.up_member = $.trim($('#txtUpMember').val());
            searchModel.reg_member = $.trim($('#txtRegMember').val());
            searchModel.start = $.trim($('#txtStartTime').datebox('getValue'));
            searchModel.end = $.trim($('#txtEndTime').datebox('getValue'));
            searchModel.empty_bill = $('#chkEmptyBill').get(0).checked ? 1 : 0;
            searchModel.true_bill = $('#chkTrueBill').get(0).checked ? 1 : 0;
            searchModel.apply_pass = $('#chkApplyPass').get(0).checked ? 1 : 0;
            searchModel.apply_other = $('#chkApplyOther').get(0).checked ? 1 : 0;
            searchModel.has_img = $('#chkHasImg').get(0).checked ? 1 : 0;
            searchModel.no_img = $('#chkNoImg').get(0).checked ? 1 : 0;
            searchModel.apply_cancel = $('#chkApplyCancel').get(0).checked ? 1 : 0;
            searchModel.is_cancel = $('#chkIsCancel').get(0).checked ? 1 : 0;
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/member/shlistexport.ashx',
                data: searchModel,
                dataType: 'json',
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        $('#exportIframe').attr('src', '/Serv/API/Common/ExportFromCache.ashx?cache=' + resp.result.cache);
                    } else {
                        alert('导出出错');
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
    </script>
</asp:Content>
