var platform = $("#platform").val(),
    sort = $("#sort").val(),
    filter = $("#searchConfirm").val();
var canLoadPage = true;
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

    $.ajax({
        type: "GET",
        url: comm.action("GetList", "coupon"),
        data: {
            page: page,
            sort: sort,
            platforms: platform,
            filter: filter,
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
            nodataCheck("#coupon ul");
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
    if ($("#search").val() != "") {
        $.ajax({
            type: "GET",
            url: comm.action("AutoComplate", "Coupon"),
            data: { keyword: $("#search").val() },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    $(".keywordTips").removeClass("hidden");
                    $("#SearchResult").children().not(".demo").remove();
                    $.each(data.Result, function (i, item) {
                        var t = item;
                        var val = $("#search").val();
                        var b = "<b>" + val + "</b>";
                        var d = t.replace(new RegExp(val, 'g'), b);
                        var demo = $("#SearchResult").find(".demo").clone().removeClass("demo hidden").append(d);
                        $(".demo").before(demo);
                    });
                }
            }
        });
    } else {
        $(".keywordTips").addClass("hidden");
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
function searchConfirm() {
    var state = true;
    var filterText = $("[name='filterText']").val();
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

$("#searchConfirm").bind('search', function () {
    searchConfirm();

    comm.addHistory("url", comm.action("SearchConfirm", "Coupon", {
        filter: filter,
        sort: sort,
        platform: platform,
    }));
});

$("#searchConfirmBtn").click(function () {
    searchConfirm();

    comm.addHistory("url", comm.action("SearchConfirm", "Coupon", {
        filter: filter,
        sort: sort,
        platform: platform,
    }));
});

//平台切换
$(".platform").click(function (e) {
    $(".platform").removeClass("active");
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
$(".sort").click(function (e) {
    $(".sort").removeClass("active");
    $(this).addClass("active");
    sort = $(this).data("sort");

    clear($('#coupon ul'));
    loadcoupon();
    comm.addHistory("url", comm.action("SearchConfirm", "Coupon", {
        filter: filter,
        sort: sort,
        platform: platform,
    }));
});

