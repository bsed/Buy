

var avatar = new imageResizeUpload("#Avatar", {
    uploaded: function (state, message, result) {
        $("[name='Avatar.ImageUrl']").val(result.url);
    }
});

$("#submit").click(function (e) {
    var data = {
        ID: $("#ID").val(),
        NickName: $("#NickName").val(),
        Avatar: $("[name='Avatar.ImageUrl']").val()
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
                }, 1500);
            }
        }
    });
});

//清除val
$("[name='clearVal']").click(function () {
    $(this).addClass("hidden");
    $(this).parent().find("input").val(null);
});

$(".account-input input").keyup(function () {
    if ($(this).val() == "") {
        $(this).parent().find("[name='clearVal']").addClass("hidden");
    } else {
        $(this).parent().find("[name='clearVal']").removeClass("hidden");
    }
});
