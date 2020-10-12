$(document).ready(function () {
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' // optional
    });

    //$('#loginForm').delegate('.btn-sign-in', 'click', function (e) {
    //    e.preventDefault();
    //    var that = this;

    //    App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
    //        if (data.Success == true) {
    //            Notify.Success(data.Title, data.Message);

    //            Role.loadTablePartial();
    //        } else {
    //            Notify.Warning(data.Title, data.Message);
    //        }
    //        // Common_Functions.showBlockUINotification(data.Html);
    //    });


    //    console.debug('create: ' + $(that).attr('href'));
    //});
});