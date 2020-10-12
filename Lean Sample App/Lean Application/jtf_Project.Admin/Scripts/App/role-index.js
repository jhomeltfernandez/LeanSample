var Role = {
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
        App_CRUDFunctions.GetListPartial($('.content-div'), "/Role/_GetList", function (vdta) {
            $('.content-div').html(jQuery.parseHTML(vdta));
            Role.initTable();
        });
    }
}



$(document).ready(function () {
    Role.loadTablePartial();


    $('.box-header').delegate('a.app-modal-create-edit-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                Role.loadTablePartial();
            } else {
                Notify.Warning(data.Title, data.Message);
            }
            // Common_Functions.showBlockUINotification(data.Html);
        });


        console.debug('create: ' + $(that).attr('href'));
    });

    $('.content-div').delegate('a.app-create-edit-btn', 'click', function (e) {
        e.preventDefault();

        var that = this;

       App_CRUDFunctions.ModalCreateEdit($(that), function (data) {
           if (data.Success == true) {
               Notify.Success(data.Title, data.Message);

               Role.loadTablePartial();
           } else {
               Notify.Danger(data.Title, data.Message);
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

                Role.loadTablePartial();
            } else {
                Notify.Danger(data.Title, data.Message);
            }
        });
    });

});
