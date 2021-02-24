
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
            if (WoncaIntl.WoncaStrings.l_AccessDenied.indexOf("Sorry") > -1) {
                WoncaIntl.WoncaStrings.l_AccessDenied = "Sorry, you don't have permission to edit this document. The file has been deleted, moved, or locked by someone else and cannot be saved at this time, please refresh the page.";
            } else if (WoncaIntl.WoncaStrings.l_AccessDenied.indexOf("抱歉") > -1) {
                WoncaIntl.WoncaStrings.l_AccessDenied = "抱歉，您无权编辑此文档。文件已被删除、移动或已被他人锁定，暂时无法进行保存，请刷新页面。";
            } else {
                WoncaIntl.WoncaStrings.l_AccessDenied = "抱歉，您無權編輯此檔案。檔案已被删除、移動或已被他人鎖定，暫時無法進行保存，請刷新頁面。";
            }
        } else {
            window.setTimeout(SetMsg, 1000);
        }
    } catch (ex) {
        window.setTimeout(SetMsg, 1000);
    }
}
