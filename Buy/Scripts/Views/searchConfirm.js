var platform = $("#platform").val(),
    sort = $("[data-sort].active").data("sort"),
    maxPrice = $("#maxPrice").val(),
    types = $("#typeID").val();
var canLoadPage = true;

function clear(target) {
    $(target).children().remove();
    var html = "";
    html += '<li class="loadModule loadModule-dataIng" data-page="0" data-next="true">加载中</li>';
    $(target).append(html);
}

function nodataCheck(target) {
    if ($(target).children().length == "0") {
        $(".nodata").addClass("hidden");

        if ($(".nodata").hasClass("hidden")) {
            $(".nodata").removeClass("hidden");
        }
    }
}

//load优惠券
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $coupon = $("#coupon");
    var $page = $coupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = $page.data("page") + 1;
    canLoadPage = false;
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "coupon"),
        data: {
            page: page,
            sort: sort,
            platforms: platform,
            types: types,
            maxPrice: maxPrice,
            orderByTime: $("#secound").val() == "secound" ? true : false,
        },
        dataType: "html",
        success: function (data) {
            $page.remove();
            var $data = $(data);

            $coupon.find(">ul").append($data);
            comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;

            nodataCheck("#coupon ul");
        }
    });

}

loadCoupon();

$(window).scroll(function () {
    if ($(window).scrollTop() > 0) {
        $(".setScrollTop").fadeIn();
    } else {
        $(".setScrollTop").fadeOut();
    }

    if (canLoadPage && comm.isWindowBottom()) {
        loadCoupon();
    }
});

$(".setScrollTop").click(function () {
    $('body').animate({ scrollTop: '0' }, 500);
});

//排序切换
$(".sort").click(function (e) {
    sort = $(this).data("sort");
    $(".sort").removeClass("active");
    $(this).addClass("active");
    clear("#coupon ul");
    loadCoupon();
});