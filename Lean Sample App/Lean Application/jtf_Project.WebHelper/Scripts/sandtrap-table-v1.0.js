/// <reference path="sandtrap-numeric-v1.0.js" />

; (function ($, window, undefined) {

    // Constants
    var rowDeleted = 'rowDeleted.table';
    var rowActivated = 'rowActivated.table';

    //Defaults
    var defaults = {
        // TODO: Any defaults and options?
    };


    // Constructor
    function table(element, options) {
        // Assign the DOM element
        this.element = $(element);
        this.options = $.extend({}, defaults, options);
        this.defaults = defaults;
        // Initialise the select
        this.initialise();
    }

    table.prototype.initialise = function () {

        var self = this;
        // Declare the main UI components
        this.body = $(this.element).children('tbody').first();
        this.newRow = $(this.element).children('tbody').last().children('tr');
        this.footer = $(this.element).children('tfoot').children('tr');
        this.addButton = this.footer.find('.add-button');
        // Set properties
        this.fieldName = $(this.element).attr('id');
        var data = $(this.element).data();
        this.isDirtyProperty = data['isdirtyproperty'];
        this.isActiveProperty = data['isactiveproperty'];
        this.newRowIndex = this.body.children('tr').length;
        // Ensure referenced plugins are present
        if (this.newRow.find('.numeric-input').length > 0 && !$.fn.numeric) {
            // TODO: Load the plugin (from where?) or combine all into one file or bundle?
            console.error('The sandtrap-numeric plugin not loaded');
            return;
        }
        if (this.newRow.find('.select-input').length > 0 && !$.fn.select) {
            // TODO: Load the plugin (from where?) or combine all into one file or bundle?
            console.error('The sandtrap-select plugin not loaded');
            return;
        }
        // Attach plugins
        this.body.find('.numeric-input').numeric();
        this.body.find('.select-input').children('input[type="text"]').select();
        // Check for the IsActive and IsDirty inputs
        var inputs = this.newRow.find('input');
        if (this.isActiveProperty !== undefined) {
            // Find the corresponding input
            var attr = '[name="' + this.fieldName + '[#].' + this.isActiveProperty + '"]';
            var isActiveInput = inputs.filter(attr);
            if (isActiveInput.length !== 1) {
                // No match found so prevent further searching
                this.isActiveProperty = undefined;
            }
        }
        if (this.isDirtyProperty !== undefined) {
            // Find the corresponding input
            var attr = '[name="' + this.fieldName + '[#].' + this.isDirtyProperty + '"]';
            var isDirtyInput = inputs.filter(attr);
            if (isDirtyInput.length !== 1) {
                // No match found so prevent further searching
                this.isDirtyProperty = undefined;
            }
        }

        // **************Events****************

        // Add new row to the table
        this.addButton.click(function (e) {
            self.addRow();
        });

        // Delete new rows from the table or archive/activate existing rows
        this.body.on('click', '.table-button', function () {
            var row = $(this).closest('tr');
            self.deleteRow(row);
        });

        // Track changes
        this.body.on('change itemSelected.select', 'input', function () {
            // TODO: What about standard html select
            if (self.isDirtyProperty === undefined) {
                // Remove the hander
                self.body.off('change itemSelected.select');
                return;
            }
            // Find the corresponding input
            var row = $(this).closest('tr');
            var inputs = row.find('input');
            var attr = '[name="' + self.fieldName + '[' + row.index() + '].' + self.isDirtyProperty + '"]';
            var isDirtyInput = inputs.filter(attr);
            // TODO: Sandtrap select uses hidden inputs which do not have a defaultValue
            // Need to fix it and delete this
            if ($(this).parent().is('.select-input')) {
                isDirtyInput.val('True');
                return;
            }
            isDirtyInput.val(self.isDirty(row) ? 'True' : 'False');
        });

        // Keyboard events
        this.element.on('keydown', 'input', function (e) {
            // Add row on CTRL+SHIFT+PLUS, delete row on Ctrl+SHIFT+MINUS
            //if (e.ctrlKey && e.shiftKey) {
            //    if (e.which === 107) {
            //        // TODO: This causes the first item to be selected in
            //        //a select control!!
            //        self.addRow()
            //    } else if (e.which === 109) {
            //        var row = $(this).closest('tr');
            //        self.deleteRow(row);
            //    };
            //}
        });

    }

    // **************Methods****************

    // Update column totals
    table.prototype.updateTotals = function (element) {
        var total = 0;
        var colIndex = $(element).closest('td').index();
        // Calculate the totals of active inputs in the column
        $.each(this.body.children('tr'), function (index, row) {
            if (!$(row).hasClass('archived')) {
                total += new Number($(row).children('td').eq(colIndex).find('.numeric-input').val());
            }
        });
        // Update the footer
        // TODO: need to get format of numeric input
        if ($(element).prev('').hasClass('currency')) {
            this.footer.children('td').eq(colIndex).text(total.toCurrency());
        } else {
            this.footer.children('td').eq(colIndex).text(total.toFixed(2));
        }
    }

    // Adds a new row to the table
    table.prototype.addRow = function () {
        // Clone the new row

        // Why doesn't this work (attach the plugins)?
        //var clone = this.newRow.clone(true, true);

        var clone = this.newRow.clone();
        // Update the index of the clone
        clone.html($(clone).html().replace(/\[#\]/g, '[' + this.newRowIndex + ']'));
        clone.html($(clone).html().replace(/"%"/g, '"' + this.newRowIndex + '"'));
        this.newRowIndex++;
        // Mark it as new
        clone.data('isnew', true); 
        // Add the new row
        this.body.append(clone);
        // Attach plugins
        clone.find('.select-input').children('input[type="text"]').select();
        clone.find('.numeric-input').numeric();

        // Add validation
        if ($.validator) {
            // TODO: can we just add the validation to the clone without complete parse of the form?
            var form = this.element.closest('form');
            // Remove current form validation
            form.removeData('validator').removeData('unobtrusiveValidation');
            // Parse the form again
            $.validator.unobtrusive.parse('form');
        }
        // Set focus to the first non hidden input
        clone.find('input:not([type=hidden])').first().focus();
    }

    // Delete new rows from the table or archive/activate existing rows
    table.prototype.deleteRow = function (row) {
        var self = this;
        row = $(row);
        if (row.data('isnew')) {
            // Its a new row so remove it (it does not need to be posted back because it never existed)
            row.remove();
        } else {
            var inputs = row.find('input');
            var checkboxes = row.find('input[type="checkbox"]');
            var isActiveInput = undefined;
            var isDirtyInput = undefined;
            if (this.isActiveProperty !== undefined) {
                // Find the corresponding input
                var attr = '[name="' + this.fieldName + '[' + row.index() + '].' + this.isActiveProperty + '"]';
                isActiveInput = inputs.filter(attr);
            }
            if (this.isDirtyProperty !== undefined) {
                // Find the corresponding input
                var attr = '[name="' + this.fieldName + '[' + row.index() + '].' + this.isDirtyProperty + '"]';
                isDirtyInput = inputs.filter(attr);
            }
            if (row.hasClass('archived')) {
                // Its an existing row so mark it as active
                row.removeClass('archived');
                // Allow user interaction
                inputs.prop('readonly', false);
                checkboxes.off();
                isActiveInput.val('True');
                isDirtyInput.val(this.isDirty(row) ? 'True' : 'False');
                // Raise event
                $(this.element).trigger({
                    type: rowActivated,
                    index: row.index()
                });
            } else {
                // Archive the row
                row.addClass('archived');
                inputs.prop('readonly', true);
                checkboxes.on('click', function () {
                    return false;
                });
                isActiveInput.val('False');
                isDirtyInput.val('True');
                // Raise event
                $(this.element).trigger({
                    type: rowDeleted,
                    index: row.index()
                });
            }
        }
        // Update totals
        row.find('.numeric-input').each(function () {
            //self.upda
            self.updateTotals($(this));
        });
    }

    // Returns a value indicating if row values have changed from their original values
    table.prototype.isDirty = function (row) {
        var isDirty = false;
        var inputs = $(row).find('input');
        $.each(inputs, function () {
            var input = $(this);
            // TODO: What about the standard html select
            if (input.is(':checkbox') && input.prop('checked') !== input.prop('defaultChecked')) {
                isDirty = true;
                return false;
            } else if (input.val() !== input.prop('defaultValue')) {
                isDirty = true;
                return false;
            }
        });
        return isDirty;
    }

    // Table definition
    $.fn.table = function (options) {
        return this.each(function () {
            if (!$.data(this, 'table')) {
                $.data(this, 'table', new table(this, options));
            }
        });
    }

}(jQuery, window));