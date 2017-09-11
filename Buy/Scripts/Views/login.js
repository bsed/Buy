setTimeout(function () {
    var UserName = $("#UserName").val()
    if (UserName != "") {
        $("#submit").prop("disabled", false);
        $("[name='clearVal']").removeClass("hidden");
    }
}, 500);

//清除val
$("[name='clearVal']").click(function () {
    $(this).addClass("hidden");
    $(this).parent().find("input").val(null);

    $("#submit").prop("disabled", true);
});


$("#submit").click(function (e) {
    var data = {
        UserName: $("#UserName").val(),
        Password: $("#Password").val(),
    };

    if (data.UserName == "") {
        comm.promptBox("请填写手机号码");
        return false
    } else if (data.Password == "") {
        comm.promptBox("请填写登录密码");
        return false
    }

    $.ajax({
        type: "POST",
        url: comm.action("Login", "Account"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                if ($("#returnUrl").val() != "") {
                    location = $("#returnUrl").val();
                }
                else {
                    location = comm.action("Index", "UserManage");
                }
            }
            else {
                comm.promptBox(data.Message)
            }
        }
    });
});

$(".account-input input").keyup(function () {
    var data = {
        UserName: $("#UserName").val(),
        Password: $("#Password").val(),
    };

    if ($(this).val() == "") {
        $(this).parent().find("[name='clearVal']").addClass("hidden");
    } else {
        $(this).parent().find("[name='clearVal']").removeClass("hidden");
    }

    if (data.UserName != "" && data.Password != "") {
        $("#submit").prop("disabled", false);
    } else {
        $("#submit").prop("disabled", true);
    }
});