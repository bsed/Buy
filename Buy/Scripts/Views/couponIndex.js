var typeID = $("#typeID").val(),
    platform = $("#platform").val(),
    sort = $("[data-sort].active").data("sort");
var canLoadPage = true;
var navTop = $("#couponNav").offset().top;
var sTop = $(".navigationBar").height();
var tTop = $(".navigationSwiper").height();
var loopState = false;

function nodataCheck(target) {
    if ($(target).children().length == "0") {
        $(".nodata").addClass("hidden");

        if ($(".nodata").hasClass("hidden")) {
            $(".nodata").removeClass("hidden");
        }
    }
}

//swiper
function typeSwipe() {
    var slideIndexHead = $("[name='type'].active").index();
    var swiper = new Swiper('.navigationSwiper .swiper-container', {
        slidesPerView: "auto",
        initialSlide: slideIndexHead
    });
}


//主导航
if ($(".couponIndex-banner").find(".swiper-wrapper").length > 0) {
    if ($(".couponIndex-banner").find(".swiper-slide").length > 1) {
        loopState: true;
    }

    var swiper = new Swiper('.couponIndex-banner .swiper-container', {
        slidesPerView: "auto",
        pagination: '.swiper-pagination',
        paginationClickable: true,
        autoplay: 2500,
        autoplayDisableOnInteraction: false,
        loop: loopState
    });
}

$(window).scroll(function (e) {
    if ($(window).scrollTop() > 0) {
        $(".setScrollTop").fadeIn();
    } else {
        $(".setScrollTop").fadeOut();
    }

    if ($(window).scrollTop() + sTop + tTop >= navTop) {
        $("#couponBox").addClass("paddingT128");
        $("#couponNav").addClass("fixTop88");
    } else {
        $("#couponBox").removeClass("paddingT128");
        $("#couponNav").removeClass("fixTop88");
    }

    if (canLoadPage && comm.isWindowBottom()) {
        loadCoupon();
    }
});

$(".setScrollTop").click(function () {
    $('body').animate({ scrollTop: '0' }, 500);
});

//2级分类查看全部
var sortGetAll = $("#sortGetAll");
var sortList = $("[name='sortList']");

//判断分类个数
var sort_sum = sortList.find("li").length;
if (sort_sum >= 10) {
    sortList.addClass("style02");
    sortGetAll.removeClass("hidden");
}

sortGetAll.click(function () {
    sortList.addClass("getAll");
    $("body").css("overflow", "hidden");
    comm.mask3();
});

$(".mask.style02").click(function () {
    sortList.removeClass("getAll");
    $("#sortOne").slideUp();
    $("body").css("overflow", "auto");
    comm.mask3();
});

//获取分类
function GetCouponType() {
    $.ajax({
        type: "GET",
        url: comm.action("GetCouponTypes", "Coupon"),
        data: { platform: platform },
        dataType: "json",
        success: function (data) {
            $.each(data.Result.Data, function (i, item) {
                var swiperdemo = $(".swiperDemo").clone().removeClass("hidden swiperDemo");
                swiperdemo.attr("data-type", item.ID);
                swiperdemo.find("span").text(item.Name);
                $(".swiperDemo").before(swiperdemo);

                var sortListDemo = $(".sortListDemo").clone().removeClass("hidden sortListDemo");
                sortListDemo.attr("data-type", item.ID);
                sortListDemo.find(".name").text(item.Name);
                sortListDemo.find("img").attr("src", item.Image);
                $(".sortListDemo").before(sortListDemo);

                var childDemo = $(".childDemo").clone().removeClass("childDemo");
                childDemo.attr("data-type", item.ID);
                $.each(item.Childs, function (ci, citem) {
                    var li = childDemo.find(".hidden").clone().removeClass("hidden");
                    li.find("a").attr("data-type", citem.ID);
                    li.find(".name").text(citem.Name);
                    li.find("img").attr("src", citem.Image);
                    childDemo.find(".hidden").before(li);
                });
                $(".childDemo").before(childDemo);
            });
            TypeClick();

            typeSwipe();

            if (Number(typeID) != 0) {
                $("#index").addClass("hidden");
                $("[name='type']").removeClass("active");
                $("[name='type'][data-type=" + typeID + "]").addClass("active");
                $("[name=sortList]").removeClass("hidden");
                $("[name=sortList]").children().addClass("hidden");
                $("[name=sortList]").find("[data-type=" + typeID + "]").removeClass("hidden");
            }

        }
    });
}
GetCouponType();
//分类点击事件
function TypeClick() {
    //1级分类切换
    var couponType = $("[name='type']");
    couponType.click(function () {
        couponType.removeClass("active");
        $(this).addClass("active");

        var date_type = $(this).data("type");
        typeID = date_type;

        if (date_type == "0") {
            $("#index").removeClass("hidden");
            $("[name=sortList]").addClass("hidden");
        }
        else {
            $("#index").addClass("hidden");
            $("[name=sortList]").removeClass("hidden");
            $("[name=sortList]").children().addClass("hidden");
            $("[name=sortList]").find("[data-type=" + date_type + "]").removeClass("hidden");
        }
        var $page = $("#coupon").find("ul li[data-page]");
        $page.data("next", "true");
        $page.data("page", "0");
        $("#coupon").find("li").not("[data-page]").remove();
        loadCoupon();

        comm.addHistory("url", comm.action("Index", "Coupon", {
            sort: sort,
            platform: platform,
            typeID: typeID,
        }));
    });

    var couponType2 = $("#sortOne [name='type']");
    couponType2.click(function () {
        var date_type = $(this).data("type");
        typeID = date_type;

        var $page = $("#coupon").find("ul li[data-page]");
        $page.data("next", "true");
        $page.data("page", "0");
        $("#coupon").find("li").not("[data-page]").remove();
        loadCoupon();

        couponType.removeClass("active");
        var index = $(this).index();
        couponType.eq(index).addClass("active");

        var swiper = new Swiper('.navigationSwiper .swiper-container', {
            slidesPerView: "auto",
            initialSlide: index
        });

        comm.addHistory("url", comm.action("Index", "Coupon", {
            sort: sort,
            platform: platform,
            typeID: typeID,
        }));

        if (date_type == "0") {
            $("#index").removeClass("hidden");
            $("[name=sortList]").addClass("hidden");
        } else {
            $("#index").addClass("hidden");
            $("[name=sortList]").removeClass("hidden");
            $("[name=sortList]").children().addClass("hidden");
            $("[name=sortList]").find("[data-type=" + date_type + "]").removeClass("hidden");
        }

        $("body").css("overflow", "auto");
        $("#sortOne").hide();
        comm.mask3();
    });

    //二级分类点击
    $("[name=childType]").click(function (e) {
        var date_type = $(this).data("type");
        typeID = date_type;
        location = comm.action("Second", "Coupon",
            {
                types: typeID,
                platform: platform
            });
    });
}

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
    $(".nodata").addClass("hidden");
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "Coupon"),
        data: {
            page: page,
            sort: sort,
            types: typeID,
            platforms: platform,
            orderByTime: true
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
            if ($coupon.find("li").length == 0) {
                nodataCheck("#coupon ul");
            }
        }
    });
}

//排序切换
if ($("#sort-down .sort").hasClass("active")) {
    $("#complex").addClass("active");
}

$(".sort").click(function (e) {
    sort = $(this).data("sort");
    var $page = $("#coupon").find("ul li[data-page]");
    $page.data("next", "true");
    $page.data("page", "0");
    $("#coupon").find("li").not("[data-page]").remove();
    loadCoupon();
    $(".sort").removeClass("active");
    $(this).addClass("active");
    $("#complex").removeClass("rotete");
    $("#sort-down").slideUp();
    if ($("#sort-down .sort").hasClass("active")) {
        $("#complex").addClass("active");
    } else {
        $("#complex").removeClass("active");
    }
    comm.addHistory("url", comm.action("Index", "Coupon", {
        sort: sort,
        platform: platform,
        typeID: typeID,
    }));
    $("#complex").find("t").text($(this).text());
});

$("#complex").click(function () {
    $("#sort-down").slideToggle();
    $(this).toggleClass("rotete");
});

//tabActive
if (platform == "" || platform == "0" || platform == "TaoBao") {
    $(".navTabBottom li.taobao").addClass("active");
} else if (platform == "4" || platform == "MoGuJie") {
    $(".navTabBottom li.mogujie").addClass("active");
} else {
    $(".navTabBottom li.jd").addClass("active");
}

$("#pullDown").click(function () {
    comm.mask3();
    $("body").css("overflow", "hidden");
    $("#sortOne").slideDown();
});