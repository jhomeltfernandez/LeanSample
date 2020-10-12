using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using jtf_Project.WebHelper.Extensions;

namespace jtf_Project.WebHelper.Html
{

    /// <summary>
    /// Renders the html for date pickers and calendars.
    /// </summary>
    public static class DatePickerHelper
    {

        #region .Declarations 

        // Error messages
        private const string _NotADate = "The property {0} is not a type of System.DateTime or System.Nullable<DateTime>";


        #endregion

        #region .Methods 

        /// <summary>
        /// Returns the html for a date picker.
        /// </summary>
        /// <param name="helper">
        /// The System.Web.Mvc.HtmlHelper instance that this method extends.
        /// </param>
        /// <param name="expression">
        /// An expression that identifies the property to display.
        /// </param>
        /// <param name="minDate">
        /// The minimum date that can be selected in the date picker.
        /// </param>
        /// <param name="maxDate">
        /// The maximum date that can be selected in the date picker.
        /// </param>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TValue>> expression, DateTime? minDate = null, DateTime? maxDate = null)
        {
            // Get the model metadata
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            // Get the fully qualified name of the property
            string fieldName = ExpressionHelper.GetExpressionText(expression);

            // Return the html
            return MvcHtmlString.Create(DatePickerForHelper(helper, metadata, fieldName, minDate, maxDate));


            //return MvcHtmlString.Create(container.ToString());









            //// Validate the model type is DateTime or Nullable<DateTime>
            //if (!(metadata.Model is DateTime || Nullable.GetUnderlyingType(metadata.ModelType) == typeof(DateTime)))
            //{
            //    throw new ArgumentException(string.Format(_NotADate, fieldName));
            //}
            ////  Validate dates
            //if (minDate.HasValue && maxDate.HasValue && minDate.Value >= maxDate.Value)
            //{
            //    throw new ArgumentException("The minimum date cannot be greater than the maximum date");
            //}
            //if (minDate.HasValue && metadata.Model != null && (DateTime)metadata.Model < minDate.Value)
            //{
            //    throw new ArgumentException("The date cannot be less than the miniumum date");
            //}
            //if (maxDate.HasValue && metadata.Model != null && (DateTime)metadata.Model > maxDate.Value)
            //{
            //    throw new ArgumentException("The date cannot be greater than the maximum date");
            //}
            ////  Construct date picker
            //StringBuilder html = new StringBuilder();
            //// Add display text
            //html.Append(DatePickerText(metadata));
            //// Add input
            //html.Append(DatePickerInput(helper, metadata, fieldName));
            //// Add drop button
            //html.Append(ButtonHelper.DropButton());
            //// Get the default date to display
            //DateTime date = DateTime.Today;
            //bool isSelected = false;
            //// If a date has been provided, use it and mark it as selected
            //if (metadata.Model != null)
            //{
            //    date = (DateTime)metadata.Model;
            //    isSelected = true;
            //}
            //// Add calendar
            //html.Append(Calendar(date, isSelected, minDate, maxDate));
            //// Build container
            //System.Web.Mvc.TagBuilder container = new System.Web.Mvc.TagBuilder("div");
            //container.AddCssClass("datepicker-container");
            //// Add min and max date attributes
            //if (minDate.HasValue)
            //{
            //    container.MergeAttribute("data-mindate", string.Format("{0:d/M/yyyy}", minDate.Value));
            //}
            //if (maxDate.HasValue)
            //{
            //    container.MergeAttribute("data-maxdate", string.Format("{0:d/M/yyyy}", maxDate.Value));
            //}
            //container.InnerHtml = html.ToString();
            //// Return the html
            //return MvcHtmlString.Create(container.ToString());
        }

        #endregion

        #region .Helper methods 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="metadata"></param>
        /// <param name="fieldName"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        private static string DatePickerForHelper(System.Web.Mvc.HtmlHelper helper, ModelMetadata metadata, 
            string fieldName, DateTime? minDate = null, DateTime? maxDate = null)
        {
            // Validate the model type is DateTime or Nullable<DateTime>
            if (!(metadata.Model is DateTime || Nullable.GetUnderlyingType(metadata.ModelType) == typeof(DateTime)))
            {
                throw new ArgumentException(string.Format(_NotADate, fieldName));
            }
            //  Validate dates
            if (minDate.HasValue && maxDate.HasValue && minDate.Value >= maxDate.Value)
            {
                throw new ArgumentException("The minimum date cannot be greater than the maximum date");
            }
            if (minDate.HasValue && metadata.Model != null && (DateTime)metadata.Model < minDate.Value)
            {
                throw new ArgumentException("The date cannot be less than the miniumum date");
            }
            if (maxDate.HasValue && metadata.Model != null && (DateTime)metadata.Model > maxDate.Value)
            {
                throw new ArgumentException("The date cannot be greater than the maximum date");
            }
            //  Construct date picker
            StringBuilder html = new StringBuilder();
            // Add display text
            html.Append(DatePickerText(metadata));
            // Add input
            html.Append(DatePickerInput(helper, metadata, fieldName));
            // Add drop button
            html.Append(ButtonHelper.DropButton());
            // Get the default date to display
            DateTime date = DateTime.Today;
            bool isSelected = false;
            // If a date has been provided, use it and mark it as selected
            if (metadata.Model != null)
            {
                date = (DateTime)metadata.Model;
                isSelected = true;
            }
            // Add calendar
            html.Append(Calendar(date, isSelected, minDate, maxDate));
            // Build container
            System.Web.Mvc.TagBuilder container = new System.Web.Mvc.TagBuilder("div");
            container.AddCssClass("datepicker-container");
            // Add min and max date attributes
            if (minDate.HasValue)
            {
                container.MergeAttribute("data-mindate", string.Format("{0:d/M/yyyy}", minDate.Value));
            }
            if (maxDate.HasValue)
            {
                container.MergeAttribute("data-maxdate", string.Format("{0:d/M/yyyy}", maxDate.Value));
            }
            container.InnerHtml = html.ToString();
            // Return the html
            return container.ToString(); ;
        }

        /// <summary>
        /// Returns the html for the date picker display text (visible when the datepicker does not have focus)
        /// </summary>
        /// <param name="metadata">
        /// The meta data of the property to display the date for.
        /// </param>
        private static string DatePickerText(ModelMetadata metadata)
        {
            System.Web.Mvc.TagBuilder text = new System.Web.Mvc.TagBuilder("div");
            text.AddCssClass("datepicker-text");
            // Add essential stype properties
            text.MergeAttribute("style", "position:absolute;z-index:-1000;");
            if (metadata.Model != null)
            {
                text.InnerHtml = ((DateTime)metadata.Model).ToLongDateString();
            }
            // Return the html
            return text.ToString();
        }

        /// <summary>
        /// Returns the html for the date picker input, including validation attributes and message.
        /// </summary>
        /// <param name="helper">
        /// The html helper.
        /// </param>
        /// <param name="metadata">
        /// The meta data of the property to display the date for.
        /// </param>
        /// <param name="name">
        /// The fully qualified name of the property.
        /// </param>
        /// <returns></returns>
        private static string DatePickerInput(System.Web.Mvc.HtmlHelper helper, System.Web.Mvc.ModelMetadata metadata, string name)
        {        
            // Construct the input
            System.Web.Mvc.TagBuilder input = new System.Web.Mvc.TagBuilder("input");
            input.AddCssClass("datepicker-input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("id", System.Web.Mvc.HtmlHelper.GenerateIdFromName(name));
            input.MergeAttribute("autocomplete", "off");
            input.MergeAttribute("name", name);
            input.MergeAttributes(helper.GetUnobtrusiveValidationAttributes(name, metadata));
            if (metadata.Model != null)
            {
                input.MergeAttribute("value", ((DateTime)metadata.Model).ToShortDateString());
            }
            // Return the html
            return input.ToString();
        }

        /// <summary>
        /// Return the html for a calendar.
        /// </summary>
        /// <param name="date">
        /// A date in the month to display.
        /// </param>
        /// <param name="isSelected">
        /// A value indicating if the date should be marked as selected.
        /// </param>
        /// <param name="minDate">
        /// The minimum date that can be selected.
        /// </param>
        /// <param name="maxDate">
        /// The maximum date that can be selected.
        /// </param>
        private static string Calendar(DateTime date, bool isSelected, DateTime? minDate, DateTime? maxDate)
        {
            StringBuilder html = new StringBuilder();
            // Add header
            html.Append(CalendarHeader(date, minDate, maxDate));
            // Add body
            html.Append(Month(date, isSelected, minDate, maxDate));
            // Construct table
            System.Web.Mvc.TagBuilder table = new System.Web.Mvc.TagBuilder("table");
            table.InnerHtml = html.ToString();
            // Construct inner container that can optionally be styled as position:absolute if within a date picker
            System.Web.Mvc.TagBuilder inner = new System.Web.Mvc.TagBuilder("div");
            inner.AddCssClass("container");
            inner.InnerHtml = table.ToString();
            // Construct outer container
            System.Web.Mvc.TagBuilder outer = new System.Web.Mvc.TagBuilder("div");
            outer.AddCssClass("calendar");
            outer.InnerHtml = inner.ToString();
            // Return the html
            return outer.ToString();
        }

        /// <summary>
        /// Returns the html for the table header displaying the selected month, navigation buttons 
        /// and days of the week.
        /// </summary>
        /// <param name="date">
        /// A date in month to display.
        /// </param>
        /// <param name="minDate">
        /// The minimum date that can be seleccted.
        /// </param>
        /// <param name="maxDate">
        /// The maximum date that can be seleccted.
        /// </param>
        private static string CalendarHeader(DateTime date, DateTime? minDate, DateTime? maxDate)
        {
            StringBuilder html = new StringBuilder();
            // Add month label and navigation buttons
            html.Append(MonthHeader(date, minDate, maxDate));
            // Add day of week labels
            html.Append(WeekHeader());
            // Construct table header
            System.Web.Mvc.TagBuilder header = new System.Web.Mvc.TagBuilder("thead");
            header.InnerHtml = html.ToString();
            // Return the html
            return header.ToString();
        }

        /// <summary>
        /// Returns the html for the table header row displaying the current month and navigation buttons.
        /// </summary>
        /// <param name="date">
        /// A date in the month to display.
        /// </param>
        /// <param name="minDate">
        /// The minimum date that can be selected.
        /// </param>
        /// <param name="maxDate">
        /// The maximmum date that can be selected.
        /// </param>
        private static string MonthHeader(DateTime date, DateTime? minDate, DateTime? maxDate)
        {
            StringBuilder html = new StringBuilder();
            // Add previous month navigation button
            bool hidePrevious = minDate.HasValue && date.FirstOfMonth() <= minDate.Value;
            html.Append(NavigationButton("previousButton", hidePrevious));
            // Add month label
            System.Web.Mvc.TagBuilder label = new System.Web.Mvc.TagBuilder("span");
            label.InnerHtml = string.Format("{0:MMMM yyyy}", date);
            System.Web.Mvc.TagBuilder cell = new System.Web.Mvc.TagBuilder("th");
            cell.MergeAttribute("colspan", "5");
            cell.InnerHtml = label.ToString();
            html.Append(cell);
            // Add next month navigation button
            bool hideNext = maxDate.HasValue && date.LastOfMonth() >= maxDate.Value;
            html.Append(NavigationButton("nextButton", hideNext));
            // Construct header row
            System.Web.Mvc.TagBuilder header = new System.Web.Mvc.TagBuilder("tr");
            header.InnerHtml = html.ToString();
            // Return the html
            return header.ToString();
        }

        /// <summary>
        /// Returns the html for a table row displaying the days of the week.
        /// </summary>
        private static string WeekHeader()
        {
            // Build a table cell for each day of the week
            string[] daysOfWeek = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            StringBuilder html = new StringBuilder();
            for (int i = 0; i < 7; i++)
            {
                System.Web.Mvc.TagBuilder day = new System.Web.Mvc.TagBuilder("td");
                day.InnerHtml = daysOfWeek[i];
                html.Append(day.ToString());
            }
            // Construct the table row
            System.Web.Mvc.TagBuilder week = new System.Web.Mvc.TagBuilder("tr");
            week.AddCssClass("calendar-daysOfWeek");
            week.InnerHtml = html.ToString();
            // Return the html
            return week.ToString();
        }

        /// <summary>
        /// Return the html for the table body representing a month in the calendar.
        /// </summary>
        /// <param name="date">
        /// A date in the month to display.
        /// </param>
        /// <param name="isSelected">
        /// A value indicating if the date should be marked as selected.
        /// </param>
        /// <param name="minDate">
        /// The minimum date that can be seleccted.
        /// </param>
        /// <param name="maxDate">
        /// The maximum date that can be seleccted.
        /// </param>
        private static string Month(DateTime date, bool isSelected, DateTime? minDate, DateTime? maxDate)
        {   
            // Get the first and last days of the month
            DateTime firstOfMonth = date.FirstOfMonth();
            DateTime lastOfMonth = date.LastOfMonth();
            // Get the first date to display in the calendar (may be in the previous month)
            DateTime startDate = firstOfMonth.AddDays(-(int)firstOfMonth.DayOfWeek);
            // Build a table containing 6 rows (weeks) x 7 columns (day of week)
            StringBuilder month = new StringBuilder();
            StringBuilder html = new StringBuilder();
            for (int i = 0;  i < 42; i++)
            {
                // Get the date to display
                DateTime displayDate = startDate.AddDays(i);
                // Determine if the date is selectable
                bool canSelect = true;
                if (displayDate.Month != date.Month)
                {
                    canSelect = false;
                }
                else if (minDate.HasValue && displayDate < minDate.Value)
                {
                    canSelect = false;
                }
                else if (maxDate.HasValue && displayDate > maxDate.Value)
                {
                    canSelect = false;
                }
                html.Append(Day(displayDate, isSelected && date == displayDate, canSelect));
                if (i % 7 == 6)
                {
                    // Its the end of the week, so start a new row in the table
                    System.Web.Mvc.TagBuilder week = new System.Web.Mvc.TagBuilder("tr");
                    week.InnerHtml = html.ToString();
                    month.Append(week.ToString());
                    html.Clear();
                }
            }
            // Construct the table body
            System.Web.Mvc.TagBuilder calendar = new System.Web.Mvc.TagBuilder("tbody");
            calendar.AddCssClass("calendar-dates");
            calendar.InnerHtml = month.ToString();
            // Return the html
            return calendar.ToString();
        }

        /// <summary>
        /// Return the html for a table cell representing a date in the calendar.
        /// </summary>
        /// <param name="date">
        /// The date to display.
        /// </param>
        /// <param name="isSelected">
        /// A value indicating if the date is selected in the calendar.
        /// </param>
        /// <param name="canSelect">
        /// A value indicating if the date can be selected.
        /// </param>
        private static string Day(DateTime date, bool isSelected, bool canSelect)
        {
            // Construct container
            System.Web.Mvc.TagBuilder day = new System.Web.Mvc.TagBuilder("div");
            day.InnerHtml = date.Day.ToString();
            // Construct table cell
            System.Web.Mvc.TagBuilder cell = new System.Web.Mvc.TagBuilder("td");
            if (!canSelect)
            {
                cell.AddCssClass("disabledDay");
            }
            else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                cell.AddCssClass("weekendDay");
            }
            else
            {
                cell.AddCssClass("workingDay");
            }
            if (isSelected)
            {
                cell.AddCssClass("selectedDay");
            }
            cell.InnerHtml = day.ToString();
            // Return the html
            return cell.ToString();
        }

        /// <summary>
        /// Returns the html for a table cell with a navigation button.
        /// </summary>
        /// <param name="className">
        /// The class name to apply to the button.
        /// </param>
        /// <param name="hide">
        /// A value indicating if the button should be rendered as hidden.
        /// </param>
        private static string NavigationButton(string className, bool hide)
        {
            // Build the button
            System.Web.Mvc.TagBuilder button = new System.Web.Mvc.TagBuilder("button");
            button.AddCssClass(className);
            button.MergeAttribute("type", "button");
            button.MergeAttribute("tabindex", "-1");
            if (hide)
            {
                button.MergeAttribute("style", "display:none;");
            }
            // Construct the table cell
            System.Web.Mvc.TagBuilder cell = new System.Web.Mvc.TagBuilder("th");
            cell.InnerHtml = button.ToString();
            // Return the html
            return cell.ToString();
        }

        #endregion

    }

}