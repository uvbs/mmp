<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="WorkHoursList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarWorkManage.WorkHoursList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl">
        <ol class="breadcrumb">
            <li class="">工时表管理</li>
            <li class="active">工时价</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            
            <button type="button" class="btn btn-primary" ng-click="pageFunc.add()">添加工时价</button>

		</div>

        <table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
					<th>#</th>
					<th>
						所属商户
					</th>
					<th class="txtCenter">车型</th>
					<th class="txtCenter">价格</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list">
					<td>{{item.AutoId}}</td>
					<td>
						{{item.SallerId}}
					</td>
					<td class="txtCenter">
						{{item.CarBrandName}}&nbsp;&nbsp;{{item.CarSeriesCateName}}&nbsp;&nbsp;{{item.CarSeriesName}}&nbsp;&nbsp;{{item.CarModelName}}
					</td>
					<td class="txtCenter">
						{{item.Price}}
					</td>
                   
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>
                        <button class="btn btn-info btn-xs" ng-click="pageFunc.delete(item)">删除</button>
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
        comeonModule.controller('ServManageCtrl', ['$scope', 'commService', '$modal', 'FileUploader', function ($scope, commService, $modal, FileUploader) {
            var pageData = $scope.pageData = {
                title: '工时价表',
                pageSize: 20,
                pageIndex: 1,
                totalCount: 0,
                handlerUrl: cc.handler.car,
                list: [],
                sallerId:''
            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                pageFunc.loadData();
            };

            pageFunc.showEdit = function (item) {
                sessionStorage.setItem('EditCarWorkhoursPriceItem', JSON.stringify(item));
                window.location.href = 'WorkHoursCompile.aspx?action=edit';
            };

            pageFunc.delete = function (item) {
                if (confirm('确定删除该数据?')) {
                    commService.postData(pageData.handlerUrl, {
                        action: 'DeleteCarWorkhoursPrice',
                        ids:item.AutoId
                    }, function (data) {
                        if (data.isSuccess) {
                            pageFunc.loadData();
                        } else {
                            alert('删除失败');
                        }
                    }, function (data) {

                    });
                }
            };

            pageFunc.add = function () {
                window.top.addTab('添加工时价', '/Admin/CarWorkManage/WorkHoursCompile.aspx');
            };

            pageFunc.loadData = function () {
                commService.get(pageData.handlerUrl, {
                    action: 'QueryCarWorkhoursPrice',
                    pageIndex: pageData.pageIndex,
                    pageSize: pageData.pageSize,
                    sallerId:pageData.sallerId
                }, function (data) {
                    pageData.list = data.returnObj.list;
                    pageData.totalCount = data.returnObj.totalCount;
                }, function (data) {

                });
            };

            pageFunc.init();

        }]);

    </script>
</asp:Content>
