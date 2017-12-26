/**
 * Created by add on 2016/1/25.
 */
distributionmodule.directive('header',function(){
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/header/tpls/index.html',
        replace: true,
        scope: {
            title:'@', //标题
            btnshow:'@',  //是否显示标题 0不显示 1显示
            isshow:'@'
        },
        controller:function($scope){
            var pageFunc=$scope.pageFunc={};
            var pageData=$scope.pageData={
                title:'财富中心',
                showBtn:$scope.btnshow,  //0不显示 1显示
                showBack:false,
                isshow:$scope.isshow
            };
            document.title = pageData.title;
            pageFunc.init=function(){
                //pageData.showBack = document.referrer != '';
                if($scope.title){
                    pageData.title = $scope.title;
                    document.title = pageData.title;
                }
            };
            pageFunc.init();
        }
    }
});
