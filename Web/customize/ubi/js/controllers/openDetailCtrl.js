
ubimodule.controller("openListCtrl", ['$scope', 'commArticle', 'commService',
function($scope, commArticle, commService){
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title : "公开课 - " + baseData.slogan,
		tag : [{
			id : '',
			name : "全部"
		}],
		type:"OpenClass",
		cateId:baseData.moduleCateIds.openclass,
		pageIndex:1,
		pageSize:5,
		totalCount:0,
		selectCate:null,
		artitle:null

	};
	document.title = pageData.title;
	pageFunc.loadCateList = function(){
		commArticle.getArticleCateList(pageData.type, pageData.cateId, 1, 100,
		 function(data) {
			for(var i=0; i<data.list.length;i++){
				pageData.tag.push({id:data.list[i].id,name:data.list[i].name});
			}
		}, 
		function(data){

		});
	}
		pageFunc
	
		pageFunc.selectCateList = function(data){

			pageData.selectCate = data;
			pageFunc.loadData();
		}
		pageFunc.loadData = function() {
		// commArticle.getArticleListByOption(`)
		commArticle.getArticleListByOption(
			{
				pageIndex:pageData.pageIndex,
				pageSize:pageData.pageSize,
				type:pageData.type,
				cateid:pageData.selectCate.id
			}, 
			function(data){
				console.info(data.list);
				pageData.artitle=data.list;
				pageData.totalCount=data.totalcount;
			}, 
			function(){});

		};
		pageFunc.gotoDetail = function(item){
			window.location.href = '#/open/'+item.openId;
		}



		pageFunc.init = function(){
			pageFunc.loadCateList();
			pageData.selectCate = pageData.tag[0];
			pageFunc.loadData();
		}
		pageFunc.init();

	} 
]);

ubimodule.controller('openDetailCtrl',['$scope','$routeParams','commArticle', 'commService'
	,function($scope,$routeParams,commArticle,commService){
		var pageFunc = $scope.pageFunc ={};
		var pageData = $scope.pageData ={
			title : "公开课详情 - " + baseData.slogan,
			articleId :$routeParams.id,
			content:'',
			
			

		}

		document.title = pageData.title;
		
		pageFunc.favoriteArticle = function() {
			 commArticle.favoriteArticle(
			 		function(data){
			 			if(data.isSuccess){
			 				alert('收藏成功')
			 			}
			 			else if(data.errcode == 10013){
			 				alert("您已收藏过了")
			 			}
			 			else if( data.errcode == 10010){
			 				$scope.showLogin(null,'您取消了登陆，继续收藏请登陆')
			 			}
			 		},function(){},
			 		pageData.articleId
			 	)
		};
		
		 pageFunc.disFavoriteArticle = function () {
	        commArticle.disFavoriteArticle(function (data) {
	            if (data.isSuccess) {
	                pageData.article.currUserIsFavorite = false;
	                pageData.article.favoriteCount -= 1;
	                alert('取消收藏成功');
	            } else {
	                alert('取消收藏失败');
	            }
	        }, function (data) { }, pageData.articleId);
	    };

		pageFunc.loadData = function(){
			commArticle.getOpenClassDetail(pageData.articleId
				,function(data){
				console.log(data);
				pageData.content = data.content;
	
				}
				,function(data){})};


		pageFunc.init = function(){
			pageFunc.loadData();
		};
		pageFunc.init();



	}]);