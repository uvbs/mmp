<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="CarDiscountRateList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarWorkManage.CarDiscountRateList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapSeries pBottom72" ng-controller="ServManageCtrl">
        <ol class="breadcrumb">
            <li class="">工时表管理</li>
            <li class="active">工时配件折扣</li>
		</ol>

        <div class="pLeft12 mBottom20 form-inline">			
            
            <button type="button" class="btn btn-primary" ng-click="pageFunc.add()">添加折扣</button>

		</div>

        <table class="table table-striped table-hover table-bordered">
			<thead>
				<tr>
					<th>#</th>
					<th>
						所属商户
					</th>
					<th class="txtCenter">星期</th>
                    <th class="txtCenter">开始时间</th>
                    <th class="txtCenter">结束时间</th>
                    <th class="txtCenter">配件折扣</th>
					<th class="txtCenter">工时折扣</th>
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
						{{item.WeekName}}
					</td>
                   <td class="txtCenter">
						{{item.StartTime}}
					</td>
                    <td class="txtCenter">
						{{item.EndTime}}
					</td>
                    <td class="txtCenter">
						{{item.WorkhoursRate}}
					</td>
                    <td class="txtCenter">
						{{item.PartsRate}}
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
                title: '',
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
                sessionStorage.setItem('EditCarDiscountRateItem', JSON.stringify(item));
                window.location.href = 'CarDiscountRateCompile.aspx?action=edit';
            };

            pageFunc.delete = function (item) {
                if (confirm('确定删除该数据?')) {
                    commService.postData(pageData.handlerUrl, {
                        action: 'DeleteCarDiscountRate',
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
                window.top.addTab('添加工时配件折扣', '/Admin/CarWorkManage/CarDiscountRateCompile.aspx');
            };

            pageFunc.loadData = function () {
                commService.get(pageData.handlerUrl, {
                    action: 'QueryCarDiscountRate',
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
