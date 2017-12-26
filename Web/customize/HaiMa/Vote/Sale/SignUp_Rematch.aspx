<%@ Page Title="" Language="C#" MasterPageFile="~/customize/HaiMa/Vote/Sale/Master.Master"
    AutoEventWireup="true" CodeBehind="SignUp_Rematch.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Vote.Sale.SignUp_Rematch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    海马真英雄-复赛
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .col
        {
            padding: 0px;
        }
        select
        {
            border-radius: 2px;
            min-width: 200px;
            height: 30px;
        }
        .bottom
        {
            margin-top: 30px;
            margin-bottom: 20px;
        }
        .bottom h1
        {
            color: White;
            text-align: center;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapUserCenter mBottom48">
        <div class="header">
            <img src="images/signuprematch.png" />
        </div>
        <form id="formsignin">
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>姓名：</span>
                            <input type="text" value="" name="Name" id="txtName" value="<%=CurrentUserInfo.TrueName %>">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>手机：</span>
                            <input type="text" value="" name="Phone" id="txtPhone" value="<%=CurrentUserInfo.Phone %>">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>身份证号码：</span>
                            <input type="text" value="" name="K1" id="txtK1" value="<%=CurrentUserInfo.Ex8%>">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>性别：</span>
                            <select id="ddlK2" name="K2">
                                <option value="">请选择 </option>
                                <option value="男">男 </option>
                                <option value="女">女 </option>
                            </select>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>年龄：</span>
                            <input type="text" value="" id="txtK3" name="K3">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>民族：</span>
                            <input type="text" value="" id="txtK4" name="K4">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>常用邮箱：</span>
                            <input type="text" value="" id="txtK5" name="K5">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>身高(cm/厘米):</span>
                            <input type="text" value="" id="txtK6" name="K6">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>体重(kg/公斤)：</span>
                            <input type="text" value="" id="txtK7" name="K7">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>血型：</span>
                            <select id="ddlK8" name="K8">
                                <option value="">请选择 </option>
                                <option value="O型">O型 </option>
                                <option value="A型">A型 </option>
                                <option value="B型">B型 </option>
                                <option value="AB型">AB型 </option>
                            </select>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>T恤尺寸：</span>
                            <select id="ddlK9" name="K9">
                                <option value="">请选择 </option>
                                <option value="S">S </option>
                                <option value="M">M </option>
                                <option value="L">L </option>
                                <option value="XL">XL </option>
                                <option value="XXL">XXL </option>
                            </select>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>出发城市：</span>
                            <input type="text" value="" id="txtK10" name="K10">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>返回城市：</span>
                            <input type="text" value="" id="txtK11" name="K11">
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>交通方式：</span>
                            <select id="ddlK12" name="K12">
                                <option value="">请选择 </option>
                                <option value="飞机">飞机</option>
                                <option value="火车">火车</option>
                                <option value="大巴">大巴</option>
                                <option value="自驾">自驾 </option>
                            </select>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span>备注</span><br />
                            <textarea id="txtK13" name="K13"></textarea>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <input id="activityID" type="hidden" value="447400" name="ActivityID" /><input id="loginName" type="hidden" value="aGFpbWE=" name="LoginName" /><input id="loginPwd" type="hidden" value="#57#0!99AF8BE5CFB578F89C1A478!CC" name="LoginPwd" />
        </form>
        <div class="wrapBtn">
            <a href="javascript:;" id="btnSumbit">确定提交</a>
        </div>
        <div class="bottom">
            <h1>
                您的报名信息我们将严格保密，感谢您的参与。
            </h1>
            
            <h1>
                本次竞赛一切解释权归海马汽车销售有限公司所有。</h1>
            <br />
             <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#btnSumbit").live("click", function () {

                var model = {
                    Name: $.trim($(txtName).val()),
                    Phone: $.trim($(txtPhone).val()),
                    K1: $("#txtK1").val(),
                    K2: $("#ddlK2").val(),
                    K3: $("#txtK3").val(),
                    K4: $("#txtK4").val(),
                    K5: $("#txtK5").val(),
                    K6: $("#txtK6").val(),
                    K7: $("#txtK7").val(),
                    K8: $("#ddlK8").val(),
                    K9: $("#ddlK9").val(),
                    K10: $("#txtK10").val(),
                    K11: $("#txtK11").val(),
                    K12: $("#ddlK12").val(),
                    K13: $("#txtK13").val()

                }

                if (model.Name == "" || model.Phone == "" || model.K1 == "" || model.K2 == "" || model.K3 == "" || model.K4 == "" || model.K5 == "" || model.K6 == "" || model.K7 == "" || model.K8 == "" || model.K9 == "" || model.K10 == "" || model.K11 == "" || model.K12 == "") {
                    layermsg("请填写完整信息");
                    return false;
                }


                try {

                    var option = {
                        url: "/serv/ActivityApiJson.ashx",
                        type: "post",
                        dataType: "json",
                        success: function (resp) {

                            if (resp.Status == 0) {
                                layermsg("提交成功!系统将于8月5日与您确认报名信息");
                                return;

                            }
                            else if (resp.Status == 1) {
                                //该用户已提交过数据
                                layermsg("您已经提交过资料了!");
                            }
                            else {
                                layermsg(resp.Msg);

                            }

                        }
                    };
                    $("#formsignin").ajaxSubmit(option);
                    return false;

                }
                catch (e) {
                    alert(e);
                }



            });


        });
    </script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "海马精英营销大赛复赛报名!",
                desc: "海马精英营销大赛复赛报名!",
                link: '',
                imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
            })
        })
    </script>
</asp:Content>
