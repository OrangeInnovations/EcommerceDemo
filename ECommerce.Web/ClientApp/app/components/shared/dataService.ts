import { Injectable } from "@angular/core";
import { Http, Response, Headers } from "@angular/http";
import { Observable } from "rxjs";
import { Product } from "./product";
import 'rxjs/add/operator/map';
import { OrderItem } from "./order";
import { Order } from "./order";

@Injectable()
export class DataService {

    constructor(private http: Http) {

    }

    private token: string = "";
    private tokenExpiration: Date;

    public order: Order = new Order();

    public products: Product[] = [];

    public loadProducts(): Observable<Product[]> {
        return this.http.get("http://localhost:64560/api/products")
            .map((result: Response) => this.products = result.json());
    }

    public getProduct(id: number): Observable<Product> {
        return this.http.get("http://localhost:64560/api/products/" + id)
            .map((result: Response) => {
                console.log(result);
                return <Product> result.json();
            });
    }


    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

    public login(creds: any) {
        return this.http.post("http://localhost:64560/api/account", creds)
            .map(response => {
                let tokenInfo = response.json();
                this.token = tokenInfo.token;
                this.tokenExpiration = tokenInfo.expiration;
                return true;
            });
    }


    public checkout() {

        return this.http.post("http://localhost:64560/api/orders", this.order, {
            headers: new Headers({ "Authorization": "Bearer " + this.token })
        })
            .map(response => {
                this.order = new Order();
                return true;
            });
    }

    public AddToOrder(product: Product) {

        let item = this.order.orderItemList.find(i => i.productId == product.id);

        if (item) {

            item.quantity++;

        } else {

            item = new OrderItem();
            item.productId = product.id;
            item.unitPrice = product.price;
            item.quantity = 1;
            item.title = product.title;
            item.categoryName = product.categoryName;

            this.order.orderItemList.push(item);
        }
    }


}