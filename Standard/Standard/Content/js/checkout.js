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

    return check;
}