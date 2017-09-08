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

$(".account-input input").keyup(function () {
    var data = {
        UserName: $("#PhoneNumber").val(),
        Code: $("#Code").val(),
        Password: $("#Password").val(),
    };

    if (data.UserName != "" && data.Password != "" && data.Password != "") {
        $("input[type='submit']").prop("disabled", false);
    } else {
        $("input[type='submit']").prop("disabled", true);
    }
});