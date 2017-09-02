var getMessage = function () {
    if (window.location.href.indexOf("Account/Login") < 0) {
        var $systemMsg = $(".systemMsg");
        var $btntext = $systemMsg.find(".buttonText");
        var $closed = $systemMsg.find(".closed");
        if ($systemMsg.length == 0) {
            return;
        }
        $.ajax({
            type: "Get",
            url: comm.action("GetMessage", "System"),
            dataType: "json",
            success: function (data) {
                var msg = data.WebData;
                if (msg.ID != null && msg.ID != "") {
                    var cookie = $.cookie('systemMessage') == undefined ? null : new Date($.cookie("systemMessage"));
                    var msgid = new Date(msg.ID);
                    if (cookie == null || cookie < msgid) {
                        if (msg.Img != null) {
                            $systemMsg.find(".image").attr("src", msg.Img);
                        } else {
                            $systemMsg.find(".title").removeClass("hidden").text(msg.Title);
                        }
                        $btntext.text(msg.ButtonText);
                        $("body").css("overflow", "hidden");
                        $systemMsg.removeClass("hidden");
                        comm.mask2.show();
                        $btntext.click(function (e) {
                            $.cookie("systemMessage", msg.ID, { expires: 365, path: "/" });
                            location = msg.Url;
                        });
                        $closed.click(function (e) {
                            $.cookie("systemMessage", msg.ID, { expires: 365, path: "/" });
                            $systemMsg.addClass("hidden");
                            $("body").css("overflow", "auto");
                            comm.mask2.hide();
                        });
                    }
                }
            }
        });
    }
}