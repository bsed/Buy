$("[name=btnUpdate]").click(function (e) {
    var $this = $(this);
    var nickName = $this.data("name");
    var id = $this.data("id");
    if (confirm("确认要给“" + nickName + "”升级为代理？")) {
        $.ajax({
            type: "POST",
            url: comm.action("Update", "UserManage"),
            data: {
                id: id
            },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    comm.alter(1, data.Message);
                    setTimeout(function () {
                        location = location;
                    }, 1000);
                 
                } else {
                    comm.alter(0, data.Message);
                }
            }
        });
    }
    return false;
});