<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="CarDiscountRateCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarWorkManage.CarDiscountRateCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="wrapSeries pBottom72" ng-controller="AddServerCtrl">
        <ol class="breadcrumb">
            <li class="">工时表管理</li>
            <li class="active">添加工时配件折扣</li>
		</ol>
       
               <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
          <div class="form-group">
            <label class="col-sm-2 control-label">所属商户</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="请输入商户ID" ng-model="pageData.obj.SallerId" required ng-readonly="pageData.isEdit">
            </div>
          </div>
          
        <div class="form-group">
			<label class="col-sm-2 control-label">星期</label>
			<div class="col-sm-10">
                <select class="form-control" required ng-model="pageData.obj.Week">
                    <option value="1">星期一</option>
                    <option value="2">星期二</option>
                    <option value="3">星期三</option>
                    <option value="4">星期四</option>
                    <option value="5">星期五</option>
                    <option value="6">星期六</option>
                    <option value="0">星期日</option>
                </select>
            </div>
		</div>

<%--            <div class="form-group">
			<label class="col-sm-2 control-label">开始时间</label>
			<div class="col-sm-10">
                <select class="form-control" required ng-model="pageData.obj.StartTime">
                    <option value="8:00">8:00</option>
                    <option value="9:00">9:00</option>
                    <option value="10:00">10:00</option>
                    <option value="11:00">11:00</option>
                    <option value="12:00">12:00</option>
                    <option value="13:00">13:00</option>
                    <option value="14:00">14:00</option>
                    <option value="15:00">15:00</option>
                    <option value="16:00">16:00</option>
                    <option value="17:00">17:00</option>
                    <option value="18:00">18:00</option>
                    <option value="19:00">19:00</option>
                    <option value="20:00">20:00</option>
                </select>
            </div>
		</div>--%>
            <%--<div class="form-group">
			<label class="col-sm-2 control-label">结束时间</label>
			<div class="col-sm-10">
                <select class="form-control" required ng-model="pageData.obj.EndTime">
                     <option value="8:00">8:00</option>
                    <option value="9:00">9:00</option>
                    <option value="10:00">10:00</option>
                    <option value="11:00">11:00</option>
                    <option value="12:00">12:00</option>
                    <option value="13:00">13:00</option>
                    <option value="14:00">14:00</option>
                    <option value="15:00">15:00</option>
                    <option value="16:00">16:00</option>
                    <option value="17:00">17:00</option>
                    <option value="18:00">18:00</option>
                    <option value="19:00">19:00</option>
                    <option value="20:00">20:00</option>
                </select>
            </div>
		</div>--%>
        <div class="form-group">
			<label class="col-sm-2 control-label">时间段</label>
			<div class="col-sm-10">
                <select class="form-control" ng-model="pageData.obj.TimeInterval">
                    <option value="8:30-11:30">8:30-11:30</option>
                    <option value="11:30-14:30">11:30-14:30</option>
                    <option value="14:30-17:30">14:30-17:30</option>
                </select>
            </div>
		</div>
          <div class="form-group">
            <label for="inputPassword3" class="col-sm-2 control-label">工时折扣率</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="工时折扣率"  required ng-model="pageData.obj.WorkhoursRate" >
            </div>
          </div>
            <div class="form-group">
            <label for="inputPassword3" class="col-sm-2 control-label">配件折扣率</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="配件折扣率" required ng-model="pageData.obj.PartsRate">
            </div>
          </div>
         <div class="form-group">
            <label class="col-sm-2 control-label">备注</label>
            <div class="col-sm-10">
                <textarea ng-model="pageData.obj.Msg" class="form-control"></textarea>
            </div>
          </div>
          <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
              
                <button  type="submit" class="btn btn-primary">保存</button>
            </div>
          </div>
        </form>
     </div>
    <script type="text/javascript">
        comeonModule.controller('AddServerCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'cateService', function ($scope, commService, $modal, FileUploader, cateService) {
            var pageData = $scope.pageData = {
                    title: '',
                    handlerUrl: cc.handler.car,
                    obj:{},
                    isEdit: 0,
                    currAction: 'AddCarDiscountRate'
                };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                var urlAction = GetParm('action');

                if (urlAction == 'edit') {
                    pageData.isEdit = 1;

                    pageData.currAction = "EditCarDiscountRate";

                    var editObj = JSON.parse(sessionStorage.getItem('EditCarDiscountRateItem'));

                    pageData.obj = editObj;

                }
            };

            pageFunc.save = function () {

                if (!pageData.obj.TimeInterval) {
                    alert('请选择一个时间段');
                    return;
                }

                if (!pageData.obj.Week) {
                    alert('请选择星期');
                    return;
                }

                var time = pageData.obj.TimeInterval.split('-');

                pageData.obj.StartTime = time[0];
                pageData.obj.EndTime = time[1];

                pageData.obj.WorkhoursRate = parseFloat(pageData.obj.WorkhoursRate);
                pageData.obj.PartsRate = parseFloat(pageData.obj.PartsRate);

                var jsonData = {
                    action: pageData.currAction,
                    data:JSON.stringify(pageData.obj)
                };

                commService.postData(
                    pageData.handlerUrl,
                    jsonData,
                    function (data) {

                        if (data.isSuccess) {
                            alert('保存成功');
                            if (pageData.currAction == 'EditCarDiscountRate') {
                                setTimeout(function () {
                                    window.location.href = "CarDiscountRateList.aspx";
                                }, 1200);
                            }

                        } else {
                            if (data.errmsg) {
                                alert(data.errmsg);
                            } else {
                                alert('保存失败');
                            }
                        }
                    }, function (data) {

                    });

            };


          
            pageFunc.init();

        }]);

    </script>
</asp:Content>

