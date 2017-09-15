var canLoadPage = true;

//加载列表
loadCoupon()
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $localCoupon = $("#localCoupon");
    var $page = $localCoupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = parseInt($page.data("page")) + 1;
    canLoadPage = false;
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "LocalCoupon"),
        data: {
            page: page,
            shopId: $("#ID").val()
        },
        dataType: "html",
        success: function (data) {
            $page.remove();
            var $data = $(data);
            $localCoupon.find("ul").append($data);
            comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;
        }
    });
}