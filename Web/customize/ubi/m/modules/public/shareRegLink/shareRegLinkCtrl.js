ubimodule.controller('shareRegLinkCtrl', ['$scope', 'commService', 'userService', 'commArticle', '$routeParams',
    function ($scope, commService, userService, commArticle, $routeParams) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '分享给朋友',
            pId: $routeParams.id
        };

        document.title = pageData.title;

        pageFunc.init = function () {
            pageFunc.shareToFriend();
        };

        pageFunc.shareToFriend = function () {
            wx.ready(function () {
                wxapi.wxshare({
                    title: '易劳-注册分享',
                    desc: '分享给朋友注册成功得积分',
                    link: "http://www.elao360.com/m#/reg/" + pageData.pId
                }, '')
            });
        }
        pageFunc.init();
    }]);
