
// 页面加载完
$(function () {
    try {
        SetMsg();
    } catch (ex) {
        console.log(ex);
    }
});

function SetMsg() {
    try {
        if (WoncaIntl != null && WoncaIntl != undefined) {
            WoncaIntl.WoncaStrings.l_WordKickOutHotStoreStatusLoad = "文件已经被其他人锁定，暂时无法进行保存。";
            WoncaIntl.WoncaStrings.l_AccessDenied = "抱歉，您无权编辑此文档。文件已被删除、移动或已被他人锁定，暂时无法进行保存。";
        } else {
            window.setTimeout(SetMsg, 1000);
        }
    } catch (ex) {
        window.setTimeout(SetMsg, 1000);
    }
}
