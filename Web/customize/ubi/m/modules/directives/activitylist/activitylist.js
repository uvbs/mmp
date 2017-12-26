ubimodule.directive('activitylist', function() {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'directives/activitylist/tpls/index.html',
		replace: true,
		scope: {
			userid: '@',
			hasimg: '@',
			hassearch: '@'
		},
		controller: function($scope, $element, $attrs,$http) {
			var pageFunc = $scope.pageFunc = {};
			var pageData = $scope.pageData = {
				hasImg: $scope.hasimg,
				hasSearch: $scope.hassearch,
				sideMenuShow: false,
				pageIndex: 1,
				pageSize: 9,
				totalCount: 0,
				cateList: [{
					categoryid: '',
					categoryname: '全部',
				}],
				currSelectCate: {
					categoryid: ''
				},
				activityData: [],
				searchKeyword: ''
			};

			pageFunc.leftMenuToggle = function () {
				var menuLeft = document.getElementById('cbp-spmenu-s1'),
					body = document.body;
				classie.toggle(body, 'cbp-spmenu-push-toright');
				classie.toggle(menuLeft, 'cbp-spmenu-open');
			};

			pageFunc.leftMenuClick = function(item) {
				pageFunc.leftMenuToggle();
				pageFunc.selectCate(item);
			};


			pageFunc.pageLoad = function() {

				pageFunc.loadCateData();
				pageFunc.loadData(true);

			};

			/**
			 * [loadData 加载活动数据]
			 * @return {[type]} [description]
			 */
			pageFunc.loadData = function(isNew) {

				var url = baseData.handlerUrl + '?action=getactivitylist&pagesize=' + pageData.pageSize + '&cateid=' + pageData.currSelectCate.categoryid + '&keyword=' + pageData.searchKeyword;

				if (isNew === true) {
					pageData.pageIndex = 1;
					pageData.activityData = [];
					url += '&pageindex=1';
				} else {
					url += '&pageindex=' + pageData.pageIndex;
				}
				$http.get(url).success(function(data) {
					console.log(data);
					pageData.totalCount = data.totalcount;
					pageData.activityData = pageData.activityData.concat(data.list);
					pageData.pageIndex++;
				});

			};

			/**
			 * [loadCateData 加载分类列表数据]
			 * @return {[type]} [description]
			 */
			pageFunc.loadCateData = function() {
				var url = baseData.handlerUrl + '?action=getactivitycategorylist&pageindex=1&pagesize=9999';
				$http.get(url).success(function(data) {
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
			pageFunc.selectCate = function(item) {
				pageData.currSelectCate = item;
				pageFunc.loadData(true);
			};

			pageFunc.pageLoad();

		}
	};
});