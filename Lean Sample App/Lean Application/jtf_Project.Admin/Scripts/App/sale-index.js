var Sale = {
    initTable: function () {
        $("table.dataTable").dataTable({
            "columnDefs": [{
                "targets": -1,
                "searchable": false,
                "orderable": false
            }]
        });
    },
    loadTablePartial: function () {
        App_CRUDFunctions.GetListPartial($('.content-div'), "/Sale/GetList", function (vdta) {
            $('.content-div').html(jQuery.parseHTML(vdta));
            Sale.initTable();
        });
    }
}



$(document).ready(function () {
    Sale.loadTablePartial();
    $('.box-header').delegate('a.app-modal-create-edit-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
            console.log(data);
            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                Sale.loadTablePartial();
            } else {
                //Notify.Warning(data.Title, data.Message);

                $('form #responseDiv').html(data.Html);
            }
            // Common_Functions.showBlockUINotification(data.Html);
        });


        console.debug('create: ' + $(that).attr('href'));
    });

    $('.content-div').delegate('a.app-modal-create-edit-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
            console.log(data);
            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                Sale.loadTablePartial();
            } else {
                //Notify.Warning(data.Title, data.Message);

                $('form #responseDiv').html(data.Html);
            }
            // Common_Functions.showBlockUINotification(data.Html);
        });


        console.debug('create: ' + $(that).attr('href'));
    });


    $('.content-div').delegate('a.app-create-edit-btn', 'click', function (e) {
        e.preventDefault();

        var that = this;

        App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
            console.log(data);

            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                Sale.loadTablePartial();



            } else {
                //Notify.Danger(data.Title, data.Message);

                $('form #responseDiv').html(data.Html);
            }
        });

        console.debug('DELETE: ' + $(that).attr('data-id') + '-' + $(that).attr("href"));
    });

    $('.box-body').delegate('.app-delete-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        //console.debug('DELETE: ' + $(that).attr('data-id') + '-' + $(that).attr("href"));

        App_CRUDFunctions.Delete($(that), function (data) {
            // console.log(data);

            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                Sale.loadTablePartial();
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



    $('.datepicker').datepicker();
});