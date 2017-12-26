<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="AddServer.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarServer.AddServer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .left-panel {
          float: left;
        }

        .left-panel img {
          width: 48px;
          height: 48px;
          vertical-align: middle;
        }

        .right-panel {
          float: left;
          margin-left: 5px;
          margin-top: 7px;
        }

        .right-panel span:first-child {
          font-size: 16px;
        }

        .right-panel span:nth-child(2) {
          font-size: 14px;
          color: gray;
        }

        .right-panel span:last-child {
          display: block;
          font-size: 14px;
          font-style: italic;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="wrapSeries" ng-controller="AddServerCtrl" style="padding-bottom: 124px;    padding-top: 50px;">
        <ol class="breadcrumb" style="
                position: fixed;
                top: 0;
                width: 100%;
            "            
            >
            <li class="">服务管理</li>
            <li class="active">添加服务</li>
		</ol>
        <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
            <carmodelselect 
                ng-attr-selectdata="pageData.selectCarData"
                ng-attr-initbrandid="{{pageData.initBrandId}}"
                ng-attr-initseriescateid="{{pageData.initSeriesCateId}}"
                ng-attr-initseriesid="{{pageData.initSeriesId}}"
                ng-attr-initmodelid="{{pageData.initModelId}}"
                ng-attr-disabled="{{pageData.isEdit}}"
                ></carmodelselect>
            <div class="form-group">
			    <label class="col-sm-2 control-label">服务类型</label>
			    <div class="col-sm-10">
                <select class="form-control"  ng-model="pageData.server.ServerType">
                    <option>门店服务</option>
                    <option>上门服务</option>
                    </select>
                </div>
		    </div>
            <div class="form-group">
		    <label class="col-sm-2 control-label">门店类型</label>
			    <div class="col-sm-10">
                    <select class="form-control" ng-model="pageData.server.ShopType">
                        <option>4S店</option>
                        <option>专修店</option>
                    </select>
                </div>
		    </div>
              <div class="form-group">
                <label  class="col-sm-2 control-label">服务分类</label>
                <div class="col-sm-10">
                    <select class="form-control" ng-model="pageData.server.Cate" ng-options="item as item.CategoryName for item in pageData.cateList">
                    
                    </select>
                </div>
              </div>
            <div class="form-group">
            <label  class="col-sm-2 control-label">服务名称</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  placeholder="服务名称" ng-model="pageData.server.ServerName" required>
            </div>
          </div>
          <%--<div class="form-group">
            <label for="inputPassword3" class="col-sm-2 control-label">服务价格</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="服务价格">
            </div>
          </div>--%>
            <%--<div class="form-group">
            <label for="inputPassword3" class="col-sm-2 control-label">指定商户</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" placeholder="指定商户id，逗号分隔多个，指定服务类别只会列出关联的商户">
            </div>
          </div>--%>
        <div class="form-group">
            <label class="col-sm-2 control-label">指定商户</label>
            <div class="col-sm-10">
                <tags-input
                    ng-model="pageData.sallers"
                    display-property="UserID" 
                    placeholder="选择一个商户" 
                     replace-spaces-with-dashes="false"
                    min-length="0"
                    >
                    <auto-complete 
                        source="pageFunc.loadSaller($query)"
                        min-length="0"
                        load-on-focus="true"
                        load-on-empty="true"
                        max-results-to-show="32"
                        template="tplAutocomplete"
                        ></auto-complete>
                </tags-input>
                
            </div>
        </div>
        
        
        <div class="form-group">
			<label class="col-sm-2 control-label">工时数量</label>
			<div class="col-sm-10">
                <input type="text" class="form-control" placeholder="工时数量" ng-model="pageData.server.WorkHours">
            </div>
		</div>

            <div class="form-group">
				<label class="col-sm-2 control-label">配件列表</label>
				<div class="col-sm-10">
                    
                    <%--<button  type="submit" class="btn btn-info" ng-click="pageFunc.showAddParts(pageData.server)">关联指定商户配件</button>--%>
                    <fieldset>
                        <legend>默认服务配件</legend>
                        <a  href="javascript:;" class="btn btn-info" ng-click="pageFunc.showAddParts(pageData.defParts)">关联默认配件</a>
                         <table class="table table-hover" >                      
			                <thead>
				                <tr>
					                <th>
						                配件名称
					                </th>
					                <th class="txtCenter">配件数量</th>
					                <th></th>
				                </tr>
			                </thead>
			                <tbody style="max-height: 100px;    overflow: auto;">
				                <tr ng-repeat="item in pageData.defParts">
					                <td>
						                {{item.parts.PartName}}
					                </td>
					                <td class="txtCenter">
						                {{item.count}}
					                </td>
					                <td class="txtCenter">
                                        <button class="btn btn-info btn-xs" ng-click="pageFunc.deleteParts(item,pageData.defParts)">移除</button>
						                <!-- <i class="iconfont icon-edit"></i> -->
					                </td>
					
				                </tr>
			                </tbody>
		                </table>
                        
                    </fieldset>
                    
                    <fieldset ng-repeat="saller in pageData.sallers">
                        <legend>商户 {{saller.UserID}} 指定服务配件</legend>
                        <a href="javascript:;" class="btn btn-info" ng-click="pageFunc.showAddSallerParts(saller);">关联指定配件</a>
                         <table class="table table-hover" >                      
			                <thead>
				                <tr>
					                <th>
						                配件名称
					                </th>
					                <th class="txtCenter">配件数量</th>
					                <th></th>
				                </tr>
			                </thead>
			                <tbody style="max-height: 100px;    overflow: auto;">
				                <tr ng-repeat="item in saller.parts">
					                <td>
						                {{item.parts.PartName}}
					                </td>
					                <td class="txtCenter">
						                {{item.count}}
					                </td>
					                <td class="txtCenter">
                                        <button class="btn btn-info btn-xs" ng-click="pageFunc.deleteParts(item,saller.parts)">移除</button>
						                <!-- <i class="iconfont icon-edit"></i> -->
					                </td>
					
				                </tr>
			                </tbody>
		                </table>
                        
                    </fieldset>

                </div>
			</div>
            
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
            
                <button  type="submit" class="btn btn-primary">保存服务</button>
           
          </div>
        </form>
     </div>
    <script type="text/javascript">
        comeonModule.controller('AddServerCtrl', ['$scope', 'commService', '$modal', 'FileUploader', 'cateService', 'carService', '$q', 'ngDialog', function ($scope, commService, $modal, FileUploader, cateService, carService, $q, ngDialog) {
            var pageData = $scope.pageData = {
                title: '添加服务',
                handlerUrl: cc.handler.car,
                server: {},

                cateList: [],//分类列表

                sallers: [], //[{ UserID: 'jubit',parts:[{ count: 100, partsId: 1, parts: { PartName:'测试'}}] }, { UserID: 'hf' }]

                defParts: [],//[{ count: 100, partsId: 1, parts: { PartName:'测试'}}],

                selectCarData: {},

                selectPartsSource: [],
                selectParts: null,
                selectPartsCount:1,

                currEditParts: [],//当前编辑的配件实体
                
                initBrandId: 0,
                initSeriesCateId: 0,
                initSeriesId: 0,
                initModelId: 0,

                isEdit: 0,

                currAction: 'AddServer'


            };
            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {

                console.log(1);
                var urlAction = GetParm('action');

                if (urlAction == 'edit') {
                    pageData.isEdit = 1;

                    pageData.currAction = "EditServer";

                    var editObj = JSON.parse(sessionStorage.getItem('editServerItem'));

                    pageData.initBrandId = editObj.CarBrandId;
                    pageData.initSeriesCateId = editObj.CarSeriesCateId;
                    pageData.initSeriesId = editObj.CarSeriesId;
                    pageData.initModelId = editObj.CarModelId;

                    pageData.server.ServerType = editObj.ServerType;
                    pageData.server.ShopType = editObj.ShopType;
                    pageData.server.ServerName = editObj.ServerName;
                    pageData.server.WorkHours = editObj.WorkHours;
                    pageData.server.ServerId = editObj.ServerId;

                    //sallers: JSON.stringify(pageData.sallers),
                    //defParts: JSON.stringify(pageData.defParts)

                    commService.loadRemoteData(pageData.handlerUrl, { action: 'GetServerRelationList', serverId: pageData.server.ServerId }, function (data) {

                        console.log(data);

                        pageData.sallers = data.sallers;
                        pageData.defParts = data.defParts;

                        if (!pageData.defParts) {
                            pageData.defParts = [];
                        }

                    }, function (data) { });

                }

                //加载服务分类列表
                cateService.cateList(cc.cate.purecar.serverCate, function (data) {
                    pageData.cateList = data;
                    if (urlAction == 'edit') {
                        for (var i = 0; i < pageData.cateList.length; i++) {
                            if (pageData.cateList[i].AutoID == editObj.CateId) {
                                pageData.server.Cate = pageData.cateList[i];
                                break;
                            }
                        }
                    }
                });

                $scope.$watch('pageData.selectCarData.currSelectCarModel', pageFunc.carModelChange, true);

                if (!pageData.defParts) {
                    pageData.defParts = [];
                }

            };

            pageFunc.carModelChange = function()
            {
                console.log(pageData.selectCarData.currSelectCarModel);
                if (pageData.selectCarData.currSelectCarModel) {

                    //加载配件数据
                    carService.loadParts({ carModelId: pageData.selectCarData.currSelectCarModel.CarModelId }, function (data) {
                        console.log(data);
                        pageData.selectPartsSource = data.list
                    });


                } else {
                    pageData.selectPartsSource = [];
                }
            };

            pageFunc.showAddSallerParts = function (saller) {
                if (!saller.parts) saller.parts = [];

                pageFunc.showAddParts(saller.parts);

            };

            pageFunc.showAddParts = function (partsObj) {

                pageData.currEditParts = partsObj;

                pageData.selectPartsCount = 1;
                pageData.selectParts = null;
                if (pageData.selectPartsSource && pageData.selectPartsSource.length > 0) {
                    ngDialog.open({ template: 'addServerParts.html', scope: $scope });
                } else {
                    if (pageData.selectCarData.currSelectCarModel) {
                        alert('当前车型下没有配件');
                    } else {
                        alert('请选择车型');
                    }
                }
                //var modal = $modal.open({
                //    animation: true,
                //    templateUrl: 'addServerParts.html',
                //    controller: function ($scope, $modalInstance, FileUploader) {
                //        var editModalFn = $scope.editModalFn = {};
                //        var editModalData = $scope.editModalData = {
                //            currEditData: JSON.parse(JSON.stringify(item)),
                //            selectPart: {},
                //            setCount:1,
                //            parts: [{
                //                id:1,
                //                name: '洗洁剂',
                //                count: 100,
                //                label: '洗洁剂'
                //            }, {
                //                id:2,
                //                name: '刷子',
                //                count: 50,
                //                label: '刷子'
                //            }, {
                //                id:3,
                //                name: '洗洁剂1',
                //                count: 100,
                //                label: '洗洁剂1'
                //            }, {
                //                id:4,
                //                name: '刷子1',
                //                count: 50,
                //                label: '刷子1'
                //            }]
                //        };

                //        editModalFn.ok = function () {                            
                //            $modalInstance.close();
                //        };

                //        editModalFn.cancel = function () {
                //            $modalInstance.close();
                //        };
                //        editModalFn.selectChange = function () {
                //            console.log("select part",editModalData.selectPart);
                //        };

                //    }
                //});
            };

            pageFunc.addParts = function () {
                
                //验证
                if (!pageData.selectParts) {
                    alert('请选择一个配件');
                    return;
                }

                if (pageData.selectPartsCount == 0) {
                    alert('配件数量不能为 0');
                    return;
                }

                //if (pageData.selectParts.Count - pageData.selectPartsCount) {

                //}
                var isNew = true;

                for (var i = 0; i < pageData.currEditParts.length; i++) {
                    if (pageData.currEditParts[i].partsId == pageData.selectParts.PartId) {
                        isNew = false;
                        pageData.currEditParts[i] = {
                            count: pageData.selectPartsCount,
                            parts: pageData.selectParts,
                            partsId: pageData.selectParts.PartId
                        };
                    }
                }
              
                if (isNew) {
                    pageData.currEditParts.push({
                        count: pageData.selectPartsCount,
                        parts: pageData.selectParts,
                        partsId: pageData.selectParts.PartId
                    });
                }

                pageData.selectPartsCount = 1;
                pageData.selectParts = null;


                ngDialog.closeAll();
            };

            pageFunc.deleteParts = function (item, array) {
                var i = array.IndexOf(item.PartId);
                if (i > -1) {
                    array.RemoveIndexOf(i);
                }
            };

            pageFunc.loadSaller = function ($query) {
                var deferred = $q.defer();
                var userIds = [];
                var filterUserIds = '';

                if (pageData.sallers) {
                    for (var i = 0; i < pageData.sallers.length; i++) {
                        userIds.push(pageData.sallers[i].UserID);
                    }
                }
                
                if (userIds.length > 0) {
                    filterUserIds = userIds.join(',');
                }

                carService.loadSallerAutocomplete($query, filterUserIds, function (resp) {
                    console.log('loadSallerAutocomplete', resp);
                    deferred.resolve(resp.data);
                });
                return deferred.promise;
            };

            pageFunc.save = function () {

                //carService.addServer({
                //    carBrandId: pageData.selectCarData.currSelectBrand.CarBrandId,
                //    carSeriesCateId: pageData.selectCarData.currSelectSeriesCate.CarSeriesCateId,
                //    carSeriesId: pageData.selectCarData.currSelectSeries.CarSeriesId,
                //    carModelId: pageData.selectCarData.currSelectCarModel.CarModelId,
                //    serverType: pageData.server.ServerType,
                //    shopType: pageData.server.ShopType,
                //    cateId: pageData.server.Cate.AutoID,
                //    serverName: pageData.server.ServerName,
                //    workHours: pageData.server.WorkHours,
                //    sallers: JSON.stringify(pageData.sallers),
                //    defParts: JSON.stringify(pageData.defParts)
                //}, function (data) {
                //    if (data.isSuccess) {
                //        alert('保存成功');

                //    } else {
                //        alert('保存失败');
                //    }
                //});

                if (!pageData.server.Cate) {
                    alert('请选择分类');
                    return;
                }

                var jsonData = {
                    action: pageData.currAction,
                    carBrandId: pageData.selectCarData.currSelectBrand.CarBrandId,
                    carSeriesCateId: pageData.selectCarData.currSelectSeriesCate.CarSeriesCateId,
                    carSeriesId: pageData.selectCarData.currSelectSeries.CarSeriesId,
                    carModelId: pageData.selectCarData.currSelectCarModel.CarModelId,
                    serverType: pageData.server.ServerType,
                    shopType: pageData.server.ShopType,
                    cateId: pageData.server.Cate.AutoID,
                    serverName: pageData.server.ServerName,
                    workHours: pageData.server.WorkHours,
                    sallers: JSON.stringify(pageData.sallers),
                    defParts: JSON.stringify(pageData.defParts),
                    serverId: pageData.server.ServerId ? pageData.server.ServerId : ''
                };

                if (jsonData.serverName == '') {
                    alert('服务名称不能为空');
                    return;
                }

                commService.postData(
                    pageData.handlerUrl,
                    jsonData,
                    function (data) {

                        if (data.isSuccess) {
                            alert('保存成功');
                            if (pageData.currAction == 'EditServer') {
                                setTimeout(function () {
                                    window.location.href = "/Admin/CarServer/List.aspx";
                                }, 1200);
                            }

                        } else {
                            alert('保存失败');
                        }
                    }, function (data) {

                    });

            };

            pageFunc.init();

        }]);

    </script>

    <script type="text/ng-template" id="addServerParts.html">
        <form ng-submit="pageFunc.addParts()">
	        <div class="modal-header">
	            <h2 class="modal-title">关联服务配件</h2>
	        </div>
	        <div class="modal-body">
				<div class="form-group">
					<label>选择配件</label>
					<select class="form-control" ng-model="pageData.selectParts" ng-options="item as item.PartName for item in pageData.selectPartsSource" >
                        
                    </select>
				</div>
                <div class="form-group">
					<label>设置数量</label>
					<input type="number" class="form-control" ng-model="pageData.selectPartsCount" />
				</div>
                
                <div class="form-group">
                    <label>配件剩余数量</label><span class="text-info mLeft6">{{pageData.selectParts.Count}} - {{pageData.selectPartsCount}} = {{pageData.selectParts.Count - pageData.selectPartsCount}}</span>
                </div>

	        </div>
           
	        <div class="modal-footer txtCenter">
	            <button  type="submit" class="btn btn-primary" ng-click="confirm()">确定</button>
	            <a href="javascript:;" class="btn btn-warning"  ng-click="closeThisDialog()">取消</a>
	        </div>
        </form>
    </script>

    <script type="text/ng-template" id="tplAutocomplete">
     <%-- <div class="left-panel">
        <img ng-src="http://mbenford.github.io/ngTagsInput/images/flags/Australia.png" />
      </div>--%>
      <div class="right-panel">
        <span ng-bind-html="$highlight($getDisplayText())"></span>
        <%--<span>({{data.UserID}})</span>--%>
        <span>{{data.Company}}</span>
        <span>{{ data.Province + ' ' + data.City + ' ' + data.District + ' ' + data.Address }}<span>
      </div>
    </script>


</asp:Content>
