<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="ServiceOrderList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarOrderManage.ServiceOrderList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl">
        <ol class="breadcrumb">
            <li class="">订单管理</li>
            <li class="active">养车订单管理</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            
            <%--<button type="button" class="btn btn-primary">订单作废</button>--%>
            <%--0受理中，1已确认，2已完成，3已取消--%>
            <div class="form-group">
				<label for="selectSeriesCate">订单状态：</label>
				<select id="selectSeriesCate" class="form-control" ng-change="pageFunc.changeStatus()" ng-model="pageData.filter.status">
                    <option value="">全部</option>
                    <option value="0">受理中</option>
                    <option value="1">已确认</option>
                    <option value="2">已完成</option>
                    <option value="3">已取消</option>
				</select>
			</div>

		</div>

        <table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
					<th>#</th>
					<th>
						所属商户
					</th>
					<th class="txtCenter">车型</th>
                    <th class="txtCenter">服务</th>
                    <th class="txtCenter">车主姓名</th>
                    <th class="txtCenter">车主手机</th>                    
                    <th class="txtCenter">到店时间</th>  
                    <th class="txtCenter">下单时间</th>  
                    <th class="txtCenter">订单状态</th>                      
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list">
					<td>{{item.OrderId}}</td>
					<td>
						{{item.Saller.Company}}
					</td>
					<td class="txtCenter">
						{{item.CarModel.AllName}}
					</td>
					<td class="txtCenter">
						{{item.Server.ShowName}}
					</td>
                    <td class="txtCenter">
						{{item.CarOwnerName}}
					</td>
                    <td class="txtCenter">
						{{item.CarOwnerPhone}}
					</td>
                    <td class="txtCenter">
						{{item.BookArrvieDateStr}}
					</td>
                    <td class="txtCenter">
						{{item.CreateTimeStr}}
					</td>
                    <td class="txtCenter">
						{{item.StatusStr}}
					</td>
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>
                        <%--<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">删除</button>--%>
						<!-- <i class="iconfont icon-edit"></i> -->
					</td>
					
				</tr>
			</tbody>
		</table>
        
    </div>
    <script>
        comeonModule.controller('ServManageCtrl', ['$scope', 'commService', '$modal', 'FileUploader', function ($scope, commService, $modal, FileUploader) {
            var pageData = $scope.pageData = {
                pageSize: 100000,
                pageIndex: 1,
                totalCount: 100,
                handlerUrl: cc.handler.car,
                list:[],
                filter:{
                    sallerId: '',
                    status:''
                }
            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                pageFunc.loadData();
            };

            pageFunc.showEdit = function (item) {
                sessionStorage.setItem("EditServiceOrder", JSON.stringify(item));
                window.location.href = "/Admin/CarOrderManage/EditServiceOrder.aspx";
            };

            pageFunc.changeStatus = function () {
                pageData.pageIndex = 1;
                pageFunc.loadData();
            };

            pageFunc.loadData = function () {
                commService.get(pageData.handlerUrl, {
                    action: 'GetCarServerOrders',
                    pageIndex: pageData.pageIndex,
                    pageSize: pageData.pageSize,
                    sallerId: pageData.filter.sallerId,
                    status:pageData.filter.status
                }, function (data) {
                    console.log('GetCarServerOrders', data);
                    pageData.list = data.returnObj.list;
                    pageData.totalCount = data.returnObj.totalCount;
                }, function (data) {
                    console.log(data);
                });
            };

            pageFunc.init();

        }]);

    </script>
</asp:Content>

