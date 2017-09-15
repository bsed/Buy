$("#btnBack").goback(comm.action("Index", "Coupon"));

if ($("#ID").length > 0) {
    function GetDetailImgs() {
        $.ajax({
            type: "POST",
            url: comm.action("GetDetailImgs", "Coupon"),
            data: { id: $("#ID").val() },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    var html = "";
                    if (data.Result.Data.length > 0) {
                        var lazyloadimg = $("#lazyloadimg").attr("src");
                        $.each(data.Result.Data, function (i, n) {
                            html += '<img src="' + lazyloadimg + '" data-original="' + n + '" />';
                        })
                        $("[name=poductDetail]").append(html);
                        comm.lazyloadALL();
                    }
                }
            }
        });
    }
    //if ($("[name=poductDetail]").find("img").length <= 0) {
    //    GetDetailImgs();
    //}
}

var poffSetTop = $("#detailTit").offset().top;
var pHeight = $("#detailTit").height();
var bodyHeight=$("body").height();
var pState = true;

//$("#pullUpLoad").click(function () {
//    if (!$(this).hasClass("style02")) {
//        $('body').animate({ scrollTop: Math.floor(poffSetTop + pHeight) }, 600);
//        GetDetailImgs();
//    } else {
//        $('body').animate({ scrollTop: 0 }, 600);
//    }
//});

$(window).scroll(function (e) {

    if ($(window).scrollTop() >= Math.floor(poffSetTop + pHeight)) {
        if (pState) {
            $("#pullUpLoad").addClass("style02").text("收回商品详情");
            $("#pullUpLoad").fadeIn();
            pState = false;
        }
    } else {
        if (!pState) {
            $("#pullUpLoad").fadeOut();
            pState = true;
        }
    }
});

$(".couponDetail").rhuiSwipe('swipeUp', function (event) {
    if (!pState) {
        $('body').animate({ scrollTop: Math.floor(poffSetTop + pHeight) }, 600);
        $("#pullUpLoad").addClass("style02").text("收回商品详情");
        GetDetailImgs();
    }
}, {
    // 可选参数
    isStopPropagation: true,
    isPreventDefault: true,
    triggerOnMove: true
});

//淘口令
$(".mask").click(function (e) {
    $(".pwdMask").addClass("hidden");
    $("body").css("overflow", "auto");
    comm.mask2.hide();
});

$("[name='clipboard']").click(function (e) {
    $(".pwdMask").removeClass("hidden");
    $("body").css("overflow", "hidden");
    comm.mask2.show();
});