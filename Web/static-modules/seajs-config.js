seajs.config({
    base: "/static-modules/",
    alias: {
        "alert": "lib/alert/alert"
    },
    map: [[/^(.*\.(?:css|js))(.*)$/i, '$1?201405011']]
});