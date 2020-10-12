$(document).ready(function () {
    layout.initSettings();
});



var layout = {
    initSettings:function(){
        $.ajax({
            url: "/Layout/GetSetting",
            success:function(data){
                layout.clickControlls(data);
            },
            error: function (request) {
                alert(JSON.parse(request.responseText));
            }
        });
    },
    clickControlls: function (data) {
        $('#isClickForUpdate').val(false);

        layout.execClickIfNotEquals($("[data-layoutoptn='0']"), data.FixedLayout);
        layout.execClickIfNotEquals($("[data-layoutoptn='1']"), data.BoxedLayout);
        layout.execClickIfNotEquals($("[data-layoutoptn='2']"), data.ToggleSideBar);
        layout.execClickIfNotEquals($("[data-layoutoptn='3']"), data.SideBarExpandOnHover);
        layout.execClickIfNotEquals($("[data-layoutoptn='4']"), data.ToggleRightSideBarSlide);
        layout.execClickIfNotEquals($("[data-layoutoptn='5']"), data.ToggleRightSideBarSkin);
        layout.clickSkinThumbnail(data.Skin);

        $('#isClickForUpdate').val(true);
    },
    execClickIfNotEquals: function (obj, optvalue) {
        var objValue = false;

        if ($(obj).is(":checked")) {
            objValue = true;
        }

        if (objValue != optvalue) {
            $(obj).trigger('click');
        }
    },
    clickSkinThumbnail: function (skinval) {
        var elementName = "[data-skinvalue='" + skinval + "']";

        $(elementName).trigger('click');
    }
}