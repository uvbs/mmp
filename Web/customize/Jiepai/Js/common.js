var handlerPath = "Handler.ashx";
function layermsg(msg) {
    layer.open({
        content: msg,
        btn: ['OK']
    });
}

function layermsgCon(title) {
    layer.open({
        title: [
            title,
            'background-color:white;'
        ],
        content: '<div id="ok">OK</div>'

    });
}