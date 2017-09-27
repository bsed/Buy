
var date = new Date();
var platform = $("#platform").val(),
    sort = $("[data-sort].active").data("sort"),
    maxPrice = $("#maxPrice").val(),
    filter = $("#filter").val(),
    time = date.getFullYear() + "-" + (date.getMonth() + 1) + "-"
     + date.getDate() + " " + date.getHours() + ":"
     + date.getMinutes() + ":" + date.getSeconds(),
    types = $("#types").val();
var canLoadPage = true;
var updateLoadTime = time;

var loading = $("#loading").attr("src");

function clear(target) {
    $(target).children().remove();
    var html = "";
    html += '<li class="loadModule loadModule-dataIng" data-page="0" data-next="true"><img class="marginR8" src="' + loading + '" />加载中</li>';
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
    $(".nodata").addClass("hidden");
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "coupon"),
        data: {
            page: page,
            sort: sort,
            platforms: platform,
            types: types,
            maxPrice: maxPrice,
            filter: filter,
            loadTime: time,
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
            if ($coupon.find("li").length == 0) {
                nodataCheck("#coupon ul");
            }
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
if ($("#sort-down .sort").hasClass("active")) {
    $("#complex").addClass("active");
    $("#complex").find("t").text($("#sort-down .sort.active").text());
}

$(".sort").click(function (e) {
    sort = $(this).data("sort");
    $(".sort").removeClass("active");
    $(this).addClass("active");
    $("#complex").removeClass("rotete");
    clear("#coupon ul");
    loadCoupon();
    $("#sort-down").slideUp();
    cleanUpdate();
    if ($("#sort-down .sort").hasClass("active")) {
        $("#complex").addClass("active");
        $("#complex").find("t").text($("#sort-down .sort.active").text());
    } else {
        $("#complex").removeClass("active");
        $("#complex").find("t").text("综合排序");
    }
    comm.addHistory("url", comm.action("Index", "Coupon", {
        sort: sort,
        platform: platform
    }));
});

$("#complex").click(function () {
    $("#sort-down").slideToggle();
    $(this).toggleClass("rotete");
    cleanUpdate()
});

//下拉刷新
function cleanUpdate() {
    $("#update1").hide();
    $("#update1 img").hide();
}

function kt_touch(contentId) {
    var update1 = $("#update1");
    var _start = 0,
        _end = 0,
        _content = document.getElementById(contentId);
    if (_content) {
        _content.addEventListener("touchstart", touchStart, false);
        _content.addEventListener("touchmove", touchMove, false);
        _content.addEventListener("touchend", touchEnd, false);
    }
    function touchStart(event) {
        //event.preventDefault();
        if (!event.touches.length) return;
        var touch = event.touches[0];
        _start = touch.pageY;
    }

    function touchMove(event) {
        // event.preventDefault();
        if (sort == "2") {
            if (!event.touches.length) return;
            var touch = event.touches[0];

            _end = (_start - touch.pageY);
            if (_end < 0) {
                update1.show();
                update1.find("span").text("松开刷新");
                update1.css("height", 1 - parseInt(_end) + "px");

            }
        }
    }

    function touchEnd(event) {
        if (sort == "2") {
            if (_end <= 0) {
                update1.find("img").show();
                update1.find("span").text("刷新中");
                $("#update1").animate({ height: '50' }, 150);
                Update();
            }
        }
    }
}
kt_touch('coupon');
function Update() {
    if (!canLoadPage) {
        return;
    }
    canLoadPage = false;
    var datetime = new Date();
    var updateTime = datetime.getFullYear() + "-" + (datetime.getMonth() + 1) + "-"
     + datetime.getDate() + " " + datetime.getHours() + ":"
     + datetime.getMinutes() + ":" + datetime.getSeconds();
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "Coupon"),
        data: {
            sort: sort,
            types: types,
            platforms: platform,
            isUpdate: true,
            loadTime: updateLoadTime,
            updateTime: updateTime,
            orderByTime: true
        },
        dataType: "html",
        success: function (data) {
            $("#update1").animate({ height: '0' }, 150);
            $("#update1").find("img").hide();
            updateLoadTime = updateTime;
            var $data = $(data);
            $("#coupon").find("ul").prepend($data);
            comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;
        }
    });
}