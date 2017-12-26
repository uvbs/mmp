

comeonModule.directive('carmodelselect', ['commService', function (commService) {
    return {
        restrict: 'ECMA',
        templateUrl: '/static-modules/app/admin/tpls/carModelSelect.html',
        replace: true,
        scope: {
            selectdata: '=',
            initbrandid: '@',
            initseriescateid: '@',
            initseriesid: '@',
            initmodelid: '@',
            disabled: '@',
            hidemodel: '@',
            ismultiseries: '@',
            ismultimodel:'@'
        },
        controller: function ($scope, $element, $attrs) {
            //selectDatacurrSelectBrand currSelectSeriesCate currSelectSeries currSelectModel
            var pageData = $scope.pageData = {
                brandList: [],
                seriesCateList: [],
                seriesList: [],
                modelList: [],
                //currSelectBrand:null,
                //currSelectSeriesCate: null,
                //currSelectSeries: null,
                //currSelectModel: null,
                initBrandId: $scope.initbrandid,
                initSeriesCateId: $scope.initseriescateid,
                initSeriesId: $scope.initseriesid,
                initModelId:$scope.initmodelid,
                handlerUrl: '/Handler/App/CarServiceHandler.ashx'
            };
            
            var pageFunc = $scope.pageFunc = {};

            pageFunc.init = function () {
                pageFunc.loadBrand();
            };

            pageFunc.loadBrand = function () {
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'QueryBrand',
                    pageSize: 10000,
                    pageIndex: 1,
                    firstLetter: '',
                    isMustInWebsite: 1
                }, function (data) {
                    pageData.brandList = data.dataList;
                    pageData.seriesCateList = null;
                    pageData.seriesList = null;
                    pageData.modelCateList = null;
                    $scope.selectdata = {};
                    if (pageData.initBrandId > 0) {
                        for (var i = 0; i < pageData.brandList.length; i++) {
                            if(pageData.initBrandId == pageData.brandList[i].CarBrandId)
                            {
                                $scope.selectdata.currSelectBrand = pageData.brandList[i];
                                pageFunc.loadSeriesCate();
                                break;
                            }
                        }
                        pageData.initBrandId = 0;
                    }

                }, function (data) {

                });
            };

            pageFunc.loadSeriesCate = function () {
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'GetSeriesCateList',
                    brandId: $scope.selectdata.currSelectBrand.CarBrandId
                }, function (data) {
                    pageData.seriesCateList = data;
                    pageData.seriesList = null;
                    pageData.modelCateList = null;

                    if (pageData.initSeriesCateId > 0) {
                        for (var i = 0; i < pageData.seriesCateList.length; i++) {
                            if (pageData.initSeriesCateId == pageData.seriesCateList[i].CarSeriesCateId) {
                                $scope.selectdata.currSelectSeriesCate = pageData.seriesCateList[i];
                                pageFunc.loadSeries();
                                break;
                            }
                        }
                        pageData.initSeriesCateId = 0;
                    }

                }, function (data) {

                });
            };

            pageFunc.loadSeries = function () {
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'GetSeriesList',
                    cateId: $scope.selectdata.currSelectSeriesCate.CarSeriesCateId,
                    brandId: $scope.selectdata.currSelectBrand.CarBrandId
                }, function (data) {
                    pageData.seriesList = data;
                    pageData.modelCateList = null;

                    if (pageData.initSeriesId > 0) {
                        for (var i = 0; i < pageData.seriesList.length; i++) {
                            if (pageData.initSeriesId == pageData.seriesList[i].CarSeriesId) {
                                $scope.selectdata.currSelectSeries = pageData.seriesList[i];
                                pageFunc.loadModel();
                                break;
                            }
                        }
                        pageData.initSeriesId = 0;
                    }

                }, function (data) {

                });
            };

            //pageFunc.loadModelCate = function () {
            //    commService.loadRemoteData(pageData.handlerUrl, {
            //        action: 'GetModelCateList',
            //        seriesId: $scope.selectdata.currSelectSeries.CarSeriesId
            //    }, function (data) {
            //        pageData.modelCateList = data;
            //    }, function (data) {

            //    });
            //};

            pageFunc.loadModel = function () {
                
                commService.loadRemoteData(pageData.handlerUrl, {
                    action: 'GetModelList',
                    modelCateId: 1,
                    seriesId: $scope.selectdata.currSelectSeries ? $scope.selectdata.currSelectSeries.CarSeriesId : 0,
                    pageSize: 100000,
                    pageIndex: 1
                }, function (data) {
                    
                    pageData.modelList = data.list;

                    if (pageData.initModelId > 0) {
                        for (var i = 0; i < pageData.modelList.length; i++) {
                            if (pageData.initModelId == pageData.modelList[i].CarModelId) {
                                $scope.selectdata.currSelectCarModel = pageData.modelList[i];
                                $scope.selectdata.currSelectMultiModel = pageData.modelList[i];
                                break;
                            }
                        }
                        pageData.initModelId = 0;
                    }

                }, function (data) {
                    
                });
            };


            pageFunc.init();

        }
    };
}]);


