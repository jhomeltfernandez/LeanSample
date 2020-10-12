using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;


public static class EnumHelper
{
    public static string GetName(this Enum enumObj, int selected)
    {
        FieldInfo field = enumObj.GetType().GetField(enumObj.GetType().GetEnumName(selected));

        DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        if (attr != null && !string.IsNullOrEmpty(attr.Description))
            return attr.Description;
        else
            return field.Name;
    }

    public static SelectList ToSelectList<TEnum>(this TEnum enumObj, int selected, bool sortByText = true)
    {
        List<SelectListItem> items = new List<SelectListItem>();

        foreach (var i in Enum.GetValues(typeof(TEnum)))
        {
            int value = (int)i;
            if (value > 0)
            {
                items.Add(new SelectListItem
                {
                    Text = GetDescriptionInternal(((int)i), enumObj),
                    Value = ((int)i).ToString(),
                    Selected = (((int)i) == selected ? true : false)
                });
            }
            
        }

        if (sortByText)
            items = items.OrderBy(x => x.Text).ToList();

        return new SelectList(items, "Value", "Text", selected);
    }

    public static SelectList ToSelectListNoExclude<TEnum>(this TEnum enumObj, int selected, bool sortByText = true)
    {
        List<SelectListItem> items = new List<SelectListItem>();

        foreach (var i in Enum.GetValues(typeof(TEnum)))
        {
            items.Add(new SelectListItem
            {
                Text = GetDescriptionInternal(((int)i), enumObj),
                Value = ((int)i).ToString(),
                Selected = (((int)i) == selected ? true : false)
            });
        }

        if (sortByText)
            items = items.OrderBy(x => x.Text).ToList();

        return new SelectList(items, "Value", "Text", selected);
    }

    public static SelectList ToSelectListWithExcludeList<TEnum>(this TEnum enumObj, int selected, bool sortByText = true, List<int> excludes = null)
    {
        List<SelectListItem> items = new List<SelectListItem>();

        foreach (var i in Enum.GetValues(typeof(TEnum)))
        {
            int value = (int)i;
            if (value > 0)
            {
                if (excludes != null && !excludes.Contains(value))
                {
                    items.Add(new SelectListItem
                    {
                        Text = GetDescriptionInternal(((int)i), enumObj),
                        Value = ((int)i).ToString(),
                        Selected = (((int)i) == selected ? true : false)
                    });
                }
                if (excludes == null)
                {
                     items.Add(new SelectListItem
                    {
                        Text = GetDescriptionInternal(((int)i), enumObj),
                        Value = ((int)i).ToString(),
                        Selected = (((int)i) == selected ? true : false)
                    });
                }
            }

        }

        if (sortByText)
            items = items.OrderBy(x => x.Text).ToList();

        return new SelectList(items, "Value", "Text", selected);
    }

    public static SelectList ToSelectListWithIncludeList<TEnum>(this TEnum enumObj, int selected, bool sortByText = true, List<int> includes = null)
    {
        List<SelectListItem> items = new List<SelectListItem>();

        foreach (var i in Enum.GetValues(typeof(TEnum)))
        {
            int value = (int)i;
            if (value > 0)
            {
                if (includes != null && includes.Contains(value))
                {
                    items.Add(new SelectListItem
                    {
                        Text = GetDescriptionInternal(((int)i), enumObj),
                        Value = ((int)i).ToString(),
                        Selected = (((int)i) == selected ? true : false)
                    });
                }
                if (includes == null)
                {
                    items.Add(new SelectListItem
                    {
                        Text = GetDescriptionInternal(((int)i), enumObj),
                        Value = ((int)i).ToString(),
                        Selected = (((int)i) == selected ? true : false)
                    });
                }
            }

        }

        if (sortByText)
            items = items.OrderBy(x => x.Text).ToList();

        return new SelectList(items, "Value", "Text", selected);
    }


    public static string GetDescriptionInternal<TEnum>(int id, TEnum enumObj)
    {
        try
        {
            FieldInfo field = enumObj.GetType().GetField(enumObj.GetType().GetEnumName(id));

            if (field != null)
            {
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attr != null)
                {
                    return attr.Description;
                }
                else
                {
                    return field.Name;
                }
            }
        }
        catch (ArgumentNullException ex)
        {
            // Ignore null exceptions
        }

        return null;
    }

    public static string GetName(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }

    public static SelectList ToSelectList<TEnum>(this TEnum enumObj) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

        var values = from TEnum e in Enum.GetValues(typeof(TEnum)) select new { ID = (int)Enum.Parse(typeof(TEnum), e.ToString()), Name = e.ToString() };
        //var values = from TEnum e in Enum.GetValues(typeof(TEnum)) select new { ID = e, Name = e.ToString() };

        return new SelectList(values, "ID", "Name");
    }
    public static SelectList ToSelectList<TEnum>(this TEnum enumObj, string selectedValue) where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

        var values = from TEnum e in Enum.GetValues(typeof(TEnum)) select new { ID = (int)Enum.Parse(typeof(TEnum), e.ToString()), Name = e.ToString() };
        //var values = from TEnum e in Enum.GetValues(typeof(TEnum)) select new { ID = e, Name = e.ToString() };
        if (string.IsNullOrWhiteSpace(selectedValue))
        {
            return new SelectList(values, "ID", "Name", enumObj);
        }
        else
        {
            return new SelectList(values, "ID", "Name", selectedValue);
        }
    }
}
