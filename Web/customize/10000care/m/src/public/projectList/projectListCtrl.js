comeonModule.controller('projectListCtrl',['$scope','commService','$routeParams',function ($scope,commService,$routeParams) {
	
	var pageData = $scope.pageData = {
		pageIndex:1,
		pageSize:6,
		area:'',
		cate:'',
		projectName:'',
		list:[],
		showList:[],
		isShowMore :true,
		userId:$routeParams.userid
	};
	var pageFunc = $scope.pageFunc = {};

	pageFunc.loadData = function () {
		var jsonData = {
			Action:'GetProjectList',
			Area:pageData.area,
			Category:pageData.cate,
			ProjectName:pageData.projectName,
			PageIndex:pageData.pageIndex,
			PageSize:pageData.pageSize
		}

		if (pageData.userId) {
			jsonData.Action = 'GetMyProjectList';
		};

		commService.loadRemoteData(baseData.handlerUrl,jsonData,function (data) {
			console.log(data);

			pageData.list = pageData.list.concat(data.ExObj);

			if (data.ExObj.length > 0) {
				pageData.isShowMore = true;
			}else{
				pageData.isShowMore = false;
			}

			for (var i = 0; i < data.ExObj.length; i++) {
				pageFunc.addShowData(data.ExObj[i]);
			};

			pageData.pageIndex++;
		},function (data) {
			
		});

	};

	pageFunc.search = function () {
		pageData.showList = [];
		pageData.pageIndex = 1;
		pageFunc.loadData();
	};

	/**
	 * [addShowData 添加展示数据]
	 * @param {[type]} item [description]
	 */
	pageFunc.addShowData = function (item) {
		
		item.ProjectCycleName = commService.getProjectCycleStr(item.ProjectCycle);

		if (pageData.showList.length > 0) {
			var lastShowData = pageData.showList[pageData.showList.length - 1];	
			if (lastShowData.length < 2) {
				lastShowData.push(item);
			}else{
				pageData.showList.push([item]);
			}
		}else{
			pageData.showList.push([item]);
		}
		
	};


	pageFunc.init = function () {
		pageFunc.loadData();
	};

	pageFunc.init();

}]);