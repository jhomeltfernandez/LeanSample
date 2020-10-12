using jtf_Project.Data.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jtf_Project.Data.HelperModel;

public static class Renders
{
    public static string Capture(this ActionResult result, ControllerContext controllerContext)
    {
        using (var it = new ResponseCapture(controllerContext.RequestContext.HttpContext.Response))
        {
            result.ExecuteResult(controllerContext);
            return it.ToString();
        }
    }


    public static string RenderPartialView(this Controller controller, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            viewResult.View.Render(viewContext, sw);

            return sw.GetStringBuilder().ToString();
        }
    }


    public static string RenderPartialView(this ControllerContext controllerContext, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controllerContext.RouteData.GetRequiredString("action");

        controllerContext.Controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
            var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
            viewResult.View.Render(viewContext, sw);

            return sw.GetStringBuilder().ToString();
        }
    }
}
