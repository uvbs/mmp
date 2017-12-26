<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="Personinfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Personinfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">个人资料</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">
    <div class="m_personinfo">
        <label for="txtName"><span class="title">姓名</span><input id="txtName" type="text" value="<%=userInfo.TrueName==null?"":userInfo.TrueName%>" ></label>
        <label for="ddlgender">
            <span class="title">性别</span>            
            <select name="" id="ddlgender">
                <option value="">选择性别</option>
                <option value="1">男</option>
                <option value="0">女</option>
            </select></label>
        <label for="txtPhone"><span class="title">联系方式</span><input  type="tel" id="txtPhone" value="<%=userInfo.Phone==null?"":userInfo.Phone%>" ></label>
        <label for="txtEmail"><span class="title">邮箱</span><input type="email" id="txtEmail" value="<%=userInfo.Email==null?"":userInfo.Email%>"></label>
       
        <a href="MyCenter.aspx" class="btn orange">取消</a><a id="btnSave" href="javascript:(0)" class="btn orange">保存</a>
    </div>

    <div class="backbar">
        <a href="MyCenter.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

    var redirecturl = GetParm("redirecturl");
    $(function () {

        var gender = "<%=userInfo.Gender%>";


        if ($.trim(gender) == "1") {
            $("#ddlgender").val("1");
        }
        else if ($.trim(gender) == "0") {
            $("#ddlgender").val("0");
        }
        else {
            $("#ddlgender").val("");
        }



        $("#btnSave").click(function () {

            try {

                var model = {
                    Action: 'EditUserInfoV1',
                    Name: $.trim($('#txtName').val()),
                    Phone: $.trim($('#txtPhone').val()),
                    Email: $.trim($('#txtEmail').val()),
                    Gender: $.trim($('#ddlgender').val()),
                    AddressArea: ""

                }

         
                    if (model.Name == "") {
                        $('#txtName').focus();
                        return false;
                    }
                    if (model.Phone == "") {
                        $('#txtPhone').focus();
                        return false;
                    }
                    if (model.Email == "") {
                        $('#txtEmail').focus();
                        return false;
                    }
                    if (model.Gender == "") {
                        //alert("请选择性别");
                        msgText.init("请选择性别", 3000);
                        return false;
                    }
                    


                

                $.ajax({
                    type: 'post',
                    url: '/Handler/User/UserHandler.ashx',
                    data: model,
                    dataType:"json",
                    success: function (resp) {
                        if (resp.Status == 1) {
                            if (redirecturl != "") {
                                window.location.href = redirecturl;
                            }
                            else {
                                //alert("保存成功");
                                msgText.init("保存成功", 3000);
                            }
                        }
                        else {
                            //alert(resp.Msg);
                            msgText.init(resp.Msg, 3000);
                        }


                    }

                });


            } catch (e) {
                //alert(e);
                msgText.init(e, 3000);
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