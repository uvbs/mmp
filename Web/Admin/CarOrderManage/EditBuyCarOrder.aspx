<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AngularAdmin.Master" AutoEventWireup="true" CodeBehind="EditBuyCarOrder.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CarOrderManage.EditBuyCarOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="wrapSeries" ng-controller="AddServerCtrl" style="padding-bottom: 124px;">
        <ol class="breadcrumb">
            <li class="">报价单管理</li>
            <li class="active">{{pageData.title}}</li>
		</ol>
        <form class="form-horizontal" style=" padding-right:50px;" ng-submit="pageFunc.save()">
          
        <fieldset>
        <legend class="txtCenter pLeft50">购车需求</legend>

            <div class="form-group">
            <label class="col-sm-2 control-label">姓名</label>
            <div class="col-sm-10">
              <input type="text" class="form-control" ng-model="pageData.parts.CarOwnerName" placeholder="姓名" required readonly="true">
            </div>
          </div>
          <div class="form-group">
            <label class="col-sm-2 control-label">手机</label>
            <div class="col-sm-10">
              <input type="text" class="form-control"  ng-model="pageData.parts.CarOwnerPhone" placeholder="手机" required readonly="true">
            </div>
          </div>

            <carmodelselect 
            ng-attr-selectdata="pageData.selectData"
            ng-attr-initbrandid="{{pageData.initBrandId}}"
            ng-attr-initseriescateid="{{pageData.initSeriesCateId}}"
            ng-attr-initseriesid="{{pageData.initSeriesId}}"
            ng-attr-initmodelid="{{pageData.initModelId}}"
            ng-attr-disabled="{{pageData.isEdit}}"
            ></carmodelselect>

            <div class="form-group">
                <label class="col-sm-2 control-label">颜色</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.CarColor" placeholder="颜色" required readonly="true">
                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">购车时间</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.BuyTime" placeholder="颜色" required readonly="true">
                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">购车方式</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.PurchaseWay" placeholder="颜色" required readonly="true">
                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">牌照</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.LicensePlate" placeholder="颜色" required readonly="true">
                </div>
            </div>

            <div class="form-group">
                <label class="col-sm-2 control-label">地区</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.District" placeholder="颜色" required readonly="true">
                    <input type="text" class="form-control" ng-model="pageData.parts.Area" placeholder="颜色" required readonly="true">
                </div>
            </div>

            
            <div class="form-group">
                <label class="col-sm-2 control-label">购车偏好</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.Preference" placeholder="购车偏好" required readonly="true">
                </div>
            </div>







        </fieldset>


            <fieldset>
            <legend class="txtCenter pLeft50">报价单</legend>

            <div class="form-group">
                <label class="col-sm-2 control-label">指导价格</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.GuidePrice" placeholder="指导价格" required>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">库存状态</label>
                <div class="col-sm-10">
                   <select class="form-control" ng-model="pageData.parts.StockDescription">
                       <option value="充足">充足</option>
                       <option value="紧张">紧张</option>
                       <option value="缺货">缺货</option>
                   </select>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">全国销量</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.NationalSalesCount" placeholder="" required>
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">环比增幅</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.Increase" placeholder="" required>
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">裸车优惠价</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.DiscountPrice" placeholder="" required>
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">是否需要店内保险</label>
                <div class="col-sm-10">
                  <input type="checkbox" ng-model="pageData.parts.IsShopInsurance">
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">上牌费</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.LicensingFees" placeholder="" required>
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">其他费用</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.OtherExpenses" placeholder="" required>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">保险预估</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.InsuranceCost" placeholder="" required>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">购置税预估</label>
                <div class="col-sm-10">
                  <input type="number" class="form-control" ng-model="pageData.parts.PurchaseTaxCost" placeholder="" required>
                </div>
            </div>
            <div class="form-group">
                <label class="col-sm-2 control-label">备注信息</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.SallerMemo" placeholder="" required>
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">合计落地价</label>
                <div class="col-sm-10">
                  {{pageData.parts.PurchaseTaxCost + pageData.parts.InsuranceCost + pageData.parts.OtherExpenses + pageData.parts.LicensingFees + pageData.parts.DiscountPrice}}
                </div>
            </div>
                <div class="form-group">
                <label class="col-sm-2 control-label">关联活动</label>
                <div class="col-sm-10">
                  <input type="text" class="form-control" ng-model="pageData.parts.ActivityId" placeholder="请输入关联活动id" required>
                </div>
            </div>
            </fieldset>


           <%-- <div class="form-group">
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
            --%>
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
                    title: '编辑报价单',
                    handlerUrl: cc.handler.car,
                    parts: {},
                    selectData: {},
                    currAction: 'EditCarQuotation',
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

                //var urlAction = GetParm('action');

                //if (urlAction == 'edit') {
                    pageData.isEdit = 1;
                    pageData.title = "编辑报价单";
                    //pageData.editId = GetParm('id');
                    pageData.parts = JSON.parse(sessionStorage.getItem("EditBuyCarOrder"));

                    pageData.initBrandId = pageData.parts.CarBrandId;
                    pageData.initSeriesCateId = pageData.parts.CarSeriesCateId;
                    pageData.initSeriesId = pageData.parts.CarSeriesId;
                    pageData.initModelId = pageData.parts.CarModelId;
                    
                    pageData.parts.IsShopInsurance = pageData.parts.IsShopInsurance == 1 ? true : false;

                    console.log('EditBuyCarOrder', pageData.parts);
                //}

            };

            pageFunc.save = function () {

                console.log('selectData', pageData.selectData);

                pageData.parts.IsShopInsurance = pageData.parts.IsShopInsurance ? 1 : 0;
                
                pageData.parts.TotalPrice = pageData.parts.PurchaseTaxCost + pageData.parts.InsuranceCost + pageData.parts.OtherExpenses + pageData.parts.LicensingFees + pageData.parts.DiscountPrice;

                var jsonData = {
                    action: 'EditCarQuotation',
                    data:JSON.stringify(pageData.parts)
                };
                
                commService.postData(
                    pageData.handlerUrl,
                    jsonData,
                    function (data) {
                        
                        if (data.isSuccess) {
                            alert('保存成功');
                          
                                setTimeout(function () {
                                    window.location.href = "/Admin/CarOrderManage/BuyCarOrderList.aspx";
                                },1200);
                            

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

