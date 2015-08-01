$(function () {
    var element = $('ul.main-menu a').filter(function () {
        var $this = $(this);
        var ahref = $this.attr("href") + $this.attr("data-href");
        return ahref.indexOf("/" + getController() + "/") != -1 && ahref.indexOf("/" + getAction() + "/") != -1;
    }).addClass('active').parents("li").addClass('active').parents("ul").show().parents("li").addClass('toggled');

    $(".Changpassword").click(function () {
        var id = $(this).attr("href");
        $(id).modal('show');
    });
    
});
