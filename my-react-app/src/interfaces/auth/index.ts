import {IUploadedFile} from "../forms";

export interface IRegisterForm {
    firstName: string;
    lastName: string;
    image: IUploadedFile | null;
    email: string;
    password: string;
}

export interface IRegister {
    firstName: string;
    lastName: string;
    imageBase64: string | undefined;
    email: string;
    password: string;
}

export interface ILoginResult {
    token: string
}

export interface IUserLoginInfo {
    name: string,
    email: string
}

export interface ILogin {
    password: string,
    email: string
}