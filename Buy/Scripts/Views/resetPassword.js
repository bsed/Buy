
$("#submit").click(function (e) {
    var data = {
        UserID: $("#UserID").val(),
        OldPassword: $("#OldPassword").val(),
        NewPassword: $("#NewPassword").val(),
    };
    $.ajax({
        type: "POST",
        url: comm.action("ResetPassword", "Account"),
        data: data,
        dataType: "json",
        success: function (data) {
            comm.promptBox(data.Message)
        }
    });
});

