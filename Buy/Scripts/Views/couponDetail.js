$("#btnBack").goback(comm.action("Index", "Coupon"));

if ($("#ID").length > 0) {
    function GetDetailImgs() {
        $.ajax({
            type: "POST",
            url: comm.action("GetDetailImgs", "Coupon"),
            data: { id: $("#ID").val() },
            dataType: "json",
            success: function (data) {
                if (data.CyState == "Success") {
                    var html = "";
                    if (data.Data.length > 0) {
                        var lazyloadimg = $("#lazyloadimg").attr("src");
                        $.each(data.Data, function (i, n) {
                            html += '<img src="' + lazyloadimg + '" data-original="' + n + '" />';
                        })
                        $("[name=poductDetail]").append(html);
                        comm.lazyloadALL();
                    }
                }
            }
        });
    }
    if ($("[name=poductDetail]").find("img").length <= 0) {
        GetDetailImgs();
    }
}