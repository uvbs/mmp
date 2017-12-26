/**
 * 需要引用
 * jquery 2.0 +
 * jquery-ui-sortable
 * 需要时添加less中的样式
 */

angular.module('zcShop', ['comm-services']);
angular.module('zcShop').directive('zcShop', ['$timeout', function ($timeout) {
    return {
        restrict: 'E',
        //templateUrl:'/OpenWebAppComponent/Plugins/angular/zcShop/index.html',
        templateUrl: '/Plugins/angular/zcShop/index.html?v=2017030301',
        replace: false,
        scope: {
            ids: '=',
            cates: '=',
            tags: '=',
            goodList: '='
        },
        controller: function ($scope, commService, $timeout, $element) {
            if (!$scope.goodList) $scope.goodList = [];
            $scope.vm = {
                showDataDiv :false,
                data: [],
                select_id_list:[],
                searchKeyword:'',
                cate:'',
                tag:'',
                page:1,
                rows :20,
                total :1,
                layerIndex : -1,
                progressIndex:-1
            }
            $scope.vmFunc = {
                init:init,
                showDataDiv:showDataDiv,
                search: search,
                select: select,
                delLi: delLi,
                changeIndex: changeIndex
            }
            $scope.vmFunc.init();
            function init() {
                console.log('cates', $scope.cates);
                console.log('tags', $scope.tags);
                getProductList(false);
                loadInitData();
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
            function select(item) {
                var oIndex = -1;
                for (var i = 0; i < $scope.vm.select_id_list.length; i++) {
                    if ($scope.vm.select_id_list[i] == item.product_id) oIndex = i;
                }
                if (oIndex >= 0) {
                    $scope.vm.select_id_list.splice(oIndex, 1);
                    $scope.goodList.splice(oIndex, 1);
                } else {
                    $scope.vm.select_id_list.push(item.product_id);
                    var itemData = angular.fromJson(angular.toJson(item));
                    $scope.goodList.push(itemData);
                }
                $scope.ids = $scope.vm.select_id_list.join(',');
            }
            function delLi(item) {
                var oIndex = -1;
                for (var i = 0; i < $scope.vm.select_id_list.length; i++) {
                    if ($scope.vm.select_id_list[i] == item.product_id) oIndex = i;
                }
                $scope.vm.select_id_list.splice(oIndex, 1);
                $scope.goodList.splice(oIndex, 1);
                $scope.ids = $scope.vm.select_id_list.join(',');
            }
            function loadInitData() {
                for (var i = 0; i < $scope.goodList.length; i++) {
                    $scope.vm.select_id_list.push($scope.goodList[i].product_id);
                }
            }
            //切换类型
            function search(){
                $scope.vm.page = 1;
                $scope.vm.data = [];
                $scope.vm.total = 1;
                getProductList(true);
            }
            //查商品
            function getProductList(hasProgress) {
                if(hasProgress) progress();
                commService.postData(
                    baseDomain+"serv/api/mall/product.ashx",
                    {
                        action: 'list',
                        category_id: $scope.vm.cate,
                        tags: $scope.vm.tag,
                        keyword: $scope.vm.searchKeyword,
                        pageindex:$scope.vm.page,
                        pagesize:$scope.vm.rows
                    },
                    function(data){
                        if(hasProgress) layer.close($scope.vm.progressIndex);
                        if(data.list) $scope.vm.data  = $scope.vm.data.concat(data.list);
                        $scope.vm.total = data.totalcount;
                    },function(data){});
            }

            //绑定滚动事件
            function bindScorll(){
                $timeout(function(){
                    $($element).find('.listSelectDiv').scroll(function(){
                        if(($scope.vm.data.length < $scope.vm.total)
                            && (($(this).get(0).scrollHeight - $(this).height() - $(this).scrollTop()) <= 5)){
                            $scope.vm.page ++;
                            getProductList(true);
                        }
                    });
                })
            }

            function changeIndex(toIds) {
                $scope.ids = toIds.join(',');
                var select_data = angular.fromJson(angular.toJson($scope.goodList));
                $scope.vm.select_id_list = toIds;
                $scope.goodList = [];
                $scope.$apply(function () {
                    for (var j = 0; j < toIds.length; j++) {
                        for (var i = 0; i < select_data.length; i++) {
                            if (toIds[j] == select_data[i].product_id) {
                                $scope.goodList.push(select_data[i]);
                                break;
                            }
                        }
                    }
                });
                //$scope.vm.select_id_list = [];
                //$scope.goodList = [];
                //    $scope.$apply(function () {
                //        $scope.vm.select_id_list = toIds;
                //        $scope.goodList = select_data;
                //        console.log('select_data', select_data);
                //        console.log('toIds', toIds.join(','));
                //        $scope.ids = toIds.join(',');
                //    });
            }
        },
        link:function (scope, element) {
            $(element).find('.selectResultUl').sortable({
                delay: 300,
                forcePlaceholderSize: true,
                opacity: 0.6,
                scroll: false,//关闭滚动事件
                placeholder: 'ui-state-highlight',
                update: function (event, ui) {
                    var ids = [];
                    $(element).find('.selectResultUl [data-id]').each(function () {
                        var cid = $(this).attr('data-id');
                        ids.push(cid);
                    });
                    console.log('update', ids);
                    scope.vmFunc.changeIndex(ids);
                }
            });
        }
    }
}]);
