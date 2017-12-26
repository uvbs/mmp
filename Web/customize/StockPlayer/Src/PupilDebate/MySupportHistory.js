
var supportItem
$(function () {
    setHeadSelected(4);

    data.support = { pageSize: 10, pageIndex: 1, type: 'PupilDebate',author:author,column: 'JuActivityID,ActivityName,Summary,CreateDate', list: [], total: 1 };

    supportItem = $('.list-notice .notice-item').clone();

    $('.list-notice').html('');

    if (author > 0) {
        loadSupport(data.support, true, true);
    } else {
        //登录验证
        tt.vf.req.addId('user_acount', 'user_pwd');
        $('#user_acount,#user_pwd').keypress(function () {
            var theEvent = window.event || arguments.callee.caller.arguments[0];
            if (theEvent.keyCode == 13) {
                $('.btn-login').click();
            }
        });
        $('.btn-login').click(function () {
            if (!tt.validateId('user_acount', 'user_pwd')) {   //userRegister是form的name  
                return false;
            }
            var username = $.trim($('#user_acount').val());
            var userpassword = $.trim($('#user_pwd').val());
            loginApi(username, userpassword);
        });
    }
   
});

function GetSupport(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        pageSize: 10,
        pageIndex: 1,
        type: 'PupilDebate'
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/Article/GetArticleList.ashx",
        data: option,
        dataType: "json",
        success: function (resp) {
            if (showProgress) layer.close(layerIndex);
            if (resp.status) {
                fn(resp.result);
            } else {
                alert('查询出错');
            }
        }
    });
}
function loadSupport(obData, showProgress, bindPage) {
    GetSupport(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData["page" + obData.pageIndex] = result.list;
            if (bindPage) {
                $('.pager1').html('');
                if (obData.total > obData.pageSize) {
                    $.pager({
                        count: obData.total, nums: obData.pageSize, numsOption: [obData.pageSize],
                        page: obData.pageIndex,
                        pageContainer: '.pager1', onchange: function () {
                            data.support.pageIndex = this.page;
                            $('.list-notice').html('');
                            var obData = data.support;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadSupport(obData, true, false,true);
                            } else {
                                viewSupport(list);
                            }
                        }
                    });
                }
            }
            viewSupport(result.list);
        });
}
function viewSupport(list) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(supportItem).clone();
            newItem.find('.head .time').text(dateFormat(list[i].createDate));
            newItem.find('.head').append('<div class="title">' + list[i].title + '</div>');
            newItem.find('.title').attr('data-id', list[i].id).click(function () {
                var id = $(this).attr('data-id');
                ToDetail(id);
            });
            newItem.find('.content').append(list[i].summary);
            newItem.find('.content').attr('data-id', list[i].id).click(function () {
                var id = $(this).attr('data-id');
                ToDetail(id);
            });
            newItem.show(); 
            $('.list-notice').append(newItem);
        }
    }
}