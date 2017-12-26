document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
    WeixinJSBridge.call('hideToolbar');
});

//用户实体
var currUserInfo = {
    UserID: '',
    TrueName: '',
    Phone: '',
    Address: '',
    WXHeadimgurlLocal: ''
};
//订购详单
//var orderDetailsModel = {
//    PID: '',
//    Price: 0.0,
//    TotalCount: 0,
//    PName:'',
//    Img:'',
//    Mark:1
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

var hfHandlerUrl = '/Handler/App/WXMallHandler.ashx';
var userHandlerUrl = '/Handler/User/UserHandler.ashx';

var currShowProduct = null;//用于展示页当前的商品实体

$(function () {
    //delCookie('currOrderInfo');

    GetCookieData();
    ShowOrderSmallCarCount();
    IniPageAction();
    BtnEvenBind();

});

//获取cookie缓存数据
function GetCookieData() {

    //获取当前用户
    if (getCookie('currUserInfo') == null || getCookie('currUserInfo') == '') {
        //获取用户信息
        $.ajax({
            type: 'post',
            url: userHandlerUrl,
            data: { Action: 'GetUserInfo' },
            success: function (result) {
                var resp = $.parseJSON(result);
                currUserInfo.UserID = resp.ExObj.UserID;
                currUserInfo.TrueName = resp.ExObj.TrueName;
                currUserInfo.Phone = resp.ExObj.Phone;
                currUserInfo.Address = resp.ExObj.Address;
                currUserInfo.WXHeadimgurlLocal = resp.ExObj.WXHeadimgurlLocal;

                //设置用户信息到cookie
                SetCookie('currUserInfo', JSON.stringify(currUserInfo));
            }
        });
    }
    else {
        currUserInfo = $.parseJSON(getCookie('currUserInfo'));
    }

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

    if (pathName.indexOf('list.aspx') > 0) {
        pageAction = 'list';
    }else if (pathName.indexOf('show.aspx') > 0) {
        pageAction = 'show';
    }else if (pathName.indexOf('order.aspx') > 0) {
        pageAction = 'order';
    }

    switch(pageAction){
        case 'list':
            AlertProcessger('正在加载商品列表...', '20%');
            $.ajax({
                type: 'post',
                url: hfHandlerUrl,
                data: { Action: 'QueryProductsObjList' },
                success: function (result) {
                    var resp = $.parseJSON(result);
                    if (resp.ExObj == null) { return; }
                    var listHtml = '';
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        var str = new StringBuilder();
                        str.AppendFormat('<li>');
                        str.AppendFormat('  <a href="Show.aspx?action=show&pid={0}" data-pid="{0}">', resp.ExObj[i].PID);
                        str.AppendFormat('      <img src="{0}" alt="" />', resp.ExObj[i].RecommendImg);
                        //str.AppendFormat('      <p class="ProText">{0}</p>', RemoveHtmlTag(resp.ExObj[i].PDescription));
                        str.AppendFormat('      <p class="ProText">{0}</p>', RemoveHtmlTag(resp.ExObj[i].PName));
                        str.AppendFormat('      <p class="ProPrince">￥{0}</p>', resp.ExObj[i].Price);
                        str.AppendFormat('  </a>');
                        str.AppendFormat('</li>');
                        listHtml += str.ToString();
                    };
                    //填入列表
                    $('.ProList').html(listHtml);
                    AlertMsgHide();
                }
            });
            break;
        case 'show':
            var pid = GetParm('pid');
            $.ajax({
                type:'post',
                url:hfHandlerUrl,
                data:{Action:'GetProductObj',PID:pid},
                success:function(result){
                    var resp = $.parseJSON(result);
                    currShowProduct = resp.ExObj;
                    //alert(JSON.stringify(currShowProduct));

                    //以下为测试数据
                    // currShowProduct = {
                    //     PID:'1',
                    //     PName:'航拍1',
                    //     PDescription:'泰国ele睡眠面膜保湿补水美白面膜细嫩滑面膜.ELE含有丰富竹炭，从而消除在你脸上的毒素从而达到清洁皮肤使皮肤光滑的目的。 刺激血液循环，增加氧气的皮肤，让脸部进行有氧呼吸。即时提升肌肤的弹性，深层美白，极度深层护理的效果是普通面膜的10倍 增强和抑制胶原蛋白的破坏 有助于减少皱纹，抗氧化剂 平滑滋润肌肤，减少黑眼圈。消除斑纹 舒缓肌肤，在睡眠期间为肌肤导入源源不断的水份与活力，在睡眠中为你的肌肤保驾护航 ELE属于睡眠面膜睡前涂抹面霜，放置约15-20分钟。可以清洗或翌日清洗。 面霜将会在睡眠期间深度渗透到脸部皮肤组织，深入滋养肌肤。在早晨醒来。 你的脸会会立刻感觉到皮肤光滑明亮白皙水嫩。',
                    //     Price:188,
                    //     UserID:'hf',
                    //     RecommendImg:'http://wd.geilicdn.com/vshop882137-1392008118-1.jpg?w=480&h=360&cp=1'
                    // }

                    ShowProductDetails(currShowProduct);
                }
            });
            break;
        case 'order':
            /*
                购物车页面加载进来，读取缓存中已选择的商品，展示已选商品到购物车
                自动填写用户信息到表单
            */

            if (currOrderInfo!=null) {
                var listHtml = '';
                for (var i = 0; i < currOrderInfo.Products.length; i++) {
                    //构造视图模板
                    var str = new StringBuilder();
                    str.AppendFormat('<li class="OrderItem" data-pid="{0}">',currOrderInfo.Products[i].PID);
                    str.AppendFormat('  <mark class="CartMask CartMaskCheck"></mark>');
                    str.AppendFormat('  <img src="{0}" alt="{1}" />',currOrderInfo.Products[i].Img,currOrderInfo.Products[i].PName);
                    str.AppendFormat('  <div class="ProText">{0}</div>',currOrderInfo.Products[i].PName);
                    str.AppendFormat('  <div class="OrderPrinceControl">');
                    str.AppendFormat('      <div class="ProPrince">￥{0}</div>',currOrderInfo.Products[i].Price);
                    str.AppendFormat('      <div class="ControlBtnSub">-</div>');
                    str.AppendFormat('      <div class="ControlTxtNum">{0}</div>',currOrderInfo.Products[i].TotalCount);
                    str.AppendFormat('      <div class="ControlBtnAdd">+</div>');
                    str.AppendFormat('  </div>');
                    str.AppendFormat('<li>');
                    listHtml+=str.ToString();
                };
                $('.OrderList>ul').html(listHtml);
                if (listHtml == '') {
                    $('.OrderList>ul').html('<div class="TipMsg">您还未选购任何商品</div>');
                }

                RefreshTotalPrinceTotalCountView();

                //自动填充姓名手机地址
                $('#txtLinkerName').val(currUserInfo.TrueName);
                $('#txtLinkerPhone').val(currUserInfo.Phone);
                $('#txtLinkerAddress').val(currUserInfo.Address);

            }
            break;
        default:
            return;
    }
}

//按钮事件绑定
function BtnEvenBind(){
    //添加到购物车按钮
    $('#btnAddInOrder').click(function(){
        if (currShowProduct != null) {
            AddInOrder(currShowProduct.PID,currShowProduct.Price,currShowProduct.PName,currShowProduct.RecommendImg);
        };
    });
    //立即购买按钮
    $('#btnBuyNow').click(function(){
         if (currShowProduct != null) {
            if (AddInOrder(currShowProduct.PID,currShowProduct.Price,currShowProduct.PName,currShowProduct.RecommendImg)) {
                AlertMsg('正在转到购物车。。');
                window.location.href = 'Order.aspx';
            }//加入购物车后立即跳转到购物车
        };
    });

    //购物车 “-” 按钮
    $('.ControlBtnSub').on('click',function(){
        var pid = $(this).closest('.OrderItem').attr('data-pid');
        UpdateCurrOrderProductCount(pid,-1);
        RefreshTotalPrinceTotalCountView();
    });

    //购物车 “+” 按钮
    $('.ControlBtnAdd').on('click',function(){
        var pid = $(this).closest('.OrderItem').attr('data-pid');
        UpdateCurrOrderProductCount(pid,1);
        RefreshTotalPrinceTotalCountView();
    });

    //购物车 "选择" 按钮
    $('.CartMask').on('click',function(){
       var pid = $(this).closest('.OrderItem').attr('data-pid');
       if ($(this).hasClass('CartMaskCheck')) {
        //已选
            $(this).removeClass('CartMaskCheck');
            UpdateCurrOrderProductMark(pid,0);//更改为未选
            RefreshTotalPrinceTotalCountView();
       }else{
        //未选
            $(this).addClass('CartMaskCheck');
            UpdateCurrOrderProductMark(pid,1);//更改为已选
            RefreshTotalPrinceTotalCountView();
       }
       ;
    });

    //购物车 "提交订单按钮" 按钮
    $('.OrderCash').on('click', function () {

        if (GetOrderTotalCount() == 0) {
            AlertMsg('购物车没有可提交商品');
            return;
        }

        //读取已订购的商品
        var pidJson = {
            Pids: []
        };
        for (var i = 0; i < currOrderInfo.Products.length; i++) {
            if (currOrderInfo.Products[i].Mark == 0) { continue;}
            var pids = [];
            pids.push(currOrderInfo.Products[i].PID);
            pids.push(currOrderInfo.Products[i].TotalCount);
            pidJson.Pids.push(pids);
        };
        //{"pids":[["130568",3],["130567",1]]}

        var ajaxData = {
            PIds: JSON.stringify(pidJson),
            Consignee:$.trim($('#txtLinkerName').val()),
            Phone:$.trim($('#txtLinkerPhone').val()),
            Address:$.trim($('#txtLinkerAddress').val()),
            OrderMemo:$.trim($('#txtOrderMemo').val()),
            Action:'SubmitWxMallOrder'
        }

        if (ajaxData.Consignee == '') {
            AlertMsg('请输入收货人姓名');
            return;
        }
        if (ajaxData.Phone == '') {
            AlertMsg('请输入可联系的手机号码');
            return;
        }

        if (!isPhone(ajaxData.Phone)) {
            AlertMsg('手机号码格式有误');
            return;
        }


        AlertProcessger('正在提交订单。。。','20%');

        setTimeout(function(){
            $.ajax({
                type:'post',
                url:hfHandlerUrl,
                data:ajaxData,
                success:function(result){
                    AlertMsgHide();
                    var resp = $.parseJSON(result);
                    if (resp == null||resp == '') {
                        AlertMsg('订单提交失败');
                        return;
                    }

                    if (resp.Status == 1) {
                        //订单提交成功，清空购物车并跳转回首页
                        ClearOrder();
                        AlertMsg('订单提交成功!','');
                        return;
                    }
                    else{
                        if (resp.Msg == '') {
                            resp.Msg = '订单提交失败';
                        }
                        AlertMsg(resp.Msg);
                        return;
                    }
                }
            });
        },2000);
        
    });

}

//清空购物车
function ClearOrder() {
    delCookie('currUserInfo');
    delCookie('currOrderInfo');
    currOrderInfo = null;
}

//刷新界面总数跟总金额界面视图
function RefreshTotalPrinceTotalCountView(){
    $('.TotalPrince').html('￥' + GetOrderTotalAmount());
    $('.TotalCount').html(GetOrderTotalCount());
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

//更改购物车中指定商品数量
function UpdateCurrOrderProductCount(pid,num){
    if (currOrderInfo == null) {return;}
    for (var i = 0; i < currOrderInfo.Products.length; i++) {
        if(currOrderInfo.Products[i].PID == pid){
            currOrderInfo.Products[i].TotalCount = currOrderInfo.Products[i].TotalCount + num;
            if (currOrderInfo.Products[i].TotalCount == 0) {
                AlertMsg('亲，至少买一件嘛');
                currOrderInfo.Products[i].TotalCount = 1;
            }

            //更新html数量统计
            $('.OrderItem[data-pid="' + pid +'"]').find('.ControlTxtNum').html(currOrderInfo.Products[i].TotalCount);

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

//显示左下角小车商品数
function ShowOrderSmallCarCount(){
    $('#CarCount').html(GetOrderTotalCount());
}

//展示商品详情
function ShowProductDetails(pro){    

    $('#lbPName').html(pro.PName);

    //添加头部图片
    $('.ProRecommendImg').html('<img alt="商品展示" src="' + pro.RecommendImg + '" />');

    //填充商品说明
    $('.ProDescription').html(pro.PDescription);

    //写入价格
    $('.ProPrince').html( "￥" + pro.Price);    
}

function AddInOrder(pid,price,pname,img){
    /*
        添加一件商品到购物车
        判断商品是否已在购物车
        如果已经购物车，则数量加1
    */

    if (currOrderInfo == null) {
        currOrderInfo = {
            Products: [{PID:pid,Price:price,TotalCount:1,PName:pname,Img:img,Mark:1}],
            OrderUserID: '',
            Address: '',
            Phone: '',
            TotalAmount: 0.0,
            OrderMemo: ''
        };
    }else{
        var isHaving = false;
        for (var i = 0; i < currOrderInfo.Products.length; i++) {
            if (currOrderInfo.Products[i].PID == pid) {
                currOrderInfo.Products[i].TotalCount++;
                currOrderInfo.Products[i].Mark = 1;
                isHaving = true;
                break;
            }
        };
        if (!isHaving) {
            currOrderInfo.Products.push({PID:pid,Price:price,TotalCount:1,PName:pname,Img:img,Mark:1});
        };
    }

    //更新cookie及左下角购物车
    SaveCurrOrderInCookie();
    ShowOrderSmallCarCount();
    return true;
}

//保存当前购物车到缓存
function SaveCurrOrderInCookie(){
    SetCookie('currOrderInfo', JSON.stringify(currOrderInfo));
}

function AlertProcessger(msg,left){
    var str = new StringBuilder();
    str.AppendFormat('<img src="/img/load_dot.gif" style="width:30px" />&nbsp;{0}',msg);
    AlertMsgShow(str.ToString(),left);
}

//弹出信息框
function AlertMsg(msg){
    if (msg == '') {
        $('#AlertContent').html('商品已在购物车咯');
    }
    else{
        $('#AlertContent').html(msg);
    }

    $('.alertBg').show();
    $('#AlertContent').show();
    $('#AlertContent').css('opacity','1');
    setTimeout(function(){
        $('#AlertContent').css('opacity','0.01');
        $('.alertBg').hide();
    },1200);
}

//弹出对话框，需要手动关闭
function AlertMsgShow(msg,left){
    if (msg == '') {
        $('#AlertContent').html('商品已在购物车咯');
    }
    else{
        $('#AlertContent').html(msg);
    }
    if (left == '') {
        $('#AlertContent').css('left','30%');
    }else{
        $('#AlertContent').css('left',left);
    }
    $('.alertBg').show();
    $('#AlertContent').show();
    $('#AlertContent').css('opacity','1');
}

function AlertMsgHide(){
    $('#AlertContent').css('opacity','0.01');
    $('#AlertContent').hide();
    $('.alertBg').hide();
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
    return /^(13|15|18)\d{9}$/i.test(value);
}

$("div[data-categoryid]").click(function () {
    var categoryid = $(this).attr("data-categoryid");
    $(".TopCategory>div").css({ "background-color": "#fff" });
    $(this).css({ "background-color": "#d74300" });
    if (categoryid != "") {
        AlertProcessger('正在加载商品列表...', '20%');
        $.ajax({
            type: 'post',
            url: hfHandlerUrl,
            data: { Action: 'QueryProductsObjList', CategoryId: categoryid },
            success: function (result) {
                var resp = $.parseJSON(result);
                if (resp.ExObj == null) { return; }
                var listHtml = '';
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    var str = new StringBuilder();
                    str.AppendFormat('<li>');
                    str.AppendFormat('  <a href="Show.aspx?action=show&pid={0}" data-pid="{0}">', resp.ExObj[i].PID);
                    str.AppendFormat('      <img src="{0}" alt="" />', resp.ExObj[i].RecommendImg);
                    str.AppendFormat('      <p class="ProText">{0}</p>', RemoveHtmlTag(resp.ExObj[i].PName));
                    str.AppendFormat('      <p class="ProPrince">￥{0}</p>', resp.ExObj[i].Price);
                    str.AppendFormat('  </a>');
                    str.AppendFormat('</li>');
                    listHtml += str.ToString();
                };
                //填入列表
                $('.ProList').html(listHtml);
                AlertMsgHide();
            }
        });


    }

});


