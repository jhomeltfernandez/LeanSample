using jtf_Project.Data.HelperModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

public static class ViewHelper
{
    private const string _InvalidCollection = "The property {0} cannot be cast to IEnumerable";
    private const string _InvalidSelect = "View data does not contain a collection property named {0}List";

    public static string IsActive(this HtmlHelper control, ViewContext vcontext, string[] urls, string returnString = "")
    {
        string result = string.Empty;


        Type type = vcontext.Controller.GetType();

        string controllerName = vcontext.RouteData.Values["Controller"].ToString();
        string actionName = vcontext.RouteData.Values["Action"].ToString();

        //string a = "/" + controllerName + "/" + actionName;
        string currentUrl = vcontext.HttpContext.Request.RawUrl.ToString();

        foreach (string url in urls)
        {
            if (url.Equals(currentUrl))
            {
                result = returnString;
                break;
            }
        }


        return result;
    }

    public static MvcHtmlString jDisplayTable(this HtmlHelper helper, GenericTableSetting setting)
    {
        // Get the model metadata
        ModelMetadata metaData = helper.ViewData.ModelMetadata;

        // Get the fully qualified name of the property
        string fieldName = ExpressionHelper.GetExpressionText(helper.ViewData.Model.GetType().FullName);
        // Get the collection to render in the table
        IEnumerable collection = metaData.Model as IEnumerable;
        if (collection == null)
        {
            throw new InvalidCastException(string.Format(_InvalidCollection, fieldName));
        }

        // Get the type in the collection
        Type type = GetCollectionType(collection);
        if (type == null)
        {
            // TODO: Wahts the right exception?
            throw new InvalidCastException("The type in the collection could not be resolved");
        }
        // Get the metadata of the type (in case there are no items in the table)
        ModelMetadata typeMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, type);

        var tableModel = new GenericTableVModel();
        FillGenericTable(ref tableModel, collection, type, setting);

        var tHeader = helper.ViewContext.Controller.ControllerContext.RenderPartialView(@"_GenericTableHeader", tableModel.Rows.FirstOrDefault());
        var tBody = helper.ViewContext.Controller.ControllerContext.RenderPartialView(@"_GenericTableBody", tableModel.Rows);

        var table = CreateTableString(tHeader, tBody, tableModel);

        return MvcHtmlString.Create(table);
    }

    public static void FillGenericTable(ref GenericTableVModel table, IEnumerable collection, Type metaData, GenericTableSetting setting)
    {
        List<GenericTableRowVmodel> rows = new List<GenericTableRowVmodel>();

        foreach (var item in collection)
        {
            ModelMetadata itemMetadata = ModelMetadataProviders.Current
                      .GetMetadataForType(() => item, metaData);

            GenericTableRowVmodel row = new GenericTableRowVmodel();
            row.Setting = setting;

            List<GenericTableCellVmodel> cells = new List<GenericTableCellVmodel>();

            foreach (ModelMetadata property in itemMetadata.Properties)
            {

                if (property.IsComplexType)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.ModelType))
                    {
                        // Its a collection within the collection so ignore.
                        continue;
                    }
                }

                GenericTableCellVmodel cell = new GenericTableCellVmodel();

                if (property.PropertyName.Equals(setting.Id))
                {
                    row.Id = GetValueDisplayControl(property);
                }

                //row.Id = property.PropertyName.Equals(setting.ForceToId) ? GetValueDisplayControl(property) : row.Id;

                string[] excludedProp = setting.PropertiesNotToShow.Split(',');

                if (!excludedProp.Contains(property.PropertyName))
                {
                    cell.Name = !string.IsNullOrEmpty(property.DisplayName) ? property.DisplayName : property.PropertyName;
                    cell.Value = GetValueDisplayControl(property);

                    cells.Add(cell);
                }
            }

            row.Cells = cells;

            rows.Add(row);
        }

        table.Setting = setting;
        table.Rows = rows;
    }

    private static string CreateTableString(string header, string body, GenericTableVModel model)
    {
        StringBuilder html = new StringBuilder();
        System.Web.Mvc.TagBuilder table = new System.Web.Mvc.TagBuilder("table");
        table.MergeAttribute("class", model.Setting.Class);
        table.MergeAttribute("id", model.Setting.Id);

        table.InnerHtml = header + body;

        html.Append(table.ToString());

        return html.ToString();
    }


    private static string TableCellCheckBox(ModelMetadata metaData, string fieldName)
    {
        StringBuilder html = new StringBuilder();
        // Checkbox 
        System.Web.Mvc.TagBuilder checkbox = new System.Web.Mvc.TagBuilder("input");
        checkbox.MergeAttribute("type", "checkbox");
        checkbox.MergeAttribute("name", fieldName);
        checkbox.MergeAttribute("id", fieldName);
        //checkbox.MergeAttribute("value", "true");
        checkbox.MergeAttribute("disabled", "disabled");
        checkbox.MergeAttribute("class", "checkbox");
        if (metaData.Model != null && (bool)metaData.Model)
        {
            checkbox.MergeAttribute("checked", "checked");
        }
        html.Append(checkbox.ToString(TagRenderMode.SelfClosing));
        // Build additional hidden input to address scenario where 
        // unchecked checkboxes are not sent in the request.

        //System.Web.Mvc.TagBuilder hidden = new System.Web.Mvc.TagBuilder("input");
        //hidden.MergeAttribute("type", "hidden");
        //hidden.MergeAttribute("name", fieldName);
        //checkbox.MergeAttribute("id", fieldName);
        //hidden.MergeAttribute("value", "false");
        //html.Append(hidden.ToString(TagRenderMode.SelfClosing));

        // Table cell

        return html.ToString();
    }

    private static string GetValueDisplayControl(ModelMetadata metaData)
    {
        if (metaData.Model == null)
        {
            return string.Empty;
        }

        if (metaData.ModelType == typeof(Nullable<bool>) || metaData.ModelType == typeof(bool))
        {
            // If we got this far it has a value
            return TableCellCheckBox(metaData, metaData.PropertyName);
        }

        // Return the formatted value
        string formatString = metaData.DisplayFormatString ?? "{0}";
        return string.Format(formatString, metaData.Model);
    }



    private static Type GetCollectionType(IEnumerable collection)
    {
        // Check types that wont make any sense to render in a table
        //if (collection.IsGrouped())
        //{
        //    return null;
        //}
        Type type = collection.GetType();
        if (type.IsGenericType)
        {
            return type.GetInterfaces().Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Single().GetGenericArguments()[0];
        }
        if (collection.GetType().IsArray)
        {
            return type.GetElementType();
        }
        // Who knows?
        return null;
    }
}
