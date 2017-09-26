
var date = new Date();
var platform = $("#platform").val(),
    sort = $("#sort").val(),
    filter = $("#searchConfirm").val();
var canLoadPage = true;
var time = date.getFullYear() + "-" + (date.getMonth() + 1) + "-"
     + date.getDate() + " " + date.getHours() + ":"
     + date.getMinutes() + ":" + date.getSeconds();
var loading = $("#loading").attr("src");

function clear(target) {
    target.children().remove();
    var html = "";
    html += '<li class="loadModule loadModule-dataIng" data-page="0" data-next="true"><img class="marginR8" src="' + loading + '"/>加载中</li>';
    target.append(html);
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
function loadcoupon() {
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
            filter: filter,
            loadTime: time,
            orderByTime: true
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

loadcoupon();

$(window).scroll(function () {
    if (canLoadPage && comm.isWindowBottom()) {
        loadcoupon();
    }
});

//cookie
var cookie = $.cookie("searchHistory");
var array = new Array();
var clearHistory = $("#clearHistory");

//历史纪录加载
function getCookie() {
    if (cookie != undefined && cookie != "") {
        array = cookie.split(",")
    }
    if (cookie == "null") {
        array.length = 0;
    }
}

getCookie();

if (cookie != "null" && cookie != undefined) {
    $("#searchHistory").removeClass("hidden");
    var html = '';
    $.each(array, function (i, n) {
        html += '<li name="hotSearchList" data-filter="' + n + '">' + n + '</li>';
    })
    $("#searchHistoryList").append(html);
} else {
    $("#searchHistory").addClass("hidden");
}
//清空历史
clearHistory.click(function () {
    $.cookie('searchHistory', null, {
        path: "/", expires: 1
    });
    $("#searchHistoryList").children().remove();
    $("#searchHistory").addClass("hidden");
});

//搜索
function search(val) {
    var state = true;
    var filterText = val;
    filter = filterText;

    if (array[0] == "null") {
        array.length = 0;
    }

    if (array.length > 0) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == filter) {
                state = false
            }
        }

        if (state) {
            array = array.concat(filter);

            $.cookie("searchHistory", array.join(","), {
                path: "/", expires: 1
            });

            window.location.href = comm.webPath + "/coupon/SearchConfirm?filter=" + filter;
        } else {
            window.location.href = comm.webPath + "/coupon/SearchConfirm?filter=" + filter;
        }
    } else {
        array = array.concat(filter);

        $.cookie("searchHistory", array.join(","), {
            path: "/", expires: 1
        });

        window.location.href = comm.webPath + "/coupon/SearchConfirm?filter=" + filter;
    }

}

//搜索提示
$("#search").keyup(function (e) {
    if ($("#search").val().trim() != "") {
        $.ajax({
            type: "GET",
            url: comm.action("AutoComplate", "Coupon"),
            data: { keyword: $("#search").val() },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    $("#SearchResult").children().not(".demo").remove();
                    $.each(data.Result, function (i, item) {
                        var t = item;
                        var val = $("#search").val();
                        var b = "<b>" + val + "</b>";
                        var d = t.replace(new RegExp(val, 'g'), b);
                        var demo = $("#SearchResult").find(".demo").clone().removeClass("demo hidden").append(d);
                        $(".demo").before(demo);
                    });
                    $(".keywordTips").removeClass("hidden");
                    $(".hotSearch").addClass("hidden");

                    $("#SearchResult li").click(function () {
                        var filterText = $(this).text();
                        search(filterText);
                    });
                }
            }
        });
    } else {
        $(".keywordTips").addClass("hidden");
        $(".hotSearch").removeClass("hidden");
    }
});

$("#search").bind('search', function () {
    var filterText = $("[name='filterText']").val();
    search(filterText);
});

$("#searchBtn").click(function () {
    var filterText = $("[name='filterText']").val();
    search(filterText);
});

$("[name='hotSearchList']").click(function () {
    var filterText = $(this).data("filter");
    search(filterText);
});

//列表-搜索
function searchConfirm(val) {
    var state = true;
    var filterText = val;
    filter = filterText;

    if (array[0] == "null") {
        array.length = 0;
    }

    if (array.length > 0) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == filter) {
                state = false
            }
        }

        if (state) {
            array = array.concat(filter);

            $.cookie("searchHistory", array.join(","), {
                path: "/", expires: 1
            });

            clear($('#coupon ul'));
            loadcoupon();
        } else {
            clear($('#coupon ul'));
            loadcoupon();
        }
    } else {
        array = array.concat(filter);

        $.cookie("searchHistory", array.join(","), {
            path: "/", expires: 1
        });

        clear($('#coupon ul'));
        loadcoupon();
    }
}

//搜索提示
$("#searchConfirm").click(function () {
    window.location.href = comm.action("Search", "Coupon");
});

$("#searchConfirmBtn").click(function () {
    window.location.href = comm.action("Search", "Coupon");
});

//平台切换
$(".platformLi").click(function (e) {
    $(".platformLi").removeClass("active");
    $(this).addClass("active");
    platform = $(this).data("platform");

    clear($('#coupon ul'));

    loadcoupon();
    comm.addHistory("url", comm.action("SearchConfirm", "Coupon", {
        filter: filter,
        sort: sort,
        platform: platform,
    }));
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
    clear($('#coupon ul'));
    loadcoupon();
    $("#sort-down").slideUp();
    if ($("#sort-down .sort").hasClass("active")) {
        $("#complex").addClass("active");
        $("#complex").find("t").text($("#sort-down .sort.active").text());
    } else {
        $("#complex").removeClass("active");
        $("#complex").find("t").text("综合排序");
    }
    comm.addHistory("url", comm.action("SearchConfirm", "Coupon", {
        filter: filter,
        sort: sort,
        platform: platform,
    }));
});

$("#complex").click(function () {
    $("#sort-down").slideToggle();
    $(this).toggleClass("rotete");
});