<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarShopManage.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl">
        <ol class="breadcrumb">
            <li class="">商户管理</li>
            <li class="active">商户列表</li>
		</ol>

<%--        <div class="pLeft12 mBottom20 form-inline">			
            
            <button type="button" class="btn btn-primary">添加商户</button>

		</div>--%>

        <table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
                    <th class="txtCenter">
						商户类别
					</th>
                    <th class="txtCenter">
						登录名
					</th>
					<th class="txtCenter">
						商户名
					</th>
					<th class="txtCenter">省市区</th>
                    <th class="txtCenter">地址</th>
                    <th class="txtCenter">售前</th>
                    <th class="txtCenter">售前电话</th>
					<th class="txtCenter">售后</th>
                    <th class="txtCenter">售后电话</th>
                    <th class="txtCenter">工时价</th>
					<th></th>
				</tr>
			</thead>
			<tbody>
				<tr ng-repeat="item in pageData.list track by item.id">
                    <td class="txtCenter">
						{{item.ex2}}
					</td>					
					<td class="txtCenter">
						{{item.userId}}
					</td>
                    <td class="txtCenter">
						{{item.company}}
					</td>
					<td class="txtCenter">
						{{ item.province + ' ' + item.city + ' ' + item.district }}
					</td>
					<td class="txtCenter">
						{{item.address}}
					</td>
                    <td class="txtCenter">
						{{item.ex3}}
					</td>
                    <td class="txtCenter">
						{{item.ex5}}
					</td>
                    <td class="txtCenter">
						{{item.name}}
					</td>
                    <td class="txtCenter">
						{{item.phone}}
					</td>
                    <td class="txtCenter">
						￥{{item.ex1}}
					</td>
					<td class="txtCenter">
						<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">编辑</button>
                        <%--<button class="btn btn-info btn-xs" ng-click="pageFunc.showEdit(item)">禁用</button>--%>
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
                list:[],
                totalCount: 0,
                pageIndex: 1,
                pageSize:10

            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                pageFunc.loadData();
            };

            pageFunc.loadData = function () {
                commService.get('/Admin/User/Handler/UserHandler.ashx', {
                    action: 'getUserList',
                    page: pageData.pageIndex,
                    rows: pageData.pageSize,
                    type:5
                }, function (data) {
                    console.log(data);
                    pageData.list = data.rows;
                    pageData.totalCount = data.total;

                }, function (data) { });
            };

            pageFunc.showEdit = function (item) {
                sessionStorage.setItem('editSallerItem', JSON.stringify(item));
                window.location.href = '/Admin/CarShopManage/ShopCompile.aspx?action=edit';
            };

            pageFunc.init();

        }]);

    </script>
</asp:Content>

