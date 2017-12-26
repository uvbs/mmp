<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BookingDoctor.Detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>膏方专家预约平台</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/customize/BookingDoctor/Style/comm.css" rel="stylesheet" />
    <style>
        body {
        }

        textarea {
            width: 95%;
            height: 200px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        select {
            width: 98%;
            height: 30px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .navbar-brand {
            padding: 8px 15px;
        }

        .navbar-default {
            background-color: #00D5C9;
            border-color: #e7e7e7;
            height: 30px;
        }

        .navbar-fixed-top, .navbar-fixed-bottom {
            position: relative;
        }

        /*.return_ico {
            height: 35px;
            width: 35px;
        }*/

        .return_img {
            height: 35px;
            width: 35px;
        }

        .imglogo {
            width: 75px;
            height: 75px;
        }

        /*.logo {
            text-align: center;
            position: absolute;
            top: 10px;
            z-index: 10000;
            width: 100%;
            
        }*/

        .introtitle {
            color: #00D5C9;
            font-weight: bolder;
            margin-left: 20px;
            margin-top: 10px;
            margin-bottom: 10px;
        }

        #btnSumbit {
            background-color: #00D5C9;
            color: white;
            font-size: 15px;
            font-weight: bold;
            width: 100%;
            text-align: center;
            margin-bottom: 10px;
        }

        .bookinginfo {
            color: #00D5C9;
            text-align: center;
            margin-bottom: 10px;
        }

        .bookinginfotitle {
            color: #00D5C9;
            font-weight: bolder;
            font-size: 20px;
        }

        .remind {
            float: right;
            margin-top: 10px;
        }

        .canbooking {
            min-height: 200px;
            max-height: 280px;
            background-image: url(images/canbooking.png);
            background-repeat: no-repeat;
            position: relative;
            width: 60%;
            margin-left: auto;
            margin-right: auto;
            background-size: contain;
        }

        .cankbookingnumber {
            position: absolute;
            z-index: 1;
            width: 50px;
            height: 50px;
            top: 40%;
            left: 21%;
            font-size: 32px;
            font-weight: bold;
            letter-spacing: 3px;
            text-align: center;
        }

        .split {
            width: 100%;
        }

        .cannotbooking {
            min-height: 200px;
            max-height: 280px;
            background-image: url(images/cannotbooking.png);
            background-repeat: no-repeat;
            position: relative;
            width: 60%;
            margin-left: auto;
            margin-right: auto;
            background-size: contain;
        }

        .head {
            text-align: center;
        }

            .head img {
                width: 100%;
            }

        .info {
            margin-top: 0px;
        }

        .introdesc {
            max-width: 100%;
        }

            .introdesc img {
                width: 100%;
            }

        .navbar {
            margin-bottom: 0px;
        }

        .home {
            float: right;
            margin-top: 5px;
            margin-right: 5px;
        }

            .home img {
                width: 35px;
            }

        .bookwait {
            text-align: center;
            color: #00d5c9;
            font-size: 20px;
            margin-bottom:50px;
        }
    </style>
</head>
<body>
    <div class="container-fluid">
        <div class="">
            <nav class="navbar navbar-default navbar-fixed-top" role="navigation">
                <div class="col-sm-1 col-xs-1">
                    <div class="navbar-header">
                        <a class="navbar-brand">
                            <%--<div class="return_ico" >
                                <img src="Images/return.png" class="return_img" />
                            </div>--%>
                        </a>
                    </div>
                </div>
                <div class="col-sm-10 col-xs-10 page_title">
                </div>
                <div class="home">
                    <img src="images/home.png" onclick="window.location.href='index.aspx'" /></div>
            </nav>
            <%--            <div class="logo" >

                <img src="images/logo.png" class="imglogo" />
            </div>--%>

            <%if (Request.UrlReferrer != null)
              {%>
            <div class="return_ico" onclick="history.go(-1);">
                <img src="Images/return.png" class="return_img" />
            </div>

            <%} %>
        </div>
        <div class="head">

            <img src="<%=model.ShowImage1 %>" />


        </div>



        <div class="pLeft10 pRight10">
            <div class="info">


                <div>
                    <h3 class="introtitle">专家介绍</h3>
                    <div class="mBottom36 introdesc">

                        <%=model.PDescription %>
                    </div>

                </div>

                <%if (model.IsOnSale == "1")
                  {%>


                <%if (model.Stock > 0)//可预约
                  {%>

                <div class="canbooking">
                    <div class="cankbookingnumber" id="lblstock"><%=model.Stock %></div>
                </div>

                <% }
                  else//预约已满
                  {%>

                <div class="cannotbooking">
                </div>


                <%  } %>
                <%}
                  else
                  {%>
                <div class="bookwait">本平台即将放号，敬请期待！</div>


                <%}%>





                <%if (model.IsOnSale == "1")
                  {%>


                <div class="bookinginfo">

                    <span class="bookinginfotitle">
                        <%if (model.Stock > 0)
                          {%>
                           预约人信息
                     <% }
                          else
                          {%>
                        <span style="font-size: 13px;">预约已满,请填写下方资料，选择其它推荐专家</span>

                        <%} %>
                    
                   


                    </span>

                    <img src="images/split.png" class="split" />
                    <span class="remind">*为必填项</span>
                </div>

                <%} %>
            </div>


            <%if (model.IsOnSale == "1")
              {%>

            <form role="form" id="form">


                <div class="form-group">
                    <input type="text" class="form-control" name="Consignee" placeholder="*姓名" maxlength="20" />
                </div>
                <div class="form-group">
                    <label>性别:</label>&nbsp;
                <input type="radio" name="Ex2" value="男" id="rdoman" checked="checked" />&nbsp;<label for="rdoman">男</label>
                    &nbsp;&nbsp;
                <input type="radio" name="Ex2" value="女" id="rdowoman" />&nbsp;<label for="rdowoman">女</label>
                </div>
                <div class="form-group">
                    <input maxlength="3" type="text" class="form-control" name="Ex1" placeholder="*年龄" onkeyup="this.value=this.value.replace(/[^\.\d]/g,'');this.value=this.value.replace('.','');" />
                </div>

                <div class="form-group">
                    <input type="text" maxlength="11" class="form-control" name="Phone" placeholder="*手机" onkeyup="this.value=this.value.replace(/[^\.\d]/g,'');this.value=this.value.replace('.','');" />

                </div>

                <%
                    System.Text.StringBuilder sb = new StringBuilder();

                    foreach (var field in fieldList)
                    {
                        sb.Append(" <div class=\"form-group\">");
                        string placeholder = field.MappingName;
                        if (field.FieldIsNull == 0)
                        {
                            placeholder = "*" + placeholder;
                        }
                        switch (field.FieldType)
                        {
                            case "text"://文本框
                                if (field.IsMultiline == 0)
                                {
                                    sb.AppendFormat(" <input type=\"text\" class=\"form-control\" name=\"{0}\" placeholder=\"{1}\" maxlength=\"50\"/>", field.Field, placeholder);
                                }
                                else
                                {
                                    sb.AppendFormat(" <textarea name=\"{0}\" placeholder=\"{1}\" maxlength=\"50\"></textarea>", field.Field, placeholder);

                                }
                                break;
                            case "combox"://下拉框
                                string isReq = "";
                                if (field.FieldIsNull == 0)
                                {
                                    isReq = "*";
                                }
                                sb.AppendFormat("<select name=\"{0}\">", field.Field);
                                sb.AppendFormat("<option value=\"\">{0}请选择{1}</option>", isReq, field.MappingName);
                                if (!string.IsNullOrEmpty(field.Options))
                                {
                                    foreach (var item in field.Options.Split(','))
                                    {
                                        sb.AppendFormat("<option value=\"{0}\">{0}</option>", item);
                                    }
                                }
                                sb.Append("</select>");
                                break;
                            case "checkbox"://多选框
                                string isReqM = "";
                                if (field.FieldIsNull == 0)
                                {
                                    isReqM = "*";
                                }
                                sb.AppendFormat("<label>请选择{0}(多选)</label>", field.MappingName);
                                sb.AppendFormat("<select name=\"{0}\" multiple=\"multiple\">", field.Field);
                                sb.AppendFormat("<option value=\"\" disabled=\"\">{0}请选择{1}(多选)</option>", isReqM, field.MappingName);
                                if (!string.IsNullOrEmpty(field.Options))
                                {
                                    foreach (var item in field.Options.Split(','))
                                    {
                                        sb.AppendFormat("<option value=\"{0}\">{0}</option>", item);
                                    }
                                }
                                sb.Append("</select>");
                                break;
                            default:
                                if (field.IsMultiline == 0)
                                {
                                    sb.AppendFormat(" <input type=\"text\" class=\"form-control\" name=\"{0}\" placeholder=\"{1}\" maxlength=\"50\"/>", field.Field, placeholder);
                                }
                                else
                                {
                                    sb.AppendFormat(" <textarea name=\"{0}\" placeholder=\"{1}\" maxlength=\"50\"></textarea>", field.Field, placeholder);

                                }
                                break;
                        }

                        sb.Append("</div>");
                    }
                    Response.Write(sb.ToString());
                %>


                <%if (model.Stock <= 0)
                  {%>

                <div class="form-group">
                    <label>请选择科室</label>
                    <select name="Ex6" id="ddlEx6">

                        <%foreach (var item in categoryList)
                          {%>
                        <option value="<%=item.AutoID %>"><%=item.CategoryName %></option>
                        <%} %>
                    </select>
                </div>
                <div class="form-group">
                    <label>请选择专家(多选)</label>
                    <select name="Ex5" id="ddlEx5" multiple="multiple">
                    </select>
                </div>
                <input type="hidden" name="OrderType" value="6" />
                <%}
                  else
                  {%>

                <input type="hidden" name="OrderType" value="5" />
                <input type="hidden" name="Ex5" value="<%=model.PName %>" />
                <input type="hidden" name="Ex6" value="<%=model.CategoryName %>" />
                <% }%>


                <input type="hidden" name="id" id="hdId" value="<%=model.PID %>" />

                <div class="form-group">
                    <textarea name="Ex3" placeholder="症状描述" maxlength="1000"></textarea>
                </div>
                <button type="button" class="btn btn-default" id="btnSumbit">确定提交</button>
            </form>
            <%} %>
        </div>



    </div>



</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Ju-Modules/bootstrap/js/bootstrap.min.js"></script>
<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
<script src="/Scripts/Common.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.Min.js"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>

<script>
    var isDisable=false;

    $(function () {

        window.alert = window.Alert = function (msg) {

            layer.open({
                content: msg,
                time:0
               
            });


        };


        //提交        $("#btnSumbit").click(function () {
                        $("#btnSumbit").attr("disabled","disabled");            $("#btnSumbit").css("background-color","#ccc");            $("#form").ajaxSubmit({
                url: "Handler.ashx?Action=AddOrder",
                type: "post",
                async: false,
                dataType: "json",
                success: function (resp) {
                    if (resp.status == true) {
                        alert("预约成功后，由客服联系确认预约细节，请保持电话信息真实、畅通");
                        $("#btnSumbit").val("预约成功");
                        var stock=parseInt($("#lblstock").html());
                        stock=stock-1;
                        $("#lblstock").html(stock);
                        $("div[index]").removeAttr("id");
                        setTimeout("window.location.href='index.aspx'",5000);


                    }
                    else {
                        alert(resp.msg);
                        $("#btnSumbit").removeAttr("disabled");
                        $("#btnSumbit").css("background-color","#00D5C9");


                    }
                   
                }
            });
        })
        //科室下拉框改变
        $("#ddlEx6").change(function () {

            $.ajax({
                type: 'post',
                url: "Handler.ashx",
                data: { Action: "GetDoctorList", categoryId: $(this).val(),currcategoryid:"<%=model.CategoryId%>",tags:"<%=model.Tags%>" },
                dataType: "json",
                success: function (resp) {
                    $("#ddlEx5").html("");
                    if (resp.result != null && resp.result.length > 0) {
                        var str = new StringBuilder();
                        for (var i = 0; i < resp.result.length; i++) {

                            str.AppendFormat("<option value=\"{0}\">{1}</option>", resp.result[i].PID, resp.result[i].PName);


                        }


                        $("#ddlEx5").append(str.ToString());


                    }
                    else {

                        $("#hdId").val("");


                    }



                }
            });




        })

        $("#ddlEx5").change(function () {

            $("#hdId").val($(this).val());


        })

        var stock=<%=model.Stock %>;
        if (stock<=0) {
            $("#hdId").val("");
        }

        var categoryId="<%=categoryId%>";
        if (categoryId!="") {
           
            $(ddlEx6).val(categoryId);
            $(ddlEx6).change();
        }




    });

</script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title:"<%=config.WebsiteTitle%>",
                desc: "<%=config.WebsiteDescription%>",
                //link: '', 
                imgUrl: "<%=model.ShowImage1%>"
            })
        })
</script>
</html>
