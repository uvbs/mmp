ubimodule.controller('askListCtrl', ['$scope', '$routeParams', 'commArticle', 'commService', '$location', 'ngDialog',
    function ($scope, $routeParams, commArticle, commService, $location, ngDialog) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '问答 - ' + baseData.slogan,
            typeList: [{ id: 0, name: '发文章' }, { id: 1, name: '发问题' }],//发表类型
            currType: [],//当前选中类型
            currPath: base64encode('#/ask'),
            currUser: commService.getCurrUserInfo(),
        };

        document.title = pageData.title;
        ////判断是否登录页面跳转函数
        //pageFunc.go = function (currPath) {
        //    var url = "#/login/" + currPath;
        //    window.location.href = url;
        //}
        //跳转到所选择的发表类型页面
        pageFunc.go = function (item) {
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                var url = "";
                if (item.id == 0) {
                    url = "#/askArticle";
                }
                else if (item.id == 1) {
                    url = "#/askqu";
                }
                window.location.href = url;
            } 
        };
        //发表选择弹框  
        pageFunc.showPublishDialog = function () {
            console.log(1);
            ngDialog.open({
                template: basePath + 'modules/public/ask/list/tpls/publishTypeDialog.html',
                plain: false,
                scope: $scope
            });
        };
        //发表类型选择
        pageFunc.publishTypeChoose = function (item) {
            pageData.currType = item;
            pageFunc.go(item);
            ngDialog.closeAll();
        }
        //初始化
        pageFunc.init = function () {
            if (pageData.currType) {
                pageData.currType = pageData.typeList[0];
            }
        }
        pageFunc.init();

    }]);