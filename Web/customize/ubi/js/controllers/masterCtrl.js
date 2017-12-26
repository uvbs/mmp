ubimodule.controller("masterCtrl", ['$scope', 'commArticle', 'commService', 'userService',
  function ($scope, commArticle, commService, userService) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
      title: '专家库 - ' + baseData.slogan,
      pageIndex: 1,
      pageSize: 5,
      sort: '',
      totalCount: 0,
      totalAllCount: 0,
      endInit: false,
      cateList: [{
        id:'',
        name: '全部'
      }],
      cateOtherList: [],
      showOther: false,
      currSelectCate: {
          id: '',
          name: '全部'
      },
      TutorList: [],
      keyword: '',
      currUser: commService.getCurrUserInfo()
    };

    document.title = pageData.title;

    pageFunc.init = function() {
        pageFunc.loadCateData();
        pageFunc.loadTutorList();
    };


    /**
     * [loadCateData 加载省份列表数据]
     * @return {[type]} [description]
     */
    pageFunc.loadCateData = function() {
        commArticle.getGetKeyVauleDatas('province', null, "1", function (data) {
        if (data && data.list) {
          for (var i = 0; i < data.list.length; i++) {
            if (i < 8) {
              pageData.cateList.push(data.list[i]);
            } else {
              pageData.cateOtherList.push(data.list[i]);
            }
          }
        }
      }, function() {});
    };

    pageFunc.loadTutorList = function() {
      var province = pageData.currSelectCate.name == "全部" ? "" : pageData.currSelectCate.id;
      commArticle.getTutors(pageData.pageIndex, pageData.pageSize, province, "", pageData.keyword, pageData.sort, function(data) {
          if (!pageData.endInit) {
              pageData.totalAllCount = data.totalallCount;
              pageData.totalCount = data.totalcount;
              pageData.initEnd = true;
          }
          for (var i = 0; i < data.list.length;i++) {
              pageData.TutorList.push(data.list[i]);
          }
          pageData.pageIndex++;
      }, function() {});
    };
    /**
     * [selectCate 选择省份]
     */
    pageFunc.selectCate = function (item) {
        if (pageData.currSelectCate != item) {
            pageData.currSelectCate = item;
            pageData.pageIndex = 1;
            pageData.TutorList = [];
            pageFunc.loadTutorList();
        }
    };
    pageFunc.setSort = function (item) {
        if (pageData.sort != item) {
            pageData.sort = item;
            pageData.pageIndex = 1;
            pageData.TutorList = [];
            pageFunc.loadTutorList();
        }
    };
    pageFunc.showOtherCate = function myfunction() {
      pageData.showOther = !pageData.showOther;
    };

    pageFunc.submitFollow = function (item) {
        userService.followUser(item.userId, function (data) {
            if (data.isSuccess) {
                item.userIsFollow = true;
                item.followUserCount += 1;
            } else if (data.errcode == 10010) {
                $scope.showLogin(function () {
                    pageFunc.submitFollow(item);
                    pageFunc.loadTutors();
                }, '您取消了登陆，关注专家必需先登录');
            } else {
                alert('关注失败');
            }
        }, function (data) {
            alert('关注失败');
        });
    }

    pageFunc.submitDisFollow = function (item) {
        userService.disFollowUser(item.userId, function (data) {
            if (data.isSuccess) {
                item.userIsFollow = false;
                item.followUserCount -= 1;
            } else if (data.errcode == 10010) {
                $scope.showLogin(function () {
                    pageFunc.submitDisFollow(item);
                    pageFunc.loadTutors();
                }, '您取消了登陆，取消关注专家必需先登录');
            } else {
                alert('取消关注失败');
            }
        }, function (data) {
            alert('取消关注失败');
        });
    }

    pageFunc.init();
  }
]);