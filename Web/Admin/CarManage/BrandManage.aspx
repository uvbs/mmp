<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="BrandManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarManage.BrandManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<div class="wrapSeries pBottom72" ng-controller="BrandManageCtrl">
		<ol class="breadcrumb">
            <li class="">车型库</li>
            <li class="active">品牌管理</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            <label>
                <input type="radio" name="rdoLibrary" checked ng-click="pageFunc.loadDataIsMustInWebSite(0)"/> 全部
            </label>
            <label>
                <input type="radio" name="rdoLibrary" ng-click="pageFunc.loadDataIsMustInWebSite(1)"/> 已选品牌
            </label>
            <input type="text" class="form-control width100" ng-model="pageData.nameQuery" placeholder="根据名称搜索...">
            <button type="button" class="btn btn-primary floatR" ng-click="pageFunc.refreshData()">更新已选品牌车系API数据</button>
            <div class="Clear"></div>
		</div>

		<table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
					<th>#</th>
					<th>配图</th>
					<th>

						品牌名称
						
					</th>
					<th class="txtCenter">养车库</th>
					<th class="txtCenter">购车库</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list | filter:pageData.nameQuery">
					<td>{{item.FirstLetter}}</td>
					<td>
						<img class="img-rounded maxw158" ng-src="{{item.BrandImg}}" ng-if="item.BrandImg != ''">
						<!-- <button class="btn btn-success btn-xs">上传图片</button> -->
					</td>
					<td>
						{{item.CarBrandName}}
					</td>
					<td class="txtCenter">
						<!-- <label class="mRight12">
							<input type="checkbox" ng-model="item.IsCurrBuyCarBrand" ng-click="pageFunc.changeCurrBuyCarBrand(item)">养车
						</label> -->
						<span ng-if="!item.IsCurrServiceCarBrand">-</span>
						<i class="iconfont icon-checked colorGreen" ng-if="item.IsCurrServiceCarBrand"></i>
					</td>
					<td class="txtCenter">
						<!-- <label>
							<input type="checkbox" ng-model="item.IsCurrServiceCarBrand" ng-click="pageFunc.changeCurrServiceCarBrand(item)">购车
						</label> -->
						
						<span ng-if="!item.IsCurrBuyCarBrand">-</span>
						<i class="iconfont icon-checked colorGreen" ng-if="item.IsCurrBuyCarBrand"></i>
					</td>
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>
						<!-- <i class="iconfont icon-edit"></i> -->
					</td>
					
				</tr>
			</tbody>
		</table>

		<!-- <div class="warpPagination">
		    <pagination ng-model="pageData.pageIndex"
		                total-items="pageData.totalCount"
		                items-per-page="pageData.pageSize"
		                max-size="5"
		                boundary-links="true"
		                rotate="false"
		                previous-text="上一页"
		                next-text="下一页"
		                first-text="首页"
		                last-text="尾页">
		    </pagination>
		</div>
 -->
	</div>
	<script type="text/javascript">
	    comeonModule.controller('BrandManageCtrl', ['$scope', 'commService','$modal','FileUploader', function ($scope, commService,$modal,FileUploader) {
	        var pageData = $scope.pageData = {
	            title: '品牌管理',
	            pageSize: 100000,
	            pageIndex: 1,
	            totalCount: 100,
	            handlerUrl: 'Handler.ashx',
	            firstLetter:'',
	            list:[],
	            currEditData:null,
	            isMustInWebsite:0,
	            nameQuery:''
	        };

	        var pageFunc = $scope.pageFunc = {};

	        pageFunc.refreshData = function () {
	            if (confirm('确认刷新接口数据？')) {
	            	layer.load();
	                commService.loadRemoteData(pageData.handlerUrl, {
	                    action: 'RefreshBrandSeriesAPIData'
	                }, function (data) {
	                	layer.closeAll();
	                }, function (data) {
	                	layer.closeAll();
	                });
	            };

	        };

	        pageFunc.loadDataIsMustInWebSite = function (type) {
	        	pageData.isMustInWebsite = type;
	        	pageFunc.loadData();
	        }

	        pageFunc.loadData = function () {
	        	layer.load();
	        	commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'QueryBrand',
                    pageSize:pageData.pageSize,
                    pageIndex:pageData.pageIndex,
                    firstLetter:pageData.firstLetter,
		            isMustInWebsite:pageData.isMustInWebsite
                }, function (data) {
                	layer.closeAll('loading');
                	pageData.totalCount = pageData.totalCount;
                	pageData.list = data.dataList;
                }, function (data) {
                	layer.closeAll('loading');
                });
	        };

	        pageFunc.changeCurrBuyCarBrand = function (item) {
	        	console.log(item);
	        };

	        pageFunc.changeCurrServiceCarBrand = function (item) {
	        	console.log(item);
	        };

	        pageFunc.showEdit = function (item) {
	        	console.log(item);
	        	//pageData.currEditData = item;
	        	 var modal = $modal.open({
		            animation: true,
		            templateUrl: 'editModalContent.html',
		            controller: function($scope, $modalInstance, FileUploader) {
		                var editModalFn = $scope.editModalFn = {};
		                var editModalData = $scope.editModalData = {
		                	currEditData:JSON.parse(JSON.stringify(item))
		                };

		                editModalFn.ok = function() {
		                	layer.load();
		               		commService.postData(pageData.handlerUrl,{
		               			action:'EditBrand',
		               			brandId:editModalData.currEditData.CarBrandId,
		               			brandImg:editModalData.currEditData.BrandImg,
		               			isBuy:editModalData.currEditData.IsCurrBuyCarBrand? 1:0,
		               			isService:editModalData.currEditData.IsCurrServiceCarBrand? 1:0
		               		},function(data){
		               			layer.closeAll();
		               			if (data.isSuccess) {
		               				for(key in editModalData.currEditData){
				                		item[key] = editModalData.currEditData[key];
				                	}
				                    $modalInstance.close();		
		               			};
		               		},function(data){
		               			layer.closeAll();

		               		});

		                };

		                editModalFn.cancel = function() {
		                    $modalInstance.close();
		                };

		                var uploader = $scope.uploader = new FileUploader({
							url: '/Handler/UeditorController.ashx?action=uploadimage&configPath=ubiConfig.json&noCache=' + Math.random(),
						});

						uploader.filters.push({
					        name: 'imageFilter',
					        fn: function (i /*{File|FileLikeObject}*/, options) {
					            var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
					            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
					        }
					    });

						uploader.onAfterAddingAll = function (addedFileItems) {
							layer.load();
					        uploader.uploadAll();
					    };
					    uploader.onCompleteItem = function (fileItem, response, status, headers) {
					    	layer.closeAll();
					        if (response.state.toLowerCase() == "success") {
					            editModalData.currEditData.BrandImg = response.url;
					        }
					    };

		            }
		        });
	        };


	        pageFunc.loadData();

	    }]);
	</script>
	 <script type="text/ng-template" id="editModalContent.html">
	 	<form ng-submit="editModalFn.ok()">
	        <div class="modal-header">
	            <h2 class="modal-title">品牌编辑</h2>
	        </div>
	        <div class="modal-body">
				<div class="form-group">
					<label>名称</label>
					<input type="text" ng-model="editModalData.currEditData.CarBrandName" class="form-control" readonly="true">
				</div>
				<div class="form-group">
					<label for="inputFile">配图</label>
					<input type="file" id="inputFile" uploader="uploader" nv-file-select="">
					
				</div>
				<div class="checkbox form-group">
					<img class="img-rounded maxw200" ng-src="{{editModalData.currEditData.BrandImg}}" ng-if="editModalData.currEditData.BrandImg != ''">
				</div>
				<div class="checkbox form-group">
					<label>
						<input type="checkbox" ng-model="editModalData.currEditData.IsCurrServiceCarBrand"> 养车库
					</label>
					<label>
						<input type="checkbox" ng-model="editModalData.currEditData.IsCurrBuyCarBrand"> 购车库
					</label>
				</div>
	        </div>
	        <div class="modal-footer txtCenter">
	            <button  type="submit" class="btn btn-primary">保存</button>
	            <a href="javascript:;" class="btn btn-warning" ng-click="editModalFn.cancel()">取消</a>
	        </div>
        </form>
    </script>
</asp:Content>

