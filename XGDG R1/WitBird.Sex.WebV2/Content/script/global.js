
// 回滚动
$(function () {
    $("#gotop").click(function (e) {
        $('body,html').animate({ scrollTop: 0 }, 300);
    });
    $("#gotop").mouseover(function (e) {
        $(this).css("background", "url(../images/gettop.png) no-repeat 0px -70px");
    });
    $("#gotop").mouseout(function (e) {
        $(this).css("background", "url(../gettop.png) no-repeat 0px -201px");
    });
    goTop();
});
function goTop() {
    $(window).scroll(function (e) {
        if ($(window).scrollTop() > 100)
            $("#gotop").fadeIn(500);
        else
            $("#gotop").fadeOut(500);
    });
};
$(function () {
    // 内容页图片居中并自适应
    $(".contes img").addClass("img-responsive center-block img-thumbnail");
    //翻页页码样式
    $(".pages a").addClass("btn btn-default");
    $(".pages span").addClass("btn btn-danger");
})