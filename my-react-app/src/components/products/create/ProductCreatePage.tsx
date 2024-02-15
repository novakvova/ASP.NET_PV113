import {Form} from "antd";
import {IProductCreate} from "./types.ts";
import {useEffect, useState} from "react";
import {Status} from "../../../enums";
import {ICategoryName} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";

const ProductCreatePage = () => {
    const [form] = Form.useForm<IProductCreate>();
    const [status, setStatus] = useState<Status>(Status.IDLE);
    const [categories, setCategories] = useState<ICategoryName[]>([]);

    useEffect(() => {
        http_common.get<ICategoryName[]>("/api/categories/names")
            .then(resp=> {
                //console.log("list categories", resp.data);
                setCategories(resp.data);
            });
    },[]);

    return (
        <>
            <h1>Додати продукт</h1>

        </>
    )
}
export default ProductCreatePage;