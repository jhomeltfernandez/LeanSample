using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;


public static class Extensions
{
    public static T GetValueIfNotNULL<T>(this T obj) where T : class, new()
    {
        if (obj != null)
        {
            return obj;
        }
        return new T();
    }

    //for nullable int, decimal, etc
    public static T SetDefaultIFNULL<T>(this T? obj) where T : struct
    {
        if (obj.HasValue)
        {
            return obj.Value;
        }

        return default(T);
    }
    public static bool isNull<T>(this T obj)
    {
        if (obj != null)
        {
            return true;
        }

        return false;
    }

    public static object ToJsonValidation(this ModelStateDictionary modelState)
    {
        var v = from m in modelState.AsEnumerable()
                from e in m.Value.Errors
                select new { m.Key, e.ErrorMessage };

        return new
        {
            IsValid = modelState.IsValid,
            PropertyErrors = v.Where(x => !string.IsNullOrEmpty(x.Key)),
            ModelErrors = v.Where(x => string.IsNullOrEmpty(x.Key))
        };
    }

    public static SelectList MakeSelection(this SelectList list, object selection)
    {
        return new SelectList(list.Items, list.DataValueField, list.DataTextField, selection);
    }

    public static IEnumerable<SelectListItem> MakeSelection(this IEnumerable<SelectListItem> list, object selection)
    {
        if (selection == null) { selection = "-1"; };

        var items = list.ToList();
        foreach (var i in items)
        {
            if (i.Value == selection.ToString())
            {
                i.Selected = true;
            }
            else
            {
                i.Selected = false;
            }
        }
        return items.AsEnumerable();
    }

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + " ...";
    }

    public static string NumberToCurrencyText(this decimal number, MidpointRounding midpointRounding)
    {
        // Round the value just in case the decimal value is longer than two digits
        number = decimal.Round(number, 2, midpointRounding);

        string wordNumber = string.Empty;

        // Divide the number into the whole and fractional part strings
        string[] arrNumber = number.ToString().Split('.');

        // Get the whole number text
        long wholePart = long.Parse(arrNumber[0]);
        string strWholePart = NumberToText(wholePart);

        // For amounts of zero dollars show 'No Dollars...' instead of 'Zero Dollars...'
        wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " Dollar and " : " Dollars and ");

        // If the array has more than one element then there is a fractional part otherwise there isn't
        // just add 'No Cents' to the end
        if (arrNumber.Length > 1)
        {
            // If the length of the fractional element is only 1, add a 0 so that the text returned isn't,
            // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
            long fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
            string strFarctionPart = NumberToText(fractionPart);

            wordNumber += (fractionPart == 0 ? " No" : strFarctionPart) + (fractionPart == 1 ? " Cent" : " Cents");
        }
        else
            wordNumber += "No Cents";

        return wordNumber;           
    }


    public static string NumberToText(this long number)
    {
        StringBuilder wordNumber = new StringBuilder();                       

        string[] powers = new string[] { "Thousand ", "Million ", "Billion " };
        string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        string[] ones = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", 
                                        "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        if (number == 0) { return "Zero"; }
        if (number < 0) 
        { 
            wordNumber.Append("Negative ");
            number = -number;
        }

        long[] groupedNumber = new long[] { 0, 0, 0, 0 };
        int groupIndex = 0;

        while (number > 0)
        {
            groupedNumber[groupIndex++] = number % 1000;
            number /= 1000;
        }

        for (int i = 3; i >= 0; i--)
        {
            long group = groupedNumber[i];

            if (group >= 100)
            {
                wordNumber.Append(ones[group / 100 - 1] + " Hundred ");
                group %= 100;

                if (group == 0 && i > 0)
                    wordNumber.Append(powers[i - 1]);
            }

            if (group >= 20)
            {
                if ((group % 10) != 0)
                    wordNumber.Append(tens[group / 10 - 2] + " " + ones[group % 10 - 1] + " ");
                else
                    wordNumber.Append(tens[group / 10 - 2] + " ");                    
            }
            else if (group > 0)
                wordNumber.Append(ones[group - 1] + " ");

            if (group != 0 && i > 0)
                wordNumber.Append(powers[i - 1]);
        }

        return wordNumber.ToString().Trim();
    }
}
