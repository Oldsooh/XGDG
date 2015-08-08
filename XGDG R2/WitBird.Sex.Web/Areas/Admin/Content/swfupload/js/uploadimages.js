var swfu;
window.onload = function () {
    var settings = {
        flash_url: "/Areas/Admin/Content/swfupload/swfupload.swf",
        upload_url: "/Areas/Admin/Content/swfupload/upload.ashx",
        file_size_limit: "100 MB",
        file_types: "*.*",
        file_types_description: "All Files",
        file_upload_limit: 100,
        file_queue_limit: 0,
        custom_settings: {

            progressTarget: "divprogresscontainer",
            progressGroupTarget: "divprogressGroup",

            //progress object
            container_css: "progressobj",
            icoNormal_css: "IcoNormal",
            icoWaiting_css: "IcoWaiting",
            icoUpload_css: "IcoUpload",
            fname_css: "fle ftt",
            state_div_css: "statebarSmallDiv",
            state_bar_css: "statebar",
            percent_css: "ftt",
            href_delete_css: "ftt",

            //sum object
            /*
            页面中不应出现以"cnt_"开头声明的元素
            */
            s_cnt_progress: "cnt_progress",
            s_cnt_span_text: "fle",
            s_cnt_progress_statebar: "cnt_progress_statebar",
            s_cnt_progress_percent: "cnt_progress_percent",
            s_cnt_progress_uploaded: "cnt_progress_uploaded",
            s_cnt_progress_size: "cnt_progress_size"
        },
        debug: false,

        // Button settings
        button_image_url: "/Areas/Admin/Content/swfupload/uploadbutton.jpg",
        button_width: "100",
        button_height: "32",
        button_placeholder_id: "spanButtonPlaceHolder",
        button_text: '<span class="theFont">&nbsp;&nbsp;选择多图</span>',
        button_text_style: ".theFont { font-size: 12;color:#0068B7; }",
        button_text_left_padding: 10,
        button_text_top_padding: 10,

        // The event handler functions are defined in handlers.js
        file_queued_handler: fileQueued,
        file_queue_error_handler: fileQueueError,
        upload_start_handler: uploadStart,
        upload_progress_handler: uploadProgress,
        upload_error_handler: uploadError,
        upload_success_handler: uploadSuccess,
        upload_complete_handler: uploadComplete,
        file_dialog_complete_handler: fileDialogComplete
    };
    swfu = new SWFUpload(settings);
};

function showGroupDiv() {
    document.getElementById("GroupDiv").style.display = 'block';
}

function closeGroupDiv() {
    document.getElementById("GroupDiv").style.display = 'none';
}