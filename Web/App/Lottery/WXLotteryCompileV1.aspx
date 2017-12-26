<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXLotteryCompileV1.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.WXLotteryCompileV1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="Stylesheet" href="css/admincomm.css?v=20161014" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="/App/Lottery/WXLotteryMgrV1.aspx">抽奖活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>新建抽奖</span>
    <a href="<%=backUrl %>" style="float: right; margin-right: 20px; color: Black;"
        title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <span class="colorRed">*</span>抽奖活动名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtLotteryName" value="" style="width: 100%;" placeholder="抽奖活动名称(必填)" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"><span class="colorRed">*</span>微信分享描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareDesc" value="" style="width: 100%;" placeholder="将显示在微信分享描述中(选填)" />
                    </td>
                </tr>
                  
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">微信分享缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="/App/Lottery/wap/images/ggl.jpg" width="100px;" id="imgshareimg" /><br />
                        <a id="a2" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="fileshareimg.click()">上传缩略图</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为100*100。
                        <input type="file" id="fileshareimg" name="fileshareimg" style="display: none;" />
                    </td>
                </tr>
                  <tr class="wrapImgThumbnailsPath">
                    <td colspan="2">
                       
                    </td>
                </tr>
                <tr class="wrapImgThumbnailsPath">
                    <td style="width: 100px;" align="right" class="tdTitle">活动大图：
                    </td>
                    <td width="*" align="left">
                        <img alt="" src="/App/Lottery/wap/images/logo.png" width="30%" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();" style="display: none;">随机缩略图</a>
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为750*580。
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                   <tr class="wrapImgThumbnailsPath">
                    <td colspan="2">
                       
                    </td>
                </tr>
                <tr class="wrapBackGroundColor">
                    <td style="width: 100px;"  align="right" class="tdTitle">背景色：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBackGroundColor" value="#fc4236" style="width: 100px;"
                            placeholder="选填" />
                        <input type="button" id="colorpicker" value="选择背景色" />
                    </td>
                </tr>
                  <tr class="wrapBackGroundColor">
                    <td colspan="2">
                       
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            介绍：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                  <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"><span class="colorRed">*</span>抽奖开始时间：
                    </td>
                    <td width="*" align="left">
                        <input class="easyui-datetimebox" style="width: 150px;" id="txtStartTime" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">抽奖结束时间：
                    </td>
                    <td width="*" align="left">
                        <input class="easyui-datetimebox" style="width: 150px;" id="txtEndTime" />
                    </td>
                </tr>
              
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">手动设置状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdostatus" id="rdostart" checked="checked" value="1" /><label
                            for="rdostart">进行中</label>
                        <input type="radio" name="rdostatus" id="rdostop" value="0" /><label for="rdostop">已停止</label>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />已停止的活动不能抽奖（手动停止后忽略设置的停止时间）
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" valign="top" class="tdTitle">抽奖方式：
                    </td>
                    <td width="*" align="left">
                        <label>
                            <input type="radio" name="luckLimitType" value="0" id="rdoLuckLimitTypeAll" checked />  每人
                        </label>                        
                        <label>
                            <input type="radio" name="luckLimitType" value="1" id="rdoLuckLimitTypeDay"/> 每人每天 
                        </label>
                        <input type="number" class="easyui-numberbox" style="width: 60px; margin-left: 12px;"  id="txtMaxCount"/> 次
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="left">
                        <br />
                        免费次数抽完之后，消耗 <input type="number" class="easyui-numberbox" style="width: 60px;" id="txtUsePoints" /> 积分可兑换一次抽奖机会( 0 或者空，则不允许积分兑换抽奖机会)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">中奖方式：
                    </td>
                    <td width="*" align="left">
                        <label>
                            <input type="radio" name="winLimitType" id="rdowinLimitType1" checked="checked" value="1" />
                            允许多次中奖
                        </label>
                        
                        <label>
                            <input type="radio" name="winLimitType" id="rdowinLimitType0" value="0" />
                            不允许多次中奖
                        </label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">自定义奖品领奖：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdogetprizetype" id="rdogetprizefrommobile" checked="checked"
                            value="1" />
                        <label for="rdogetprizefrommobile">
                            现场领奖</label>
                        <input type="radio" name="rdogetprizetype" value="0" id="rdogetprizefromback" /><label
                            for="rdogetprizefromback">后台设置领奖</label>

                        <input type="radio" name="rdogetprizetype" value="2" id="rdogetprizefromsumbitinfo" /><label
                            for="rdogetprizefromsumbitinfo">现场提交信息领奖</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                
                <tr class="wrapToolBarButtonSet">
                    <td style="width: 100px;" align="right" class="tdTitle">底部工具栏：
                    </td>
                    <td width="*" align="left">
                        <select id="txtToolBarButton" style="width:150px;">
                        </select>
                    </td>
                </tr>
                <tr class="wrapToolBarButtonSet">
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
            </table>
            
            <h3>奖品设置</h3>
            <div class="question" id="wrapLotteryAwards">
                <table style="width: 100%; margin-left: 10px;">
                    <th>奖品类型
                    </th>
                    <th>奖品
                    </th>
                    <th>自定义名称
                    </th>
                    <th>自定义说明
                    </th>                    
                    <th>中奖概率
                    </th>
                    <th>数量
                    </th>
                    <th></th>
                    <tr v-for="(item, $index) in awards">
                        <td align="center">
                            <select class="lotteryAwardsType txtCenter" v-model="item.AwardsType">
                                <option value="0">自定义</option>
                                <option value="2">优惠券</option>
                                <option value="1">积分</option>
                            </select>
                        </td>
                           <td>
                            <div class="lotteryValue txtCenter">
                                
                                <input type="text" v-model="item.Value" name="lotteryScore" placeholder="请输入积分" v-if="item.AwardsType == 1" />

                                <%--<select class="" v-if="item.AwardsType == 2" v-model="item.Value">
                                    <option value="1">优惠券1</option>
                                    <option value="2">优惠券2</option>
                                    <option value="3">优惠券3</option>
                                </select>--%>

                                <a href="javascript:;" title="{{item.ValueName}}" v-if="item.AwardsType == 2" @click="selectCardCoupon(item)">
                                    <span v-show="!item.ValueName">请选择优惠券</span>
                                    <span v-show="item.ValueName">{{item.ValueName}}</span>
                                </a>

                                <span v-if="item.AwardsType == 0">自定义</span>


                            </div>
                        </td>
                        <td>
                            <input type="text" name="lotteryname" placeholder="奖品名称(必填)" v-model="item.PrizeName" />
                        </td>
                         <td>
                            <input type="text" name="lotteryDescription" placeholder="奖品说明"  v-model="item.Description"/>
                        </td>
                        <td>
                            <input class="" type="text" name="lotteryproportion" placeholder="中奖概率(1-100)"  v-model="item.Probability"/>%
                        </td>
                        <td>
                            <input class="" type="text" name="lotterycount" placeholder="奖品数量(必填)"  v-model="item.PrizeCount"/>
                        </td>
                        <td>
                            <img src="/img/delete.png" width="20" height="20" class="deleteanswer" @click="deleteItem($index)" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <a class="button button-rounded button-primary" style="margin: 20px 0;" @click="add" >添加奖项</a>
                        </td>
                    </tr>
                </table>
            </div>
        
                 <div class="wrapOperate">
                     <a href="javascript:;" id="btnSave" class="button button-rounded button-primary">保存</a> 
                     <a href="javascript:;" id="btnReset" class="button button-rounded button-flat">重置</a>
                 </div>
        </div>
    </div>


    <div id="wrapCardCouponSelect" style="display:none;">
        <table id="grvCardCouponData" fitcolumns="true">
        </table>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/lotteryCompile.js" type="text/javascript"></script>
    <script type="text/javascript">
        var lotteryType = GetParm("lotteryType");//空和1为刮奖，2为抽奖
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var editor;
        var lavm = new Vue({
            el: '#wrapLotteryAwards',
            data: {
                awards: [{
                    PrizeCount: 0,
                    PrizeName: '',
                    Probability: 0,
                    AwardsType: 0,
                    Value: '',
                    ValueName:'',
                    Description:''
                }],
                currEditAward: {}
            },
            methods: {
                add: function () {
                    this.awards.push({
                        PrizeCount: 0,
                        PrizeName: '',
                        Probability: 0,
                        AwardsType: 0,
                        Value: '',
                        ValueName: '',
                        Description: ''
                    });
                },
                deleteItem: function ($index) {
                    this.awards.RemoveIndexOf($index);
                },
                selectCardCoupon(item) {
                    this.currEditAward = item;

                    layer.open({
                        type: 1,
                        scrollbar:false,
                        offset: '100px',
                        area: ['620px', '430px'],
                        title:'请选择优惠券',
                        content: $('#wrapCardCouponSelect')
                    });
                    LoadCardCouponData();
                }
            }
        });//LotteryAwardsVM  奖项组件
        
        //var cardCouponVM = new Vue({
        //    el: '#wrapCardCouponSelect',
        //    data: {

        //    },
        //    methods: {
                
        //    }
        //});


        $(function () {

            ProcessPageElementShow(lotteryType,true);

            var select = $("#txtToolBarButton");
            select.empty();
            $.ajax({
                type: 'post',
                url: "/Serv/API/Admin/CompanyWebsite/ToolBar/grouplist.ashx",
                dataType: "json",
                success: function (resp) {
                    for (var i = 0; i < resp.result.length; i++) {
                        var option = $("<option  value=" + resp.result[i] + ">" + resp.result[i] + "</option>")
                        select.append(option);
                    }
                }
            });

            $('#btnSave').click(function () {
                try {
                    var model = GetModel();
                    var JsonData = JSON.stringify(model);
                    if (model.LotteryName == '') {
                        Alert('请输入抽奖活动名称！');

                        return;
                    }
                    if (model.MaxCount == '') {
                        Alert('请输入每个用户最多抽奖次数！');
                        return;
                    }
                    if (model.MaxCount < 0) {
                        Alert('用户最多抽奖次数需大于等于0');
                        return;
                    }
                    if (model.StartTime == "") {
                        Alert('请选择抽奖开始时间');
                        return;
                    }
                    if (model.EndTime == "") {
                        Alert('请选择抽奖结束时间');
                        return;
                    }
                    if (model.Awards.length == 0) {
                        Alert('请至少添加一个奖项');
                        return;
                    }
                    var checkresult = true;
                    //$("input[name='lotteryname']").each(function () {
                    //    if ($.trim($(this).val()) == "") {
                    //        checkresult = false;
                    //        $(this).focus();
                    //    }
                    //})
                    //$("input[name='lotterycount']").each(function () {
                    //    if ($.trim($(this).val()) == "") {

                    //        checkresult = false;
                    //        $(this).focus();

                    //    }
                    //})
                    //lotteryScore
                    $("input[name='lotteryproportion'],input[name='lotterycount'],input[name='lotteryname'],input[name='lotteryScore']").each(function () {
                        if ($.trim($(this).val()) == "") {
                            checkresult = false;
                            $(this).focus();
                        }
                    })
                    if (checkresult == false) {
                        return false;
                    }
                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: { Action: "AddWXLotteryV1", JsonData: JsonData },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                alert("添加成功");
                                window.location = "<%=backUrl%>";
                            }
                            else {
                                alert(resp.Msg);
                            }
                        },
                        complete: function () {
                            $.messager.progress('close');
                        }

                    });

                } catch (e) {
                    Alert(e);
                }


            });

            $('#btnReset').click(function () {
                ResetCurr();

            });

            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }

            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=LotteryImage',
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

            $("#fileshareimg").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=LotteryImage&filegroup=fileshareimg',
                         secureuri: false,
                         fileElementId: 'fileshareimg',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#imgshareimg').attr('src', resp.ExStr);
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

            $("input[name='lotteryproportion'],input[name='lotterycount'],input[name='lotteryScore']").live("keyup", function () {

                $(this).val($(this).val().replace(/\D/g, ''));

            })

        });

        var loadCardCouponDataDone = false;

        function LoadCardCouponData() {
            if (loadCardCouponDataDone) {
                $('#grvCardCouponData').datagrid('fixColumnSize');
                return;
            }
            loadCardCouponDataDone = true;
            ///serv/api/admin/mall/cardcoupon.ashx?action=list&pageindex=1&pagesize=10&cardcoupon_type=
            $('#grvCardCouponData').datagrid({
                url: '/serv/api/admin/mall/cardcoupon.ashx',
                queryParams: {
                    action: 'list',
                    cardcoupon_type:''
                },
                height: 385,
                //width:600,
                pagination: true,
                singleSelect:true,
                columns: [[
                    { field: 'cardcoupon_id', title: '编号', width: 20 },
                    { field: 'cardcoupon_type', title: '类型', width: 20 },
                    { field: 'cardcoupon_name', title: '名称', width: 100 },
                    { field: 'valid_to', title: '有效期', width: 50 },
                    {
                        field: 'opt', title: '操作', width: 20, formatter: function (value,row,index) {
                            var str = new StringBuilder();

                            str.AppendFormat('<a href="javascript:;" onclick="SelectCardcouponToAward({0},\'{1}\')"  style="color:blue">选择</a>',
                                row.cardcoupon_id,
                                row.cardcoupon_name
                                );

                            return str.ToString();
                        }
                    }
                ]]
            });

        }

        function SelectCardcouponToAward(id,name) {
            lavm.$data.currEditAward.Value = id;
            lavm.$data.currEditAward.ValueName = name;
            layer.closeAll();
        }

        function ResetCurr() {
            $(":input[type!=radio]").val("");
            editor.html("");

        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
            var colorpicker;
            K('#colorpicker').bind('click', function (e) {
                e.stopPropagation();
                if (colorpicker) {
                    colorpicker.remove();
                    colorpicker = null;
                    return;
                }
                var colorpickerPos = K('#colorpicker').pos();
                colorpicker = K.colorpicker({
                    x: colorpickerPos.x,
                    y: colorpickerPos.y + K('#colorpicker').height(),
                    z: 19811214,
                    selectedColor: 'default',
                    noColor: '无颜色',
                    click: function (color) {
                        K('#txtBackGroundColor').val(color);
                        colorpicker.remove();
                        colorpicker = null;
                    }
                });
            });
            K(document).click(function () {
                if (colorpicker) {
                    colorpicker.remove();
                    colorpicker = null;
                }
            });




        });

        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
        }

        function GetModel() {
            var model =
                    {
                        LotteryType: lotteryType == '2' ? "shake" : "scratch",
                        ToolBarButton:$("#txtToolBarButton").val(),
                        ThumbnailsPath: $('#imgThumbnailsPath').attr('src'),
                        LotteryName: $.trim($('#txtLotteryName').val()),
                        LotteryContent: editor.html(),
                        Status: $("input[name=rdostatus]:checked").val(),
                        LuckLimitType: $("input[name=luckLimitType]:checked").val(),
                        MaxCount: $.trim($('#txtMaxCount').val()),
                        BackGroundColor: $.trim($('#txtBackGroundColor').val()),
                        StartTime: $('#txtStartTime').datetimebox('getValue'),
                        EndTime: $('#txtEndTime').datetimebox('getValue'),
                        ShareImg: $("#imgshareimg").attr("src"),
                        ShareDesc: $("#txtShareDesc").val(),
                        IsGetPrizeFromMobile: $("input[name=rdogetprizetype]:checked").val(),
                        Awards: lavm.$data.awards,
                        WinLimitType: $("input[name=winLimitType]:checked").val(),
                        UsePoints: $('#txtUsePoints').val()
                    };

            return model;

        }

    </script>
</asp:Content>
