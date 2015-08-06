function CheckCheckout(flag) {
    var check = true;
    $("input, textarea", ".validationWrapper").each(function () {
        var value = $(this).val();
        if (value == "") {
            check = false;
            $(this).parents(".form-group").addClass("has-error");
        } else {
            $(this).parents(".form-group").removeClass("has-error");
        }
    });

    if ($('input.onCheckbox:checked').length > 0) {
        $("#loaiticket").addClass("hide");
    } else {
        check = false;
        $("#loaiticket").removeClass("hide");
    }
    getValueTableEdit();
    return check;
}

$(function () {
    $('#TicketStatusChange').change(function () {
        var id = $(this).val();
        location.href = "/RequestBill/Index?status=" + id;
    });

    $('.YearMonthChange').change(function () {
        var y = $("#tk-year").val();
        var m = $("#tk-month").val();
        location.href = "/RequestBill/ThongKe?y=" + y + "&m=" + m;
    });
});

function TinhTien() {
    var vndCong = 0;
    $(".vndetail").each(function () {
        var value = parseFloat($(this).children().val()) || 0;
        vndCong += value;
    });
    var tamungVND = parseFloat($(".vndTamung").children().val()) || 0;
    var phiNHVND = parseFloat($(".vndPhiNH").children().val()) || 0;
    var vndTongCong = vndCong - tamungVND + phiNHVND;
    $(".vndCong").text(formatMoney(vndCong, ''));
    $(".vndTongCong").text(formatMoney(vndTongCong, ''));

    var usdCong = 0;
    $(".usdetail").each(function () {
        var value = parseFloat($(this).children().val()) || 0;
        usdCong += value;
    });

    var usdTamung = parseFloat($(".usdTamung").children().val()) || 0;
    var usdPhiNH = parseFloat($(".usdPhiNH").children().val()) || 0;
    var usdTongCong = usdCong - usdTamung + usdPhiNH;
    $(".usdCong").text(formatMoney(usdCong, ''));
    $(".usdTongCong").text(formatMoney(usdTongCong, ''));
}

function getValueTableEdit() {
    var listCheckoutDetailJson = [];
    $("table#mainTable tbody tr.trtableedit").each(function () {
        var listtds = $(this).children();
        var obj = {
            DeptCode: $(listtds[0]).children().val(),
            No: $(listtds[1]).children().val(),
            Date: $(listtds[2]).children().val(),
            Title: $(listtds[3]).children().val(),
            VND: $(listtds[4]).children().val(),
            USD: $(listtds[5]).children().val()
        };
        listCheckoutDetailJson.push(obj);
    });
    $("#listCheckoutDetailJson").val(JSON.stringify(listCheckoutDetailJson));

    var tamung = {
        VND: $("td.vndTamung").children().val(),
        USD: $("td.usdTamung").children().val()
    };
    $("#tamUng").val(JSON.stringify(tamung));

    var phiNganhang = {
        VND: $("td.vndPhiNH").children().val(),
        USD: $("td.usdPhiNH").children().val()
    };
    $("#phiNganHang").val(JSON.stringify(phiNganhang));

    $("#InWords").val($("#BangChu").children().val());
}