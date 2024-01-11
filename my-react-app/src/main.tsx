import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import {BrowserRouter} from "react-router-dom";
import {Provider} from "react-redux";
import {store} from "./store";
import {IUserLoginInfo} from "./interfaces/auth";
import {jwtDecode} from "jwt-decode";
import {AuthReducerActionType} from "./components/auth/AuthReducer.ts";

if(localStorage.token) {
    const user: IUserLoginInfo = jwtDecode<IUserLoginInfo>(localStorage.token);
    store.dispatch({
            type: AuthReducerActionType.LOGIN_USER,
            payload: {
                name: user.name,
                email: user.email
            } as IUserLoginInfo
        }
    );
}

ReactDOM.createRoot(document.getElementById('root')!).render(
    <Provider store={store}>
        <BrowserRouter>
            <App/>
        </BrowserRouter>
    </Provider>,
)
