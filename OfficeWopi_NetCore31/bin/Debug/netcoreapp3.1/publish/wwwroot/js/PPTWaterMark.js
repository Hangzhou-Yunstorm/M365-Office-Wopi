// 水印图片
var water_mark, repeat;

// 页面加载完
$(function () {
    try {
        SetMsg();
        var type = getQueryString("PowerPointView");
        if (type == "ReadingView") {
            GetWaterMark();
        }
    } catch (ex) {
        console.log(ex);
    }
});

function SetMsg() {
    try {
        if (ResourceStrings != null && ResourceStrings != undefined) {
            ResourceStrings.errorMessage_CannotSaveDocument = "抱歉，未保存所做的更改。文件已经被他人锁定，暂时无法进行保存。";
            ResourceStrings.errorMessage_AccessDenied = "PowerPoint Online 无法打开此演示文稿，因为你没有权限或此文件已被删除、移动或已被他人锁定。";
        } else {
            window.setTimeout(SetMsg, 1000);
        }
    } catch (ex) {
        window.setTimeout(SetMsg, 1000);
    }
}

function GetWaterMark() {
    try {
        var wopiUrl = decodeURIComponent(getQueryString("Wopisrc")).split("wopi/files")[0];
        var token = encodeURIComponent(getQueryString("access_token"));
        $.ajax({
            type: "POST",
            url: wopiUrl + "WaterMark/GetWaterMark",
            data: {
                words: token
            },
            success: function (response) {
                if (response.success) {
                    water_mark = response.data;
                    repeat = response.repeat;
                    if (water_mark != null && water_mark != "") {
                        SetWaterMark();
                    }
                } else {
                    console.log(response.message);
                }
            },
            failure: function (response) {
                console.log(response);
            }
        });
    } catch (ex) {
        console.log(ex);
    }
}

// 设置水印
function SetWaterMark() {
    try {
        if (document.getElementById("SlideBorder") != null && document.getElementById("browserLayerViewId") != null && document.getElementById("preview") != null) {
            //预览
            $("#preview").before("<div id='previewDiv'></div>");
            document.querySelector('#previewDiv').appendChild(document.querySelector('#preview'));

            var preview = document.getElementById("preview");
            var top = preview.style.top;
            var left = preview.style.left;
            $("#previewDiv").css("position", "absolute");
            $("#previewDiv").css("top", top);
            $("#previewDiv").css("left", left);
            $("#preview").css("position", "static");
            $("#preview").css("left", "0");
            $("#preview").css("top", "0");
            $("#preview").before("<div class='WACScrollerWaterMark'></div>");

            //$("#CommentOverlay").before("<div class='WACScrollerWaterMark'></div>");
            // 幻灯片
            $("#browserLayerViewId").before("<div class='WACScrollerWaterMark'></div>");

            if (repeat) {
                $(".WACScrollerWaterMark").css("background", "url(" + water_mark + ") repeat");
            } else {
                $(".WACScrollerWaterMark").css("background", "url(" + water_mark + ") no-repeat");
                $(".WACScrollerWaterMark").css("background-size", "100% 100%");
            }
            $(".WACScrollerWaterMark").css("width", "100%");
            $(".WACScrollerWaterMark").css("height", "100%");
            $(".WACScrollerWaterMark").css("position", "absolute");
            $(".WACScrollerWaterMark").css("top", "0");
            $(".WACScrollerWaterMark").css("left", "0");
            $(".WACScrollerWaterMark").css("z-index", "4");
        } else {
            window.setTimeout(SetWaterMark, 100);
        }
    } catch (ex) {
        console.log(ex);
    }
}

// 获取链接参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}