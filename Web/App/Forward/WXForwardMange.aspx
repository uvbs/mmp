﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXForwardMange.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.WXForwardMange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>所有转发活动</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
               onclick="Delete()">删除</a>&nbsp;
              <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                onclick="ShowDistributionQRCode()">生成分销员二维码</a>
            <br />
            活动名称:<input id="txtName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Searchkey();">查询</a>
            <a href="javascript:;" onclick="ShowQRcode()" style="color:blue;">获取转发中心链接</a>&nbsp;&nbsp;
            是否显示排行榜:
            <input type="radio" id="rdoShow" class="positionTop2" value="1" name="rank" /><label for="rdoShow">显示</label>
            <input type="radio" id="rdoHide" class="positionTop2" value="0" name="rank" /><label for="rdoHide">隐藏</label>
            &nbsp; &nbsp; &nbsp; &nbsp;
            排行榜类型:
            <input type="radio" id="rdoSignCount" class="positionTop2" value="0" name="sort" /><label for="rdoSignCount">带来报名人数</label>
            <input type="radio" id="rdoReadCount" class="positionTop2" value="1" name="sort" /><label for="rdoReadCount">带来的阅读量</label>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
     <div id="dlgDistributionOwner" class="easyui-dialog" closed="true" title="" data-options="iconCls:'icon-tip'" modal="true" style="width: 675px; padding: 15px;">
          姓名:<input type="text" id="txtKeyWord" style="width: 150px;" 
placeholder="姓名" />
               <select id="txtDistributionOwner">
            <option value="0">全部</option>
            <option value="1">渠道</option>
            <option value="2">分销员</option>
        </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">
                查询</a>
        <br />
           <br />
        <table id="grvDistributionOwnerData" fitcolumns="true">
        </table>
    </div>
    <div id="dlgDisQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="QRcodeImg" width="220" height="220" />
        <br />
        <a id="textUrl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXForwardHandler.ashx";
     var domain =  '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
     var isshowRank = '<%=model.IsShowForwardRank %>';
     var sortType = '<%=model.SortType%>';
     var activityId = '0';
     var forwardType = '';
     if (isshowRank==1) {
         rdoShow.checked=true;
     }else {
         rdoHide.checked=true;
     }

     if (sortType == 0) {
         rdoSignCount.checked = true;
     } else {
         rdoReadCount.checked = true;
     }
    

     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetForwarList" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ActivityName', title: '活动名称', width: 20, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看二维码" href="javascript:" onclick="ShowQRcode(\'{0}\')">{1}</a>', rowData.ActivityId, rowData.ActivityName);
                                    return str.ToString();
                                }
                                },
                                { field: 'ForwardType', title: '转发类型', width: 10, align: 'left' },
                                { field: 'ForwarNum', title: '转发人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (rowData.ForwarNum == 0) {
                                        str.AppendFormat("{0}", rowData.ForwarNum);
                                    } else {
                                        if (rowData.ForwardType == '问卷') {
                                            str.AppendFormat('<a class="listClickNum" title="点击查看转发人数" href="WxQuestionnaireUserInfo.aspx?id={0}&Mid={1}" >{2}</a>', rowData.ActivityId, rowData.Mid, rowData.ForwarNum);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" title="点击查看转发人数" href="WXForwarUserInfo.aspx?id={0}&Mid={1}" >{2}</a>', rowData.ActivityId, rowData.Mid, rowData.ForwarNum);
                                        }
                                        
                                    }
                                    return str.ToString();
                                }
                                },
                                { field: 'UV', title: '微信阅读人数',sortable:true, width: 10, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (rowData.UV == '' || rowData.UV == null) {
                                        str.AppendFormat('{0}', 0);
                                    } else {
                                        str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1" title="点击查看统计详情">{1}</a>', rowData.ActivityId, rowData.UV);
                                    }
                                    return str.ToString();
                                }
                                },
                                {
                                    field: 'IPPV', title: 'IP/PV',sortable:true,width: 10, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (value == 0) {
                                            str.AppendFormat("{0}", 0);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={2}" title="点击查看统计详情">{0}/{1}</a>', rowData.IP, rowData.PV, rowData.ActivityId);
                                        }
                                        
                                        return str.ToString();
                                    }
                                },
                                { field: 'InsertDate', title: '创建时间', width: 20, align: 'left', formatter: FormatDate },
                             ]]
	            }
            );

         //是否显示排行榜
         $("input[name=rank]").click(function () {
             var rank = $(this).val();
             $.ajax({
                 type: "Post",
                 url: handlerUrl,
                 data: { Action: "UpdateWebsiteForwardRank", IsShowForwardRank: rank },
                 dataType: "json",
                 success: function (resp) {
                     if (resp.Status == 0) {
                         $('#grvData').datagrid('reload');
                     }
                     else {
                         Alert(resp.Msg);
                     }
                 }

             });
         });
         $("input[name=sort]").click(function () {
             var sort = $(this).val();
             $.ajax({
                 type: "Post",
                 url: handlerUrl,
                 data: { Action: "UpdateWebsiteForwardRank", sort_type: sort },
                 dataType: "json",
                 success: function (resp) {
                     if (resp.Status == 0) {
                         $('#grvData').datagrid('reload');
                     }
                     else {
                         Alert(resp.Msg);
                     }
                 }

             });
         });
         //分销员列表
         //$('#grvDistributionOwnerData').datagrid(
         //    {
         //        method: "Post",
         //        url: "/Handler/App/CationHandler.ashx",
         //        queryParams: { Action: "QueryWebsiteUserDistributionOnLine" },
         //        height: 300,
         //        pagination: true,
         //        striped: true,
         //        pageSize: 10,
         //        rownumbers: true,
         //        singleSelect:true,
         //        rowStyler: function () { return 'height:25px'; },
         //        columns: [[
         //                       { title: 'ck', width: 5, checkbox: true },
         //                     { field: 'AutoID', title: '编号', width: 50, align: 'left', formatter: FormatterTitle },
         //                       {
         //                           field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
         //                               if (value == '' || value == null)
         //                                   return "";
         //                               var str = new StringBuilder();
         //                               str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
         //                               return str.ToString();
         //                           }
         //                       },
         //                       { field: 'WXNickname', title: '微信昵称', width: 60, align: 'left', formatter: FormatterTitle },
         //                       { field: 'TrueName', title: '真实姓名', width: 80, align: 'left', formatter: FormatterTitle },
         //                       { field: 'Phone', title: '手机', width: 90, align: 'left', formatter: FormatterTitle },
         //                        { field:'QRCodeUrl', title: '专属链接', width: 330, align: 'left', formatter: function(value,rowData){
         //                            var str=new StringBuilder();
         //                            str.AppendFormat('<a href="javascript:;" onclick=ShowDisQRCode("{1}","{2}") title="点击弹出二维码">http://{0}/{1}/{2}/details.chtml</a>',domain,toHex(activityId),toHex(rowData.AutoID));
         //                            return str.ToString();
         //                        } }
         //        ]]
         //    }
         //);

         //$("#dlgDistributionOwner").dialog({
         //    buttons: [{
         //        text: '生成',
         //        handler: function () {
                    
         //            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         //            var rows1=$('#grvDistributionOwnerData').datagrid('getSelections');
         //            if(rows1.length<=0){
         //                $.messager.alert("系统提示","请选择分销员!","warning");
         //                return;
         //            }
         //            $('#dlgDistributionOwner').dialog('close');
                    
         //            ShowDisQRCode(rows[0].ActivityId,rows1[0].AutoID);
         //        }
         //    }, {
         //        text: '取消',
         //        handler: function () {
         //            $('#dlgDistributionOwner').dialog('close');
         //        }
         //    }]

         //});

         $(document).keydown(function(event){ 
             if(event.keyCode==13){
                 Search();
             }
         });


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
                     data: { Action: "DeleteForwar", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 0) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });


     }

     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].ActivityId
                 );
         }
         return ids;
     }


     function Searchkey() {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetForwarList", ActivityName: $("#txtName").val() }
	            });
     }



     function ShowQRcode() {
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Forward/wap/allforwardlistwap.aspx');
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/Forward/wap/allforwardlistwap.aspx";
         $("#alinkurl").html(linkurl).attr("href", linkurl);
     }
     //生成分销员二维码
     function ShowDistributionQRCode(){
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         if(rows.length>1){
             $.messager.alert('系统提示', "只能选择一条数据进行操作！", "warning");
             return
         }
         $('#dlgDistributionOwner').dialog({ title: '分销员列表' });
         $('#dlgDistributionOwner').dialog('open');
         activityId = rows[0].ActivityId;
         forwardType = rows[0].ForwardType;
         ShowDisDataGrid();
     }
     function ShowDisDataGrid(){
         $('#grvDistributionOwnerData').datagrid(
            {
                method: "Post",
                url: "/Handler/App/CationHandler.ashx",
                queryParams: { Action: "QueryWebsiteUserDistributionOnLine" },
                height: 300,
                pagination: true,
                striped: true,
                pageSize: 10,
                rownumbers: true,
                singleSelect:true,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                               { title: 'ck', width: 5, checkbox: true },
                             { field: 'AutoID', title: '编号', width: 50, align: 'left', formatter: FormatterTitle },
                               {
                                   field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                       if (value == '' || value == null)
                                           return "";
                                       var str = new StringBuilder();
                                       str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                       return str.ToString();
                                   }
                               },
                               { field: 'WXNickname', title: '微信昵称', width: 60, align: 'left', formatter: FormatterTitle },
                               { field: 'TrueName', title: '真实姓名', width: 80, align: 'left', formatter: FormatterTitle },
                               { field: 'Phone', title: '手机', width: 90, align: 'left', formatter: FormatterTitle },
                              
                                {
                                    field: 'QRCodeUrl', title: '专属链接', width: 330, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if(forwardType=='活动'){
                                        str.AppendFormat('<a href="javascript:;" onclick=ShowDisQRCode("{1}","{2}","{3}") title="点击弹出二维码">http://{0}/{1}/{2}/details.chtml</a>', domain, toHex(activityId), toHex(rowData.AutoID),forwardType);
                                    } else {
                                        str.AppendFormat('<a href="javascript:;" onclick=ShowDisQRCode("{1}","{2}","{3}")  title="点击弹出二维码">http://{0}/App/Questionnaire/wap/Questionnaire.aspx?id={1}&&uid={2}</a>', domain, activityId, rowData.UserID, forwardType);
                                    }
                                    return str.ToString();
                                } }
                ]]
            }
        );
     }

     function Search(){
         $('#grvDistributionOwnerData').datagrid(
                   {
                       method: "Post",
                       url: "/Handler/App/CationHandler.ashx",
                       queryParams: { Action: "QueryWebsiteUserDistributionOnLine", keyword: $(txtKeyWord).val(),  isDistributionOwner: $(txtDistributionOwner).val() }
                   });
     }

     //生成分销员二维码
     function ShowDisQRCode(aid, autoid,type) {
         var linkurl = '';
         if (type=='活动') {
              linkurl = "http://" + domain + "/" + aid + "/" +autoid+"/"+"details.chtml";
         } else {
             linkurl = "http://" + domain + "/App/Questionnaire/wap/Questionnaire.aspx?id=" + aid + "&&uid=" + autoid;
         }
        

         $.ajax({
             type: 'post',
             url: "/Handler/QCode.ashx",
             data: { code: linkurl },
             success: function (result) {
                 $("#QRcodeImg").attr("src", result);
             }
         });
         $('#dlgDisQRCode').dialog('open');
         $("#textUrl").html(linkurl);
         $("#textUrl").attr("href", linkurl);
     }
    
     //转16进制
     function toHex(num){
         var rs = "";
         var temp;
         while(num/16 > 0){
             temp = num%16;
             rs = (temp+"").replace("10","a").replace("11","b").replace("12","c").replace("13","d").replace("14","e").replace("15","f") + rs;
             num = parseInt(num/16);
         }
         return rs;
     }
  
 </script>
</asp:Content>
