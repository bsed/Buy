
var checked = comm.checkboxList("#chkAll", "[name=chkItem]")

var typeTree = function () {
    var $target = $(".couponTicketTypeManageIndex-tree");

    function isMove() {
        return $target.hasClass("move");
    }

    var _moveMode = function (pid) {
        $target.addClass("move");
    }

    $target.find("a").click(function () {
        return !isMove();
    });;
    var $btnEdit = $target.find("#btnEditMode");
    $.each($target.find("a"), function (i, n) {
        var $item = $(n);
        $item.attr("href", comm.action("Index", "CouponTypeManage", { pid: $item.data("id") }))
    });

    var $sort = $target.find(">ul ul").sortable({
        connectWith: $target.find(">ul ul"),
        disabled: true,
        update: function (e, ui) {
            $.each($target.find("ul"), function (i, n) {
                var ids = new Array();
                $.each(ui.item.parent().children(), function (i, n) {
                    ids.push($(n).data("id"));
                });
                var data = {
                    ids: ids,
                    pid: ui.item.parent().parent().data("id"),
                };


                //刷新箭头
                var hasChild = $(n).children().length > 0;
                var sp = $(n).prev("div").find("span");
                if (hasChild) {
                    sp.removeClass("vhidden");
                } else {
                    sp.addClass("vhidden");
                }

                $.ajax({
                    type: "POST",
                    url: comm.action("Move", "CouponTypeManage"),
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        console.log(data.CyState);
                    }
                });
            });

        }
    });


    $btnEdit.click(function (e) {
        $target.toggleClass("move");
        $sort.sortable({ disabled: !isMove() });
        if (!isMove()) {
            location = location;
        }
    });



}
var tree = new typeTree();
