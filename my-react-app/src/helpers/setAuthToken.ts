import http_common from "../http_common.ts";

const setAuthToken = (token: string) => {
    if(token) {
        http_common.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    }
    else {
        delete http_common.defaults.headers.common["Authorization"];
    }
}

export default setAuthToken;