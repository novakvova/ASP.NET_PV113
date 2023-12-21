import {IUploadedFile} from "../forms";

export interface ICategoryItem {
    id: number;
    name: string;
    image: string;
    description: string;
}

export interface ICategoryCreate {
    name: string;
    image: File | undefined | null;
    description: string;
}

export interface ICategoryCreateForm {
    name: string;
    image: IUploadedFile | null;
    description: string;
}

