
// TODO: 
// Scroll only in increments of the item height
// Unobtrusive validation not working 

; (function ($, window, undefined) {

    // Constants
    var excluded = '.group, .disabled';
    var itemSelected = 'itemSelected.select';
    var itemAdded = 'itemAdded.select';
    // Enums
    var scrollDirection = Object.freeze({ UP: 'up', DOWN: 'down' })
    //Defaults
    var defaults = {
        maxVisibleItems: 8,
        randomSearchCharacterCount: 3,
        ajaxLoadCharacterCount: 2,
        separatorCharacter: ' &#8226; '
    };

    // Constructor
    function select(element, options) {
        // Assign the DOM element
        this.element = $(element).closest('.select');
        this.options = $.extend({}, defaults, options);
        this.defaults = defaults;
        // Initialise the select
        this.initialise();
    }

    // Initialise the select
    select.prototype.initialise = function () {
        var self = this;
        // Declare the main UI components 
        var input = $(this.element).children('.select-input');
        var validation = $(this.element).children('.select-validation');
        var list = $(this.element).children('.select-list');
        this.search = input.find('input[type="text"]');
        this.display = input.find('div');
        this.button = input.find('button');
        this.values = input.find('input[type="hidden"]');
        //this.validationInput = validation.find('input');
        this.validationMessage = validation.find('span');
        this.dropDown = list.children('ul');
        this.items = list.find('div');
        // Set the scroll height
        var height = 0;
        if (this.items.length === 0 || this.items.eq(0).height() === 0) {
            // Create dummy list to check the height
            var list = $('<div class="select-list"><ul><li><div>Text</div></li></ul></div>');
            // Append to closest form (in case the select itself is hidden)
            $(this.element).closest('form').append(list);
            height = list.find('div').outerHeight() * this.options.maxVisibleItems;
            list.remove();
        }
        else {
            height = this.items.eq(0).outerHeight() * this.options.maxVisibleItems;
        }
        this.scrollHeight = height;
        // Set the maximum height of the drop-down (adjusted for padding and borders)
        height += this.dropDown.outerHeight() - this.dropDown.height();
        this.dropDown.css('max-height', height);
        // Declare properties
        this.cache = {};
        this.isVisible = false;
        this.selectedItem = undefined;
        this.url = undefined;
        this.urlParameter = undefined;
        this.idProperty = undefined;
        this.displayProperty = undefined;
        this.isRequired = false;
        // Get the data
        var data = $(this.element).data();
        this.propertyName = data['propertyname'];
        if (data['idproperty']) {
            this.idProperty = data['idproperty'];
        }
        if (data['displayproperty']) {
            this.displayProperty = data['displayproperty'];
        }
        if (data['url']) {
            this.url = data['url'];
        }
        if (data['urlparameter']) {
            this.urlParameter = data['urlparameter'];
        }
        if (data['isrequired']) {
            this.isRequired = true;
            // There may be a validation error message for a [Required] attribute
            // which should be removed if something is selected
            // Assume its a sibling - or use $(window).find('span')?
            // TODO: Find a better solution that works with onobtrusive calidation
            this.jQueryValidationMessage = $(this.element).siblings('span').filter(function () {
                return $(this).attr('data-valmsg-for').toLowerCase() === self.propertyName.toLowerCase()
            });
        }
        // Set the selected item
        var selected = this.items.filter('.selected');
        if (selected.length > 0) {
            this.selectedItem = selected.first();
        }
        if (this.url != undefined && this.display.text().length > 0) {
            // Simulate the search text
            var searchText = this.display.text().substr(0, this.options.ajaxLoadCharacterCount);
            // Get the item selected item ID
            var idProperty = this.propertyName + '.' + this.idProperty;
            var id = this.values.filter(function () {
                return $(this).attr('name').toLowerCase() === idProperty.toLowerCase();
            }).val();
            this.fetchItems(searchText, id, function () {
                // Format the display
                self.update();
            });
        } else {
            // Format the display
            this.update();
        }

        // Declare events
        this.button.mousedown(function (e) {
            // Prevent the input losing focus
            e.preventDefault();
            // Ensure the search box has focus
            self.search.focus();
            // Toggle the dropdown display
            if (self.isVisible) {
                self.hideItems();
            } else {
                self.showItems();
            }
        });

        this.display.mousedown(function (e) {
            // Prevent selection of the display text
            e.preventDefault();
            // Ensure the search text is focused
            if (!self.search.is(':focus')) {
                self.search.focus();
            }
        });

        this.search.keydown(function (e) {
            if (e.keyCode === 115) {
                // F4 key (toggle dropdown display)
                if (self.isVisible) {
                    self.hideItems();
                } else {
                    self.showItems();
                }
                return;
            }
            if (e.keyCode === 13) {
                // RETURN
                if (self.isVisible) {
                    e.preventDefault()
                }
                self.hideItems();
                return;
            }
            if (e.keyCode == 27) {
                // ESCAPE key
                self.hideItems();
                return;
            }
            // Get the selectable items
            var selectableItems = self.getSelectableItems();
            if (selectableItems.length === 0) {
                return;
            }
            if (e.keyCode === 40) {
                // DOWN key (select next item)
                if (self.selectedItem === undefined) {
                    // Nothing has been selected yet so select the first item
                    self.selectedItem = selectableItems.first().addClass('selected');
                    // Update
                    self.update()
                    return;
                }
                else {
                    // Get the next selectable item
                    var index = selectableItems.index(self.selectedItem) + 1;
                    selectableItems = selectableItems.slice(index);
                    if (selectableItems.length === 0) {
                        // We are already at the last item
                        return
                    }
                    // Unselect the current item
                    self.selectedItem.removeClass('selected');
                    // Select the next item              
                    self.selectedItem = selectableItems.first().addClass('selected');
                    // Bring into view
                    self.bringIntoView(scrollDirection.DOWN);
                    // Update
                    self.update();
                }
            } else if (e.keyCode === 38) {
                // UP key (select the previous item)
                var index = selectableItems.index(self.selectedItem);
                selectableItems = selectableItems.slice(0, index);
                if (selectableItems.length === 0) {
                    // We are already at the first item
                    return
                }
                // Unselect the current item
                self.selectedItem.removeClass('selected');
                // Select the previous item
                self.selectedItem = selectableItems.last().addClass('selected');;
                // Bring into view
                self.bringIntoView(scrollDirection.UP);
                // Update
                self.update();
            }
        });

        this.search.keyup(function (e) {
            if ((e.keyCode < 47 && e.keyCode !== 8) || (e.keyCode > 112 && e.keyCode < 146)) {
                // Non character
                return;
            }
            if (e.keyCode === 8 && self.search.val().length === 0) {
                // If the search text is cleared, unselect the current item
                if (self.selectedItem != undefined) {
                    self.selectedItem.removeClass('selected');
                    self.selectedItem = undefined;
                }
                // Updat
                self.update();
                // Ensure all items are selectable
                self.items.show();
                // Bring the first item into view
                self.bringIntoView();
                return;
            }
            if (self.url !== undefined && self.search.val().length == self.options.ajaxLoadCharacterCount) {
                // Load items from the server
                self.fetchItems(self.search.val());
                return;
            }
            // Show matches
            self.showMatches(self.dropDown.children('li'));
            if (!self.isMatch(self.selectedItem)) {
                if (self.selectedItem !== undefined) {
                    self.selectedItem.removeClass('selected');
                }      
                // Get the first selectable item
                var selectableItems = self.getSelectableItems();
                if (selectableItems.length === 0) {
                    self.selectedItem = undefined;
                } else {
                    for (var i = 0; i < selectableItems.length; i++) {
                        var item = selectableItems.eq(i);
                        if (self.isMatch(item)) {
                            self.selectedItem = item.addClass('selected');
                            break;
                        }
                    }
                }
            }
            // Update
            self.update();
        });

        this.dropDown.mousedown(function (e) {
            // Prevent search input losing focus
            e.preventDefault();
        });

        this.dropDown.on('mousedown', 'div:not(' + excluded + ')', function (e) {
            // Get the item
            var item = $(this);
            if (!item.hasClass('selected')) {
                // If its not the current selection, select it
                if (self.selectedItem !== undefined) {
                    self.selectedItem.removeClass('selected');
                }
                self.selectedItem = item;
                self.selectedItem.addClass('selected');
                // Clear any search text
                self.search.val('');
                // Update
                self.update();
            }
            // Hide the items
            self.hideItems();
        });

        this.search.blur(function () {
            // Clear any search text
            self.search.val('');

            // Disable the validation input to prevent post back
            //self.validationInput.prop('disabled', true);

            // Remove em tags
            self.display.html(self.display.html().replace(/(<em>|<\/em>)/, ''));
            // Ensure all items are selectable when re-focussed
            self.items.show();
            // Hide the items
            self.hideItems();
        });

    }

    select.prototype.getSelectableItems = function () {
        var items = this.items.filter(function () {
            return $(this).css('display') !== 'none';
        });
        items = items.not(excluded);
        return items;
    }

    // Recursively add list items
    select.prototype.appendItems = function (data, parent, id) {
        if (data === undefined || data === null || $.isEmptyObject(data)) {
            return;
        }
        // In case its a single object
        if (!(data instanceof Array)) {
            data = [data];
        }
        var self = this;
        $.each(data, function (index, item) {
            // Create list item
            var listItem = $('<li></li>');
            // Create div for the display name
            var div = $('<div></div>');
            listItem.append(div);
            parent.append(listItem);
            // Add display name and attributes
            $.each(item, function (key, value) {
                if (value instanceof Array) {
                    if (value.length > 0) {
                        // Create a sub-list
                        var sublist = $('<ul></ul>');
                        listItem.append(sublist);
                        // Recursive call to add the array items to the sub-list
                        self.appendItems(value, sublist, id);
                    }
                } else {
                    // Add data
                    listItem.data(key.toLowerCase(), value);
                    // TODO: Find a better way of determining if its a
                    // grouped list when returning JSON data
                    // What if the name of a property is "group"!!
                    if (key.toLowerCase() === 'group') {
                        div.text(value).addClass('group');
                    } else if (key === self.displayProperty) {
                        div.text(value);
                    } else if (key === self.idProperty && value == id) {
                        // Its the selected iitem
                        div.addClass('selected');
                        self.selectedItem = div;
                    }
                }
            });
            // Trigger event to allow additional styling of the item
            $(self.search).trigger({
                type: itemAdded,
                element: div,
                values: listItem.data()
            });
        });
    }

    // Update items in the list
    select.prototype.updateItems = function (data, append, id) {
        if (!append) {
            this.dropDown.empty();
            this.selectedItem = undefined;
            this.update();
        }
        this.appendItems(data, this.dropDown, id);
        // Set the items
        this.items = this.dropDown.find('div');
        var visibleItems = this.getSelectableItems();
        // Display message if there are no items to display
        if (visibleItems.length === 0) {
            this.validationMessage.text('No items match the search text');
            if (this.selectedItem !== undefined) {
                this.selectedItem = undefined;
                this.update();
            }
        }

        //else if (visibleItems.length === 1 && this.isRequired) {
        //    // TODO: This does not correctly if adding one item via JSON, 
        //    // then appending more items

        //    // There is only one selectable item, so select it
        //    this.selectedItem = visibleItems.first().addClass('selected');
        //    this.update();
        //    this.validationMessage.text('');
        //}

        else {
            // Let the user select the item
            this.validationMessage.text('');
        }


        // UNDONE: The user should select the item, not the software!
        //if (this.selectedItem === undefined) {
        //    this.selectedItem = this.getSelectableItems().eq(0).addClass('selected');
        //    // Update
        //    this.update();
        //}

    }

    // Fetch items from the server.
    select.prototype.fetchItems = function (searchText, id, callback) {
        var self = this;
        // Check the cache
        if (self.cache[searchText] === undefined) {
            // Build url with query parameter
            var url = this.url + '?' + this.urlParameter + '=' + searchText;
            $.getJSON(url, function (data) {
                // Add to cache
                self.cache[searchText] = data;
                self.updateItems(self.cache[searchText], false, id);
                if (typeof(callback) == 'function') {
                    callback();
                }
            });
        }
        else {
            self.updateItems(self.cache[searchText], false, id);
            if (typeof (callback) == 'function') {
                callback();
            }
        }
    }

    // Shows items based on the search text
    // The method is called recursively if its a hierachial list
    select.prototype.showMatches = function (items, isRecursive) {
        if (!isRecursive) {
            // This is the first call so hide all items
            this.items.hide();
        }
        items = $(items);
        var searchText = this.search.val().toLowerCase();
        var searchLength = searchText.length;
        var randomCount = this.options.randomSearchCharacterCount;
        //var foundMatch = false;
        for (var i = 0; i < items.length; i++) {
            var item = items.eq(i).children('div');
            subItems = items.eq(i).children('ul')
            // Search for a match
            var index = item.text().toLowerCase().indexOf(searchText);
            // Search at beginning of the text if up to randomCount characters, otherwise anywhere
            if (searchLength < randomCount && index == 0 || searchLength >= randomCount && index >= 0) {
                // If a match, ensure the item and its parents are visible
                item.show();
                item.parents('ul').prev('div').show();
            } else if (subItems.length > 0) {
                // Search its children for a match
                this.showMatches(subItems.children('li'), true);
            }
        }
    }

    // Returns a value indicating if the item matches the search text
    select.prototype.isMatch = function(item)
    {
        if (item == undefined) {
            return false;
        }
        item = $(item);
        var searchText = this.search.val();
        var searchLength = searchText.length;
        if (item.hasClass('group') || item.hasClass('disabled')) {
            // Never match group headings or disabled items
            return false;
        } else if (searchLength === 0) {
            return true;
        } else {
            var randomCount = this.options.randomSearchCharacterCount;
            var index = item.text().toLowerCase().indexOf(searchText);
            if (searchLength < randomCount && index == 0 || searchLength >= randomCount && index >= 0) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    // Shows the select items
    select.prototype.showItems = function () {
        // Determine if the items need to be displayed above or below the search input
        var windowHeight = $(window).height();
        var searchTop = this.search.offset().top - $(window).scrollTop();
        var searchHeight = this.search.outerHeight();
        var itemsHeight = this.dropDown.outerHeight();
        var heightBelow = windowHeight - searchTop - searchHeight;
        var heightAbove = searchTop;// - $(window).scrollTop();
        if (itemsHeight > heightBelow && itemsHeight < heightAbove) {
            // Position above the search input
            this.dropDown.css('top', '').css('bottom', searchHeight + 1);
        } else {
            // Position below the search input
            this.dropDown.css('bottom', '').css('top', 1);
        }
        // Show the items
        this.dropDown.show();
        this.isVisible = true;
        // Ensure the selected item is visible in the drop down
        this.bringIntoView();
    }

    // Hides the select items.
    select.prototype.hideItems = function () {
        this.dropDown.hide();
        this.isVisible = false;
    }

    // Scroll the select items if necessary to bring the selected item into view.
    select.prototype.bringIntoView = function (direction) {
        direction = direction || scrollDirection.DOWN;
        if (!this.isVisible) {
            return;
        }
        if (this.selectedItem === undefined) {
            // Scroll to the top
            this.dropDown.scrollTop(0);
            return;
        }
        // Get the selectable items
        var visibleItems = this.items.filter(function () {
            return $(this).css('display') !== 'none';
        });
        if (visibleItems.length <= this.options.maxVisibleItems) {
            // All items are visible
            return;
        }
        // Get the index of the selected item
        var index = visibleItems.index(this.selectedItem);
        var height = 0;
        for (var i = 0; i < index; i++) {
            height += visibleItems.eq(0).outerHeight();
        }
        if (direction === scrollDirection.DOWN) {
            height += this.selectedItem.outerHeight();
            if (height - this.dropDown.scrollTop() > this.scrollHeight) {
                this.dropDown.scrollTop(height - this.scrollHeight);
            }
        } else if (height < this.dropDown.scrollTop()) {
            this.dropDown.scrollTop(height);
        }
    }

    // Updates the input values for postback
    // The method is called recursively for complex properties
    select.prototype.updateInputs = function (data, name) {
        var self = this;
        $.each(data, function (attr, value) {
            var input = self.values.filter(function () {
                return $(this).attr('name').toLowerCase() === attr.toLowerCase();
            }).val(value);
            if (input.length === 1) {
                // Its a value type so there is only one input
                return;
            }
            var propertyName = name + '.' + attr;
            if (typeof (value) === 'object') {
                // Recursive call
                self.updateInputs(value, propertyName);
            }
            else {
                // Get the matching input
                input = self.values.filter(function () {
                    return $(this).attr('name').toLowerCase() === propertyName.toLowerCase();
                }).val(value);
            }
        });
    }

    // Updates the display text and hidden inputs for postback.
    select.prototype.update = function () {
        var self = this;
        var searchText = this.search.val().toLowerCase();
        if (this.selectedItem == undefined) {
            //this.validationInput.val('');
            // Disable the inputs values to prevent postback
            this.values.val('');
            this.values.prop('disabled', true);
            if (searchText.length === 0) {
                // Nothing to show
                this.display.text('');
            } else {
                // Just show the search text
                this.display.html('<span><em>' + searchText + '</em></span>');
            }
        } else {
            // Something has been selected so remove any error messages
            //this.validationInput.val(this.selectedItem.text());
            this.validationMessage.text('');
            if (this.jQueryValidationMessage !== undefined) {
                this.jQueryValidationMessage.text('');
            }
            // Ensure the inputs values are enabled
            this.values.prop('disabled', false);
            // Get the data attributes of the selected item
            var data = this.selectedItem.closest('li').data();
            // Update the hidden input values
            this.updateInputs(data, this.propertyName);
            // Update display text
            var displayText = '';
            var selectedText = this.selectedItem.text();
            // Build parent text if hierachial
            this.selectedItem.parents('ul').prev('div').each(function () {
                displayText = '<span>' + $(this).text() + self.options.separatorCharacter + '</span>' + displayText;
            });
            var selectableItems = this.getSelectableItems();
            if (searchText.length === 0 || !this.isMatch(this.selectedItem)) {
                // Append the selected item without formatting
                displayText += '<span>' + selectedText + '</span>';
            } else {
                // Get the index of the search text
                var index = selectedText.toLowerCase().indexOf(searchText);
                var length = searchText.length;
                // Build the display text with emphasis on the search text
                displayText += '<span>'
                displayText += selectedText.substr(0, index);
                displayText += '<em>' + selectedText.substr(index, length) + '</em>';
                displayText += selectedText.substr(index + length);
                displayText += '</span>'
            }
            this.display.html(displayText);
            // Trim any parents if the text does not fit
            var maxWidth = this.display.width();
            var count = this.display.children('span').length - 1;
            for (var i = 0; i < count; i++) {
                if (this.getTextWidth() > maxWidth) {
                    // UNDONE: Use html() so special characters can be used as the seperator
                    // this.display.children('span').eq(i).text('...' + self.options.separatorCharacter);
                    this.display.children('span').eq(i).html('...' + self.options.separatorCharacter);
                } else {
                    break;
                }
            }
            // If its still to long, trim right
            if (this.getTextWidth() > maxWidth) {
                this.display.css('overflow', 'hidden').css('white-space', 'nowrap').css('text-overflow', 'ellipsis');
            }
        }
        // Trigger the itemSelected event
        $(self.search).trigger({
            type: itemSelected,
            element: self.selectedItem,
            values: data
        });
    }

    // Returns the total width of the display text
    select.prototype.getTextWidth = function () {
        var width = 0;
        var spans = this.display.children('span');
        $.each(spans, function (index, span) {
            width += $(this).width();
        });
        return width;
    }

    // Select definition
    $.fn.select = function (options) {
        return this.each(function () {
            if (!$.data(this, 'select')) {
                $.data(this, 'select', new select(this, options));
            }
        });
    }

    // Add items to the select list
    $.fn.updateItems = function (data, append) {
        return this.each(function () {
            var self = $.data(this, 'select');
            if (!self) {
                self = new select(this);
                $.data(this, 'select', self);
            }
            // TODO: Option to append or replace existing items?
            append = append || false;
            self.updateItems(data, append);
        });
    }

    // Select defaults
    $.fn.select.defaults = defaults;

}(jQuery, window));