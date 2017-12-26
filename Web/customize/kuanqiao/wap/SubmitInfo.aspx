<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true" CodeBehind="SubmitInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Customize.kuanqiao.SubmitInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>


.box {
    width: 100%;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="box">
    <div class="header">
        <%--<img src="/img/offline_user.png" alt="">
        <h2></h2>
        <p></p>
        <a href="#" class="btn">更新头像</a>
        <div class="line"></div>--%>
        <div style="text-align:center;">
        <h1 style="font-size:16px;">喜开业</h1>
        </div>
    </div>

    <div class="prompt">
        <p>请填写您的核名信息</p>
    </div>
    <form  class="formbox" novalidate="novalidate" id="formsignin" >
        <label for="K2">申请企业名称</label>
        <input name="K2" id="K2" type="text" class="short" placeholder="必填" />
        <label style="color:#ccc;"></label>
        <label for="K3">备选企业名称</label>
        <input name="K3" id="K3" type="text" class="short" placeholder="必填" />
        <label style="color:#ccc;"></label>
        <input name="K4" id="K4" type="text" class="short" placeholder="备选企业名称2(选填)" style="display:none;"/>
        <input name="K5" id="K5" type="text" class="short" placeholder="备选企业名称3(选填)" style="display:none;"/>

        <label for="K6">注册资本（万元）</label>
        <input name="K6" id="K6" type="number" class="short" pattern="\d*"  placeholder="必填" onkeyup="this.value=this.value.replace(/\D|^0/g,'')"  />
        <label style="color:#ccc;">（例：100）</label>
        <label for="K7" style="display:none;">企业类型 (必选)</label>
        <select name="K7" id="K7" style="display:none;">
            <option value=''>请选择</option>
            <option value="有限责任公司">有限责任公司</option>
            <option value="非公司企业法人">非公司企业法人</option>
            <option value="合伙企业">合伙企业</option>
            <option value="股份有限公司">股份有限公司</option>
            <option value="企业非法人分支机构">企业非法人分支机构</option>
            <option value="个人独资企业">个人独资企业 </option>
            <option value="分公司">分公司</option>
            <option value="营业单位">营业单位</option>
            <option value="其他">其他</option>
        </select>

        <label for="K8">企业主要经营范围</label>
        <input name="K8" id="K8" type="text" class="short" placeholder="选填" />
        <label style="color:#ccc;">（例：信息科技）</label>
        <label for="K9" style="display:none;">投资人1</label>
        <input name="K9" id="K9" type="text" class="short" placeholder="名称或姓名" style="display:none;"/>
        <input name="K10" id="K10" type="text" class="wide" autocorrect="off"  placeholder="证照号码" style="display:none;" />

        <label for="K11" style="display:none;">投资人2</label>
        <input name="K11" id="K11" type="text" class="short" placeholder="如有第二个投资人请填写" style="display:none;"/>
        <input name="K12" id="K12" type="text" class="wide" autocorrect="off"  placeholder="证照号码" style="display:none;" />

        <label for="K13" style="display:none;">投资人3</label>
        <input name="K13" id="K13" type="text" class="short" placeholder="如有第三投资人请填写" style="display:none;"/>
        <input name="K14" id="K14" type="text" class="wide" autocorrect="off"  placeholder="证照号码" style="display:none;" />


        <label style="display:none;">联系人手机</label>
        <label style="display:none;">姓名</label>
        <input name="Name" id="txtName" type="text" value="未填写"  class="wide" autocorrect="off"  placeholder="姓名 (必填)" style="display:none;"/>
        <label >联系人手机</label>
        <input name="Phone" id="txtPhone" type="tel" class="wide" autocorrect="off"  placeholder="必填" onkeyup="this.value=this.value.replace(/\D|^0/g,'')"  />
        <label style="color:#ccc;">（例：18021051150）</label>
        <label style="display:none;" >电子邮箱</label>
        <input name="K1" id="K1" type="email" class="wide" autocorrect="off"  placeholder="电子邮箱 (必填)" style="display:none;" />
        <input id="activityID" type="hidden" value="130725" name="ActivityID" />        <input id="loginName" type="hidden" value="eGlrYWl5ZQ==" name="LoginName" />        <input id="loginPwd" type="hidden" value="78A#DACFD740!28#10A0##!!84547E58" name="LoginPwd" />        <input id="WXCurrOpenerOpenID" type="hidden" value="<%=WxOpenId%>" name="WeixinOpenID" />
        <input type="hidden" value="待处理" name="K15" />
        <input type="hidden" value="待处理" name="K16" />
        <input type="hidden" value="WeixinOpenID,K2,K15" name="DistinctKeys" />
        <button id="btnSignIn">免费核名提交</button>  
    </form>
</section>

<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script type="text/javascript">

    $(function () {

        $("#btnSignIn").live("click", function () {
            var Name = $.trim($("#txtName").val());
            var Phone = $.trim($("#txtPhone").val());
            var Email = $.trim($("#K1").val());
            var K2 = $.trim($("#K2").val());
            var K3 = $.trim($("#K3").val());
            var K6 = $.trim($("#K6").val()); //
            var K7 = $.trim($("#K7").val()); //企业类型
            var K8 = $.trim($("#K8").val());
            var K9 = $.trim($("#K9").val());
            var K10 = $.trim($("#K10").val());
            //            var K11 = $.trim($("#K11").val());
            //            var K12 = $.trim($("#K12").val());
            //            var K13 = $.trim($("#K13").val());
            //            var K14 = $.trim($("#K14").val());
            if (K2 == "") {
                $("#K2").focus();
                return false;
            }
            if (K3 == "") {
                $("#K3").focus();
                return false;
            }
            if (K2 == K3) {
                layermsg("备选企业名称重复");
                return false;
            }
            if (K6 == "") {
                $("#K6").focus();
                return false;
            }
            //            if (K8 == "") {
            //                $("#K8").focus();
            //                return false;
            //            }
            //            if (K7 == "") {
            //                alert("请选择企业类型");
            //                return false;
            //            }

            //            if (K9 == "") {
            //                $("#K9").focus();
            //                return false;
            //            }
            //            if (K10 == "") {
            //                $("#K10").focus();
            //                return false;
            //            }
            //            //            if (K11 == "") {
            //                $("#K11").focus();
            //                return false;
            //            }
            //            if (K12 == "") {
            //                $("#K12").focus();
            //                return false;
            //            }
            //            if (K13 == "") {
            //                $("#K13").focus();
            //                return false;
            //            }
            //            if (K14 == "") {
            //                $("#K14").focus();
            //                return false;
            //            }
            if (Name == "") {
                $("#txtName").focus();
                return false;

            }
            if (Phone == "") {
                $("#txtPhone").focus();
                return false;
            }
            //            if (Email == "") {
            //                $("#K1").focus();
            //                return false;
            //            }

            try {
                //$("#btnSignIn").html("正在提交...");
                $("#btnSignIn").attr("disabled", true);
                var option = {
                    url: "/serv/ActivityApiJson.ashx",
                    type: "post",
                    dataType: "text",
                    timeout: 60000,
                    success: function (result) {

                        $("#btnSignIn").attr("disabled", false);
                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {//清空
                            $('input:text').val("");
                            $('textarea').val("");
                            layermsg("申请提交成功，请稍后将投资人身份证正反面图片发送到微信公众号");
                            window.location.href = "/customize/kuanqiao/wap/success.htm";
//                            $.post("/Handler/App/CationHandler.ashx", { Action: "SendKeFuMsgKuanQiao" },function (result) {
//                                if (result== "1") {
//                                    window.location.href = "/customize/kuanqiao/wap/success.htm";
//                                }
//                                else {
//                                    alert("申请提交成功，请稍后将投资人身份证正反面图片发送到微信公众号 宽桥企业帮");
//                                    window.location.href = "/customize/kuanqiao/wap/success.htm";
//                                }



            // });

                        }
                        else if (resp.Status == 1) {
                            //该用户已提交过数据
                            layermsg("企业名称重复");
                            $("#btnSignIn").attr("disabled", false);
                            $("#btnSignIn").html("重新提交");

                        }
                        else {
                            layermsg(resp.Msg);
                            $("#btnSignIn").attr("disabled", false);
                            $("#btnSignIn").html("重新提交");


                        }

                    },
                    fail: function () {
                        $("#btnSignIn").attr("disabled", false);
                        layermsg("网络超时，请重新提交");
                        $("#btnSignIn").html("重新提交");

                    }
                };
                $("#formsignin").ajaxSubmit(option);
                return false;

            }
            catch (e) {
                layermsg(e);
            }

        })


    })
           
       function layermsg(msg) {
    layer.open({
        content: msg,
        btn: ['OK']
    });
}



   
</script>

</asp:Content>
