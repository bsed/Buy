var typeID = $("#typeID").val(),
    platform = $("#platform").val(),
    sort = $("[data-sort].active").data("sort");
var canLoadPage = true;

//swiper
if ($(".navigationSwiper").length > 0) {
    var slideIndexHead = $("[name='type'].active").index();
    var swiper = new Swiper('.navigationSwiper .swiper-container', {
        slidesPerView: "auto",
        initialSlide: slideIndexHead
    });
}

//主导航
if ($("swiper-container").length > 0) {
    var swiper = new Swiper('.couponIndex-banner .swiper-container', {
        slidesPerView: "auto",
        pagination: '.swiper-pagination',
        paginationClickable: true,
        autoplay: 2500,
        autoplayDisableOnInteraction: false
    });
}

$(window).scroll(function () {
    if (canLoadPage && comm.isWindowBottom()) {
        loadCoupon();
    }
});

//2级分类查看全部
var sortGetAll = $("#sortGetAll");
var sortList = $("[name='sortList']");

//判断分类个数
var sort_sum = sortList.find("li").length;
if (sort_sum >= 9) {
    sortList.addClass("style02");
    sortGetAll.removeClass("active");
}

sortGetAll.click(function () {
    sortList.addClass("getAll");
    comm.mask3();
});

$(".mask.style02").click(function () {
    sortList.removeClass("getAll");
    comm.mask3();
});

$("[name=childType]").click(function (e) {
    var date_type = $(this).data("type");
    typeID = date_type;
    location = comm.action("Second", "Coupon",
        {
            typeID: typeID,
            platform: platform
        });
});

//1级分类切换
var couponType = $("[name='type']");
couponType.click(function () {
    var date_type = $(this).data("type");
    typeID = date_type;
    location = comm.action("Index", "Coupon",
        {
            typeID: typeID,
            platform: platform,
            sort: sort
        });

    if (date_type == "0") {
        $("#index").removeClass("hidden");
    } else {
        $("[name='sortList']").removeClass("hidden");
    }
});

//加载列表
loadCoupon()
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $coupon = $("#coupon");
    var $page = $coupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = parseInt($page.data("page")) + 1;
    canLoadPage = false;
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "Coupon"),
        data: {
            page: page,
            sort: sort,
            types: typeID,
            platforms: platform,
            orderByTime:true
        },
        dataType: "html",
        success: function (data) {
            $page.remove();
            var $data = $(data);
            $coupon.find("ul").append($data);
            sort = null;
            comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;
        }
    });
}

//排序切换
$(".sort").click(function (e) {
    sort = $(this).data("sort");
    var $page = $("#coupon").find("ul li[data-page]");
    $page.data("next", "true");
    $page.data("page", "0");
    $("#coupon").find("li").not("[data-page]").remove();
    loadCoupon();
    $(".sort").removeClass("active");
    $(this).addClass("active");
    comm.addHistory("url", comm.action("Index", "Coupon", {
        sort: sort,
        platform: platform,
        typeID: typeID,
    }));
});

//tabActive
if (platform == "" || platform == "0" || platform == "TaoBao") {
    $(".navTabBottom li.taobao").addClass("active");
} else if (platform == "4" || platform == "MoGuJie") {
    $(".navTabBottom li.mogujie").addClass("active");
} else {
    $(".navTabBottom li.jd").addClass("active");
}

$(".navTabBottom li.jd,.navTabBottom li.mogujie,.navTabBottom li.taobao").click(function (e) {
    e.preventDefault();
    platform = $(this).data("platform");
    location = comm.action("Index", "Coupon",
        {
            typeID: typeID,
            platform: platform,
            sort: sort
        });
});
