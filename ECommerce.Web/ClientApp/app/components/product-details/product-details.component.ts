import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from "../shared/product";
import { DataService } from "../shared/dataService";

@Component({
    selector: 'product-details',
    templateUrl: './product-details.component.html',
    styleUrls: ['./product-details.component.css']
})
/** productDetails component*/
export class ProductDetailsComponent implements OnInit{
    ngOnInit(): void {
        let id = this._route.snapshot.paramMap.get('id');
        if (id === null) {
            return;
        }
        const intId = parseInt(id);

        this.data.getProduct(intId).subscribe((response: Product) => {
            this.product = response;
        });

        //let id: number = parseInt( this._route.params['id']);
    }

    public product: Product;

    /** productDetails ctor */
    constructor(private _route: ActivatedRoute,
        private _router: Router, private data: DataService) {

    }



}