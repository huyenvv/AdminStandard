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
});

function TinhTien() {
    var vndCong = 0;
    $(".vndetail").each(function () {
        var value = parseFloat($(this).text()) || 0;
        vndCong += value;
    });
    var tamungVND = parseFloat($(".vndTamung").text()) || 0;
    var phiNHVND = parseFloat($(".vndPhiNH").text()) || 0;
    var vndTongCong = vndCong - tamungVND + phiNHVND;
    $(".vndCong").text(formatMoney(vndCong, ''));
    $(".vndTongCong").text(formatMoney(vndTongCong, ''));

    var usdCong = 0;
    $(".usdetail").each(function () {
        var value = parseFloat($(this).text()) || 0;
        usdCong += value;
    });

    var usdTamung = parseFloat($(".usdTamung").text()) || 0;
    var usdPhiNH = parseFloat($(".usdPhiNH").text()) || 0;
    var usdTongCong = usdCong - usdTamung + usdPhiNH;
    $(".usdCong").text(formatMoney(usdCong, ''));
    $(".usdTongCong").text(formatMoney(usdTongCong, ''));
}

function getValueTableEdit() {
    var listCheckoutDetailJson = [];
    $("table#mainTable tbody tr").each(function () {
        var listtds = $(this).children();
        var obj = {
            Title: $(listtds[3]).text(),
            VND: $(listtds[4]).text(),
            USD: $(listtds[5]).text()
        };
        listCheckoutDetailJson.push(obj);
    });
    $("#listCheckoutDetailJson").val(JSON.stringify(listCheckoutDetailJson));

    var tamung = {
        VND: $("td.vndTamung").text(),
        USD: $("td.usdTamung").text()
    };
    $("#tamUng").val(JSON.stringify(tamung));

    var phiNganhang = {
        VND: $("td.vndPhiNH").text(),
        USD: $("td.usdPhiNH").text()
    };
    $("#phiNganHang").val(JSON.stringify(phiNganhang));
}