$(document).ready(function () {
    //$.ajax({
    //    type: "GET",
    //    url: "/Home/CountNoti",
    //    complete: function (value) {
    //        $("#icon-noti").val(value);
    //    }
    //});
    
    $("#clear-noti").click(
        function (event) {
            $.ajax({
                type: "GET",
                url: "/Home/ClearNoti"
            });
            $("#icon-noti").html("");
        }
    );
    $("#icon-noti").click(
        function (event) {
            $("#ListNoti").html("<a class='lv-item' href=''>Loading...</a>");
            $.ajax({
                type: "GET",
                url: "/Home/ListNoti",
                success: function (dataCheck) {
                    $("#ListNoti").html(dataCheck);
                }
            });
        }
    );
});