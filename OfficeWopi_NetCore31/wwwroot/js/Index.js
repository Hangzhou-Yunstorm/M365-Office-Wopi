function SetNLBUrls() {
    var urls = $("#NLBUrls").val();
    $.ajax({
        type: "POST",
        url: "/Home/WriteNLBUrls",
        data: { urls: urls },
        success: function (response) {
            if (response.success) {
                alert(response.message);
                location.reload();
            } else {
                alert(response.message);
            }
        },
        failure: function (response) {
            console.log(response);
        }
    });
}

function ShowSet() {
    $(".nlb_set").show();
    $(".nlb_note").hide();
}

function CancelSet(){
    $(".nlb_note").show();
    $(".nlb_set").hide();
}