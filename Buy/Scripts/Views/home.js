var sum = 0;

$(".app").addClass("active");

$(window).on('mousewheel', function (event, delta) {
    sum = sum + delta;

    if (sum <= -2) {
        var itemAct = $("#slideBox .active");

        if (itemAct.hasClass("app")) {
            itemAct.next().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -100%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        } else if ($("#slideBox .active").hasClass("coupon")) {
            itemAct.next().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -200%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        } else if ($("#slideBox .active").hasClass("jm")) {
            itemAct.next().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -300%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        }
        //else if ($("#slideBox .active").hasClass("yj")) {
        //    itemAct.next().addClass("active");
        //    itemAct.removeClass("active");
        //    $("#slideBox").css("transform", "translate3d(0px, -400%, 0px)");
        //    sum = 0;

        //    var index = $("#slideBox .active").index();
        //    $("#nav li").removeClass("active");
        //    $("#nav li").eq(index).addClass("active");
        //}
        else if ($("#slideBox .active").hasClass("hz")) {
            sum = 0;
        }
    }

    if (sum >= 2) {
        var itemAct = $("#slideBox .active");

        if (itemAct.hasClass("app")) {
            sum = 0;
        } else if ($("#slideBox .active").hasClass("coupon")) {
            itemAct.prev().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, 0%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        } else if ($("#slideBox .active").hasClass("jm")) {
            itemAct.prev().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -100%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        }
        //else if ($("#slideBox .active").hasClass("yj")) {
        //    itemAct.prev().addClass("active");
        //    itemAct.removeClass("active");
        //    $("#slideBox").css("transform", "translate3d(0px, -200%, 0px)");
        //    sum = 0;

        //    var index = $("#slideBox .active").index();
        //    $("#nav li").removeClass("active");
        //    $("#nav li").eq(index).addClass("active");
        //}
        else if ($("#slideBox .active").hasClass("hz")) {
            itemAct.prev().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -200%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        }
    }

    if ($("#slideBox .active").hasClass("app")) {
        if (sum > 0) {
            sum = 0;
        }
    }

    if ($("#slideBox .active").hasClass("hz")) {
        if (sum < 0) {
            sum = 0;
        }
    }
});

$("#nav li").click(function () {
    $("#nav li").removeClass("active");
    $(this).addClass("active");
    var index = $(this).index();

    $("#slideBox .homeIndex-slide-item").removeClass("active");
    $("#slideBox .homeIndex-slide-item").eq(index).addClass("active");

    if ($("#slideBox .active").hasClass("app")) {
        $("#slideBox").css("transform", "translate3d(0px, 0%, 0px)");
        sum = 0;
    } else if ($("#slideBox .active").hasClass("coupon")) {
        $("#slideBox").css("transform", "translate3d(0px, -100%, 0px)");
        sum = 0;
    } else if ($("#slideBox .active").hasClass("jm")) {
        $("#slideBox").css("transform", "translate3d(0px, -200%, 0px)");
        sum = 0;
    } else if ($("#slideBox .active").hasClass("yj")) {
        $("#slideBox").css("transform", "translate3d(0px, -300%, 0px)");
        sum = 0;
    } else if ($("#slideBox .active").hasClass("hz")) {
        $("#slideBox").css("transform", "translate3d(0px, -300%, 0px)");
        sum = 0;
    }
});

//pc滚动向上
var loop2 = true;
var scrollUpListLength = $("#scrollUpList li").length;
var scrollUpListLiHeight = $("#scrollUpList li").height();
if (scrollUpListLength < 15) {
    loop2 = false;
}

function autoScrolls(obj) {
    if (loop2) {
        if ($(obj).hasClass("mobile")) {
            $(obj).find("#scrollUpList").animate({
                marginTop: -scrollUpListLiHeight
            }, 500, function () {
                $(this).css({ marginTop: "0px" }).find("li:first").appendTo(this);
            });
        } else {
            $(obj).find("#scrollUpList").animate({
                marginTop: "-45px"
            }, 500, function () {
                $(this).css({ marginTop: "0px" }).find("li:lt(2)").appendTo(this);
            });
        }
    }
}

$("#scrollUpList").mouseover(function () {
    loop2 = false;
});
$("#scrollUpList").mouseout(function () {
    loop2 = true;
});

setInterval('autoScrolls("#slideBox")', 3000);


//移动端js
var swiper = new Swiper('.homeIndexM-swiper .swiper-container', {
    direction: 'vertical'
});

var android_apk = $("#android_apk").attr("href");
//安卓包
$("#android_download").click(function () {
    if (new check().isWeiXin()) {
        comm.mask();
        $("#download-tips").show();
        $(".linkToCoupon").hide();
    } else {
        location.href = comm.webPath + android_apk;
    }
});

$("#download-tips").click(function () {
    comm.mask();
    $("#download-tips").hide();
    $(".linkToCoupon").show();
});

if (new check().isMoblieDevice()) {
    if (new check().isWeiXin()) {
        comm.mask();
        $("#download-tips").show();
        $(".linkToCoupon").hide();
    } else {
        location.href = comm.webPath + android_apk;
    }
}