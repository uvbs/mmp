////用户实体
//var currUserInfo = {
//    UserID: '',
//    TrueName: '',
//    Phone: '',
//    Address: '',
//    WXHeadimgurlLocal: ''
//};

//订单
var currOrderInfo = {
    Products: [],
    OrderUserID: '',
    Address: '',
    Phone: '',
    TotalAmount: 0.0,
    OrderMemo: ''
};
var sellCount = 0;//销量
var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
//var userHandlerUrl = '/Handler/User/UserHandler.ashx';
var productSort = "";

var currShowProduct = null;//用于展示页当前的商品实体

$(function () {
    
    try {
        GetCookieData();
        ShowOrderSmallCarCount();
        IniPageAction();
        BtnEvenBind();

    } catch (e) {
        alert(e);
    }



});

//获取cookie缓存数据
function GetCookieData() {

//    //获取当前用户
//    if (getCookie('currUserInfo') == null || getCookie('currUserInfo') == '') {
//        //获取用户信息
//        $.ajax({
//            type: 'post',
//            url: userHandlerUrl,
//            data: { Action: 'GetUserInfo' },
//            dataType:"json",
//            success: function (resp) {
//                currUserInfo.UserID = resp.ExObj.UserID;
//                currUserInfo.TrueName = resp.ExObj.TrueName;
//                currUserInfo.Phone = resp.ExObj.Phone;
//                currUserInfo.Address = resp.ExObj.Address;
//                currUserInfo.WXHeadimgurlLocal = resp.ExObj.WXHeadimgurlLocal;

//                //设置用户信息到cookie
//                SetCookie('currUserInfo', JSON.stringify(currUserInfo));



//            }
//        });
//    }
//    else {
//        currUserInfo = $.parseJSON(getCookie('currUserInfo'));
//    }

    //获取当前订单
    if (getCookie('currOrderInfo') != null && getCookie('currOrderInfo') != '') {
        currOrderInfo = $.parseJSON(getCookie('currOrderInfo'));
    };

    //清除订单中已经被够选掉的商品
    var tmpArr = [];
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if (currOrderInfo.Products[i].Mark == 1) {
            tmpArr.push(currOrderInfo.Products[i]);
        }
    };
    currOrderInfo.Products = tmpArr.slice(0);

}

//页面初始化处理
function IniPageAction(){

    var pathName = window.location.pathname.toLowerCase();
    var pageAction = '';//GetParm('action');

    if (pathName.indexOf('index.aspx') > 0) {
        pageAction = 'index';
    }
    else if (pathName.indexOf('productlist.aspx') > 0) {
        pageAction = 'productlist';
    }
    else if (pathName.indexOf('productdetail.aspx') > 0) {
        pageAction = 'productdetail';
    }else if (pathName.indexOf('shopcar.aspx') > 0) {
        pageAction = 'shopcar';
    }
    else if (pathName.indexOf('orderconfirm.aspx') > 0) {
        pageAction = 'orderconfirm';
    }
    else if (pathName.indexOf('mycenter.aspx') > 0) {
        pageAction = 'mycenter';
    }

    switch(pageAction){
        case 'index':
            //AlertProcessger('正在加载商品列表...', '20%');
//            if (categoryid != "") {
//                if (parseInt(categoryid) > 0) {
//                    //$("li[data-categoryid]").removeClass("current");
//                    $("li[data-categoryid]").each(function () {

//                        if ($(this).attr("data-categoryid") == categoryid) {
//                            $(this).addClass("current");
//                        }



//                    });
//                    LoadProductList(categoryid);
//                    return;
//                }
//            }
//            if ($("li[data-categoryid]").length > 0) {//有分类，默认加载第一个分类
//                $("li[data-categoryid]").first().addClass("current");
//                var defaultcateid = $("li[data-categoryid]").first().attr("data-categoryid");
//                LoadProductList(defaultcateid);
//                return;

//            }

            //LoadProductList("");
//            var pid = GetParm('pid');
//            if (pid!="") {
//             SetCookie('sid', GetParm('sid')); //sid 表示推荐人用户编号 
//            }
//	    clearCookie();
            break;
        case "productlist":
            LoadProductList();
            break;
        case 'productdetail':
            var pid = GetParm('pid');
            $.ajax({
                type: 'post',
                url: mallHandlerUrl,
                data: { Action: 'GetProductObj', PID: pid },
                dataType: "json",
                success: function (resp) {
                    //SetCookie('sid', GetParm('sid'));//sid 表示推荐人用户编号 
                    currShowProduct = resp.ExObj;
                    sellCount = resp.ExInt;
                    ShowProductDetails(currShowProduct);
                }
            });
            //UpdateProductIPPV(pid);
            break;
        case 'shopcar':
            /*
            购物车页面加载进来，读取缓存中已选择的商品，展示已选商品到购物车
            自动填写用户信息到表单
            */
            if (currOrderInfo != null) {
                var listHtmlOrder = '';
                var str = new StringBuilder();
                for (var i = 0; i < currOrderInfo.Products.length; i++) {
                    //构造视图模板
                    
//                    str.AppendFormat('<div class="product" data-pid="{0}">', currOrderInfo.Products[i].PID);
//                    str.AppendFormat('<input class="checkbox" type="checkbox" checked="checked" id="cbproduct{0}">', i);
//                    str.AppendFormat('<label for="cbproduct{0}"><span class="icon"></span></label>', i);
//                    str.AppendFormat('<div class="concent">');
//                    str.AppendFormat('<img src="{0}" alt="">', currOrderInfo.Products[i].Img);
//                    str.AppendFormat('<div class="info">');
//                    str.AppendFormat('<h2 class="name">{0}</h2>', currOrderInfo.Products[i].PName);
//                    str.AppendFormat('<span class="price">￥{0}</span>', currOrderInfo.Products[i].Price);
//                    str.AppendFormat('</div>');
//                    str.AppendFormat('</div>');
//                    str.AppendFormat('<div class="numbox">');
//                    str.AppendFormat('<p class="text">购买数量</p>');
//                    str.AppendFormat('<span class="addbtn"><span class="icon"></span></span>');
//                    str.AppendFormat('<input type="text" readonly="readonly" class="num" value="{0}">', currOrderInfo.Products[i].TotalCount);
//                    str.AppendFormat('<span class="minus"><span class="icon"></span></span>');
//                    str.AppendFormat('</div>');
//                    str.AppendFormat('</div>');
//                    listHtmlOrder += str.ToString();

                    //

                    str.AppendFormat('<li data-pid="{0}">', currOrderInfo.Products[i].PID);   
                    str.AppendFormat('<table>');
                    str.AppendFormat('<tr>');
                    str.AppendFormat('<td class="td0">');
                    str.AppendFormat('<input type="checkbox" checked="checked" id="cbproduct{0}"/>',i);
                    str.AppendFormat('</td>');
                    str.AppendFormat('<td class="td1">');
                    str.AppendFormat('<img src="{0}" title="" />',currOrderInfo.Products[i].Img);
                    str.AppendFormat('</td>');
                    str.AppendFormat(' <td class="td2">');
                    str.AppendFormat('<span>{0} </span><br/>￥<span class="color2">{1} </span>', currOrderInfo.Products[i].PName, currOrderInfo.Products[i].Price);
                    str.AppendFormat('<div style="float: right">');
                    str.AppendFormat('<input  name="" type="button" value="" class="btn_box text_box_1" />');
                    str.AppendFormat('<input  readonly="readonly" type="text" value="{0}" class="text_box" />', currOrderInfo.Products[i].TotalCount);
                    str.AppendFormat('<input name="" type="button" value="" class="btn_box text_box_2" />');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</td>');
                    str.AppendFormat('</tr>');
                    str.AppendFormat('</table>');
                    str.AppendFormat('</li>');
                    //


                };
                listHtmlOrder= str.ToString();
                $('#orderlist').html(listHtmlOrder);
                if (listHtmlOrder == '') {
                    //$('#orderlist').html('<div style="margin-left:10px;"><label style="color: #666;">购物车内暂时没有商品</label><br/></div><div style="margin-top:10px;"><a style="color: #005ea7;margin-left:10px;" href="Index.aspx">去首页<a></div>');
                    $('#orderlist').html('<div class="noproduct"><span class="icon"></span><a href="Index.aspx" class="btn orange">去首页逛逛</a></div>');

                }
                RefreshTotalPrinceTotalCountView();

            }
            break;


        case 'orderconfirm':
            /*
            配送方式
            */
            if (currOrderInfo != null) {
                var listHtmlOrderConfirm = '';
                var str = new StringBuilder();
               for (var i = 0; i < currOrderInfo.Products.length; i++) {

                    //构造视图模板
                   
//                    str.AppendFormat('<div class="product">');
//                    str.AppendFormat('<img src="{0}" >', currOrderInfo.Products[i].Img);
//                    str.AppendFormat('<div class="info">');
//                    str.AppendFormat('<span class="text">{0}</span>', currOrderInfo.Products[i].PName);
//                    str.AppendFormat('<span class="price">￥{0}<span class="num">x{1}</span></span>', currOrderInfo.Products[i].Price, currOrderInfo.Products[i].TotalCount);
//                    str.AppendFormat('</div>');
//                    str.AppendFormat('</div>');

                   str.AppendFormat('<tr>');
                   str.AppendFormat('<td class="td1">');
                   str.AppendFormat('<img src="{0}" alt="" title="" />', currOrderInfo.Products[i].Img);
                   str.AppendFormat('</td>');
                   str.AppendFormat('<td>');
                   str.AppendFormat('<span>{0}</span>', currOrderInfo.Products[i].PName);
                   str.AppendFormat('</br><span class="color2">{0}&nbsp;X&nbsp;{1} </span>', currOrderInfo.Products[i].Price, currOrderInfo.Products[i].TotalCount);

                   str.AppendFormat('</td>');
                   str.AppendFormat('</tr>');






                };
                listHtmlOrderConfirm += str.ToString();

                $('#tbproductlist').html(listHtmlOrderConfirm);
                if (listHtmlOrderConfirm == '') {
                  
                }
                RefreshTotalPrinceTotalCountView();
            }
            //Transport_Fee();
            break;

         case 'mycenter'://个人中心
            // RefreshTotalPrinceTotalCountView();
             break;


        default:
            return;
    }
}

//按钮事件绑定
function BtnEvenBind() {

    //添加收藏
    $('#btnAddProductCollect').on('click', function () {

        $.ajax({
            type: 'post',
            url: mallHandlerUrl,
            data: { Action: 'AddProductCollect', Pid: GetParm("pid") },
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 1) {//已经登录 跳转到 配送方式 页
                    msgText.init("收藏成功", 3000);
                }
                else {
                    msgText.init(resp.Msg,3000);


                }

            }
        });




    });
    
    //添加到购物车按钮
    $('#btnAddInOrder').click(function(){
        if (currShowProduct != null) {
            AddInOrder(currShowProduct.PID, currShowProduct.Price, currShowProduct.PName, currShowProduct.RecommendImg,parseInt($("#txtCount_Product").val()));
        };
    });

//    //立即购买按钮
//    $('#btnBuyNow').click(function(){
//         if (currShowProduct != null) {
//            if (AddInOrder(currShowProduct.PID,currShowProduct.Price,currShowProduct.PName,currShowProduct.RecommendImg)) {
//                //AlertMsg('正在转到购物车。。');
//                window.location.href = 'Orderv1.aspx';
//            }//加入购物车后立即跳转到购物车
//        };
//    });

    //购物车 “+”按钮 新的
    $('.text_box_2').on('click', function () {
        var pid = $(this).closest('li').attr('data-pid');
        UpdateCurrOrderProductCount(pid, 1);
        RefreshTotalPrinceTotalCountView();
    });

    //购物车 “-” 按钮 新的
    $('.text_box_1').on('click', function () {
        var pid = $(this).closest('li').attr('data-pid');
        UpdateCurrOrderProductCount(pid, -1);
        RefreshTotalPrinceTotalCountView();
    });

    

   //购物车 "选择" 按钮 新的
    $('input[type=checkbox]').on('click', function () {
        var pid = $(this).closest('li').attr('data-pid');
        if ($(this).is(':checked')) {
            //已选
            UpdateCurrOrderProductMark(pid, 1); //更改为已选
            RefreshTotalPrinceTotalCountView();

        } else {
            //未选
            UpdateCurrOrderProductMark(pid, 0); //更改为未选
            RefreshTotalPrinceTotalCountView();


        }
        ;
    });



    //购物车 "去结算" 按钮 新的
    $('#btnSettlement').on('click', function () {

        if (GetOrderTotalCount() == 0) {
           
            msgText.init("购物车内暂时没有商品", 3000);
            return;
        }
        window.location = 'OrderConfirm.aspx';

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
                AddressId:addressid,
                CouponNumber: $.trim($('#txtCouponNumber').val()),
                Action: 'SubmitWxMallOrderTuao'
            }

//            if (ajaxData.DeliveryAutoId == null || ajaxData.DeliveryAutoId == "") {
//                //alert('请选择配送方式');
//                msgText.init("请选择配送方式", 3000);
//                return;
//            }
//            if (ajaxData.PaymenttypeAutoId == null || ajaxData.PaymenttypeAutoId == "") {
//                //alert('请选择支付方式');
//                msgText.init("请选择支付方式", 3000);
//                return;
//            }
//            if (ajaxData.Consignee == '') {
//                //alert('请输入收货人姓名');
//                msgText.init("请输入收货人姓名", 3000);
//                return;
//            }
//            if (ajaxData.Phone == '') {
//                //alert('请输入联系号码');
//                msgText.init("请输入联系电话", 3000);
//                return;
//            }
//            if (ajaxData.Address == '') {
//                //alert('请输入联系号码');
//                msgText.init("请输入配送地址", 3000);
//                return;
//            }

            $("#btnSumbitOrder").html("正在提交...");
            $.ajax({
                type: 'post',
                url: mallHandlerUrl,
                data: ajaxData,
                timeout: 60000,
                dataType:'json',
                success: function (resp) {
                    $("#btnSumbitOrder").html("提交订单");
                    if (resp.Status == 1) {
                        ClearOrder(); //清空购物车
                        window.location = "DoWXPay.aspx?oid=" + resp.ExStr;
                        return;
                    }
                    else {
                        alert(resp.Msg);
                       // msgText.init(resp.Msg, 3000);

                        return;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $("#btnSumbitOrder").html("提交订单");
                    if (textStatus == "timeout") {
                        alert("操作超时，请重新提交订单");
                        //msgText.init("操作超时，请重新提交订单", 3000);
                    }
                    else {
                        alert(textStatus + "请重新提交订单");
                        //msgText.init("请重新提交订单", 3000);
                    }
                }
            }); //

        } catch (e) {
            // alert(e);
        }



    });



//    //首页商品分类筛选
//    $("li[data-categoryid]").click(function () {
//       // $(".kindbox .current").removeClass("current");
//        $(".kindbox_s .current").removeClass("current");
//        if ($(this).data("rootcategroy")=="1") {
//            $(".kindbox .current").removeClass("current");
//        }
//        $(this).addClass("current");
//        var categoryid = $(this).attr("data-categoryid");
//        $(".kindbox_s").hide();
//        $(".kindbox_s_noshow").hide();
//        LoadProductList(categoryid);

//    });

//    $("#ddldelivery").change(function () {

//        Transport_Fee();

//    })


}

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
function RefreshTotalPrinceTotalCountView(){
    $('#lbltotalprice').html(GetOrderTotalAmount().toFixed(2));
    $('#lbltotalnum').html(GetOrderTotalCount());


}

//更改购物车商品Mark
function UpdateCurrOrderProductMark(pid,mark){
    if (currOrderInfo == null) {return;}
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if(currOrderInfo.Products[i].PID == pid){
            currOrderInfo.Products[i].Mark = mark;
            break;
        }
    }
    SaveCurrOrderInCookie();
}


//更改购物车中指定商品数量 新的
function UpdateCurrOrderProductCount(pid, num) {
    if (currOrderInfo == null) { return; }
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if (currOrderInfo.Products[i].PID == pid) {
            currOrderInfo.Products[i].TotalCount = currOrderInfo.Products[i].TotalCount + num;
            if (currOrderInfo.Products[i].TotalCount == 0) {
                //alert('亲，至少买一件嘛');
                currOrderInfo.Products[i].TotalCount = 1;
            }

            //更新html数量统计
            $('li[data-pid="' + pid + '"]').find('input[type=text]').val(currOrderInfo.Products[i].TotalCount);

            break;
        }
    }

    SaveCurrOrderInCookie();
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

//显示购物车商品数
function ShowOrderSmallCarCount() {
    var cartnum = GetOrderTotalCount();
    if (parseInt(cartnum)>0) {
        $('.cartnum').html(cartnum);
        $('.cartnum').show();
        
    }
    else {
        $('.cartnum').hide();
    }
    



}

//展示商品详情
function ShowProductDetails(pro){

    //商品图片
    $("#productImg").attr("src", pro.RecommendImg);
    //商品名称
    $('#lblProductName').html(pro.PName);

    if (pro.Price=="0") {
        //写入价格
        $('#lblProductPrice').html("未定价");

    }
    else {
        //写入价格
        $('#lblProductPrice').html("￥" + pro.Price);

    }
    $('#lblPrePrice').html("原价￥" + pro.PreviousPrice);
    if (pro.Stock == "0") {
        //写入库存
        //$('#lblProductStock').html("已售完");

    }
    else {


    }
    $('#spbuycount').html("销量:" + sellCount+"件");
    //商品幻灯片
    var str = new StringBuilder();
   

    if (pro.ShowImage1 != null && pro.ShowImage1 != "") {
        str.AppendFormat('<li><img src="{0}"  title="" border="0" /></li>', pro.ShowImage1);

    }
    if (pro.ShowImage2 != null && pro.ShowImage2 != "") {
        str.AppendFormat('<li><img src="{0}"  title="" border="0" /></li>', pro.ShowImage2);

    }
    if (pro.ShowImage3 != null && pro.ShowImage3 != "") {
        str.AppendFormat('<li><img src="{0}"  title="" border="0" /></li>', pro.ShowImage3);

    }
    if (pro.ShowImage4 != null && pro.ShowImage4 != "") {
        str.AppendFormat('<li><img src="{0}"  title="" border="0" /></li>', pro.ShowImage4);

    }
    if (pro.ShowImage5 != null && pro.ShowImage5!= "") {
        str.AppendFormat('<li><img src="{0}"  title="" border="0" /></li>', pro.ShowImage5);

    }
    $("#ulslidesproduct").html(str.ToString());
    

   



    $('.panels_slider').flexslider({
        animation: "slide",
        directionNav: false,
        controlNav: true,
        animationLoop: true,
        slideToStart: 0,
        slideshowSpeed: 3000,
        animationDuration: 300,
        slideshow: true
    });
    //商品幻灯片

    //填充商品介绍
    $('#lblProductDescription').html(pro.PDescription);

            wx.ready(function () {
            wxapi.wxshare({
                title: pro.PName,
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://" + location.host + pro.RecommendImg
            })
        })

      
}

////商品详情-添加到购物车
//function AddInOrder(pid,price,pname,img){
//    /*
//        添加一件商品到购物车
//        判断商品是否已在购物车
//        如果已经购物车，则数量加1
//    */

//    if (currOrderInfo == null) {
//        currOrderInfo = {
//            Products: [{PID:pid,Price:price,TotalCount:1,PName:pname,Img:img,Mark:1}],
//            OrderUserID: '',
//            Address: '',
//            Phone: '',
//            TotalAmount: 0.0,
//            OrderMemo: ''
//        };
//    }else{
//        var isHaving = false;
//        for (var i = 0; i < currOrderInfo.Products.length; i++) {
//            if (currOrderInfo.Products[i].PID == pid) {
//                currOrderInfo.Products[i].TotalCount++;
//                currOrderInfo.Products[i].Mark = 1;
//                isHaving = true;
//                break;
//            }
//        };
//        if (!isHaving) {
//            currOrderInfo.Products.push({PID:pid,Price:price,TotalCount:1,PName:pname,Img:img,Mark:1});
//        };
//    }

//    //更新cookie及左下角购物车
//    //SaveCurrOrderInCookie();
//    //ShowOrderSmallCarCount();
//    msgText.init("所选商品已成功加入购物车", 3000);
//    return true;
//}

//商品详情-添加到购物车
function AddInOrder(pid, price, pname, img,count) {
    /*
    添加商品到购物车
    判断商品是否已在购物车
    如果已经购物车，则数量加
    */
    if (currOrderInfo == null) {
        currOrderInfo = {
            Products: [{ PID: pid, Price: price, TotalCount: count, PName: pname, Img: img, Mark: 1}],
            OrderUserID: '',
            Address: '',
            Phone: '',
            TotalAmount: 0.0,
            OrderMemo: ''
        };
    } else {
        var isHaving = false;
        for (var i = 0; i < currOrderInfo.Products.length; i++) {
            if (currOrderInfo.Products[i].PID == pid) {
                currOrderInfo.Products[i].TotalCount+=count;
                currOrderInfo.Products[i].Mark = 1;
                isHaving = true;
                break;
            }
        };
        if (!isHaving) {
            currOrderInfo.Products.push({ PID: pid, Price: price, TotalCount:count, PName: pname, Img: img, Mark: 1 });
        };
    }

    //更新cookie及左下角购物车
    SaveCurrOrderInCookie();
    //ShowOrderSmallCarCount();
    msgText.init("所选商品已成功加入购物车!", 3000);
    return true;
}

//保存当前购物车到缓存
function SaveCurrOrderInCookie(){
    SetCookie('currOrderInfo', JSON.stringify(currOrderInfo));
    //$.cookie('currOrderInfo', JSON.stringify(currOrderInfo), { expires:365, path: '/' }); 
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

//移除html标签
function RemoveHtmlTag(str) {
    str = str.toLowerCase();
    while (str.indexOf('</script>') > 0) {
        str = str.substring(str.indexOf('</script>') + 9);
    }
    while (str.indexOf('</style>') > 0) {
        str = str.substring(str.indexOf('</style>') + 8);
    }
    str = str.replace(/<script.*?>.*?<\/script>/ig, '');
    str = str.replace(/<\/?[^>]*>/g, ''); //去除HTML tag
    str = str.replace(/[ | ]*\n/g, '\n'); //去除行尾空白
    str = str.replace(/\n[\s| | ]*\r/g, '\n'); //去除多余空行
    str = str.replace(/&nbsp;/ig, ''); //去掉&nbsp;
    return str;
}

//判断是不是手机号码
function isPhone(value) {
    return /^(13|15|17|18)\d{9}$/i.test(value);
}

//加载商品
function LoadProductList() {
    $('#productList').html("加载中...");
    $.ajax({
        type: 'post',
        url: mallHandlerUrl,
        data: { Action: 'QueryProductsObjList', CategoryId: GetParm("cid"), sort: productSort, KeyWord: keyWord },
        dataType:'json',
        success: function (resp) {
            //if (resp.ExObj.length ==0) { return; }
            var listHtml = '';
            var str = new StringBuilder();
            for (var i = 0; i < resp.ExObj.length; i++) {
                //构造视图模板
                str.AppendFormat('<li>');
                str.AppendFormat('<a href="ProductDetail.aspx?pid={0}"><img src="{1}" /></a>', resp.ExObj[i].PID, resp.ExObj[i].RecommendImg);
                str.AppendFormat('</li>');

            };
            listHtml= str.ToString();
            if (listHtml != "") {
                //填入列表
                $('#productList').html(listHtml);

            }
            else {
                $('#productList').html("<div style=\"margin-top:50px;\">此分类暂时没有商品.</div>");
            }


        }
    });



}


////更新商品IP PV
//function UpdateProductIPPV(productid) {


//    $.ajax({
//        type: 'get',
//        url: mallHandlerUrl,
//        data: { Action: 'UpdateProductIPPV', PID: productid },
//        success: function (result) {




//        }
//    });



//}

////刷新物流费用
//function Transport_Fee() {

//    //计算物流费用
//    //读取已订购的商品
//    var pidJson = {
//        Pids: []
//    };
//    for (var i = 0; i < currOrderInfo.Products.length; i++) {
//        if (currOrderInfo.Products[i].Mark == 0) { continue; }
//        var pids = [];
//        pids.push(currOrderInfo.Products[i].PID);
//        pids.push(currOrderInfo.Products[i].TotalCount);
//        pidJson.Pids.push(pids);
//    };
//    $.ajax({
//        type: 'post',
//        url: mallHandlerUrl,
//        data: { Action: "CalcTransportFee", PIds: JSON.stringify(pidJson), DeliveryAutoId: $("#ddldelivery").val() },
//        timeout: 60000,
//        dataType: "json",
//        success: function (resp) {

//            if (resp.Status == 1) {
//                $("#sptransportFee").html(resp.ExStr);
//                var productfee = GetOrderTotalAmount().toFixed(2);
//                var totalfee = (parseFloat(productfee) + parseFloat(resp.ExStr)).toFixed(2);
//                $("#sptotal").html(totalfee);
//            }
//            else {

//            }


//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            if (textStatus == "timeout") {
//                //alert("操作超时");
//                msgText.init("操作超时", 3000);
//            }
//            else {
//                //alert(textStatus);
//                msgText.init(textStatus, 3000);
//            }
//        }
//        
//    })
//    //
//}

function clearCookie(){ 
var keys=document.cookie.match(/[^ =;]+(?=\=)/g); 
if (keys) { 
for (var i = keys.length; i--;) 
document.cookie=keys[i]+'=0;expires=' + new Date( 0).toUTCString() 
}
}

function Sort(sort) {
    productSort = sort;
    LoadProductList();


}



