import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Product } from "../shared/product";

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: ["productList.component.css"]
})
export class ProductList implements OnInit {
    public products: Product[];

    public displayProducts: Product[];

    filterBy: string ;

    constructor(private data: DataService) {
        //this.products = data.products;
        //this.displayProducts = this.products.slice(0);

       
    }

    ngOnInit() {
        this.data.loadProducts()
            .subscribe(() => {
                this.products = this.data.products;
                this.displayProducts = this.products.slice(0);
            });
    }

    addProduct(product: Product) {
        this.data.AddToOrder(product);
    }

   

    _listFilter: string;
    get listFilter(): string {
        return this._listFilter;
    }
    set listFilter(value: string) {
        this._listFilter = value;
        this.displayProducts = this.listFilter ? this.performFilter(this.listFilter) : this.products;
    }
    performFilter(filterBy: string): Product[] {
        filterBy = filterBy.toLocaleLowerCase();
        return this.products.filter((product: Product) =>
            product.title.toLocaleLowerCase().indexOf(filterBy) !== -1);
    }
}