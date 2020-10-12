using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

public static class ContextHelper
{

    public static string GetCurrentCollerDisplayName(this ViewContext vcontext)
    {
        Type type = vcontext.Controller.GetType();

        string controllerName = vcontext.RouteData.Values["Controller"].ToString();
        var atts = type.GetCustomAttributes(typeof(DisplayNameAttribute), false);

        return ShowDisplayName(atts, controllerName);
    }

    public static string GetCurrentActionDisplayName(this ViewContext vcontext)
    {
        Type type = vcontext.Controller.GetType();

        string actionName = vcontext.RouteData.Values["action"].ToString();
        MethodInfo method = type.GetMethods().Where(s => s.Name == actionName).First();
        var atts = method.GetCustomAttributes(typeof(DisplayNameAttribute), false);

        return ShowDisplayName(atts, actionName);
    }

    public static string GetCurrentControllerName(this ViewContext vcontext)
    {
        return vcontext.RouteData.Values["controller"].ToString();
    }

    public static string GetCurrentActionName(this ViewContext vcontext)
    {
        return vcontext.RouteData.Values["action"].ToString();
    }

    

    public static string GetTitle(this ViewContext vcontext)
    {
        Type type = vcontext.Controller.GetType();

        string controllerName = vcontext.RouteData.Values["Controller"].ToString();
        var atts = type.GetCustomAttributes(typeof(DisplayNameAttribute), false);

        string projectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
        projectName = !string.IsNullOrEmpty(projectName) ? projectName : "[Page]";

        return ShowTitle(atts, projectName, controllerName);

    }
    private static string ShowTitle(object[] atts, string projectName, string typeName)
    {
        string resultName = string.Empty;

        if (atts.Length > 0)
        {
            resultName = ((DisplayNameAttribute)atts[0]).DisplayName;
        }
        else
        {
            resultName = typeName;
        }


        return string.Format("{0} - {1}", projectName, resultName);
    }
    private static string ShowDisplayName(object[] atts, string typeName)
    {
        string resultName = string.Empty;

        if (atts.Length > 0)
        {
            resultName = ((DisplayNameAttribute)atts[0]).DisplayName;
        }
        else
        {
            resultName = typeName;
        }
        return resultName;
    }
}