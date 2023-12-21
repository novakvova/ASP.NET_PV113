import {useState} from "react";

import { PlusOutlined } from '@ant-design/icons';
import {Button, Form, Input, message, Modal, Row, Upload} from 'antd';
import type { RcFile, UploadProps } from 'antd/es/upload';
import type { UploadFile } from 'antd/es/upload/interface';
import http_common from "../../../http_common.ts";
import {IRegisterForm, IRegister, ILoginResult, IUserLoginInfo} from "../../../interfaces/auth";
import {jwtDecode} from "jwt-decode";
import {imageConverter} from "../../../interfaces/forms";
import {useNavigate} from "react-router-dom";


const RegisterPage = () => {
    const navigator = useNavigate();
    const [previewOpen, setPreviewOpen] = useState<boolean>(false);
    const [previewImage, setPreviewImage] = useState('');
    const [previewTitle, setPreviewTitle] = useState('');
    const [file, setFile] = useState<UploadFile | null>();
    const [form] = Form.useForm<IRegisterForm>();

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
    const onFinish = async (values: IRegisterForm) => {
        const data : IRegister = {...values, imageBase64: values.image?.thumbUrl}
        console.log("Data send server", data);

        try {
            const result =
                await http_common.post<ILoginResult>("/api/account/register", data);

            const {token} = result.data;
            const user: IUserLoginInfo = jwtDecode<IUserLoginInfo>(token);
            console.log("User info", user);
            localStorage.token=token;
            success();
            onClear();
            navigator("/");
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

    return (
        <Row gutter={16}>
            {contextHolder}
            <h1>Реєстрація на сайті</h1>
            <Form
                form={form}
                onFinish={onFinish}
                layout="vertical"
                style={{minWidth: '100%', display: 'flex', flexDirection: "column", justifyContent: "center", padding:20}}
            >
                <Form.Item
                    label="Ім'я"
                    name="firstName"
                    htmlFor="firstName"
                    rules={[
                        {required: true, message: 'Це поле є обов\'язковим!'},
                        {min: 2, message: 'Ім\'я повинна містити мінімум 2 символи!'}
                    ]}
                >
                    <Input autoComplete="firstName"/>
                </Form.Item>

                <Form.Item
                    label="Прізвище"
                    name="lastName"
                    htmlFor="lastName"
                    rules={[
                        {required: true, message: 'Це поле є обов\'язковим!'},
                        {min: 2, message: 'Прізвище повинна містити мінімум 2 символи!'}
                    ]}
                >
                    <Input autoComplete="lastName"/>
                </Form.Item>

                <Form.Item
                    label="Електронна пошта"
                    name="email"
                    htmlFor="email"
                    rules={[
                        {
                            type: 'email',
                            message: 'Формати пошти не правильний!',
                        },
                        {required: true, message: 'Це поле є обов\'язковим!'},
                        {min: 2, message: 'Пошта повинна містити мінімум 2 символи!'}
                    ]}
                >
                    <Input autoComplete="email" />
                </Form.Item>

                <Form.Item
                    label="Фото"
                    name={"image"}
                    getValueFromEvent={imageConverter}
                >
                    <Upload
                        beforeUpload={() => false}
                        maxCount={1}
                        listType="picture-card"
                        onChange={handleChange}
                        onPreview={handlePreview}
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

                <Form.Item
                    name="password"
                    label="Пароль"
                    rules={[
                        { required: true, message: 'Вкажіть Ваш пароль!', },
                        { min: 6, message: 'Пароль має мати мінімум 6 символів!', },
                    ]}
                    hasFeedback
                >
                    <Input.Password/>
                </Form.Item>

                <Form.Item
                    name="confirm"
                    label="Повторіть Пароль"
                    dependencies={['password']}
                    hasFeedback
                    rules={[
                        {
                            required: true,
                            message: 'Будь-ласка підтвердіть пароль!',
                        },
                        ({getFieldValue}) => ({
                            validator(_, value) {
                                if (!value || getFieldValue('password') === value) {
                                    return Promise.resolve();
                                }
                                return Promise.reject(new Error('Пароль не співпадають!'));
                            },
                        }),
                    ]}
                >
                    <Input.Password/>
                </Form.Item>


                <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={handleCancel}>
                    <img alt="example" style={{width: '100%'}} src={previewImage}/>
                </Modal>

                <Row style={{display: 'flex', justifyContent: 'center'}}>
                    <Button style={{margin:10}} type="primary" htmlType="submit">
                        Реєструватися
                    </Button>
                    <Button  style={{margin:10}}  htmlType="button" onClick={onReset}>
                        Reset
                    </Button>
                </Row>
            </Form>
        </Row>
    )
}

export default  RegisterPage;