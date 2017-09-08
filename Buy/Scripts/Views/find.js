$(".navTabBottom li.find").addClass("active");

//banner
var swiper = new Swiper('.find-banner .swiper-container', {
    slidesPerView: "auto",
    pagination: '.swiper-pagination',
    paginationClickable: true,
    autoplay: 2500,
    autoplayDisableOnInteraction: false
});