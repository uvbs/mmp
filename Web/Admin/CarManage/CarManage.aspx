<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="CarManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarManage.CarManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="wrapSeries" ng-controller="SeriesCtrl" style="padding-bottom:100px;">
		<ol class="breadcrumb">
            <li class="">车型库</li>
            <li class="active">车型管理</li>
		</ol>
		
		<form class="form-inline pLeft12 mBottom20">
			<div class="form-group">
				<label for="selectBrand">品牌：</label>
				<select id="selectBrand" class="form-control" ng-change="pageFunc.loadSeriesCate()" ng-options="item.CarBrandName for item in pageData.brandList"  ng-model="pageData.currSelectBrand">
				</select>
			</div>
			<div class="form-group">
				<label for="selectSeriesCate">车系分类：</label>
				<select id="selectSeriesCate" class="form-control" ng-change="pageFunc.loadSeries()" ng-model="pageData.currSelectSeriesCate" ng-options="item.CarSeriesCateName for item in pageData.seriesCateList">
				</select>
			</div>
			<div class="form-group">
				<label for="selectSeries">车系：</label>
				<select id="selectSeries" class="form-control" ng-change="pageFunc.loadModel()" ng-model="pageData.currSelectSeries" ng-options="item.CarSeriesName for item in pageData.seriesList">
				</select>
			</div>
			<div class="form-group hidden">
				<label for="selectModelCate">车型分类：</label>
				<select id="selectModelCate" class="form-control" ng-change="pageFunc.loadModel()" ng-model="pageData.currSelectModelCate" ng-options="item.CarModelCateName for item in pageData.modelCateList">
				</select>
			</div>
			<!-- <button type="submit" class="btn btn-primary mLeft6">查找车型</button> -->
		</form>

		<table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
                    <th>配图</th>
					<th>年份</th>
					<th>车型</th>
					<th>指导价</th>
                    <th>是否有颜色</th>
                    <th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.modelList.list">
                    <td>
                        <img class="img-rounded" ng-src="{{item.Img}}" ng-if="item.Img != ''" style="    max-width: 64px;">
                    </td>
					<td>{{item.Year}}</td>
					<td>{{item.CarModelName}}</td>					
					<td>{{item.GuidePrice}}</td>
                    <td>{{item.Colors == ''? '否':'是'}}</td>
                    <td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>                        
					</td>
				</tr>
				
			</tbody>
		</table>

		<div class="warpPagination" ng-if="pageData.modelList.totalCount>pageData.modelList.pageSize">
		    <pagination ng-model="pageData.modelList.pageIndex"
		                total-items="pageData.modelList.totalCount"
		                items-per-page="pageData.modelList.pageSize"
		                max-size="5"
		                boundary-links="true"
		                rotate="false"
		                previous-text="上一页"
		                next-text="下一页"
		                first-text="首页"
		                last-text="尾页">
		    </pagination>
		</div>

	</div>
	<script type="text/javascript">
		comeonModule.controller('SeriesCtrl',['$scope','commService',function ($scope,commService) {
		    var pageData = $scope.pageData = {
                title:'车型库管理',                
                handlerUrl:'Handler.ashx',
                brandList:[],
                seriesCateList:[],
                seriesList:[],
                modelCateList:[],
                modelList:{
                	pageSize:10,
	                pageIndex:1,
	                totalCount:100,
	                list:[]
                },
                currSelectBrand:null,
                currSelectSeriesCate:null,
                currSelectSeries:null,
                currSelectModelCate:null
		    };
		    var pageFunc = $scope.pageFunc = {};

		    pageFunc.loadBrand = function () {		    	
	        	commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'QueryBrand',
                    pageSize:10000,
                    pageIndex:1,
                    firstLetter:'',
		            isMustInWebsite:1
                }, function (data) {                	
                	pageData.brandList = data.dataList;
                	pageData.seriesCateList = null;
                	pageData.seriesList = null;
                	pageData.modelCateList = null;
                }, function (data) {
                	
                });
		    };

		    pageFunc.loadSeriesCate = function () {
		    	commService.loadRemoteData(pageData.handlerUrl,{
		    		action:'GetSeriesCateList',
		    		brandId:pageData.currSelectBrand.CarBrandId
		    	},function(data) {
		    		pageData.seriesCateList = data;
		    		pageData.seriesList = null;
                	pageData.modelCateList = null;
		    	},function(data) {
		    		
		    	});	
		    };

		    pageFunc.loadSeries = function () {
		    	commService.loadRemoteData(pageData.handlerUrl,{
		    		action:'GetSeriesList',
		    		cateId: pageData.currSelectSeriesCate.CarSeriesCateId,
		    		brandId: pageData.currSelectBrand.CarBrandId
		    	},function(data) {
		    		pageData.seriesList = data;
                	pageData.modelCateList = null;
		    	},function(data) {
		    		
		    	});	
		    };

		    pageFunc.loadModelCate = function () {
		    	commService.loadRemoteData(pageData.handlerUrl,{
		    		action:'GetModelCateList',
		    		seriesId:pageData.currSelectSeries.CarSeriesId
		    	},function(data) {
		    		pageData.modelCateList = data;
		    	},function(data) {
		    		
		    	});	
		    };

		    pageFunc.loadModel = function () {
		    	layer.load();
		    	commService.loadRemoteData(pageData.handlerUrl,{
		    		action:'GetModelList',
		    		modelCateId:1,
		    		seriesId:pageData.currSelectSeries?pageData.currSelectSeries.CarSeriesId:0,
		    		pageSize:pageData.modelList.pageSize,
		    		pageIndex:pageData.modelList.pageIndex
		    	},function(data) {
		    		layer.closeAll();
		    		pageData.modelList.totalCount = data.totalCount;
		    		pageData.modelList.list = data.list;
		    	},function(data) {
		    		layer.closeAll();
		    	});	
		    };

		    pageFunc.showEdit = function (item) {
		        sessionStorage.setItem('EditCarModel', JSON.stringify(item));
		        window.location.href = '/Admin/CarManage/EditCarModel.aspx';
		    };

		    pageFunc.init = function () {
		    	pageFunc.loadBrand();
		    	pageFunc.loadModel();
		    	$scope.$watch('pageData.modelList.pageIndex', pageFunc.loadModel);
		    };
		    
		    pageFunc.init();

		}]);
	</script>
</asp:Content>
