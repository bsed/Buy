var searchUser = function (option) {
    var $target = $(option.target);
    if (option.click == undefined) {
        option.click = function () { };
    }
    if (option.changeing == undefined) {
        option.changeing = function () { };
    }
    var $resultUl = $("<ul>");
    $resultUl.addClass("autocomplete").addClass("hidden");
    $target.attr("autocomplete", "off");
    $target.after($resultUl);
    $target.keyup(function (e) {
        var filter = $(this).val().trim();
        option.changeing(filter);
        $resultUl.addClass("hidden").find("li").remove();
        if (filter == "") {
            return;
        }
        $.ajax({
            type: "POST",
            url: comm.action("SearchUsers", "Account"),
            data: { filter: filter },
            dataType: "json",
            success: function (data) {
                if (data.length > 0) {
                    $resultUl.removeClass("hidden");
                    $.each(data, function (i, n) {
                        var $li = $("<li>");
                        $li.text(n.UserName + "(" + n.NickName + ")");
                        $resultUl.append($li);
                        $li.click(function () {
                            $resultUl.addClass("hidden");
                            option.click(n);
                        });
                    });
                }


            }
        });
    });
}