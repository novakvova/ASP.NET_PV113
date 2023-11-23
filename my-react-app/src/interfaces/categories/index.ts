export interface ICategoryItem {
    id: number;
    name: string;
    image: string;
    description: string;
}

export interface ICategoryCreate {
    name: string;
    image: File | null;
    description: string;
}
