/*
　 *　方法:Array.Contains(e)
　 *　功能:程序是否存在某元素.
　 *　参数:元素.
　 *　返回:true/false.
　 */
Array.prototype.Contains = function (e) {
    for (i = 0; i < this.length && this[i] != e; i++);
    return !(i == this.length);
}

// 查找某个元素所在的键值
Array.prototype.IndexOf = function (val) {
    for (var i = 0, l = this.length; i < l; i++) {
        if (this[i] == val) return i;
    }
    return null;
}
/*
　 *　方法:Array.RemoveIndexOf(dx)
　 *　功能:删除数组元素.
　 *　参数:dx删除元素的下标.
　 *　返回:在原数组上修改数组.
　 */
Array.prototype.RemoveIndexOf = function (dx) {
    if (isNaN(dx) || dx > this.length) { return false; }
    this.splice(dx, 1);
}

Array.prototype.RemoveItem = function (it) {
    this.RemoveIndexOf(this.IndexOf(it));
}
