wisdomlifemodule.controller('rankCtrl', ['$scope', 'commService','$routeParams', function ($scope, commService,$routeParams) {
    var pageData = $scope.pageData= {
        title: '英雄榜',
        isDrop: false,
        isMediaDrop:false,//自媒体点击是否出现下拉选项
        isSelected: 1,
        isDropSelected: '',
        isMediaDropSelected:'',//自媒体下拉选项是否选中
        cateId: '',
        id:$routeParams.id,
        advertiser: 
            {
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0
                //cateId:
            }
        ,
        selfmedia: 
            {
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount:0
            }
        ,
        currSelected: [],
        dataList: [],
        keyword: "",
        rankByAdv:'日榜排行',
        rankByMedia:'日榜排行'
    };

    var pageFunc = $scope.pageFunc = {};

    document.title = pageData.title;

    pageFunc.dropClick = function () {
        pageData.isDrop = true;
    };

    //加载广告主列表数据
    pageFunc.loadData = function (isNew, type, item,isNull,orderby) {
        if (type == 2)
        {
            pageData.isDrop = false;
        }else if(type==1)
        {
            pageData.isMediaDrop=false;
        }
        pageData.currSelected = item;
        if (type == 1)
        {
            pageData.cateId = baseData.moduleCateIds.advertiser;
        }
        else if(type==2)
        {
            pageData.cateId = baseData.moduleCateIds.selfmedia;
        }
        if (isNew) {
            //pageData.dataList = [];
            //pageData.pageIndex = 1;
            item.list = [];
            item.pageIndex = 1;
        }
        else {
            // pageData.pageIndex++;
            item.pageIndex++;
        }
        var model = {
            cateId: pageData.cateId,
            keyword: pageData.keyword,
            pageIndex: item.pageIndex,
            pageSize: item.pageSize,
            orderby: orderby
        }
        commService.getArticleListByOption(model, function (data) {
            if (isNull != '')
            {
                pageData.isDrop = false;
                pageData.isMediaDrop=false;
                if(type==1)
                {
                    if(isNull==1)
                    {
                        pageData.rankByAdv='日榜排行';
                    }
                    else if(isNull==2)
                    {
                        pageData.rankByAdv='周榜排行';
                    }
                    else if(isNull==3)
                    {
                        pageData.rankByAdv='月榜排行';
                    }
                }
                else if(type==2)
                {
                    if(isNull==1)
                    {
                        pageData.rankByMedia='日榜排行';
                    }
                    else if(isNull==2)
                    {
                        pageData.rankByMedia='周榜排行';
                    }
                    else if(isNull==3)
                    {
                        pageData.rankByMedia='月榜排行';
                    }
                }

            }
            
            item.totalCount = data.totalcount;
            for (var i = 0; i < data.list.length; i++) {
                item.list.push(data.list[i]);
            }
            pageData.dataList = data.list;
        }, function (data) {
        });
    }
    pageFunc.loadMore = function () {
        pageFunc.loadData(false, pageData.isSelected, pageData.currSelected,'','');
    };

    pageFunc.init = function () {
        if(pageData.id!=undefined)
        {
            if (pageData.id==1)
            {
                pageFunc.loadData(true, 1, pageData.advertiser, '', 'K5 desc');
                pageData.isSelected=1;
            }
            else if (pageData.id==2)
            {
                pageFunc.loadData(true, 2, pageData.selfmedia, 1, 'K5 desc');
                pageData.isSelected=2;
            }
        }
        else
        {
            pageFunc.loadData(true, pageData.isSelected, pageData.advertiser, '', 'K5 desc');
        }

      //  pageFunc.loadData(true, pageData.isSelected, pageData.advertiser,'','');
    };
    pageFunc.init();

}]);