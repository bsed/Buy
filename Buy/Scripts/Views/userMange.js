var unionUrl = "http://localhost:18434/";

if ($(".productProxy").length > 0) {

    $("#productProxy_ck").click(function (e) {
        ckChange();
    });

    function ckChange() {
        if ($("#productProxy_ck").is(":checked")) {
            $(".productProxy").removeClass("hidden");
        } else {
            $(".productProxy").addClass("hidden");
        }
    }

    GetProduct();
    function GetProduct() {
        $.ajax({
            type: "GET",
            url: unionUrl + "ProductProxy/GetProduct",
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    $.each(data.Result, function (i, item) {
                        var demo = $("#productProxy_sel").find(".demo").clone().removeClass("demo");
                        demo.val(item.ID);
                        demo.text(item.Name);
                        $("#productProxy_sel").append(demo);
                    });
                    $("#productProxy_sel").find(".demo").remove();
                }
            }
        });
    }

    function GetProductProxy() {
        $.ajax({
            type: "GET",
            url: unionUrl + "ProductProxy/Get",
            data: { userId: $("#Id").val() },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    $("#productProxy_ck").attr("checked", 'true');
                    $("#productProxy_ck").val(data.Result.ID)
                    ckChange();
                    $("#productProxy_sel").val(data.Result.ProductID);
                    $("#productProxy_max").val(data.Result.Max)
                }
            }
        });
    }
    if ($("#Id").length > 0) {
        GetProductProxy();
    }
}


$("#creat").click(function (e) {
    var data = {
        UserType: $("#UserType").val(),
        ParentUserNickName: $("#ParentUserNickName").val(),
        ParentUserID: $("#ParentUserID").val(),
        PhoneNumber: $("#PhoneNumber").val(),
        NickName: $("#NickName").val(),
        Password: $("#Password").val(),
    };

    if ($(".productProxy").length > 0 && $("#productProxy_ck").is(":checked")) {
        if (Number($("#productProxy_sel").val()) <= 0) {
            comm.alter(0, "选择商品");
            return false;
        }
    }

    $.ajax({
        type: "POST",
        url: comm.action("Create", "UserManage"),
        data: data,
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                if ($(".productProxy").length > 0 && $("#productProxy_ck").is(":checked")) {
                    var unionData = {
                        ProductID: $("#productProxy_sel").val(),
                        ProxyUserID: data.Result.Id,
                        Count: 0,
                        Max: $("#productProxy_max").val() == "" ? 0 : $("#productProxy_max").val()
                    }
                    $.ajax({
                        type: "POST",
                        url: unionUrl + "ProductProxy/Creat",
                        data: unionData,
                        dataType: "json",
                        success: function (unionData) {
                            if (unionData.State == "Success") {
                                location = data.Result.ReturnUrl
                            }
                        }
                    });
                } else {
                    location = data.Result.ReturnUrl
                }
            } else {
                comm.alter(0, data.Message);
                return false;
            }
        }
    });
});


$("#edit").click(function (e) {
    if ($(".productProxy").length > 0) {
        if ($("#productProxy_ck").is(":checked")) {
            if (Number($("#productProxy_sel").val()) <= 0) {
                comm.alter(0, "选择商品");
                return false;
            }
            if ($("#productProxy_ck").val() == "") {
                //参加
                var data = {
                    ProductID: $("#productProxy_sel").val(),
                    ProxyUserID: $("#Id").val(),
                    Max: $("#productProxy_max").val() == "" ? 0 : $("#productProxy_max").val()
                }
                $.ajax({
                    type: "POST",
                    url: unionUrl + "ProductProxy/Creat",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        if (data.State == "Success") {
                        }
                    }
                });
            } else {
                //修改
                console.log("00000");
                var data = {
                    id: $("#productProxy_ck").val(),
                    Max: $("#productProxy_max").val() == "" ? 0 : $("#productProxy_max").val(),
                    ProductID: $("#productProxy_sel").val(),
                }
                $.ajax({
                    type: "POST",
                    url: unionUrl + "ProductProxy/Edit",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                    }
                });
            }
        } else {
            if ($("#productProxy_ck").val() != "") {
                //删除活动
                $.ajax({
                    type: "POST",
                    url: unionUrl + "ProductProxy/Delete",
                    data: { id: $("#productProxy_ck").val() },
                    dataType: "json",
                    success: function (data) {
                    }
                });
            }
        }
    }
    return true;
});

