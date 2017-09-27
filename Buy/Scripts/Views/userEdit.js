

var avatar = new imageResizeUpload("#Avatar", {
    uploaded: function (state, message, result) {
        $("[name='Avatar.ImageUrl']").val(result.url);
    }
});

$("#submit").click(function (e) {
    var data = {
        userID: $("#ID").val(),
        nickName: $("#NickName").val(),
        avatar: $("[name='Avatar.ImageUrl']").val()
    };
    console.log(data);
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "User"),
        data: data,
        dataType: "json",
        success: function (data) {
            comm.promptBox(data.Message);
            if (data.State == "Success") {
                setTimeout(function () {
                    location = comm.action("Index", "User")
                }, 3000);
            }
        }
    });
});


