export interface IProductCreate {
    name: string,
    price: string,
    description: string,
    quantity: number,
    images: File[] | null,
    categoryId: number,
}