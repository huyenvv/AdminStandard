$(document).ready(function () {
    $(".confirmDelete").click(function () {
        return confirm("Bạn chắc chắn?");
    });
    $('input.onCheckbox').on('change', function () {
        $('input.onCheckbox').not(this).prop('checked', false);
    });
});
// lay danh sách list ID
function getListID(classWrapper, hiddenID) {
    var listID = "";
    $("." + classWrapper + " input.checkItem:checkbox:checked").each(function () {
        listID += "," + $(this).val();
    });
    $("#" + hiddenID).val(listID);
    return true;
}

function validateDate(testdate) {
    // format date dd/mm/yyyy
    var date_regex = /^(0[1-9]|1\d|2\d|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$/;
    return date_regex.test(testdate);
}