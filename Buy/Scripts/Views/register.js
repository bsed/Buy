//注册按钮
$("#registerBtn").click(function () {
    var data = {
        UserName: $("#PhoneNumber").val(),
        Code: $("#Code").val(),
        Password: $("#Password").val(),
    };
    $.ajax({
        type: "POST",
        url: comm.action("Register", "Account"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                location = comm.action("Activation", "Account");
            } else {
                comm.promptBox(data.Message)
            }
        }
    });
})

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
                codeCountDown(60);
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

$(".register-input").keyup(function () {
    var data = {
        UserName: $("#PhoneNumber").val(),
        Code: $("#Code").val(),
        Password: $("#Password").val(),
    };

    if (data.UserName != "" && data.Password != "" && data.Password != "") {
        $("#registerBtn").prop("disabled", false);
    } else {
        $("#registerBtn").prop("disabled", true);
    }
});

//邀请码按钮
$("#inviteBtn").click(function (e) {
    var data = {
        userId: $("#Id").val(),
        Code: $("#inviteCode").val(),
    };
    $.ajax({
        type: "POST",
        url: comm.action("Activation", "Account"),
        data: "",
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                location = comm.action("Index", "Coupon");
            } else {
                comm.promptBox(data.Message)
            }
        }
    });
});

$(".invite-input").keyup(function () {
    var data = {
        inviteCode: $("#inviteCode").val()
    };

    if (data.inviteCode != "") {
        $("#inviteBtn").prop("disabled", false);
    } else {
        $("#inviteBtn").prop("disabled", true);
    }
});
