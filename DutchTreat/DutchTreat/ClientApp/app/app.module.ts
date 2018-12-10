
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ProductList } from "./shop/productlist.components";
import { DataService } from "./shared/dataService";
import { HttpClientModule } from "@angular/common/http";



@NgModule({
    declarations: [
        AppComponent,
        ProductList
    ],
    imports: [
        BrowserModule,
        HttpClientModule
    ],
    providers: [DataService],  /*in sectiunea providers se adauga ceea ce vrem sa injectam in componentele noastre*/
    bootstrap: [AppComponent]
})
export class AppModule { }
