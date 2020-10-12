var Common_Functions = new function () {
    return {
        Message:null,
        loading: function () {
            return '<div class="center-block text-center loading-div"><i class="fa fa-spinner fa-spin fa-2x"></i></div>';
        },
        loadingSecond: function () {
            return '<div class="overlay"><i class="fa fa-refresh fa-spin"></i></div>';
        },
        removeLoadingSecond: function (elem) {
            $(elem).find('div.overlay').remove();
        },
        showBlockUINotification: function (msg) {
            $.blockUI({
                message: msg,
                fadeIn: 700,
                fadeOut: 700,
                timeout: 2000,
                showOverlay: true,
                centerY: true,
                border: 'none',
                onOverlayClick: $.unblockUI(),
                css: {
                    width: '350px',
                    top: '14%',
                    border: 'none',
                    backgroundColor: 'none',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .6,
                    color: 'none'
                },
                overlayCSS: {
                    backgroundColor: 'none'
                }
            });
        },
        showNotiffication: function (alertContainer, ntype) {
            alertContainer = alertContainer || $("#alert-container");
            switch (ntype) {
                case 0: $(alertContainer).html(data.Html);
                    break;
            }
        },
        delayPageReload: function (milisec) {
            setTimeout(milisec, function () {
                window.location.reload(true);
            });
        },
        delayPageRedirect: function (milisec, redirecttourl) {
            setTimeout(function () {
                window.location.replace(redirecttourl);
            }, milisec);
        }
    }
}


var Notify = {
    Success: function (title, msg) {
        Notify.show(title, 'success', msg, 'glyphicon glyphicon-check');
    },
    Info: function (title, msg) {
        Notify.show(title, 'info', msg, 'glyphicon glyphicon-info-sign');
    },
    Warning: function (title, msg) {
        Notify.show(title, 'warning', msg, 'glyphicon glyphicon-warning-sign');
    },
    Danger: function (title, msg) {
        Notify.show(title, 'danger', msg, 'glyphicon glyphicon-exclamation-sign');
    },
    show: function (title, type, msg, icon) {
        $.notify({
            // options
            icon: icon,
            title: title,
            message: msg,
            target: '_blank'
        }, {
            // settings
            element: 'body',
            position: null,
            type: type,
            allow_dismiss: true,
            newest_on_top: false,
            showProgressbar: false,
            placement: {
                from: "top",
                align: "center"
            },
            offset: {
                x: 50,
                y: 52
            },
            spacing: 10,
            z_index: 1031,
            delay: 3500,
            timer: 1000,
            url_target: '_blank',
            mouse_over: null,
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            onShow: null,
            onShown: null,
            onClose: null,
            onClosed: null,
            icon_type: 'class',
            template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert" style="opacity: 0.8;">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
            '</div>'
        });
    }
}


//var BModal = new BootstrapDialog();

//var GenericButtons = {
//    CancelAndContinue: function () {
//        var buttons = [];

//        //Calcel BUtton
//        buttons.push({
//            label: 'Cancel',
//            cssClass: 'btn-default',
//            action: function (dialog) {
//                dialog.setType(type);
//            }
//        });

//        //Continue Button
//        buttons.push({
//            icon: 'glyphicon glyphicon-send',
//            label: 'Continue',
//            cssClass: 'btn-warning',
//            autospin: true,
//            action: function (dialogRef) {
//                dialogRef.enableButtons(false);
//                dialogRef.setClosable(false);
//                dialogRef.getModalBody().html('Dialog closes in 5 seconds.');
//                setTimeout(function () {
//                    dialogRef.close();
//                }, 5000);
//            }
//        });

//    }
//}