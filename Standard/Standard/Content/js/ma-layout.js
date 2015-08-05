$(function () {
    var element = $('ul.main-menu a').filter(function () {
        var $this = $(this);
        var ahref = $this.attr("href") + $this.attr("data-href");
        return ahref.indexOf("/" + getController() + "/") != -1 && ahref.indexOf("/" + getAction() + "/") != -1;
    }).addClass('active').parents("li").addClass('active').parents("ul").show().parents("li").addClass('toggled');

    $(".Changpassword").click(function () {
        var id = $(this).attr("href");
        $.ajax({
            type: "GET",
            url: "/Account/ChangePassword",
            success: function (kq) {
                $(id).html(kq).modal('show');
            }
        });

    });

});

function AddEventEditTable() {
    $('table td.typeDate').on('validate', function (evt, newValue) {
        if (!isDate(newValue)) {
            return false; // mark cell as invalid 
        }
    });
    $('table td.noChange').on('validate', function (evt, newValue) {
        return false;
    });
    $('table td.typeNumber').on('validate', function (evt, newValue) {
        if (!isNumeric(newValue)) {
            return false; // mark cell as invalid 
        }
    });
}

function addInputForTableID(id) {
    $("#" + id + " td:not(.noedit)").each(function () {
        var $this = $(this);
        var getClass = $this.attr("class");
        var input = "<input type='text' class='edittable " + getClass + "' value='"+$this.text()+"' />";
        $this.html(input);
        $('input.typeDate').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    });
}
