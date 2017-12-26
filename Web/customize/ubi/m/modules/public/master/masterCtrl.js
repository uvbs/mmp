ubimodule.controller('masterCtrl', ['$scope', 'commArticle', 'commService', function ($scope, commArticle, commService) {

    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '专家库 - ' + baseData.slogan,
        list: [],
        totalCount: 0,
        totalAllCount:0,
        pageSize: 5,
        pageIndex: 1,
        //province: '',
        keyword: '',

        province: null,//当前选中省份
        provinceList: [],//所有省份列表
        sort: '',
        endInit: false,
    };

    document.title = pageData.title;

    //获取省份
    pageFunc.selectProvince = function () {
        var ProChoose = { id: "0", name: "请选择省份" };
        //获取省份
        commService.getGetKeyVauleDatas({
            type: 'province'
        }, function (data) {
            pageData.provinceList = data.list;
            pageData.provinceList.unshift(ProChoose);
            pageData.province = pageData.provinceList[0];
            pageFunc.loadData();
            console.log(data);
        }, function () { });
    };

    pageFunc.loadData = function () {
        //var provinceId = "";
      //  if()
        if (pageData.province.id == 0)
        {
            pageData.province.id = '';
        }
        commArticle.getTutors(
			pageData.pageIndex,
			pageData.pageSize,
			pageData.province.id,
			'',
			pageData.keyword,
			pageData.sort,
			function (data) {
			    console.log(data);
			    pageData.list = pageData.list.concat(data.list);
			    pageData.totalAllCount = data.totalallCount;
			    pageData.totalCount = data.totalCount;
			    pageData.pageIndex++;
			},
			function (data) {

			}
		);
    };
    /**
    * [selectCate 选择省份]
    */
    pageFunc.selectCate = function (item) {
        //if (pageData.province != item) {
        //    pageData.province = item;
            pageData.pageIndex = 1;
            pageData.list = [];
            pageFunc.loadData();
            if(pageData.province!=null)
            {
                pageData.province.id = "";
            }
      //  }
    };
    //排序
    pageFunc.setSort = function (item) {
        if (pageData.sort != item) {
            pageData.sort = item;
            pageData.pageIndex = 1;
            pageData.list = [];
            pageFunc.loadData();
        }
    };

    pageFunc.init = function () {
        pageFunc.selectProvince();
      //  pageFunc.loadData();
    }
    pageFunc.init();
}]);