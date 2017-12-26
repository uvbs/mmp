<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="BuyCarOrderList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarOrderManage.BuyCarOrderList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl" style="padding-bottom: 124px;">
        <ol class="breadcrumb">
            <li class="">订单管理</li>
            <li class="active">购车报价单</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            
            <%--<button type="button" class="btn btn-primary">订单作废</button>--%>
            <span>
                订单状态筛选：
            </span>
            <select class="form-control" ng-model="pageData.filter.status" ng-change="pageFunc.loadDataByChange();">
                <option value="">全部</option>
                <option value="0">未处理</option>
                <option value="1">进行中</option>
                <option value="2">已过期</option>
                <option value="3">已取消</option>
                
            </select>

		</div>

        <table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
					<th>#</th>
					<th>
						车型
					</th>
					<th class="txtCenter">购车时间</th>
					<th class="txtCenter">购车方式</th>
                    <th class="txtCenter">牌照</th>
                    <th class="txtCenter">姓名</th>
                    <th class="txtCenter">手机</th>
                    <th class="txtCenter">所在区域</th>
                    <th class="txtCenter">购车偏好</th>
                    <th class="txtCenter">状态</th>
                    <th class="txtCenter">提交时间</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list">
					<td>{{item.QuotationId}}</td>
					<td>
						{{item.ShowCarModel}}
					</td>
					<td class="txtCenter">
						{{item.BuyTime}}
					</td>
					<td class="txtCenter">
						{{item.PurchaseWay}}
					</td>
                    <td class="txtCenter">
						{{item.LicensePlate}}
					</td>
                   
                    <td class="txtCenter">
						{{item.CarOwnerName}}
					</td>
                    <td class="txtCenter">
						{{item.CarOwnerPhone}}
					</td>
                    <td class="txtCenter">
						{{ite.District + ' ' + item.Area}}
					</td>
                    <td class="txtCenter">
						{{item.Preference}}
					</td>
                    <td class="txtCenter">
						{{item.StatusStr}}
					</td>
                    <td class="txtCenter">
						{{item.CreateTime | date:'yyyy-MM-dd hh-mm'}}
					</td>
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑报价单</button>
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
                filter:{
                    sallerId: '',
                    status: ''
                }
            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                pageFunc.loadData();
            };

            pageFunc.showEdit = function (item) {
                sessionStorage.setItem("EditBuyCarOrder",JSON.stringify(item));
                window.location.href = "/Admin/CarOrderManage/EditBuyCarOrder.aspx";
            };

            pageFunc.loadDataByChange = function()
            {
                pageData.pageIndex = 1;
                pageFunc.loadData();
            }

            pageFunc.loadData = function () {
                commService.get(pageData.handlerUrl, {
                    action: 'GetCarQuotationList',
                    pageIndex: pageData.pageIndex,
                    pageSize: pageData.pageSize,
                    sallerId: pageData.filter.sallerId,
                    status: pageData.filter.status
                }, function (data) {
                    pageData.list = data.returnObj.list;
                    pageData.totalCount = data.returnObj.totalCount;
                    console.log('GetCarQuotationList', data);
                }, function (data) {
                    console.log(data);
                });
            };

            pageFunc.init();

        }]);

    </script>
</asp:Content>


