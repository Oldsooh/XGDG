$(document).ready(function () {
    $(".picbox div").hover(function () {
        //$(this).animate({"top": "-350px"}, 300, "swing");  pic-list-b.png
        $(this).animate({ "top": "-350px" }, 0, "swing");
    }, function () {
        //$(this).stop(true,false).animate({"top": "0px"}, 300, "swing");  pic-list-b.png
        $(this).stop(true, false).animate({ "top": "0px" }, 0, "swing");
    });
});
$(function () {
    //图片延迟加载
    $("img").lazyload({
        effect: "fadeIn",
        threshold: 100
    });
})
function goTop() {
    $(window).scroll(function (e) {
        if ($(window).scrollTop() > 100)
            $("#gotop").fadeIn(500);
        else
            $("#gotop").fadeOut(500);
    });
};
$(function () {
    $("#gotop").click(function (e) {
        $('body,html').animate({ scrollTop: 0 }, 300);
    });
    $("#gotop").mouseover(function (e) {
        $(this).css("background", "url(/content/images/gettop.png) no-repeat 0px 0px");
    });
    $("#gotop").mouseout(function (e) {
        $(this).css("background", "url(/content/images/gettop.png) no-repeat 0px -131px");
    });
    goTop();
});

$(function () {
    $("#search").focus(function () {
        if (!$(this).hasClass("focus")) {

            $(this).addClass("focus");
        }
    });
    $("#search").blur(function () {
        if ($(this).hasClass("focus")) {

            $(this).removeClass("focus");
        }
    });
});

$(function () {
    $('#search').keydown(function (e) {
        var currKey = 0, e = e || event;
        currKey = e.keyCode || e.which || e.charCode;
        if (currKey == 13) {
            searchKeywords();
        }
    });

    $('#searchbtn').click(function () {
        searchKeywords();
    });
});

function searchKeywords() {
    var keywords = $('#search').val();
    if (keywords == '') {
        return false;
    }
    else {
        window.location.href = '/search/' + keywords;
    }
}

function AddFavorite() {
    var title = "性感帝国";
    var url = "http://www.xgdg.cn";
    var ctrl = (navigator.userAgent.toLowerCase()).indexOf('mac') != -1 ? 'Command/Cmd' : 'CTRL';
    if (document.all) {
        window.external.addFavorite(url, title);
    }
    else if (window.sidebar) {
        window.sidebar.addPanel(title, url, "");
    }
    else {
        alert('您可以通过快捷键Ctrl + D 加入到收藏夹');
    }
}