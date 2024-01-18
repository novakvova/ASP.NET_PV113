//Дані які буде зберігати наш редусер і може їх мінять
import {IUserLoginInfo} from "../../interfaces/auth";

export enum AuthReducerActionType {
    LOGIN_USER = "AUTH_LOGIN_USER"
}

export interface IAuthReducerState {
    isAuth: boolean, //чи коритувач є авторихованим
    user: IUserLoginInfo|null //інформація про авторизованого користувача
}

const initState: IAuthReducerState = {
    isAuth: false,
    user: null
}

//редюсер збергіє інформацію і він може модифікувати інформацію згідно різних подій
//В редюсері можу відбуватися різні події. Згідно події він подифікує свої дані
const AuthReducer = (state= initState, action: any) : IAuthReducerState => {
    switch(action.type) {
        case AuthReducerActionType.LOGIN_USER: {
            //console.log("payload", action.payload);
            //console.log("Потрібно повернути новий стан, щоб змінилися дані на сторінці")
            return {
                isAuth: true,
                user: action.payload
            };
        }
        default:
            return state;
    }
}

export default AuthReducer;