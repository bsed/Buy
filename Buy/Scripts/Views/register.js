var e;

$("input[type='submit']").click(function () {
    e = setInterval("error()", 1000)
})

function error() {
    var error = $("#errorText li").first().text();

    if (error != "") {
        comm.promptBox(error)
        clearInterval(e)
        return false;
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

$(".invite-input").keyup(function () {
    var data = {
        inviteCode:$(this).val()
    };

    if (data.inviteCode != "") {
        $("#inviteBtn").prop("disabled", false);
    } else {
        $("#inviteBtn").prop("disabled", true);
    }
});