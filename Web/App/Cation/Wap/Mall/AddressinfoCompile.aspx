<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="AddressinfoCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.AddressinfoCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<%
    ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
    ZentCloud.BLLJIMP.Model.WXConsigneeAddress AddressInfo = new ZentCloud.BLLJIMP.Model.WXConsigneeAddress();
    if (!string.IsNullOrEmpty(Request["id"]))
    {
        
    }
        
 %>

<section class="box">
    <div class="m_personinfo">
        <label for="txtConsigneeName"><span class="title">收货人</span><input type="text"  id="txtConsigneeName" placeholder="收货人姓名" value="<%=AddressInfo.ConsigneeName %>"></label>
        <label for="txtPhone"><span class="title">联系方式</span><input type="tel" id="txtPhone" placeholder="手机号码" value="<%=AddressInfo.Phone %>"></label>
        <label for="txtAddress"><span class="title">地址</span><input type="text" id="txtAddress" placeholder="收货地址" value="<%=AddressInfo.Address %>"></label>
        <a href="MyAddressList.aspx" class="btn orange">取消</a><a id="btnSave" href="javascript:void(0)" class="btn orange">保存</a>
    </div>

    <div class="backbar">
        <a href="MyAddressList.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/WXMallHandler.ashx";
    var action = GetParm("action");
    var selectid = GetParm("id");
    $('#btnSave').click(function () {
        var model =
                    {
                        AutoID: selectid,
                        ConsigneeName: $.trim($('#txtConsigneeName').val()),
                        Action: action == 'add' ? 'AddWXConsigneeAddress' : 'EditWXConsigneeAddress',
                        Address: $.trim($('#txtAddress').val()),
                        Phone: $('#txtPhone').val()


                    };

        if (model.ConsigneeName == '') {
            $('#txtConsigneeName').focus();
            return;
        }
        if (model.Address == '') {
            $('#txtAddress').focus();
            return;
        }
        if (model.Phone == '') {
            $('#txtPhone').focus();
            return;
        }


        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: model,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 1) {
                    alert("保存成功");
                    window.location.href = "MyAddressList.aspx";

                }
                else {
                    alert(resp.Msg);
                }
            }
        });




    });

    //获取get参数
    function GetParm(parm) {
        //获取当前URL
        var local_url = window.location.href;

        //获取要取得的get参数位置
        var get = local_url.indexOf(parm + "=");
        if (get == -1) {
            return "";
        }
        //截取字符串
        var get_par = local_url.slice(parm.length + get + 1);
        //判断截取后的字符串是否还有其他get参数
        var nextPar = get_par.indexOf("&");
        if (nextPar != -1) {
            get_par = get_par.slice(0, nextPar);
        }
        return get_par;
    }
</script>
</asp:Content>

