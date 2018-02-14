import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';


import { ProductList } from "./components/shop/productList.component";
import { ProductDetailsComponent } from "./components/product-details/product-details.component";

import { Cart } from "./components/shop/cart.component";
import { Shop } from "./components/shop/shop.component";
import { Checkout } from "./components/checkout/checkout.component";
import { Login } from "./components/login/login.component";

import { DataService } from "./components/shared/dataService"


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        ProductList,
        ProductDetailsComponent,
        Cart,
        Shop,
        Checkout,
        Login
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'Shop', pathMatch: 'full' },
            { path: 'products', component: ProductList },
            { path: 'products/:id', component: ProductDetailsComponent },
            { path: "Shop", component: Shop },
            { path: "checkout", component: Checkout },
            { path: "login", component: Login },
            { path: '**', redirectTo: 'Shop' }
        ])
    ],
    providers: [
        DataService
    ]
})
export class AppModuleShared {
}
