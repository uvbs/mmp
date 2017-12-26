<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysAddVoteCount.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.BalanceVoteCount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        input[type='text']
        {
            height: 30px;
            border-radius: 5px;
        }
        input[type='button']
        {
            
         width:100px;
         height:30px;   
         }
    </style>
</head>
<body>
    <div>
        随机投票
        <hr />
        投票ID
        <input type="text" id="txtVoteId" value="127" />
        <table>
            <tr>
                <td>
                    增加票数范围
                </td>
                <td>
                    <input type="text" id="txtAddCountFrom" value="1" />票到<input type="text" id="txtAddCountTo" value="1" />票 增加票数随机
                        
                </td>
            </tr>
            <tr>
                <td>
                    选手票数当前范围
                </td>
                <td>
                    <input type="text" id="txtVoteCountFrom" />票 到<input type="text" id="txtVoteCountTo" />票
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="button" value="投票" id="btnSumbit" />
                </td>
            </tr>
        </table>
        <hr />
        
        <div style="height:20px;"></div>
        给指定选手投票
        <hr />
        <table>
            <tr>
                <td>
                    选手编号
                </td>
                <td>
                    <input type="text" id="txtNumber" />
                </td>
            </tr>
            <tr>
                <td>
                    增加票数
                </td>
                <td>
                    <input type="text" id="txtAddCount" value="1" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="button" value="投票" id="btnSumbit1" />
                </td>
            </tr>
        </table>
    </div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script type="text/javascript">

    $(function () {

        //随机投票
        $("#btnSumbit").click(function () {


            var model = { Action: 'SysAddvoteCountRand',
                VoteID: $("#txtVoteId").val(),
                AddCountFrom: $("#txtAddCountFrom").val(),
                AddCountTo: $("#txtAddCountTo").val(),
                VoteCountFrom: $("#txtVoteCountFrom").val(),
                VoteCountTo: $("#txtVoteCountTo").val()
            }
            if (model.AddCountFrom == "") {
                alert("票数下限必填");
                return false;
            }
            if (model.AddCountTo == "") {
                alert("票数上限必填");
                return false;
            }
            if (model.VoteCountFrom == "") {
                alert("选手票数下限必填");
                return false;
            }
            if (model.AddCountTo == "") {
                alert("选手票数上限必填");
                return false;
            }
            $("#btnSumbit").val("正在处理。。。。");
            $.ajax({
                type: 'post',
                url: "/customize/beachhoney/handler.ashx",
                data: model,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        alert("投票成功");

                    }
                    else {
                        alert(resp.errmsg);
                    }


                },
                timeout: 6000000,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时，请刷新页面");

                    }

                },
                complete: function () {
                    $("#btnSumbit").val("投票");
                }
            });



        });

        //给指定选手编号投票
        $("#btnSumbit1").click(function () {
            var model = { Action: 'SysAddvoteCount',
                VoteID: $("#txtVoteId").val(),
                Number: $("#txtNumber").val(),
                AddCount: $("#txtAddCount").val()
            }
            if (model.Number == "") {
                alert("选手编号必填");
                return false;
            }
            if (model.AddCount == "") {
                alert("票数必填");
                return false;
            }
            $("#btnSumbit1").val("正在处理。。。。");
            $.ajax({
                type: 'post',
                url: "/customize/beachhoney/handler.ashx",
                data: model,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        alert("投票成功");

                    }
                    else {
                        alert(resp.errmsg);
                    }


                },
                timeout: 6000000,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时，请刷新页面");

                    }

                },
                complete: function () {
                    $("#btnSumbit1").val("投票");
                }
            });



        });
    })
</script>
</html>
