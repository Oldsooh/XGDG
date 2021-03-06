﻿
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
        $('body,html').animate({ scrollTop: 0 }, 1000);
    });
    $("#gotop").mouseover(function (e) {
        $(this).css("background", "url(/content/images/gettop.png) no-repeat 0px -70px");
    });
    $("#gotop").mouseout(function (e) {
        $(this).css("background", "url(/content/images/gettop.png) no-repeat 0px -201px");
    });
    goTop();
});

$(function () {
    $('#q').keydown(function (e) {
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
    var keywords = $('#q').val();
    if (keywords == '') {
        return false;
    }
    else {
        window.location.href = '/search/' + keywords;
    }
}

jQuery.fn.LoadImage = function () {
    $(this).each(function () {
        var t = $(this);
        var src = t.attr("src")
        var img = new Image();
        img.src = src;

        if (img.complete) {
            return;
        }

        if (t[0].parentElement.id != 'bdshare') {
            var loadingImg = "/content/images/loading.gif";
            var loading = $("<img alt=\"loading...\" title=\"loading...\" src=\"" + loadingImg +
                "\" style=\"margin: 0 auto;display: block;width: 20px;height:20px;margin-top: 20%;margin-bottom: 20%;\"/>");

            t.hide();
            t.after(loading);
            t.attr("src", "");

            $(img).load(function () {
                loading.hide();
                loading.remove();
                t.attr("src", this.src);
                t.show();
            });
        }
    });
}


$(function () {
    $('.container img').LoadImage();
});