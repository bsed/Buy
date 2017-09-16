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
                        var dttop = $("#detailTit").offset().top;
                        var dth = $("#detailTit").height();
                        $.each(data.Result.Data, function (i, n) {
                            html += '<img src="' + lazyloadimg + '" data-original="' + n + '" />';
                        })
                        $("[name=poductDetail]").append(html);
                        comm.lazyloadALL();

                        if ($("[name='poductDetail'] img").length > 0) {
                            setTimeout(function () {
                                $('body').animate({ scrollTop: Math.floor(dttop + dth) }, 600);
                                $("#pullUpLoad").addClass("style02").text("收回商品详情");
                                scState = true;
                            }, 700)
                        }
                    }
                }
            }
        });
    }
}

var poffSetTop = $("#detailTit").offset().top;
var pHeight = $("#detailTit").height();
var dVal = $("body").height() - $(window).height();
var pState = false;
var scState = false;

if ($("body").height() == $(window).height()) {
    pState = true;
}

$(window).scroll(function (e) {
    if ($(window).scrollTop() == dVal && dVal != 0) {
        pState = true;
    }
    if ($(window).scrollTop() >= Math.floor(poffSetTop + pHeight)) {
        pState = false;
        scState = true;
        $("#pullUpLoad").fadeIn();
    } else {
        if (scState) {
            $("#pullUpLoad").fadeOut();
            pState = false;
        }    }
});

$("#pullUpLoad").click(function () {
    if (scState) {
        $('body').animate({ scrollTop: 0 }, 600);
    }
});

$("#detail").rhuiSwipe('swipeUp', function (event) {
    if (pState) {
        $("#pullUpLoad span").addClass("active");
        setTimeout(function () {
            $("#pullUpLoad span").addClass("hidden");
            $("#pullUpLoad .loading").removeClass("hidden");
        },300)
        GetDetailImgs();
    }
}, {
    // 可选参数
    isStopPropagation: false,
    isPreventDefault: false,
    triggerOnMove: false
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

var share_name = $("#share_name").val();
var share_or_price = $("#share_or_price").val();
var share_price = $("#share_price").val();
var share_url = $("#share_url").val();
var val = share_name + "\n【在售价】" + share_or_price + "元\n【券后价】" + share_price + "元\n【下单链接】" + share_url + "\n-------------------------------------\n复制这条信息，{淘口令}，打开【手机淘宝】即可查看"
$("textarea").val(val);