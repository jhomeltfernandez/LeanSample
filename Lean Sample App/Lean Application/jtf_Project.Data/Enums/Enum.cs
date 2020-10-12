using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jtf_Project.Data.Enums
{
    public enum AccountStatuses
    {
        [Description("Active")]
        Active = 0,
        [Description("Inactive")]
        Inactive = 1
    }

    public enum ResponseMessage
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("Error")]
        Error = 1,
        [Description("save")]
        Save = 2,
        [Description("update")]
        Update = 3,
        [Description("delete")]
        Delete = 4,
        [Description("unblock")]
        Unblock = 5
    }

    public enum RequestResultInfoType
    {
        Information = 0,
        Success = 1,
        Warning = 2,
        ErrorOrDanger = 3
    }

    public enum NotifyType
    {
        PageInline = 0,
        DialogInline = 1,
        Dialog = 2,
        PageFixedPopUp = 3
    }

    public enum GenderEnum
    {
        [Description("Male")]
        Male = 0,
        [Description("Female")]
        Female = 1
    }

    public enum LayoutOption
    {
        Fixed = 0,
        Boxed = 1,
        ToggleSideBar = 2,
        SideBarExpandOnHover = 3,
        ToggleRightSideBarSlide = 4,
        ToggleRightSideBarSkin = 5,
        Skin = 6
    }

    public enum LayoutSkin
    {
        Blue = 0,
        Black = 1,
        Purple = 2,
        Green = 3,
        Red = 4,
        Yellow = 5,
        BlueLight = 6,
        BlackLight = 7,
        PurpleLight = 8,
        GreenLight = 9,
        RedLight = 10,
        YellowLight = 11,
    }
}
