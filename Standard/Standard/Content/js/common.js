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