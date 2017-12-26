function pagegoback(url) {
    url = url ? url : "/Wubuhui/MyCenter/Index.aspx";
    if (document.location.search.match("isappinstalled")) {
        window.location.href = url;
    } else {
        window.history.go(-1);
    }
}