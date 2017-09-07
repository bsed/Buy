//swiper
var slideIndexHead = $("[name='type'].active").index();
var swiper = new Swiper('.navigationSwiper .swiper-container', {
    slidesPerView: "auto",
    initialSlide: slideIndexHead
});

//主导航
var swiper = new Swiper('.couponIndex-banner .swiper-container', {
    slidesPerView: "auto",
    pagination: '.swiper-pagination',
    paginationClickable: true,
    autoplay: 2500,
    autoplayDisableOnInteraction: false
});

//2级分类查看全部
var sortGetAll = $("#sortGetAll");

sortGetAll.click(function () {
    $("[name='sortList']").addClass("getAll");
    comm.mask3();
});

$(".mask.style02").click(function () {
    $("[name='sortList']").removeClass("getAll");
    comm.mask3();
});

//1级分类切换
var couponType = $("[name='type']");

function clear() {
    $("#index").addClass("hidden");
    $("[name='sortList']").addClass("hidden");
}

couponType.click(function () {
    var date_type = $(this).data("type");
    couponType.removeClass("active");
    $(this).addClass("active");
    clear();

    if (date_type == "-1") {
        $("#index").removeClass("hidden");
    } else {
        $("[name='sortList']").removeClass("hidden");
    }
});