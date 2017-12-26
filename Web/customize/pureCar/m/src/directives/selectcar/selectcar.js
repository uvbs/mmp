pureCarModule.directive('selectcar', function() {
	return {
		restrict: 'ECMA',
		templateUrl: basePath + 'directives/selectcar/tpls/index.html',
		replace: true,
		scope: {
			allowInput: '=allowInput',
			selectModel: '=selectModel',
		},
		controller: function($scope, $element, $attrs) {
			$scope.baseServer = baseServer;
			var pageFunc = $scope.pageFunc = {
				handleModelSelectStage: function (stage) {
					var methods = [
						pageFunc.loadBrands.bind(pageFunc),
						pageFunc.loadCategories.bind(pageFunc, pageData.brandId),
						pageFunc.loadSeries.bind(pageFunc, pageData.categoryId),
						pageFunc.loadModels.bind(pageFunc, pageData.seriesId),
					];
					if(pageData.modelSelectStage > stage) { pageData.modelSelectStage = stage; }
					else if(pageData.modelSelectStage == stage) { methods[stage - 1]() } 
				},
				loadBrands: function() {
					pageData.modelSelectStage = 1;
					pageData.loading = true;
					getReturnObj('car/model/brandlist.ashx').then(function (returnObj) {
						pageData.loading = false;
						pageData.brands = returnObj.list;
						$scope.$digest();
					});
				},
				loadCategories: function(brandId) {
					pageData.modelSelectStage = 2;
					pageData.loading = true;
					pageData.brandId = brandId;
					getReturnObj('car/model/SeriesCateList.ashx', {brandId: brandId}).then(function (returnObj) {
						pageData.loading = false;
						pageData.categories = returnObj.list;
						$scope.$digest();
					});
				},
				loadSeries: function(categoryId) {
					pageData.modelSelectStage = 3;
					pageData.loading = true;
					pageData.categoryId = categoryId;
					getReturnObj('car/model/SeriesList.ashx', {
						brandId: pageData.brandId,
						cateId: categoryId,
					}).then(function (returnObj) {
						pageData.loading = false;
						pageData.series = returnObj.list;
						$scope.$digest();
					});
				},
				loadModels: function (seriesId) {
					pageData.modelSelectStage = 4;
					pageData.loading = true;
					pageData.seriesId = seriesId;
					getReturnObj('car/model/ModelList.ashx', {seriesId: seriesId}).then(function (returnObj) {
						pageData.loading = false;
						pageData.models = returnObj.list;
						$scope.$digest();
					});
				}
			};
			var pageData = $scope.pageData = {
				topTabSelectIndex: 0,
				topTab: [{
					index: 0,
					title: '选择车辆型号'
				}, {
					index: 1,
					title: '没有我的车型'
				}],
				modelSelectStage: 1,
				loading: true,
				brands: null,
				categories: null,
				series: null,
				models: null,
				brandId: null,
				categoryId: null,
				seriesId: null
			};

			pageFunc.loadBrands();
		}
	};
});