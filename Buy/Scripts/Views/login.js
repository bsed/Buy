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
                $.ajax({
                    type: "GET",
                    url: comm.action("GetUserInfo", "Account"),
                    data: { userId: data.Result.ID },
                    dataType: "json",
                    success: function (result) {
                        if (result.State == "Success") {
                            if (result.Result.Data.UserType==1) {
                                if ($("#returnUrl").val() != "") {
                                    location = $("#returnUrl").val();
                                }
                                else {
                                    location = comm.action("Index", "UserManage");
                                }
                            } else {
                                if (result.Result.Data.IsActivation) {
                                    if ($("#returnUrl").val() != "") {
                                        location = $("#returnUrl").val();
                                    }
                                    else {
                                        location = comm.action("Index", "Coupon");
                                    }
                                } else {
                                    location = comm.action("Activation", "Account");
                                }
                            }
                        }
                    }
                });
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

    if (data.UserName != "" && data.Password != "") {
        $("#submit").prop("disabled", false);
    } else {
        $("#submit").prop("disabled", true);
    }
});