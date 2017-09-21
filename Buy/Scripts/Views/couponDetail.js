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
        }
    }
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
        }, 300)
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
    var codeMessage = $("#codeMessage").val();
    if (codeMessage == "NotLogin") {
        location = comm.action("Login", "Account");
        return false;
    }
    if (codeMessage == "NotActivation") {
        location = comm.action("Activation", "Account");
        return false;
    }
    if (codeMessage == "NotOwnUser") {
        location = comm.action("Index", "Coupon");
        return false;
    }
    $.ajax({
        type: "GET",
        url: comm.action("GetPwd", "Coupon"),
        data: { id: $("#ID").val() },
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                $("#pwdMask-text").text(data.Result.Data);
                $(".pwdMask").attrdata("clipboard-text", data.Result.Data);
                $(".pwdMask").removeClass("hidden");
                $("body").css("overflow", "hidden");
                comm.mask2.show();
            }
        }
    });
});

//复制粘贴板
var clipboard = new Clipboard('.pwdMask');
clipboard.on('success', function (e) {
    $(".pwdMask").addClass("hidden");
    $("body").css("overflow", "auto");
    comm.mask2.hide();
    comm.promptBox("复制成功,请打开淘宝APP查看");
});
clipboard.on('error', function (e) {
    comm.promptBox("长按口令复制");
    $(".pwdMask").removeClass("hidden");
    $("body").css("overflow", "hidden");
    comm.mask2.show();
});




$("#btnShare").click(function (e) {
    $("#detail").addClass("hidden");
    $("#share").removeClass("hidden");

    if ($("#Output").children().length <= 0) {
        $.ajax({
            type: "GET",
            url: comm.action("GetPwd", "Coupon"),
            data: { id: $("#ID").val() },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    var share_name = $("#share_name").val();
                    var share_or_price = $("#share_or_price").val();
                    var share_price = $("#share_price").val();
                    var share_url = $("#share_url").val();
                    var val = share_name + "\n【在售价】" + share_or_price + "元\n【券后价】" + share_price + "元\n【下单链接】" + share_url + "\n-------------------------------------\n复制这条信息，{" + data.Result.Data + "}，打开【手机淘宝】即可查看"
                    $("textarea").val(val);

                    var img = document.getElementById('shareImgModule_img');
                    var data = getBase64Image(img);
                    $('#shareImgModule_img').prop("src", data);

                    html2canvas(document.getElementById("shareImgModule"), {
                        allowTaint: true,
                        taintTest: true,
                        useCORS: true,
                        onrendered: function (canvas) {
                            var url = canvas.toDataURL();
                            var img = new Image();
                            img.src = url;
                            document.getElementById('Output').appendChild(img);
                            $("#shareImgModule").addClass("hidden");
                        }
                    });
                }
            }
        });
    }
});

function getBase64Image(img) {
    var canvas = document.createElement("canvas");
    canvas.width = img.width;
    canvas.height = img.height;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0, img.width, img.height);
    var dataURL = canvas.toDataURL("image/png");
    return dataURL
}

$("#shareback").click(function (e) {
    $("#detail").removeClass("hidden");
    $("#share").addClass("hidden");
});
