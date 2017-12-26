ubimodule.controller("openCtrl", ['$scope', 'commArticle', 'commService',
	function($scope, commArticle, commService) {
		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: "公开课",
			tag: [{
				id: '',
				name: "全部"
			}],
			type: "OpenClass",
			cateId: baseData.moduleCateIds.openclass,
			pageIndex: 1,
			pageSize: 3,
			totalCount: 0,
			selectCate: null,
			artitle: [],
			searchKeyword: '',
			notice: ''

		};
		document.title = pageData.title;

		pageFunc.toggleMenuClick = function () {
			var menuLeft = document.getElementById('cbp-spmenu-s1'),
				body = document.body;
			classie.toggle(body, 'cbp-spmenu-push-toright');
			classie.toggle(menuLeft, 'cbp-spmenu-open');
		};

		pageFunc.leftMenuClick = function(item) {
			pageFunc.selectCateList(item);
			pageFunc.toggleMenuClick();
		};

		pageFunc.loadCateList = function() {
			commArticle.getArticleCateList(pageData.type, pageData.cateId, 1, 100,
				function(data) {
					for (var i = 0; i < data.list.length; i++) {
						pageData.tag.push({
							id: data.list[i].id,
							name: data.list[i].name
						});
					}
				},
				function(data) {

				});
		};

		pageFunc.selectCateList = function(data) {
			pageData.pageIndex = 1;
			pageData.artitle = [];
			pageData.selectCate = data;
			pageFunc.loadData();
		};

		pageFunc.loadData = function() {

			commArticle.getArticleListByOption({
					pageIndex: pageData.pageIndex,
					pageSize: pageData.pageSize,
					type: pageData.type,
					cateid: pageData.selectCate.id,
					keyword: pageData.searchKeyword
				},
				function(data) {
					console.log(data);
					pageData.artitle = pageData.artitle.concat(data.list);
					pageData.totalCount = data.totalcount;
					pageData.pageIndex++;
				},
				function() {});

		};

		pageFunc.gotoDetail = function(item) {
			window.location.href = '#/open/' + item.id;
		};

		pageFunc.search = function() {
			pageData.pageIndex = 1;
			pageData.artitle = [];
			pageFunc.loadData();
		};

		pageFunc.init = function() {
			pageFunc.loadCateList();
			pageData.selectCate = pageData.tag[0];
			pageFunc.loadData();
		};

		//$scope.$watch('pageData.pageIndex', pageFunc.loadData);
		pageFunc.init();

	}
]);