/**
 * Created by Samma on 2016/8/22.
 */

angular.module('zcSelectLink', ['comm-services']);
angular.module('zcSelectLink').directive('zcSelectLink', [function () {
    return {
        restrict: 'E',
        // templateUrl:'/OpenWebAppComponent/Plugins/angular/zcSelectLink/index.html',
        templateUrl:'/Plugins/angular/zcSelectLink/index.html?v=2017031601',
        replace: false,
        scope: {
            sType:'=',
            sValue:'=',
            sText:'=',
            sLink:'=',
            sLType:'=',
            sEdit:'='
        },
        controller: function ($scope,commService,$timeout,$element) {
            $scope.vm = {
                showDiv :false,
                showDataDiv: false,
                sLType: $scope.sLType,
                sLink: $scope.sLink,
                lTypeList: ['链接', '电话', '短信'],
                typeList:['自定义','页面','文章','活动','商品','签到','优惠券','众筹','刮一刮','摇一摇','游戏','微秀','问卷','答题','投票(选题)','投票(排行)','我的','其他'],
                tagObj :{
                    MallHome:'站点首页',
                    PersonalCenter:'个人中心'
                },
                sTempType:'',
                data: [],
                searchKeyword:'',
                cate:'',
                page:1,
                rows :20,
                total :1,
                layerIndex : -1,
                progressIndex:-1
            }
            $scope.vmFunc = {
                init:init,
                toLinkPage:toLinkPage,
                selectLi:selectLi,
                searchData:searchData,
                select:select,
                haveTag:haveTag,
                getTag:getTag,
                changeSLType:changeSLType,
                changeLink:changeLink,
                rowMouseOver:rowMouseOver,
                rowMouseLeave:rowMouseLeave
            }
            $scope.vmFunc.init();
            function init(){
                if ($.trim($scope.sType) == '') $scope.sType = '自定义';
                if (!$scope.sLType) {
                    if ($scope.sLink) {
                        if ($scope.sLink.indexOf('tel:') == 0) {
                            $scope.sLType = '电话';
                            $scope.vm.sLink = $scope.sLink.replace(new RegExp('tel:', 'gm'), '');
                        } else if ($scope.sLink.indexOf('sms:') == 0) {
                            $scope.sLType = '短信';
                            $scope.vm.sLink = $scope.sLink.replace(new RegExp('sms:', 'gm'), '');
                        } else {
                            $scope.sLType = '链接';
                        }
                        $scope.vm.sLType = $scope.sLType;
                    } else {
                        $scope.sLType = '链接';
                        $scope.vm.sLType = '链接';
                    }
                } else {
                    $scope.vm.sLType = $scope.sLType;
                    if ($scope.sLink) {
                        if ($scope.sLink.indexOf('tel:') == 0) {
                            $scope.vm.sLink = $scope.sLink.replace(new RegExp('tel:', 'gm'), '');
                        } else if ($scope.sLink.indexOf('sms:') == 0) {
                            $scope.vm.sLink = $scope.sLink.replace(new RegExp('sms:', 'gm'), '');
                        }
                    }
                }
            }
            function toLinkPage(){
                if($scope.sLink.length == 0) return;
                if($scope.sLink.indexOf('tel:')== 0) return;
                if($scope.sLink.indexOf('sms:')== 0) return;
                var tLink = $scope.sLink;
                if(tLink.substr(0,1) == '/') {
                    tLink = baseDomain + tLink.substr(1,tLink.length-1);
                }
                window.open(tLink,'_blank','');
            }
            function selectLi(type){
                if($scope.vm.typeList.indexOf(type) == -1){
                    alert('即将推出，敬请期待');
                    return;
                }
                if(type == '自定义'){
                    if ($scope.sType != type) {
                        $scope.sLType = '链接';
                        $scope.vm.sLType = $scope.sLType;
                        $scope.sType = type;
                        $scope.sValue = '';
                        $scope.sText = '';
                    }
                    $scope.vm.showDiv = false;
                }
                else {
                    selectLiShowData(type);
                }
            }
            function selectLiShowData(type){
                if($scope.vm.sTempType != type) {
                    $scope.vm.searchKeyword='';
                    if(($scope.vm.sTempType == '我的'|| $scope.vm.sTempType == '其他') && (type == '我的'|| type== '其他')) $scope.sValue = '';
                    $scope.vm.sTempType = type;
                    $scope.vm.cate='';
                    $scope.vm.page = 1;
                    $scope.vm.data = [];
                    $scope.vm.total = 1;
                    if(type == '页面') getCompotentList();
                    if(type == '文章' || type == '活动') getArticleList();
                    if(type == '商品') getMallList();
                    if(type == '签到') getSignInAddressList();
                    if(type == '优惠券') getCardcouponList();
                    if(type == '众筹') getCrowdfundList();
                    if(type == '刮一刮' || type == '摇一摇') getWXLotteryV1List();
                    if(type == '游戏') getGameList();
                    if(type == '微秀') getWXShowList();
                    if(type == '问卷') getQuestionnaireList();
                    if(type == '答题') getQuestionnaireSetList();
                    if(type == '投票(选题)') getWXTheVoteList();
                    if(type == '投票(排行)') getVoteTopList();
                    if(type == '我的') getMyList();
                    if(type == '其他') getOtherList();
                }
                $scope.vm.showDiv = false;
                showDataDiv();
            }
            //提示
            function alert(msg){
                $scope.vm.layerIndex = layer.open({
                    type:3,
                    content: msg,
                    time: 200 //2秒后自动关闭
                });
            }
            //加载数据中
            function progress(){
                $scope.vm.progressIndex = layer.open({type: 2});
            }
            //弹出内容选择框
            function showDataDiv(){
                $scope.vm.showDataDiv = true;
                bindScorll();
            }
            //选择生成对应链接
            function select(row){
                if(!!$scope.sLType) $scope.sLType = '链接';
                if($scope.sEdit!=undefined)  $scope.sEdit = 1;
                if($scope.vm.sTempType == '页面') {
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.component_id;
                    $scope.sText = row.component_name;
                    $scope.sLink = row.component_link_url;
                    if($.trim(row.component_key) !=''){
                        $scope.sLink = ReplaceUrlParamToParam(row.component_link_url,'cgid','key',row.component_key);
                    }
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '文章' || $scope.vm.sTempType == '活动'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.JuActivityID;
                    $scope.sText = row.ActivityName;
                    var articleid_16 = parseInt(row.JuActivityID).toString(16); //获得十六进制文章ID
                    $scope.sLink = '/' + articleid_16 + '/details.chtml';
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '商品'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.product_id;
                    $scope.sText = row.product_title;
                    $scope.sLink = '/customize/shop/?v=1.0&ngroute=/productDetail/' + row.product_id + '#/productDetail/' + row.product_id;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '签到'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.id;
                    $scope.sText = row.address;
                    $scope.sLink = '/App/Cation/wap/LBSSigIn/SignIn.aspx?addressId=' + row.id;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '优惠券'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.cardcoupon_id;
                    $scope.sText = row.cardcoupon_name;
                    $scope.sLink = '/customize/shop/?ngroute=getcoupon/' + row.cardcoupon_id + '#/getcoupon/' + row.cardcoupon_id;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '众筹'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.crowdfund_id;
                    $scope.sText = row.crowdfund_title;
                    $scope.sLink = '/app/crowdfund/m/?ngroute=/raiseprodetail/' + row.crowdfund_id + '#/raiseprodetail/' + row.crowdfund_id;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '刮一刮'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.LotteryID;
                    $scope.sText = row.LotteryName;
                    $scope.sLink = '/App/Lottery/wap/ScratchV1.aspx?id=' + row.LotteryID;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '摇一摇'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.LotteryID;
                    $scope.sText = row.LotteryName;
                    $scope.sLink = '/customize/shake/?ngroute=/shake/' + row.LotteryID + '#/shake/' + row.LotteryID;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '游戏'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.AutoID;
                    $scope.sText = row.PlanName;
                    $scope.sLink = '/Game/Game.aspx?pid=' + row.AutoID;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '微秀'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.AutoId;
                    $scope.sText = row.ShowName;
                    $scope.sLink = '/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=' + row.AutoId;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '问卷'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.QuestionnaireID;
                    $scope.sText = row.QuestionnaireName;
                    $scope.sLink = '/App/Questionnaire/Wap/Questionnaire.aspx?id=' + row.QuestionnaireID;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '答题'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.id;
                    $scope.sText = row.title;
                    $scope.sLink = '/customize/dati/index.aspx?id=' + row.id;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '投票(选题)'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.AutoId;
                    $scope.sText = row.VoteName;
                    $scope.sLink = '/App/TheVote/wap/WxTheVoteInfo.aspx?autoid=' + row.AutoId;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '投票(排行)'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.AutoID;
                    $scope.sText = row.VoteName;
                    $scope.sLink = '/App/Cation/Wap/Vote/Comm/Index.aspx?vid=' + row.AutoID;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
                else if($scope.vm.sTempType == '我的'|| $scope.vm.sTempType == '其他'){
                    $scope.sType = $scope.vm.sTempType;
                    $scope.sValue = row.id;
                    $scope.sText = row.title;
                    $scope.sLink = row.link;
                    $scope.vm.sLink = $scope.sLink;
                    $scope.vm.showDataDiv = false;
                }
            }
            //
            function changeSLType(){
                if ($scope.sEdit != undefined) $scope.sEdit = 1;
                $scope.sLType = $scope.vm.sLType;
                $scope.sLink = $scope.sLink.replace(new RegExp('tel:', 'gm'), '').replace(new RegExp('sms:', 'gm'), '');
                $scope.vm.sLink = $scope.sLink;
                if ($scope.sLType == '电话') $scope.sLink = 'tel:' + $scope.sLink;
                if ($scope.sLType == '短信') $scope.sLink = 'sms:' + $scope.sLink;
            }
            //链接修改
            function changeLink(){
                if($scope.sEdit!=undefined) $scope.sEdit = 1;
                $scope.vm.sLink = $.trim($element.find('.nLinkText').val());
                $scope.sLink = $scope.vm.sLink;
                if ($scope.sLType == '电话') $scope.sLink = 'tel:' + $scope.vm.sLink;
                if ($scope.sLType == '短信') $scope.sLink = 'sms:' + $scope.vm.sLink;
            }
            //判断是否有页面标签
            function haveTag(row){
                return !!$scope.vm.tagObj[row.component_key];
            }
            //获取页面标签
            function getTag(row){
                if(!$scope.vm.tagObj[row.component_key]) return '';
                return $scope.vm.tagObj[row.component_key];
            }
            //链接参数替换
            function ReplaceUrlParamToParam(url, parm, toparm, value) {
                var reg = new RegExp("(^|[&?])"+ parm +"=([^&]*)([&#]|$)");
                var reg1 = new RegExp("(^|[&?])"+ toparm +"=([^&]*)([&#]|$)");
                var r = url.substr(1).match(reg);
                var r1 = url.substr(1).match(reg1);
                if(r1!=null){
                    return url;
                }
                if(r!=null){
                    return url.replace(parm+'='+r[2],toparm+'='+value);
                }
                if(url.indexOf('?')==-1){
                    return url+'?'+ toparm+'='+value;
                }
                return url.replace('?','?'+ toparm+'='+value);
            }
            //查询页面
            function getCompotentList(){
                var model = {
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    name:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"serv/api/admin/component/list.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        if(data.status){
                            $scope.vm.data  = $scope.vm.data.concat(data.result.list);
                            $scope.vm.total = data.result.totalcount;
                        }
                    },function(data){});
            }
            //查询文章或活动
            function getArticleList(){
                var ArticleType = 'Article';
                if($scope.vm.sTempType == '活动') ArticleType = 'activity';
                var model = {
                    Action:'QueryJuActivityForWeb',
                    ArticleType:ArticleType,
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    ActivityName:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/CationHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.rows);
                        $scope.vm.total = data.total;
                    },function(data){});
            }
            //查询商品
            function getMallList(){
                var model = {
                    action:'list',
                    pageindex:$scope.vm.page,
                    pagesize:$scope.vm.rows,
                    keyword:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"serv/api/admin/mall/product.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.list);
                        $scope.vm.total = data.totalcount;
                    },function(data){});
            }
            //查询签到地址
            function getSignInAddressList(){
                var model = {
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    keyword:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"serv/api/admin/signin/address/list.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        if(data.status){
                            $scope.vm.data  = $scope.vm.data.concat(data.result.list);
                            $scope.vm.total = data.result.totalcount;
                        }
                    },function(data){});
            }
            //查询优惠券
            function getCardcouponList(){
                var model = {
                    action:'list',
                    pageindex:$scope.vm.page,
                    pagesize:$scope.vm.rows,
                    cardcoupon_type:$scope.vm.cate
                };
                progress();
                commService.postData(
                    baseDomain+"serv/api/admin/mall/cardcoupon.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.list);
                        $scope.vm.total = data.totalcount;
                    },function(data){});
            }
            //查询众筹
            function getCrowdfundList(){
                var model = {
                    action:'list',
                    pageindex:$scope.vm.page,
                    pagesize:$scope.vm.rows,
                    crowdfund_type:$scope.vm.cate,
                    keyword:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"serv/api/admin/crowdfund/crowdfund.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.list);
                        $scope.vm.total = data.totalcount;
                    },function(data){});
            }
            //获取抽奖列表
            function getWXLotteryV1List(){
                var LotteryType = 'scratch';
                if($scope.vm.sTempType == '摇一摇') LotteryType = 'shake';
                var model = {
                    action:'QueryWXLotteryV1',
                    LotteryType:LotteryType,
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    LotteryName:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/CationHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.rows);
                        $scope.vm.total = data.total;
                    },function(data){});
            }
            //查询游戏列表
            function getGameList(){
                var model = {
                    Action:'QueryGamePlan',
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    planName:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/CationHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.ExObj);
                        $scope.vm.total = data.ExInt;
                    },function(data){});
            }
            //查询微秀列表
            function getWXShowList(){
                var model = {
                    Action:'GetWxShowInfos',
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    Name:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/WXShowInfoHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.rows);
                        $scope.vm.total = data.total;
                    },function(data){});
            }
            //查询问卷列表
            function getQuestionnaireList(){
                var model = {
                    Action:'QueryQuestionnaire',
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    QuestionnaireName:$scope.vm.searchKeyword,
                    QuestionnaireType:1
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/CationHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.rows);
                        $scope.vm.total = data.total;
                    },function(data){});
            }
            //查询答题列表
            function getQuestionnaireSetList(){
                var model = {
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    title:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Serv/API/Admin/QuestionnaireSet/list.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        if(data.status){
                            $scope.vm.data  = $scope.vm.data.concat(data.result.list);
                            $scope.vm.total = data.result.totalcount;
                        }
                    },function(data){});
            }
            //查询投票(选题)列表
            function getWXTheVoteList(){
                var model = {
                    Action:'GetTheVoteInfos',
                    page:$scope.vm.page,
                    rows:$scope.vm.rows,
                    VoteName:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/WXTheVoteInfoHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.rows);
                        $scope.vm.total = data.total;
                    },function(data){});
            }
            //查询投票(排行)列表
            function getVoteTopList(){
                var model = {
                    Action:'QueryVoteInfo',
                    page:$scope.vm.page,
                    rows:$scope.vm.rows
                    //,
                    //VoteName:$scope.vm.searchKeyword
                };
                progress();
                commService.postData(
                    baseDomain+"Handler/App/CationHandler.ashx",
                    model,
                    function(data){
                        layer.close($scope.vm.progressIndex);
                        $scope.vm.data  = $scope.vm.data.concat(data.rows);
                        $scope.vm.total = data.total;
                    },function(data){});
            }
            //查询我的列表
            function getMyList(){
                $scope.vm.data  = [
                    {id:1,title:'我的订单',link:'/customize/shop/?v=1.0&ngroute=/orderList#/orderList'},
                    {id:2,title:'我的活动',link:'/App/Cation/Wap/MyActivityLlists.aspx'},
                    {id:3,title:'我的积分',link:'/customize/shop/?v=1.0&ngroute=/myscores#/myscores'},
                    {id:4,title:'我的礼品',link:'/customize/shop/?ngroute=/giftorderlist/0#/giftorderlist/0'},
                    {id:5,title:'我的优惠券',link:'/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons'},
                    {id:6,title:'我的团购',link:'/customize/shop/Index.aspx?v=1.0&ngroute=/groupGoodsList#/groupGoodsList'},
                    {id:7,title:'我是代言人',link:'/App/Cation/Wap/Mall/Distribution/Index.aspx'},
                    {id:8,title:'我的二维码',link:'/App/Cation/Wap/Mall/Distribution/MyDistributionQCode.aspx'},
                    {id:9,title:'我的转发',link:'/App/Forward/wap/allforwardlistwap.aspx'},
                    {id:10,title:'我的商机',link:'/app/distribution/m/index.aspx?ngroute=/mystatic#/mystatic'},
                    {id:11,title:'我的信息',link:'/customize/shop/?v=1.0&ngroute=/userMange#/userMange'},
                    { id: 12, title: '我的订单-待付款', link: '/customize/shop/?v=1.0&ngroute=/orderList%3fstatus%3d待付款#/orderList?status=待付款' },
                    { id: 13, title: '我的订单-待发货', link: '/customize/shop/?v=1.0&ngroute=/orderList%3fstatus%3d待发货#/orderList?status=待发货' },
                    { id: 14, title: '我的订单-待收货', link: '/customize/shop/?v=1.0&ngroute=/orderList%3fstatus%3d待收货#/orderList?status=待收货' },
                    { id: 15, title: '我的订单-待评价', link: '/customize/shop/?v=1.0&ngroute=/orderList%3fstatus%3d待评价#/orderList?status=待评价' },
                    { id: 16, title: '我的订单-待退款', link: '/customize/shop/?v=1.0&ngroute=/orderList%3fstatus%3d待退款#/orderList?status=待退款' },
                    { id: 17, title: '用户信息-设置', link: '/customize/shop/?v=1.0#/userInfo' }
                ];
                $scope.vm.total = 0;
            }
            //查询其他列表
            function getOtherList(){
                $scope.vm.data  = [
                    {id:1,title:'转发大厅',link:'/App/Forward/wap/allforwardlistwap.aspx'},
                    {id:2,title:'限时特卖',link:'/customize/shop/?v=1.0&ngroute=/saleList#/saleList'},
                    {id:3,title:'全部商品',link:'/customize/shop/?v=1.0&ngroute=/productList#/productList'},
                    {id:4,title:'话题',link:'/App/Review/M/List.aspx'},
                    {id:5,title:'活动列表',link:'/App/Cation/Wap/ActivityLlists.aspx'},
                    {id:6,title:'站点首页',link:'/customize/comeoncloud/Index.aspx?key=MallHome'},
                    {id:7,title:'个人中心',link:'/customize/comeoncloud/Index.aspx?key=PersonalCenter'},
                    {id:8,title:'用户信息',link:'/customize/shop/?v=1.0&ngroute=/userMange#/userMange'},
                    {id:9,title:'上一页', link: 'javascript:zcBack();' },
                    {id:10,title:'空链接',link:'javascript:void(0);'},
                    { id: 11, title: 'empty', link: 'empty' }
                ];
                $scope.vm.total = 0;
            }
            //搜索事件
            function searchData(){
                $scope.vm.page = 1;
                $scope.vm.data = [];
                $scope.vm.total = 1;
                if($scope.vm.sTempType == '页面') getCompotentList();
                if($scope.vm.sTempType == '文章' || $scope.vm.sTempType == '活动') getArticleList();
                if($scope.vm.sTempType == '商品') getMallList();
                if($scope.vm.sTempType == '签到') getSignInAddressList();
                if($scope.vm.sTempType == '优惠券') getCardcouponList();
                if($scope.vm.sTempType == '众筹') getCrowdfundList();
                if($scope.vm.sTempType == '刮一刮' || $scope.vm.sTempType == '摇一摇') getWXLotteryV1List();
                if($scope.vm.sTempType == '游戏') getGameList();
                if($scope.vm.sTempType == '微秀') getWXShowList();
                if($scope.vm.sTempType == '问卷') getQuestionnaireList();
                if($scope.vm.sTempType == '答题') getQuestionnaireSetList();
                if($scope.vm.sTempType == '投票(选题)') getWXTheVoteList();
                if($scope.vm.sTempType == '投票(排行)') getVoteTopList();
                if($scope.vm.sTempType == '我的') getMyList();
                if($scope.vm.sTempType == '其他') getOtherList();

            }
            //绑定滚动事件
            function bindScorll(){
                $timeout(function(){
                    $($element).find('.listSelectDiv').scroll(function(){
                        if(($scope.vm.data.length < $scope.vm.total)
                            && (($(this).get(0).scrollHeight - $(this).height() - $(this).scrollTop()) <= 5)){
                            $scope.vm.page ++;
                            if($scope.vm.sTempType == '页面') getCompotentList();
                            if($scope.vm.sTempType == '文章' || $scope.vm.sTempType == '活动') getArticleList();
                            if($scope.vm.sTempType == '商品') getMallList();
                            if($scope.vm.sTempType == '签到') getSignInAddressList();
                            if($scope.vm.sTempType == '优惠券') getCardcouponList();
                            if($scope.vm.sTempType == '众筹') getCrowdfundList();
                            if($scope.vm.sTempType == '刮一刮' || $scope.vm.sTempType == '摇一摇') getWXLotteryV1List();
                            if($scope.vm.sTempType == '游戏') getGameList();
                            if($scope.vm.sTempType == '微秀') getWXShowList();
                            if($scope.vm.sTempType == '问卷') getQuestionnaireList();
                            if($scope.vm.sTempType == '答题') getQuestionnaireSetList();
                            if($scope.vm.sTempType == '投票(选题)') getWXTheVoteList();
                            if($scope.vm.sTempType == '投票(排行)') getVoteTopList();
                            if($scope.vm.sTempType == '我的') getMyList();
                            if($scope.vm.sTempType == '其他') getOtherList();
                        }
                    });
                })
            }
            //鼠标移到
            function rowMouseOver(e){
                if($(e.currentTarget).hasClass('rowSelected')) return;
                if(!$(e.currentTarget).hasClass('rowMouseOver')) $(e.currentTarget).addClass('rowMouseOver');
            }
            //鼠标移开
            function rowMouseLeave(e){
                if($(e.currentTarget).hasClass('rowSelected')) return;
                if($(e.currentTarget).hasClass('rowMouseOver')) $(e.currentTarget).removeClass('rowMouseOver');
            }
        },
        link:function (scope, element) {
        }
    }
}]);
