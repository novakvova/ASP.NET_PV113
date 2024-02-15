// import {IUploadedFile} from "../forms";

export interface ICategoryName {
    id: number,
    name: string,
}

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

export interface ICategoryImage {
    file: File;
}

export interface ICategoryCreateForm {
    name: string;
    image: ICategoryImage | null;
    description: string;
}

