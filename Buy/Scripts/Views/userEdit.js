

var avatar = new imageResizeUpload("#Avatar", {
    uploaded: function (state, message, result) {
        $("[name='Avatar.ImageUrl']").val(result.url);
    }
});

$("#submit").click(function (e) {
    var model = {
        ID: $("#ID").val(),
        NickName: $("[name=NickName]").val(),
        Avatar: $("[name='Avatar.ImageUrl']").val()
    }
    $.ajax({
        type: "POST",
        url: comm.action("Edit", "User"),
        data: data,
        dataType: "json",
        success: function (data) {
            comm.promptBox(data.Message)
        }
    });
});


