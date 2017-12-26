

//订单
var currOrderInfo = {
    Products: [],
    OrderUserID: '',
    Address: '',
    Phone: '',
    TotalAmount: 0.0,
    OrderMemo: ''
};

var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
var userHandlerUrl = '/Handler/User/UserHandler.ashx';

$(function () {

    try {
        
        GetCookieData();
//        ShowOrderSmallCarCount();
        IniPageAction();
        BtnEvenBind();

    } catch (e) {
        //alert(e);
        msgText.init(e, 3000);
    }




});

//获取cookie缓存数据
function GetCookieData() {
    //获取当前订单
    if (getCookie('currOrderInfo') != null && getCookie('currOrderInfo') != '') {
        currOrderInfo = $.parseJSON(getCookie('currOrderInfo'));
    };

    //清除订单中已经被够选掉的商品
    var tmpArr = [];
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if ((currOrderInfo.Products[i].Mark == 1)&&(currOrderInfo.Products[i].TotalCount>0)) {
            tmpArr.push(currOrderInfo.Products[i]);
        }
    };
    currOrderInfo.Products = tmpArr.slice(0);

}

//页面初始化处理
function IniPageAction(){
    var pathName = window.location.pathname.toLowerCase();
    var pageAction = '';
    if (pathName.indexOf('indexv2.aspx') > 0) {
        pageAction = 'index';
        ClearOrder();
    }
    else if (pathName.indexOf('orderdeliveryv2.aspx') > 0) {
        pageAction = 'orderdelivery';
    }
    else if (pathName.indexOf('mycenter.aspx') > 0) {
        pageAction = 'mycenter';
    }

    switch(pageAction){
        case 'index':
            if (categoryid != "") {
                if (parseInt(categoryid) > 0) {
                  
                    $("li[data-categoryid]").each(function () {

                        if ($(this).attr("data-categoryid") == categoryid) {
                            $(this).addClass("current");
                        }



                    });
                    LoadProductList(categoryid);
                    return;
                }
            }
            if ($("li[data-categoryid]").length > 0) {//有分类，默认加载第一个分类
                $("li[data-categoryid]").first().addClass("current");
                var defaultcateid = $("li[data-categoryid]").first().attr("data-categoryid");
                LoadProductList(defaultcateid);
                return;

            }
            LoadProductList("");
            break;
        case 'orderdelivery':
            /*
            配送方式
            */
            if (currOrderInfo != null) {
                var listHtmlOrderConfirm = '';
                for (var i = 0; i < currOrderInfo.Products.length; i++) {
                   
                    //构造视图模板
                    var str = new StringBuilder();
                    //
                    str.AppendFormat('<div class="product">');
                    str.AppendFormat('<img src="{0}" >', currOrderInfo.Products[i].Img);
                    str.AppendFormat('<div class="info">');
                    str.AppendFormat('<span class="text">{0}</span>', currOrderInfo.Products[i].PName);
                    str.AppendFormat('<span class="price">￥{0}<span class="num">x{1}</span></span>', currOrderInfo.Products[i].Price, currOrderInfo.Products[i].TotalCount);
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');
                    //
                    listHtmlOrderConfirm += str.ToString();


                };

                $('#orderconfirm').html(listHtmlOrderConfirm);
                if (listHtmlOrderConfirm == '') {
                     $('#orderconfirm').html('<div style="margin-left:10px;"><label style="color: #666;">购物车内暂时没有商品</label><br/></div><div style="margin-top:10px;"><a style="color: #005ea7;margin-left:10px;" href="Indexv2.aspx">去首页<a></div>');
                }
                 RefreshTotalPrinceTotalCountView();

            }
             break;

         case 'mycenter'://个人中心
             RefreshTotalPrinceTotalCountView();
             break;


        default:
            return;
    }
}

//按钮事件绑定
function BtnEvenBind(){

    //购物车 "去结算" 按钮 新的
    $('#btnSettlement').on('click', function () {
        if (GetOrderTotalCount() == 0) {
            //alert('请选择商品');
            msgText.init("请选择商品", 3000);
            return;
        }
        //检查库存
        //读取已订购的商品
        var pidJson = {
            Pids: []
        };
        for (var i = 0; i < currOrderInfo.Products.length; i++) {
            if (currOrderInfo.Products[i].Mark == 0) { continue; }
            var pids = [];
            pids.push(currOrderInfo.Products[i].PID);
            pids.push(currOrderInfo.Products[i].TotalCount);
            pidJson.Pids.push(pids);
        };
        $.ajax({
            type: 'post',
            url: mallHandlerUrl,
            data: { Action: "CheckStock",PIds: JSON.stringify(pidJson)},
            timeout: 60000,
            success: function (result) {
                var resp = $.parseJSON(result);
                if (resp.Status == 1) {
                    //库存检查通过
                    $.ajax({
                        type: 'post',
                        url: "/Handler/OpenGuestHandler.ashx",
                        data: { Action: 'CheckLogin' },
                        cache: false,
                        async: false,
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {//已经登录 跳转到 配送方式 页
                                window.location = "/App/Cation/Wap/Mall/OrderDeliveryv2.aspx";
                                return;


                            }
                            else {//还没有登录

                                window.location = "/App/Cation/Wap/Login.aspx?redirecturl=/App/Cation/Wap/Mall/OrderDeliveryv2.aspx";
                                return;

                            }

                        }
                    });
                    //库存检查通过

                }
                else {
                    //库存不足
                    //alert(resp.Msg);
                    msgText.init(resp.Msg, 3000);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    //alert("操作超时，请重新提交订单");
                    msgText.init("操作超时，请重新提交订单", 3000);

                }
                else {
                    //alert(textStatus + "请重新提交订单");
                    msgText.init(textStatus + "请重新提交订单", 3000);
                }
            }
        })
        //


    });



    //提交订单
    $('#btnSumbitOrder').on('click', function () {

        try {
            //
            if (GetOrderTotalCount() == 0) {
                //alert('购物车内暂时没有商品');
                msgText.init("购物车内暂时没有商品", 3000);
                return;
            }
            //
            //读取已订购的商品
            var pidJson = {
                Pids: []
            };
            for (var i = 0; i < currOrderInfo.Products.length; i++) {
                if (currOrderInfo.Products[i].Mark == 0) { continue; }
                var pids = [];
                pids.push(currOrderInfo.Products[i].PID);
                pids.push(currOrderInfo.Products[i].TotalCount);
                pidJson.Pids.push(pids);
            };

            var ajaxData = {
                PIds: JSON.stringify(pidJson),
                Consignee: $.trim($('#txtLinkerName').val()),
                Phone: $.trim($('#txtLinkerPhone').val()),
                Address: $.trim($('#txtLinkerAddress').val()),
                OrderMemo: $.trim($('#txtOrderMemo').val()),
                DeliveryTime: null,
                DeliveryAutoId: $("#ddldelivery").val(),
                PaymenttypeAutoId: $("#ddlpaymenttype").val(),
                Action: 'SubmitWxMallOrderV1'
            }

            if (ajaxData.DeliveryAutoId == null || ajaxData.DeliveryAutoId == "") {
                //alert('请选择配送方式');
                msgText.init("请选择配送方式", 3000);
                return;
            }
            if (ajaxData.PaymenttypeAutoId == null || ajaxData.PaymenttypeAutoId == "") {
                //alert('请选择支付方式');
                msgText.init("请选择支付方式", 3000);
                return;
            }
            if (ajaxData.Consignee == '') {
                //alert('请输入收货人信息');
                msgText.init("请输入收货人信息", 3000);
                return;
            }
            if (ajaxData.Phone == '') {
                //alert('请输入手机号码');
                msgText.init("请输入手机号码", 3000);
                return;
            }
            ajaxData.DeliveryTime = $("#ddlday").val() + " " + $("#ddlhour").val() +":"+ $("#ddlmin").val();
            $("#btnSumbitOrder").html("正在提交...");
            $.ajax({
                type: 'post',
                url: mallHandlerUrl,
                data: ajaxData,
                timeout: 60000,
                success: function (result) {
                    $("#btnSumbitOrder").html("提交订单");
                    var resp = $.parseJSON(result);
                    if (resp == null || resp == '') {
                        //alert('订单提交失败');
                        msgText.init("订单提交失败", 3000);
                        return;
                    }
                    if (resp.Status == 1) {
                        ClearOrder(); //清空购物车
                        window.location = "success.aspx?oid=" + resp.ExStr + "&gopage=indexv2.aspx";
                        return;
                    }
                    else {
                       // alert(resp.Msg);
                        msgText.init(resp.Msg, 3000);
                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#btnSumbitOrder").html("提交订单");
                    if (textStatus == "timeout") {
                       // alert("操作超时，请重新提交订单");
                        msgText.init("操作超时，请重新提交订单", 3000);
                    }
                    else {
                        //alert(textStatus + "请重新提交订单");
                        msgText.init("请重新提交订单", 3000);
                    }
                }
            }); //

        } catch (e) {
            //alert(e);
            msgText.init(e, 3000);
        }



    });



    //首页商品分类筛选
    $("li[data-categoryid]").click(function () {
        ClearOrder();
        RefreshTotalPrinceTotalCountView();
        $("li[data-categoryid]").removeClass("current");
        $(this).addClass("current");
        var categoryid = $(this).attr("data-categoryid");
        LoadProductList(categoryid);

    });

    $("#ddldelivery").change(function () {

        Transport_Fee();




    })
    Transport_Fee();


}

var cartprice = {
    addprice: function () {
        //var totalprice = 0;
        var totalnum = 0;
        $(".numbox").each(function () {
            if (!$(this).hasClass("checknone")) {
                //var price = $(this).parent().find(".orangetext").text()
                var num = Number($(this).find(".num")[0].value);
//                price = Number(price.substring(1, price.length));
//                totalprice += price * num;
                totalnum += num;
            }
        })
        if (totalnum > 0) {
            $(".toolbar").find(".btn").removeClass("gary2")
        } else {
            $(".toolbar").find(".btn").addClass("gary2")
        }
       // $(".totalnum").text(totalnum);
        //$(".totalprice").text(totalprice.toFixed(1));
    },
    addnum: function (container) {
        var nump = $(container).parent()
        nump.removeClass("checknone");
        nump.find(".num")[0].value++;
    },
    minus: function (container) {
        var nump = $(container).parent();
        var numc = nump.find(".num")[0];
        if (numc.value > 1) {
            numc.value--;
        } else {
            numc.value--;
            nump.addClass("checknone");
        }
    }
}

//购物车加 
function addnuma(object) {
     
    var pid = $(object).closest('.productbox').attr('data-pid');
    if (!CheckProductIsInOrder(pid)) {
        var price = 0;
        var productname = "";
        var priductimage = "";
        productname = $(object).closest('.productbox').find("h2").first().text();
        priductimage = $(object).closest('.productbox').find("img").first().attr("src");
        price = $(object).closest('.productbox').find("span[class=orangetext]").text().replace("￥", "");
        price = parseFloat(price);
        AddInOrder(pid, price, productname, priductimage);

    }

    UpdateCurrOrderProductCount(pid, 1);
    RefreshTotalPrinceTotalCountView();
    cartprice.addnum($(object));
    cartprice.addprice();
}


//购物车减
function minusa(object) {
    var pid = $(object).closest('.productbox').attr('data-pid');
    if (!CheckProductIsInOrder(pid)) {
        var price = 0;
        var productname = "";
        var priductimage = "";
        productname = $(object).closest('.productbox').find("h2").first().text();
        priductimage = $(object).closest('.productbox').find("img").first().attr("src");
        price = $(object).closest('.productbox').find("span[class=orangetext]").text().replace("￥", "");
        price = parseFloat(price);
        AddInOrder(pid, price, productname, priductimage);


    }
    UpdateCurrOrderProductCount(pid, -1);
    RefreshTotalPrinceTotalCountView();
    cartprice.minus($(object));
    cartprice.addprice();

}

//



//清空购物车
function ClearOrder() {
    currOrderInfo = {
        Products: [],
        OrderUserID: '',
        Address: '',
        Phone: '',
        TotalAmount: 0.0,
        OrderMemo: ''
    };
    SaveCurrOrderInCookie();
    
}

//刷新界面总数跟总金额界面视图
function RefreshTotalPrinceTotalCountView() {

    $('#lbltotalprice').html(GetOrderTotalAmount().toFixed(2));
    $('#lbltotalnum').html(GetOrderTotalCount());
    

}




//更改购物车中指定商品数量 新的
function UpdateCurrOrderProductCount(pid, num) {

    if (!CheckProductIsInOrder(pid)) {
    //不存在购物车中，先加入购物车
        currOrderInfo.Products.push({ PID: pid, Price: price, TotalCount: 1, PName: pname, Img: img, Mark: 1 });

    }

    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if (currOrderInfo.Products[i].PID == pid) {
            currOrderInfo.Products[i].TotalCount = currOrderInfo.Products[i].TotalCount + num;
            if (currOrderInfo.Products[i].TotalCount <= 0) {
                currOrderInfo.Products[i].TotalCount =0;
                
            }
            break;
        }
       
    }

    SaveCurrOrderInCookie();
}


//加入购物车
function AddInOrder(pid, price, pname, img) {
currOrderInfo.Products.push({ PID: pid, Price: price, TotalCount:0, PName: pname, Img: img, Mark: 1 });
//更新cookie及左下角购物车
SaveCurrOrderInCookie();
}



//检查商品是否在购物车中 false 不在购物车中  true 已经在购物车中
function CheckProductIsInOrder(pid) {
    var isHaving = false;
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if (currOrderInfo.Products[i].PID == pid) {
            isHaving = true;
            break;
        }
    }
    return isHaving;
}


//获取购物车总数
function GetOrderTotalCount(){
    var result = 0;
    if (currOrderInfo != null) {
        for (var i = 0; i < currOrderInfo.Products.length; i++) {
            //过滤Mark为0的
            if (currOrderInfo.Products[i].Mark == 0) { continue;}

            result += currOrderInfo.Products[i].TotalCount;
        };  
    }
    
    return result;
}

//获取购物车总金额
function GetOrderTotalAmount(){
    var result = 0;
    if (currOrderInfo != null) {
        for (var i = 0; i < currOrderInfo.Products.length; i++) {
            //过滤Mark为0的
            if (currOrderInfo.Products[i].Mark == 0) { continue;}

            result += currOrderInfo.Products[i].TotalCount * currOrderInfo.Products[i].Price;
        }
    }
    
    return result;
}


//保存当前购物车到缓存
function SaveCurrOrderInCookie(){
    SetCookie('currOrderInfo', JSON.stringify(currOrderInfo));
}

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


//判断是不是手机号码
function isPhone(value) {
    return /^(13|15|18)\d{9}$/i.test(value);
}

//加载商品
function LoadProductList(categoryid) {


    $.ajax({
        type: 'post',
        url: mallHandlerUrl,
        data: { Action: 'QueryProductsObjList', CategoryId: categoryid },
        success: function (result) {
            var resp = $.parseJSON(result);
            if (resp.ExObj == null) { return; }
            var listHtml = '';
            for (var i = 0; i < resp.ExObj.length; i++) {
                //构造视图模板
                var str = new StringBuilder();
                    str.AppendFormat('<li>');
                    str.AppendFormat('<div class="productbox" data-pid={0}>', resp.ExObj[i].PID);
                    str.AppendFormat('<span href="#" class="img">');
                    str.AppendFormat('<img src="{0}">', resp.ExObj[i].RecommendImg);
                    str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].PName);
                    str.AppendFormat('</span>');
                    str.AppendFormat('<p class="price">价格：<span class="orangetext">￥{0}</span></p>', resp.ExObj[i].Price);
                    str.AppendFormat('<div class="numbox checknone">');
                    if (resp.ExObj[i].Stock > 0) {
                        
                        str.AppendFormat('<span class="addbtn" onclick="addnuma(this)"><span class="icon"></span></span>');
                        str.AppendFormat('<input type="number" readonly="readonly" pattern="\d*"  class="num" value="0">');
                        str.AppendFormat('<span class="minus" onclick="minusa(this)"><span class="icon"></span></span>');
                        
                    }
                    else {
                    str.AppendFormat('<p style="color:red;font-size: 14px;">已售完</p>');
                    }
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</li>');
                    listHtml += str.ToString();

            };
            if (listHtml != "") {

                //填入列表
                $('#productList').html(listHtml);

            }
            else {
                $('#productList').html("暂时没有商品.");
            }


        }
    });



}

//刷新物流费用
function Transport_Fee() {

    //计算物流费用
    //读取已订购的商品
    var pidJson = {
        Pids: []
    };
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if (currOrderInfo.Products[i].Mark == 0) { continue; }
        var pids = [];
        pids.push(currOrderInfo.Products[i].PID);
        pids.push(currOrderInfo.Products[i].TotalCount);
        pidJson.Pids.push(pids);
    };
    $.ajax({
        type: 'post',
        url: mallHandlerUrl,
        data: { Action: "CalcTransportFee", PIds: JSON.stringify(pidJson), DeliveryAutoId: $("#ddldelivery").val() },
        timeout: 60000,
        dataType: "json",
        success: function (resp) {

            if (resp.Status == 1) {
                $("#sptransportFee").html(resp.ExStr);
                var productfee = GetOrderTotalAmount().toFixed(2);
                var totalfee = (parseFloat(productfee) +parseFloat(resp.ExStr)).toFixed(2);
                $("#sptotal").html(totalfee);
            }
            else {

            }


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (textStatus == "timeout") {
                //alert("操作超时");
                msgText.init("操作超时", 3000);
            }
            else {
                //alert(textStatus);
                msgText.init(textStatus, 3000);
            }
        }
    })
    //






}



