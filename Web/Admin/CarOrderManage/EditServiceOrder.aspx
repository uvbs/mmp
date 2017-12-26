<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="EditServiceOrder.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarOrderManage.EditServiceOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="wrapSeries" ng-controller="AddServerCtrl" style="padding-bottom: 124px;">
        <ol class="breadcrumb">
            <li class="">订单管理</li>
            <li class="active">{{pageData.title}}</li>
		</ol>
        <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
        <fieldset>
        <legend class="txtCenter pLeft50">订单信息</legend>

          <div class="form-group">
            <label class="col-sm-2 control-label">车主姓名</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.order.CarOwnerName" placeholder="姓名" required readonly="true">
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 control-label">车主手机</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.order.CarOwnerPhone" placeholder="手机" required readonly="true">
            </div>
          </div>

            <div class="form-group">
            <label class="col-sm-2 control-label">车型</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.order.CarModel.AllName" placeholder="姓名" required readonly="true">
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 control-label">服务</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.order.Server.ShowName" placeholder="手机" required readonly="true">
            </div>
          </div>

            <div class="form-group">
            <label class="col-sm-2 control-label">所属商户</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.order.Saller.Company" placeholder="姓名" required readonly="true">
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 control-label">到店时间</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.order.BookArrvieDateStr" placeholder="手机" required readonly="true">
            </div>
          </div>

            <div class="form-group">
            <label class="col-sm-2 control-label">下单时间</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.order.CreateTimeStr" placeholder="姓名" required readonly="true">
            </div>
          </div>
          <%--<div class="form-group">
            <label class="col-sm-2 control-label">订单状态</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.order.StatusStr" placeholder="手机" required readonly="true">
            </div>
          </div>--%>



        </fieldset>


           <div style="
                    border-top: 1px solid #DDDDDD;
                    position: fixed;
                    bottom: 59px;
                    height: 60px;
                    line-height: 60px;
                    text-align: center;
                    width: 100%;
                    background-color: rgb(245, 245, 245);
              ">
               <span class="mRight12">订单状态：{{pageData.order.StatusStr}}</span>
               <button  type="submit" class="btn btn-primary" ng-show="pageData.order.Status == 0" ng-click="pageFunc.confirmOrder()">确认订单</button>
               <button  type="submit" class="btn btn-primary" ng-show="pageData.order.Status == 1" ng-click="pageFunc.useOrder()">核销订单</button>
               <button  type="submit" class="btn btn-primary" ng-hide="pageData.order.Status == 2 || pageData.order.Status == 3" ng-click="pageFunc.cancelOrder()">取消订单</button>
            </div>
         
        </form>
     </div>
    <script type="text/javascript">
        comeonModule.controller('AddServerCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'cateService', function ($scope, commService, $modal, FileUploader, cateService) {
            var pageData = $scope.pageData = {
                title: '养车订单',
                handlerUrl: cc.handler.car,
                order: {},

                currAction: 'EditCarQuotation',

            };

            //pageData.parts.CarModelId

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {

                //var urlAction = GetParm('action');

                //if (urlAction == 'edit') {

                //pageData.editId = GetParm('id');
                pageData.order = JSON.parse(sessionStorage.getItem("EditServiceOrder"));


                console.log('EditBuyCarOrder', pageData.order);
                //}

            };

            //确认订单
            pageFunc.confirmOrder = function () {
                if (confirm("确认当前订单？")) {
                    commService.postData(
                        pageData.handlerUrl,
                        {
                            action: 'EditServerOrderStatus',
                            ids: pageData.order.OrderId,
                            status:1
                        },
                        function (data) {

                            if (data.isSuccess) {
                                pageData.order.Status = 1;
                                pageData.order.StatusStr = '已确认';


                                if (data.isSuccess) {
                                    pageData.order.Status = 2;
                                    pageData.order.StatusStr = '已确认';

                                    alert('已确认');
                                    setTimeout(function () {
                                        window.location.href = "/Admin/CarOrderManage/ServiceOrderList.aspx";
                                    }, 1200);

                                } else {
                                    alert('操作失败');
                                }

                            } else {
                                alert('操作失败');
                            }
                        }, function (data) {

                        }
                    );
                }
            };

            //核销订单
            pageFunc.useOrder = function () {
                if (confirm("核销当前订单？")) {
                    commService.postData(
                        pageData.handlerUrl,
                        {
                            action: 'EditServerOrderStatus',
                            ids: pageData.order.OrderId,
                            status: 2
                        },
                        function (data) {

                            if (data.isSuccess) {
                                pageData.order.Status = 2;
                                pageData.order.StatusStr = '已核销';

                                alert('已核销');
                                setTimeout(function () {
                                    window.location.href = "/Admin/CarOrderManage/ServiceOrderList.aspx";
                                }, 1200);

                            } else {
                                alert('操作失败');
                            }
                        }, function (data) {

                        }
                    );
                }
            };

            pageFunc.cancelOrder = function () {
                if (confirm("取消当前订单？")) {
                    commService.postData(
                        pageData.handlerUrl,
                        {
                            action: 'EditServerOrderStatus',
                            ids: pageData.order.OrderId,
                            status: 3
                        },
                        function (data) {

                            if (data.isSuccess) {
                                alert('已成功取消');
                                setTimeout(function () {
                                    window.location.href = "/Admin/CarOrderManage/ServiceOrderList.aspx";
                                }, 1200);

                            } else {
                                alert('操作失败');
                            }
                        }, function (data) {

                        }
                    );
                }
            };

            pageFunc.init();

        }]);

    </script>
</asp:Content>

