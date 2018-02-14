export class Product {
    id: number;
    title: string;
    description: string;
    price: number;
    categoryId: number;
    categoryName: string;

    reviewList: Array<ReView> = new Array<ReView>();
}

export class ReView {
    id: string;
    reviewContent: string;
    reviewerFullName: string;
    createDateTimeOffset:Date;
}