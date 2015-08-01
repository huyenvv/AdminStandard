$(function () {
    $('#ThemChiTiet').click(function () {

        var check = true;
        var soluong = $("#Quantity").val();
        $("input, textarea", "#frmChiTietYeuCau").each(function myfunction() {
            var value = $(this).val();
            if (value == "" || (this.id == "Quantity" && parseInt(value) <= 0)) {
                check = false;
                $(this).addClass("input-error");
            } else {
                $(this).removeClass("input-error");
            }
        });
        if (check) {
            $("#messerror").addClass("hide");
            var no = $("table tbody tr").length + 1;
            var dienGiai = $("#DienGiai").val();
            var mucDich = $("#MucDich").val();
            var ngayCan = $("#NgayCan").val();
            var data = {
                Id: no,
                DienGiai: dienGiai,
                SoLuong: soluong,
                LyDo: mucDich,
                NgayCan: ngayCan
            };
            $.post("/Ticket/AddTicketDetail", data)
              .done(function (ok) {
                  $("table tbody").append("<tr><td>" + no + "</td><td>" + dienGiai + "</td><td>" + soluong + "</td><td>" + mucDich + "</td><td>" + ngayCan + "</td><td><a href='#' class='color-red' onclick='return removeTicketDetail(" + no + ",this);'><i class='fa fa-times'></i></a></td></tr>");
              });
        } else {
            $("#messerror").removeClass("hide");
        }
        return false;
    });



    $('input.inlineCheckbox1').on('change', function () {
        $('input.inlineCheckbox1').not(this).prop('checked', false);
    });

});
function removeTicketDetail(id, e) {
    if (confirm("Bạn chắc chắn?")) {
        $.getJSON("/Ticket/DeleteTicketDetail?id=" + id, function (data) {
            $(e).parents("tr").remove();
        });
    }
    return false;
}

function CheckTicket() {
    if ($('input.inlineCheckbox1:checked').length > 0) {
        $("#loaiticket").addClass("hide");
        if ($("table tbody tr").length > 0) {
            $("#loaiticketDetail").addClass("hide");
            return true;
        }
        $("#loaiticketDetail").removeClass("hide");
    } else {
        $("#loaiticket").removeClass("hide");
    }
    return false;
}
