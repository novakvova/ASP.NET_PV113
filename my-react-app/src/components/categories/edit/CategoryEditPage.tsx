import {useEffect, useState} from "react";

import {UploadOutlined} from "@ant-design/icons";
import {Button, Divider, Form, Image, Input, message, Row, Upload} from 'antd';
import type {RcFile, UploadProps} from 'antd/es/upload';
import type {UploadFile} from 'antd/es/upload/interface';
import TextArea from "antd/es/input/TextArea";
import {ICategoryCreate, ICategoryItem} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";
import {Link, useNavigate, useParams} from "react-router-dom";
import {APP_ENV} from "../../../env";


const CategoryEditPage: React.FC = () => {
    const {id} = useParams();
    const [previewImage, setPreviewImage] = useState('');
    const [file, setFile] = useState<UploadFile | null>();
    const [form] = Form.useForm<ICategoryCreate>();
    const navigate = useNavigate();

    const [messageApi, contextHolder] = message.useMessage();

    const handlePreview = async (file: UploadFile) => {
        if (!file.url && !file.preview) {
            file.preview = URL.createObjectURL(file.originFileObj as RcFile);
        }
        setPreviewImage(file.url || (file.preview as string));
    };

    const handleChange: UploadProps['onChange'] = ({fileList: newFile}) => {
        const newFileList = newFile.slice(-1);
        setFile(newFileList[0]);
        handlePreview(newFileList[0]);
    };

    const onFinish = async (values: any) => {
        //console.log("send data", values);
        if (file) {
            values.image = file.originFileObj;
        }
        values.id = id;
        try {
            await http_common.put<ICategoryItem>(`/api/categories`, values,
                {
                    headers: {
                        "Content-Type": "multipart/form-data"
                    }
                });
            success();
            onClear();
            navigate("/");
        } catch {
            error();
        }
    };

    const onClear = () => {
        form.resetFields();
        setFile(null)
    };

    const success = () => {
        messageApi.open({
            type: 'success',
            duration: 10,
            content: 'Категорію успішно створено!',
        });
    };

    const error = () => {
        messageApi.open({
            type: 'error',
            duration: 10,
            content: 'Помилка створення категорії!',
        });
    };

    useEffect(() => {
        http_common.get<ICategoryItem>(`/api/categories/${id}`)
            .then(resp => {
                const {data} = resp;
                // console.log("Good request", data);
                form.setFieldsValue(data);
                setPreviewImage(`${APP_ENV.BASE_URL}/images/${data.image}`);
            })
            .catch(error => {
                console.log("Error server ", error);
            });
    }, [id]);

    return (
        <Row gutter={16}>
            {contextHolder}
            <Divider orientation="left">Змінити категорію</Divider>
            <Form
                form={form}
                onFinish={onFinish}
                layout="vertical"
                style={{
                    minWidth: '100%',
                    display: 'flex',
                    flexDirection: "column",
                    justifyContent: "center",
                    padding: 20
                }}
            >
                <Form.Item
                    label="Назва"
                    name="name"
                    htmlFor="name"
                    rules={[
                        {required: true, message: 'Це поле є обов\'язковим!'},
                        {min: 3, message: 'Назва повинна містити мінімум 3 символи!'}
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
                        {min: 10, message: 'Опис повинен містити мінімум 10 символів!'}
                    ]}
                >
                    <TextArea/>
                </Form.Item>

                <Form.Item label="Фото" htmlFor="image">
                    <Row style={{display: 'flex', alignItems: 'center', flexWrap: 'nowrap'}}>
                        <Image height={100}
                               src={previewImage || 'https://lightwidget.com/wp-content/uploads/localhost-file-not-found.jpg'}
                               style={{borderRadius: 10}}/>

                        <Row style={{marginLeft: 10}}>
                            <Upload
                                id="image"
                                name="image"
                                beforeUpload={() => false}
                                listType="picture"
                                maxCount={1}
                                onChange={handleChange}
                                fileList={file ? [file] : []}
                                accept="image/*"
                            >
                                <Button icon={<UploadOutlined/>}>Обрати нове фото</Button>
                            </Upload>
                        </Row>
                    </Row>
                </Form.Item>


                <Row style={{display: 'flex', justifyContent: 'center'}}>
                    <Button style={{margin: 10}} type="primary" htmlType="submit">
                        Зберегти
                    </Button>
                    <Link to={"/"}>
                        <Button style={{margin: 10}} htmlType="button">
                            Скасувать
                        </Button>
                    </Link>
                </Row>
            </Form>
        </Row>
    )
}

export default CategoryEditPage;