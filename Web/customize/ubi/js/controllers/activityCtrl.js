ubimodule.controller("activityCtrl", ['$scope', '$http', 'datePickerCore', function($scope, $http, datePickerCore) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '活动日历 - ' + baseData.slogan,
		years: [],
		months: [],
		now: new Date(),
		currSelectYear: new Date().getFullYear(),
		currSelectMonth: new Date().getMonth() + 1,
		weeks: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'],
		currDays: null,
		hotActivityData: [],
		searchKeyword:'',
	};
	document.title = pageData.title;
	/**
	 * [pageLoad 页面加载初始化]
	 * @return {[type]} [description]
	 */
	pageFunc.pageLoad = function() {
		for (var i = 0; i < 10; i++) {
			pageData.years.push({
				value: pageData.now.getFullYear() + 3 - i,
				label: pageData.now.getFullYear() + 3 - i + '年',
			});
		}
		for (var i = 1; i <= 12; i++) {
			pageData.months.push(i);
		}
		//test
		for (var i = 1; i <= 5; i++) {
			pageData.hotActivityData.push({
				title: '热门活动阿斯顿撒' + i,
				applyCount: i * 22,
			});
		}
		pageFunc.getCurrDays();
	};
	/**
	 * [getCurrDays 获取当前日历数据及活动数据]
	 * @return {[type]} [description]
	 */
	pageFunc.getCurrDays = function() {
		pageData.currDays = datePickerCore.createDayArr(pageData.currSelectYear, pageData.currSelectMonth);
		//TODO:加载活动列表并呈现
		var url = baseData.handlerUrl + '?action=getactivitylist&pageindex=1&pagesize=1000&year=' + pageData.currSelectYear + '&month=' + pageData.currSelectMonth;
		$http.get(url).success(function(data) {
			for (var i = 0; i < pageData.currDays.length; i++) {
				for (var k = 0; k < pageData.currDays[i].length; k++) {
					var day = pageData.currDays[i][k];
					day.activity = [];
					day.hasOverActivity = false;
					day.hasActivity = false;
					for (var j = 0; j < data.list.length; j++) {
						var activity = data.list[j];
						var startDate = new Date(activity.starttimestr);
						if (startDate.getDate() == day.day) {
							activity.isOver = false;
							day.hasActivity = true;
							if (activity.status == 1) {
								day.hasOverActivity = true;
								activity.isOver = true;
							}
							day.activity.push(activity);
						}
					}
				}
			}
		});
	};
	pageFunc.search = function(){
		if (pageData.searchKeyword != '') {
			window.location.href = '#/activityList/' + pageData.searchKeyword;
		}
	};
	/**
	 * [selectYear 选择年份]
	 * @return {[type]} [description]
	 */
	pageFunc.selectYear = function() {
		pageFunc.getCurrDays();
	};
	/**
	 * [selectMonth 选择月份]
	 * @param  {[type]} item [月份对象]
	 * @return {[type]}      [description]
	 */
	pageFunc.selectMonth = function(item) {
		pageData.currSelectMonth = item;
		pageFunc.getCurrDays();
	};

	pageFunc.gotoDetail = function (item) {
	    window.location.href = '#/activity/' + item.activityid;
	};

	pageFunc.pageLoad();
}]);