<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="PartsCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarParts.PartsCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="wrapSeries pBottom72" ng-controller="AddServerCtrl">
        <ol class="breadcrumb">
            <li class="">配件管理</li>
            <li class="active">{{pageData.title}}</li>
		</ol>
        <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
            <carmodelselect 
                ng-attr-selectdata="pageData.selectData"
                ng-attr-initbrandid="{{pageData.initBrandId}}"
                ng-attr-initseriescateid="{{pageData.initSeriesCateId}}"
                ng-attr-initseriesid="{{pageData.initSeriesId}}"
                ng-attr-initmodelid="{{pageData.initModelId}}"
                ng-attr-disabled="{{pageData.isEdit}}"
                ng-attr-ismultimodel="1"
                ></carmodelselect>
            <div class="form-group">
                <label  class="col-sm-2 control-label">配件分类</label>
                <div class="col-sm-10">
                    <select class="form-control" ng-model="pageData.selectPartsCate" ng-options="item as item.CategoryName for item in pageData.partsCateList">
                    
                    </select>
                </div>
              </div>
            <div class="form-group">
                <label  class="col-sm-2 control-label">配件品牌</label>
                <div class="col-sm-10">
                    <select class="form-control" ng-model="pageData.selectPartsBrand" ng-options="item as item.CategoryName for item in pageData.partsBrandList">
                    
                    </select>
                </div>
              </div>
          <div class="form-group">
            <label for="inputEmail3" class="col-sm-2 control-label">配件名称</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" id="inputEmail3" ng-model="pageData.parts.PartName" placeholder="配件名称" required>
            </div>
          </div>
          
        <div class="form-group">
			<label class="col-sm-2 control-label">配件数量</label>
			<div class="col-sm-10">
                <input type="text" ng-model="pageData.parts.Count" class="form-control" placeholder="配件数量" required>
            </div>
		</div>
            <div class="form-group">
			<label class="col-sm-2 control-label">配件规格</label>
			<div class="col-sm-10">
                <input type="text" ng-model="pageData.parts.PartsSpecs" class="form-control" placeholder="配件规格" required>
            </div>
		</div>

            <div class="form-group">
            <label for="inputPassword3" class="col-sm-2 control-label">配件价格</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.parts.Price" placeholder="配件价格" required>
            </div>
          </div>
            
          <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
              
                <button  type="submit" class="btn btn-primary">保存配件</button>
            </div>
          </div>
        </form>
     </div>
    <script type="text/javascript">
        comeonModule.controller('AddServerCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'cateService', function ($scope, commService, $modal, FileUploader, cateService) {
            var pageData = $scope.pageData = {
                    title: '添加配件',
                    handlerUrl: cc.handler.car,
                    parts: {},
                    selectData: {},
                    currAction: 'AddParts',
                    editId: 0,
                    initBrandId: 0,
                    initSeriesCateId: 0,
                    initSeriesId: 0,
                    initModelId: 0,
                    isEdit: 0,
                    partsBrandList:[],
                    partsCateList:[],
                    selectPartsCate:{},
                    selectPartsBrand:{}
                };

            //pageData.parts.CarModelId

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {

                var urlAction = GetParm('action');

                if (urlAction == 'edit') {
                    pageData.isEdit = 1;
                    pageData.currAction = 'EditParts';
                    pageData.title = "编辑配件";
                    pageData.editId = GetParm('id');
                    pageData.parts = JSON.parse(sessionStorage.getItem("editPartsItem"));

                    pageData.initBrandId = pageData.parts.CarBrandId;
                    pageData.initSeriesCateId = pageData.parts.CarSeriesCateId;
                    pageData.initSeriesId = pageData.parts.CarSeriesId;
                    pageData.initModelId = pageData.parts.CarModelId;

                    console.log('editPartsItem', pageData.parts);
                }

                //加载配件分类
                cateService.cateList(cc.cate.purecar.partsCate, function (data) {
                    pageData.partsCateList = data;
                    if (pageData.isEdit == 1) {
                        for (var i = 0; i < pageData.partsCateList.length; i++) {
                            if (pageData.partsCateList[i].AutoID == pageData.parts.PartsCateId) {
                                pageData.selectPartsCate = pageData.partsCateList[i];
                                break;
                            }
                        }
                    }
                });

                //加载配件品牌
                cateService.cateList(cc.cate.purecar.partsBrand, function (data) {
                    pageData.partsBrandList = data;
                    if (pageData.isEdit == 1) {
                        for (var i = 0; i < pageData.partsBrandList.length; i++) {
                            if (pageData.partsBrandList[i].AutoID == pageData.parts.PartsBrandId) {
                                pageData.selectPartsBrand = pageData.partsBrandList[i];
                                break;
                            }
                        }
                    }
                });



            };

            pageFunc.save = function () {



                if (!pageData.selectData.currSelectBrand) {
                    alert("请选择车系品牌");
                    return;
                }
                if (!pageData.selectData.currSelectSeriesCate) {
                    alert("请选择车系类别");
                    return;
                }

                if (!pageData.selectData.currSelectSeries) {
                    alert("请选择车系");
                    return;
                }

                if (!pageData.selectData.currSelectMultiModel) {
                    alert("请选择车型");
                    return;
                }

                if (!pageData.selectPartsCate) {
                    alert('请选择分类');
                    return;
                }

                if (!pageData.selectPartsBrand) {
                    alert('请选择品牌');
                    return;
                }

                //console.log('selectData', pageData.selectData);

                var modelId = [];
                var modelList = pageData.selectData.currSelectMultiModel;
                for (var i = 0; i < modelList.length; i++) {
                    modelId.push(modelList[i].CarModelId);
                }

                var jsonData = {
                    partName: pageData.parts.PartName,
                    price: pageData.parts.Price,
                    carBrandId: pageData.selectData.currSelectBrand.CarBrandId,
                    carSeriesCateId: pageData.selectData.currSelectSeriesCate.CarSeriesCateId,
                    carSeriesId: pageData.selectData.currSelectSeries.CarSeriesId,
                    carModelId: modelId.join(','), //pageData.selectData.currSelectCarModel.CarModelId,
                    count: pageData.parts.Count,
                    partId: pageData.editId,
                    action:pageData.currAction,
                    partsCateId:pageData.selectPartsCate.AutoID,
                    partsCateName:pageData.selectPartsCate.CategoryName,
                    partsBrandId:pageData.selectPartsBrand.AutoID,
                    partsBrandName:pageData.selectPartsBrand.CategoryName,
                    partsSpecs:pageData.parts.PartsSpecs
                };
                
                commService.postData(
                    pageData.handlerUrl,
                    jsonData,
                    function (data) {
                        
                        if (data.isSuccess) {
                            alert('保存成功');
                            setTimeout(function () {
                                window.location.href = "/Admin/CarParts/List.aspx";
                            }, 1200);

                            //if (pageData.currAction == 'EditParts') {
                            //    setTimeout(function () {
                            //        window.location.href = "/Admin/CarParts/List.aspx";
                            //    },1200);
                            //}

                        } else {
                            alert('保存失败');
                        }
                    }, function (data) {

                    });

            };
          
            pageFunc.init();

        }]);

    </script>
</asp:Content>
