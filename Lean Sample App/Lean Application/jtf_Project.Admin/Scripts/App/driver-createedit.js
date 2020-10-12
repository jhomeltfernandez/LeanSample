
$(document).ready(function () {
    User.initPhoto();

    Map.initMap($('#Profile_Address').val(), '');

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

var Map = {
    mapDiv: document.getElementById("googleMap"),
    gMap: null,
    gMarker: null,
    initMap: function (addressOne, AddressTwo) {
        var geocoder = new google.maps.Geocoder();
        var address = addressOne;

        if (addressOne == "") {
            address = AddressTwo;
        }

        geocoder.geocode({ 'address': address }, function (results, status) {

            if (status == google.maps.GeocoderStatus.OK) {
                var latitude = results[0].geometry.location.lat();
                var longitude = results[0].geometry.location.lng();

                Map.createMap(latitude, longitude, address);
            }
        });
    },
    createMap: function (_lat, _lng, _address) {

        gMap = new google.maps.Map(Map.mapDiv, {
            center: { lat: _lat, lng: _lng },
            zoom: 16
        });

        Map.createMarker(_lat, _lng, _address);
    },
    createMarker: function (_lat, _lng, _address) {
        gMarker = new google.maps.Marker({
            position: { lat: _lat, lng: _lng },
            map: gMap,
            title: _address
        });
    }
}