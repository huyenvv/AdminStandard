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
function checkAllByClassName(className) {
    // show hide button Edit & Delete when click checkbox
    if ($("." + className + " .checkItem:checked").length == $("." + className + " .checkItem").length) {
        $("." + className + " .checkAll:first-child").prop('checked', true);
        $("." + className).find(".collapse ").addClass("in");
    }
    $("." + className + " .checkAll:first-child").click(function () {
        if ($(this).is(':checked')) {
            $("." + className + " .checkItem").prop('checked', true);
        } else {
            $("." + className + " .checkItem").prop('checked', false);
        }
    });
    $("." + className + " .checkItem").click(function () {
        var numberOfChecked = $("." + className + " .checkItem:checked").length;
        var numberItem = $("." + className + " .checkItem").length;
        if ($(this).is(':checked')) {
            if (numberItem == numberOfChecked) {
                $("." + className + " .checkAll:first-child").prop('checked', true);
            }
        } else {
            $("." + className + " .checkAll:first-child").prop('checked', false);
        }
    });
}