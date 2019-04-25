"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var HabrService = /** @class */ (function () {
    function HabrService(http) {
        var _this = this;
        http.get('https://localhost:44398/api/Default/GetTitleArticles').subscribe(function (result) {
            _this.news = result;
        }, function (error) { return console.error(error); });
    }
    HabrService.prototype.getData = function () {
        return this.news;
    };
    return HabrService;
}());
exports.HabrService = HabrService;
//# sourceMappingURL=habr.service.js.map