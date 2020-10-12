var User = {
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
        App_CRUDFunctions.GetListPartial($('.content-div'), "/User/GetList", function (vdta) {
            $('.content-div').html(jQuery.parseHTML(vdta));
            User.initTable();
        });
    }
}



$(document).ready(function () {

    User.loadTablePartial();

    $('.box-body').delegate('.app-delete-btn', 'click', function (e) {
        e.preventDefault();
        var that = this;

        //console.debug('DELETE: ' + $(that).attr('data-id') + '-' + $(that).attr("href"));

        App_CRUDFunctions.Delete($(that), function (data) {
            // console.log(data);

            if (data.Success == true) {
                Notify.Success(data.Title, data.Message);

                User.loadTablePartial();
            } else {
                Notify.Danger(data.Title, data.Message);
            }
        });
    });

});
