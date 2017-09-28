$(".navTabBottom li.user").addClass("active");

var avatar = new imageResizeUpload("#Avatar", {
    uploaded: function (state, message, result) {
        var data = {
            userID: $("#Id").val(),
            nickName: $("#NickName").val(),
            avatar: result.url
        };
        $.ajax({
            type: "POST",
            url: comm.action("Edit", "User"),
            data: data,
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    $(".avatav").attr("src", comm.imgFullUrl(result.url) + "?h=100&scale=Both&404=Avatar")
                }
            }
        });
        //$("[name='Avatar.ImageUrl']").val(result.url);
    }
});