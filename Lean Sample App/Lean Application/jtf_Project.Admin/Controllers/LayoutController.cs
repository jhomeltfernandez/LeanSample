using jtf_Project.Data.Enums;
using jtf_Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jtf_Project.Data;

namespace jtf_Project.Admin.Controllers
{
    public class LayoutController : BaseController
    {
        // GET: Layout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update(int layoutOption, bool value = false, int skin = 0)
        {
            bool isResultSuccess = false;

            LayoutSetting setting = Context.LayoutSettingRepo.GetFirstOrDeffault();

            if(setting != null)
            {
                switch (layoutOption)
                {
                    case (int)LayoutOption.Boxed:
                        setting.BoxedLayout = value;
                        break;
                    case (int)LayoutOption.Fixed:
                        setting.FixedLayout = value;
                        break;
                    case (int)LayoutOption.SideBarExpandOnHover:
                        setting.SideBarExpandOnHover = value;
                        break;
                    case (int)LayoutOption.ToggleRightSideBarSkin:
                        setting.ToggleRightSideBarSkin = value;
                        break;
                    case (int)LayoutOption.ToggleRightSideBarSlide:
                        setting.ToggleRightSideBarSlide = value;
                        break;
                    case (int)LayoutOption.ToggleSideBar:
                        setting.ToggleSideBar = value;
                        break;
                    case (int)LayoutOption.Skin:
                        setting.Skin = skin;
                        break;
                    default:
                        break;
                }

                Context.LayoutSettingRepo.Update(setting);
                Context.Save();

                isResultSuccess = true;
            }
            return Json(new { Success = isResultSuccess }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSetting()
        {
            LayoutSetting setting = Context.LayoutSettingRepo.GetFirstOrDeffault();

            if (setting != null)
            {
                return Json(setting, JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("Error! No layout settings found!");
        }

    }
}