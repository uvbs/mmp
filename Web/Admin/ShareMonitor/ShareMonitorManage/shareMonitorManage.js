var shareMonitorManage = {},
    pageData = {
        handlerUrl: '/Admin/ShareMonitor/Handler.ashx'
    },
    $document = $(document);

shareMonitorManage.init = function() {
    $('#grvData').datagrid({
        method: "Post",
        url: pageData.handlerUrl,
        queryParams: {
            Action: "QueryShareMonitor"
        },
        height: document.documentElement.clientHeight - 170,
        pagination: true,
        striped: true,
        
        rownumbers: true,
        singleSelect: false,
        rowStyler: function() {
            return 'height:25px';
        },
        columns: [
            [{
                title: 'ck',
                width: 5,
                checkbox: true
            }, {
                field: 'MonitorId',
                title: '编号',
                width: 5,
                align: 'left',
                formatter: FormatterTitle
            }, {
                field: 'MonitorName',
                title: '名称',
                width: 20,
                align: 'left'
            }, {
                field: 'MonitorUrl',
                title: '链接',
                width: 50,
                align: 'left'
            }, {
                field: 'ShareCount',
                title: '分享',
                width: 20,
                align: 'left',
                formatter: function(value,row,index){
                    var str = '<a href="/Admin/ShareMonitor/ShareTree/tree.html?mid=';
                    str += row['MonitorId'];
                    str += '" ';
                    if (parseInt(value) > 0) {
                        str += ' style="color:green" ';
                    }
                    str += ' > ';
                    str += value;
                    str += "</a>";

                    console.log(str);
                    return str;
                }
            }, {
                field: 'ReadCount',
                title: '阅读',
                width: 20,
                align: 'left'
            }, {
                field: 'CreateTime',
                title: '创建时间',
                width: 20,
                align: 'left',
                formatter: FormatDate
            }]
        ]
    });
    shareMonitorManage.bindEvent();
}

shareMonitorManage.bindEvent = function() {


    var $txtName = $('.warpAddDiv .txtName'),
        $txtUrl = $('.warpAddDiv .txtUrl');

    $document.on('click', '#btnAdd', function() {

        $txtName.val('');
        $txtUrl.val('');

        var tagDiv = layer.open({
            type: 1,
            shade: [0.2, '#000'],
            shadeClose: false,
            area: ['580', '250'],
            title: ['新建监测', 'background:#1B9AF7; color:#fff;'],
            border: [0],
            content: $('.warpAddDiv')
        });
    });

    $document.on('click', '#btnDeleteBatch', function() {
        shareMonitorManage.deleteBatch();
    });
    
    $document.on('click', '.warpAddDiv .warpOpeate .btnSave', function() {



        var reqData = {
            action: 'AddShareMonitor',
            name: $.trim($txtName.val()),
            url: $.trim($txtUrl.val())
        };

        if (reqData.name == '') {
            alert('名称不能为空', 3);
            $txtName.focus();
            return;
        }

        if (reqData.url == '') {
            alert('链接不能为空', 3);
            $txtUrl.focus();
            return;
        }

        if (!IsURL(reqData.url)) {
            alert('请输入正确的链接格式地址', 3);
            $txtUrl.focus();
            return;
        }

        $.ajax({
            url: pageData.handlerUrl,
            type: 'post',
            data: reqData,
            success: function(data) {
                if (data.isSuccess) {
                    $('#grvData').datagrid('reload');
                    layer.closeAll();
                } else {
                    alert('添加失败:' + data.errmsg, 2);
                }
            }
        });
    });

    $document.on('click', '.warpAddDiv .warpOpeate .btnCancel', function() {

        layer.closeAll();
    });
    $document.on('click', '#btnSearch', function () {

        shareMonitorManage.search();
    });

};


shareMonitorManage.deleteBatch = function() {
    var rows = $('#grvData').datagrid('getSelections');
    if (!EGCheckIsSelect(rows))
        return;

    $.messager.confirm("系统提示", "确认删除选中数据?", function(r) {
        if (r) {

            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].MonitorId);
            }

            var dataModel = {
                Action: 'DeleteShareMonitor',
                ids: ids.join(',')
            }

            $.ajax({
                type: 'post',
                url: pageData.handlerUrl,
                data: dataModel,
                success: function(resp) {
                    alert('成功删除数据:' + resp + '条');
                    $('#grvData').datagrid('reload');
                }
            });

        }
    });
};

//搜索
shareMonitorManage.search = function () {
    
    $('#grvData').datagrid(
       {
           method: "Post",
           url: pageData.handlerUrl,
           queryParams: {
               Action: "QueryShareMonitor",
               KeyWord: $(txtKeyWord).val()
           },
       });


};