import * as tslib_1 from "tslib";
import { HttpClient } from '@angular/common/http';
// pentru DataService este configurat in app.modules.ts ca fiind parte in providers,
// asta inseamna ca va putea fi injectat.Dar si HttpClient este injectat in constructorul lui DataService,
// din acest motiv se decoreaza clasa DataService astfel:
import { Injectable } from "@angular/core";
// subscribe() este metoda catre care se face call back imediat ce raspunsul a sosit 
// din partea web api-ul-ui. Dar daca se doreste maparea datelor inainte de a rula ceva in call back 
// se impoerta {map}. Tot ceea ce se regaseste in iteriorul pipe()
// reprezinta o lista de interceptori
import { map } from "rxjs/operators";
var DataService = /** @class */ (function () {
    function DataService(http) {
        this.http = http;
        this.products = [];
    }
    DataService.prototype.loadProducts = function () {
        var _this = this;
        return this.http.get("/api/products")
            .pipe(map(function (data) {
            _this.products = data;
            return true;
        }));
    };
    DataService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpClient])
    ], DataService);
    return DataService;
}());
export { DataService };
//# sourceMappingURL=dataService.js.map