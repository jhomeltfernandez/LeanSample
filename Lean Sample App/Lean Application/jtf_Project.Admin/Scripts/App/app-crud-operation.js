var App_CRUDFunctions = new function() {
    return {
        Form: null,
        Data: null,
        CreateEditButton: null,
        ModalCreateEditButton: null,
        DeleteButton: null,
        InitModalCreateEditButton: function (container, element, callbackfunction) {
            $(container).delegate(element, 'click', function (e) {
                e.preventDefault();
                App_CRUDFunctions.ModalCreateEdit(e, callbackfunction);
            });
        },
        InitDeleteButton: function (container, element, successCallback) {
            $('.content-div').delegate('a.app-delete-btn', 'click', function (e) {
                e.preventDefault();

                App_CRUDFunctions.Delete(e, successCallback);
            });
        },
        ModalCreateEdit: function (e, callbackfunction) {
            var that = $(e);

            BootstrapDialog.show({
                title: $(that).data('action'),
                message: $('<div></div>').html(Common_Functions.loading()).load($(that).attr("href"), function () {
                    $('.loading-div').remove();
                }),
                buttons: [{
                    label: 'Cancel',
                    cssClass: 'btn-default',
                    action: function (dialogRef) {
                        dialogRef.close();
                    }
                }, {
                    icon: 'glyphicon glyphicon-save',
                    label: 'Save',
                    cssClass: 'btn-primary',
                    autospin: true,
                    action: function (dialogRef) {

                        var modalBody = dialogRef.getModalBody();
                        var form = $(modalBody).find('form').first();

                        $.ajax({
                            type: "POST",
                            async: true,
                            url: form.attr('action'),
                            datatype: "json",
                            data: $(form).serialize(),
                            beforeSend: function (xhr, settings) {
                                dialogRef.enableButtons(false);
                                dialogRef.setClosable(false);
                            },
                            success: function (data, textStatus, xhr) {
                                //var jsonData = data.requestModel;
                                callbackfunction(data);

                                
                                dialogRef.enableButtons(true);
                                dialogRef.setClosable(true);

                                if (data.Success == true) {
                                    dialogRef.close();
                                }
                                
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                alert(xhr, textStatus, errorThrown);
                                console.log(errorThrown);

                                dialogRef.enableButtons(true);
                                dialogRef.setClosable(true);
                            }
                        });
                    }
                }]
            });

        },
        CreateEditGeneric: function (form, beforeSendCallback, successCallback, completeCallback) {
            var that = this,
                beforeSendCallback = beforeSendCallback || that.beforeSendCallback,
                completeCallback = completeCallback || that.completeCallback;

            form = form || this.GetForm();

            $.ajax({
                type: "POST",
                async: true,
                url: form.attr('action'),
                datatype: "json",
                data: $(form).serialize(),
                beforeSend: function (xhr, settings) {
                    if (beforeSendCallback) {
                        beforeSendCallback(xhr, settings);
                    }
                },
                success: function (data, textStatus, xhr) {
                    if (successCallback) {
                        var jsonData = data.requestModel;
                        successCallback(jsonData, textStatus, xhr);
                    }
                },
                complete: function (xhr, textStatus) {
                    if (completeCallback) {
                        completeCallback(xhr, textStatus);
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(xhr, textStatus, errorThrown);
                    console.log(errorThrown);
                }
            });
        },
        Delete: function (e, successCallback) {
            //var thatObj = this,
            //    e = e || thatObj.DeleteButton,
            //callbackfunction = callbackfunction || thatObj.Events.onSaveSuccess;

            var that = $(e);

            BootstrapDialog.show({
                title: '<h5><i class="fa fa-warning"></i> Delete</h5>',
                message: "Are you sure do you want to delete this record?<br/>Note: Deleted data cannot be restored.",
                type: BootstrapDialog.TYPE_WARNING,
                buttons: [{
                    label: 'Cancel',
                    cssClass: 'btn-default',
                    action: function (dialogRef) {
                        dialogRef.close();
                    }
                }, {
                    label: 'Delete',
                    cssClass: 'btn-warning',
                    autospin: true,
                    action: function (dialogRef) {
                        $.ajax({
                            type: "Post",
                            url: $(that).attr("href"),
                            async: true,
                            data: {
                                id: $(that).attr('data-id')
                            },
                            beforeSend: function () {
                                dialogRef.enableButtons(false);
                                dialogRef.setClosable(false);
                            },
                            success: function (data, textStatus, xhr) {
                                successCallback(data);

                            },
                            complete: function (xhr, textStatus) {
                                dialogRef.enableButtons(true);
                                dialogRef.setClosable(true);
                                dialogRef.close();
                            },
                            error: function (xhr, textStatus, errorThrown) {
                                alert(xhr, textStatus, errorThrown);
                                console.log(errorThrown);
                            }
                        });
                    }
                }]
            });
        },
        GetListPartial: function (elem, url, completeCallback) {
            $.ajax({
                url: url,
                beforeSend: function () {
                    $(elem).html(Common_Functions.loading());
                },
                success: function (data) {
                    //$(elem).html(data);
                    completeCallback(data);
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(xhr, textStatus, errorThrown);
                    console.log(errorThrown);
                }
            });
        },
        GetListPartialWithId: function (elem, id, url) {
            $.ajax({
                url: url,
                data: {
                    id: id
                },
                beforeSend: function () {
                    $(elem).block({ message: Common_Functions.loading() });
                },
                success: function (data) {
                    $(elem).html(data);
                    $(elem).unblock();
                }
            });
        },
        GetForm: function () {
            return this.Form;
        },
        CreateEditWithFile: function (form, callbackfunction) {
            var formData = new FormData($(form)[0]);
            $.ajax({
                type: "Post",
                url: $(form).attr('action'),
                data:formData,
                processData: false,
                contentType: false,
                cache: false,
                success: function (data) {
                    callbackfunction(data);
                },
                error: function (xhr, textStatus, errorThrown) {
                    alert(xhr, textStatus, errorThrown);
                    console.log(errorThrown);
                }
            });
        }
    }
}


//var AppCRUDModal = new function () {
//    return {
//        showCreateEditD: function (e) {

//        }
//    }
//}


//jQuery(document).ready(function () {
//    $('.app-modal-create-edit-btn').on('click', function (e) {
//        e.preventDefault();
//        App_CRUDFunctions.ModalCreateEdit(e, callbackfunction);
//    });

//    $('.app-delete-btn').on('click', function (e) {
//        e.preventDefault();
//        App_CRUDFunctions.Delete(e, callbackfunction);
//    });
//});