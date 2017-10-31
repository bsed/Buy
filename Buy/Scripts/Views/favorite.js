
var canLoadPage = true;
var isCoupon = $(".couponFavorite").length > 0 ? true : false;
var platforms = $("[data-platform].active").data("platform");

var loading = $("#loading").attr("src");

function clear(target) {
    $(target).children().remove();
    var html = "";
    html += '<li class="loadModule loadModule-dataIng" data-page="0" data-next="true"><img class="marginR8" src="' + loading + '"/>加载中</li>';
    $(target).append(html);
}

//加载列表
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $coupon = isCoupon ? $(".couponFavorite") : $(".localCouponFavorite");
    var $page = $coupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = parseInt($page.data("page")) + 1;
    canLoadPage = false;
    $(".nodata").addClass("hidden");
    $.ajax({
        type: "GET",
        url: comm.action(isCoupon ? "Coupon" : "LocalCoupon", "Favorite"),
        data: {
            page: page,
            platforms: platforms
        },
        dataType: "html",
        success: function (data) {
            var $data = $(data);
            if ($data.length > 0) {
                if (isCoupon) {
                    $(".couponFavorite li[data-page]").remove();
                } else {
                    $(".localCouponFavorite li[data-page]").remove();
                }
            }
            $coupon.find("ul").append($data);
            if (!isCoupon) {
                favorite();
            }
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

$("[data-platform]").click(function () {
    var $this = $(this);
    $("[data-platform]").removeClass("active");
    $this.addClass("active");
    platforms = $this.data("platform");
    clear(".couponFavorite ul");
    canLoadPage = true;
    loadCoupon();
});

//删除收藏按鈕
function favorite() {

    $(".localcoupon-delCardBtn").click(function (e) {
        var $this = $(this);
        var data = {
            id: $this.data("id"),
        };

        $.ajax({
            type: "POST",
            url: comm.action("Delete", "Favorite"),
            data: data,
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    comm.promptBox(data.Message);
                    $this.parents("li").remove();

                    if ($(".localCouponFavorite ul").find("li").not(".loadModule").length == "0") {
                        $(".loadModule").remove();
                    }
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