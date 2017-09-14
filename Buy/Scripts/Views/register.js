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

//图片验证组件
function vercode(option) {
    if (option == undefined) {
        option = {};
    }
    if (option.success == undefined) {
        option.success = function () { }
    }
    if (option.fail == undefined) {
        option.fail = function () { }
    }
    var $target = $("#vercode");
    var $txt = $target.find("#txtVercode");
    var $img = $target.find("#imgVerCode");
    function _show() {
        $target.removeClass("hidden");
    }
    this.show = function () {
        _show();
    };

    function _hide() {
        $target.addClass("hidden");
    }

    this.hide = function () {
        _hide();
    }



    $txt.keyup(function () {
        if ($txt.val().length == 4)

        $.ajax({
            type: "POST",
            url: comm.action("CheckCode", "Account"),
            data: { code: $txt.val() },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    option.success();
                   // alert("验证成功");
                } else {
                    option.fail();
                    //alert("验证失败");
                }
            }
        });
    });

    $img.click(function () {
        $img.attr("src", comm.action("VerCode", "Account", { ts: new Date().getTime() }))
    });

}

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
    var _vercode = new vercode({
        success: function () {
            _vercode.hide();
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
        },
        fail: function () {
        }
    });
    _vercode.show();


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

setTimeout(function () {
    var PhoneNumber = $("#PhoneNumber");
    var Code = $("#Code");
    var Password = $("#Password");
    if (Code.val() != "") {
        Code.parent().find("[name='clearVal']").removeClass("hidden");
        Password.parent().find("[name='clearVal']").removeClass("hidden");
    }
    if (PhoneNumber.val() != "") {
        $("#registerBtn").prop("disabled", false);
        PhoneNumber.parent().find("[name='clearVal']").removeClass("hidden");
    }
}, 500);

//注册页清除val
$("[name='clearVal']").click(function () {
    $(this).addClass("hidden");
    $(this).parent().find(".register-input").val(null);

    $("#registerBtn").prop("disabled", true);
});
//注册页input
$(".register-input").keyup(function () {
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
        $("#registerBtn").prop("disabled", false);
    } else {
        $("#registerBtn").prop("disabled", true);
    }
});

//邀请码页input
$("#inviteCode").keyup(function () {
    var data = {
        inviteCode: $("#inviteCode").val()
    };

    if ($(this).val() == "") {
        $(this).parent().find("[name='clearVal']").addClass("hidden");
    } else {
        $(this).parent().find("[name='clearVal']").removeClass("hidden");
    }

    if (data.inviteCode != "") {
        $("#inviteBtn").prop("disabled", false);
    } else {
        $("#inviteBtn").prop("disabled", true);
    }
});

//邀请码页清除val
$("[name='clearVal']").click(function () {
    $(this).addClass("hidden");
    $(this).parent().find("#inviteCode").val(null);

    $("#inviteBtn").prop("disabled", true);
});

$("#btnBack").goback(comm.action("Index", "Coupon"));
