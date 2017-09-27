setTimeout(function () {
    var PhoneNumber = $("#PhoneNumber");
    var Code = $("#Code");
    var Password = $("#Password");
    if (Code.val() != "") {
        Code.parent().find("[name='clearVal']").removeClass("hidden");
        Password.parent().find("[name='clearVal']").removeClass("hidden");
    }
    if (PhoneNumber.val() != "") {
        $("#forgotBtn").prop("disabled", false);
        PhoneNumber.parent().find("[name='clearVal']").removeClass("hidden");
    }
}, 500);

//清除val
$("[name='clearVal']").click(function () {
    $(this).addClass("hidden");
    $(this).parent().find(".forgot-input").val(null);

    $("#forgotBtn").prop("disabled", true);
});

$(".forgot-input").keyup(function () {
    var data = {
        PhoneNumber: $("#PhoneNumber").val(),
        Code: $("#Code").val(),
        Password: $("#Password").val()
    };

    if ($(this).val() == "") {
        $(this).parent().find("[name='clearVal']").addClass("hidden");
    } else {
        $(this).parent().find("[name='clearVal']").removeClass("hidden");
    }

    if (data.PhoneNumber != "" && data.Code != "" && data.Password != "") {
        $("#forgotBtn").prop("disabled", false);
    } else {
        $("#forgotBtn").prop("disabled", true);
    }
});

//获取验证码
$("#btnGetCode").click(function () {
    var phone = $("#PhoneNumber").val();
    if (phone == "") {
        comm.promptBox("请输入手机号");
        return false;
    }
    if (!new check().isPhone(phone)) {
        comm.promptBox("请输入正确的手机号");
        return false;
    }
    $.ajax({
        type: "POST",
        url: comm.action("SendCode", "Account"),
        data: { Phone: $("#PhoneNumber").val() },
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                //codeCountDown(60);
            } else {
                comm.promptBox(data.Message);
            }
        }
    });
})

//验证码倒计时
function codeCountDown(timespan) {
    var $btnSendCode = $("#btnGetCode");
    $btnSendCode.prop("disabled", true)
    if ($.isNumeric(timespan) && timespan > 0) {
        var id = setInterval(function () {
            $btnSendCode.val("重发（" + timespan + "）");
            if (timespan <= 0) {
                clearInterval(id);
                $btnSendCode.val("重发").prop("disabled", false)
            }
            timespan--;
        }, 1000);
    }
}

//忘记密码按钮
$("#forgotBtn").click(function (e) {
    var data = {
        PhoneNumber: $("#PhoneNumber").val(),
        Code: $("#Code").val(),
        Password: $("#Password").val(),
    };
    $.ajax({
        type: "POST",
        url: comm.action("ForgotPassword", "Account"),
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
                            if (result.Result.Data.UserType == 1) {
                                location = comm.action("Index", "UserManage");
                            } else {
                                if (result.Result.Data.IsActivation) {
                                    location = comm.action("Index", "Coupon");
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
