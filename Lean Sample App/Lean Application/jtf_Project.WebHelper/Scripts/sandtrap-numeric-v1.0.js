

; (function ($, window, undefined) {

    // Enums
    var numberFormat = Object.freeze({ NUMBER: 'number', CURRENCY: 'currency', PERCENT: 'percent' })
    // Constants
    var _background = 'background-color;'
    var _color = 'color';
    var _transparent = 'transparent';
    //Defaults
    var defaults = {
        decimalPlaces: undefined
    };

    // Constructor
    function numeric(element, options) {
        // Assign the DOM element
        this.element = $(element).closest('div');
        this.options = $.extend({}, defaults, options);
        this.defaults = defaults;
        // Initialise the select
        this.initialise();
    }


    // Initialise
    numeric.prototype.initialise = function () {
        var self = this;
        // Declare the main UI components
        this.container = $(this.element);
        this.displayText = $(this.element).children('.numeric-text');
        this.input = $(this.element).children('.numeric-input');
        // Get current input colours
        this.colour = this.input.css(_color);
        this.background = this.input.css(_background);
        // Determine format
        this.format = numberFormat.NUMBER;
        if (this.displayText.hasClass('currency')) {
            this.format = numberFormat.CURRENCY;
        } else if (this.displayText.hasClass('percent')) {
            this.format = numberFormat.PERCENT;
        }
        this.decimals = this.options.decimalPlaces;
        if (this.decimals === undefined) {
            this.decimals = this.container.data['decimalplaces'];
        } else {
            this.formatDisplay();
        }
        // Have the display text adopt the inputs colour and background
        //this.displayText.css(_color, _background);
        // Make the input transparent
        this.input.css(_color, _transparent).css(_background, _transparent);

        // Events

        // Sets the colour and background colors
        this.input.focus(function (e) {
            self.displayText.hide();
            self.input.css(_color, self.colour).css(_background, self.background);
        });

        // Allow only numbers, decimal point and minus sign
        this.input.keypress(function (e) {
            var k = e.keyCode;
            if (!((k > 47 && k < 58) || k == 46 || k == 45)) {
                window.event.returnValue = false;
            };
        });

        // Sets the colour and background colors and formats the display text
        this.input.blur(function (e) {
            // Format display text
            self.formatDisplay();
            // Hide the input text and show the display text
            self.input.css(_color, _transparent).css(_background, _transparent);
            self.displayText.show();
        });

    }

    numeric.prototype.formatDisplay = function () {
        var value = this.input.val();
        if (value.length === 0) {
            this.displayText.removeClass('negative').text('');
            return;
        }
        var decimals = this.decimals;
        var number = new Number(value);
        if (number < 0) {
            this.displayText.addClass('negative');
        }
        else {
            this.displayText.removeClass('negative');
        }
        if (this.format === numberFormat.PERCENT) {
            this.displayText.text(number.toPerent())
        } else if (this.format === numberFormat.CURRENCY) {
            this.displayText.text(number.toCurrency())
        } else {
            this.displayText.text(number.toFixed(decimals));
        }
    }

    // Numeric definition
    $.fn.numeric = function (options) {
        return this.each(function () {
            if (!$.data(this, 'numeric')) {
                $.data(this, 'numeric', new numeric(this, options));
            }
        });
    }

    $.fn.decimalToCurrency = function (num) {
        console.log('decimal to currency');
    }

    Number.prototype.toCurrency = function () {
        number = this.toFixed(2);
        var isNegative = number < 0 ? true : false;
        var values = number.toString().split('.')
        var dollars = isNegative ? values[0].substr(1) : values[0];
        var cents = values[1];
        var regex = /(\d+)(\d{3})/;
        while (regex.test(dollars)) {
            dollars = dollars.replace(regex, '$1' + ',' + '$2');
        }
        if (isNegative) {
            return '-$' + dollars + '.' + cents
        } else {
            return '$' + dollars + '.' + cents
        }
    }

    Number.prototype.toPercent = function () {
        return (this * 100) + '%';
    }

}(jQuery, window));