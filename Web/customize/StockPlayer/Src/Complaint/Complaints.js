
var ue;
$(function () {
    setHeadSelected(7);

    ue = UE.getEditor('suggestion', {
        initialFrameWidth: 560,
        initialFrameHeight: 320,
        scaleEnabled: true,
        autoFloatEnabled: false,
        toolbars: ueditorToobars
    });

    $('.lbTip').click(function () {
        var msg = $(this).attr('data-tip-msg');
        layer.tips(msg, '.lbTip');
    });
    
    $('.btn-add').click(function () {
        var model = {
            type: 'Complaint',
            title: $.trim($('#title').val()),
            content: ue.getContent(),
            summary: '',
            action: 'AddArticle',
        };
        if ($.trim(model.title) == '') {
            alert('请输入标题');
            return;
        }
     
        if ($.trim(model.content) == '') {
            alert('请输入内容');
            return;
        }
      
        var url = '/Serv/PubApi.ashx';
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: url,
            data: model,
            dataType: 'JSON',
            success: function (resp) {
                layer.close(layerIndex);
                if (resp.isSuccess) {
                    alert('你的建议或投诉已提交给后台');
                    $('#title').val('');
                    ue.setContent('');
                } else if (resp.errcode == 10010) {
                    alert('请先登录');
                } else {
                    alert('提交出错');
                }
            }
        });
    });
    //重置
    $('.btn-reset').click(function () {
        $('#title').val('');
        ue.setContent('');
    });

});