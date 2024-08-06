(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@angular/core'), require('@angular/forms'), require('@angular/common'), require('lodash')) :
    typeof define === 'function' && define.amd ? define('ngx-treeview', ['exports', '@angular/core', '@angular/forms', '@angular/common', 'lodash'], factory) :
    (global = global || self, factory(global['ngx-treeview'] = {}, global.ng.core, global.ng.forms, global.ng.common, global.lodash));
}(this, (function (exports, core, forms, common, lodash) { 'use strict';

    var DropdownDirective = /** @class */ (function () {
        function DropdownDirective() {
            this.internalOpen = false;
            this.openChange = new core.EventEmitter();
        }
        Object.defineProperty(DropdownDirective.prototype, "isOpen", {
            get: function () {
                return this.internalOpen;
            },
            enumerable: false,
            configurable: true
        });
        DropdownDirective.prototype.onKeyupEsc = function () {
            this.close();
        };
        DropdownDirective.prototype.onDocumentClick = function (event) {
            if (event.button !== 2 && !this.isEventFromToggle(event)) {
                this.close();
            }
        };
        DropdownDirective.prototype.open = function () {
            if (!this.internalOpen) {
                this.internalOpen = true;
                this.openChange.emit(true);
            }
        };
        DropdownDirective.prototype.close = function () {
            if (this.internalOpen) {
                this.internalOpen = false;
                this.openChange.emit(false);
            }
        };
        DropdownDirective.prototype.toggle = function () {
            if (this.isOpen) {
                this.close();
            }
            else {
                this.open();
            }
        };
        DropdownDirective.prototype.isEventFromToggle = function (event) {
            return !lodash.isNil(this.toggleElement) && this.toggleElement.contains(event.target);
        };
        return DropdownDirective;
    }());
    DropdownDirective.decorators = [
        { type: core.Directive, args: [{
                    selector: '[ngxDropdown]',
                    exportAs: 'ngxDropdown'
                },] }
    ];
    DropdownDirective.propDecorators = {
        internalOpen: [{ type: core.Input, args: ['open',] }],
        openChange: [{ type: core.Output }],
        isOpen: [{ type: core.HostBinding, args: ['class.show',] }],
        onKeyupEsc: [{ type: core.HostListener, args: ['keyup.esc',] }],
        onDocumentClick: [{ type: core.HostListener, args: ['document:click', ['$event'],] }]
    };

    var DropdownMenuDirective = /** @class */ (function () {
        function DropdownMenuDirective(dropdown) {
            this.dropdown = dropdown;
        }
        return DropdownMenuDirective;
    }());
    DropdownMenuDirective.decorators = [
        { type: core.Directive, args: [{
                    selector: '[ngxDropdownMenu]',
                    host: {
                        '[class.dropdown-menu]': 'true',
                        '[class.show]': 'dropdown.isOpen'
                    }
                },] }
    ];
    DropdownMenuDirective.ctorParameters = function () { return [
        { type: DropdownDirective }
    ]; };

    var DropdownToggleDirective = /** @class */ (function () {
        function DropdownToggleDirective(dropdown, elementRef) {
            this.dropdown = dropdown;
            dropdown.toggleElement = elementRef.nativeElement;
        }
        return DropdownToggleDirective;
    }());
    DropdownToggleDirective.decorators = [
        { type: core.Directive, args: [{
                    selector: '[ngxDropdownToggle]',
                    host: {
                        class: 'dropdown-toggle',
                        'aria-haspopup': 'true',
                        '[attr.aria-expanded]': 'dropdown.isOpen',
                        '(click)': 'dropdown.toggle()'
                    }
                },] }
    ];
    DropdownToggleDirective.ctorParameters = function () { return [
        { type: DropdownDirective },
        { type: core.ElementRef }
    ]; };

    /*! *****************************************************************************
    Copyright (c) Microsoft Corporation.

    Permission to use, copy, modify, and/or distribute this software for any
    purpose with or without fee is hereby granted.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
    REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
    AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
    INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
    LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
    OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
    PERFORMANCE OF THIS SOFTWARE.
    ***************************************************************************** */
    /* global Reflect, Promise */
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b)
                if (b.hasOwnProperty(p))
                    d[p] = b[p]; };
        return extendStatics(d, b);
    };
    function __extends(d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    }
    var __assign = function () {
        __assign = Object.assign || function __assign(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s)
                    if (Object.prototype.hasOwnProperty.call(s, p))
                        t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };
    function __rest(s, e) {
        var t = {};
        for (var p in s)
            if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
                t[p] = s[p];
        if (s != null && typeof Object.getOwnPropertySymbols === "function")
            for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
                if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                    t[p[i]] = s[p[i]];
            }
        return t;
    }
    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function")
            r = Reflect.decorate(decorators, target, key, desc);
        else
            for (var i = decorators.length - 1; i >= 0; i--)
                if (d = decorators[i])
                    r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }
    function __param(paramIndex, decorator) {
        return function (target, key) { decorator(target, key, paramIndex); };
    }
    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function")
            return Reflect.metadata(metadataKey, metadataValue);
    }
    function __awaiter(thisArg, _arguments, P, generator) {
        function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try {
                step(generator.next(value));
            }
            catch (e) {
                reject(e);
            } }
            function rejected(value) { try {
                step(generator["throw"](value));
            }
            catch (e) {
                reject(e);
            } }
            function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    }
    function __generator(thisArg, body) {
        var _ = { label: 0, sent: function () { if (t[0] & 1)
                throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function () { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f)
                throw new TypeError("Generator is already executing.");
            while (_)
                try {
                    if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done)
                        return t;
                    if (y = 0, t)
                        op = [op[0] & 2, t.value];
                    switch (op[0]) {
                        case 0:
                        case 1:
                            t = op;
                            break;
                        case 4:
                            _.label++;
                            return { value: op[1], done: false };
                        case 5:
                            _.label++;
                            y = op[1];
                            op = [0];
                            continue;
                        case 7:
                            op = _.ops.pop();
                            _.trys.pop();
                            continue;
                        default:
                            if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) {
                                _ = 0;
                                continue;
                            }
                            if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) {
                                _.label = op[1];
                                break;
                            }
                            if (op[0] === 6 && _.label < t[1]) {
                                _.label = t[1];
                                t = op;
                                break;
                            }
                            if (t && _.label < t[2]) {
                                _.label = t[2];
                                _.ops.push(op);
                                break;
                            }
                            if (t[2])
                                _.ops.pop();
                            _.trys.pop();
                            continue;
                    }
                    op = body.call(thisArg, _);
                }
                catch (e) {
                    op = [6, e];
                    y = 0;
                }
                finally {
                    f = t = 0;
                }
            if (op[0] & 5)
                throw op[1];
            return { value: op[0] ? op[1] : void 0, done: true };
        }
    }
    var __createBinding = Object.create ? (function (o, m, k, k2) {
        if (k2 === undefined)
            k2 = k;
        Object.defineProperty(o, k2, { enumerable: true, get: function () { return m[k]; } });
    }) : (function (o, m, k, k2) {
        if (k2 === undefined)
            k2 = k;
        o[k2] = m[k];
    });
    function __exportStar(m, exports) {
        for (var p in m)
            if (p !== "default" && !exports.hasOwnProperty(p))
                __createBinding(exports, m, p);
    }
    function __values(o) {
        var s = typeof Symbol === "function" && Symbol.iterator, m = s && o[s], i = 0;
        if (m)
            return m.call(o);
        if (o && typeof o.length === "number")
            return {
                next: function () {
                    if (o && i >= o.length)
                        o = void 0;
                    return { value: o && o[i++], done: !o };
                }
            };
        throw new TypeError(s ? "Object is not iterable." : "Symbol.iterator is not defined.");
    }
    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m)
            return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done)
                ar.push(r.value);
        }
        catch (error) {
            e = { error: error };
        }
        finally {
            try {
                if (r && !r.done && (m = i["return"]))
                    m.call(i);
            }
            finally {
                if (e)
                    throw e.error;
            }
        }
        return ar;
    }
    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }
    function __spreadArrays() {
        for (var s = 0, i = 0, il = arguments.length; i < il; i++)
            s += arguments[i].length;
        for (var r = Array(s), k = 0, i = 0; i < il; i++)
            for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
                r[k] = a[j];
        return r;
    }
    ;
    function __await(v) {
        return this instanceof __await ? (this.v = v, this) : new __await(v);
    }
    function __asyncGenerator(thisArg, _arguments, generator) {
        if (!Symbol.asyncIterator)
            throw new TypeError("Symbol.asyncIterator is not defined.");
        var g = generator.apply(thisArg, _arguments || []), i, q = [];
        return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
        function verb(n) { if (g[n])
            i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
        function resume(n, v) { try {
            step(g[n](v));
        }
        catch (e) {
            settle(q[0][3], e);
        } }
        function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r); }
        function fulfill(value) { resume("next", value); }
        function reject(value) { resume("throw", value); }
        function settle(f, v) { if (f(v), q.shift(), q.length)
            resume(q[0][0], q[0][1]); }
    }
    function __asyncDelegator(o) {
        var i, p;
        return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
        function verb(n, f) { i[n] = o[n] ? function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; } : f; }
    }
    function __asyncValues(o) {
        if (!Symbol.asyncIterator)
            throw new TypeError("Symbol.asyncIterator is not defined.");
        var m = o[Symbol.asyncIterator], i;
        return m ? m.call(o) : (o = typeof __values === "function" ? __values(o) : o[Symbol.iterator](), i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i);
        function verb(n) { i[n] = o[n] && function (v) { return new Promise(function (resolve, reject) { v = o[n](v), settle(resolve, reject, v.done, v.value); }); }; }
        function settle(resolve, reject, d, v) { Promise.resolve(v).then(function (v) { resolve({ value: v, done: d }); }, reject); }
    }
    function __makeTemplateObject(cooked, raw) {
        if (Object.defineProperty) {
            Object.defineProperty(cooked, "raw", { value: raw });
        }
        else {
            cooked.raw = raw;
        }
        return cooked;
    }
    ;
    var __setModuleDefault = Object.create ? (function (o, v) {
        Object.defineProperty(o, "default", { enumerable: true, value: v });
    }) : function (o, v) {
        o["default"] = v;
    };
    function __importStar(mod) {
        if (mod && mod.__esModule)
            return mod;
        var result = {};
        if (mod != null)
            for (var k in mod)
                if (Object.hasOwnProperty.call(mod, k))
                    __createBinding(result, mod, k);
        __setModuleDefault(result, mod);
        return result;
    }
    function __importDefault(mod) {
        return (mod && mod.__esModule) ? mod : { default: mod };
    }
    function __classPrivateFieldGet(receiver, privateMap) {
        if (!privateMap.has(receiver)) {
            throw new TypeError("attempted to get private field on non-instance");
        }
        return privateMap.get(receiver);
    }
    function __classPrivateFieldSet(receiver, privateMap, value) {
        if (!privateMap.has(receiver)) {
            throw new TypeError("attempted to set private field on non-instance");
        }
        privateMap.set(receiver, value);
        return value;
    }

    var TreeviewI18n = /** @class */ (function () {
        function TreeviewI18n() {
        }
        return TreeviewI18n;
    }());
    TreeviewI18n.decorators = [
        { type: core.Injectable }
    ];
    var DefaultTreeviewI18n = /** @class */ (function (_super) {
        __extends(DefaultTreeviewI18n, _super);
        function DefaultTreeviewI18n() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        DefaultTreeviewI18n.prototype.getText = function (selection) {
            if (selection.uncheckedItems.length === 0) {
                if (selection.checkedItems.length > 0) {
                    return this.getAllCheckboxText();
                }
                else {
                    return '';
                }
            }
            switch (selection.checkedItems.length) {
                case 0:
                    return 'Select options';
                case 1:
                    return selection.checkedItems[0].text;
                default:
                    return selection.checkedItems.length + " options selected";
            }
        };
        DefaultTreeviewI18n.prototype.getAllCheckboxText = function () {
            return 'All';
        };
        DefaultTreeviewI18n.prototype.getFilterPlaceholder = function () {
            return 'Filter';
        };
        DefaultTreeviewI18n.prototype.getFilterNoItemsFoundText = function () {
            return 'No items found';
        };
        DefaultTreeviewI18n.prototype.getTooltipCollapseExpandText = function (isCollapse) {
            return isCollapse ? 'Expand' : 'Collapse';
        };
        return DefaultTreeviewI18n;
    }(TreeviewI18n));
    DefaultTreeviewI18n.decorators = [
        { type: core.Injectable }
    ];

    var TreeviewConfig = /** @class */ (function () {
        function TreeviewConfig() {
            this.hasAllCheckBox = true;
            this.hasFilter = false;
            this.hasCollapseExpand = false;
            this.decoupleChildFromParent = false;
            this.maxHeight = 500;
        }
        Object.defineProperty(TreeviewConfig.prototype, "hasDivider", {
            get: function () {
                return this.hasFilter || this.hasAllCheckBox || this.hasCollapseExpand;
            },
            enumerable: false,
            configurable: true
        });
        TreeviewConfig.create = function (fields) {
            var config = new TreeviewConfig();
            Object.assign(config, fields);
            return config;
        };
        return TreeviewConfig;
    }());
    TreeviewConfig.decorators = [
        { type: core.Injectable }
    ];

    var TreeviewHelper = {
        findItem: findItem,
        findItemInList: findItemInList,
        findParent: findParent,
        removeItem: removeItem,
        concatSelection: concatSelection
    };
    function findItem(root, value) {
        var e_1, _a;
        if (lodash.isNil(root)) {
            return undefined;
        }
        if (root.value === value) {
            return root;
        }
        if (root.children) {
            try {
                for (var _b = __values(root.children), _c = _b.next(); !_c.done; _c = _b.next()) {
                    var child = _c.value;
                    var foundItem = findItem(child, value);
                    if (foundItem) {
                        return foundItem;
                    }
                }
            }
            catch (e_1_1) { e_1 = { error: e_1_1 }; }
            finally {
                try {
                    if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                }
                finally { if (e_1) throw e_1.error; }
            }
        }
        return undefined;
    }
    function findItemInList(list, value) {
        var e_2, _a;
        if (lodash.isNil(list)) {
            return undefined;
        }
        try {
            for (var list_1 = __values(list), list_1_1 = list_1.next(); !list_1_1.done; list_1_1 = list_1.next()) {
                var item = list_1_1.value;
                var foundItem = findItem(item, value);
                if (foundItem) {
                    return foundItem;
                }
            }
        }
        catch (e_2_1) { e_2 = { error: e_2_1 }; }
        finally {
            try {
                if (list_1_1 && !list_1_1.done && (_a = list_1.return)) _a.call(list_1);
            }
            finally { if (e_2) throw e_2.error; }
        }
        return undefined;
    }
    function findParent(root, item) {
        var e_3, _a;
        if (lodash.isNil(root) || lodash.isNil(root.children)) {
            return undefined;
        }
        try {
            for (var _b = __values(root.children), _c = _b.next(); !_c.done; _c = _b.next()) {
                var child = _c.value;
                if (child === item) {
                    return root;
                }
                else {
                    var parent = findParent(child, item);
                    if (parent) {
                        return parent;
                    }
                }
            }
        }
        catch (e_3_1) { e_3 = { error: e_3_1 }; }
        finally {
            try {
                if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
            }
            finally { if (e_3) throw e_3.error; }
        }
        return undefined;
    }
    function removeItem(root, item) {
        var parent = findParent(root, item);
        if (parent) {
            lodash.pull(parent.children, item);
            if (parent.children.length === 0) {
                parent.children = undefined;
            }
            else {
                parent.correctChecked();
            }
            return true;
        }
        return false;
    }
    function concatSelection(items, checked, unchecked) {
        var e_4, _a;
        var checkedItems = __spread(checked);
        var uncheckedItems = __spread(unchecked);
        try {
            for (var items_1 = __values(items), items_1_1 = items_1.next(); !items_1_1.done; items_1_1 = items_1.next()) {
                var item = items_1_1.value;
                var selection = item.getSelection();
                checkedItems = lodash.concat(checkedItems, selection.checkedItems);
                uncheckedItems = lodash.concat(uncheckedItems, selection.uncheckedItems);
            }
        }
        catch (e_4_1) { e_4 = { error: e_4_1 }; }
        finally {
            try {
                if (items_1_1 && !items_1_1.done && (_a = items_1.return)) _a.call(items_1);
            }
            finally { if (e_4) throw e_4.error; }
        }
        return {
            checked: checkedItems,
            unchecked: uncheckedItems
        };
    }

    var TreeviewItem = /** @class */ (function () {
        function TreeviewItem(item, autoCorrectChecked) {
            var _this = this;
            if (autoCorrectChecked === void 0) { autoCorrectChecked = false; }
            this.internalDisabled = false;
            this.internalChecked = true;
            this.internalCollapsed = false;
            if (lodash.isNil(item)) {
                throw new Error('Item must be defined');
            }
            if (lodash.isString(item.text)) {
                this.text = item.text;
            }
            else {
                throw new Error('A text of item must be string object');
            }
            this.value = item.value;
            if (lodash.isBoolean(item.checked)) {
                this.checked = item.checked;
            }
            if (lodash.isBoolean(item.collapsed)) {
                this.collapsed = item.collapsed;
            }
            if (lodash.isBoolean(item.disabled)) {
                this.disabled = item.disabled;
            }
            if (!lodash.isNil(item.children) && item.children.length > 0) {
                this.children = item.children.map(function (child) {
                    if (_this.disabled === true) {
                        child.disabled = true;
                    }
                    return new TreeviewItem(child);
                });
            }
            if (autoCorrectChecked) {
                this.correctChecked();
            }
        }
        Object.defineProperty(TreeviewItem.prototype, "checked", {
            get: function () {
                return this.internalChecked;
            },
            set: function (value) {
                if (!this.internalDisabled) {
                    if (this.internalChecked !== value) {
                        this.internalChecked = value;
                    }
                }
            },
            enumerable: false,
            configurable: true
        });
        Object.defineProperty(TreeviewItem.prototype, "indeterminate", {
            get: function () {
                return this.checked === undefined;
            },
            enumerable: false,
            configurable: true
        });
        TreeviewItem.prototype.setCheckedRecursive = function (value) {
            if (!this.internalDisabled) {
                this.internalChecked = value;
                if (!lodash.isNil(this.internalChildren)) {
                    this.internalChildren.forEach(function (child) { return child.setCheckedRecursive(value); });
                }
            }
        };
        Object.defineProperty(TreeviewItem.prototype, "disabled", {
            get: function () {
                return this.internalDisabled;
            },
            set: function (value) {
                if (this.internalDisabled !== value) {
                    this.internalDisabled = value;
                    if (!lodash.isNil(this.internalChildren)) {
                        this.internalChildren.forEach(function (child) { return child.disabled = value; });
                    }
                }
            },
            enumerable: false,
            configurable: true
        });
        Object.defineProperty(TreeviewItem.prototype, "collapsed", {
            get: function () {
                return this.internalCollapsed;
            },
            set: function (value) {
                if (this.internalCollapsed !== value) {
                    this.internalCollapsed = value;
                }
            },
            enumerable: false,
            configurable: true
        });
        TreeviewItem.prototype.setCollapsedRecursive = function (value) {
            this.internalCollapsed = value;
            if (!lodash.isNil(this.internalChildren)) {
                this.internalChildren.forEach(function (child) { return child.setCollapsedRecursive(value); });
            }
        };
        Object.defineProperty(TreeviewItem.prototype, "children", {
            get: function () {
                return this.internalChildren;
            },
            set: function (value) {
                if (this.internalChildren !== value) {
                    if (!lodash.isNil(value) && value.length === 0) {
                        throw new Error('Children must be not an empty array');
                    }
                    this.internalChildren = value;
                    if (!lodash.isNil(this.internalChildren)) {
                        var checked_1 = null;
                        this.internalChildren.forEach(function (child) {
                            if (checked_1 === null) {
                                checked_1 = child.checked;
                            }
                            else {
                                if (child.checked !== checked_1) {
                                    checked_1 = undefined;
                                    return;
                                }
                            }
                        });
                        this.internalChecked = checked_1;
                    }
                }
            },
            enumerable: false,
            configurable: true
        });
        TreeviewItem.prototype.getSelection = function () {
            var checkedItems = [];
            var uncheckedItems = [];
            if (lodash.isNil(this.internalChildren)) {
                if (this.internalChecked) {
                    checkedItems.push(this);
                }
                else {
                    uncheckedItems.push(this);
                }
            }
            else {
                var selection = TreeviewHelper.concatSelection(this.internalChildren, checkedItems, uncheckedItems);
                checkedItems = selection.checked;
                uncheckedItems = selection.unchecked;
            }
            return {
                checkedItems: checkedItems,
                uncheckedItems: uncheckedItems
            };
        };
        TreeviewItem.prototype.correctChecked = function () {
            this.internalChecked = this.getCorrectChecked();
        };
        TreeviewItem.prototype.getCorrectChecked = function () {
            var e_1, _a;
            var checked = null;
            if (!lodash.isNil(this.internalChildren)) {
                try {
                    for (var _b = __values(this.internalChildren), _c = _b.next(); !_c.done; _c = _b.next()) {
                        var child = _c.value;
                        child.internalChecked = child.getCorrectChecked();
                        if (checked === null) {
                            checked = child.internalChecked;
                        }
                        else if (checked !== child.internalChecked) {
                            checked = undefined;
                            break;
                        }
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
            }
            else {
                checked = this.checked;
            }
            return checked;
        };
        return TreeviewItem;
    }());

    var TreeviewEventParser = /** @class */ (function () {
        function TreeviewEventParser() {
        }
        return TreeviewEventParser;
    }());
    TreeviewEventParser.decorators = [
        { type: core.Injectable }
    ];
    var DefaultTreeviewEventParser = /** @class */ (function (_super) {
        __extends(DefaultTreeviewEventParser, _super);
        function DefaultTreeviewEventParser() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        DefaultTreeviewEventParser.prototype.getSelectedChange = function (component) {
            var checkedItems = component.selection.checkedItems;
            if (!lodash.isNil(checkedItems)) {
                return checkedItems.map(function (item) { return item.value; });
            }
            return [];
        };
        return DefaultTreeviewEventParser;
    }(TreeviewEventParser));
    DefaultTreeviewEventParser.decorators = [
        { type: core.Injectable }
    ];
    var DownlineTreeviewEventParser = /** @class */ (function (_super) {
        __extends(DownlineTreeviewEventParser, _super);
        function DownlineTreeviewEventParser() {
            return _super !== null && _super.apply(this, arguments) || this;
        }
        DownlineTreeviewEventParser.prototype.getSelectedChange = function (component) {
            var _this = this;
            var items = component.items;
            if (!lodash.isNil(items)) {
                var result_1 = [];
                items.forEach(function (item) {
                    var links = _this.getLinks(item, null);
                    if (!lodash.isNil(links)) {
                        result_1 = result_1.concat(links);
                    }
                });
                return result_1;
            }
            return [];
        };
        DownlineTreeviewEventParser.prototype.getLinks = function (item, parent) {
            var _this = this;
            if (!lodash.isNil(item.children)) {
                var link_1 = {
                    item: item,
                    parent: parent
                };
                var result_2 = [];
                item.children.forEach(function (child) {
                    var links = _this.getLinks(child, link_1);
                    if (!lodash.isNil(links)) {
                        result_2 = result_2.concat(links);
                    }
                });
                return result_2;
            }
            if (item.checked) {
                return [{
                        item: item,
                        parent: parent
                    }];
            }
            return null;
        };
        return DownlineTreeviewEventParser;
    }(TreeviewEventParser));
    DownlineTreeviewEventParser.decorators = [
        { type: core.Injectable }
    ];
    var OrderDownlineTreeviewEventParser = /** @class */ (function (_super) {
        __extends(OrderDownlineTreeviewEventParser, _super);
        function OrderDownlineTreeviewEventParser() {
            var _this = _super.apply(this, __spread(arguments)) || this;
            _this.currentDownlines = [];
            _this.parser = new DownlineTreeviewEventParser();
            return _this;
        }
        OrderDownlineTreeviewEventParser.prototype.getSelectedChange = function (component) {
            var newDownlines = this.parser.getSelectedChange(component);
            if (this.currentDownlines.length === 0) {
                this.currentDownlines = newDownlines;
            }
            else {
                var intersectDownlines_1 = [];
                this.currentDownlines.forEach(function (downline) {
                    var foundIndex = -1;
                    var length = newDownlines.length;
                    for (var i = 0; i < length; i++) {
                        if (downline.item.value === newDownlines[i].item.value) {
                            foundIndex = i;
                            break;
                        }
                    }
                    if (foundIndex !== -1) {
                        intersectDownlines_1.push(newDownlines[foundIndex]);
                        newDownlines.splice(foundIndex, 1);
                    }
                });
                this.currentDownlines = intersectDownlines_1.concat(newDownlines);
            }
            return this.currentDownlines;
        };
        return OrderDownlineTreeviewEventParser;
    }(TreeviewEventParser));
    OrderDownlineTreeviewEventParser.decorators = [
        { type: core.Injectable }
    ];

    var FilterTreeviewItem = /** @class */ (function (_super) {
        __extends(FilterTreeviewItem, _super);
        function FilterTreeviewItem(item) {
            var _this = _super.call(this, {
                text: item.text,
                value: item.value,
                disabled: item.disabled,
                checked: item.checked,
                collapsed: item.collapsed,
                children: item.children
            }) || this;
            _this.refItem = item;
            return _this;
        }
        FilterTreeviewItem.prototype.updateRefChecked = function () {
            var e_1, _a;
            this.children.forEach(function (child) {
                if (child instanceof FilterTreeviewItem) {
                    child.updateRefChecked();
                }
            });
            var refChecked = this.checked;
            if (refChecked) {
                try {
                    for (var _b = __values(this.refItem.children), _c = _b.next(); !_c.done; _c = _b.next()) {
                        var refChild = _c.value;
                        if (!refChild.checked) {
                            refChecked = false;
                            break;
                        }
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
            }
            this.refItem.checked = refChecked;
        };
        return FilterTreeviewItem;
    }(TreeviewItem));
    var TreeviewComponent = /** @class */ (function () {
        function TreeviewComponent(i18n, defaultConfig, eventParser) {
            this.i18n = i18n;
            this.defaultConfig = defaultConfig;
            this.eventParser = eventParser;
            this.selectedChange = new core.EventEmitter();
            this.filterChange = new core.EventEmitter();
            this.filterText = '';
            this.config = this.defaultConfig;
            this.allItem = new TreeviewItem({ text: 'All', value: undefined });
        }
        Object.defineProperty(TreeviewComponent.prototype, "hasFilterItems", {
            get: function () {
                return !lodash.isNil(this.filterItems) && this.filterItems.length > 0;
            },
            enumerable: false,
            configurable: true
        });
        Object.defineProperty(TreeviewComponent.prototype, "maxHeight", {
            get: function () {
                return "" + this.config.maxHeight;
            },
            enumerable: false,
            configurable: true
        });
        TreeviewComponent.prototype.ngOnInit = function () {
            this.createHeaderTemplateContext();
            this.generateSelection();
        };
        TreeviewComponent.prototype.ngOnChanges = function (changes) {
            var itemsSimpleChange = changes.items;
            if (!lodash.isNil(itemsSimpleChange) && !lodash.isNil(this.items)) {
                this.updateFilterItems();
                this.updateCollapsedOfAll();
                this.raiseSelectedChange();
            }
        };
        TreeviewComponent.prototype.onAllCollapseExpand = function () {
            var _this = this;
            this.allItem.collapsed = !this.allItem.collapsed;
            this.filterItems.forEach(function (item) { return item.setCollapsedRecursive(_this.allItem.collapsed); });
        };
        TreeviewComponent.prototype.onFilterTextChange = function (text) {
            this.filterText = text;
            this.filterChange.emit(text);
            this.updateFilterItems();
        };
        TreeviewComponent.prototype.onAllCheckedChange = function () {
            var checked = this.allItem.checked;
            this.filterItems.forEach(function (item) {
                item.setCheckedRecursive(checked);
                if (item instanceof FilterTreeviewItem) {
                    item.updateRefChecked();
                }
            });
            this.raiseSelectedChange();
        };
        TreeviewComponent.prototype.onItemCheckedChange = function (item, checked) {
            if (item instanceof FilterTreeviewItem) {
                item.updateRefChecked();
            }
            this.updateCheckedOfAll();
            this.raiseSelectedChange();
        };
        TreeviewComponent.prototype.raiseSelectedChange = function () {
            var _this = this;
            this.generateSelection();
            var values = this.eventParser.getSelectedChange(this);
            setTimeout(function () {
                _this.selectedChange.emit(values);
            });
        };
        TreeviewComponent.prototype.createHeaderTemplateContext = function () {
            var _this = this;
            this.headerTemplateContext = {
                config: this.config,
                item: this.allItem,
                onCheckedChange: function () { return _this.onAllCheckedChange(); },
                onCollapseExpand: function () { return _this.onAllCollapseExpand(); },
                onFilterTextChange: function (text) { return _this.onFilterTextChange(text); }
            };
        };
        TreeviewComponent.prototype.generateSelection = function () {
            var checkedItems = [];
            var uncheckedItems = [];
            if (!lodash.isNil(this.items)) {
                var selection = TreeviewHelper.concatSelection(this.items, checkedItems, uncheckedItems);
                checkedItems = selection.checked;
                uncheckedItems = selection.unchecked;
            }
            this.selection = {
                checkedItems: checkedItems,
                uncheckedItems: uncheckedItems
            };
        };
        TreeviewComponent.prototype.updateFilterItems = function () {
            var _this = this;
            if (this.filterText !== '') {
                var filterItems_1 = [];
                var filterText_1 = this.filterText.toLowerCase();
                this.items.forEach(function (item) {
                    var newItem = _this.filterItem(item, filterText_1);
                    if (!lodash.isNil(newItem)) {
                        filterItems_1.push(newItem);
                    }
                });
                this.filterItems = filterItems_1;
            }
            else {
                this.filterItems = this.items;
            }
            this.updateCheckedOfAll();
        };
        TreeviewComponent.prototype.filterItem = function (item, filterText) {
            var _this = this;
            var isMatch = lodash.includes(item.text.toLowerCase(), filterText);
            if (isMatch) {
                return item;
            }
            else {
                if (!lodash.isNil(item.children)) {
                    var children_1 = [];
                    item.children.forEach(function (child) {
                        var newChild = _this.filterItem(child, filterText);
                        if (!lodash.isNil(newChild)) {
                            children_1.push(newChild);
                        }
                    });
                    if (children_1.length > 0) {
                        var newItem = new FilterTreeviewItem(item);
                        newItem.collapsed = false;
                        newItem.children = children_1;
                        return newItem;
                    }
                }
            }
            return undefined;
        };
        TreeviewComponent.prototype.updateCheckedOfAll = function () {
            var e_2, _a;
            var itemChecked = null;
            try {
                for (var _b = __values(this.filterItems), _c = _b.next(); !_c.done; _c = _b.next()) {
                    var filterItem = _c.value;
                    if (itemChecked === null) {
                        itemChecked = filterItem.checked;
                    }
                    else if (itemChecked !== filterItem.checked) {
                        itemChecked = undefined;
                        break;
                    }
                }
            }
            catch (e_2_1) { e_2 = { error: e_2_1 }; }
            finally {
                try {
                    if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                }
                finally { if (e_2) throw e_2.error; }
            }
            if (itemChecked === null) {
                itemChecked = false;
            }
            this.allItem.checked = itemChecked;
        };
        TreeviewComponent.prototype.updateCollapsedOfAll = function () {
            var e_3, _a;
            var hasItemExpanded = false;
            try {
                for (var _b = __values(this.filterItems), _c = _b.next(); !_c.done; _c = _b.next()) {
                    var filterItem = _c.value;
                    if (!filterItem.collapsed) {
                        hasItemExpanded = true;
                        break;
                    }
                }
            }
            catch (e_3_1) { e_3 = { error: e_3_1 }; }
            finally {
                try {
                    if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                }
                finally { if (e_3) throw e_3.error; }
            }
            this.allItem.collapsed = !hasItemExpanded;
        };
        return TreeviewComponent;
    }());
    TreeviewComponent.decorators = [
        { type: core.Component, args: [{
                    selector: 'ngx-treeview',
                    template: "<ng-template #defaultItemTemplate let-item=\"item\" let-onCollapseExpand=\"onCollapseExpand\"\r\n  let-onCheckedChange=\"onCheckedChange\">\r\n  <div class=\"form-inline row-item\">\r\n    <i *ngIf=\"item.children\" (click)=\"onCollapseExpand()\" aria-hidden=\"true\" [ngSwitch]=\"item.collapsed\">\r\n      <svg *ngSwitchCase=\"true\" width=\"0.8rem\" height=\"0.8rem\" viewBox=\"0 0 16 16\" class=\"bi bi-caret-right-fill\"\r\n        fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\">\r\n        <path\r\n          d=\"M12.14 8.753l-5.482 4.796c-.646.566-1.658.106-1.658-.753V3.204a1 1 0 0 1 1.659-.753l5.48 4.796a1 1 0 0 1 0 1.506z\" />\r\n      </svg>\r\n      <svg *ngSwitchCase=\"false\" width=\"0.8rem\" height=\"0.8rem\" viewBox=\"0 0 16 16\" class=\"bi bi-caret-down-fill\"\r\n        fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\">\r\n        <path\r\n          d=\"M7.247 11.14L2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z\" />\r\n      </svg>\r\n    </i>\r\n    <div class=\"form-check\">\r\n      <input type=\"checkbox\" class=\"form-check-input\" [(ngModel)]=\"item.checked\" (ngModelChange)=\"onCheckedChange()\"\r\n        [disabled]=\"item.disabled\" [indeterminate]=\"item.indeterminate\" />\r\n      <label class=\"form-check-label\" (click)=\"item.checked = !item.checked; onCheckedChange()\">\r\n        {{item.text}}\r\n      </label>\r\n    </div>\r\n  </div>\r\n</ng-template>\r\n<ng-template #defaultHeaderTemplate let-config=\"config\" let-item=\"item\" let-onCollapseExpand=\"onCollapseExpand\"\r\n  let-onCheckedChange=\"onCheckedChange\" let-onFilterTextChange=\"onFilterTextChange\">\r\n  <div *ngIf=\"config.hasFilter\" class=\"row row-filter\">\r\n    <div class=\"col-12\">\r\n      <input class=\"form-control\" type=\"text\" [placeholder]=\"i18n.getFilterPlaceholder()\" [(ngModel)]=\"filterText\"\r\n        (ngModelChange)=\"onFilterTextChange($event)\" />\r\n    </div>\r\n  </div>\r\n  <div *ngIf=\"hasFilterItems\">\r\n    <div *ngIf=\"config.hasAllCheckBox || config.hasCollapseExpand\" class=\"row row-all\">\r\n      <div class=\"col-12\">\r\n        <div class=\"form-check form-check-inline\" *ngIf=\"config.hasAllCheckBox\">\r\n          <input type=\"checkbox\" class=\"form-check-input\" [(ngModel)]=\"item.checked\" (ngModelChange)=\"onCheckedChange()\"\r\n            [indeterminate]=\"item.indeterminate\" />\r\n          <label class=\"form-check-label\" (click)=\"item.checked = !item.checked; onCheckedChange()\">\r\n            {{i18n.getAllCheckboxText()}}\r\n          </label>\r\n        </div>\r\n        <label *ngIf=\"config.hasCollapseExpand\" class=\"float-right form-check-label\" (click)=\"onCollapseExpand()\">\r\n          <i [title]=\"i18n.getTooltipCollapseExpandText(item.collapsed)\" aria-hidden=\"true\" [ngSwitch]=\"item.collapsed\">\r\n            <svg *ngSwitchCase=\"true\" width=\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-arrows-angle-expand\"\r\n              fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\">\r\n              <path fill-rule=\"evenodd\"\r\n                d=\"M1.5 10.036a.5.5 0 0 1 .5.5v3.5h3.5a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5z\" />\r\n              <path fill-rule=\"evenodd\"\r\n                d=\"M6.354 9.646a.5.5 0 0 1 0 .708l-4.5 4.5a.5.5 0 0 1-.708-.708l4.5-4.5a.5.5 0 0 1 .708 0zm8.5-8.5a.5.5 0 0 1 0 .708l-4.5 4.5a.5.5 0 0 1-.708-.708l4.5-4.5a.5.5 0 0 1 .708 0z\" />\r\n              <path fill-rule=\"evenodd\"\r\n                d=\"M10.036 1.5a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 .5.5v4a.5.5 0 1 1-1 0V2h-3.5a.5.5 0 0 1-.5-.5z\" />\r\n            </svg>\r\n            <svg *ngSwitchCase=\"false\" width=\"1em\" height=\"1em\" viewBox=\"0 0 16 16\" class=\"bi bi-arrows-angle-contract\"\r\n              fill=\"currentColor\" xmlns=\"http://www.w3.org/2000/svg\">\r\n              <path fill-rule=\"evenodd\"\r\n                d=\"M9.5 2.036a.5.5 0 0 1 .5.5v3.5h3.5a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5v-4a.5.5 0 0 1 .5-.5z\" />\r\n              <path fill-rule=\"evenodd\"\r\n                d=\"M14.354 1.646a.5.5 0 0 1 0 .708l-4.5 4.5a.5.5 0 1 1-.708-.708l4.5-4.5a.5.5 0 0 1 .708 0zm-7.5 7.5a.5.5 0 0 1 0 .708l-4.5 4.5a.5.5 0 0 1-.708-.708l4.5-4.5a.5.5 0 0 1 .708 0z\" />\r\n              <path fill-rule=\"evenodd\"\r\n                d=\"M2.036 9.5a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 .5.5v4a.5.5 0 0 1-1 0V10h-3.5a.5.5 0 0 1-.5-.5z\" />\r\n            </svg>\r\n          </i>\r\n        </label>\r\n      </div>\r\n    </div>\r\n    <div *ngIf=\"config.hasDivider\" class=\"dropdown-divider\"></div>\r\n  </div>\r\n</ng-template>\r\n<div class=\"treeview-header\">\r\n  <ng-template [ngTemplateOutlet]=\"headerTemplate || defaultHeaderTemplate\"\r\n    [ngTemplateOutletContext]=\"headerTemplateContext\">\r\n  </ng-template>\r\n</div>\r\n<div [ngSwitch]=\"hasFilterItems\">\r\n  <div *ngSwitchCase=\"true\" class=\"treeview-container\" [style.max-height.px]=\"maxHeight\">\r\n    <ngx-treeview-item *ngFor=\"let item of filterItems\" [config]=\"config\" [item]=\"item\"\r\n      [template]=\"itemTemplate || defaultItemTemplate\" (checkedChange)=\"onItemCheckedChange(item, $event)\">\r\n    </ngx-treeview-item>\r\n  </div>\r\n  <div *ngSwitchCase=\"false\" class=\"treeview-text\">\r\n    {{i18n.getFilterNoItemsFoundText()}}\r\n  </div>\r\n</div>\r\n",
                    styles: [":host .treeview-header .row-filter{margin-bottom:.5rem}:host .treeview-header .row-all .bi{cursor:pointer}:host .treeview-container .row-item{flex-wrap:nowrap;margin-bottom:.3rem}:host .treeview-container .row-item .bi{cursor:pointer;margin-right:.3rem}.treeview-container{overflow-y:auto;padding-right:.3rem}.treeview-text{padding:.3rem 0;white-space:nowrap}"]
                },] }
    ];
    TreeviewComponent.ctorParameters = function () { return [
        { type: TreeviewI18n },
        { type: TreeviewConfig },
        { type: TreeviewEventParser }
    ]; };
    TreeviewComponent.propDecorators = {
        headerTemplate: [{ type: core.Input }],
        itemTemplate: [{ type: core.Input }],
        items: [{ type: core.Input }],
        config: [{ type: core.Input }],
        selectedChange: [{ type: core.Output }],
        filterChange: [{ type: core.Output }]
    };

    var DropdownTreeviewComponent = /** @class */ (function () {
        function DropdownTreeviewComponent(i18n, defaultConfig) {
            this.i18n = i18n;
            this.defaultConfig = defaultConfig;
            this.buttonClass = 'btn-outline-secondary';
            this.selectedChange = new core.EventEmitter(true);
            this.filterChange = new core.EventEmitter();
            this.config = this.defaultConfig;
        }
        DropdownTreeviewComponent.prototype.onSelectedChange = function (values) {
            this.buttonLabel = this.i18n.getText(this.treeviewComponent.selection);
            this.selectedChange.emit(values);
        };
        DropdownTreeviewComponent.prototype.onFilterChange = function (text) {
            this.filterChange.emit(text);
        };
        return DropdownTreeviewComponent;
    }());
    DropdownTreeviewComponent.decorators = [
        { type: core.Component, args: [{
                    selector: 'ngx-dropdown-treeview',
                    template: "<div class=\"dropdown\" ngxDropdown>\r\n  <button class=\"btn\" [ngClass]=\"buttonClass\" type=\"button\" role=\"button\" ngxDropdownToggle>\r\n    {{buttonLabel}}\r\n  </button>\r\n  <div ngxDropdownMenu aria-labelledby=\"dropdownMenu\" (click)=\"$event.stopPropagation()\">\r\n    <div class=\"dropdown-container\">\r\n      <ngx-treeview [config]=\"config\" [headerTemplate]=\"headerTemplate\" [items]=\"items\" [itemTemplate]=\"itemTemplate\"\r\n        (selectedChange)=\"onSelectedChange($event)\" (filterChange)=\"onFilterChange($event)\">\r\n      </ngx-treeview>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                    styles: [".dropdown{display:inline-block;width:100%}.dropdown button{margin-right:.9rem;overflow:hidden;padding-right:30px;text-align:left;text-overflow:ellipsis;width:100%}.dropdown button:after{margin-top:.6rem;position:absolute;right:.6rem}.dropdown .dropdown-menu .dropdown-container{padding:0 .6rem}"]
                },] }
    ];
    DropdownTreeviewComponent.ctorParameters = function () { return [
        { type: TreeviewI18n },
        { type: TreeviewConfig }
    ]; };
    DropdownTreeviewComponent.propDecorators = {
        buttonClass: [{ type: core.Input }],
        headerTemplate: [{ type: core.Input }],
        itemTemplate: [{ type: core.Input }],
        items: [{ type: core.Input }],
        config: [{ type: core.Input }],
        selectedChange: [{ type: core.Output }],
        filterChange: [{ type: core.Output }],
        treeviewComponent: [{ type: core.ViewChild, args: [TreeviewComponent, { static: false },] }]
    };

    var TreeviewItemComponent = /** @class */ (function () {
        function TreeviewItemComponent(defaultConfig) {
            var _this = this;
            this.defaultConfig = defaultConfig;
            this.checkedChange = new core.EventEmitter();
            this.onCollapseExpand = function () {
                _this.item.collapsed = !_this.item.collapsed;
            };
            this.onCheckedChange = function () {
                var checked = _this.item.checked;
                if (!lodash.isNil(_this.item.children) && !_this.config.decoupleChildFromParent) {
                    _this.item.children.forEach(function (child) { return child.setCheckedRecursive(checked); });
                }
                _this.checkedChange.emit(checked);
            };
            this.config = this.defaultConfig;
        }
        TreeviewItemComponent.prototype.onChildCheckedChange = function (child, checked) {
            var e_1, _a;
            if (!this.config.decoupleChildFromParent) {
                var itemChecked = null;
                try {
                    for (var _b = __values(this.item.children), _c = _b.next(); !_c.done; _c = _b.next()) {
                        var childItem = _c.value;
                        if (itemChecked === null) {
                            itemChecked = childItem.checked;
                        }
                        else if (itemChecked !== childItem.checked) {
                            itemChecked = undefined;
                            break;
                        }
                    }
                }
                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                    }
                    finally { if (e_1) throw e_1.error; }
                }
                if (itemChecked === null) {
                    itemChecked = false;
                }
                if (this.item.checked !== itemChecked) {
                    this.item.checked = itemChecked;
                }
            }
            this.checkedChange.emit(checked);
        };
        return TreeviewItemComponent;
    }());
    TreeviewItemComponent.decorators = [
        { type: core.Component, args: [{
                    selector: 'ngx-treeview-item',
                    template: "<div *ngIf=\"item\" class=\"treeview-item\">\r\n  <ng-template [ngTemplateOutlet]=\"template\"\r\n    [ngTemplateOutletContext]=\"{item: item, onCollapseExpand: onCollapseExpand, onCheckedChange: onCheckedChange}\">\r\n  </ng-template>\r\n  <div *ngIf=\"!item.collapsed\">\r\n    <ngx-treeview-item [config]=\"config\" *ngFor=\"let child of item.children\" [item]=\"child\" [template]=\"template\"\r\n      (checkedChange)=\"onChildCheckedChange(child, $event)\">\r\n    </ngx-treeview-item>\r\n  </div>\r\n</div>\r\n",
                    styles: [":host{display:block}:host .treeview-item{white-space:nowrap}:host .treeview-item .treeview-item{margin-left:2rem}"]
                },] }
    ];
    TreeviewItemComponent.ctorParameters = function () { return [
        { type: TreeviewConfig }
    ]; };
    TreeviewItemComponent.propDecorators = {
        config: [{ type: core.Input }],
        template: [{ type: core.Input }],
        item: [{ type: core.Input }],
        checkedChange: [{ type: core.Output }]
    };

    var TreeviewPipe = /** @class */ (function () {
        function TreeviewPipe() {
        }
        TreeviewPipe.prototype.transform = function (objects, textField) {
            if (lodash.isNil(objects)) {
                return undefined;
            }
            return objects.map(function (object) { return new TreeviewItem({ text: object[textField], value: object }); });
        };
        return TreeviewPipe;
    }());
    TreeviewPipe.decorators = [
        { type: core.Pipe, args: [{
                    name: 'ngxTreeview'
                },] }
    ];

    var TreeviewModule = /** @class */ (function () {
        function TreeviewModule() {
        }
        TreeviewModule.forRoot = function () {
            return {
                ngModule: TreeviewModule,
                providers: [
                    TreeviewConfig,
                    { provide: TreeviewI18n, useClass: DefaultTreeviewI18n },
                    { provide: TreeviewEventParser, useClass: DefaultTreeviewEventParser }
                ]
            };
        };
        return TreeviewModule;
    }());
    TreeviewModule.decorators = [
        { type: core.NgModule, args: [{
                    imports: [
                        forms.FormsModule,
                        common.CommonModule
                    ],
                    declarations: [
                        TreeviewComponent,
                        TreeviewItemComponent,
                        TreeviewPipe,
                        DropdownDirective,
                        DropdownMenuDirective,
                        DropdownToggleDirective,
                        DropdownTreeviewComponent
                    ], exports: [
                        TreeviewComponent,
                        TreeviewPipe,
                        DropdownTreeviewComponent
                    ]
                },] }
    ];

    /*
     * Public API Surface of ngx-treeview
     */

    /**
     * Generated bundle index. Do not edit.
     */

    exports.DefaultTreeviewEventParser = DefaultTreeviewEventParser;
    exports.DefaultTreeviewI18n = DefaultTreeviewI18n;
    exports.DownlineTreeviewEventParser = DownlineTreeviewEventParser;
    exports.DropdownDirective = DropdownDirective;
    exports.DropdownToggleDirective = DropdownToggleDirective;
    exports.DropdownTreeviewComponent = DropdownTreeviewComponent;
    exports.OrderDownlineTreeviewEventParser = OrderDownlineTreeviewEventParser;
    exports.TreeviewComponent = TreeviewComponent;
    exports.TreeviewConfig = TreeviewConfig;
    exports.TreeviewEventParser = TreeviewEventParser;
    exports.TreeviewHelper = TreeviewHelper;
    exports.TreeviewI18n = TreeviewI18n;
    exports.TreeviewItem = TreeviewItem;
    exports.TreeviewModule = TreeviewModule;
    exports.TreeviewPipe = TreeviewPipe;
    exports.ɵa = TreeviewItemComponent;
    exports.ɵb = DropdownMenuDirective;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=ngx-treeview.umd.js.map
