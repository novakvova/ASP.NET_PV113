import {useEffect, useState} from "react";

import { PlusOutlined } from '@ant-design/icons';
import {Button, Divider, Form, Input, message, Modal, Row, Upload} from 'antd';
import type { RcFile, UploadProps } from 'antd/es/upload';
import type { UploadFile } from 'antd/es/upload/interface';
import TextArea from "antd/es/input/TextArea";
import {ICategoryCreate, ICategoryItem} from "../../../interfaces/categories";
import http_common from "../../../http_common.ts";
import {Link, useParams} from "react-router-dom";


const CategoryEditPage: React.FC = () => {
    const {id} = useParams();
    const [previewOpen, setPreviewOpen] = useState<boolean>(false);
    const [previewImage, setPreviewImage] = useState('');
    const [previewTitle, setPreviewTitle] = useState('');
    const [file, setFile] = useState<UploadFile | null>();
    const [form] = Form.useForm<ICategoryCreate>();

    const [messageApi, contextHolder] = message.useMessage();
    const handleCancel = () => setPreviewOpen(false);
    const handlePreview = async (file: UploadFile) => {
        if (!file.url && !file.preview) {
            file.preview = URL.createObjectURL(file.originFileObj as RcFile);
        }

        setPreviewImage(file.url || (file.preview as string));
        setPreviewOpen(true);
        setPreviewTitle(file.name || file.url!.substring(file.url!.lastIndexOf('/') + 1));
    };

    const handleChange: UploadProps['onChange'] = ({fileList: newFile}) => {
        const newFileList = newFile.slice(-1);
        setFile(newFileList[0]);
    };
    const onReset = () => {
        onClear();
    };
    const onFinish = async (values: any) => {

        console.log("send data", values);
        if (values.image) {
            values.image = values.image.file
        }
        try {
            const result =
                await http_common.post<ICategoryItem>("/api/categories", values,
                    {
                        headers: {
                            "Content-Type":"multipart/form-data"
                        }
                    });
            console.log("Create new category", result);
            success();
            onClear();
        }
        catch {
            error();
        }
    };
    const onClear = ()=>{
        form.resetFields();
        setFile(null)
    }

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
                const { data } = resp;
                console.log("Good request", data);
                form.setFieldsValue(data);
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
                style={{minWidth: '100%', display: 'flex', flexDirection: "column", justifyContent: "center", padding:20}}
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

                <Form.Item
                    label="Фото"
                    name={"image"}
                >
                    <Upload
                        beforeUpload={() => false}
                        maxCount={1}
                        listType="picture-card"
                        onChange={handleChange}
                        onPreview={handlePreview}
                        fileList={file ? [file] : []}
                        accept="image/*"
                    >
                        {file ? null :
                            (
                                <div>
                                    <PlusOutlined/>
                                    <div style={{marginTop: 8}}>Upload</div>
                                </div>)
                        }
                    </Upload>
                </Form.Item>


                <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={handleCancel}>
                    <img alt="example" style={{width: '100%'}} src={previewImage}/>
                </Modal>

                <Row style={{display: 'flex', justifyContent: 'center'}}>
                    <Button style={{margin:10}} type="primary" htmlType="submit">
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

export default  CategoryEditPage;