
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