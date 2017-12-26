<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="MyAddressList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.MyAddressList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    我的收货地址</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <style>
        .h1, h2, h4, h5, h6
        {
            clear: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="m_listbox">
        <a href="AddressinfoCompile.aspx?action=add" class="list">
            <span class="mark green"><span class="icon add"></span></span>
            <h2>添加收货地址</h2>
            <span class="righticon"></span>
        </a>
    </div>

        <%
            ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
            List<ZentCloud.BLLJIMP.Model.WXConsigneeAddress> addressList = bllMall.GetConsigneeAddressList(currentUserInfo.UserID);
            if (addressList.Count > 0)
            {
                for (int i = 0; i < addressList.Count; i++)
                {
                    Response.Write(string.Format("<div class=\"m_addressbox \" data-id=\"{0}\">", addressList[i].AutoID));
                    Response.Write(string.Format("<span class=\"name\"><span class=\"nameinfo\">{0}</span></span>", addressList[i].ConsigneeName));
                    Response.Write(string.Format("<span class=\"phone\">{0}</span>", addressList[i].Phone));
                    Response.Write(string.Format("<span class=\"address\"> <span class=\"addressinfo\">{0}</span></span>", addressList[i].Address));
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
                Response.Write("<h2>您还没有添加过收货地址</h2>");
                Response.Write("</div>");

            }
            
          %>



        <%if (addressList.Count > 0)
          { %>
          <span class="btn orange" id="delicon">删除收货地址</span>
        <%} %>

</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerurl = "/Handler/App/WXMallHandler.ashx";
        var selectid = "";

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
        <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
</asp:Content>
