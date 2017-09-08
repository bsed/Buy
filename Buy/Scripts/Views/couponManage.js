
$("[name=createTime]").date();

var check = comm.checkboxList("#chkAll", ".chkItem")

//筛选
var data = {
    filter: $("[name=filter]").val(),
    platform: $("[name=platform]").val(),
    typeid: $("[name=typeid]").val(),
    createTime: $("[name=createTime]").val()
};
$("#search_btn").click(function (e) {
    if ($("[name=filter]").val() != "") {
        data.filter = $("[name=filter]").val();
    }
    if ($("[name=createTime]").val() != "") {
        data.createTime = $("[name=createTime]").val();
    }
    location = comm.action("Index", "CouponManage", data);
});
$(".platform").click(function (e) {
    if ($(this).data("type") != undefined) {
        data.platform = $(this).data("type");
    }
    else {
        data.platform = null;
    }
    location = comm.action("Index", "CouponManage", data);
});
$(".typeid").click(function (e) {
    if ($(this).data("type") != undefined) {
        data.typeid = $(this).data("type");
    } else {
        data.platform = null;
    }
    location = comm.action("Index", "CouponManage", data);
});

//删除勾选优惠券
$("#btnDelTicket").click(function (e) {
    if (check.selectedValues().length <= 0) {
        comm.alter(2, "没有勾选商品");
        return false;
    }
    var ids = check.selectedValuesString(",");
    $.ajax({
        type: "POST",
        url: comm.action("DeleteTicket", "CouponManage"),
        data: { ids: ids },
        dataType: "json",
        success: function (data) {
            if (data.State == "Success") {
                comm.alter(1, data.Message);
                setTimeout(function () {
                    location = location;
                }, 1000);
            }
            else {
                comm.alter(0, data.Message);
            }
        }
    });
});

//批量删除优惠券
var modDel = function () {
    var $target = $("#modDel");
    var $btnHid = $target.find(".glyphicon-remove");
    var $mask = comm.mask2.create();
    var $chkPlatform = $target.find("[name=delPlatform]");
    var $dpDate = $target.find("#delDate");
    var $alterList = $(".panel-body-alterlist");
    var $btnDel = $target.find("#btnDel");
    $dpDate.date();

    $target.after($mask);
    function _hide() {
        $target.addClass("hidden");
        $mask.hide();
    }

    function _show() {
        $target.removeClass("hidden");
        $mask.show();
    }

    function _del() {
        var items = new Array();
        var dete = $dpDate.val();
        $.each($chkPlatform.filter(":checked"), function (i, n) {
            items.push($(n).val());
        });
        if (items.length == 0) {
            _alter(2, "未选择类型");
        } else if (dete == "") {
            _alter(2, "结束时间不能为空");
        }
        $.ajax({
            type: "POST",
            url: comm.action("Delete", "ThirdPartyTicketManage"),
            data: {
                date: dete,
                types: items
            },
            dataType: "json",
            success: function (data) {
                if (data.State == "Success") {
                    _alter(1, "删除成功");
                    setTimeout(function () {
                        location = location;
                    }, 1000);

                } else {
                    _alter(0, data.Message);
                }
            }
        });
    }

    function _alter(type, meg) {
        comm.alter(type, meg, null, $alterList);
    }

    this.hide = function () {
        _hide();
    }

    this.show = function () {
        _show();
    }

    $btnHid.click(function () {
        _hide();
    });

    $btnDel.click(function () {
        _del();
    });
}
var $modDel = new modDel();

$("#btnDelSetting").click(function () {
    $modDel.show();
});

