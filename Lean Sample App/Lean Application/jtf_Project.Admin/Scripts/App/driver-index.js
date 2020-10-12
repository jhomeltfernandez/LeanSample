var Driver = {
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
        App_CRUDFunctions.GetListPartial($('.content-div'), "/Driver/GetList", function (vdta) {
            $('.content-div').html(jQuery.parseHTML(vdta));
            Driver.initTable();
        });
    }
}

$(document).ready(function () {

    Driver.loadTablePartial();

    $('.box-body').delegate('.app-delete-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        //console.debug('DELETE: ' + $(that).attr('data-id') + '-' + $(that).attr("href"));

        App_CRUDFunctions.Delete($(that), function (data) {
            // console.log(data);

            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                $(that).closest('tr').fadeOut('slow');

            } else {
                Notify.Danger(data.Title, data.Message);
            }
        });
    });

});
