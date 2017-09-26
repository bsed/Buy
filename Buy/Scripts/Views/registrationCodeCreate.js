
$("#btnSubmit").click(function () {
    var msg = "确认要添加" + $("#Count").val() + "个注册码么";
    return confirm(msg);
});

$("#ActiveDateTime,#UseEndDateTime").date();
$("[data-code] :checkbox").click(function () {
    $("[data-code] :checkbox").not(this).prop("checked", false);
});