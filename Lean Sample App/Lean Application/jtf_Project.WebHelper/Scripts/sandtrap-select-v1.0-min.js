(function (d, n, f) {
	function e(a, c) { this.element = d(a).closest(".select"); this.options = d.extend({}, m, c); this.defaults = m; this.initialise() } var k = Object.freeze({ UP: "up", DOWN: "down" }), m = { maxVisibleItems: 8, randomSearchCharacterCount: 3, ajaxLoadCharacterCount: 2, separatorCharacter: " &#8226; " }; e.prototype.initialise = function () {
		var a = this, c = d(this.element).children(".select-input"), g = d(this.element).children(".select-validation"), b = d(this.element).children(".select-list"); this.search = c.find('input[type="text"]');
		this.display = c.find("div"); this.button = c.find("button"); this.values = c.find('input[type="hidden"]'); this.validationMessage = g.find("span"); this.dropDown = b.children("ul"); this.items = b.find("div"); c = 0; 0 === this.items.length || 0 === this.items.eq(0).height() ? (b = d('<div class="select-list"><ul><li><div>Text</div></li></ul></div>'), d(this.element).append(b), c = b.find("div").outerHeight() * this.options.maxVisibleItems, b.remove()) : c = this.items.eq(0).outerHeight() * this.options.maxVisibleItems; this.scrollHeight = c;
		c += this.dropDown.outerHeight() - this.dropDown.height(); this.dropDown.css("max-height", c); this.cache = {}; this.isVisible = !1; this.displayProperty = this.idProperty = this.urlParameter = this.url = this.selectedItem = f; this.isRequired = !1; b = d(this.element).data(); this.propertyName = b.propertyname; b.idproperty && (this.idProperty = b.idproperty); b.displayproperty && (this.displayProperty = b.displayproperty); b.url && (this.url = b.url); b.urlparameter && (this.urlParameter = b.urlparameter); b.isrequired && (this.isRequired = !0, this.jQueryValidationMessage =
		d(this.element).siblings("span").filter(function () { return d(this).attr("data-valmsg-for").toLowerCase() === a.propertyName.toLowerCase() })); b = this.items.filter(".selected"); 0 < b.length && (this.selectedItem = b.first()); if (this.url != f && 0 < this.display.text().length) { var b = this.display.text().substr(0, this.options.ajaxLoadCharacterCount), h = this.propertyName + "." + this.idProperty, c = this.values.filter(function () { return d(this).attr("name").toLowerCase() === h }).val(); this.fetchItems(b, c) } this.selectedItem === f &&
		this.values.prop("disabled", !0); this.update(); this.button.mousedown(function (b) { b.preventDefault(); a.search.focus(); a.isVisible ? a.hideItems() : a.showItems() }); this.display.mousedown(function (b) { b.preventDefault(); a.search.is(":focus") || a.search.focus() }); this.search.keydown(function (b) {
			if (115 === b.keyCode) a.isVisible ? a.hideItems() : a.showItems(); else if (13 === b.keyCode) a.isVisible && b.preventDefault(), a.hideItems(); else if (27 == b.keyCode) a.hideItems(); else {
				var c = a.getSelectableItems(); 0 !== c.length && (40 ===
				b.keyCode ? a.selectedItem === f ? (a.selectedItem = c.first().addClass("selected"), a.update()) : (b = c.index(a.selectedItem) + 1, c = c.slice(b), 0 !== c.length && (a.selectedItem.removeClass("selected"), a.selectedItem = c.first().addClass("selected"), a.bringIntoView(k.DOWN), a.update())) : 38 === b.keyCode && (b = c.index(a.selectedItem), c = c.slice(0, b), 0 !== c.length && (a.selectedItem.removeClass("selected"), a.selectedItem = c.last().addClass("selected"), a.bringIntoView(k.UP), a.update())))
			}
		}); this.search.keyup(function (b) {
			if (!(47 >
			b.keyCode && 8 !== b.keyCode || 112 < b.keyCode && 146 > b.keyCode)) if (8 === b.keyCode && 0 === a.search.val().length) a.selectedItem != f && (a.selectedItem.removeClass("selected"), a.selectedItem = f), a.update(), a.items.show(), a.bringIntoView(); else if (a.url !== f && a.search.val().length == a.options.ajaxLoadCharacterCount) a.fetchItems(a.search.val()); else {
				a.showMatches(a.dropDown.children("li")); if (!a.isMatch(a.selectedItem)) if (a.selectedItem !== f && a.selectedItem.removeClass("selected"), b = a.getSelectableItems(), 0 === b.length) a.selectedItem =
				f; else for (var c = 0; c < b.length; c++) { var d = b.eq(c); if (a.isMatch(d)) { a.selectedItem = d.addClass("selected"); break } } a.update()
			}
		}); this.dropDown.mousedown(function (a) { a.preventDefault() }); this.dropDown.on("mousedown", "div:not(.group, .disabled)", function (b) { b = d(this); b.hasClass("selected") || (a.selectedItem !== f && a.selectedItem.removeClass("selected"), a.selectedItem = b, a.selectedItem.addClass("selected"), a.search.val(""), a.update()); a.hideItems() }); this.search.blur(function () {
			a.search.val(""); a.display.html(a.display.html().replace(/(<em>|<\/em>)/,
			"")); a.items.show(); a.hideItems()
		})
	}; e.prototype.getSelectableItems = function () { var a = this.items.filter(function () { return "none" !== d(this).css("display") }); return a = a.not(".group, .disabled") }; e.prototype.appendItems = function (a, c, g) {
		a instanceof Array || (a = [a]); var b = this; d.each(a, function (a, f) {
			var e = d("<li></li>"), l = d("<div></div>"); e.append(l); c.append(e); d.each(f, function (a, c) {
				if (c instanceof Array) { if (0 < c.length) { var h = d("<ul></ul>"); e.append(h); b.appendItems(c, h, g) } } else e.data(a.toLowerCase(),
				c), a === b.displayProperty ? l.text(c) : a === b.idProperty && c == g && (l.addClass("selected"), b.selectedItem = l)
			}); d(b.search).trigger({ type: "itemAdded.select", element: l, values: e.data() })
		})
	}; e.prototype.updateItems = function (a, c) {
		replaceExisting = !0; this.dropDown.empty(); this.selectedItem = f; null == a || 0 === a.length ? (this.validationMessage.text("No items match the search text"), this.selectedItem = f, this.update()) : (this.validationMessage.text(""), this.appendItems(a, this.dropDown, c), this.items = this.dropDown.find("div"),
		this.selectedItem === f && (this.selectedItem = this.getSelectableItems().eq(0).addClass("selected"), this.update()))
	}; e.prototype.fetchItems = function (a, c) { var g = this, b = this.url + "?" + this.urlParameter + "=" + a; g.cache[a] === f ? d.getJSON(b, function (b) { g.cache[a] = b; g.updateItems(g.cache[a], c) }) : g.updateItems(g.cache[a], c) }; e.prototype.showMatches = function (a, c) {
		c || this.items.hide(); a = d(a); for (var g = this.search.val().toLowerCase(), b = g.length, h = this.options.randomSearchCharacterCount, e = 0; e < a.length; e++) {
			var f = a.eq(e).children("div");
			subItems = a.eq(e).children("ul"); var l = f.text().toLowerCase().indexOf(g); b < h && 0 == l || b >= h && 0 <= l ? (f.show(), f.parents("ul").prev("div").show()) : 0 < subItems.length && this.showMatches(subItems.children("li"), !0)
		}
	}; e.prototype.isMatch = function (a) { if (a == f) return !1; a = d(a); var c = this.search.val(), g = c.length; if (a.hasClass("group") || a.hasClass("disabled")) return !1; if (0 === g) return !0; var b = this.options.randomSearchCharacterCount; a = a.text().toLowerCase().indexOf(c); return g < b && 0 == a || g >= b && 0 <= a ? !0 : !1 }; e.prototype.showItems =
	function () { var a = d(n).height(), c = this.search.offset().top - d(n).scrollTop(), g = this.search.outerHeight(), b = this.dropDown.outerHeight(); b > a - c - g && b < c ? this.dropDown.css("top", "").css("bottom", g + 1) : this.dropDown.css("bottom", "").css("top", 1); this.dropDown.show(); this.isVisible = !0; this.bringIntoView() }; e.prototype.hideItems = function () { this.dropDown.hide(); this.isVisible = !1 }; e.prototype.bringIntoView = function (a) {
		a = a || k.DOWN; if (this.isVisible) if (this.selectedItem === f) this.dropDown.scrollTop(0); else {
			var c =
			this.items.filter(function () { return "none" !== d(this).css("display") }); if (!(c.length <= this.options.maxVisibleItems)) { for (var g = c.index(this.selectedItem), b = 0, e = 0; e < g; e++) b += c.eq(0).outerHeight(); a === k.DOWN ? (b += this.selectedItem.outerHeight(), b - this.dropDown.scrollTop() > this.scrollHeight && this.dropDown.scrollTop(b - this.scrollHeight)) : b < this.dropDown.scrollTop() && this.dropDown.scrollTop(b) }
		}
	}; e.prototype.updateInputs = function (a, c) {
		var g = this; d.each(a, function (a, e) {
			var f = g.values.filter(function () {
				return d(this).attr("name").toLowerCase() ===
				a.toLowerCase()
			}).val(e); if (1 !== f.length) { var k = c + "." + a; "object" === typeof e ? g.updateInputs(e, k) : f = g.values.filter(function () { return d(this).attr("name").toLowerCase() === k.toLowerCase() }).val(e) }
		})
	}; e.prototype.update = function () {
		var a = this, c = this.search.val().toLowerCase(); if (this.selectedItem == f) this.values.val(""), this.values.prop("disabled", !0), 0 === c.length ? this.display.text("") : this.display.html("<span><em>" + c + "</em></span>"); else {
			this.validationMessage.text(""); this.jQueryValidationMessage !==
			f && this.jQueryValidationMessage.text(""); this.values.prop("disabled", !1); var e = this.selectedItem.closest("li").data(); this.updateInputs(e, this.propertyName); var b = "", h = this.selectedItem.text(); this.selectedItem.parents("ul").prev("div").each(function () { b = "<span>" + d(this).text() + a.options.separatorCharacter + "</span>" + b }); this.getSelectableItems(); if (0 !== c.length && this.isMatch(this.selectedItem)) var k = h.toLowerCase().indexOf(c), c = c.length, b = b + "<span>", b = b + h.substr(0, k), b = b + ("<em>" + h.substr(k, c) + "</em>"),
			b = b + h.substr(k + c), b = b + "</span>"; else b += "<span>" + h + "</span>"; this.display.html(b); h = this.display.width(); k = this.display.children("span").length - 1; for (c = 0; c < k; c++) if (this.getTextWidth(), this.getTextWidth() > h) this.display.children("span").eq(c).text("..." + a.options.separatorCharacter); else break; this.getTextWidth() > h && this.display.css("overflow", "hidden").css("white-space", "nowrap").css("text-overflow", "ellipsis")
		} d(a.search).trigger({ type: "itemSelected.select", element: a.selectedItem, values: e })
	}; e.prototype.getTextWidth =
	function () { var a = 0, c = this.display.children("span"); d.each(c, function (c, b) { a += d(this).width() }); return a }; d.fn.select = function (a) { return this.each(function () { d.data(this, "select") || d.data(this, "select", new e(this, a)) }) }; d.fn.updateItems = function (a) { return this.each(function () { var c = d.data(this, "select"); c || (c = new e(this), d.data(this, "select", c)); c.updateItems(a) }) }; d.fn.select.defaults = m
})(jQuery, window);