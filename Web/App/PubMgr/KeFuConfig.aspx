<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="KeFuConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.KeFuConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微客服&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>微信转发设置</span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
   <div class="ActivityBox">
        <div style="margin-bottom:10px;">
         转发微信号OPENID：<label id="lblopenid" style="font-size:16px;font-weight:bold;"><%=currWebSiteUserInfo.WeiXinKeFuOpenId %></label>
         <label style="float:right;">还没有客服?<a href="/App/PubMgr/WXKeFuManage.aspx" style="color:Blue">点击添加</a></label>
         </div>

        <table id="grvData" fitcolumns="true">
             </table>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

    //处理文件路径
    var handlerUrl = '/Handler/App/CationHandler.ashx';
    //加载文档

    $(function () {

        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryKuFuList" },
	                height: document.documentElement.clientHeight - 155,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TrueName', title: '姓名', width: 40, align: 'left' },
                                { field: 'Phone', title: '手机号', width: 35, align: 'left' },
                                { field: 'WeiXinOpenID', title: 'OpenID', width: 35, align: 'left' },
                                { field: 'op', title: '操作', width: 20, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" class=\"button button-rounded button-primary button-small\" style=\"margin-top:2px;margin-bottom:2px;margin-left:2px;margin-right:2px;font-size:12px;\" onclick="SetKeFuConfig(\'{0}\');" >设置为转发客服</a>', rowData.WeiXinOpenID);
                                    return str.ToString();
                                }
                                }


                             ]]
	            }
            );


        $("#btnSave").click(function () {

            SetKeFuConfig();


        })



    });

    //        ///配置客服
    //        function SetKeFuConfig() {
    //            var data ={ Action: "SetKeFuConfig",
    //                        KeFuOpenID: $("#txtKeFuOpenID").val()
    //                      }
    //                    jQuery.ajax({
    //                        type: "Post",
    //                        url: handlerUrl,
    //                        data:data,
    //                        success: function (result) {
    //                            Alert(result);

    //                        }
    //                    });
    //                
    //           
    //        };


    ///配置客服
    function SetKeFuConfig(openid) {
        var data = { Action: "SetKeFuConfig",
            KeFuOpenID: openid
        }
        jQuery.ajax({
            type: "Post",
            url: handlerUrl,
            data: data,
            dataType: "json",
            success: function (resp) {
                Alert(resp.Msg);
                $("#lblopenid").html(openid);

            }
        });


    };




        

       

    </script>
</asp:Content>
