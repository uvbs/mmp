<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="MyAddressList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.MyAddressList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">我的收货地址</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">

        <%
            if (AddressList.Count>0)
            {
                for (int i = 0; i < AddressList.Count; i++)
                {
                    Response.Write(string.Format("<div class=\"m_addressbox \" data-id=\"{0}\">", AddressList[i].AutoID));
                    Response.Write(string.Format("<span class=\"name\"><span class=\"nameinfo\">{0}</span></span>", AddressList[i].ConsigneeName));
                    Response.Write(string.Format("<span class=\"phone\">{0}</span>", AddressList[i].Phone));
                    Response.Write(string.Format("<span class=\"address\">地址 : <span class=\"addressinfo\">{0}</span></span>", AddressList[i].Address));
                    Response.Write("<span class=\"icon\"></span>");
                    Response.Write("<a href=\"javascript:void(0)\" class=\"deladdress\">");
                    Response.Write("<span class=\"delbox\"><span class=\"delicon\"></span></span>");
                    Response.Write("</a>");
                    Response.Write("</div>");


                }
            }
            else
            {
                Response.Write(string.Format("<div class=\"m_listbox \">"));
                Response.Write("<h2>您还没有收货地址</h2>");
                Response.Write("</div>");

            }
            
          %>


    <div class="m_listbox">
        <a href="AddressinfoCompile.aspx?action=add" class="list">
            <span class="mark green"><span class="icon add"></span></span>
            <h2>添加收货地址</h2>
            <span class="righticon"></span>
        </a>
    </div>

    <div class="backbar">
        <a href="MyCenter.aspx" class="back"><span class="icon"></span></a>
        <%if (AddressList.Count>0){ %>
          <span class="btn orange" id="delicon">删除收货地址</span>
        <%} %>

    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerurl = "/Handler/App/WXMallHandler.ashx";
     var selectid = "";
     document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
         WeixinJSBridge.call('hideToolbar');
     });
     $(function () {
         $("#delicon").click(function () {
             if ($(this).text() === "删除收货地址") {
                 $(this).text("取消");
                 $(".m_addressbox").addClass("m_deladdress");
             } else {
                 $(this).text("删除收货地址");
                 $(".m_deladdress").removeClass("m_deladdress");
             }
         })

         $(".deladdress").click(function () {
             selectid = $(this).closest(".m_addressbox").attr("data-id");
             //删除收货地址
             if (confirm("确认删除?")) {
                 $.ajax({
                     type: 'post',
                     url: handlerurl,
                     data: { Action: 'DeleteWXConsigneeAddress', id: selectid },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             //alert("删除成功");
                             $(".m_addressbox[data-id]").each(function () {
                                 if ($(this).attr("data-id") == selectid) {
                                     $(this).remove();
                                 }

                             })
                             if ($(".m_addressbox[data-id]").length == 0) {
                                 $("#delicon").remove();
                                 $(".m_listbox").before("<div class=\"m_listbox \"><h2>您还没有收货地址</h2></div>");
                             }

                         }
                         else {
                             
                             msgText.init("删除失败", 1000);
                         }
                     }
                 });


             }
             else {

             }


         })
         //点击 进入编辑页面
         $(".name,.phone,.address,.icon").click(function () {

             window.location.href = "AddressinfoCompile.aspx?action=edit&id=" + $(this).closest(".m_addressbox").attr("data-id");

         })
     })

 </script>
</asp:Content>