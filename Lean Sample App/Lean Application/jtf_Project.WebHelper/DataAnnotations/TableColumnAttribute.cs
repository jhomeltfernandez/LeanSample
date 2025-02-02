﻿using System;
using System.Linq;
using System.Web.Mvc;
using jtf_Project.WebHelper.Extensions;

namespace jtf_Project.WebHelper.DataAnnotations
{

    /// <summary>
    /// An attribute used to define additional properties used in rendering tables 
    /// generated by the <see cref="Sandtrap.Web.Html.TableHelper.TableDisplayFor"/> and 
    /// <see cref="Sandtrap.Web.Html.TableHelper.TableEditorFor"/> methods.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// If <see cref="DisplayProperty"/> is applied to a complex type and the type does 
    /// not contain a property named <see cref="DisplayProperty"/>.
    /// </exception>
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumnAttribute : Attribute, System.Web.Mvc.IMetadataAware
    {

        #region .Declarations 

        // Error messages
        private const string _InvalidProperty = "The class definition '{0}' does not contain " + 
            "a public property named '{1}'";

        #endregion

        #region .Constructors 

        /// <summary>
        /// Initialises a new instance of the TableColumnAttribute class with default properties.
        /// </summary>
        public TableColumnAttribute()
        {
        }

        #endregion

        #region .Metadata keys 

        /// <summary>
        /// Gets the key for the <see cref="Exclude"/> property.
        /// </summary>
        public static string ExcludeKey
        {
            get { return "TableColumnExclude"; }
        }

        /// <summary>
        /// Gets the key for the <see cref="IsReadOnly"/> property.
        /// </summary>
        public static string IsReadOnlyKey
        {
            get { return "TableColumnIsReadonly"; }
        }

        /// <summary>
        /// Gets the key for the <see cref="DisplayProperty"/> property.
        /// </summary>
        public static string DisplayPropertyKey
        {
            get { return "TableColumnDisplayProperty"; }
        }

        /// <summary>
        /// Gets the key for the <see cref="IDProperty"/> property.
        /// </summary>
        public static string IDPropertyKey
        {
            get { return "TableColumnIDProperty"; }
        }

        /// <summary>
        /// Gets the key for the <see cref="IncludeTotal"/> property.
        /// </summary>
        public static string IncludeTotalKey
        {
            get { return "TableColumnIncludeTotal"; }
        }

        /// <summary>
        /// Gets the key for the <see cref="NoRepeat"/> property.
        /// </summary>
        public static string NoRepeatKey
        {
            get { return "TableColumnNoRepeat"; }
        }

        #endregion

        #region .Properties 

        /// <summary>
        /// Gets or sets a value indicating if the property should be excluded 
        /// from the table. The default is false.
        /// </summary>
        /// <remarks>
        /// If true, the property is excluded in both readonly and editable tables.
        /// </remarks>
        public bool Exclude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the property should readonly in an 
        /// editable table.
        /// </summary>
        /// <remarks>
        /// If true, hidden inputs are created for the property (including properties
        /// of a complex type) and the text displayed in the table cell is the the value 
        /// of the property or the value of the <see cref="DisplayProperty"/> if the 
        /// property is a complex type.
        /// </remarks>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if a total should be rendered in the 
        /// table footer.
        /// </summary>
        /// <remarks>
        /// The value is ignored if the property is not a numeric type.
        /// </remarks>
        public bool IncludeTotal { get; set; }

        /// <summary>
        /// If applied to a property which is a complex type, gets or sets the name of
        /// property to uniquely identify the property in a select control.
        /// </summary>
        /// <remarks>
        /// The property is only applicable to edit tables.
        /// The value is ignored if the property is not a complex type.
        /// </remarks>
        public string IDProperty { get; set; }

        /// <summary>
        /// If applied to a property which is a complex type, gets or sets the name of
        /// property to display in the table cell.
        /// In a readonly table all other child properties are ignored. 
        /// In an editable table, all other child properties are rendered as hidden inputs.
        /// </summary>
        /// <remarks>
        /// If not provided on a complex type, the properties ToString() method is used to 
        /// as the table cells display text.
        /// The value is ignored if the property is not a complex type.
        /// </remarks>
        public string DisplayProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the property value should be displayed 
        /// in a table if the preceding table row contains the same value.
        /// The default is false.
        /// </summary>
        /// <remarks>
        /// The value is ignored if <see cref="IncludeTotal"/> is true.
        /// </remarks>
        public bool NoRepeat { get; set; }

        #endregion

        #region .Methods 

        /// <summary>
        /// Adds additional metadata value used to render tables.
        /// </summary>
        /// <remarks>
        /// For boolean values, the value is only added if true (then we only need to check 
        /// if the key exists, rather that check for the key and then the get the value.
        /// </remarks>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (Exclude)
            {
                metadata.AdditionalValues[ExcludeKey] = true;
                // Other settings are irrelevant
                return;
            }
            if (IsReadOnly)
            {
                metadata.AdditionalValues[IsReadOnlyKey] = true;
            }
            // Get the display property
            ModelMetadata propertyMetadata = null;
            if (IDProperty != null && metadata.IsComplexType)
            {
                // Check the ID property exists
                propertyMetadata = metadata.Properties
                    .FirstOrDefault(m => m.PropertyName == IDProperty);
                if (propertyMetadata == null)
                {
                    throw new ArgumentException(string
                        .Format(_InvalidProperty, metadata.ModelType.Name, IDProperty));
                }
                metadata.AdditionalValues[IDPropertyKey] = IDProperty;
            }
            if (DisplayProperty != null && metadata.IsComplexType)
            {
                // Check the display property exists
                propertyMetadata = metadata.Properties
                    .FirstOrDefault(m => m.PropertyName == DisplayProperty);
                if (propertyMetadata == null)
                {
                    throw new ArgumentException(string
                        .Format(_InvalidProperty, metadata.ModelType.Name, DisplayProperty));
                }
                metadata.AdditionalValues[DisplayPropertyKey] = DisplayProperty;
            }
            // If rendering totals, check we can
            if (IncludeTotal)
            {
                if (propertyMetadata != null && propertyMetadata.ModelType.IsNumeric())
                {
                    metadata.AdditionalValues[IncludeTotalKey] = true;
                }
                else if (metadata.ModelType.IsNumeric())
                {
                    metadata.AdditionalValues[IncludeTotalKey] = true;
                }
                else
                {
                    // Reset
                    IncludeTotal = false;
                }
            }
            if (NoRepeat && !IncludeTotal)
            {
                metadata.AdditionalValues[NoRepeatKey] = true;
            }
        }

        #endregion

    }

}
