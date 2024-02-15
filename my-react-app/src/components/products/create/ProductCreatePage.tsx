import {Button, Divider, Form, Input, InputNumber, message, Row, Select, Spin, Upload} from "antd";
import {IProductCreate} from "./types.ts";
import {useEffect, useState} from "react";
import {Status} from "../../../enums";
import {ICategoryName} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";
import TextArea from "antd/es/input/TextArea";
import {imageConverterToFileArray} from "../../../helpers/converter.ts";
import {DownloadOutlined } from '@ant-design/icons';

const ProductCreatePage = () => {
    const [form] = Form.useForm<IProductCreate>();
    const [status, setStatus] = useState<Status>(Status.IDLE);
    const [categories, setCategories] = useState<ICategoryName[]>([]);

    const [messageApi, contextHolder] = message.useMessage();

    useEffect(() => {
        http_common.get<ICategoryName[]>("/api/categories/names")
            .then(resp=> {
                //console.log("list categories", resp.data);
                setCategories(resp.data);
            });
    },[]);

    const onFinish = async (values: IProductCreate) => {
        try {
            console.log("Submit form", values);
            //const response = await dispatch(addProduct(values));
            //unwrapResult(response);
            //navigate(`/categories/products-category/${values.categoryId}`);
        } catch (error) {
            //handleError(error);
        }
    };

    const onChangePrice = (value: string | null) => {
        form.setFieldValue('price', value?.replace('.', ','));
    };

    const categoriesData = categories?.map(item => ({label: item.name, value: item.id}));

    return (
        <>
            <h1>Додати продукт</h1>
            <Spin spinning={status === Status.LOADING}>
                <Row gutter={16}>
                    {contextHolder}
                    <Form
                        form={form}
                        onFinish={onFinish}
                        layout="vertical"
                        style={{
                            minWidth: '100%',
                            display: 'flex',
                            flexDirection: 'column',
                            justifyContent: 'center',
                            padding: 20,
                        }}
                        initialValues={{
                            ['price']: 100,
                            ['quantity']: 100,
                            ['discount']: 0,
                        }}
                    >
                        <Form.Item
                            label="Назва"
                            name="name"
                            htmlFor="name"
                            rules={[
                                {required: true, message: 'Це поле є обов\'язковим!'},
                                {min: 3, message: 'Назва повинна містити мінімум 3 символи!'},
                            ]}
                        >
                            <Input autoComplete="name"/>
                        </Form.Item>

                        <Form.Item
                            label="Опис"
                            name="description"
                            htmlFor="description"
                            rules={[
                                {required: true, message: 'Це поле є обов\'язковим!'},
                                {min: 10, message: 'Опис повинен містити мінімум 10 символів!'},
                            ]}
                        >
                            <TextArea/>
                        </Form.Item>




                        <Form.Item
                            label="Ціна"
                            name="price"
                            htmlFor="price"
                            rules={[
                                {required: true, message: 'Це поле є обов\'язковим!'},
                            ]}
                        >
                            <InputNumber<string>
                                stringMode={true}
                                onChange={onChangePrice}
                                decimalSeparator=","
                                step={0.01}
                                addonAfter="ШТ"
                                min="0"
                            />
                        </Form.Item>

                        <Form.Item
                            label="К-сть доступних од. товару"
                            name="quantity"
                            htmlFor="quantity"
                            rules={[
                                {required: true, message: 'Це поле є обов\'язковим!'},
                            ]}
                        >
                            <InputNumber addonAfter="ШТ" min={0}/>
                        </Form.Item>


                        <Form.Item
                            label="Категорія"
                            name="categoryId"
                            htmlFor="categoryId"
                            rules={[
                                {required: true, message: 'Це поле є обов\'язковим!'},
                            ]}
                        >
                            <Select
                                placeholder="Оберіть категорію: "
                                options={categoriesData}
                            />
                        </Form.Item>

                        <Form.Item
                            label="Фото"
                            name="images"
                            htmlFor="images"
                            valuePropName="file"
                            getValueFromEvent={imageConverterToFileArray}
                            rules={[
                                {required: true, message: 'Це поле є обов\'язковим!'},
                            ]}
                        >
                            <Upload
                                listType="picture-card"
                                maxCount={10}
                                multiple
                                showUploadList={{showPreviewIcon: false}}
                                beforeUpload={() => false}
                                accept="image/*"
                            >
                                <Button type="primary" shape="round" icon={<DownloadOutlined/>}/>
                            </Upload>
                        </Form.Item>

                        <Row style={{display: 'flex', justifyContent: 'center'}}>
                            <Button style={{margin: 10}} type="primary" htmlType="submit">
                                Додати
                            </Button>
                            <Button style={{margin: 10}} htmlType="button">
                                Скинути
                            </Button>
                        </Row>
                    </Form>
                </Row>
            </Spin>
        </>
    )
}
export default ProductCreatePage;