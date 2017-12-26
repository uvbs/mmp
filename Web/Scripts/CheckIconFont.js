function CheckIconFont() {
    var newNode = document.createElement("i");
    newNode.setAttribute('id', 'chkIconFont');
    newNode.setAttribute('class', 'iconfont icon-gougou');
    newNode.setAttribute('style', 'font-size: 50px;display: inline-block;position:relative;top: -1000px;');
    document.body.appendChild(newNode);
    var chkIconFont = document.getElementById('chkIconFont');
    //console.log(chkIconFont.clientHeight);
    setTimeout(function () {
        var chkIconFont = document.getElementById('chkIconFont');
        if (chkIconFont.clientHeight > 100) {
            var newStyleNode = document.createElement("style");
            newStyleNode.setAttribute('type', 'text/css');
            newStyleNode.innerHTML = '.iconfont{position:relative;display:inline-block;} .iconfont:before{position:relative;top:-326%;display:inline-block;}';
            document.head.appendChild(newStyleNode);
        }
        document.body.removeChild(chkIconFont);
    });
};
CheckIconFont();