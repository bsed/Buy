$("[name=btnUpdate]").click(function (e) {
    var nickName = $(this).data("name");
    if (confirm("确认要给" + nickName + "升级该代理?")) {
        $.ajax({
            type: "POST",
            url: comm.action("Update", "UserMange"),
            data: {
                id: $("#hidUserID").val()
            },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    comm.alter(1, data.Message)
                } else {
                    comm.alter(0, data.Message)
                }
            }
        });
    }
 
});