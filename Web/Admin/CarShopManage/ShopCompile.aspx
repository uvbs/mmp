<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="ShopCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarShopManage.ShopCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .modal-body{

        }
        .modal-body .col-sm-10{
            margin-bottom:10px;
        }
        .modal-body .control-label{
            margin-top: 8px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="wrapSeries" ng-controller="AddServerCtrl" style="padding-bottom: 124px;">
        <ol class="breadcrumb">
            <li class="">商户管理</li>
            <li class="active">添加商户</li>
		</ol>
        <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">


            <fieldset>
              <legend class="txtCenter pLeft50">基本信息</legend>
          <div class="form-group">
		    <label class="col-sm-2 control-label">商户类别</label>
			    <div class="col-sm-10">
                    <select class="form-control" ng-model="pageData.shopType">
                        <option>4S店</option>
                        <option>专修店</option>
                    </select>
                </div>
		    </div>

            
            <div class="form-group">
            <label  class="col-sm-2 control-label">商户登录名</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="商户登录名(注意创建后不能更改)" ng-model="pageData.userId" required ng-readonly="pageData.isEdit == 1"  onkeyup="value=value.replace(/[^\w\.\/]/ig,'')">
            </div>
          </div>
        
              <div class="form-group">
            <label  class="col-sm-2 control-label">商户登录密码</label>
            <div class="col-sm-10">
              <input type="password" class="form-control" placeholder="商户登录密码" ng-model="pageData.pwd" required>
            </div>
          </div>
        

          <div class="form-group">
            <label for="inputEmail3" class="col-sm-2 control-label">商户名称</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.company" placeholder="请输入商户名称" required>
            </div>
          </div>
          <div class="form-group">
            <label for="inputEmail3" class="col-sm-2 control-label">工时价</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.workHourPrice" placeholder="请输入工时价" required>
            </div>
          </div>
          
        <div class="form-group">
			<label class="col-sm-2 control-label">省份</label>
			<div class="col-sm-10">
                <select class="form-control" ng-options="item.name for item in pageData.provinceList" ng-model="pageData.selectProvince" ng-change="pageFunc.loadCity()">
                    
                </select>
            </div>
		</div>
            <div class="form-group">
			<label class="col-sm-2 control-label">城市</label>
			<div class="col-sm-10">
                <select class="form-control" ng-options="item.name for item in pageData.cityList" ng-model="pageData.selectCity" ng-change="pageFunc.loadDistrict()">
                    
                </select>
            </div>
		</div>
            <div class="form-group">
			<label class="col-sm-2 control-label">地区</label>
			<div class="col-sm-10">
                <select class="form-control" ng-options="item.name for item in pageData.districtList" ng-model="pageData.selectDistrict">
                    
                </select>
            </div>
		</div>
            <div class="form-group">
            <label  class="col-sm-2 control-label">详细地址</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="详细地址" ng-model="pageData.address" required>
            </div>
          </div>
          </fieldset>
          
            <fieldset>
              <legend class="txtCenter pLeft50">联系人信息</legend>

          <div class="form-group">
            <label  class="col-sm-2 control-label">售后联系人</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="售后联系人" ng-model="pageData.trueName" required>
            </div>
          </div>
            <div class="form-group">
            <label  class="col-sm-2 control-label">售后联系人职位</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="售后联系人职位" ng-model="pageData.postion" required>
            </div>
          </div>
            <div class="form-group">
            <label  class="col-sm-2 control-label">售后联系人电话</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="售后联系人电话" ng-model="pageData.phone" required>
            </div>
          </div>

            <div class="form-group">
            <label  class="col-sm-2 control-label">售前联系人</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="售前联系人" ng-model="pageData.preSaleLinker" required>
            </div>
          </div>
            <div class="form-group">
            <label  class="col-sm-2 control-label">售前联系人职位</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="售前联系人职位" ng-model="pageData.preSaleLinkerPostion" required>
            </div>
          </div>
            <div class="form-group">
            <label  class="col-sm-2 control-label">售前联系人电话</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="售前联系人电话" ng-model="pageData.preSaleLinkerPhone" required>
            </div>
          </div>
            
          </fieldset>


           <fieldset>
              <legend class="txtCenter pLeft50">商户车系</legend>
               <div class="form-group">
                
                <div class="col-sm-2">
                    
                </div>

                <div class="col-sm-10">
                    <a href="javascript:;" class="btn btn-info" ng-click="pageFunc.showAddCarSeries(saller);">关联指定车系</a>
                    <a href="javascript:;" class="btn btn-info" ng-click="pageFunc.clearCarSeries();">清空车系</a>
                   <table class="table table-hover" >                      
			                <thead>
				                <tr>
					                <th>
						                适配车系
					                </th>
					                <th></th>
				                </tr>
			                </thead>
			                <tbody style="max-height: 100px;    overflow: auto;">
				                <tr ng-repeat="item in pageData.currCarSeries">
					                <td>
						                {{item.brandName}}&nbsp;/&nbsp;{{item.seriesCateName}}&nbsp;/&nbsp;{{item.seriesName}}
					                </td>
					                
					                <td class="txtCenter">
                                        <a href="javascripe:;" class="btn btn-info btn-xs" ng-click="pageFunc.removeCarSeries(item,pageData.currCarSeries)">移除</a>
						                <!-- <i class="iconfont icon-edit"></i> -->
					                </td>
					
				                </tr>
			                </tbody>
		                </table>
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
           
                <button  type="submit" class="btn btn-primary">保存商户</button>
            
          </div>
        </form>
     </div>
    <script type="text/javascript">
        comeonModule.controller('AddServerCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'cateService', 'ngDialog', function ($scope, commService, $modal, FileUploader, cateService, ngDialog) {
            var pageData = $scope.pageData = {

                selectProvince: null,
                selectCity: null,
                selectDistrict: null,

                provinceList: [],
                cityList: [],
                districtList: [],

                company: '',
                address: '',
                trueName: '',
                postion: '',
                phone: '',
                userId: '',
                pwd: '',
                workHourPrice: 0,
                shopType: '',

                preSaleLinker: '',
                preSaleLinkerPostion: '',
                preSaleLinkerPhone: '',

                currAction: 'AddSysUser',

                isEdit: 0,
                editObj: null,

                selectCarData: [],
                currCarSeries: []


            };

            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {

                pageFunc.loadProvince();

                var urlAction = GetParm('action');

                if (urlAction == 'edit') {
                    pageData.isEdit = 1;

                    pageData.currAction = "EditSysUser";

                    pageData.editObj = JSON.parse(sessionStorage.getItem('editSallerItem'));

                    pageData.company = pageData.editObj.company;
                    pageData.address = pageData.editObj.address;
                    pageData.trueName = pageData.editObj.name;
                    pageData.postion = pageData.editObj.postion;
                    pageData.phone = pageData.editObj.phone;
                    pageData.pwd = pageData.editObj.pwd;
                    pageData.workHourPrice = pageData.editObj.ex1;

                    pageData.shopType = pageData.editObj.ex2;//商户类别

                    pageData.preSaleLinker = pageData.editObj.ex3;
                    pageData.preSaleLinkerPostion = pageData.editObj.ex4;
                    pageData.preSaleLinkerPhone = pageData.editObj.ex5;

                    pageData.userId = pageData.editObj.userId;

                    //company:'',
                    //address:'',
                    //trueName: '',
                    //postion:'',
                    //phone: '',
                    //pwd: '',
                    //workHourPrice:0,

                    commService.loadRemoteData(cc.handler.car, {
                        action: 'GetSallerAndCarSeries',
                        sallerId: pageData.userId
                    }, function (data) {
                        pageData.currCarSeries = data;
                    }, function (data) { });

                }

            };


            pageFunc.loadProvince = function () {
                commService.loadRemoteData("/Serv/pubapi.ashx", {
                    action: 'getGetKeyVauleDatas',
                    type: 'province'
                }, function (data) {
                    pageData.provinceList = data.list;

                    if (pageData.isEdit == 1) {
                        for (var i = 0; i < pageData.provinceList.length; i++) {
                            if (pageData.provinceList[i].name == pageData.editObj.province) {
                                pageData.selectProvince = pageData.provinceList[i];
                                pageFunc.loadCity();
                                break;
                            }
                        }
                    }

                }, function (data) { });
            };

            pageFunc.loadCity = function () {
                commService.loadRemoteData("/Serv/pubapi.ashx", {
                    action: 'getGetKeyVauleDatas',
                    type: 'city',
                    prekey: pageData.selectProvince.id
                }, function (data) {
                    pageData.cityList = data.list;

                    if (pageData.isEdit == 1) {
                        for (var i = 0; i < pageData.cityList.length; i++) {
                            if (pageData.cityList[i].name == pageData.editObj.city) {
                                pageData.selectCity = pageData.cityList[i];
                                pageFunc.loadDistrict();
                                break;
                            }
                        }
                    }

                }, function (data) { });
            };

            pageFunc.loadDistrict = function () {
                commService.loadRemoteData("/Serv/pubapi.ashx", {
                    action: 'getGetKeyVauleDatas',
                    type: 'district',
                    prekey: pageData.selectCity.id
                }, function (data) {
                    pageData.districtList = data.list;

                    if (pageData.isEdit == 1) {
                        for (var i = 0; i < pageData.districtList.length; i++) {
                            if (pageData.districtList[i].name == pageData.editObj.district) {
                                pageData.selectDistrict = pageData.districtList[i];
                                break;
                            }
                        }
                    }

                }, function (data) { });
            };

            pageFunc.showAddCarSeries = function () {
                ngDialog.open({ template: 'addCarSeries.html', scope: $scope });
            };

            pageFunc.removeCarSeries = function (item, array) {
                var i = array.IndexOf(item.seriesId);
                if (i > -1) {
                    array.RemoveIndexOf(i);
                }
            };

            pageFunc.addCarSeries = function () {

                if (!pageData.selectCarData.currSelectBrand) {
                    alert("请选择车系品牌");
                    return;
                }
                if (!pageData.selectCarData.currSelectSeriesCate) {
                    alert("请选择车系类别");
                    return;
                }

                //if (!pageData.selectCarData.currSelectSeries) {
                //    alert("请选择车系");
                //    return;
                //}

                if (!pageData.selectCarData.currSelectMultiSeries) {
                    alert("请选择车系");
                    return;
                }

                //console.log("pageData.selectCarData.currSelectMultiSeries",pageData.selectCarData.currSelectMultiSeries);
                //return;

                var multiSeries = pageData.selectCarData.currSelectMultiSeries;

                for (var i = 0; i < multiSeries.length; i++) {

                    var data = multiSeries[i];

                    var isNew = true;

                    for (var j = 0; j < pageData.currCarSeries.length; j++) {
                        if (pageData.currCarSeries[j].seriesId == data.CarSeriesId
                            ) {
                            isNew = false;
                        }
                    }

                    if (isNew) {
                        pageData.currCarSeries.push({
                            brandId: pageData.selectCarData.currSelectBrand.CarBrandId,
                            brandName: pageData.selectCarData.currSelectBrand.CarBrandName,
                            seriesCateId: pageData.selectCarData.currSelectSeriesCate.CarSeriesCateId,
                            seriesCateName: pageData.selectCarData.currSelectSeriesCate.CarSeriesCateName,
                            seriesId: data.CarSeriesId,
                            seriesName: data.CarSeriesName
                        });
                    }

                }

                

                //currCarSeries: [{ brandId: 0, seriesCateId: 0, seriesId: 0, brandName: '', seriesCateName: '', seriesName: '' }]

                ngDialog.closeAll();


            };

            pageFunc.clearCarSeries = function () {
                pageData.currCarSeries = [];
            };



            pageFunc.save = function () {

                if (!pageData.selectProvince) {
                    alert('请选择省份');
                    return;
                }
                if (!pageData.selectCity) {
                    alert('请选择城市');
                    return;
                }
                if (!pageData.selectDistrict) {
                    alert('请选择地区');
                    return;
                }

                var jsonData = {
                    action: pageData.currAction,
                    UserType: 5,
                    Company: pageData.company,
                    Province: pageData.selectProvince.name,
                    City: pageData.selectCity.name,
                    District: pageData.selectDistrict.name,
                    Address: pageData.address,
                    TrueName: pageData.trueName,
                    Postion:pageData.postion,
                    Phone: pageData.phone,
                    UserID: pageData.userId,
                    Pwd: pageData.pwd,
                    Ex1: pageData.workHourPrice,//purecar中 ex1 为工时价
                    Ex2:pageData.shopType,
                    Ex3: pageData.preSaleLinker,
                    Ex4: pageData.preSaleLinkerPostion,
                    Ex5: pageData.preSaleLinkerPhone,
                    isSubAccount:'1'
                };

                commService.postData('/Handler/App/CationHandler.ashx', jsonData, function (data) {

                    if (data.Status == 1) {
                        

                        commService.postData(
                            cc.handler.car,
                            {
                                action: 'AddSallerAndCarSeries',
                                data: JSON.stringify(pageData.currCarSeries),
                                sallerId: pageData.userId
                            },
                            function (data) {
                                console.log('AddSallerAndCarSeries', data);
                                alert('保存成功');
                                if (pageData.isEdit == 1) {
                                    setTimeout(function () {
                                        window.location.href = "/Admin/CarShopManage/List.aspx";
                                    }, 1200);
                                }

                            }, function (data) {
                                console.log('AddSallerAndCarSeries', data);
                            });

                        
                    } else {
                        alert(data.Msg);
                    }

                }, function (data) { });
            };
          
            pageFunc.init();

        }]);

    </script>

     <script type="text/ng-template" id="addCarSeries.html">
        <form class="" ng-submit="pageFunc.addCarSeries()">
	        <div class="modal-header">
	            <h2 class="modal-title">关联车系</h2>
	        </div>
	        <div class="modal-body" style="height:180px">
				<carmodelselect 
                ng-attr-selectdata="pageData.selectCarData"
                ng-attr-initbrandid="{{pageData.initBrandId}}"
                ng-attr-initseriescateid="{{pageData.initSeriesCateId}}"
                ng-attr-initseriesid="{{pageData.initSeriesId}}"
                ng-attr-initmodelid="{{pageData.initModelId}}"
                ng-attr-hidemodel="1"
                ng-attr-ismultiseries="1"
                ></carmodelselect>
	        </div>
           
	        <div class="modal-footer txtCenter">
	            <button  type="submit" class="btn btn-primary" ng-click="confirm()">确定</button>
	            <a href="javascript:;" class="btn btn-warning"  ng-click="closeThisDialog()">取消</a>
	        </div>
        </form>
    </script>

</asp:Content>

