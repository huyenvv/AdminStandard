$(document).ready(function () {
    $(".confirmDelete").click(function () {
        return confirm("Bạn chắc chắn?");
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