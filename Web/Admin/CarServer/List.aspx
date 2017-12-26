<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarServer.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl">
        <ol class="breadcrumb">
            <li class="">服务管理</li>
            <li class="active">服务列表</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            
            <%--<button type="button" class="btn btn-primary">添加服务</button>--%>

            <div class="form-group">
			    <label class="col-sm-4 control-label pRight0 pLeft0 mTop8">服务类型:</label>
			    <div class="col-sm-8">
                <select class="form-control"  ng-model="pageData.serverType"  ng-change="pageFunc.loadData()">
                    <option>门店服务</option>
                    <option>上门服务</option>
                    </select>
                </div>
		    </div>
            <div class="form-group">
		    <label class="col-sm-4 control-label pRight0 pLeft0 mTop8">门店类型:</label>
			    <div class="col-sm-8">
                    <select class="form-control" ng-change="pageFunc.loadData()" ng-model="pageData.shopType">
                        <option>4S店</option>
                        <option>专修店</option>
                    </select>
                </div>
		    </div>

            <div class="form-group">
                <label  class="col-sm-4 control-label pRight0 pLeft0 mTop8">服务分类</label>
                <div class="col-sm-8">
                    <select class="form-control"  ng-change="pageFunc.loadDataByCate()" ng-model="pageData.cate" ng-options="item as item.CategoryName for item in pageData.cateList">
                    
                    </select>
                </div>
              </div>
            <br />
            <br />
            <div class="form-group">
				<label for="selectBrand">品牌：</label>
				<select id="selectBrand" class="form-control" ng-change="pageFunc.loadSeriesCate();pageFunc.loadData();" ng-options="item.CarBrandName for item in pageData.brandList"  ng-model="pageData.currSelectBrand">
				</select>
			</div>
			<div class="form-group">
				<label for="selectSeriesCate">车系分类：</label>
				<select id="selectSeriesCate" class="form-control" ng-change="pageFunc.loadSeries();pageFunc.loadData();" ng-model="pageData.currSelectSeriesCate" ng-options="item.CarSeriesCateName for item in pageData.seriesCateList">
				</select>
			</div>
			<div class="form-group">
				<label for="selectSeries">车系：</label>
				<select id="selectSeries" class="form-control" ng-change="pageFunc.loadModel();pageFunc.loadData();" ng-model="pageData.currSelectSeries" ng-options="item.CarSeriesName for item in pageData.seriesList">
				</select>
			</div>

            <div class="form-group">
				<label for="selectCarModel">车型：</label>
				<select id="selectCarModel" class="form-control" ng-change="pageFunc.loadData()" ng-model="pageData.currSelectCarModel" ng-options="item.ShowName for item in pageData.modelList">
				</select>
			</div>

		</div>

        <table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
                    
					<th class="txtCenter">
						服务名称
					</th>
                    <th>
						分类
					</th>
                    <th>
						适配车型
					</th>
                    <th class="txtCenter">门店类别</th>
                    <th class="txtCenter">服务类型</th>
					<th class="txtCenter">工时数</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list track by item.ServerId">	
                    				
					<td class="txtCenter">
						{{item.ServerName}}
					</td>
                    <td>
						{{item.ShowCate}}
					</td>
                    <td>
						{{item.ShowCarModel}}
					</td>
                    <td class="txtCenter">
						{{item.ShopType}}
					</td>
                    <td class="txtCenter">
						{{item.ServerType}}
					</td>
					<td class="txtCenter">
						{{item.WorkHours}}
					</td>
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>
                        <%--<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">删除</button>--%>
						<!-- <i class="iconfont icon-edit"></i> -->
					</td>
					
				</tr>
			</tbody>
		</table>
        <div class="warpPagination" ng-show="pageData.totalCount > pageData.pageSize">
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
    </div>
    <script>
        comeonModule.controller('ServManageCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'carService', 'cateService', function ($scope, commService, $modal, FileUploader, carService, cateService) {
            var pageData = $scope.pageData = {
                title: '服务管理',
                handlerUrl: '/Handler/App/CarServiceHandler.ashx',
                pageSize: 10,
                pageIndex: 1,
                totalCount: 0,
                list: [],
                cateId:0,
                carBrandId: 0,
                carSeriesCateId: 0,
                carSeriesId: 0,
                carModelId: 0,
                serverType: '',
                shopType: '',

                cate:{},
                cateList: []//分类列表
            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                $scope.$watch('pageData.pageIndex', pageFunc.loadData);

                //加载服务分类列表
                cateService.cateList(cc.cate.purecar.serverCate, function (data) {
                    pageData.cateList = data;
                });

                pageFunc.loadBrand();
            };

            pageFunc.loadDataByCate = function () {
                pageData.cateId = pageData.cate.AutoID;
                pageFunc.loadData();
            };

            pageFunc.loadData = function () {

                if (pageData.currSelectCarModel) {
                    pageData.carModelId = pageData.currSelectCarModel.CarModelId;
                }
                if (pageData.currSelectBrand) {
                    pageData.carBrandId = pageData.currSelectBrand.CarBrandId;
                }

                if (pageData.currSelectSeriesCate) {
                    pageData.carSeriesCateId = pageData.currSelectSeriesCate.CarSeriesCateId;
                }

                if (pageData.currSelectSeries) {
                    pageData.carSeriesId = pageData.currSelectSeries.CarSeriesId;
                }


                carService.loadServer({
                    pageSize: pageData.pageSize,
                    pageIndex: pageData.pageIndex,
                    cateId: pageData.cateId,
                    carBrandId: pageData.carBrandId,
                    carSeriesCateId: pageData.carSeriesCateId,
                    carSeriesId: pageData.carSeriesId,
                    carModelId: pageData.carModelId,
                    serverType: pageData.serverType,
                    shopType: pageData.shopType
                }, function (data) {
                    console.log(data);
                    pageData.totalCount = data.totalCount;
                    pageData.list = data.list;
                });
            };

            pageFunc.showEdit = function (item) {
                
                sessionStorage.setItem('editServerItem', JSON.stringify(item));
                window.location.href = '/Admin/CarServer/AddServer.aspx?action=edit';

                //pageData.currEditData = item;
                //var modal = $modal.open({
                //    animation: true,
                //    templateUrl: 'editModalContent.html',
                //    controller: function ($scope, $modalInstance, FileUploader) {
                //        var editModalFn = $scope.editModalFn = {};
                //        var editModalData = $scope.editModalData = {
                //            currEditData: JSON.parse(JSON.stringify(item))
                //        };

                //        editModalFn.ok = function () {
                //            //layer.load();
                //            //commService.postData(pageData.handlerUrl, {
                //            //    action: 'EditBrand',
                //            //    brandId: editModalData.currEditData.CarBrandId,
                //            //    brandImg: editModalData.currEditData.BrandImg,
                //            //    isBuy: editModalData.currEditData.IsCurrBuyCarBrand ? 1 : 0,
                //            //    isService: editModalData.currEditData.IsCurrServiceCarBrand ? 1 : 0
                //            //}, function (data) {
                //            //    layer.closeAll();
                //            //    if (data.isSuccess) {
                //            //        for (key in editModalData.currEditData) {
                //            //            item[key] = editModalData.currEditData[key];
                //            //        }
                //            //        $modalInstance.close();
                //            //    };
                //            //}, function (data) {
                //            //    layer.closeAll();

                //            //});
                //            $modalInstance.close();
                //        };

                //        editModalFn.cancel = function () {
                //            $modalInstance.close();
                //        };


                //    }
                //});
            };

            pageFunc.loadBrand = function () {
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'QueryBrand',
                    pageSize: 10000,
                    pageIndex: 1,
                    firstLetter: '',
                    isMustInWebsite: 1
                }, function (data) {
                    pageData.brandList = data.dataList;
                    pageData.seriesCateList = null;
                    pageData.seriesList = null;
                    pageData.modelCateList = null;

                    pageData.currSelectBrand = null;
                    pageData.currSelectSeriesCate = null;
                    pageData.currSelectSeries = null;
                    pageData.currSelectCarModel = null;
                }, function (data) {

                });
            };

            pageFunc.loadSeriesCate = function () {
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'GetSeriesCateList',
                    brandId: pageData.currSelectBrand.CarBrandId
                }, function (data) {
                    pageData.seriesCateList = data;
                    pageData.seriesList = null;
                    pageData.modelCateList = null;

                    pageData.currSelectSeriesCate = null;
                    pageData.currSelectSeries = null;
                    pageData.currSelectCarModel = null;
                }, function (data) {

                });
            };

            pageFunc.loadSeries = function () {
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'GetSeriesList',
                    cateId: pageData.currSelectSeriesCate.CarSeriesCateId
                }, function (data) {
                    pageData.seriesList = data;
                    pageData.modelCateList = null;
                }, function (data) {
                    pageData.currSelectSeries = null;
                    pageData.currSelectCarModel = null;
                });
            };

            pageFunc.loadModel = function () {

                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'GetModelList',
                    modelCateId: 1,
                    seriesId: pageData.currSelectSeries ? pageData.currSelectSeries.CarSeriesId : 0,
                    pageSize: 10000,
                    pageIndex: 1
                }, function (data) {
                    pageData.modelList = data.list;
                    //layer.closeAll();
                    //pageData.modelList.totalCount = data.totalCount;
                    //pageData.modelList.list = data.list;

                    pageData.currSelectCarModel = null;
                }, function (data) {
                });
            };


            pageFunc.init();

        }]);

    </script>
    <%--<script type="text/ng-template" id="editModalContent.html">
	 	<form ng-submit="editModalFn.ok()">
	        <div class="modal-header">
	            <h2 class="modal-title">服务编辑</h2>
	        </div>
	        <div class="modal-body">
				<div class="form-group">
					<label>名称</label>
					<input type="text" ng-model="editModalData.currEditData.ServerName" class="form-control">
				</div>
				<div class="form-group">
					<label>工时</label>
					<input type="text" ng-model="editModalData.currEditData.Price" class="form-control">
				</div>
                <div class="form-group">
					<label>服务价格</label>
					<input type="text" ng-model="editModalData.currEditData.WorkHours" class="form-control">
				</div>

        <div class="form-group">
					<label>配件列表</label>
				<div >
                    <table class="table table-hover" >
                      
			<thead>
				<tr>
					<th>
						配件名称
					</th>
					<th class="txtCenter">剩余数量</th>
					<th></th>
				</tr>
			</thead>
			<tbody style="max-height: 100px;    overflow: auto;">
				<tr ng-repeat="item in editModalData.currEditData.Parts">
					<td>
						{{item.name}}
					</td>
					<td class="txtCenter">
						{{item.count}}
					</td>
					<td class="txtCenter">
                        <button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">移除</button>
						<!-- <i class="iconfont icon-edit"></i> -->
					</td>
					
				</tr>
			</tbody>
		</table>
        </div>
                </div>

	        </div>
	        <div class="modal-footer txtCenter">
	            <button  type="submit" class="btn btn-primary">保存</button>
	            <a href="javascript:;" class="btn btn-warning" ng-click="editModalFn.cancel()">取消</a>
	        </div>
        </form>
    </script>--%>


</asp:Content>
