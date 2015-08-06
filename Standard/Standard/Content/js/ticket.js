//$(document).ready(function () {
//    notify('Welcome back Mallinda Hollaway', 'inverse');
//});
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
            var id = 10000 + no;
            var dienGiai = $("#DienGiai").val();
            var mucDich = $("#MucDich").val();
            var ngayCan = $("#NgayCan").val();
            var data = {
                ID: id,
                Title: dienGiai,
                Quantity: soluong,
                Reason: mucDich,
                DateRequire: ngayCan
            };
            $.post("/Ticket/AddTicketDetail", data)
              .done(function (ok) {
                  $("table").removeClass("hide");
                  $("table tbody").append("<tr><td>" + no + "</td><td>" + dienGiai + "</td><td>" + soluong + "</td><td>" + mucDich + "</td><td>" + ngayCan + "</td><td>" +
                      "<button class='color-red btn btn-icon' onclick='return removeTicketDetail(" + id + ",this);'><span class='md md-delete'></span></button>" +
                      "</td></tr>");
              });
        } else {
            $("#messerror").removeClass("hide");
        }
        return false;
    });


    $('#PhanHoi').click(function () {
        var ykien = $("#txtPhanHoi").val();
        location.href = "/Ticket/PhanHoi?ykien=" + ykien
    });

    $('#TicketStatusChange').change(function () {
        var id = $(this).val();
        location.href = "/Ticket/Index?status=" + id;
    });
    $('.YearMonthChange').change(function () {
        var y = $("#tk-year").val();
        var m = $("#tk-month").val();
        location.href = "/Ticket/ThongKe?y=" + y + "&m=" + m;
    });
});

function addAttachFile() {
    $("#ListFile").append('<div class="fileinput fileinput-new" data-provides="fileinput"><span class="btn-file m-r-10 waves-effect"><a href="#" class="fileinput-new"><i class="md md-attach-file"></i>Đính kèm tệp</a><input type="hidden"><input type="file" name="files" class="ticketFiles"></span><a href="#" class="fileinput-filename"></a><a href="#" class="close fileinput-exists" data-dismiss="fileinput" onclick="removeAttachFile(this);">×</a></div><div class="clearfix"></div>');
    return false;
}
function removeAttachFile(obj) {
    $(obj).parents("div.fileinput").remove();
    return false;
}

function removeTicketDetail(id, e) {
    if (confirm("Bạn chắc chắn?")) {
        $.getJSON("/Ticket/DeleteTicketDetail?id=" + id, function (data) {
            $(e).parents("tr").remove();
            if ($("table tbody tr").length == 0) {
                $("table").addClass("hide");
            }
        });
    }
    return false;
}

function CheckTicket(flag) {
    if ($('input.onCheckbox:checked').length > 0) {
        $("#loaiticket").addClass("hide");
        if ($("table tbody tr").length > 0) {
            $("#loaiticketDetail").addClass("hide");
            $("#isSend").val(flag);

            //before send create ticket details
            GetDataTable('#mainTable');
            return true;
        }
        $("#loaiticketDetail").removeClass("hide");
    } else {
        $("#loaiticket").removeClass("hide");
    }
    return false;
}
function showFile(url) {
    var root = location.protocol + '//' + location.host;
    var splitFile = url.split(".");
    var extension = splitFile[splitFile.length - 1];
    switch (extension) {
        case 'pdf':
        case 'doc':
        case 'docx':
            window.open("https://docs.google.com/gview?url=" + root + url);
            break;
        default:
            window.open(root + url);
    }
    return false;
}



function AddRows(id) {
    var obj = $(id + " tbody");
    var no = obj.find("tr.trtableedit").length + 1;
    obj.append('<tr class="trtableedit"><td class="noedit">' + no + '</td><td><input type="text" class="edittable focus"></td><td class="typeNumber"><input type="text" class="edittable typeNumber"></td><td><input type="text" class="edittable"></td></td><td class="typeDate"><input type="text" class="edittable typeDate"></td></td></tr>');
    $('.typeDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    $('#mainTable input.focus').focus();
}

function GetDataTable(id) {
    var data = [];
    $(id + " tbody tr.trtableedit").each(function () {
        var listtds = $(this).children();
        var title = $(listtds[1]).children().val(),
            quantity = $(listtds[2]).children().val(),
            reason = $(listtds[3]).children().val(),
            dateRequire = $(listtds[4]).children().val();
        if (title != '' || reason != '') {
            var obj = {
                Title: $(listtds[1]).children().val(),
                Quantity: $(listtds[2]).children().val(),
                Reason: $(listtds[3]).children().val(),
                DateRequire: $(listtds[4]).children().val()
            };
            data.push(obj);
        }
    });
    $("#listTicketDetailJson").val(JSON.stringify(data));
}
