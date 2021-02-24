// 水印图片
var water_mark,repeat;

// 页面加载完
$(function () {
    try {
        GetWaterMark();
    } catch (ex) {
        console.log(ex);
    }
});

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
        if (document.getElementById("WACScroller") != null) {
            $("#WACScroller img").before("<div class='WACScrollerWaterMark'></div>");

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