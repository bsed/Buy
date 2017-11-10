var canLoadPage = true;

//加载列表
loadCoupon()
function loadCoupon() {
    if (!canLoadPage) {
        return;
    }
    var $localCoupon = $("#localCoupon");
    var $page = $localCoupon.find("ul li[data-page]");
    if (!$page.data("next")) {
        return;
    }
    var page = parseInt($page.data("page")) + 1;
    canLoadPage = false;
    $.ajax({
        type: "GET",
        url: comm.action("GetList", "LocalCoupon"),
        data: {
            page: page,
            shopId: $("#ID").val()
        },
        dataType: "html",
        success: function (data) {
            $page.remove();
            var $data = $(data);
            $localCoupon.find("ul").append($data);
            comm.lazyloadALL();
        },
        complete: function () {
            canLoadPage = true;
        }
    });
}

$("#btnBack").goback(comm.action("Index", "Find"));


var share_name = $("#share_name").val();
var share_price = $("#share_price").val();
var share_url = $("#share_url").val();
var share_logo = $("#share_logo").val();
var share_time = $("#share_time").val();
var share_shop = $("#share_shop").val();
var shareImgModule = $("#shareImgModule");
var share_img = $("#shareImgModule_img");
var qrcode = $("#qrcode");
var platformLogo = $("#platformLogo");

//文案
var val = "【" + share_shop + "】" + share_name + "\n【券后价】" + share_price + "\n【有效期】" + share_time + "\n【下单链接】" + share_url;

$("#shareText").val(val);
$("#shareTextBtn").attrdata("clipboard-text", val);

//复制分享文案
var clipShareText = new Clipboard('#shareTextBtn');
clipShareText.on('success', function (e) {
    alert("复制成功！可粘贴文案");
    $("#shareText-tips").text("（文案已复制，可粘贴文案）");
});
clipShareText.on('error', function (e) {
    selectText("shareText");
    alert("*无法复制，请长按文案复制");
    $("#shareText-tips").text("（无法复制，请长按文案复制）");
});

$("#shareback").click(function (e) {
    $("#detail").removeClass("hidden");
    $("#share").addClass("hidden");
    $("#shareText-tips").text("");
});

$("#shareback").click(function (e) {
    $("#detail").removeClass("hidden");
    $("#share").addClass("hidden");
    $("#shareText-tips").text("");
});

$("#btnShare").click(function (e) {
    $("#detail").addClass("hidden");
    $("#share").removeClass("hidden");
    //判断
    if (!new check().isWeiXin()) {
        $("#sharePhoto-tips").text("长按保存图片")
    }

    //canvas图片
    var canvas = document.createElement("canvas");
    var cxt = canvas.getContext("2d");
    canvas.width = shareImgModule.width();
    canvas.height = shareImgModule.height();
    cxt.fillStyle = "#ffffff";
    cxt.fillRect(0, 0, shareImgModule.width(), shareImgModule.height());

    var share_img_i = document.getElementById('shareImgModule_img');
    var qrcode_i = document.getElementById('qrcode');
    var platformLogo_i = document.getElementById('platformLogo');
    cxt.fillStyle = "#ffffff";
    cxt.drawImage(share_img_i, 0, 0, share_img.width(), share_img.height());
    cxt.drawImage(qrcode_i, shareImgModule.width() - qrcode.width() - 5, share_img.width() + 5, qrcode.width(), qrcode.height());
    cxt.drawImage(platformLogo_i, 5, share_img.height() + 5, platformLogo.width(), platformLogo.height());

    var biaotword = share_name;
    var biaotword1 = biaotword.substring(0, 12);
    var biaotword2 = biaotword.substring(12, 22);
    cxt.font = "14px Helvetica Neue";
    cxt.fillStyle = "rgba(0,0,0,0.87)";
    cxt.fillText(biaotword1, platformLogo.width() + 9, share_img.height() + 20);
    cxt.fillText(biaotword2, 5, share_img.height() + 5 + 32);

    cxt.font = "12px Helvetica Neue";
    cxt.fillStyle = "#ff5913";
    cxt.fillText("长按识别二维码", shareImgModule.width() - qrcode.width() - 8, share_img.height() + qrcode.height() + 20);

    cxt.font = "12px Helvetica Neue";
    cxt.fillStyle = "#ff5913";
    cxt.fillText("¥", 5, share_img.height() + 75);

    cxt.font = "24px Helvetica Neue";
    cxt.fillStyle = "#ff5913";
    cxt.fillText(share_price, 15, share_img.height() + 75);

    cxt.font = "12px Helvetica Neue";
    cxt.fillStyle = "rgba(0,0,0,0.54)";
    cxt.fillText("有效期" + share_time, 5, share_img.height()+95);

    var url = canvas.toDataURL("image/jpeg");
    var img = new Image();
    img.src = url;
    document.getElementById('Output').appendChild(img);

    $("#shareImgModule").addClass("hidden");
});