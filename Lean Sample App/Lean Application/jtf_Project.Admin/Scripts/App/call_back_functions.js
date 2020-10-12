

var callback_Functions = new function () {

    return {
        onSuccess: function (data) {
            //this.showNotiffication(data.NotifyType);
        },
        onBeforeSend: function (xhr, settings) {
            //commonApp.disableFormButton();
            //commonApp.showLoadingImage();
        },
        onComplete: function () {
            //commonApp.enableFormButton();
            //commonApp.hideLoadingImage();
        },
        onError: function (xhr, textStatus, errorThrown) {
            //commonApp.enableFormButton();
            //commonApp.hideLoadingImage();
            alert(xhr + textStatus + errorThrown);
            console.log(errorThrown);
        }
    }
}