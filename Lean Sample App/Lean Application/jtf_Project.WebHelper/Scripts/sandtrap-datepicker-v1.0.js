
; (function ($, window, undefined) {

    // Define valid key codes (backspace, tab, home, end, left and right keys, 
    // numbers and forward slash)
    var validKeyCodes = [8, 9, 35, 36, 37, 39, 48, 49, 50, 51, 52, 53, 54,
        55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 111, 191];

    // Constructor
    function datepicker(element) {
        // Assign the DOM element
        this.element = $(element).closest('div');
        // Initialise the date picker
        this.initialise();
    }

    // Initialise the datepicker
    datepicker.prototype.initialise = function () {
        var self = this;
        // Define timer
        this.timer = undefined;;
        if ($.validator) {
            // Override default date error message
            $.validator.unobtrusive.adapters.add('date', function (options) {
                if (options.message) {
                    options.messages['date'] = 'Please enter a valid date';
                    if (options.messages['required'] === "The Date field is required.") {
                        // Dont like the default message!
                        options.messages['required'] = 'Please enter a date';
                    }
                }
            });
            // Override default date validator format to allow dd/mm/yyyy format
            $.validator.methods.date = function (value, element) {
                return this.optional(element) || new DateAU(value).isValid();
            };
        }

        // Declare the main UI components 
        this.container = $(this.element);
        this.input = this.container.children('.datepicker-input');
        this.text = this.container.children('.datepicker-text');
        this.button = this.container.children('.drop-button');
        this.calendar = this.container.children('.calendar');
        this.nextButton = this.calendar.find('.nextButton');
        this.previousButton = this.calendar.find('.previousButton');
        this.dates = this.calendar.find('.calendar-dates');
        // Set dates
        this.selectedDate = new DateAU(this.input.val());
        this.displayMonth = this.selectedDate.getFirstOfMonth();
        this.minDate = new Date().minDate;
        this.maxDate = new Date().maxDate;
        // Get the minimum and maximum dates
        var data = this.container.data();
        if (data['mindate']) {
            this.minDate = new DateAU(data['mindate']);
        }
        if (data['maxdate']) {
            this.maxDate = new DateAU(data['maxdate']);
        }
        // TODO: What if the input has min and max dates?
        // Events

        // Updates the display
        this.input.change(function () {
            // Set the selected date
            self.selectedDate = new DateAU(self.input.val());
            if (self.selectedDate.isValid()) {
                // Ensure its within range
                // TODO: Display validation message
                if (self.selectedDate < self.minDate) {
                    self.selectedDate = self.minDate.clone();
                } else if (self.selectedDate > self.maxDate) {
                    self.selectedDate = self.maxDate.clone();
                }
            }
            // Update display
            self.update(true);
        });

        // Toggle the calendar display
        this.button.mousedown(function (e) {
            // Prevent the input losing focus
            e.preventDefault();
            self.input.focus();
            // Toggle the calendar display
            self.calendar.toggle();

        });

        // Hide the calendar
        this.input.blur(function () {
            // Hide the calendar
            self.calendar.hide();
        });

        this.nextButton.mousedown(function (e) {
            // Prevent the input losing focus
            e.preventDefault();
            // Display the next month
            self.nextMonth();
            // Set timer to increment the calendar while mouse held down
            if ($(this).is(':visible')) {
                // Repeat at 200ms intervals
                self.timer = setInterval(function () {
                    self.nextMonth();
                }, 200);
            }
        });

        this.nextButton.mouseup(function () {
            clearInterval(self.timer);
        });

        this.previousButton.mousedown(function (e) {
            // Prevent the input losing focus
            e.preventDefault();
            // Display the next month
            self.previousMonth();
            // Set timer to increment the calendar while mouse held down
            if ($(this).is(':visible')) {
                // Repeat at 200ms intervals
                self.timer = setInterval(function () {
                    self.previousMonth();
                }, 200);
            }
        });

        this.previousButton.mouseup(function () {
            clearInterval(self.timer);
        });

        // Mouse selection
        this.calendar.find('th, td').mousedown(function (e) {
            // Prevent the input losing focus
            e.preventDefault();
            if ($(this).hasClass('selectedDay')) {
                // The selected date has not changed so exit
                self.calendar.hide();
            } else if ($(this).hasClass('workingDay') || $(this).hasClass('weekendDay')) {
                // Update the value of the selected date
                self.selectedDate.setDate($(this).children('div').text());
                self.selectedDate.setMonth(self.displayMonth.getMonth());
                self.selectedDate.setFullYear(self.displayMonth.getFullYear());
                self.update(true);
                self.calendar.hide();
            }
        });

        // Keyboard navigation
        this.input.keydown(function (e) {
            if (e.keyCode == 115) {
                // F4: toggle the calendar display
                self.calendar.toggle();
            } else if (e.keyCode === 13) {
                // Enter: close the calendar
                e.preventDefault();
                self.calendar.hide();
            }
            else if (e.keyCode === 27) {
                // Escape: close the calendar
                self.calendar.hide();
            } else if (e.keyCode == 107 || e.keyCode == 187) {
                // +: increment selected date
                e.preventDefault();
                if (!self.selectedDate.isValid()) {
                    // Set selected date to today
                    self.selectedDate = new DateAU();
                    self.update(true);
                } else if (self.selectedDate < self.maxDate) {
                    // Increment the selected date if its less than the maximum date
                    self.selectedDate.setDate(self.selectedDate.getDate() + 1);
                    self.update(true);
                } else if (!self.maxDate) {
                    // Increment the selected date
                    self.selectedDate.setDate(self.selectedDate.getDate() + 1);
                    self.update(true);
                }
                // Select all in the input
                self.input.select();
            } else if (e.keyCode == 109 || e.keyCode == 189) {
                // -: decrement date
                e.preventDefault();
                if (!self.selectedDate.isValid()) {
                    // Set selected date to today
                    self.selectedDate = new DateAU();
                    self.update(true);
                } else if (self.minDate && self.selectedDate > self.minDate) {
                    // Decrement the selected date if its greater than the minimum date
                    self.selectedDate.setDate(self.selectedDate.getDate() - 1);
                    self.update(true);
                } else if (!self.minDate) {
                    // Decrement the selected date
                    self.selectedDate.setDate(self.selectedDate.getDate() - 1);
                    self.update(true);
                }
                // Select all in the input
                self.input.select();
            } else if (validKeyCodes.indexOf(e.keyCode) === -1) {
                // Its not a valid key
                e.preventDefault();
            }
        });

    }

    // Increment the calendar display by one month
    datepicker.prototype.nextMonth = function () {
        // Increment the display month
        this.displayMonth.setMonth(this.displayMonth.getMonth() + 1);
        // Draw the calendar
        this.drawCalendar();
    }

    // Decrement the calendar display by one month
    datepicker.prototype.previousMonth = function () {
        // Decrement the display month
        this.displayMonth.setMonth(this.displayMonth.getMonth() - 1);
        // Draw the calendar
        this.drawCalendar();
    }

    // Refresh the calendar based on the display date
    datepicker.prototype.drawCalendar = function () {
        var date = this.displayMonth.clone();
        // Get the month and year of the date
        var month = date.getMonth();
        var year = date.getFullYear();
        // Adjust to the first day of the week
        date.setDate(date.getDate() - date.getDay());
        // Draw the dates
        var rowIndex = 0;
        var colIndex = 0;
        //var current = this.selectedDate === undefined ? 0 : this.selectedDate.getTime();
        for (var i = 0; i < 42; i++) {
            // Get the table cell
            var cell = this.dates.children('tr').eq(rowIndex).children('td').eq(colIndex);
            // Remove existing classes
            cell.removeClass();
            // Add new classes
            if (date < this.minDate) {
                cell.addClass('disabledDay');
            } else if (date > this.maxDate) {
                cell.addClass('disabledDay');
            } else if (date.getMonth() != month) {
                cell.addClass('disabledDay');
            } else if (date.getDay() === 0 || date.getDay() === 6) {
                cell.addClass('weekendDay');
            } else {
                cell.addClass('workingDay');
            }
            // Check if its the selected date
            if (date.getTime() === this.selectedDate.getTime() && !cell.hasClass('disabledDay')) {
                cell.addClass('selectedDay');
            }
            // Set date text
            cell.children('div').text(date.getDate());
            // Increment date and indicies
            date.setDate(date.getDate() + 1);
            colIndex++;
            if ((i + 1) % 7 == 0) {
                // Start a new row
                rowIndex++;
                colIndex = 0;
            }
        }
        // Update header
        $(this.calendar).find('span').text(this.displayMonth.getMonthName() + ' ' + year);
        // Update navigation button visibility
        var minYear = this.minDate.getFullYear();
        var minMonth = this.minDate.getMonth();
        if (year < minYear || (year === minYear && month === minMonth)) {
            clearInterval(this.timer)
            this.previousButton.hide();
        } else {
            this.previousButton.show();
        }
        var maxYear = this.maxDate.getFullYear();
        var maxMonth = this.maxDate.getMonth();

        if (year > maxYear || (year === maxYear && month === maxMonth)) {
            clearInterval(this.timer)
            this.nextButton.hide();
        } else {
            this.nextButton.show();
        }
    }

    // Update the display text and calendar display
    datepicker.prototype.update = function (drawCalendar) {
        if (this.selectedDate.isValid()) {
            this.input.val(this.selectedDate.toShortDateString());
            this.text.text(this.selectedDate.toLongDateString());
            this.displayMonth = this.selectedDate.getFirstOfMonth();
        } else {
            this.text.text('');
            this.displayMonth = new Date().getFirstOfMonth();
        }
        if (drawCalendar) {
            this.drawCalendar();
        }
    }

    // Updates the selected, minimum and maximum dates.
    datepicker.prototype.setDates = function (settings) {
        var date = this.selectedDate.clone();
        var minDate = this.minDate.clone();
        var maxDate = this.maxDate.clone();
        if (settings.date) {
            date = new DateAU(settings.date);
        }
        if (settings.minDate) {
            minDate = new DateAU(settings.minDate);
        }
        if (settings.maxDate) {
            maxDate = new DateAU(settings.maxDate);
        }
        //  Ensure the dates are valid
        if (date < minDate) {
            date = minDate.clone();
        }
        if (maxDate < date) {
            maxDate = date.clone();
        }
        // Update
        this.selectedDate = date;
        this.minDate = minDate;
        this.maxDate = maxDate;
        this.update(true);
    }

    $.fn.datepicker = function () {
        return this.each(function () {
            if (!$.data(this, 'datepicker')) {
                $.data(this, 'datepicker', new datepicker(this));
            }
        });
    }

    $.fn.setDates = function (settings) {        
        return this.each(function () {
            var self = $.data(this, 'datepicker');
            if (!self) {
                self = new datepicker(this);
                $.data(this, 'datepicker', self);
            }
            self.setDates(settings);
        });
    }

    // Date properties
    Date.prototype.monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]

    Date.prototype.dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    Date.prototype.minDate = new Date(-8640000000000000);

    Date.prototype.maxDate = new Date(8640000000000000);

    // Date methods 
    Date.prototype.isValid = function () {
        return !isNaN(this.getTime());
    }

    Date.prototype.clone = function () {
        return new Date(this.getTime());
    }

    Date.prototype.getFirstOfMonth = function () {
        return new Date(this.getFullYear(), this.getMonth(), 1);
    }

    Date.prototype.getMonthName = function () {
        return this.monthNames[this.getMonth()];
    }

    Date.prototype.getDayName = function () {
        return this.dayNames[this.getDay()];
    }

    Date.prototype.toLongDateString = function () {
        return this.getDayName() + ' ' + this.getDate() + ' ' + this.getMonthName() + ' ' + this.getFullYear();
    }

    Date.prototype.toShortDateString = function () {
        return this.getDate() + '/' + (this.getMonth() + 1) + '/' + this.getFullYear();
    }

    // Custom date
    DateAU = function () {
        var date = new Date();
        if (arguments === undefined || arguments.length === 0) {
            date.setMilliseconds(0);
            date.setSeconds(0);
            date.setMinutes(0);
            date.setHours(0);
            return date;
        }
        // Test the argument for a valid format
        var pattern = new RegExp(/^\d{1,2}\/\d{1,2}\/\d{4}($|\s{1}\d{1,2}:\d{2}:\d{2}\s(AM|PM)$)/);
        if (!pattern.test(arguments[0])) {
            date.setDate(Number.NaN);
            return date;
        }
        var args = arguments[0].toString().split(/[\/ ]/);
        var day = parseInt(args[0], 10);
        var month = parseInt(args[1], 10) - 1;
        var year = parseInt(args[2], 10);
        test = new Date(year, month, day);
        if (isNaN(test.getTime())) {
            date.setDate(Number.NaN);
            return date;
        }
        if (test.getDate() !== day || test.getMonth() !== month && test.getFullYear() !== year) {
            date.setDate(Number.NaN);
            return date;
        }
        // Set date properties
        date.setMilliseconds(0);
        date.setSeconds(0);
        date.setMinutes(0);
        date.setHours(0);
        date.setDate(parseInt(args[0], 10));
        date.setMonth(parseInt(args[1], 10) - 1);
        date.setFullYear(parseInt(args[2], 10));
        return date;
    }

}(jQuery));