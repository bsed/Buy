var platform = "",
    sort ="" ,
    filter = $("#searchConfirm").val();
var canLoadPage = true;

function clear(target) {
    target.children().remove();
    var html = "";
    html += '<li class="loadModule loadModule-dataIng" data-page="0" data-next="true">加载中</li>';
    target.append(html);
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
            orderByTime: false
        },
        dataType: "html",
        success: function (data) {
            $page.remove();
            var $data = $(data);

            $coupon.find(">ul").append($data);
            //comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;
        }
    });

}

loadcoupon();

//cookie
var cookie = $.cookie("searchHistory");
var array = new Array();
var clearHistory = $("#clearHistory");

//历史纪录加载
function getCookie() {
    if (cookie != undefined && cookie != "") {
        array = cookie.split(",")
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
    $.cookie('searchHistory', null);
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
});

$("#searchConfirmBtn").click(function () {
    searchConfirm();
});

//平台切换
$(".platform").click(function (e) {
    $(".platform").removeClass("active");
    $(this).addClass("active");
    sort = $(this).data("platform");

    clear($('#coupon ul'));

    loadcoupon();
});

//排序切换
$(".sort").click(function (e) {
    $(".sort").removeClass("active");
    $(this).addClass("active");
    sort = $(this).data("sort");

    clear($('#coupon ul'));

    loadcoupon();
});