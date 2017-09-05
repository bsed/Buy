$("#submit").click(function (e) {
    var data = {
        UserName: $("#UserName").val(),
        Password: $("#Password").val(),
    };
    $.ajax({
        type: "POST",
        url: comm.action("Login", "Account"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.CyState == "Success") {
                if ($("#returnUrl").val() != "") {
                    location = $("#returnUrl").val();
                }
                else {
                    location = comm.action("Index", "UserManage");
                }
            }
            else {
                comm.alter(0, data.CyMessage);
            }
        }
    });
});