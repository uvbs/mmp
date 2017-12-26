<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Action.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Flow.Action" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .upload-img-list .upload-img-item{
            display:inline;
            width:90px;
            height:90px;
            padding:5px;
            position: relative;
        }
        .upload-img-list .upload-img-item img{
            width:80px;
            height:80px;
        }
        .upload-img-list .upload-img-item input[type=file]{
            position: absolute;
            width: 80px;
            height: 80px;
            left: 5px;
            opacity:0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=module_name %>
    <a href="/Admin/Flow/List.aspx?flow_key=<%=flow_key %>&module_name=<%=module_name %>&hide_status=<%=hide_status %>" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="FlowAction" style="width: 100%; height: 100%;">
        <table class="tbFlowAction" style="width: 100%;">
            <tr>
                <td class="tdLeft" style="width: 800px;">
                    <div class="flowFrom" style="padding: 10px 15px 30px 10px; overflow-y: auto; overflow-x: hidden; height: 100%;">
                        <table class="flowFromTable" style="width:100%; border: solid #d3d3d3 1px; border-collapse: collapse;display:none;">
                            <tr>
                                <td class="FlowName" colspan="2" style="text-align: center; font-size: 24px; line-height: 40px; border: solid #d3d3d3 1px; padding: 5px;">线下充值</td>
                            </tr>
                            <tr>
                                <td class="FlowMemberName" style="width: 50%; font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">会员：颂和测试03</td>
                                <td class="FlowAmount" style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">金额：10000</td>
                            </tr>
                            <tr class="FlowTrueAmount">
                                <td class="DeductAmount" style="width: 50%; font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">扣税：100</td>
                                <td class="TrueAmount" style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">到账：9900</td>
                            </tr>
                            <tr>
                                <td class="FlowCreateUser" style="width: 50%; font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">提交人：颂和测试03</td>
                                <td class="FlowCreateDate" style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">提交时间：2016年12月17日 14:01</td>
                            </tr>
                            <tr class="FlowEx">
                                <td colspan="2" style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">充值渠道：POS机</td>
                            </tr>
                            <tr class="FlowContent">
                                <td colspan="2" style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; height: 52px; padding: 5px; vertical-align: top; word-break:break-all;">备注：线下充值10000元
                                </td>
                            </tr>
                            <tr class="FlowFiles">
                                <td colspan="2">
                                    <div class="FlowFileDiv" style="display: inline-block; padding: 5px;">
                                        <a href="http://open-files.comeoncloud.net/www/hf/jubit/image/20161209/3DF388A18EE9473B88445C78276BB12F.png" target="_blank" style="display: inline-block;">
                                            <img src="http://open-files.comeoncloud.net/www/hf/jubit/image/20161209/3DF388A18EE9473B88445C78276BB12F.png" style="width: 80px; height: 80px;" />
                                        </a>
                                    </div>
                                </td>
                            </tr>
                            <tr class="FlowStep">
                                <td colspan="2" style="font-size: 18px; line-height: 21px; background-color: #f9f9f9; border: solid #d3d3d3 1px; min-height: 52px; padding: 5px;">财务审核
                                </td>
                            </tr>
                            <tr class="FlowHandle">
                                <td class="FlowHandleUser" style="width: 50%; font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">审核人：小强</td>
                                <td class="FlowHandleDate" style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">审核时间：2016年12月17日 14:01</td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td class="tdRight" style="vertical-align: top;">
                    <div style="padding: 10px 15px 30px 10px; overflow-y: auto; overflow-x: hidden; height: 100%;">
                        <table style="width: 100%; border: solid #d3d3d3 1px; border-collapse: collapse;">
                            <tr>
                                <td style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">
                                    会员编号：<span class="sp_member_id"></span><br />
                                    会员姓名：<span class="sp_member_name"></span><br />
                                    会员手机：<span class="sp_member_phone"></span><br />
                                    会员级别：<span class="sp_levelname"></span>
                                </td>
                            </tr>
                            <tr class="trHandleDate" style="display:none;">
                                <td style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;"><span class="spSelectDateName">到账时间：</span><input id="txtDate" class="easyui-datetimebox" showseconds="false" />
                                </td>
                            </tr>
                            <tr class="trHandle" style="display:none;">
                                <td style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">备注：<br />
                                    <div id="txtContent" contenteditable="true" style="width: 96%;line-height: 21px;min-height: 73px;padding: 5px;border: solid #d3d3d3 1px;">
                                    </div>
                                </td>
                            </tr>
                            <%--<tr class="trHandle" style="display:none;">
                                <td style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">
                                    照片：<br />
                                    <div class="upload-img-list">
                                        <div class="upload-img-item">
                                            <img src="/App/Wap/img/upload.jpg" id="img1" class="imgUploadShowImg" />
                                            <input class="imgUpload" type="file" data-img-id="img1" accept="image/jpeg;image/png;image/gif;"/>
                                        </div>
                                        <div class="upload-img-item">
                                            <img src="/App/Wap/img/upload.jpg" id="img2" class="imgUploadShowImg" />
                                            <input class="imgUpload" type="file" data-img-id="img2" accept="image/jpeg;image/png;image/gif;"/>
                                        </div>
                                        <div class="upload-img-item">
                                            <img src="/App/Wap/img/upload.jpg" id="img3" class="imgUploadShowImg" />
                                            <input class="imgUpload" type="file" data-img-id="img3" accept="image/jpeg;image/png;image/gif;"/>
                                        </div>
                                    </div>
                                </td>
                            </tr>--%>
                            <tr class="trHandleEx1" style="display:none;">
                                <td style="font-size: 16px; line-height: 21px; border: solid #d3d3d3 1px; padding: 5px;">
                                    <input id="ckHandleEx1" type="checkbox" class="positionTop3" /><label class="lblHandleEx1" for="ckHandleEx1">退还报单金额</label>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-size: 16px; line-height: 21px; text-align: center; border: solid #d3d3d3 1px; padding: 5px;">
                                    <a href="javascript:void(0);" id="btnAuditPass" class="easyui-linkbutton" iconcls="icon-start" style="display:none;" onclick="AuditPass(1,'确认审核通过？');">通过</a>
                                    <a href="javascript:void(0);" id="btnAuditNoPass" class="easyui-linkbutton" iconcls="icon-stop" style="display:none;" onclick="AuditPass(2,'确认审核不通过？',true);">不通过</a>
                                    <a href="javascript:void(0);" id="btnAuditCancel" class="easyui-linkbutton" iconcls="icon-start" style="display:none;" onclick="AuditPass(3,'确认同意取消？');">同意取消</a>
                                    <a href="javascript:void(0);" id="btnAuditNoCancel" class="easyui-linkbutton" iconcls="icon-stop" style="display:none;" onclick="AuditPass(4,'确认拒绝取消？',true);">拒绝取消</a>
                                    <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-print" onclick="printForm();">打印</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/flow/';
        var flow_key = "<%=flow_key%>";
        var module_name = "<%=module_name %>";
        var hide_status = "<%=hide_status %>";
        var id = "<%=id %>";
        var action_data;
        var empty_Table;
        var flow_ex;
        var flow_content;
        var flow_files;
        var flow_file_div;
        var flow_step;
        var flow_handle;
        $(function () {
            $('.tbFlowAction').height($('.right_centent').height()-50);
            bindUpload();
            GetEmptyTable();
            GetData();
        });
        function bindUpload() {
            $(".imgUpload").on('change', function () {
                var ob = $(this);
                var obimg = $(this).attr('data-img-id');
                try {
                    $.ajaxImgUpload({
                        url: '/Serv/API/Common/File.ashx',
                        data:{action:'Add',dir:'image'},
                        maxWidth: 800,
                        secureuri: false,
                        fileElement: ob,
                        dataType: 'json',
                        success: function (resp) {
                            if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                                $("#" + obimg).attr("src", resp.file_url_list[0]);
                            } else {
                                alert(resp.errmsg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });
        }
        function GetEmptyTable() {
            flow_file_div = $('.flowFromTable .FlowFileDiv').clone();
            $('.flowFromTable .FlowFileDiv').remove();
            flow_files = $('.flowFromTable .FlowFiles').clone();
            $('.flowFromTable .FlowFiles').remove();
            flow_ex = $('.flowFromTable .FlowEx').clone();
            $('.flowFromTable .FlowEx').remove();
            flow_content = $('.flowFromTable .FlowContent').clone();
            $('.flowFromTable .FlowContent').remove();
            flow_step = $('.flowFromTable .FlowStep').clone();
            $('.flowFromTable .FlowStep').remove();
            flow_handle = $('.flowFromTable .FlowHandle').clone();
            $('.flowFromTable .FlowHandle').remove();

            empty_Table = $('.flowFromTable').clone();
            $('.flowFromTable').remove();
        }
        function printForm() {
            $('.sort').css('display', 'none');
            $('.centent_r_btm').css('border', '0px');
            $('.tdRight').css('display', 'none');
            $('.tdLeft').css('width', '100%');
            $('body').css('background-color', '#ffffff');
            window.print();
            $('.sort').css('display', 'block');
            $('.centent_r_btm').css('border', '1px #BEBEBE solid');
            $('.tdRight').css('display', 'table-cell');
            $('.tdLeft').css('width', '800px');
            $('body').css('background-color', '#f4f4f4');
        }
        function GetData() {
            $.ajax({
                type: "Post",
                url: '/serv/api/admin/flow/get.ashx',
                data: { id: id },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        action_data = resp.result;
                        if (action_data.can_act) {
                            if (action_data.status == 11) {
                                $('#btnAuditCancel').show();
                                $('#btnAuditNoCancel').show();
                            } else {
                                $('#btnAuditPass').show();
                                if (flow_key != 'PerformanceReward' && flow_key != "EmptyBilFill") {
                                    $('#btnAuditNoPass').show();
                                }
                            }
                            $('.trHandle').show();
                            if (action_data.stepname == '财务审核' && (flow_key == "RegisterOffLine" || flow_key == "OfflineRecharge" || flow_key == "OfflineUpgrade" ||
                                flow_key == "Withdraw" || flow_key == "PerformanceReward")) {
                                if (flow_key == "Withdraw" ) $('.spSelectDateName').text('打款时间：');
                                if (flow_key == "PerformanceReward") $('.spSelectDateName').text('发放时间：');
                                $('.trHandleDate').show();
                            }
                        }
                        BuildForm();
                    }
                }
            });
        }
        function BuildForm() {
            var show_table = $(empty_Table).clone();
            $(show_table).show();
            $(show_table).find('.FlowName').text(action_data.flowname);
            $(show_table).find('.FlowMemberName').text('会员：' + action_data.member_name);
            if (flow_key == 'CancelRegister') {
                $(show_table).find('.FlowAmount').text('账面余额：' + action_data.amount);
            } else if (flow_key == 'PerformanceReward') {
                $(show_table).find('.FlowAmount').text('奖金：' + action_data.amount);
            } else {
                $(show_table).find('.FlowAmount').text('金额：' + action_data.amount);
            }

            if (action_data.create_user_name == '系统') {
                $(show_table).find('.FlowCreateUser').text('提交人：管理员');
            } else {
                $(show_table).find('.FlowCreateUser').text('提交人：' + action_data.create_user_name);
            }
            $(show_table).find('.FlowCreateDate').text('提交时间：' + new Date(action_data.start).format('yyyy年MM月dd日 hh:mm'));
            if (action_data.up) {
                var uphandle = $(flow_handle).clone();
                $(uphandle).find('.FlowHandleUser').text('推荐人：' + action_data.up.name);
                $(uphandle).find('.FlowHandleDate').text('推荐人手机：' + action_data.up.phone);
                $(show_table).append(uphandle);
            }

            if (flow_key == 'CancelRegister' && action_data.reg) {
                $('.trHandleEx1').show();
                var reghandle = $(flow_handle).clone();
                $(reghandle).find('.FlowHandleUser').text('报单人：' + action_data.reg.name);
                $(reghandle).find('.FlowHandleDate').text('报单人手机：' + action_data.reg.phone);
                $(show_table).append(reghandle);

                var reghandle1 = $(flow_handle).clone();
                $(reghandle1).find('.FlowHandleUser').text('报单金额：' + action_data.reg.amount);
                $(reghandle1).find('.FlowHandleDate').text('报单时间：' + action_data.member_regtime);
                $(show_table).append(reghandle1);
            }
            else if (flow_key == 'PerformanceReward') {
                $('.lblHandleEx1').text("发放到余额");
                $('.trHandleEx1').show();
            }

            $('.sp_member_id').text(action_data.member_id);
            $('.sp_member_name').text(action_data.member_name);
            $('.sp_member_phone').text(action_data.member_phone);
            $('.sp_levelname').text(action_data.lvname);
            if (flow_key == 'Withdraw') {
                $('.spSelectDateName').text('打款时间：');
            }
            else if (flow_key == 'CancelRegister') {
                $('.spSelectDateName').text('退款时间：');
            }
            
            if (flow_key == 'Withdraw' && action_data.deduct_amount > 0) {
                $(show_table).find('.FlowTrueAmount .DeductAmount').text('扣税金额：' + action_data.deduct_amount);
                $(show_table).find('.FlowTrueAmount .TrueAmount').text('实际金额：' + action_data.true_amount);
            } 
            else if (flow_key == 'CancelRegister') {
                $(show_table).find('.FlowTrueAmount .DeductAmount').text('账面公积金：' + action_data.deduct_amount);
                $(show_table).find('.FlowTrueAmount .TrueAmount').text('可用余额：' + action_data.true_amount);
            }
            else if (flow_key == 'PerformanceReward') {
                $(show_table).find('.FlowTrueAmount .DeductAmount').text('公积金：' + action_data.deduct_amount);
                $(show_table).find('.FlowTrueAmount .TrueAmount').text('开票金额：' + action_data.true_amount);
            }
            else if (flow_key == 'OfflineUpgrade') {
                $(show_table).find('.FlowTrueAmount .DeductAmount').text('充值渠道：' + action_data.ex1);
                $(show_table).find('.FlowTrueAmount .TrueAmount').text('升级级别：' + action_data.ex3);
            } else {
                $(show_table).find('.FlowTrueAmount').remove();
            }
            if (flow_key != 'RegisterEmptyBill' && (action_data.status == 8 || action_data.status == 9 || action_data.status == 10)) {
                var rstatus = $(flow_ex).clone();
                var rstatustext = '通过';
                if (action_data.status == 8) {
                    rstatustext = '<span style="color:red;">未通过</span>';
                } else if (action_data.status == 10) {
                    rstatustext = '<span style="color:red;">已取消</span>';
                } else if (action_data.status == 10) {
                    rstatustext = '<span style="color:red;">拒绝取消</span>';
                }
                rstatustext = '处理结果：' + rstatustext;
                $(rstatus).find('td').html(rstatustext);
                $(show_table).append(rstatus);
            }
            var act_ex1_txt = $.trim(action_data.ex1);
            if (act_ex1_txt != '' && (flow_key == 'OfflineRecharge' || flow_key == 'RegisterOffLine' || flow_key == 'Withdraw' || flow_key == 'PerformanceReward')) {
                var act_ex1 = $(flow_ex).clone();
                if (flow_key == 'Withdraw') {
                    act_ex1_txt = '开户银行：' + act_ex1_txt;
                } else if (flow_key == 'PerformanceReward') {
                    act_ex1_txt = '月份：' + act_ex1_txt;
                } else {
                    act_ex1_txt = '充值渠道：' + act_ex1_txt;
                }
                $(act_ex1).find('td').text(act_ex1_txt);
                $(show_table).append(act_ex1);
            }
            var act_ex2_txt = $.trim(action_data.ex2);
            if (act_ex2_txt != '' &&  flow_key == 'Withdraw') {
                var act_ex2 = $(flow_ex).clone();
                act_ex2_txt = '开户号：' + act_ex2_txt;
                $(act_ex2).find('td').text(act_ex2_txt);
                $(show_table).append(act_ex2);
            }
            var act_ex3_txt = $.trim(action_data.ex3);
            if (act_ex3_txt != '' && flow_key == 'Withdraw') {
                var act_ex3 = $(flow_ex).clone();
                act_ex3_txt = '银行卡号：' + act_ex3_txt;
                $(act_ex3).find('td').text(act_ex3_txt);
                $(show_table).append(act_ex3);
            }
            var act_content_txt = $.trim(action_data.content);
            if (act_content_txt != '') {
                var act_content = $(flow_content).clone();
                var act_content_txt = act_content_txt;
                //var act_content_txt = '备注：<br />' + act_content_txt;
                $(act_content).find('td').html(act_content_txt);
                $(show_table).append(act_content);
            }
            if (action_data.handles.length > 0 && action_data.handles[0].files.length > 0) {
                var sfiles = $(flow_files).clone();
                for (var i = 0; i < action_data.handles[0].files.length; i++) {
                    var sfurl = $.trim(action_data.handles[0].files[i].url);
                    if (sfurl != '') {
                        var sfilediv = $(flow_file_div).clone();
                        $(sfilediv).find('img').attr('src', sfurl);
                        $(sfilediv).find('a').attr('href', sfurl);
                        $(sfiles).find('td').append(sfilediv);
                    }
                }
                $(show_table).append(sfiles);
            }
            for (var i = 1; i < action_data.handles.length; i++) {
                var rh = action_data.handles[i];
                var rex1 = $.trim(rh.ex1);
                var rstepname = $.trim(rh.stepname);
                var rcontent = $.trim(rh.content);
                var rselect_date = $.trim(rh.select_date);
                if (i != 0) {
                    var rstep = $(flow_step).clone();
                    $(rstep).find('td').text(rstepname);
                    $(show_table).append(rstep);
                }
                if (rex1 != '') {
                    var rex1 = $(flow_ex).clone();
                    var ex1text = rh.ex1;
                    $(rex1).find('td').text(ex1text);
                    $(show_table).append(rex1);
                }
                if (rstepname = '财务审核' && rselect_date != '' && rselect_date.indexOf('0001') < 0) {
                    var rdt = $(flow_ex).clone();
                    var rdttext = new Date(rh.select_date).format('yyyy年MM月dd日 hh:mm');
                    if(flow_key == 'Withdraw'){
                        rdttext = '打款时间：' + rdttext;
                    } if (flow_key == 'CancelRegister') {
                        rdttext = '退款时间：' + rdttext;
                    } else {
                        rdttext = '到账时间：' + rdttext;
                    }
                    $(rdt).find('td').text(rdttext);
                    $(show_table).append(rdt);
                }
                if (rcontent != '') {
                    var rct = $(flow_content).clone();

                    var rcttext = rh.content;
                    //if (rh.stepname == '申请取消') {
                    //    rcttext = '申请原因：<br />' + rh.content;
                    //} else {
                    //    rcttext = '备注：<br />' + rh.content;
                    //}
                    $(rct).find('td').html(rcttext);
                    $(show_table).append(rct);
                }
                if (rh.files && rh.files.length && rh.files.length > 0) {
                    var rfiles = $(flow_files).clone();
                    for (var j = 0; j < rh.files.length; j++) {
                        var rfurl = $.trim(rh.files[j].url);
                        if (rfurl != '') {
                            var rfilediv = $(flow_file_div).clone();
                            $(rfilediv).find('img').attr('src', rfurl);
                            $(rfilediv).find('a').attr('href', rfurl);
                            $(rfiles).find('td').append(rfilediv);
                        }
                    }
                    $(show_table).append(rfiles);
                }
                if (i != 0) {
                    var event = rh.stepname.indexOf('申请') >= 0 ? '申请' : '审核';
                    var rhandle = $(flow_handle).clone();
                    $(rhandle).find('.FlowHandleUser').text(event + '人：' + rh.handle_user_name);
                    $(rhandle).find('.FlowHandleDate').text(event + '时间：' + new Date(rh.handle_time).format('yyyy年MM月dd日 hh:mm'));
                    $(show_table).append(rhandle);
                }
            }
            $(empty_Table).find('.FlowCreateDate').text(new Date(action_data.start).format('yyyy年MM月dd日 hh:mm'));
            $('.flowFrom').append(show_table);
        }
        function AuditPass(num, msg, red) {
            if (flow_key == 'CancelRegister' && $('#ckHandleEx1').get(0).checked) {
                msg += '并退还报单金额？';
            }
            if (red) msg = '<span style="color:red;">' + msg + '</span>';
            $.messager.confirm("系统提示", msg, function (r) {
                if (r) {
                    PostAction(num);
                }
            });
        }
        function AuditNoPass() {
            $.messager.confirm("系统提示", '<span style="color:red;">确定不通过审核？</span>', function (r) {
                if (r) {
                    PostAction(2);
                }
            });
        }
        function PostAction(num) {
            var content = $.trim($('#txtContent').html());
            if (content.length > 500) {
                alert("备注最多能输入500个字");
                return;
            }
            var files = [];
            //$('.imgUploadShowImg').each(function () {
            //    var file_url = $.trim($(this).attr('src')).toLocaleLowerCase();
            //    if (file_url != '' && file_url != '/app/wap/img/upload.jpg') {
            //        files.push(file_url);
            //    }
            //});

            var postData = {
                action_id: id,
                content: content,
                files: files.join(','),
                audit:num
            };
            var select_date = $.trim($('#txtDate').datetimebox('getValue'));
            if (select_date != '' && (new Date(select_date).getFullYear() == select_date.substr(0, 4))) {
                postData.select_date = select_date;
            }
            if (flow_key == 'CancelRegister' || flow_key == 'PerformanceReward') {
                postData.ex1 = $('#ckHandleEx1').get(0).checked ? 1 : 0;
            }
            $.ajax({
                type: "Post",
                url: '/serv/api/admin/flow/action.ashx',
                data: postData,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        var icon = 'info';
                        if(resp.msg.indexOf('失败')>=0) icon = 'warning';
                        $.messager.alert("系统提示", resp.msg,icon,function(){
                            window.location.href = '/Admin/Flow/List.aspx?flow_key=' + flow_key + '&module_name=' + module_name + '&hide_status=' + hide_status;
                        });
                    }
                    else{
                        $.messager.alert("系统提示", resp.msg,'error');
                    }
                }
            });
        }

    </script>
</asp:Content>
