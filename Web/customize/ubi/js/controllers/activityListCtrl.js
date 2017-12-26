ubimodule.controller("activityListCtrl", ['$scope', '$http', '$routeParams', 'commActivity', function ($scope, $http, $routeParams, commActivity) {

	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '活动列表 - ' + baseData.slogan,
		pageIndex: 1,
		pageSize: 9,
		totalCount: 0,
		cateList:[{
			categoryid:'',
			categoryname:'全部',
		}],
		currSelectCate:{
			categoryid:''
		},
		activityData:[],
		searchKeyword:$routeParams.searchkey == null? '':$routeParams.searchkey,
	};

	document.title = pageData.title;

	pageFunc.pageLoad = function () {

		pageFunc.loadCateData();

	};

	/**
	 * [loadData 加载活动数据]
	 * @return {[type]} [description]
	 */
	pageFunc.loadData = function (isNew) {
		
		var url = baseData.handlerUrl + '?action=getactivitylist&pagesize=' + pageData.pageSize + '&cateid=' + pageData.currSelectCate.categoryid + '&keyword=' + pageData.searchKeyword;

		if (isNew === true) {
			url += '&pageindex=1';
		}else{
			url += '&pageindex=' + pageData.pageIndex; 
		}
		$http.get(url).success(function(data){
			pageData.totalCount = data.totalcount;
			pageData.activityData = data.list;
		});

	};

	/**
	 * [loadCateData 加载分类列表数据]
	 * @return {[type]} [description]
	 */
	pageFunc.loadCateData = function(){
		var url = baseData.handlerUrl + '?action=getactivitycategorylist&pageindex=1&pagesize=9999';
		$http.get(url).success(function(data){
			if (data && data.list) {
				for (var i = 0; i < data.list.length; i++) {
					pageData.cateList.push(data.list[i]);
				}
				
			}
		});
	};

	/**
	 * [selectCate 选择列表]
	 * @param  {[type]} item [description]
	 * @return {[type]}      [description]
	 */
	pageFunc.selectCate = function(item){
		pageData.currSelectCate = item;
		pageFunc.loadData(true);

	};

	pageFunc.gotoDetail = function (item) {
	    window.location.href = '#/activity/' + item.activityid;
	};

	$scope.$watch('pageData.pageIndex', pageFunc.loadData);

	pageFunc.pageLoad();


}]);