

var pager1 = null;
var bs_keys = { pageIndex: 1, pageSize: 10, type: 'Cognizance', column: 'JuActivityID,ActivityName,Summary,UserID,CreateDate,CommentCount,PraiseCount,RewardTotal', order_all: 'Sort desc, JuActivityID desc', total: 1, list: [] };
var bs_keys2 = { pageIndex: 1, pageSize: 10, author: u_id, type: 'Cognizance', column: 'JuActivityID,ActivityName,Summary,UserID,CreateDate,CommentCount,PraiseCount,RewardTotal', order_all: 'Sort desc, JuActivityID desc', total: 1, list: [] };
var bs_cognizance_item;
$(function () {
    setHeadSelected(5);

    bs_cognizance_item = $('.index-comment-item').clone();
    $('.index-comment').html('');

    data.cognizance = $.extend({}, bs_keys);
    loadCognizance(data.cognizance, true, true);


    $('#REMEN').click(function () {
        var sthis = $(this);
        checkLogin(function (result) {
            if (result.is_login) {
                $(sthis).addClass('key1');
                $('#XINFABU').removeClass('key1');
                $('#XINFABU').addClass('key3');
                $('.index-comment').html('');
                data.cognizance = $.extend({}, bs_keys2);
                loadCognizance(data.cognizance, true, true);
            } else {
                showLoginDialog(function (data) {
                    u_id = data.id;
                    bs_keys2.author = data.id;
                    $(sthis).addClass('key1');
                    $('#XINFABU').removeClass('key1');
                    $('#XINFABU').addClass('key3');
                    $('.index-comment').html('');
                    data.cognizance = $.extend({}, bs_keys2);
                    loadCognizance(data.cognizance, true, true);
                });
            }
        });



    });

    //单击新发布排序
    $('#XINFABU').click(function () {
        $(this).addClass('key1');
        $('#REMEN').removeClass('key1');
        $('#REMEN').addClass('key3');
        $('.index-comment').html('');
        data.cognizance = $.extend({}, bs_keys);
        loadCognizance(data.cognizance, true, true);
    });


    $('.warp-button button').click(function () {
        checkLogin(function (result) {
            if (result.is_login) {
                window.location.href = '/customize/StockPlayer/Src/Cognizance/Add.aspx';
            } else {
                showLoginDialog(function () {
                    window.location.href = '/customize/StockPlayer/Src/Cognizance/Add.aspx';
                });
            }
        });
    });


});

function loadCognizance(obData, showProgress, bindPage) {
    GetContent(
       obData,
       showProgress,
       function (result) {
           obData.total = result.totalcount;
           obData["page" + obData.pageIndex] = result.list;
           if (bindPage) {
               $('.pager').html('');
               if (obData.total > obData.pageSize) {
                   $.pager({
                       count: obData.total, nums: obData.pageSize, numsOption: [obData.pageSize],
                       pageContainer: '.pager', onchange: function () {
                           data.cognizance.pageIndex = this.page;
                           $('.index-comment').html('');
                           var obData = data.cognizance;
                           var list = obData['page' + this.page];
                           if (!list) {
                               loadCognizance(obData, true, false, false);
                           } else {
                               viewCognizance(list, obData.pageIndex);
                           }
                       }
                   });
               }
           }
           viewCognizance(result.list, obData.pageIndex);
       });
}


function viewCognizance(list, page) {
    var listHtml = new StringBuilder();
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(bs_cognizance_item).clone();

            newItem.find('.content').text(list[i].summary);
            newItem.find('.time').text(dateFormat(list[i].createDate));

            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);



            newItem.find('.content').attr('data-id', list[i].id).click(function () {
                var id = $(this).attr('data-id');
                ToDetail(id);
            });


            if (!!list[i].pubUser) {
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
                newItem.find('.company-name,.username').text(list[i].pubUser.userName);
                newItem.find('.userimg,.head-img,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.head-img,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.head-img,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg,.head-img,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.head-img,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.head-img,.username').attr('data-describe', list[i].pubUser.describe);
            } else {
                newItem.find('.company-name,.username').text('淘股玩家');
                newItem.find('.userimg img').attr('src', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.head-img,.username').attr('data-id', 0);
                newItem.find('.userimg,.head-img,.username').attr('data-nickname', '淘股玩家');
                newItem.find('.userimg,.head-img,.username').attr('data-friend', 1);
                newItem.find('.userimg,.head-img,.username').attr('data-times', 0);
                newItem.find('.userimg,.head-img,.username').attr('data-avatar', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.head-img,.username').attr('data-describe', '');
            }
            newItem.show();
            $('.index-comment').append(newItem);
        }



    } else {
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.index-comment').append(listHtml.ToString());
    }
}