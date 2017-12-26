<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarParts.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl">
        <ol class="breadcrumb">
            <li class="">配件管理</li>
            <li class="active">配件列表</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            
            <a href="/Admin/CarParts/PartsCompile.aspx" target="_self" class="btn btn-primary">添加配件</a>
            <a href="javascript:;" ng-click="pageFunc.delete()" target="_self" class="btn btn-primary">删除配件</a>

            <br />
            <br />

            <div class="form-group">
                <label  class="">配件分类</label>
                <select class="form-control" ng-change="pageFunc.loadData()" ng-model="pageData.selectPartsCate" ng-options="item as item.CategoryName for item in pageData.partsCateList">
                    
                </select>
              </div>
            <div class="form-group">
                <label >配件品牌</label>
                    <select class="form-control" ng-change="pageFunc.loadData()" ng-model="pageData.selectPartsBrand" ng-options="item as item.CategoryName for item in pageData.partsBrandList">

                    </select>
              </div>
            <br />
            <br />
            <div class="form-group">
				<label for="selectBrand">车型品牌：</label>
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
                        <label>
                            全选
                            <input type="checkbox" ng-model="pageData.checkAll" ng-change="pageFunc.changeCheckAll()" />
                        </label>                        
					</th>
                    <th>
						适配车型
					</th>
                    <th>
						所属分类
					</th>
                    <th>
						所属品牌
					</th>
					<th>
						配件名称
					</th>
					<th class="txtCenter">数量</th>
                    <th class="txtCenter">规格</th>
					<th class="txtCenter">配件价格</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list">
					<td class="txtCenter">
                        <label>                            
                            <input type="checkbox" ng-model="item.check"/>
                        </label>
					</td>
                    <td>
						{{item.ShowCarModel}}
					</td>
					<td>
						{{item.PartsCateName}}
					</td>
                    <td>
						{{item.PartsBrandName}}
					</td>
                    <td>
						{{item.PartName}}
					</td>
					<td class="txtCenter">
						{{item.Count}}
					</td>
                    <td class="txtCenter">
						{{item.PartsSpecs}}
					</td>
					<td class="txtCenter">
						￥{{item.Price}}
					</td>
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>
                        <%--<button class="btn btn-info btn-xs" ng-click="pageFunc.delete(item)">删除</button>--%>
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
                pageSize: 10,
                pageIndex: 1,
                totalCount: 0,
                handlerUrl: cc.handler.car,
                firstLetter: '',
                list: [],
                currEditData: null,
                isMustInWebsite: 0,
                nameQuery: '',
                checkAll:false
            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                //pageFunc.loadData();
                pageFunc.loadBrand();

                //加载配件分类
                cateService.cateList(cc.cate.purecar.partsCate, function (data) {
                    pageData.partsCateList = data;
                    
                });

                //加载配件品牌
                cateService.cateList(cc.cate.purecar.partsBrand, function (data) {
                    pageData.partsBrandList = data;
                   
                });

                $scope.$watch("pageData.pageIndex", pageFunc.loadData);

            };

            pageFunc.showEdit = function (item) {

                sessionStorage.setItem('editPartsItem',JSON.stringify(item));

                window.location.href = '/Admin/CarParts/PartsEdit.aspx?action=edit&id=' + item.PartId;

            };

            pageFunc.loadData = function () {
                //console.log(pageData.pageIndex);

                //var carModelId = Convert.ToInt32(context.Request["carModelId"]);
                //var brandId = Convert.ToInt32(context.Request["brandId"]);
                //var seriesCateId = Convert.ToInt32(context.Request["seriesCateId"]);
                //var seriesId = Convert.ToInt32(context.Request["seriesId"]);

                var jsonData = {
                    carModelId: 0,
                    brandId: 0,
                    seriesCateId: 0,
                    seriesId: 0,
                    pageIndex: pageData.pageIndex,
                    pageSize: pageData.pageSize
                }


                if (pageData.selectPartsCate) {
                    jsonData.cateId = pageData.selectPartsCate.AutoID;
                }
                if (pageData.selectPartsBrand) {
                    jsonData.partsBrandId = pageData.selectPartsBrand.AutoID;
                }

                if (pageData.currSelectSeriesCate) {
                    jsonData.seriesCateId = pageData.currSelectSeriesCate.CarSeriesCateId;
                }

                if (pageData.currSelectSeries) {
                    jsonData.seriesId = pageData.currSelectSeries.CarSeriesId;
                }

                if (pageData.currSelectCarModel) {
                    jsonData.carModelId = pageData.currSelectCarModel.CarModelId;
                }
                if (pageData.currSelectCarModel) {
                    jsonData.carModelId = pageData.currSelectCarModel.CarModelId;
                }

                carService.loadParts(jsonData, function (data) {
                    console.log(data);
                    pageData.totalCount = data.totalCount;

                    for (var i = 0; i < data.list.length; i++) {
                        data.list[i].check = false;
                    }

                    pageData.list = data.list;

                })
            };

            pageFunc.delete = function () {

                var ids = [];

                for (var i = 0; i < pageData.list.length; i++) {
                    if (pageData.list[i].check) {
                        ids.push(pageData.list[i].PartId);
                    }
                }

                if (ids.length == 0) {
                    alert('未选择任何数据');
                    return;
                }

                //alert(ids.join(','));
                if (confirm('确定删除选定数据？')) {
                    carService.deleteParts(ids.join(','), function (data) {
                        pageFunc.loadData();
                    });
                }
            };

            pageFunc.changeCheckAll = function () {
                for (var i = 0; i < pageData.list.length; i++) {
                    pageData.list[i].check = pageData.checkAll;
                }
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

                    pageData.currSelectSeries = null;
                    pageData.currSelectCarModel = null;

                }, function (data) {

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
</asp:Content>
