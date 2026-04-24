export interface OrderItem {
    productId: string;
    quantity: number;
    price: number;
}

export interface Order {
    id?: string;
    userId: string;
    items: OrderItem[];
    totalPrice: number;
    status: string;
    createdAt: Date;
}