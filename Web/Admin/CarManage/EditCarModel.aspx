<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="EditCarModel.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarManage.EditCarModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="wrapSeries" ng-controller="AddServerCtrl" style="padding-bottom: 124px;">
        <ol class="breadcrumb">
            <li class="">车型库</li>
            <li class=""> <a href="CarManage.aspx" target="_self">车型管理</a></li>
            <li class="active">{{pageData.title}}</li>
		</ol>
        <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
        <fieldset>
        <legend class="txtCenter pLeft50">品牌车系信息</legend>

            <carmodelselect 
            ng-attr-selectdata="pageData.selectData"
            ng-attr-initbrandid="{{pageData.initBrandId}}"
            ng-attr-initseriescateid="{{pageData.initSeriesCateId}}"
            ng-attr-initseriesid="{{pageData.initSeriesId}}"
            ng-attr-initmodelid="{{pageData.initModelId}}"
            ng-attr-disabled="{{pageData.isEdit}}"
            ></carmodelselect>

        </fieldset>


            <fieldset>
            <legend class="txtCenter pLeft50">基本信息</legend>

            <div class="form-group">
                <label class="col-sm-2 control-label">车型名称</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.CarModelName" placeholder="车型名称" required>
                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">指导价格</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.GuidePrice" placeholder="指导价格" required>
                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">汽车图片</label>
                <div class="col-sm-10">
                  <input type="file" id="inputFile" uploader="uploader" nv-file-select="">
                    <img class="img-rounded" style="margin-top:16px;    max-width: 200px;" ng-src="{{pageData.parts.Img}}" ng-if="pageData.parts.Img != ''">

                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">支持颜色</label>
                <div class="col-sm-10">
                  <div>
                      <input type="text" class="form-control" ng-model="pageData.addColorName" placeholder="颜色名称">
                      <a href="javascript:;" class="btn btn-info" ng-click="pageFunc.addColor();" style="margin:10px 0;">添加颜色</a>
                  </div>
                  <div>

                      <table class="table table-hover" >                      
			                <thead>
				                <tr>
					                <th>
						                颜色名称
					                </th>
					                <th></th>
				                </tr>
			                </thead>
			                <tbody style="max-height: 100px;    overflow: auto;">
				                <tr ng-repeat="item in pageData.parts.Colors">
					                <td>
						                {{item.Name}}
					                </td>
					                <td class="txtCenter">
                                        <a href="javascript:;" class="btn btn-info btn-xs" ng-click="pageFunc.deleteColor(item)">移除</a>
						                <!-- <i class="iconfont icon-edit"></i> -->
					                </td>
					
				                </tr>
			                </tbody>
		                </table>

                  </div>
                </div>
            </div>
            
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
                <button  type="submit" class="btn btn-primary">保存</button>
            </div>
         
        </form>
     </div>
    <script type="text/javascript">
        comeonModule.controller('AddServerCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'cateService', function ($scope, commService, $modal, FileUploader, cateService) {
            var pageData = $scope.pageData = {
                    title: '编辑车型',
                    handlerUrl: cc.handler.car,
                    parts: {},
                    selectData: {},
                    currAction: 'EditCarModel',
                    initBrandId: 0,
                    initSeriesCateId: 0,
                    initSeriesId: 0,
                    initModelId: 0,
                    isEdit: 0,
                    partsBrandList:[],
                    partsCateList:[],
                    selectPartsCate:{},
                    selectPartsBrand:{},


                };

            //pageData.parts.CarModelId

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {

                //var urlAction = GetParm('action');

                //if (urlAction == 'edit') {
                    pageData.isEdit = 1;
                    pageData.title = "编辑车型";
                    //pageData.editId = GetParm('id');
                    pageData.parts = JSON.parse(sessionStorage.getItem("EditCarModel"));

                    pageData.initBrandId = pageData.parts.CarBrandId;
                    pageData.initSeriesCateId = pageData.parts.CarSeriesCateId;
                    pageData.initSeriesId = pageData.parts.CarSeriesId;
                    pageData.initModelId = pageData.parts.CarModelId;
                    
                    if (pageData.parts.Colors) {
                        pageData.parts.Colors = JSON.parse(pageData.parts.Colors);
                    }
                    
                    console.log('EditBuyCarOrder', pageData.parts);
                //}

            };

            pageFunc.save = function () {

                console.log('selectData', pageData.selectData);

                pageData.parts.IsShopInsurance = pageData.parts.IsShopInsurance ? 1 : 0;
                
                if (pageData.parts.Colors) {
                    pageData.parts.Colors = JSON.stringify(pageData.parts.Colors);
                }

                var jsonData = {
                    action: 'EditCarModel',
                    data:JSON.stringify(pageData.parts)
                };
                
                commService.postData(
                    pageData.handlerUrl,
                    jsonData,
                    function (data) {
                        
                        if (data.isSuccess) {
                            alert('保存成功');
                          
                                setTimeout(function () {
                                    window.location.href = "/Admin/CarManage/CarManage.aspx";
                                },1200);
                            

                        } else {
                            alert('保存失败');
                        }
                    }, function (data) {

                    });

            };
          

            pageFunc.addColor = function () {
                if (!pageData.addColorName) {
                    alert('请输入颜色名称!');
                    return;
                }
                var array = pageData.parts.Colors;
                for (var i = 0; i < array.length; i++) {
                    if (array[i].Name == pageData.addColorName) {
                        alert('已存在颜色!');
                        return;
                    }
                }

                array.push({
                    Name: pageData.addColorName,
                    Value: '',
                    Img:''
                });
                pageData.addColorName = '';

            };

            pageFunc.deleteColor = function (item) {
                var array = pageData.parts.Colors;
                var i = array.IndexOf(item.Name);
                if (i > -1) {
                    array.RemoveIndexOf(i);
                }
            };

            var uploader = $scope.uploader = new FileUploader({
                url: '/Handler/UeditorController.ashx?action=uploadimage&configPath=ubiConfig.json&noCache=' + Math.random(),
            });

            uploader.filters.push({
                name: 'imageFilter',
                fn: function (i /*{File|FileLikeObject}*/, options) {
                    var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            });

            uploader.onAfterAddingAll = function (addedFileItems) {
                layer.load();
                uploader.uploadAll();
            };
            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                layer.closeAll();
                if (response.state.toLowerCase() == "success") {
                    pageData.parts.Img = response.url;
                }
            };


            pageFunc.init();

        }]);

    </script>
</asp:Content>
