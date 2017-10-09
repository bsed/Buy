var shopId = $("#shopId").val();
var canLoadPage = true;
var loading = $("#loading").attr("src");
var canLoadPage = true;


$(".navTabBottom li.find").addClass("active");

//banner
var swiper = new Swiper('.find-banner .swiper-container', {
    slidesPerView: "auto",
    pagination: '.swiper-pagination',
    paginationClickable: true,
    autoplay: 2500,
    autoplayDisableOnInteraction: false,
    loop:true
});

function clear(target) {
    $(target).children().remove();
    var html = "";
    html += '<li class="loadModule loadModule-dataIng" data-page="0" data-next="true"><img class="marginR8" src="' + loading + '"/>加载中</li>';
    $(target).append(html);
}

//swiper
function typeSwipe() {
    var slideIndexHead = $("[name='type'].active").index();
    var swiper = new Swiper('.navigationSwiper .swiper-container', {
        slidesPerView: "auto",
        initialSlide: slideIndexHead
    });
}

//获取分类
function GetCouponType() {
    $.ajax({
        type: "GET",
        url: comm.action("GetShop", "LocalCoupon"),
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
                sortListDemo.find("img").attr("src", item.Logo);
                $(".sortListDemo").before(sortListDemo);
            });

            TypeClick();
            typeSwipe();

            if (Number(shopId) != "") {
                $("[name='type']").removeClass("active");
                $("[name='type'][data-type=" + shopId + "]").addClass("active");
                $("[name=sortList]").removeClass("hidden");
                $("[name=sortList]").children().addClass("hidden");
                $("[name=sortList]").find("[data-type=" + shopId + "]").removeClass("hidden");
            } else {
                $("[name='type']").first().addClass("active");
                shopId = $("[name='type']").first().data("type");
            }

            loadCoupon();

        }
    });
}
GetCouponType();

function TypeClick() {
    //1级分类切换
    var couponType = $("[name='type']");
    couponType.click(function () {
        couponType.removeClass("active");
        $(this).addClass("active");

        var date_type = $(this).data("type");
        shopId = date_type;
        clear("#localCoupon ul");
        loadCoupon();

        comm.addHistory("url", comm.action("Index", "Find", {
            shopId: shopId
        }));
    });

    var couponType2 = $("#sortOne [name='type']");
    couponType2.click(function () {
        couponType.removeClass("active");
        var index = $(this).index();
        couponType.eq(index).addClass("active");
        var date_type = $(this).data("type");
        shopId = date_type;
        clear("#localCoupon ul");
        loadCoupon();

        var swiper = new Swiper('.navigationSwiper .swiper-container', {
            slidesPerView: "auto",
            initialSlide: index
        });

        comm.addHistory("url", comm.action("Index", "Find", {
            shopId: shopId
        }));

        $("body").css("overflow", "auto");
        $("#sortOne").hide();
        comm.mask3();
    });
}

//加载列表
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $coupon = $("#localCoupon");
    var $page = $coupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = parseInt($page.data("page")) + 1;
    canLoadPage = false;
    $(".nodata").addClass("hidden");
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "LocalCoupon"),
        data: {
            page: page,
            shopId: shopId
        },
        dataType: "html",
        success: function (data) {
            //$page.remove();
            
            var $data = $(data);
            if ($data.length > 0) {
                $("#localCoupon li[data-page]").remove();
            }
            $coupon.find("ul").append($data);
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

$("#pullDown").click(function () {
    comm.mask3();
    $("body").css("overflow", "hidden");
    $("#sortOne").slideDown();
    $("#sort-down").hide();
});

$("#sortList_closed").click(function () {
    $("#sortOne").slideUp();
    $("body").css("overflow", "auto");
    comm.mask3();
});

//滑动
$(window).scroll(function (e) {
    if (canLoadPage && comm.isWindowBottom()) {
        loadCoupon();
    }

});