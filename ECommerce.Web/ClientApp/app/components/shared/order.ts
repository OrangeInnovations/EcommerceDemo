import * as _ from "lodash";

export class Order {
  
    orderItemList: Array<OrderItem> = new Array<OrderItem>();
    

  get subtotal(): number {
      return _.sum(_.map(this.orderItemList, i => i.unitPrice * i.quantity));
  };

}

export class OrderItem {
    productId: number;
    quantity: number;
    unitPrice: number;
    title: string;
    categoryName: string;
}