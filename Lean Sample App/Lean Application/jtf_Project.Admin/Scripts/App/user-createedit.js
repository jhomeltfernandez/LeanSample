
$(document).ready(function () {
    User.initPhoto();

    $('div.fileinput-remove').hide();

    $('.fileinput-remove').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        $(this).parent('span.file-input').find('div.file-preview-thumbnails').first().html('');
        $('#kv-avatar-errors').hide();
        $(this).parent('span.file-input').find('div.file-preview-thumbnails').first().html('<div class="file-preview-frame file-preview-initial" id="preview-1453689512076-init_0" data-fileindex="init_0">' +
        '<img src="/Images/User/default_avatar_male.jpg" class="file-preview-image" alt="The Moon" title="The Moon" style="max-width:100%;"></div>');
    });

    $('#createEditForm input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_flat-blue',
        radioClass: 'iradio_flat-blue'
    });

    $('.datepicker').datepicker();


    $('.btn-gen-save').on('click', function (e) {
        e.preventDefault();
        var $that = $(this);

        var form = $('#createEditForm');
        if (form.valid()) {
            $that.attr('disabled', true);
            App_CRUDFunctions.CreateEditWithFile(form, function (data) {
                if (data.Success == true) {
                    Notify.Success(data.Title, data.Message);

                    form.find('input#Id').first().val(data.ReturnId);

                    var urlRedirectTo = $('a#redirectTo').attr('href');
                    setTimeout(function () {
                        window.location.replace(urlRedirectTo);
                    }, 3000);
                } else {

                    $('#responseDiv').html(data.Html);
                    $('html,body').animate({ scrollTop: $('#MessageContainer').offset().top + $('html,body').scrollTop() });
                    //Notify.Warning(data.Title, data.Message);
                }

                $that.attr('disabled', false);
            });
        }
    });


    $('.btn-gen-save-close').hide();
    //$('.btn-gen-save-close').on('click', function (e) {
    //    e.preventDefault();
    //    var form = $('#createEditForm');
    //    if (form.valid()) {
    //        App_CRUDFunctions.CreateEditWithFile(form, function (data) {
    //            console.log(data);
    //            if (data.Success == true) {
    //                Notify.Success(data.Title, data.Message);

    //                var urlRedirectTo = $('a#redirectTo').attr('href');

    //                setTimeout(function () {
    //                    window.location.replace(urlRedirectTo);
    //                }, 3000);

    //                //setTimeout(function () {
    //                //    $('a#redirectTo').click();
    //                //}, 2500);
    //            } else {
    //                $('#responseDiv').html(data.Html);
    //            }
    //            // Common_Functions.showBlockUINotification(data.Html);
    //        });
    //    }
        
    //});

    $('.app-delete-btn').on('click', function (e) {
        e.preventDefault();
        var that = this;

        //console.debug('DELETE: ' + $(that).attr('data-id') + '-' + $(that).attr("href"));

        App_CRUDFunctions.Delete($(that), function (data) {
            // console.log(data);

            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                $('tr#' + $(that).data('id')).fadeOut('slow');

            } else {
                Notify.Danger(data.Title, data.Message);
            }
        });
    });
});

$(function () {
    $('.chosen-select').chosen();
    $('.chosen-select-deselect').chosen({
        allow_single_deselect: true,
    });
    $('.chosen-container').css("width", "100%");
});



var User = {
    initPhoto: function () {
        //var btnCust = '<button type="button" class="btn btn-default" title="Add picture tags" ' +
        //    'onclick="alert(\'Call your custom code here.\')">' +
        //    '<i class="glyphicon glyphicon-tag"></i>' +
        //    '</button>';
        var imagepath = $("input#UserPhoto").data('imgpath');

        $("input#UserPhoto").fileinput({
            overwriteInitial: true,
            maxFileSize: 1500,
            allowedFileExtensions: ["jpg", "png", "gif"],
            maxFileCount: 1,
            showRemove: true,
            showUpload: false,
            showCancel: false,
            showCaption: false,
            browseLabel: '',
            removeLabel: '',
            initialPreview: ['<img src="' + imagepath + '" class="file-preview-image" alt="The Moon" title="The Moon" style="max-width:100%;" />'],
            browseIcon: '<i class="glyphicon glyphicon-folder-open"></i>',
            removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
            removeTitle: 'Cancel or reset changes',
            elErrorContainer: '#kv-avatar-errors',
            msgErrorClass: 'alert alert-block alert-danger',
            layoutTemplates: { main2: '{preview} ' + ' {remove} {browse}' },
        });
    }
}