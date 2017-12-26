<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="WorkHoursCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarWorkManage.WorkHoursCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="wrapSeries pBottom72" ng-controller="AddServerCtrl">
        <ol class="breadcrumb">
            <li class="">工时表管理</li>
            <li class="active">添加工时价</li>
		</ol>
      <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
          <div class="form-group">
            <label class="col-sm-2 control-label">所属商户</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="请输入商户ID" ng-model="pageData.obj.SallerId" required ng-readonly="pageData.isEdit">
            </div>
          </div>

           <carmodelselect 
                ng-attr-selectdata="pageData.selectCarData"
                ng-attr-initbrandid="{{pageData.initBrandId}}"
                ng-attr-initseriescateid="{{pageData.initSeriesCateId}}"
                ng-attr-initseriesid="{{pageData.initSeriesId}}"
                ng-attr-initmodelid="{{pageData.initModelId}}"
                ng-attr-disabled="{{pageData.isEdit}}"
                ></carmodelselect>

          <div class="form-group">
            <label class="col-sm-2 control-label">工时价</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.obj.Price" placeholder="工时价">
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
                    title: '添加工时价',
                    handlerUrl: cc.handler.car,
                    obj:{},
                    isEdit: 0,
                    currAction: 'AddCarWorkhoursPrice'
                };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                var urlAction = GetParm('action');

                if (urlAction == 'edit') {
                    pageData.isEdit = 1;

                    pageData.currAction = "EditCarWorkhoursPrice";

                    var editObj = JSON.parse(sessionStorage.getItem('EditCarWorkhoursPriceItem'));

                    pageData.initBrandId = editObj.CarBrandId;
                    pageData.initSeriesCateId = editObj.CarSeriesCateId;
                    pageData.initSeriesId = editObj.CarSeriesId;
                    pageData.initModelId = editObj.CarModelId;

                    pageData.obj = editObj;

                }
            };

            pageFunc.save = function () {


                if (typeof(pageData.selectCarData.currSelectBrand) != 'undefined') {
                    pageData.obj.CarBrandId = pageData.selectCarData.currSelectBrand.CarBrandId;
                    pageData.obj.CarBrandName = pageData.selectCarData.currSelectBrand.CarBrandName;
                }
                
                if (typeof(pageData.selectCarData.currSelectSeriesCate) != 'undefined') {
                    pageData.obj.CarSeriesCateId = pageData.selectCarData.currSelectSeriesCate.CarSeriesCateId;
                    pageData.obj.CarSeriesCateName = pageData.selectCarData.currSelectSeriesCate.CarSeriesCateName;
                }

                if (typeof(pageData.selectCarData.currSelectSeries) != 'undefined') {
                    pageData.obj.CarSeriesId = pageData.selectCarData.currSelectSeries.CarSeriesId;
                    pageData.obj.CarSeriesName = pageData.selectCarData.currSelectSeries.CarSeriesName;
                }

                if (typeof (pageData.selectCarData.currSelectCarModel) != 'undefined') {
                    pageData.obj.CarModelId = pageData.selectCarData.currSelectCarModel.CarModelId;
                    pageData.obj.CarModelName = pageData.selectCarData.currSelectCarModel.CarModelName;
                }

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
                            if (pageData.currAction == 'EditCarWorkhoursPrice') {
                                setTimeout(function () {
                                    window.location.href = "WorkHoursList.aspx";
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
