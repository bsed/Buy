
var canLoadPage = true;

//加载列表
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $coupon = $(".localCouponFavorite");
    var $page = $coupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = parseInt($page.data("page")) + 1;
    canLoadPage = false;
    $(".nodata").addClass("hidden");
    $.ajax({
        type: "GET",
        url: comm.action("LocalCoupon", "Favorite"),
        data: {
            page: page
        },
        dataType: "html",
        success: function (data) {
            var $data = $(data);
            if ($data.length > 0) {
                $(".localCouponFavorite li[data-page]").remove();
            }
            $coupon.find("ul").append($data);
            favorite();
            comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;
            //if ($coupon.find("li").length == 0) {
            //    nodataCheck("#coupon ul");
            //}
        }
    });
}
loadCoupon();

//删除收藏按鈕
function favorite() {

    $(".localcoupon-delCardBtn").click(function (e) {
        var data = {
            CouponID: $(this).data("id"),
        };
        $.ajax({
            type: "POST",
            url: comm.action("Delete", "Favorite"),
            data: data,
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    comm.promptBox(data.Message);
                } else {
                    comm.promptBox(data.Message);
                }
            }
        });
    });
}

//滑动
$(window).scroll(function (e) {
    if (canLoadPage && comm.isWindowBottom()) {
        loadCoupon();
    }
});