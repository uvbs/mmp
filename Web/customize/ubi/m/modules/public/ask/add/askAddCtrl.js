ubimodule.controller("askAddCtrl", ['$scope', 'commArticle', '$routeParams', 'commService', '$timeout', 'userService', 'ngDialog',
	function ($scope, commArticle, $routeParams, commService, $timeout, userService, ngDialog) {
	    var pageFunc = $scope.pageFunc = {};
	    var pageData = $scope.pageData = {
	        title: '问答发布 - ' + baseData.slogan,
	        questCate: baseData.moduleCateIds.askquestion, //问答-问题
	        articleCate: baseData.moduleCateIds.askarticle, //问答-文章
	        titleNm: '',//标题描述
	        contentDesc: '',//内容描述
	        provinceList: [],//所有省份列表
	        cityList: [{ id: "0", name: "请选择城市" }],//所有城市列表
	        province: null,//当前选中省份
	        city: null,//当前选中城市
	        tagList: [],//标签列表
	        currTagList: [],//选中标签列表
	        otherTagList: [],//清空时用
	        isHide: false,//选择完标签后隐藏

	        canInvitUsersList: [],//邀请人列表
	        currInvitedUserList: [],//当前选中邀请人
	        isInviteHide: false,//选择完邀请人后隐藏
	        currPath: base64encode('#/askArticle'),
	        currQuestionPath: base64encode('#/askqu'),
	        currUser: commService.getCurrUserInfo(),
	    };

	    document.title = pageData.title;

	    //获取城市
	    pageFunc.selectProvince = function () {
	        var CityChoose = { id: "0", name: "请选择所属城市" };
	        //获取城市
	        commService.getGetKeyVauleDatas({
	            type: 'city',
	            prekey: pageData.province.id
	        }, function (data) {
	            pageData.cityList = data.list;
	            pageData.cityList.unshift(CityChoose);
	            pageData.city = 0;
	            pageData.city = pageData.cityList[0];
	        }, function (data) { });
	    };

	    //获取标签
	    pageFunc.getTags = function () {
	        commArticle.getTags(function (data) {
	            if (data) {
	                for (var i = 0; i < data.length; i++) {
	                    data[i].checked = false;
	                }
	                pageData.tagList = data;
	            }

	        }, function (argument) {

	        });
	        console.log(pageData.tagList);
	    };
	    //标签选择弹框  
	    pageFunc.showTagsDialog = function () {
	        ngDialog.open({
	            template: basePath + 'modules/public/ask/add/tpls/tagsChooseDialog.html',
	            plain: false,
	            scope: $scope
	        });
	    };
	    //标签选择弹框取消 0为文章 1为问题
	    pageFunc.tagsCancel = function (type) {
	        if (type == 0) {
	            pageData.otherTagList = pageData.currTagList;
	            pageData.currTagList = [];//清除所选择的标签
	            for (var i = 0; i < pageData.otherTagList.length; i++) {
	                pageFunc.checkSelect(pageData.otherTagList[i], pageData.currTagList);
	            }
	        } else if (type == 1) {
	            pageData.otherTagList = pageData.currInvitedUserList;
	            pageData.currInvitedUserList = [];//清除所选择的邀请人
	            for (var i = 0; i < pageData.otherTagList.length; i++) {
	                pageFunc.checkSelect(pageData.otherTagList[i], pageData.currInvitedUserList);
	            }
	        }

	    };
	    //标签选择确定 0为文章 1为问题
	    pageFunc.tagsConfirm = function (type) {
	        switch (type) {
	            case 0:
	                pageData.isHide = true;
	                break;
	            case 1:
	                pageData.isInviteHide = true;
	                break;
	            default:
	                break;
	        }
	        ngDialog.closeAll();
	    }
	    //单个标签点击 list为存储当前所选的对象  type值为0时为标签弹框 1为邀请人弹框
	    pageFunc.tagClick = function (item, list, type) {
	        if (list) {
	            if (list.length > 0) {
	                var isHave = false;//判断是否有
	                for (var i = 0; i < list.length; i++) {
	                    if (type == 0) {
	                        if (item.id == list[i].id) {
	                            list.splice(i, 1);
	                            isHave = true;
	                        }
	                    }
	                    else if (type == 1) {
	                        if (item.userId == list[i].userId) {
	                            list.splice(i, 1);
	                            isHave = true;
	                        }
	                    }
	                }
	                if (!isHave) {
	                    if (list.length < 5)
	                    {
	                        list.push(item);
	                    }
	                    else
	                    {
	                        alert("最多只能选5个！")
	                        return;
	                    }
	                }
	            } else {
	                list.push(item);
	            }
	        }
	        console.log(list);
	    }
	    //判断是否选中，选中换背景颜色 type值为0时为标签弹框 1为邀请人弹框
	    pageFunc.checkSelect = function (item, list, type) {
	        if (type == 0) {
	            for (var i = 0; i < list.length; i++) {
	                if (item.id == list[i].id) return true;
	            }
	            return false;
	        }
	        else if (type == 1) {
	            for (var i = 0; i < list.length; i++) {
	                if (item.userId == list[i].userId) return true;
	            }
	            return false;
	        }

	    }

	    //获取邀请人
	    pageFunc.getCanInvitUsers = function () {
	        //判断当前是否已登陆
	        if (!pageData.currUser) {
	            $scope.go('#/login/' + pageData.currQuestionPath);
	        } else {
	            userService.getCanInvitUsers({
	                pageIndex: 1,
	                pageSize: 5000,
	                keyword: ""
	            },
              function (data) {
                  if (data) {
                      pageData.canInvitUsersList = data.list;
                  }
              },
              function () {
              });
	        }

	        console.log(pageData.canInvitUsersList);
	    }
	    //邀请人选择弹框  
	    pageFunc.showUserInviteDialog = function () {
	        // pageFunc.getCanInvitUsers();
	        ngDialog.open({
	            template: basePath + 'modules/public/ask/add/tpls/userInviteDialog.html',
	            plain: false,
	            scope: $scope
	        });
	    };

	    //清除选中标签  type 值0为标签弹框 1为邀请人弹框
	    pageFunc.deleteTags = function (item, list, type) {
	        if (list) {
	            for (var i = 0; i < list.length; i++) {
	                if (type == 0) {
	                    if (item.id == list[i].id) {
	                        list.splice(i, 1);
	                    }
	                } else if (type == 1) {
	                    if (item.userId == list[i].userId) {
	                        list.splice(i, 1);
	                    }
	                }

	            }
	            if (list.length == 0) {
	                switch (type) {
	                    case 0:
	                        pageData.isHide = false;
	                        break;
	                    case 1:
	                        pageData.isInviteHide = false;
	                        break;
	                    default:
	                        break;
	                }

	            }
	        }

	    }
	    //发布  type 值0为文章发布 1为问题发布
	    pageFunc.publish = function (type) {
	        var selectedTags = [];//选择的标签
	        var selectedUsers = [];//选择的邀请人
	        if (pageData.titleNm == "")
	        {
	            alert("标题不能为空");
	            return;
	        }
	        if (pageData.contentDesc == "")
	        {
	            alert("内容不能为空");
	            return;
	        }
	        if (pageData.currTagList)
	        {
	            for (var i = 0; i < pageData.currTagList.length; i++) {
	                selectedTags.push(pageData.currTagList[i].tag);
	            }
	        }
	        var reqData = [];
	        var path = "";
	        switch (type) {
	            case 0:
	                path = pageData.currPath;
	                reqData = {
	                    action: 'addArticle',
	                    type: 'Article',
	                    cateId: pageData.articleCate,
	                    province: pageData.province,
	                    city: pageData.city,
	                    tag: selectedTags.join(','),
	                    title: pageData.titleNm,
	                    content: pageData.contentDesc,
	                };
	                break;
	            case 1:
	                if (pageData.currInvitedUserList)
	                {
	                    for (var i = 0; i < pageData.currInvitedUserList.length; i++) {
	                        selectedUsers.push(pageData.currInvitedUserList[i].userId);
	                    }
	                }
	                path = pageData.currQuestionPath;
	                reqData = {
	                    action: 'addArticle',
	                    type: 'Question',
	                    cateId: pageData.questCate,
	                    province: pageData.province,
	                    city: pageData.city,
	                    tag: selectedTags.join(','),
	                    title: pageData.titleNm,
	                    content: pageData.contentDesc,
	                    receivers: selectedUsers.join(',')
	                };
	                break;
	            default:
	                break;
	        }


	        //判断当前是否已登陆
	        if (!pageData.currUser) {
	            $scope.go('#/login/' + path);
	        } else {
	            commService.postData(baseData.handlerUrl, reqData, function (data) {
	                if (data.isSuccess) {
	                    alert('发布成功');
	                    pageFunc.reset();
	                    $scope.go('#/ask');
	                }
	                else {
	                    if (data.errmsg != null) {
	                        alert('发布失败,' + data.errmsg);
	                    }
	                    else {
	                        alert('发布失败');
	                    }
	                }
	            }, function (data) {
	                // body...
	            });
	        }
	    };
	    //重置   type 值0为文章重置 1为问题重置
	    pageFunc.reset = function (type) {
	        pageData.titleNm = "";
	        pageData.contentDesc = "";
	        pageData.province = "";
	        pageData.city = "";
	        pageData.currTagList = [];
	        if (type == 1) {
	            pageData.currInvitedUserList = [];
	        }
	    };
	    //初始化
	    pageFunc.init = function () {
	        var ProChoose = { id: "0", name: "请选择省份" };
	        pageData.city = pageData.cityList[0];
	        //获取省份
	        commService.getGetKeyVauleDatas({
	            type: 'province'
	        }, function (data) {
	            pageData.provinceList = data.list;
	            pageData.provinceList.unshift(ProChoose);
	            pageData.province = pageData.provinceList[0];
	            console.log(data);
	        }, function () { });
	        pageFunc.getTags();
	        pageFunc.getCanInvitUsers();
	    };
	    pageFunc.init();

	}]);