

var scoreItem;


$(function () {
    setHeadSelected(4);
    data.score = { rows: 10, page: 1, list: [], total: 1,all_score:'1' };
    scoreItem = $('.list-notice .notice-item').clone();
    $('.list-notice').html('');
    loadScoreDetails(data.score,true,true);
});


function GetScoreDetail(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        rows: 10,
        page: 1
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/Score/List.ashx",
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



function loadScoreDetails(obData, showProgress, bindPage) {
    GetScoreDetail(
        obData,
        showProgress,
        function (result) {
            obData.total = result.total;
            obData["page" + obData.page] = result.list;
            if (bindPage) {
                if (obData.total > obData.rows) {
                    $.pager({
                        count: obData.total, nums: obData.rows, numsOption: [obData.rows],
                        page: obData.page,
                        pageContainer: '.pager1', onchange: function () {
                            data.score.page = this.page;
                            $('.list-notice').html('');
                            var obData = data.score;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadScoreDetails(obData, true, false);
                            } else {
                                viewScore(list);
                            }
                        }
                    });
                }
            }
            viewScore(result.list);
        });
}




function viewScore(list) {
    console.log(list);
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(scoreItem).clone();
            newItem.find('.head .time').text(dateFormat(list[i].time));
            newItem.find('.content').append(list[i].addnote);
            newItem.find('.content').append(',余额'+list[i].totalscore);
            newItem.show();
            $('.list-notice').append(newItem);
        }
    }
}