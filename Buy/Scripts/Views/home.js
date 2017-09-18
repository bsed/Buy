var sum = 0;

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
        } else if ($("#slideBox .active").hasClass("yj")) {
            itemAct.next().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -400%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        } else if ($("#slideBox .active").hasClass("hz")) {
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
        } else if ($("#slideBox .active").hasClass("yj")) {
            itemAct.prev().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -200%, 0px)");
            sum = 0;

            var index = $("#slideBox .active").index();
            $("#nav li").removeClass("active");
            $("#nav li").eq(index).addClass("active");
        } else if ($("#slideBox .active").hasClass("hz")) {
            itemAct.prev().addClass("active");
            itemAct.removeClass("active");
            $("#slideBox").css("transform", "translate3d(0px, -300%, 0px)");
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
        $("#slideBox").css("transform", "translate3d(0px, -400%, 0px)");
        sum = 0;
    }
});