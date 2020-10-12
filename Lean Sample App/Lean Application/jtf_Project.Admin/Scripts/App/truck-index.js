var Truck = {
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
        App_CRUDFunctions.GetListPartial($('.content-div'), "/Truck/GetList", function (vdta) {
            $('.content-div').html(jQuery.parseHTML(vdta));
            Truck.initTable();
        });
    }
}



$(document).ready(function () {
    Truck.loadTablePartial();


    $('.box-header').delegate('a.app-modal-create-edit-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
            console.log(data);
            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                Truck.loadTablePartial();

                
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

                Truck.loadTablePartial();
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

                Truck.loadTablePartial();
            } else {
                Notify.Danger(data.Title, data.Message);
            }
        });
    });

});


$(function () {
    $('.model .chosen-select').chosen();
    $('.model .chosen-select-deselect').chosen({
        allow_single_deselect: true,
    });
    $('.model .chosen-container').css("width", "100%");
});