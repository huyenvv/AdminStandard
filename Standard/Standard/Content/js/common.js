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

function isDate(str) {
    // format date dd/mm/yyyy
    if (str == undefined) { return false; }
    var parms = str.split(/[\.\-\/]/);
    var yyyy = parseInt(parms[2], 10);
    var mm = parseInt(parms[1], 10);
    var dd = parseInt(parms[0], 10);
    var date = new Date(yyyy, mm - 1, dd, 12, 0, 0, 0);
    return parms.length == 3 &&
        parms[2].length == 4 &&
        mm === (date.getMonth() + 1) &&
        dd === date.getDate() &&
        yyyy === date.getFullYear();
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function formatMoney(n, currency) {
    return currency + " " + n.toFixed(0).replace(/./g, function (c, i, a) {
        return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "." + c : c;
    });
}